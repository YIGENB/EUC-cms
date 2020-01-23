using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Web;

namespace DY.OAuthV2SDK.Helpers
{
    public class HttpHelper
    {
        #region HttpWebRequest请求
        /// <summary>
        /// 同步方式发起http get请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">参数字符串</param>
        /// <returns>请求返回值</returns>
        public static string HttpGet(string url, string queryString)
        {
            string responseData = null;

            if (!string.IsNullOrEmpty(queryString))
            {
                url += "?" + queryString.Trim(' ', '?', '&');
            }

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Timeout = 20000;
            webRequest.KeepAlive = false;

            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;

        }

        /// <summary>
        /// 获取返回结果http get请求
        /// </summary>
        /// <param name="webRequest">webRequest对象</param>
        /// <returns>请求返回值</returns>
        protected static string WebResponseGet(HttpWebRequest webRequest)
        {
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader responseReader = null;
                string responseData = String.Empty;
                responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
                responseData = responseReader.ReadToEnd();
                responseReader.Close();
                responseReader = null;
                return responseData;
            }
            catch (WebException wex)
            {
                string responseData = String.Empty;
                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    try
                    {
                        responseData = String.Empty;
                        StreamReader responseReader = new StreamReader(wex.Response.GetResponseStream());
                        responseData = responseReader.ReadToEnd();
                        responseReader.Close();
                        responseReader = null;
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(responseData))
                {
                    return responseData;
                }
                throw wex;
            }
        }


        /// <summary>
        /// 同步方式发起http get请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="paras">请求参数列表</param>
        /// <returns>请求返回值</returns>
        public static string HttpGet(string url, NameValueCollection paras)
        {
            string querystring = GetQueryFromParas(paras);
            return HttpGet(url, querystring);
        }



        /// <summary>
        /// 同步方式发起http post请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">参数字符串</param>
        /// <returns>请求返回值</returns>
        public static string HttpPost(string url, string queryString)
        {
            StreamWriter requestWriter = null;

            string responseData = null;

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Timeout = 20000;
            webRequest.KeepAlive = false;

            try
            {
                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(queryString);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (requestWriter != null)
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }


            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;

        }
        /// <summary>
        /// 同步方式发起http post请求
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="paras">请求参数列表</param>
        /// <returns>请求返回值</returns>
        public static string HttpPost(string url, NameValueCollection paras)
        {
            string querystring = GetQueryFromParas(paras);
            return HttpPost(url, querystring);
        }
        /// <summary>
        /// 同步方式发起http post请求，可以同时上传文件
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">请求参数字符串</param>
        /// <param name="files">上传文件列表</param>
        /// <returns>请求返回值</returns>
        public static string HttpPostWithFile(string url, string queryString, NameValueCollection files)
        {
            return HttpPostWithFile(url, GetParasFromQuery(queryString), files, false);
        }
        /// <summary>
        /// 同步方式发起http post请求，可以同时上传文件
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="queryString">请求参数字符串</param>
        /// <param name="files">上传文件列表</param>
        /// <param name="parasEncode">参数是否编码(解决中文乱码)</param>
        /// <returns>请求返回值</returns>
        public string HttpPostWithFile(string url, string queryString, NameValueCollection files, bool parasEncode)
        {
            return HttpPostWithFile(url, GetParasFromQuery(queryString), files, parasEncode);
        }
        /// <summary>
        /// 同步方式发起http post请求，可以同时上传文件
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="paras">请求参数列表</param>
        /// <param name="files">上传文件列表</param>
        /// <returns>请求返回值</returns>
        public static  string HttpPostWithFile(string url, NameValueCollection paras, NameValueCollection files)
        {
            return HttpPostWithFile(url, paras, files, false);
        }
        /// <summary>
        /// 同步方式发起http post请求，可以同时上传文件
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="paras">请求参数列表</param>
        /// <param name="files">上传文件列表</param>
        /// <param name="parasEncode">参数是否编码(解决中文乱码)</param>
        /// <returns>请求返回值</returns>
        public static  string HttpPostWithFile(string url, NameValueCollection paras, NameValueCollection files, bool parasEncode)
        {
            Stream requestStream = null;
            string responseData = null;
            string boundary = DateTime.Now.Ticks.ToString("x");

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Timeout = 20000;
            webRequest.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            webRequest.Method = "POST";
            webRequest.KeepAlive = false;
            webRequest.Credentials = CredentialCache.DefaultCredentials;


            try
            {
                Stream memStream = new MemoryStream();

                byte[] beginBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
                byte[] endBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                foreach (string key in paras.AllKeys)
                {
                    // 写入头
                    memStream.Write(beginBoundary, 0, beginBoundary.Length);

                    string value = parasEncode == true ? System.Web.HttpUtility.UrlDecode(paras[key]) : paras[key];
                    string formitem = string.Format(formdataTemplate, key, value);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: \"{2}\"\r\n\r\n";

                foreach (string key in files.AllKeys)
                {
                    string name = key;
                    string filePath = files[key];
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string file = Path.GetFileName(filePath);
                        string contentType = GetContentType(file);

                        // 写入头
                        memStream.Write(beginBoundary, 0, beginBoundary.Length);

                        string header = string.Format(headerTemplate, name, file, contentType);
                        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                        memStream.Write(headerbytes, 0, headerbytes.Length);

                        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[1024];
                        int bytesRead = 0;

                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            memStream.Write(buffer, 0, bytesRead);
                        }

                        // 写入结尾
                        memStream.Write(endBoundary, 0, endBoundary.Length);

                        fileStream.Close();
                    }
                }

                webRequest.ContentLength = memStream.Length;

                requestStream = webRequest.GetRequestStream();

                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                    requestStream = null;
                }
            }

            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }
        #endregion
        #region 参数转化操作
        /// <summary>
        /// 参数集合转化成查询语句
        /// </summary>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string GetQueryFromParas(NameValueCollection paras)
        {
            if (paras != null || paras.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (string key in paras.AllKeys)
                {
                    list.Add(string.Format("{0}={1}", key, HttpUtility.UrlEncode(paras[key])));
                }
                return string.Join("&", list.ToArray());
            }

            return string.Empty;
        }
        /// <summary>
        /// 查询语句转化成参数集合
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static NameValueCollection GetParasFromQuery(string query)
        {
            NameValueCollection paras = new NameValueCollection();
            if (!string.IsNullOrEmpty(query))
            {
                foreach (string item in query.Trim(' ', '?', '&').Split('&'))
                {
                    if (item.IndexOf('=') > -1)
                    {
                        string[] temp = item.Split('=');
                        paras.Add(temp[0], temp[1]);
                    }
                    else
                    {
                        paras.Add(item, string.Empty);
                    }
                }
            }
            return paras;
        }
        /// <summary>
        /// 根据文件名获取文件类型
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetContentType(string fileName)
        {
            string contentType = "application/octetstream";
            string ext = Path.GetExtension(fileName).ToLower();
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ext);

            if (registryKey != null && registryKey.GetValue("Content Type") != null)
            {
                contentType = registryKey.GetValue("Content Type").ToString();
            }

            return contentType;
        }
        #endregion
    }
}