using DY.OAuthSDK.Connect.Model;
using DY.OAuthSDK.Connect.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DY.OAuthSDK.Login
{
    public abstract class Connect
    {
        protected String appKey = String.Empty;
        protected String appSecret = String.Empty;
        protected String callbackUrl = String.Empty;

        public String GetAppKey
        {
            get { return this.appKey; }
        }

        public String GetAppSecret
        {
            get { return this.appSecret; }
        }

        public String GetCallBackUrl
        {
            get { return this.callbackUrl; }
        }

        protected Connect(QQSite site)
        {
            switch (site)
            {
                case QQSite.Tentent:
                    this.appKey = QQ_APPKEY;
                    this.appSecret = QQ_APPSECRET;
                    break;
                case QQSite.Weibo:
                    this.appKey = SINA_APPKEY;
                    this.appSecret = SINA_APPSECRET;
                    break;
            }
            this.callbackUrl = CALLBACK;
        }

        /// <summary>
        /// OAuth2的authorize接口
        /// 获取Authorization Code
        /// </summary>
        public abstract String GetAuthorizeURL(ResponseType response = ResponseType.Code, string state = null, DisplayType display = DisplayType.Default);

        public abstract AccessToken GetAccessTokenByAuthorizationCode(String code);

        internal String Request(String url, RequestMethod method, params WeiboParaeter[] parameter)
        {
            String result = String.Empty;
            UriBuilder uri = new UriBuilder(url);
            uri.Query = SkyUtility.BuildQueryString(parameter);
            HttpWebRequest http = HttpWebRequest.Create(uri.Uri) as HttpWebRequest;
            http.ServicePoint.Expect100Continue = false;
            http.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.0)";
            switch (method)
            {
                case RequestMethod.Get:
                    http.Method = "GET";
                    break;
                case RequestMethod.Post:
                    http.Method = "Post";
                    http.ContentType = "application/x-www-form-urlencoded";
                    using (StreamWriter sw = new StreamWriter(http.GetRequestStream()))
                    {
                        try
                        {
                            sw.Write(SkyUtility.BuildQueryString(parameter));
                        }
                        catch (IOException ex)
                        {
                            throw new Exception("post写入数据出错:" + ex.Message);
                        }
                    }
                    break;
            }
            try
            {
                using (HttpWebResponse response = http.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (System.Net.WebException exc)
            {
                if (exc.Response != null)
                {
                    using (StreamReader sr = new StreamReader(exc.Response.GetResponseStream()))
                    {
                        String message = sr.ReadToEnd();
                        throw new Exception(message);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 以下是自己申请的开发者账号
        /// 新浪与腾讯,务必填写正确
        /// </summary>
        private const String SINA_APPKEY = "";
        private const String SINA_APPSECRET = "";
        private const String CALLBACK = "http://www.xs521.cn";//返回页面

        private const String QQ_APPKEY = "100258447";
        private const String QQ_APPSECRET = "22e9167adfe8d54c974d32f1fb178812";
    }
}
