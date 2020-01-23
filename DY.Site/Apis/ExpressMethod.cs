using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.IO;

namespace DY.Site
{
    /// <summary>
    /// 百度接口请求类
    /// </summary>
    public partial class ExpressMethod
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        } 

        #region 快递post
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="parameters">参数</param>
        /// <param name="charset">编码</param>
        /// <returns></returns>
        public static string GetPostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            DY.Entity.BaseConfigInfo config = DY.Config.BaseConfig.Get();
            HttpWebRequest request = null;
            //HTTPSQ请求  
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Headers.Add("apikey", config.Baidu_api_key);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream1 = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream1); //创建一个stream读取流  
            string html = sr.ReadToEnd();
            return html;

        }
        #endregion

        #region 快递post
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="parameters">参数</param>
        /// <param name="charset">编码</param>
        /// <param name="referer">来源地址</param>
        /// <returns></returns>
        public static string GetPostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset, string referer = "")
        {
            HttpWebRequest request = null;
            //如果需要POST数据     
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                //HTTPSQ请求  
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url + "?" + buffer.ToString()) as HttpWebRequest;
                request.Referer = referer;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = DefaultUserAgent;


                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream1 = response.GetResponseStream();   //获取响应的字符串流  
            StreamReader sr = new StreamReader(stream1,charset); //创建一个stream读取流  
            string html = sr.ReadToEnd();
            return html;

        }
        #endregion
    }
}
