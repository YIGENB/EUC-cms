using System;
using System.Collections.Generic;
using System.Web;

namespace DY.OAuthV2SDK.Helpers
{
    /// <summary>
    /// 常用函数助手
    /// </summary>
    public static class UtilHelper
    {
        /// <summary>
        /// json数据转对象
        /// </summary>
        /// <param name="strJson">json数据</param>
        /// <returns></returns>
        public static T ParseJson<T>(string strJson)
        {
            System.IO.StringReader s_reader = new System.IO.StringReader(strJson);
            try
            {
                Type j_serializer = Type.GetType("Newtonsoft.Json.JsonSerializer,Newtonsoft.Json", true, true);
                Type e_handel = Type.GetType("Newtonsoft.Json.MissingMemberHandling,Newtonsoft.Json");
                object serializer = Activator.CreateInstance(j_serializer);
                j_serializer.GetProperty("MissingMemberHandling").SetValue(serializer, e_handel.GetField("Ignore").GetValue(null), null);
                object value = j_serializer.GetMethod("Deserialize", new Type[] { typeof(System.IO.StringReader), typeof(Type) }).Invoke(serializer, new object[] { s_reader, typeof(T) });
                return (T)value;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            finally
            {
                s_reader.Close();
            }
        }

        /// <summary>
        /// 对象转json数据
        /// </summary>
        /// <param name="strJson">json数据</param>
        /// <returns></returns>
        public static string ParseJson(object objModel)
        {
            System.IO.StringWriter s_write = new System.IO.StringWriter();
            try
            {
                Type j_serializer = Type.GetType("Newtonsoft.Json.JsonSerializer,Newtonsoft.Json", true, true);
                Type e_handel = Type.GetType("Newtonsoft.Json.MissingMemberHandling,Newtonsoft.Json");
                object serializer = Activator.CreateInstance(j_serializer);
                j_serializer.GetProperty("MissingMemberHandling").SetValue(serializer, e_handel.GetField("Ignore").GetValue(null), null);
                j_serializer.GetMethod("Serialize", new Type[] { typeof(System.IO.StringWriter), typeof(object) }).Invoke(serializer, new object[] { s_write, objModel }); ;
                string value = s_write.ToString();
                return value;
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
            finally
            {
                s_write.Close();
            }
        }


        /// <summary>
        /// Utc时间转本地时间
        /// </summary>
        /// <param name="strValue">原格式：Wed Nov 17 15:07:48 +0800 2010</param>
        /// <returns></returns>
        public static string UtcToDateTime(string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                //原格式：Wed Nov 17 15:07:48 +0800 2010
                string[] str = strValue.Split(' ');
                //转格式：Wed Nov 17 2010 15:07:48
                return str[0] + " " + str[1] + " " + str[2] + " " + str[5] + " " + str[3];
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }
    }
}