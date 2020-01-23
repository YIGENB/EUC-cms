using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using System.Data;
using RUNWINZIP;
using DY.Config;

namespace DY.Web.tools
{
    public partial class up : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string verPath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.Update_client);
            //服务器地址
            string strServerAddress = RunZIP.ReadConfig(verPath, "ServerAddress");
            //服务器文件
            string strServerConfig = strServerAddress +
                    RunZIP.ReadConfig(verPath, "ServerConfigFile");
            //本地版本
            string nowVer = RunZIP.ReadConfig(verPath, "LocalVersion");
            //读取服务器配置文件中记录的版本号
            string updVer = RunZIP.ReadConfig(strServerConfig, "RemoteVersion");
            //读取服务器配置文件制定的更新文件压缩包
            string strUrl = strServerAddress + RunZIP.ReadConfig(strServerConfig, "TargetFile");
            RunZIP.Uptade(strUrl, updVer);
        }
    }
}
