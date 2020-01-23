using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Xml.Serialization;

using DY.Entity;
using System.IO;
using System.Xml;

namespace DY.Common
{
    public class RollNewsXMLHelper
    {
        /// <summary>
        /// 文本化XML序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToXml<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromXml<T>(string str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }



    [XmlType(TypeName = "response")]
    public class response
    {
        public int code { get; set; }

        public string msg { get; set; }

        public string dext { get; set; }

    }

    [XmlType(TypeName = "data")]
    public class data
    {
        public int count { get; set; }

        public int page { get; set; }

        public string article_info { get; set; }

    }
    [XmlType(TypeName = "root")]
    public class Rollroot
    {
        // Internal member variables
        public data data { get; set; }
        public response response { get; set; }
    }
}

