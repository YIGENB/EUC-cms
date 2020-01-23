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
    public class UrlRewriteCofing
    {
        /// <summary>
        /// 站内地址配置路径
        /// </summary>
        public static readonly string path = ConfigurationManager.AppSettings["UrlRewrite"];

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns></returns>
        public static T GetUrlRewrite<T>()
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
    public class UrlRewrite
    {

        // Internal member variables

        private string product;
        private string article;
        private string page;
        private string htmlSuffix;

        private string htmlIndex;

        private string http;
        private string html;
        /// <summary>
        /// 静态地址目录
        /// </summary>
        public string Html
        {
            get { return html; }
            set { html = value; }
        }

        /// <summary>
        /// 协议类型
        /// </summary>
        public string Http
        {
            get { return http; }
            set { http = value; }
        }
        /// <summary>
        /// 首页地址
        /// </summary>
        public string HtmlIndex
        {
            get { return htmlIndex; }
            set { htmlIndex = value; }
        }
        /// <summary>
        /// 静态地址后缀名
        /// </summary>
        public string HtmlSuffix
        {
            get { return htmlSuffix; }
            set { htmlSuffix = value; }
        }

        /// <summary>
        /// 页面伪静态路径
        /// </summary>
        public string Page
        {
            get { return page; }
            set { page = value; }
        }
        private string download;
        /// <summary>
        /// 下载伪静态路径
        /// </summary>
        public string Download
        {
            get { return download; }
            set { download = value; }
        }
        private string productDetail;
        /// <summary>
        /// 产品详细页伪静态路径
        /// </summary>
        public string ProductDetail
        {
            get { return productDetail; }
            set { productDetail = value; }
        }
        private string articleDetail;
        /// <summary>
        /// 资讯详细页伪静态路径
        /// </summary>
        public string ArticleDetail
        {
            get { return articleDetail; }
            set { articleDetail = value; }
        }
        private string downloadDetail;
        /// <summary>
        /// 下载详细页伪静态路径
        /// </summary>
        public string DownloadDetail
        {
            get { return downloadDetail; }
            set { downloadDetail = value; }
        }
      
        /// <summary>
        /// 产品伪静态路径
        /// </summary>
        public string Product
        {
            get { return product; }
            set { product = value; }
        }

        /// <summary>
        /// 资讯伪静态路径
        /// </summary>
        public string Article
        {
            get { return article; }
            set { article = value; }
        }
    }
}
