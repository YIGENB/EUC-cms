namespace DY.Site
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.IO.Compression;

    /// <summary>
    /// 网络请求封装
    /// </summary>
    public class HttpHelper
    {
        private HttpWebRequest httpWebRequest;
        private HttpWebResponse httpWebResponse;
        public string UserAgent { get; set; }
        public string Accept { get; set; }
        public string ContentType { get; set; }

        /// 设置或获取提交的数据
        /// </summary>
        public string PostData { get; set; }

        /// <summary>
        /// 设置固定编码
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 是否保持连接
        /// </summary>
        public bool IsKeepAlive { get; set; }

        /// <summary>
        /// 是否开启Gzip压缩
        /// </summary>
        public bool IsGzip { get; set; }

        /// <summary>
        /// 超时时间（毫秒），默认15000
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 请求头集合
        /// </summary>
        public System.Net.WebHeaderCollection Headers { get; set; }

        /// <summary>
        ///  压缩类型
        /// </summary>
        public string CompressType { get; set; }

        /// <summary>
        /// 响应长度
        /// </summary>
        public long ResponseContentLength { get; private set; }

        public HttpHelper()
        {
            this.UserAgent = "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/5.0)";
            this.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            this.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            this.Timeout = 15000;
            this.Headers = new WebHeaderCollection();
            this.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            this.Headers.Add("Accept-Charset", "GB2312,utf-8;q=0.7,*;q=0.7");
            this.IsGzip = true;
        }

        public HttpHelper(bool iskeeplive)
            : this()
        {
            this.IsKeepAlive = iskeeplive;
        }

       

        #region 公共方法
        /// <summary>
        /// 获取HTML代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string GetHtml(string url, string ip = null)
        {
            return GetHtml(url, null, false, ip);
        }

        /// <summary>
        /// 获取HTML代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="isPost"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string GetHtml(string url, string postData, bool isPost, string ip = null)
        {
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpWebRequest.Headers = this.Headers;
            httpWebRequest.Accept = Accept;
            httpWebRequest.AllowAutoRedirect = true;
            if (this.IsGzip)
            {
                httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            }
            httpWebRequest.UserAgent = UserAgent;
            
            httpWebRequest.Method = isPost ? "POST" : "GET";
            if (isPost)
            {
                httpWebRequest.ContentType = ContentType;
            }

            httpWebRequest.KeepAlive = this.IsKeepAlive;
            byte[] byteRequest = null;
            if (!string.IsNullOrEmpty(postData))
            {
                byteRequest = Encoding.GetBytes(postData);
                httpWebRequest.ContentLength = byteRequest.Length;
            }
            try
            {
                if (!string.IsNullOrEmpty(postData))
                {
                    using (Stream SendStream = httpWebRequest.GetRequestStream())
                    {
                        SendStream.Write(byteRequest, 0, byteRequest.Length);
                    }
                }
                IAsyncResult result = httpWebRequest.BeginGetResponse(null, null);
                bool bl = result.AsyncWaitHandle.WaitOne(this.Timeout, true);
                if (!bl)
                {
                    throw new WebException("主机连接超时", WebExceptionStatus.Timeout);
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(result);
                this.ResponseContentLength = httpWebResponse.ContentLength;
                

                CompressType = httpWebResponse.ContentEncoding.ToLower();
                
                Stream stream = null;
                if (CompressType == "gzip")
                {
                    this.IsGzip = true;
                    stream = new System.IO.Compression.GZipStream(httpWebResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (CompressType == "deflate")
                {
                    this.IsGzip = true;
                    stream = new System.IO.Compression.DeflateStream(httpWebResponse.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    this.IsGzip = false;
                    stream = httpWebResponse.GetResponseStream();
                }
                using (System.IO.StreamReader sr = new StreamReader(stream, Encoding))
                {
                    return sr.ReadToEnd();
                }
            }
            catch 
            {
                return null;
            }
            finally
            {
                if (httpWebResponse != null)
                    httpWebResponse.Close();
                httpWebRequest.Abort();
            }
        }
        #endregion

    }
}
