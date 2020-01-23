using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Data;

using DY.Entity;
using System.IO;

namespace DY.Common
{
    public class LanguageConfig
    {
        /// <summary>
        /// 语言包路径
        /// </summary>
        public static readonly string path = ConfigurationManager.AppSettings["Language"];

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns></returns>
        public static T GetLanguage<T>()
        {
            return (T)Load(typeof(T), HttpContext.Current.Server.MapPath(path));
        }

        #region 文本化XML反序列化
        /// <summary>
        /// 文件化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// 文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }
        #endregion
    }
    public class Language
    {

        // Internal member variables

        private string index;
        private string end;
        private string onPage;
        private string next;
        private string total;
        private string information;
        private string eachPage;
        //class
        private string indexClass;
        private string fristClass;
        private string shuClass;
        private string nextClass;
        private string endClass;
        private string yesClass;

        public string YesClass
        {
            get { return yesClass; }
            set { yesClass = value; }
        }

        public string EndClass
        {
            get { return endClass; }
            set { endClass = value; }
        }

        public string NextClass
        {
            get { return nextClass; }
            set { nextClass = value; }
        }

        public string ShuClass
        {
            get { return shuClass; }
            set { shuClass = value; }
        }

        public string FristClass
        {
            get { return fristClass; }
            set { fristClass = value; }
        }

        public string IndexClass
        {
            get { return indexClass; }
            set { indexClass = value; }
        }

        public string EachPage
        {
            get { return eachPage; }
            set { eachPage = value; }
        }

        public string Information
        {
            get { return information; }
            set { information = value; }
        }


        public string Total
        {
            get { return total; }
            set { total = value; }
        }

        public string Next
        {
            get { return next; }
            set { next = value; }
        }

        public string OnPage
        {
            get { return onPage; }
            set { onPage = value; }
        }

        public string End
        {
            get { return end; }
            set { end = value; }
        }

        public string Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}
