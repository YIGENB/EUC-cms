using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;

/// <summary>
///JsonUtility 的摘要说明
/// </summary>
public class Utility
{
    /// <summary>
    /// Json工具类
    /// </summary>
    public static class JsonUtility
    {
        /// <summary>
        /// 添加时间转换器
        /// </summary>
        /// <param name="serializer"></param>
        private static void AddIsoDateTimeConverter(JsonSerializer serializer)
        {
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            //定义时间转化格式
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //idtc.DateTimeFormat = "yyyy-MM-dd";
            serializer.Converters.Add(idtc);
        }

        /// <summary>
        /// Json转换配置
        /// </summary>
        /// <param name="serializer"></param>
        private static void SerializerSetting(JsonSerializer serializer)
        {
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //serializer.NullValueHandling = NullValueHandling.Ignore;
            //serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            //serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
        }

        /// <summary>
        /// 返回结果消息编码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sucess"></param>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ReturnMessage(bool sucess, int total, string message, string exMessage, string data)
        {
            message = message.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "");
            exMessage = exMessage.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "");

            return string.Format("{{success:{0},total:{1},data:{2},message:\"{3}\",exMessage:\"{4}\"}}",
                sucess.ToString().ToLower(), total, data, message, exMessage);
        }

        /// <summary>
        /// 返回失败信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <returns></returns>
        public static string ReturnFailureMessage(string message, string exMessage)
        {
            return ReturnMessage(false, 0, message, exMessage, "[]");
        }

        /// <summary>
        /// 返回失败信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <returns></returns>
        public static string ReturnFailureMessageTouch(string message, string exMessage)
        {
            return "{\"success\":\"false\",\"msg\":\"" + exMessage + "\"}";
        }

        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <param name="objList"></param>
        /// <returns></returns>
        public static string ReturnSuccessMessage<T>(int total, string message, string exMessage, List<T> objList)
        {
            string data = ListToJson<T>(objList);
            return ReturnMessage(true, total, message, exMessage, data);
        }

        public static string ReturnSuccessMessageTouch<T>(T obj)
        {
            string data = ObjectToJson<T>(obj);
            return data;
        }

        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <param name="objList"></param>
        /// <returns></returns>
        public static string ReturnSuccessMessage(string message, string exMessage)
        {
            return ReturnMessage(true, 0, message, exMessage, "[]");
        }

        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <param name="objList"></param>
        /// <returns></returns>
        public static string ReturnSuccessMessageTouch(string message, string exMessage)
        {
            return "{\"success\":\"true\",\"msg\":\"" + message + "\"}";
        }



        /// <summary>
        /// 返回成功信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <param name="data">JSON 对象</param>
        /// <returns></returns>
        public static string ReturnSuccessMessage(string message, string exMessage, string data)
        {
            return ReturnMessage(true, 0, message, exMessage, "[" + data + "]");
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="message"></param>
        /// <param name="exMessage"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ReturnSuccessMessage<T>(int total, string message, string exMessage, T obj)
        {
            string data = ObjectToJson<T>(obj);
            return ReturnMessage(true, total, message, exMessage, data);
        }

        /// <summary>
        /// 把对象列表编码为Json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objList"></param>
        /// <returns></returns>
        public static string ListToJson<T>(List<T> objList)
        {
            JsonSerializer serializer = new JsonSerializer();
            SerializerSetting(serializer);
            AddIsoDateTimeConverter(serializer);

            using (TextWriter sw = new StringWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, objList);
                return sw.ToString();
            }
        }

        /// <summary>
        ///  把一个对象编码为Json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson<T>(T obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            SerializerSetting(serializer);
            AddIsoDateTimeConverter(serializer);

            using (TextWriter sw = new StringWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
                return sw.ToString();
            }
        }


        /// <summary>
        /// 根据传入的Json数据，解码为对象(一个)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DecodeObject<T>(string data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            AddIsoDateTimeConverter(serializer);
            StringReader sr = new StringReader(data);
            return (T)serializer.Deserialize(sr, typeof(T));


        }

        /// <summary>
        /// 功能同DecodeObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> DecodeObjectList<T>(string data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            AddIsoDateTimeConverter(serializer);
            StringReader sr = new StringReader(data);
            return (List<T>)serializer.Deserialize(sr, typeof(List<T>));
        }

        public static string EncodeAjaxResponseJson(string jsonString, string callback)
        {
            String responseString = "";
            //判断是否jsonp调用
            if (!String.IsNullOrEmpty(callback))
            {
                //jsonp调用，需要封装回调函数，并返回
                responseString = callback + "(" + jsonString + ")";
            }
            else
            {
                //普通ajax调用，直接返回Json数据
                responseString = jsonString;
            }

            return responseString;
        }

        public static string ExtGridSortInfo(string property, string direction)
        {
            return string.Format("[{{\"property\":\"{0}\",\"direction\":\"{1}\"}}]", property, direction);
        }
    }
}