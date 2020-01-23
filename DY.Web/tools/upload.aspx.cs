using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DY.Common;

namespace DY.Web.tools
{
    public partial class upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DY.Entity.OnlineUserInfo oluserinfo = DY.Site.OnlineAdminUsers.UpdateInfo(DY.Config.BaseConfig.WebEncrypt);
            //if (oluserinfo.Userid <= 0)
            //{
            //    Context.Response.Write("错误提示：请先登录！");
            //    return;
            //}

            string savePath = "/include/upload/images/";   //图片保存路径
            string savefolder = DYRequest.getRequest("save_folder");
            if (savefolder != "")
            {
                savePath += savefolder+"/";
            }
            string limitType = "jpg,gif,png,bmp"; //可上传的文件类型
            int limitSize = 80000;  //可上传文件大小(kb)

            if (DYRequest.getRequest("t") == "file")
            {
                limitType = "zip,rar,pdf,xls";
                limitSize = 100000;
                savePath = "/include/upload/download/";
            }

            string[] results = CommonUtils.UploadFile(FileUpload1.PostedFile, limitType, limitSize * 1024, true, Server.MapPath(savePath));

            string filePath = savePath + results[1];

            if (results[4] == "成功")
            {
                //返回上传后的文件路径
                Response.Write("<script>if (window.parent.document.getElementById('" + DYRequest.getRequest("target_obj") + "')){window.parent.document.getElementById('" + DYRequest.getRequest("target_obj") + "').value='" + filePath + "';window.parent.document.getElementById('" + DYRequest.getRequest("imgobj") + "').src='" + filePath + "';window.parent.pop_close('pop_upload');}else{window.parent.document.all." + DYRequest.getRequest("target_obj") + ".value='" + filePath + "';window.parent.pop_close('pop_upload');}</script>");
            }
            else
            {
                Response.Write(results[4]+"<a href=\"javascript:history.back();\">返回</a>");
                Response.End();
            }
        }
    }
}
