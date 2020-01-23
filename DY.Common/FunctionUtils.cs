using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.IO;
using Microsoft.VisualBasic;
using System.Security.Cryptography;

namespace DY.Common
{
    /// <summary> 
    /// ����
    /// </summary> 
    public class AESEncrypt
    {

        private static byte[] Keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };

        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = FunctionUtils.Text.GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            rijndaelProvider.IV = Keys;
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Convert.ToBase64String(encryptedData);
        }


        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = FunctionUtils.Text.GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');

                RijndaelManaged rijndaelProvider = new RijndaelManaged();
                rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
                rijndaelProvider.IV = Keys;
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return "";
            }

        }

    }

    /// <summary> 
    /// ����
    /// </summary> 
    public class DESEncrypt
    {
        //Ĭ����Կ����
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };


        /// <summary>
        /// DES�����ַ���
        /// </summary>
        /// <param name="encryptString">�����ܵ��ַ���</param>
        /// <param name="encryptKey">������Կ,Ҫ��Ϊ8λ</param>
        /// <returns>���ܳɹ����ؼ��ܺ���ַ���,ʧ�ܷ���Դ��</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = FunctionUtils.Text.GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());

        }

        /// <summary>
        /// DES�����ַ���
        /// </summary>
        /// <param name="decryptString">�����ܵ��ַ���</param>
        /// <param name="decryptKey">������Կ,Ҫ��Ϊ8λ,�ͼ�����Կ��ͬ</param>
        /// <returns>���ܳɹ����ؽ��ܺ���ַ���,ʧ�ܷ�Դ��</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = FunctionUtils.Text.GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Function �����ܺ����࣬�ַ����������
    /// </summary>
    public abstract class FunctionUtils
    {
        #region ������ʽ��ʹ��

        /// <summary>
        /// �ж�������ַ����Ƿ���ȫƥ������
        /// </summary>
        /// <param name="RegexExpression">������ʽ</param>
        /// <param name="str">���жϵ��ַ���</param>
        /// <returns></returns>
        public static bool IsValiable(string RegexExpression, string str)
        {
            bool blResult = false;

            Regex rep = new Regex(RegexExpression, RegexOptions.IgnoreCase);

            //blResult = rep.IsMatch(str);
            Match mc = rep.Match(str);

            if (mc.Success)
            {
                if (mc.Value == str) blResult = true;
            }


            return blResult;
        }

        /// <summary>
        /// ת�������е�URL·��Ϊ����URL·��
        /// </summary>
        /// <param name="sourceString">Դ����</param>
        /// <param name="replaceURL">�滻Ҫ��ӵ�URL</param>
        /// <returns>string</returns>
        public static string ConvertURL(string sourceString, string replaceURL)
        {
            Regex rep = new Regex(" (src|href|background|value)=('|\"|)([^('|\"|)http://].*?)('|\"| |>)");
            sourceString = rep.Replace(sourceString, " $1=$2" + replaceURL + "$3$4");
            return sourceString;
        }

        /// <summary>
        /// ��ȡ����������ͼƬ����HTTP��ͷ��URL��ַ
        /// </summary>
        /// <param name="sourceString">��������</param>
        /// <returns>ArrayList</returns>
        public static ArrayList GetImgFileUrl(string sourceString)
        {
            ArrayList imgArray = new ArrayList();

            Regex r = new Regex("<IMG(.*?)src=('|\"|)(http://.*?)('|\"| |>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!imgArray.Contains(mc[i].Result("$3")))
                {
                    imgArray.Add(mc[i].Result("$3"));
                }
            }

            return imgArray;
        }

        /// <summary>
        /// ��ȡ�����������ļ�����HTTP��ͷ��URL��ַ
        /// </summary>
        /// <param name="sourceString">��������</param>
        /// <returns>ArrayList</returns>
        public static Hashtable GetFileUrlPath(string sourceString)
        {
            Hashtable url = new Hashtable();

            Regex r = new Regex(" (src|href|background|value)=('|\"|)(http://.*?)('|\"| |>)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            MatchCollection mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
            {
                if (!url.ContainsValue(mc[i].Result("$3")))
                {
                    url.Add(i, mc[i].Result("$3"));
                }
            }

            return url;
        }

        /// <summary>
        /// ��ȡһ��SQL����е�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public static ArrayList SqlParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@"@(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// ��ȡһ��SQL����е�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public static ArrayList OracleParame(string sql)
        {
            ArrayList list = new ArrayList();
            Regex r = new Regex(@":(?<x>[0-9a-zA-Z]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sql);
            for (int i = 0; i < mc.Count; i++)
            {
                list.Add(mc[i].Result("$1"));
            }

            return list;
        }

        /// <summary>
        /// ��HTML����ת���ɴ��ı�
        /// </summary>
        /// <param name="sourceHTML">HTML����</param>
        /// <returns></returns>
        public static string ConvertText(string sourceHTML)
        {
            string strResult = "";
            Regex r = new Regex("<(.*?)>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection mc = r.Matches(sourceHTML);

            if (mc.Count == 0)
            {
                strResult = sourceHTML;
            }
            else
            {
                strResult = sourceHTML;

                for (int i = 0; i < mc.Count; i++)
                {
                    strResult = strResult.Replace(mc[i].ToString(), "");
                }
            }

            return strResult;
        }
        #endregion

        #region �Զ��崦��

        /// <summary>
        /// ��ȡ web.config �ļ���ָ�� key ��ֵ
        /// </summary>
        /// <param name="keyName">key����</param>
        /// <returns></returns>
        public static string GetAppSettings(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }


        /// <summary>
        /// ����ָ����ʽ���ʱ��
        /// </summary>
        /// <param name="NowDate">ʱ��</param>
        /// <param name="type">�������</param>
        /// <returns></returns>
        public static string WriteDate(string NowDate, int type)
        {
            double TimeZone = 0;
            DateTime NewDate = DateTime.Parse(NowDate).AddHours(TimeZone);
            string strResult = "";

            switch (type)
            {
                case 1:
                    strResult = NewDate.ToString();
                    break;
                case 2:
                    strResult = NewDate.ToShortDateString().ToString();
                    break;
                case 3:
                    strResult = NewDate.Year + "��" + NewDate.Month + "��" + NewDate.Day + "�� " + NewDate.Hour + "��" + NewDate.Minute + "��" + NewDate.Second + "��";
                    break;
                case 4:
                    strResult = NewDate.Year + "��" + NewDate.Month + "��" + NewDate.Day + "��";
                    break;
                case 5:
                    strResult = NewDate.Year + "��" + NewDate.Month + "��" + NewDate.Day + "�� " + NewDate.Hour + "��" + NewDate.Minute + "��";
                    break;
                case 6:
                    strResult = NewDate.Year + "-" + NewDate.Month + "-" + NewDate.Day + "  " + NewDate.Hour + ":" + NewDate.Minute;
                    break;
                default:
                    strResult = NewDate.ToString();
                    break;
            }
            return strResult;
        }


        private static int Instr(string strA, string strB)
        {
            if (string.Compare(strA, strA.Replace(strB, "")) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// �жϿͻ��˲���ϵͳ�������������
        /// </summary>
        /// <param name="Info">�ͻ��˷��ص�ͷ��Ϣ(Request.UserAgent)</param>
        /// <param name="Type">��ȡ���ͣ�1Ϊ����ϵͳ�� 2Ϊ�����</param>
        /// <returns></returns>
        public static string GetInfo(string Info, int Type)
        {

            string GetInfo = "";
            switch (Type)
            {
                case 1:
                    if (Instr(Info, @"NT 5.1") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows XP";
                    }
                    else if (Instr(Info, @"Tel") > 0)
                    {
                        GetInfo = "����ϵͳ��Telport";
                    }
                    else if (Instr(Info, @"webzip") > 0)
                    {
                        GetInfo = "����ϵͳ������ϵͳ��webzip";
                    }
                    else if (Instr(Info, @"flashget") > 0)
                    {
                        GetInfo = "����ϵͳ��flashget";
                    }
                    else if (Instr(Info, @"offline") > 0)
                    {
                        GetInfo = "����ϵͳ��offline";
                    }
                    else if (Instr(Info, @"NT 5") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows 2000";
                    }
                    else if (Instr(Info, @"NT 4") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows NT4";
                    }
                    else if (Instr(Info, @"98") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows 98";
                    }
                    else if (Instr(Info, @"95") > 0)
                    {
                        GetInfo = "����ϵͳ��Windows 95";
                    }
                    else
                    {
                        GetInfo = "����ϵͳ��δ֪";
                    }
                    break;
                case 2:
                    if (Instr(Info, @"NetCaptor 6.5.0") > 0)
                    {
                        GetInfo = "� �� ����NetCaptor 6.5.0";
                    }
                    else if (Instr(Info, @"MyIe 3.1") > 0)
                    {
                        GetInfo = "� �� ����MyIe 3.1";
                    }
                    else if (Instr(Info, @"NetCaptor 6.5.0RC1") > 0)
                    {
                        GetInfo = "� �� ����NetCaptor 6.5.0RC1";
                    }
                    else if (Instr(Info, @"NetCaptor 6.5.PB1") > 0)
                    {
                        GetInfo = "� �� ����NetCaptor 6.5.PB1";
                    }
                    else if (Instr(Info, @"MSIE 6.0b") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 6.0b";
                    }
                    else if (Instr(Info, @"MSIE 6.0") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 6.0";
                    }
                    else if (Instr(Info, @"MSIE 5.5") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 5.5";
                    }
                    else if (Instr(Info, @"MSIE 5.01") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 5.01";
                    }
                    else if (Instr(Info, @"MSIE 5.0") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 5.0";
                    }
                    else if (Instr(Info, @"MSIE 4.0") > 0)
                    {
                        GetInfo = "� �� ����Internet Explorer 4.0";
                    }
                    else
                    {
                        GetInfo = "� �� ����δ֪";
                    }
                    break;
            }
            return GetInfo;
        }


        /// <summary>
        /// ��ȡ������������MAC��ַ
        /// </summary>
        /// <returns></returns>
        public static string GetMAC_Address()
        {
            string strResult = "";

            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True") strResult = mo["MacAddress"].ToString();
            }

            return strResult;
        }



        /// <summary>
        /// ת���ļ�·���в������ַ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string</returns>
        public static string convertDirURL(string path)
        {
            return AddLast(path.Replace("/", "\\"), "\\");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string</returns>
        public static string convertXmlString(string str)
        {
            return "<![CDATA[" + str + "]]>";
        }

        /// <summary>
        /// ת��һ��double�����ִ�Ϊʱ�䣬��ʼ 0 Ϊ 1970-01-01 08:00:00
        /// ԭ����ǣ�ÿ��һ�����������ִ����ۼ�һ
        /// </summary>
        /// <param name="d">double ������</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = DateTime.MinValue;

            DateTime startTime = DateTime.Parse("1970-01-01 08:00:00");

            time = startTime.AddSeconds(d);

            return time;
        }

        /// <summary>
        /// ת��ʱ��Ϊһ��double�����ִ�����ʼ 0 Ϊ 1970-01-01 08:00:00
        /// ԭ����ǣ�ÿ��һ�����������ִ����ۼ�һ
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>double</returns>
        public static double ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;

            DateTime startTime = DateTime.Parse("1970-01-01 08:00:00");

            intResult = (time - startTime).TotalSeconds;

            return intResult;
        }


        /// <summary>
        /// ��ȡһ��URL�����õ��ļ����ƣ�������׺����
        /// </summary>
        /// <param name="url">URL��ַ</param>
        /// <returns>string</returns>
        public static string GetFileName(string url)
        {
            //string[] Name = FunctionUtils.SplitArray(url,'/');
            //return Name[Name.Length - 1];

            return System.IO.Path.GetFileName(url);
        }

        /// <summary>
        /// ���ĳһ�ַ����ĵ�һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£������ڸ��ַ���ǰ��������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string AddFirst(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.StartsWith(Str))
            {
                strResult = Strings;
            }
            else
            {
                strResult = String.Concat(Str, Strings);
            }
            return strResult;
        }


        /// <summary>
        /// ���ĳһ�ַ��������һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£������ڸ��ַ���ĩβ��������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string AddLast(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.EndsWith(Str))
            {
                strResult = Strings;
            }
            else
            {
                strResult = String.Concat(Strings, Str);
            }
            return strResult;
        }

        /// <summary>
        /// ���ĳһ�ַ����ĵ�һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£���ͬ��ȥ������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string DelFirst(string Strings, string Str)
        {
            string strResult = "";
            if (Strings.Length == 0) throw new Exception("ԭʼ�ַ�������Ϊ��");

            if (Strings.StartsWith(Str))
            {
                strResult = Strings.Substring(Str.Length, Strings.Length - 1);
            }
            else
            {
                strResult = Strings;
            }

            return strResult;
        }

        /// <summary>
        /// ���ĳһ�ַ��������һ���ַ��Ƿ���ָ����
        /// �ַ�һ�£���ͬ��ȥ������ַ�
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="Str">�ַ�</param>
        /// <returns>���� string</returns>
        public static string DelLast(string Strings, string Str)
        {
            string strResult = "";

            if (Strings.EndsWith(Str))
            {
                strResult = Strings.Substring(0, Strings.Length - Str.Length);
            }
            else
            {
                strResult = Strings;
            }

            return strResult;
        }

        /// <summary>
        /// ��ȡһ��Ŀ¼�ľ���·����������WEBӦ�ó���
        /// </summary>
        /// <param name="folderPath">Ŀ¼·��</param>
        /// <returns></returns>
        public static string GetRealPath(string folderPath)
        {
            string strResult = "";

            if (folderPath.IndexOf(":\\") > 0)
            {
                strResult = AddLast(folderPath, "\\");
            }
            else
            {
                if (folderPath.StartsWith("~/"))
                {
                    strResult = AddLast(System.Web.HttpContext.Current.Server.MapPath(folderPath), "\\");
                }
                else
                {
                    string webPath = System.Web.HttpContext.Current.Request.ApplicationPath + "/";
                    strResult = AddLast(System.Web.HttpContext.Current.Server.MapPath(webPath + folderPath), "\\");
                }
            }

            return strResult;
        }

        /// <summary>
        /// ��ȡһ���ļ��ľ���·����������WEBӦ�ó���
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <returns>string</returns>
        public static string GetRealFile(string filePath)
        {
            string strResult = "";

            //strResult = ((file.IndexOf(@":\") > 0 || file.IndexOf(":/") > 0) ? file : System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath + "/" + file));
            strResult = ((filePath.IndexOf(":\\") > 0) ?
                filePath :
                System.Web.HttpContext.Current.Server.MapPath(filePath));

            return strResult;
        }
        /// <summary>
        /// ���ַ������� HTML �������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static string strEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }


        /// <summary>
        /// �� HTML �ַ������н������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <returns></returns>
        public static string strDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// �Խű�������д���
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertScript(string str)
        {
            string strResult = "";
            if (str != "")
            {
                StringReader sr = new StringReader(str);
                string rl;
                do
                {
                    strResult += sr.ReadLine();
                } while ((rl = sr.ReadLine()) != null);
            }

            strResult = strResult.Replace("\"", "&quot;");

            return strResult;
        }


        /// <summary>
        /// ��һ���ַ�����ĳһ�ض��ַ��ָ���ַ�������
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="str">�ָ��ַ�</param>
        /// <returns>string[]</returns>
        public static string[] SplitArray(string Strings, char str)
        {
            string[] strArray = Strings.Trim().Split(new char[] { str });

            return strArray;
        }

        /// <summary>
        /// ��һ���ַ�����ĳһ�ַ��ָ������
        /// </summary>
        /// <param name="Strings">�ַ���</param>
        /// <param name="str">�ָ��ַ�</param>
        /// <returns>string[]</returns>
        public static string[] SplitArray(string Strings, string str)
        {
            Regex r = new Regex(str);
            string[] strArray = r.Split(Strings.Trim());

            return strArray;
        }

        /// <summary>
        /// ���һ���ַ������Ƿ������һ���Թ̶��ָ���ָ���ַ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="Strings">�̶��ָ���ָ���ַ���</param>
        /// <param name="Str">�ָ��</param>
        /// <returns></returns>
        public static bool InArray(string str, string Strings, char Str)
        {
            bool blResult = false;

            string[] array = SplitArray(Strings, Str);
            for (int i = 0; i < array.Length; i++)
            {
                if (str == array[i])
                {
                    blResult = true;
                    break;
                }
            }

            return blResult;
        }

        /*
                /// <summary>
                /// ���һ���ַ������Ƿ������һ���Թ̶��ָ���ָ���ַ�����
                /// </summary>
                /// <param name="str">�ַ���</param>
                /// <param name="Strings">�̶��ָ���ָ���ַ���</param>
                /// <param name="Str">�ָ��</param>
                /// <returns></returns>
                public static bool InArray(string str, string Strings, string Str)
                {
                    bool blResult = false;

                    string[] array = SplitArray(Strings, Str);
                    for(int i = 0; i < array.Length; i++)
                    {
                        if(str == array[i])
                        {
                            blResult = true;
                            break;
                        }
                    }

                    return blResult;
                }
                */

        /// <summary>
        /// ���һ���ַ������Ƿ������һ���Թ̶��ָ���ָ���ַ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="array">�ַ�������</param>
        /// <returns></returns>
        public static bool InArray(string str, string[] array)
        {
            bool blResult = false;

            for (int i = 0; i < array.Length; i++)
            {
                if (str == array[i])
                {
                    blResult = true;
                    break;
                }
            }

            return blResult;
        }


        /// <summary>
        /// ���ֵ�Ƿ���Ч��Ϊ null �� "" ��Ϊ��Ч
        /// </summary>
        /// <param name="obj">Ҫ����ֵ</param>
        /// <returns></returns>
        public static bool CheckValiable(object obj)
        {
            if (Object.Equals(obj, null) || Object.Equals(obj, string.Empty))
                return false;
            else
                return true;
        }
        #endregion

        #region ���ַ����ļ���/����

        /// <summary>
        /// ���ַ���������Ӧ ServU �� MD5 ����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ServUEncrypt(string str)
        {
            string strResult = "";
            strResult = Text.RandomSTR(2);
            str = strResult + str;
            str = NoneEncrypt(str, 1);
            str = strResult + str;

            return str;
        }

        /// <summary>
        /// ���ַ���������ͨmd5����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            str = NoneEncrypt(str, 1);
            return str;
        }

        /// <summary>
        /// ���ַ������м��ܣ������棩
        /// </summary>
        /// <param name="Password">Ҫ���ܵ��ַ���</param>
        /// <param name="Format">���ܷ�ʽ,0 is SHA1,1 is MD5</param>
        /// <returns></returns>
        public static string NoneEncrypt(string Password, int Format)
        {
            string strResult = "";
            switch (Format)
            {
                case 0:
                    strResult = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
                    break;
                case 1:
                    strResult = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
                    break;
                default:
                    strResult = Password;
                    break;
            }

            return strResult;
        }


        /// <summary>
        /// ���ַ������м���
        /// </summary>
        /// <param name="Passowrd">�����ܵ��ַ���</param>
        /// <returns>string</returns>
        public static string Encrypt(string Passowrd)
        {
            string strResult = "";

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(Passowrd, true, 2);
            strResult = FormsAuthentication.Encrypt(ticket).ToString();

            return strResult;
        }


        /// <summary>
        /// ���ַ������н���
        /// </summary>
        /// <param name="Passowrd">�Ѽ��ܵ��ַ���</param>
        /// <returns></returns>
        public static string Decrypt(string Passowrd)
        {
            string strResult = "";

            strResult = FormsAuthentication.Decrypt(Passowrd).Name.ToString();

            return strResult;
        }

        #endregion

        #region �ַ����Ĵ�������

        /// <summary>
        /// �ַ����Ĵ�������
        /// </summary>
        public abstract class Text
        {
            public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
            {
                string text = p_SrcString;
                if (p_Length < 0)
                {
                    return text;
                }
                byte[] sourceArray = Encoding.Default.GetBytes(p_SrcString);
                if (sourceArray.Length <= p_Length)
                {
                    return text;
                }
                int length = p_Length;
                int[] numArray = new int[p_Length];
                byte[] destinationArray = null;
                int num2 = 0;
                for (int i = 0; i < p_Length; i++)
                {
                    if (sourceArray[i] > 0x7f)
                    {
                        num2++;
                        if (num2 == 3)
                        {
                            num2 = 1;
                        }
                    }
                    else
                    {
                        num2 = 0;
                    }
                    numArray[i] = num2;
                }
                if ((sourceArray[p_Length - 1] > 0x7f) && (numArray[p_Length - 1] == 1))
                {
                    length = p_Length + 1;
                }
                destinationArray = new byte[length];
                Array.Copy(sourceArray, destinationArray, length);
                return (Encoding.Default.GetString(destinationArray) + p_TailString);
            }


            /// <summary>
            /// �� Stream ת���� string
            /// </summary>
            /// <param name="s">Stream��</param>
            /// <returns>string</returns>
            public static string ConvertStreamToString(Stream s)
            {
                string strResult = "";
                StreamReader sr = new StreamReader(s, Encoding.UTF8);

                Char[] read = new Char[256];

                // Read 256 charcters at a time.    
                int count = sr.Read(read, 0, 256);

                while (count > 0)
                {
                    // Dump the 256 characters on a string and display the string onto the console.
                    string str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }


                // �ͷ���Դ
                sr.Close();

                return strResult;
            }

            /// <summary>
            /// �Դ��ݵĲ����ַ������д�����ֹע��ʽ����
            /// </summary>
            /// <param name="str">���ݵĲ����ַ���</param>
            /// <returns>String</returns>
            public static string ConvertSql(string str)
            {
                str = str.Trim();
                str = str.Replace("'", "''");
                str = str.Replace(";--", "");
                str = str.Replace("=", "");
                str = str.Replace(" or ", "");
                str = str.Replace(" and ", "");

                return str;
            }


            /// <summary>
            /// ��ʽ��ռ�ÿռ��С�����
            /// </summary>
            /// <param name="size">��С</param>
            /// <returns>���� String</returns>
            public static string FormatNUM(long size)
            {
                decimal NUM;
                string strResult;

                if (size > 1073741824)
                {
                    NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1073741824));
                    strResult = NUM.ToString("N") + " G";
                }
                else if (size > 1048576)
                {
                    NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1048576));
                    strResult = NUM.ToString("N") + " M";
                }
                else if (size > 1024)
                {
                    NUM = (Convert.ToDecimal(size) / Convert.ToDecimal(1024));
                    strResult = NUM.ToString("N") + " KB";
                }
                else
                {
                    strResult = size + " �ֽ�";
                }

                return strResult;
            }

            /// <summary>
            /// �ж��ַ����Ƿ�Ϊ��Ч���ʼ���ַ
            /// </summary>
            /// <param name="email"></param>
            /// <returns></returns>
            public static bool IsValidEmail(string email)
            {
                return Regex.IsMatch(email, @"^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$");
            }

            /// <summary>
            /// �ж��ַ����Ƿ�Ϊ��Ч��URL��ַ
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public static bool IsValidURL(string url)
            {
                return Regex.IsMatch(url, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
            }

            /// <summary>
            /// �ж��ַ����Ƿ�ΪInt���͵�
            /// </summary>
            /// <param name="val"></param>
            /// <returns></returns>
            public static bool IsValidInt(string val)
            {
                return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
            }

            /// <summary>
            /// ����ַ����Ƿ�ȫΪ������
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool IsNum(string str)
            {
                bool blResult = true;//Ĭ��״̬��������

                if (str == "")
                    blResult = false;
                else
                {
                    foreach (char Char in str)
                    {
                        if (!char.IsNumber(Char))
                        {
                            blResult = false;
                            break;
                        }
                    }
                    if (blResult)
                    {
                        if (int.Parse(str) == 0)
                            blResult = false;
                    }
                }
                return blResult;
            }


            //�õ���url
            public static string GetUrlRoot(System.Web.HttpRequest Request)
            {
                string curpath = Request.Url.AbsoluteUri;
                int ipos = curpath.LastIndexOf("/");
                return curpath.Substring(0, ipos + 1);

            }

            /// <summary>
            /// ����ַ����Ƿ�ȫΪ������
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static bool IsDouble(string str)
            {
                bool blResult = true;//Ĭ��״̬��������

                if (str == "")
                    blResult = false;
                else
                {
                    foreach (char Char in str)
                    {
                        if (!char.IsNumber(Char) && Char.ToString() != "-")
                        {
                            blResult = false;
                            break;
                        }
                    }
                }
                return blResult;
            }

            /// <summary>
            /// �����ͬһ�ַ���ɵ�ָ�����ȵ��ַ���
            /// </summary>
            /// <param name="Char">����ַ����磺A</param>
            /// <param name="i">ָ������</param>
            /// <returns></returns>
            public static string Strings(char Char, int i)
            {
                string strResult = null;

                for (int j = 0; j < i; j++)
                {
                    strResult += Char;
                }
                return strResult;
            }


            /// <summary>
            /// �����ַ�������ʵ���ȣ�һ�������ַ��൱��������λ����
            /// </summary>
            /// <param name="str">ָ���ַ���</param>
            /// <returns></returns>
            public static int Len(string str)
            {
                int intResult = 0;

                foreach (char Char in str)
                {
                    if ((int)Char > 127)
                        intResult += 2;
                    else
                        intResult++;
                }
                return intResult;
            }


            /// <summary>
            /// ������Ϊ��׼���һ�����Ե�����
            /// </summary>
            /// <returns>���� String</returns>
            public static string MakeName()
            {
                /*
                string y = DateTime.Now.Year.ToString();
                string m = DateTime.Now.Month.ToString();
                string d = DateTime.Now.Day.ToString();
                string h = DateTime.Now.Hour.ToString();
                string n = DateTime.Now.Minute.ToString();
                string s = DateTime.Now.Second.ToString();
                return y + m + d + h + n + s;
                */

                return DateTime.Now.ToString("yyyyMMddHHmmssfff");
            }


            /// <summary>
            /// �����ַ�������ʵ���ȣ�һ�������ַ��൱��������λ����(ʹ��Encoding��)
            /// </summary>
            /// <param name="str">ָ���ַ���</param>
            /// <returns></returns>
            public static int GetLen(string str)
            {
                int intResult = 0;
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] bytes = gb2312.GetBytes(str);
                intResult = bytes.Length;
                return intResult;
            }


            /// <summary>
            /// �����ַ�����ʵ�ʳ��Ƚ�ȡָ�����ȵ��ַ���
            /// </summary>
            /// <param name="text">�ַ���</param>
            /// <param name="Length">ָ������</param>
            /// <param name="showomit">��ʾʡ�Ժ�</param>
            /// <returns></returns>
            public static string CutLen(string text, int length, string cutText)
            {
                if (text == null) return string.Empty;
                int i = 0, j = 0;
                foreach (char Char in text)
                {
                    if ((int)Char > 127)
                        i += 2;
                    else
                        i++;

                    if (i > length)
                    {
                        text = text.Substring(0, j);
                        text += cutText;
                        break;
                    }
                    j++;
                }
                return text;
            }


            /// <summary>
            /// ��ȡָ�����ȵĴ�����������ִ�
            /// </summary>
            /// <param name="intLong">���ִ�����</param>
            /// <returns>�ַ���</returns>
            public static string RandomNUM(int intLong)
            {
                string strResult = "";

                Random r = new Random();
                for (int i = 0; i < intLong; i++)
                {
                    strResult = strResult + r.Next(10);
                }

                return strResult;
            }

            /// <summary>
            /// ��ȡһ����26��Сд��ĸ��ɵ�ָ�����ȵ��漴�ַ���
            /// </summary>
            /// <param name="intLong">ָ������</param>
            /// <returns></returns>
            public static string RandomSTR(int intLong)
            {
                string strResult = "";
                string[] array = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

                Random r = new Random();

                for (int i = 0; i < intLong; i++)
                {
                    strResult += array[r.Next(26)];
                }

                return strResult;
            }

            /// <summary>
            /// ��ȡһ�������ֺ�26��Сд��ĸ��ɵ�ָ�����ȵ��漴�ַ���
            /// </summary>
            /// <param name="intLong">ָ������</param>
            /// <returns></returns>
            public static string RandomNUMSTR(int intLong)
            {
                string strResult = "";
                string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

                Random r = new Random();

                for (int i = 0; i < intLong; i++)
                {
                    strResult += array[r.Next(36)];
                }

                return strResult;
            }

            /// <summary>
            /// ����������ת���ɷ�������
            /// </summary>
            /// <param name="str">���������ַ���</param>
            /// <returns>string</returns>
            public static string ConvertToTraditionalChinese(string str)
            {
                return Microsoft.VisualBasic.Strings.StrConv(str, VbStrConv.TraditionalChinese, System.Globalization.CultureInfo.CurrentUICulture.LCID);
            }

            /// <summary>
            /// ����������ת���ɼ�������
            /// </summary>
            /// <param name="str">���������ַ���</param>
            /// <returns>string</returns>
            public static string ConvertToSimplifiedChinese(string str)
            {
                return Microsoft.VisualBasic.Strings.StrConv(str, VbStrConv.SimplifiedChinese, System.Globalization.CultureInfo.CurrentUICulture.LCID);
            }


            /// <summary>
            /// ��ָ���ַ����еĺ���ת��Ϊƴ������ĸ����д�����зǺ��ֱ���Ϊԭ�ַ�
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static string ConvertSpellFirst(string text)
            {
                char pinyin;
                byte[] array;
                StringBuilder sb = new StringBuilder(text.Length);
                foreach (char c in text)
                {
                    pinyin = c;
                    array = Encoding.Default.GetBytes(new char[] { c });

                    if (array.Length == 2)
                    {
                        int i = array[0] * 0x100 + array[1];

                        if (i < 0xB0A1) pinyin = c;
                        else
                            if (i < 0xB0C5) pinyin = 'a';
                            else
                                if (i < 0xB2C1) pinyin = 'b';
                                else
                                    if (i < 0xB4EE) pinyin = 'c';
                                    else
                                        if (i < 0xB6EA) pinyin = 'd';
                                        else
                                            if (i < 0xB7A2) pinyin = 'e';
                                            else
                                                if (i < 0xB8C1) pinyin = 'f';
                                                else
                                                    if (i < 0xB9FE) pinyin = 'g';
                                                    else
                                                        if (i < 0xBBF7) pinyin = 'h';
                                                        else
                                                            if (i < 0xBFA6) pinyin = 'j';
                                                            else
                                                                if (i < 0xC0AC) pinyin = 'k';
                                                                else
                                                                    if (i < 0xC2E8) pinyin = 'l';
                                                                    else
                                                                        if (i < 0xC4C3) pinyin = 'm';
                                                                        else
                                                                            if (i < 0xC5B6) pinyin = 'n';
                                                                            else
                                                                                if (i < 0xC5BE) pinyin = 'o';
                                                                                else
                                                                                    if (i < 0xC6DA) pinyin = 'p';
                                                                                    else
                                                                                        if (i < 0xC8BB) pinyin = 'q';
                                                                                        else
                                                                                            if (i < 0xC8F6) pinyin = 'r';
                                                                                            else
                                                                                                if (i < 0xCBFA) pinyin = 's';
                                                                                                else
                                                                                                    if (i < 0xCDDA) pinyin = 't';
                                                                                                    else
                                                                                                        if (i < 0xCEF4) pinyin = 'w';
                                                                                                        else
                                                                                                            if (i < 0xD1B9) pinyin = 'x';
                                                                                                            else
                                                                                                                if (i < 0xD4D1) pinyin = 'y';
                                                                                                                else
                                                                                                                    if (i < 0xD7FA) pinyin = 'z';
                    }

                    sb.Append(pinyin);
                }

                return sb.ToString();
            }

            private static int[] pyValue = new int[]
                {
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
                };

            private static string[] pyName = new string[]
                {
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
                };


            /// <summary>
            /// ��ָ���ַ����еĺ���ת��Ϊƴ����ĸ�����зǺ��ֱ���Ϊԭ�ַ�
            /// </summary>
            /// <param name="text">Ҫת�����ı�����</param>
            /// <returns>string</returns>
            public static string ConvertSpellFull(string hzString)
            {
                // ƥ�������ַ�
                Regex regex = new Regex("^[\u4e00-\u9fa5]$");
                // ƥ��Ӣ��
                Regex regex_en = new Regex("^[a-zA-Z0-9]$");
                byte[] array = new byte[2];
                string pyString = "";
                int chrAsc = 0;
                int i1 = 0;
                int i2 = 0;
                char[] noWChar = hzString.ToCharArray();

                for (int j = 0; j < noWChar.Length; j++)
                {
                    // �����ַ�
                    if (regex.IsMatch(noWChar[j].ToString()))
                    {
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // ������������
                            if (chrAsc == -9254)  // �������ڡ���
                                pyString += "Zhen";
                            else if (chrAsc == -10536)  // �������ء���
                                pyString += "Chong";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //Ӣ���ַ�
                    else if (regex_en.IsMatch(noWChar[j].ToString()))
                    {
                        pyString += noWChar[j].ToString();
                    }
                    // ���������ַ�
                    else
                    {
                        //pyString += noWChar[j].ToString();
                    }
                }
                return pyString;
            }

            /**/
            /// <summary>
            /// ȫ��ת��ǵĺ���(DBC case)
            /// </summary>
            /// <param name="input">�����ַ���</param>
            /// <returns>����ַ���</returns>
            ///<remarks>
            ///ȫ�ǿո�Ϊ12288����ǿո�Ϊ32
            ///�����ַ����(33-126)��ȫ��(65281-65374)�Ķ�Ӧ��ϵ�ǣ������65248
            ///</remarks>
            public static string ToDBC(string input)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    char[] c = input.ToCharArray();
                    for (int i = 0; i < c.Length; i++)
                    {
                        if (c[i] == 12288)
                        {
                            c[i] = (char)32;
                            continue;
                        }
                        if (c[i] > 65280 && c[i] < 65375)
                            c[i] = (char)(c[i] - 65248);
                    }
                    return new string(c);
                }
                return input;
            }
        }
        #endregion
    }
}
