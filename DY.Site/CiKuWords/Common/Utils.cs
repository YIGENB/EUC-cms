using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace DY.Site
{
    public class KWUtils
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        public const string ErrorInfoFormat = "ERROR:{0}";

        /// <summary>
        /// 
        /// </summary>
        public const string MessageTip = "以下是{0}”{1}“的查询结果";

        public const string MessageTipLoading = "正在处理请求，请稍候。。。";

        public const string TipMessage = "Tips:测试状态下只返回测试数据";

        /// <summary>
        /// 分组符
        /// </summary>
        public const char gs = (char)29;
        public static readonly string strgs = ((char)29).ToString();

        public static Regex htmlReg = new Regex("<[^>]+>|&[a-z]+;");

        #region DES加解密
        /// <summary>
        /// 默认密钥
        /// </summary>
        public const string DefaultEncryptKey = "!@wgsd54#$#@f";
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static string Encode(string encryptString)
        {
            return Encode(encryptString,DefaultEncryptKey);
        }
        public static string Decode(string decryptString)
        {
            return Decode(decryptString, DefaultEncryptKey);
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = GetUnicodeSubString(encryptKey, 8, "");
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
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = GetUnicodeSubString(decryptKey, 8, "");
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
                return "";
            }

        }
        #endregion

        /// <summary>
        /// MD5加密方法
        /// </summary>
        /// <param name="strEncrypt">加密的内容</param>
        /// <returns>返回加密后的内容</returns>
        public static string MD5Encrypt(string strEncrypt)
        {
            strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(strEncrypt, "MD5");
            return strEncrypt;
        }

        /// <summary>
        /// 字符串截取一个汉字两个字节
        /// </summary>
        /// <param name="str">被截取的数据</param>
        /// <param name="len">截取的长度</param>
        /// <param name="p_TailString">超出后替换字符</param>
        /// <returns></returns>
        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }

        /// <summary>
        /// URL UTF-8编码
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public static string UrlEncode(string instr)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                return System.Web.HttpUtility.UrlEncode(instr, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="SearchStr">原始字符串</param>
        /// <param name="StartStr">起始截取</param>
        /// <param name="EndStr">终止截取</param>
        /// <returns></returns>
        public static string GetSubString(string SearchStr, string StartStr, string EndStr)
        {
            if (string.IsNullOrEmpty(SearchStr) || string.IsNullOrEmpty(StartStr))
            {
                return string.Empty;
            }

            int b = SearchStr.IndexOf(StartStr);
            if (b == -1)
                return string.Empty;
            int startIndex = b + StartStr.Length;
            if (startIndex >= SearchStr.Length)
                return string.Empty;
            if (string.IsNullOrEmpty(EndStr))
            {
                return SearchStr.Substring(startIndex);
            }
            int e = SearchStr.IndexOf(EndStr, startIndex);
            if (e == -1)
                return string.Empty;
            return SearchStr.Substring(startIndex, e - startIndex);
        }

       /// <summary>
        /// 字符串截取
       /// </summary>
       /// <param name="SearchStr"></param>
       /// <param name="length">长度</param>
       /// <param name="replaceMent">替代字符</param>
       /// <returns></returns>
        public static string GetSubString(string SearchStr, int length,string replaceMent)
        {
            if (string.IsNullOrEmpty(SearchStr))
            {
                return string.Empty;
            }
            if (SearchStr.Length > length)
                SearchStr = SearchStr.Substring(0, length) + replaceMent;
            return SearchStr;
        }

        /// <summary>
        /// 转成中文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UnicodeToString(string s)
        {
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            return reg.Replace(s, delegate(Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        }

        public static int GetPageNum(int total, int pageSize)
        {
            if (total == 0)
                return 0;
            if (total % pageSize == 0)
                return total / pageSize;
            else
                return total / pageSize + 1;
        
        }
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
    }
}
