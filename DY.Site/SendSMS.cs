﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Data;
using DY.Entity;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Xml.Serialization;
using System.Xml;
using DY.Site.SMS;

/*例子
                  SMSInfo smsinfo = new SMSInfo();
                 smsinfo.Mobiles = "18680769880";
                 smsinfo.Content = entity.email_content;
                 SendSMS.HttpPostSMS(smsinfo);//返回错误代码
 */
namespace DY.Site
{
    /// <summary>
    /// 短信相关类
    /// </summary>
    public class SendSMS
    {
        /// <summary>
        /// 发送短信返回结果
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public static string HttpPostSMS(SMSInfo sms)
        {
            EmsServicesClient ems = new EmsServicesClient();
            string xml= ems.sendSMS(sms.EnterpriseID, sms.LoginName, System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sms.Password, "MD5").ToLower(), sms.SmsId, sms.SubPort, sms.Content, sms.Mobiles, sms.SendTime);

            //ASCIIEncoding ascii = new ASCIIEncoding();
            //Byte[] encodedBytes = ascii.GetBytes(sms.Content);
            HttpPost httppost = new HttpPost();
            //HttpItem objhttpitem=new HttpItem();
            //objhttpitem.Encoding="utf-8";
            //objhttpitem.Method="POST";
            //objhttpitem.URL = sms.Url;
            //objhttpitem.Cookie = "postsms";
            //objhttpitem.Postdata="enterpriseID="+sms.EnterpriseID+"&loginName="+sms.LoginName+"&password="+System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sms.Password, "MD5").ToLower()+"&smsId="+sms.SmsId+"&subPort="+sms.SubPort+"&content="+sms.Content+"&mobiles="+sms.Mobiles+"&sendTime="+ sms.SendTime;
            //string ssss = httppost.GetHtml(objhttpitem);
            return httppost.FromXml<Response>(xml).Result;
        }
    }

    #region HttpPost
    public class HttpPost
    {

        public string HttpRequest(HttpItem objhttpitem)
        {
            string str = "";
            System.Net.HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(GetUrl(objhttpitem.URL));
            request.Method = objhttpitem.Method;
            Stream newStream = request.GetRequestStream();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = encoding.GetBytes(objhttpitem.Postdata);
            newStream.Write(postdata, 0, objhttpitem.Postdata.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            str = reader.ReadToEnd();
            response.Close();
            return str;
        }

        #region 预定义方法或者变更

        //默认的编码
        private Encoding encoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest request = null;
        //获取影响流的数据对象
        private HttpWebResponse response = null;
        //读取流的对象
        private StreamReader reader = null;
        //需要返回的数据对象
        private string returnData = "String Error";

        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private string GetHttpRequestData(HttpItem objhttpitem)
        {
            try
            {
                #region 得到请求的response

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    try
                    {
                        objhttpitem.CookieCollection = response.Cookies;
                        objhttpitem.Cookie = response.Headers["set-cookie"].Trim();
                    }
                    catch { }
                    objhttpitem.Response = response;
                    objhttpitem.Request = request;
                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {

                        MemoryStream _stream = new MemoryStream();
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            objhttpitem.Reader = reader;
                            //开始读取流并设置编码方式
                            //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                            //.net4.0以下写法
                            _stream = GetMemoryStream(response.GetResponseStream());
                        }
                        else
                        {
                            objhttpitem.Reader = reader;
                            //response.GetResponseStream().CopyTo(_stream, 10240);
                            // .net4.0以下写法
                            _stream = GetMemoryStream(response.GetResponseStream());
                        }
                        byte[] RawResponse = _stream.ToArray();
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        //<meta(.*?)charset([\s]?)=[^>](.*?)>
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        returnData = encoding.GetString(RawResponse);
                    }
                    else
                    {
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //开始读取流并设置编码方式
                            using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding))
                            {
                                objhttpitem.Reader = reader;
                                returnData = reader.ReadToEnd();

                            }
                        }
                        else
                        {
                            //开始读取流并设置编码方式
                            using (reader = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                objhttpitem.Reader = reader;
                                returnData = reader.ReadToEnd();
                            }
                        }
                    }
                }

                #endregion
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                returnData = "String Error";
                response = (HttpWebResponse)ex.Response;
                objhttpitem.Response = response;
            }
            if (objhttpitem.IsToLower)
            {
                returnData = returnData.ToLower();
            }
            return returnData;
        }

        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            // write the required bytes  
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }

        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="objhttpItem">参数列表</param>
        /// <param name="_Encoding">读取数据时的编码方式</param>
        private void SetRequest(HttpItem objhttpItem)
        {
            #region 验证证书

            if (!string.IsNullOrEmpty(objhttpItem.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback =
                    new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.URL));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(objhttpItem.CerPath);
                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objhttpItem.URL));
            }
            #endregion

            #region 设置代理
            if (string.IsNullOrEmpty(objhttpItem.ProxyUserName) && string.IsNullOrEmpty(objhttpItem.ProxyPwd) && string.IsNullOrEmpty(objhttpItem.ProxyIp))
            {
                //不需要设置
            }
            else
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(objhttpItem.ProxyIp, false);
                //建议连接
                myProxy.Credentials = new NetworkCredential(objhttpItem.ProxyUserName, objhttpItem.ProxyPwd);
                //给当前请求对象
                request.Proxy = myProxy;
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            #endregion

            //请求方式Get或者Post
            request.Method = objhttpItem.Method;
            //Accept
            request.Accept = objhttpItem.Accept;
            //ContentType返回类型
            request.ContentType = objhttpItem.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = objhttpItem.UserAgent;

            #region 编码
            if (string.IsNullOrEmpty(objhttpItem.Encoding.ToLower().Trim()) || objhttpItem.Encoding.ToLower().Trim() == "null")
            {
                //读取数据时的编码方式
                encoding = null;
            }
            else
            {
                //读取数据时的编码方式
                encoding = System.Text.Encoding.GetEncoding(objhttpItem.Encoding);
            }
            #endregion

            #region Cookie
            if (string.IsNullOrEmpty(objhttpItem.Cookie))
            {
                //设置Cookie
                request.CookieContainer.Add(objhttpItem.CookieCollection);
            }
            else
            {
                //Cookie
                request.Headers[HttpRequestHeader.Cookie] = objhttpItem.Cookie;
            }
            #endregion

            //来源地址
            request.Referer = objhttpItem.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = objhttpItem.Allowautoredirect;

            #region Post数据
            //验证在得到结果时是否有传入数据
            if (!string.IsNullOrEmpty(objhttpItem.Postdata) && request.Method.Trim().ToLower().Contains("post"))
            {
                byte[] buffer = encoding.GetBytes(objhttpItem.Postdata);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }
            #endregion

            //设置最大连接
            if (objhttpItem.Connectionlimit > 0)
            {
                request.ServicePoint.ConnectionLimit = objhttpItem.Connectionlimit;
            }
        }

        //回调验证证书问题
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }

        #endregion

        #region 普通类型

        /// <summary>    
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>    
        /// <param name="URL">url</param>   
        /// <returns>
        /// </returns>    
        public static string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
        }

        ///<summary>
        ///采用https协议访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="objhttpItem">参数列表</param>
        ///<returns>String类型的数据</returns>
        public string GetHtml(HttpItem objhttpItem)
        {
            //准备参数
            SetRequest(objhttpItem);

            //调用专门读取数据的类
            return GetHttpRequestData(objhttpItem);
        }
        #endregion

        #region 文本化XML反序列化
        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromXml<T>(string str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {

                return (T)serializer.Deserialize(reader);
            }
        }
        #endregion
    }
    #endregion

    #region 发送短信结果
    /// <summary>
    /// 发送短信结果
    /// </summary>
    public class Response
    {
        [XmlElement("Result")]//发送短信结果
        public string Result { get; set; }

        [XmlElement("SmsId")]// 消息id,用于配对状态报告,每个包提交返回消息id唯一。若发送时设置消息id，则返回是设置消息id值。
        public string SmsId { get; set; }
    }
