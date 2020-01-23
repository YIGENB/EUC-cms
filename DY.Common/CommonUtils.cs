using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Threading;

namespace DY.Common
{
    /// <summary>
    /// WebUtility : ����System.Web����չ�ࡣ
    /// </summary>
    public abstract class CommonUtils
    {
        /// <summary>
        /// ���ָ���� Uri �Ƿ���Ч
        /// </summary>
        /// <param name="url">ָ����Url��ַ</param>
        /// <returns>bool</returns>
        public static bool ValidateUrl(string url)
        {
            Uri newUri = new Uri(url);

            try
            {
                WebRequest req = WebRequest.Create(newUri);
                //req.Timeout				= 10000;
                WebResponse res = req.GetResponse();
                HttpWebResponse httpRes = (HttpWebResponse)res;

                if (httpRes.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #region �ļ�����
        // ���Ӳ���ļ����ṩ���� ֧�ִ��ļ����������ٶ����ơ���Դռ��С
        // ������� _fileName: �����ļ����� _fullPath: ���ļ�������·���� _speed ÿ���������ص��ֽ���
        // �����Ƿ�ɹ�
        /// <summary>
        /// ���Ӳ���ļ����ṩ���� ֧�ִ��ļ����������ٶ����ơ���Դռ��С
        /// </summary>
        /// <param name="_fileName">�����ļ���</param>
        /// <param name="_fullPath">���ļ�������·��</param>
        /// <param name="_speed">ÿ���������ص��ֽ���</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool DownloadFile(string _fullPath, string _fileName, long _speed)
        {
            HttpRequest _Request = System.Web.HttpContext.Current.Request;
            HttpResponse _Response = System.Web.HttpContext.Current.Response;

            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes
                    //int sleep = 200;   //ÿ��5��   ��5*10K bytesÿ��
                    double dblValue = 1000 * pack / _speed;
                    int sleep = (int)Math.Floor(dblValue) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));


                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    dblValue = (fileLength - startBytes) / pack;
                    int maxCount = (int)Math.Floor(dblValue) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ���ط������ϵ��ļ��������ڷ�WEB·���£����Ǵ��ļ����÷�����IE�в�����ʾ���ؽ��ȣ�
        /// </summary>
        /// <param name="path">�����ļ��ľ���·��</param>
        /// <param name="saveName">������ļ�����������׺��</param>
        public static void Download(string path, string saveName)
        {
            HttpResponse Response = System.Web.HttpContext.Current.Response;

            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveName));
            Response.TransmitFile(path);
            Response.End();
        }


        /// <summary>
        /// ���ط������ϵ��ļ��������ڷ�WEB·���£����Ǵ��ļ����÷�����IE�л���ʾ���ؽ��ȣ�
        /// </summary>
        /// <param name="path">�����ļ��ľ���·��</param>
        /// <param name="saveName">������ļ�����������׺��</param>
        public static void DownloadFile(string path, string saveName)
        {
            Stream iStream = null;


            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new Byte[10240];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file to download including its path.
            string filepath = path;

            // Identify the file name.
            string filename = Path.GetFileName(filepath);

            try
            {
                // Open the file.
                iStream = new System.IO.FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Web.HttpContext.Current.Response.Clear();

                // Total bytes to read:
                dataToRead = iStream.Length;

                long p = 0;
                if (System.Web.HttpContext.Current.Request.Headers["Range"] != null)
                {
                    System.Web.HttpContext.Current.Response.StatusCode = 206;
                    p = long.Parse(System.Web.HttpContext.Current.Request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
                }
                if (p != 0)
                {
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Range", "bytes " + p.ToString() + "-" + ((long)(dataToRead - 1)).ToString() + "/" + dataToRead.ToString());
                }
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", ((long)(dataToRead - p)).ToString());
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(saveName, System.Text.Encoding.UTF8));

                iStream.Position = p;
                dataToRead = dataToRead - p;
                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (System.Web.HttpContext.Current.Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, 10240);

                        // Write the data to the current output stream.
                        System.Web.HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        System.Web.HttpContext.Current.Response.Flush();

                        buffer = new Byte[10240];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                System.Web.HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }

                System.Web.HttpContext.Current.Response.End();
            }
        }
        #endregion

        #region ��ȡָ��ҳ�������
        /// <summary>
        /// ��ָ����URL����ҳ������(ʹ��WebRequest)
        /// </summary>
        /// <param name="url">ָ��URL</param>
        /// <returns></returns>
        public static string LoadURLString(string url)
        {
            try
            {
                HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                Stream stream = myWebResponse.GetResponseStream();

                string strResult = "";
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                int i = 0;
                while (count > 0)
                {
                    i += Encoding.UTF8.GetByteCount(read, 0, 256);
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }

                return strResult;
            }
            catch {
                return null;
            }
        }


        /// <summary>
        /// ��ָ����URL����ҳ������(ʹ��WebClient)
        /// </summary>
        /// <param name="url">ָ��URL</param>
        /// <returns></returns>
        public static string LoadPageContent(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            byte[] pageData = wc.DownloadData(url);
            return (Encoding.GetEncoding("utf-8").GetString(pageData));
        }
        #endregion

        #region Զ�̷����������ļ�
        /// <summary>
        /// Զ����ȡ�ļ���������������(ʹ��WebRequest)
        /// </summary>
        /// <param name="url">һ��URI�ϵ��ļ�</param>
        /// <param name="saveurl">�ļ������ڷ������ϵ�ȫ·��</param>
        public static void RemoteGetFile(string url, string saveurl)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream stream = myWebResponse.GetResponseStream();

