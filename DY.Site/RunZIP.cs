using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Net;
using System.IO;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using DY.Config;

namespace RUNWINZIP
{
    public class RunZIP : System.Web.UI.Page
    {

        /// <summary>
        /// ����Ƿ�����°汾
        /// </summary>
        /// <returns></returns>
        public static bool CheckedVer()
        {
            bool flag = false;
            #region ���汾
            string verPath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Update_client);
            if (File.Exists(verPath))
            {
                //��������ַ
                string strServerAddress = RunZIP.ReadConfig(verPath, "ServerAddress");
                //�������ļ�
                string strServerConfig = strServerAddress +
                        RunZIP.ReadConfig(verPath, "ServerConfigFile");
                //���ذ汾
                string nowVer = RunZIP.ReadConfig(verPath, "LocalVersion");
                //��ȡ�����������ļ��ƶ��ĸ����ļ�ѹ����
                string strUrl = strServerAddress + RunZIP.ReadConfig(strServerConfig, "TargetFile");
                //��ȡ�����������ļ��м�¼�İ汾��
                string updVer = RunZIP.ReadConfig(strServerConfig, "RemoteVersion");

                Version v1 = new Version(nowVer);
                Version v2 = new Version(updVer);
                //�汾�űȽϣ��ж��Ƿ���Ҫ����
                if (v1.CompareTo(v2) < 0)
                {
                    flag = true;
                    //RunZIP.Uptade(strUrl, updVer);
                }
            }
            return flag;
            #endregion
        }
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="strUrl">����������·��</param>
        /// <param name="update_client">����xml·��</param>
        /// <param name="updVer">�������汾��</param>
        public static void Uptade(string strUrl, string updVer)
        {
            try
            {
                string directorypath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Upd_forder);
                string lockFile = directorypath + "up.lock";
                if (!File.Exists(lockFile))
                {
                    #region ������ʱ�ļ���
                    if (System.IO.Directory.Exists(directorypath) == false)
                    {
                        System.IO.Directory.CreateDirectory(directorypath);
                    }
                    #endregion


                    #region ����
                    //this.Visible = true;
                    //lbl_Updata.Text = "�����ļ�...";
                    //lbl_Version.Visible = true;
                    //label1.Visible = true;
                    //lbl_Version.Text = "�Ӿɰ汾(" + nowVer + ")����"
                    //                 + "���°汾(" + updVer + ")";

                    RunZIP.DownFile(strUrl, directorypath + "temp.zip");
                    #endregion

                    #region ��ѹ��
                    string pa = directorypath + "temp.zip";
                    RunZIP.UnZip(pa, System.Web.HttpContext.Current.Server.MapPath("/"));
                    //fzip.decompress(pa, Server.MapPath("/"));

                    RunZIP.UpdateConfig(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Update_client), "LocalVersion", updVer);
                    #endregion
                    File.WriteAllText(lockFile, "����ʱ�䣺" + DateTime.Now);
                    System.Web.HttpContext.Current.Response.Write("�������");
                }
                else
                    System.Web.HttpContext.Current.Response.Write("��ɾ��/log/updateĿ¼�µ�up.lock�ļ���Ȼ���������");
                
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write(e.Message);
            }
        }
        /// <summary>
        /// ѹ��
        /// </summary> 
        /// <param name="filename"> ѹ������ļ���(��������·��)</param>
        /// <param name="directory">��ѹ�����ļ���(��������·��)</param>
        public static void PackFiles(string filename, string directory)
        {
            try
            {
                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                fz.CreateZip(filename, directory, true, "");
                fz = null;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="URL">������url</param>
        /// <param name="Filename">�ļ���</param>
        public static void DownFile(string URL, string Filename)
        {
            #region �����ļ�
            HttpWebRequest Myrq = (HttpWebRequest)HttpWebRequest.Create(URL);
            HttpWebResponse myrp = (HttpWebResponse)Myrq.GetResponse();

            Stream st = myrp.GetResponseStream();
            Stream so = new FileStream(Filename, FileMode.Create);
            byte[] by = new byte[512];
            int osize = st.Read(by, 0, (int)by.Length);
            while (osize > 0)
            {
                so.Write(by, 0, osize);
                osize = st.Read(by, 0, (int)by.Length);
            }
            so.Close();
            st.Close();
            #endregion
        }

        /// <summary>
        /// �޸�xml�ļ�
        /// </summary>
        /// <param name="file">XML�ļ���ַ</param>
        /// <param name="key">�ڵ���</param>
        /// <param name="Xvalue">ֵ</param>
        public static void UpdateConfig(string file, string key, string Xvalue)
        {
            #region �޸�xml�ļ�

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode node = doc.SelectSingleNode("/Update/" + key);
            node.InnerText = Xvalue;
            doc.Save(file);
            #endregion
        }

        /// <summary>
        /// ��ȡxml�ļ�
        /// </summary>
        /// <param name="file">XML�ļ���ַ</param>
        /// <param name="key">�ڵ���</param>
        /// <returns></returns>
        public static string ReadConfig(string file, string key)
        {
            #region ��ȡxml�ļ�
            try
            {
                string path=file;
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                        
                XmlNode node = doc.SelectSingleNode("/Update/" + key);

                return node.InnerText;
            }
            catch (WebException ex) 
            {
                throw ex;
            }
            #endregion
        }

                /// <summary>
        /// ��ѹ
        /// </summary>
        /// <param name="FileToUpZip">����ѹ���ļ�</param>
        /// <param name="ZipedFolder">��ѹĿ����Ŀ¼</param>
        public static void UnZip(string FileToUpZip, string ZipedFolder)
        {
            if (!File.Exists(FileToUpZip))
            {
                return;
            }
            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }
            ZipInputStream s = null;
            ZipEntry theEntry = null;
            string fileName;
            FileStream streamWriter = null;
            try
            {
                s = new ZipInputStream(File.OpenRead(FileToUpZip));
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }
                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }
    }
}