#endregion

#region Http请求参考类 
     /// <summary>
    /// Http请求参考类 
    /// </summary>
    public class HttpItem
    {
        string _URL;
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }

        string _Method = "GET";
        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }

        string _Accept = "text/html, application/xhtml+xml, */*";
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }

        string _ContentType = "text/html";
        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }

        string _Encoding = "NULL";
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }

        string _Postdata;
        /// <summary>
        /// Post请求时要发送的Post数据
        /// </summary>
        public string Postdata
        {
            get { return _Postdata; }
            set { _Postdata = value; }
        }

        string _Cookie;
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }

        string _Referer;
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return _Referer; }
            set { _Referer = value; }
        }

        string _CerPath = string.Empty;
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return _CerPath; }
            set { _CerPath = value; }
        }

        CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }

        private HttpWebRequest request;
        /// <summary>
        /// HttpWebRequest对象用来发起请求
        /// </summary>
        public HttpWebRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        private HttpWebResponse response;
        /// <summary>
        /// 获取影响流的数据对象
        /// </summary>
        public HttpWebResponse Response
        {
            get { return response; }
            set { response = value; }
        }

        private Boolean isToLower = true;
        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public Boolean IsToLower
        {
            get { return isToLower; }
            set { isToLower = value; }
        }

        private StreamReader reader;
        /// <summary>
        ///  读取流的对象
        /// </summary>
        public StreamReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        private Boolean allowautoredirect = true;
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public Boolean Allowautoredirect
        {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }

        private int connectionlimit = 1024;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }

        private string proxyusername = string.Empty;
        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyusername; }
            set { proxyusername = value; }
        }

        private string proxypwd = string.Empty;
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }

        private string proxyip = string.Empty;
        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyip; }
            set { proxyip = value; }
        }
    }
    #endregion

                                                                                                                                                                                                                                                                                                                                                                                #region 发送短信字段
    /// <summary>
    /// 发送短信字段
    /// </summary>
    public class SMSInfo
    {
        private string enterpriseID = "13520";
        private string password = "ctmon";
        private string loginName = "admin";
        private string url = "http://119.145.9.12/sendSMS.action"; //发送短信接口
        private string smsId;
        private string subPort;
        private string content;
        private string mobiles;
        private string sendTime;

        /// <summary>
        /// 定时发送时间
        /// </summary>
        public string SendTime
        {
          get { return sendTime; }
          set { sendTime = value; }
        }

        /// <summary>
        /// 接收手机号码(String，不可以为空。支持移动，联通，电信混合提交;多个号码中间采用半角逗号分隔,每个包最大支持100个号码。例：13500000000,13000000000,15100000000)
        /// </summary>
        public string Mobiles
        {
          get { return mobiles; }
          set { mobiles = value; }
        }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string Content
        {
          get { return content; }
          set { content = value; }
        }

        /// <summary>
        /// 扩展端口String,可以为空。空表示不再扩展，若扩展必须是数字如：00，01，02…若扩展输入00则手机收到端口号为：10657*****00(备注：扩展必须此帐户配置通道支持)
        /// </summary>
        public string SubPort
        {
          get { return subPort; }
          set { subPort = value; }
        }

        /// <summary>
        /// 消息id
        /// </summary>
        public string SmsId
        {
          get { return smsId; }
          set { smsId = value; }
        }

        /// <summary>
        /// 企业ID
        /// </summary>
        public string EnterpriseID
        {
            get { return enterpriseID; }
            set { enterpriseID = value; }
        }
        
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; }
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

    }
#endregion
}
