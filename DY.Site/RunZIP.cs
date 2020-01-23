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
        /// 检测是否存在新版本
        /// </summary>
        /// <returns></returns>
        public static bool CheckedVer()
        {
            bool flag = false;
            #region 检测版本
            string verPath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Update_client);
            if (File.Exists(verPath))
            {
                //服务器地址
                string strServerAddress = RunZIP.ReadConfig(verPath, "ServerAddress");
                //服务器文件
                string strServerConfig = strServerAddress +
                        RunZIP.ReadConfig(verPath, "ServerConfigFile");
                //本地版本
                string nowVer = RunZIP.ReadConfig(verPath, "LocalVersion");
                //读取服务器配置文件制定的更新文件压缩包
                string strUrl = strServerAddress + RunZIP.ReadConfig(strServerConfig, "TargetFile");
                //读取服务器配置文件中记录的版本号
                string updVer = RunZIP.ReadConfig(strServerConfig, "RemoteVersion");

                Version v1 = new Version(nowVer);
                Version v2 = new Version(updVer);
                //版本号比较，判断是否需要更新
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
        /// 开始升级操作
        /// </summary>
        /// <param name="strUrl">升级包完整路径</param>
        /// <param name="update_client">本地xml路径</param>
        /// <param name="updVer">服务器版本号</param>
        public static void Uptade(string strUrl, string updVer)
        {
            try
            {
                string directorypath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Upd_forder);
                string lockFile = directorypath + "up.lock";
                if (!File.Exists(lockFile))
                {
                    #region 设置临时文件夹
                    if (System.IO.Directory.Exists(directorypath) == false)
                    {
                        System.IO.Directory.CreateDirectory(directorypath);
                    }
                    #endregion


                    #region 下载
                    //this.Visible = true;
                    //lbl_Updata.Text = "升级文件...";
                    //lbl_Version.Visible = true;
                    //label1.Visible = true;
                    //lbl_Version.Text = "从旧版本(" + nowVer + ")升级"
                    //                 + "到新版本(" + updVer + ")";

                    RunZIP.DownFile(strUrl, directorypath + "temp.zip");
                    #endregion

                    #region 解压缩
                    string pa = directorypath + "temp.zip";
                    RunZIP.UnZip(pa, System.Web.HttpContext.Current.Server.MapPath("/"));
                    //fzip.decompress(pa, Server.MapPath("/"));

                    RunZIP.UpdateConfig(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Update_client), "LocalVersion", updVer);
                    #endregion
                    File.WriteAllText(lockFile, "升级时间：" + DateTime.Now);
                    System.Web.HttpContext.Current.Response.Write("升级完成");
                }
                else
                    System.Web.HttpContext.Current.Response.Write("请删除/log/update目录下的up.lock文件，然后进行升级");
                
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.Write(e.Message);
            }
        }
        /// <summary>
        /// 压缩
        /// </summary> 
        /// <param name="filename"> 压缩后的文件名(包含物理路径)</param>
        /// <param name="directory">待压缩的文件夹(包含物理路径)</param>
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
        /// 下载文件
        /// </summary>
        /// <param name="URL">服务器url</param>
        /// <param name="Filename">文件名</param>
        public static void DownFile(string URL, string Filename)
        {
            #region 下载文件
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
        /// 修改xml文件
        /// </summary>
        /// <param name="file">XML文件地址</param>
        /// <param name="key">节点名</param>
        /// <param name="Xvalue">值</param>
        public static void UpdateConfig(string file, string key, string Xvalue)
        {
            #region 修改xml文件

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode node = doc.SelectSingleNode("/Update/" + key);
            node.InnerText = Xvalue;
            doc.Save(file);
            #endregion
        }

        /// <summary>
        /// 读取xml文件
        /// </summary>
        /// <param name="file">XML文件地址</param>
        /// <param name="key">节点名</param>
        /// <returns></returns>
        public static string ReadConfig(string file, string key)
        {
            #region 读取xml文件
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
        /// 解压
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">解压目标存放目录</param>
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
