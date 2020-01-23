using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Web.UI;
using System.Data;
using DY.Config;
using DY.Common;
using System.Collections;

namespace DY.Site
{
    public class Plugin
    {
        private static string msg = "";
        /// <summary>
        /// 安装插件
        /// </summary>
        /// <param name="strPluginName">插件的目录名</param>
        /// <param name="page">当前页</param>
        public static void InstallPlugin(string strPluginName,Page page)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                //获取插件配置文件
                doc.Load(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath+strPluginName + "/config.xml"));
            }
            catch (Exception e)
            {
                throw new PluginInstallException("插件载入错误：" + e.ToString());
            }

            //获取插件名称
            string name = doc.DocumentElement.SelectSingleNode("main/name").InnerText.ToLower();
            //获取插件版本
            string version = doc.DocumentElement.SelectSingleNode("main/version").InnerText.ToLower();
            //获取程序集
            string dll = doc.DocumentElement.SelectSingleNode("main/dll").InnerText.ToLower();
            //是否更新插件
            bool isUpdate = false;

            //判断插件是否已安装
            if (PluginIsInstalled(strPluginName))
            {
                //检查是否可以升级
                XmlDocument docInstalled = new XmlDocument();
                docInstalled.Load(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + "installedplugins.xml"));
                XmlElement xeInstalled = (XmlElement)docInstalled.DocumentElement.SelectSingleNode("plugin[@name='" + strPluginName + "']");
                isUpdate = xeInstalled.GetAttribute("version") == version;
                if (isUpdate)
                {
                    //如果版本号相同则停止安装
                    throw new PluginInstallException("该插件暂无更新");
                }
            }

            //生成程序集
            string newAsmName = string.IsNullOrEmpty(dll) ? strPluginName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".dll" : dll;
            //生成dll文件名
            string dllPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + strPluginName + "/bin/"), newAsmName);
            try
            {
                XmlNode xn = doc.DocumentElement.SelectSingleNode("csharpcode");
                string code = string.Empty;
                if (xn != null)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.HasAttribute("link"))
                    {
                        using (
                            StreamReader sr =
                                System.IO.File.OpenText(
                                    Path.Combine(
                                        System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + strPluginName + "/"), xe.GetAttribute("link")
                                    )
                                )
                         )
                        {
                            code = sr.ReadToEnd();
                            sr.Close();
                        }
                    }
                    else
                    {
                        code = xe.InnerText;
                    }
                }
                //如果没有定义cs，则不产生dll
                if (code.Trim().Length > 0)
                {
                    string dir = Path.GetDirectoryName(dllPath);
                    foreach (string file in Directory.GetFiles(dir, strPluginName + "*.dll"))
                    {
                        //将原dll重命名,一般是升级插件时用
                        File.Move(Path.Combine(dir, file), Path.Combine(dir, file + ".temp"));
                    }

                    //生成dll
                    CodeSnippetCompileUnit unit = new CodeSnippetCompileUnit(code);
                    CSharpCodeProvider compiler = new CSharpCodeProvider();
                    CompilerParameters para = new CompilerParameters();
                    para.ReferencedAssemblies.Add("System.dll");
                    para.ReferencedAssemblies.Add("System.Data.dll");
                    para.ReferencedAssemblies.Add("System.XML.dll");
                    para.ReferencedAssemblies.Add("System.Drawing.dll");
                    para.ReferencedAssemblies.Add("System.Web.dll");
                    //加载项目所有程序集
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Common.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Entity.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Config.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Cache.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Weixin.MP.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.OAuthSDK.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Data.SqlServer.dll"));
                    para.ReferencedAssemblies.Add(System.Web.HttpContext.Current.Server.MapPath(page.Request.ApplicationPath + "/bin/DY.Site.dll"));
                    para.GenerateExecutable = false;
                    para.OutputAssembly = dllPath;

                    //编译结果
                    CompilerResults cr = compiler.CompileAssemblyFromDom(para, unit);
                    if (cr.Errors.Count > 0)
                    {
                        msg+="插件安装失败，编译错误：<br/>";
                        //遍历编译错误
                        foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                        {
                            msg+=ce.ErrorText + "（文件：" + ce.FileName + "，行号：" + ce.Line.ToString() + "，错误编号：" + ce.ErrorNumber + "）<br/>";
                        }
                        throw new PluginInstallException(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new PluginInstallException("插件安装失败：" + ex.ToString());
            }

            //如果定义了dll，则载入插件程序集并调用其安装或升级方法，此dllPath必为新安装的
            if (File.Exists(dllPath))
            {
                //加载程序集
                System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(dllPath);
                //激活实例
                IPlugin plugin = (IPlugin)asm.CreateInstance("DY.Site.plugins." + strPluginName);
                try
                {
                    if (isUpdate)
                        plugin.Update(page);                   //执行更新代码
                    else
                        plugin.Install(page);                     //执行安装代码
                }
                //捕获安装异常，由安装程序抛出
                catch (PluginInstallException e)
                {
                    throw new PluginInstallException("安装失败：" + e.ToString());
                }
            }

            //将插件记录在已安装插件记录中
            string filePath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath+"installedplugins.xml");
            XmlDocument docSaveInstalled = new XmlDocument();
            if (File.Exists(filePath))
            {
                docSaveInstalled.Load(filePath);
                XmlNodeList xnl = docSaveInstalled.DocumentElement.SelectNodes("plugin[@name='" + strPluginName + "']");
                foreach (XmlNode xn in xnl) docSaveInstalled.DocumentElement.RemoveChild(xn);
            }
            else
            {
                XmlElement root = docSaveInstalled.CreateElement("installedplugins");
                docSaveInstalled.AppendChild(root);
                docSaveInstalled.InsertBefore(docSaveInstalled.CreateXmlDeclaration("1.0", "utf-8", null), root);
            }

            //创建新的节点
            XmlElement xeNewPlugin = docSaveInstalled.CreateElement("plugin");
            XmlAttribute xa;
            xa = docSaveInstalled.CreateAttribute("name");
            xa.Value = strPluginName;
            xeNewPlugin.Attributes.Append(xa);
            xa = docSaveInstalled.CreateAttribute("version");
            xa.Value = version;
            xeNewPlugin.Attributes.Append(xa);
            xa = docSaveInstalled.CreateAttribute("assembly");
            xa.Value = newAsmName;
            xeNewPlugin.Attributes.Append(xa);
            docSaveInstalled.DocumentElement.AppendChild(xeNewPlugin);

            docSaveInstalled.Save(filePath);
        }

        /// <summary>
        /// 检查插件是否已经安装
        /// </summary>
        /// <param name="strPluginName">插件名称</param>
        /// <returns></returns>
        public static bool PluginIsInstalled(string strPluginName)
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + "installedplugins.xml");
            if (!File.Exists(filePath)) return false;
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            return doc.DocumentElement.SelectSingleNode("plugin[@name='" + strPluginName + "']") != null;
        }

        /// <summary>
        /// 获取插件的输出内容
        /// </summary>
        /// <param name="strPluginName">插件名称</param>
        /// <param name="page">页面名称</param>
        /// <returns>输出内容</returns>
        public static void GetPluginOutPut(string strPluginName,Page page)
        {
            try
            {
                //载入插件程序集
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + "installedplugins.xml"));

                string asmName = ((System.Xml.XmlElement)doc.DocumentElement.SelectSingleNode("plugin[@name='" + strPluginName + "']")).GetAttribute("assembly");

                System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath +strPluginName+"/bin/" + asmName));
                IPlugin plugin = (IPlugin)asm.CreateInstance("DY.Site.plugins." + strPluginName);

                plugin.GetOutput(BaseConfig.PlunginPath+strPluginName, page);
            }
            catch (Exception ex)
            {
                throw new PluginInstallException("插件‘" + strPluginName + "’载入失败：" + ex.ToString());
            }
        }

        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="strPluginName">插件名称</param>
        /// <param name="page"></param>
        public static void UnInstallPlugin(string strPluginName,Page page)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                //获取插件配置文件
                doc.Load(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + strPluginName + "/config.xml"));
            }
            catch (Exception ex)
            {
                throw new PluginInstallException("插件载入错误：" + ex.ToString());
            }

            //从已安装插件文件中删除记录
            string filePath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath+"installedplugins.xml");
            XmlDocument docDeleteNode = new XmlDocument();
            docDeleteNode.Load(filePath);
            XmlElement xeOld = (XmlElement)docDeleteNode.DocumentElement.SelectSingleNode("plugin[@name='" + strPluginName + "']");
            docDeleteNode.DocumentElement.RemoveChild(xeOld);
            docDeleteNode.Save(filePath);

            string dllPath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath+strPluginName+"bin/" + xeOld.GetAttribute("assembly"));
            //如果程序集存在，则载入插件程序集并调用其卸载方法
            if (File.Exists(dllPath))
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFile(dllPath);
                IPlugin plugin = (IPlugin)asm.CreateInstance("DY.Site.plugins." + strPluginName);

                plugin.Uninstall(page);
            }

            //删除dll文件
            if (File.Exists(dllPath))
            {
                //标记旧的dll为temp
                File.Move(dllPath, dllPath + ".temp");
            }
            //删除temp文件
            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(dllPath), "*.temp"))
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
        }

        /// <summary>
        /// 升级插件
        /// </summary>
        /// <param name="strPluginName">插件名称</param>
        /// <param name="page"></param>
        public static void UpdatePlugin(string strPluginName,Page page)
        {
            InstallPlugin(strPluginName, page);
        }

        /// <summary>
        /// 获取插件列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList PluginList()
        {
            ArrayList list = new ArrayList();
            foreach (DataRow dir in FileOperate.getDirectoryInfos(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath), FileOperate.FsoMethod.Folder).Rows)
            {
                if (dir["name"].ToString() != "bin")
                {
                    PluginInfo pluginInfo = new PluginInfo();
                    pluginInfo.filename = dir["name"].ToString();
                    //获取插件详细信息
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        string configxml = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.PlunginPath + dir["name"].ToString() + "/config.xml");
                        if (File.Exists(configxml))
                        {
                            //获取插件配置文件
                            doc.Load(configxml);

                            //获取插件名称
                            pluginInfo.name = doc.DocumentElement.SelectSingleNode("main/name").InnerText.ToLower();
                            //获取插件版本
                            pluginInfo.version = doc.DocumentElement.SelectSingleNode("main/version").InnerText.ToLower();
                            pluginInfo.from = doc.DocumentElement.SelectSingleNode("main/from").InnerText.ToLower();
                            pluginInfo.author = doc.DocumentElement.SelectSingleNode("main/author").InnerText.ToLower();
                            pluginInfo.description = doc.DocumentElement.SelectSingleNode("main/description").InnerText.ToLower();
                            pluginInfo.ico = doc.DocumentElement.SelectSingleNode("main/ico").InnerText.ToLower();

                            list.Add(pluginInfo);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new PluginInstallException("插件配置文件载入错误");
                    }
                }
            }
            return list;
        }
    }
    public class PluginInfo
    {
        public string filename { get; set; }
        public string ico { get; set; }
        public string dll { get; set; }
        public string version { get; set; }
        public string name { get; set; }
        public string from { get; set; }
        public string author { get; set; }
        public string description { get; set; }
    }
}
