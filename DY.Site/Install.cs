using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Data;
using DY.Entity;
using System.Collections;

namespace DY.Site
{
    /// <summary>
    /// 安装程序检测相关类
    /// </summary>
    public class Install : System.Web.UI.Page
    {
        /// <summary>
        /// Install类构造函数
        /// </summary>
        public Install()
        {
            #region 判断安装目录文件信息

            if (System.IO.Directory.Exists(Server.MapPath("/install/")))
            {
                if (System.IO.File.Exists(Server.MapPath("/install/lock.lock")))
                {
                    if (SiteUtils.IsExistsSetupFile())
                    {
                        string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                        message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>请将您的安装目录即install/目录下的文件全部删除, 以免其它用户运行安装该程序!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                        message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head>><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                        message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                        message += "请将您的安装目录(install/)下的.aspx文件及bin/DY.Install.dll全部删除, 以免其它用户运行安装或升级程序!</div></div></body></html>";
                        Context.Response.Write(message);
                        Context.Response.End();
                        return;
                    }
                }
                else
                    Context.Response.Redirect("/install/index.aspx");
            }
            #endregion
        }
    }
}
