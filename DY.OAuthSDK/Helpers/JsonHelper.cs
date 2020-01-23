using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace  DY.OAuthSDK.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 将实体类转换成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SetJsonSerialization<T>(T t) where T : class
        {
            DataContractJsonSerializer dataJson = new DataContractJsonSerializer(t.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                dataJson.WriteObject(ms, t);
                string strJson = Encoding.UTF8.GetString(ms.ToArray());
                return strJson;
            }
        }

        /// <summary>
        /// 将一个json字符串
        /// 转换为实体类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T GetJosnModel<T>(string jsonStr) where T : class , new()
        {
            //T t = Activator.CreateInstance<T>(); 另一种创建实体 ,通过反射来实现
            T t = new T();
            DataContractJsonSerializer dataJson = null;
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            {
                dataJson = new DataContractJsonSerializer(t.GetType());
                return (T)dataJson.ReadObject(ms);
            }
        }
    }
}