            //���������ļ���С
            int fileSize = (int)myWebResponse.ContentLength;

            int bufferSize = 102400;
            byte[] buffer = new byte[bufferSize];
            FileOperate.WriteFile(saveurl, "temp");
            // ����һ��д���ļ���������
            FileStream saveFile = File.Create(saveurl, bufferSize);
            int bytesRead;
            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                saveFile.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            saveFile.Flush();
            saveFile.Close();
            stream.Flush();
            stream.Close();
        }

        /// <summary>
        /// Զ����ȡһ���ļ���������������(ʹ��WebClient)
        /// </summary>
        /// <param name="url">һ��URI�ϵ��ļ�</param>
        /// <param name="saveurl">�����ļ�</param>
        public static void WebClientGetFile(string url, string saveurl)
        {
            WebClient wc = new WebClient();

            try
            {
                FileOperate.WriteFile(saveurl, "temp");
                wc.DownloadFile(url, saveurl);
            }
            catch
            { }

            wc.Dispose();
        }

        /// <summary>
        /// Զ����ȡһ���ļ���������������(ʹ��WebClient)
        /// </summary>
        /// <param name="urls">һ��URI�ϵ��ļ�</param>
        /// <param name="saveurls">�����ļ�</param>
        public static void WebClientGetFile(string[] urls, string[] saveurls)
        {
            WebClient wc = new WebClient();
            for (int i = 0; i < urls.Length; i++)
            {
                try
                {
                    wc.DownloadFile(urls[i], saveurls[i]);
                }
                catch
                { }
            }
            wc.Dispose();
        }
        #endregion

        #region �ļ��ϴ�
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="upfile">��ȡ�ͻ����ϴ����ļ�</param>
        /// <param name="limitType">�����ϴ����ļ����ͣ�nullֵΪ������</param>
        /// <param name="limitSize">�ϴ��ļ��Ĵ�С����(0Ϊ������)</param>
        /// <param name="autoName">�Ƿ��Զ�����</param>
        /// <param name="savepath">�ϴ��ļ��ı���·��</param>
        /// <returns>string[]</returns>
        public static string[] UploadFile(HttpPostedFile upfile, string limitType, int limitSize, bool autoName, string savepath)
        {
            string[] strResult = new string[5];
            string[] extName = null;
            if (!Object.Equals(limitType, null) || Object.Equals(limitType, ""))
            {
                extName = FunctionUtils.SplitArray(limitType, ',');
            }

            int fileSize = upfile.ContentLength;								// �ϴ��ļ���С
            string fileClientName = upfile.FileName;							// �ڿͻ��˵��ļ�ȫ·��
            string fileFullName = Path.GetFileName(fileClientName);				// �ϴ��ļ�����������׺����
            if (fileFullName == null || fileFullName == "")
            {
                strResult[0] = "���ļ�";
                strResult[1] = "";
                strResult[2] = "";
                strResult[3] = "";
                strResult[4] = "<font color=red>ʧ��</font>";
                return strResult;
            }
            else
            {
                string fileType = upfile.ContentType;								// �ϴ��ļ���MIME����
                string[] array = FunctionUtils.SplitArray(fileFullName, '.');
                string fileExt = array[array.Length - 1];							// �ϴ��ļ���׺��
                int fileNameLength = fileFullName.Length - fileExt.Length - 1;
                string fileName = "";												// �ϴ��ļ�������������׺����
                if (autoName)
                {
                    fileName = "_" + FunctionUtils.Text.MakeName();
                }
                else
                {
                    fileName = fileFullName.Substring(0, fileNameLength);
                }


                string savename = fileName + "." + fileExt.ToLower();
                strResult[0] = fileClientName;
                strResult[1] = savename;
                strResult[2] = fileType;
                strResult[3] = fileSize.ToString();
                bool EnableUpload = false;
                if (limitSize <= fileSize && limitSize != 0)
                {
                    strResult[4] = "<font color=red>ʧ��</font>���ϴ��ļ�����";
                    EnableUpload = false;
                }
                else if (extName != null)
                {
                    for (int i = 0; i <= extName.Length - 1; i++)
                    {
                        if (string.Compare(fileExt, extName[i].ToString(), true) == 0)
                        {
                            EnableUpload = true;
                            break;
                        }
                        else
                        {
                            strResult[4] = "<font color=red>ʧ��</font>���ļ����Ͳ������ϴ�";
                            EnableUpload = false;
                        }
                    }
                }
                else
                {
                    EnableUpload = true;
                }

                // �����ϴ���������ʼִ���ϴ��ļ�������
                if (EnableUpload)
                {
                    try
                    {
                        string savefile = savepath + savename;
                        FileOperate.WriteFile(savefile, "��ʱ�ļ�");
                        upfile.SaveAs(savefile);
                        strResult[4] = "�ɹ�";
                        //strResult[4] = "�ɹ�<!--" + FunctionUtils.GetRealPath(savepath) + savename + "-->";
                    }
                    catch (Exception exc)
                    {
                        strResult[4] = "<font color=red>ʧ��</font><!-- " + exc.Message + " -->";
                    }
                }

                return strResult;
            }
        }
        #endregion
    }
}
