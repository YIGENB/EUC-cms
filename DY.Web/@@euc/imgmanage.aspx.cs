using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.Config;

namespace DY.Web.admin
{
    public partial class imgmanage : AdminPage
    {
        public string directoryPath = "/include/upload/kind/image/";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DYRequest.getRequest("folder") != "")
                directoryPath = "/include/upload/kind/image/" + DYRequest.getRequest("folder")+"/";
            IDictionary context = new Hashtable();
            context.Add("type", DYRequest.getRequest("type"));
            context.Add("backid", DYRequest.getRequest("backid"));
            context.Add("folderlist", FileOperate.getDirectoryAllInfos(Server.MapPath("/include/upload/kind/image/"), FileOperate.FsoMethod.Folder, "*.*").Rows);
            context.Add("list", FileOperate.getDirectoryAllInfos(Server.MapPath(directoryPath), FileOperate.FsoMethod.File, "*.*").Rows);
            context.Add("imgid", DYRequest.getRequest("imgid"));
            base.DisplayTemplate(context, "systems/imgmanage");
        }
    }
}
