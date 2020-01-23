using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Text;

using System.Management;
using DY.Config;
using DY.Common;//需要在项目中添加System.Management引用
namespace DY.Site
{
    public class SoftReg : System.Web.UI.Page
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SoftReg()
        {
            if (!CheckDomain())
            {
                string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>请向您的服务商获取授权信息</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                message += "您的网站或域名未授权，请咨询您的服务商索取授权！</div></div></body></html>";
                Context.Response.Write(message);
                Context.Response.End();
                return;
            }
        }

        /// <summary>
        /// 检测域名的授权合法性
        /// </summary>
        /// <returns></returns>
        protected bool CheckDomain()
        {
            bool isAssemblyInexistence=false;
            string[] assemblylist = BaseConfig.SoftReg.Split(',');
            string domain = new SiteUtils().GetDomain();
            foreach (string assembly in assemblylist)
            {
                if (AESEncrypt.Decode(assembly, BaseConfig.WebEncrypt) == domain || domain.Contains("localhost") || domain.Contains("127.0.0.1") || domain.Contains("eyouc.com"))
                {
                    isAssemblyInexistence = true;
                }
            }
            return isAssemblyInexistence;
        }






        /// <summary>
        /// 取得设备硬盘的卷标号
        /// </summary>
        /// <returns></returns>
        public string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }
        /// <summary>
        /// 获得CPU的序列号
        /// </summary>
        /// <returns></returns>
        public string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <returns></returns>
        public string getMNum()
        {
            string strNum = getCpu() + GetDiskVolumeSerialNumber();//获得24位Cpu和硬盘序列号
            string strMNum = strNum.Substring(0, 24);//从生成的字符串中取出前24个字符做为机器码
            return strMNum;
        }
        public int[] intCode = new int[127];//存储密钥
        public int[] intNumber = new int[25];//存机器码的Ascii值
        public char[] Charcode = new char[25];//存储机器码字
        public void setIntCode()//给数组赋值小于10的数
        {
            for (int i = 1; i < intCode.Length; i++)
            {
                intCode[i] = i % 9;
            }
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <returns></returns>
        public string getRNum()
        {
            setIntCode();//初始化127位数组
            string MNum = this.getMNum();//获取注册码
            for (int i = 1; i < Charcode.Length; i++)//把机器码存入数组中
            {
                Charcode[i] = Convert.ToChar(MNum.Substring(i - 1, 1));
            }
            for (int j = 1; j < intNumber.Length; j++)//把字符的ASCII值存入一个整数组中。
            {
                intNumber[j] = intCode[Convert.ToInt32(Charcode[j])] + Convert.ToInt32(Charcode[j]);
            }
            string strAsciiName = "";//用于存储注册码
            for (int j = 1; j < intNumber.Length; j++)
            {
                if (intNumber[j] >= 48 && intNumber[j] <= 57)//判断字符ASCII值是否0－9之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 65 && intNumber[j] <= 90)//判断字符ASCII值是否A－Z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else if (intNumber[j] >= 97 && intNumber[j] <= 122)//判断字符ASCII值是否a－z之间
                {
                    strAsciiName += Convert.ToChar(intNumber[j]).ToString();
                }
                else//判断字符ASCII值不在以上范围内
                {
                    if (intNumber[j] > 122)//判断字符ASCII值是否大于z
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 10).ToString();
                    }
                    else
                    {
                        strAsciiName += Convert.ToChar(intNumber[j] - 9).ToString();
                    }
                }
            }
            return strAsciiName;//返回注册码
        }
    }
}