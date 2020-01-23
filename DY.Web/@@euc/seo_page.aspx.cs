using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class seo_page : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.act == "cnzz")
            {
                //检测权限
                this.IsChecked("seo_page_cnzz");
                string cnzz=SiteUtils.ReadFileToCnzz();
                //跳转并登陆cnzz
                if (cnzz.Contains("@"))
                {
                    string siteid = SiteUtils.ReadFileToCnzz().Split('@')[0];
                    string pwd = SiteUtils.ReadFileToCnzz().Split('@')[1];
                    string url = "http://wss.cnzz.com/user/companion/ctmon_login.php?site_id=" + siteid + "&password=" + pwd + "&cms=" + DY.Config.BaseConfig.OemCms;
                    Response.Redirect(url);
                }
                else
                {
                    //显示提示信息
                    //base.DisplayMessage(RetrunCnzzCode(cnzz), 1, "?act=list");
                    IDictionary context = new Hashtable();
                    context.Add("code", RetrunCnzzCode(cnzz));
                    base.DisplayTemplate(context, "seo/cnzz_page");
                }
            }
            else if (this.act == "loadcnzz")
            {
                string message = "";
                int error = 0;
                if (SiteUtils.WriteFileToCnzz())
                {
                    message = "分配成功！";
                }
                else
                {
                    error = 1;
                    message = "分配失败！";
                }
                base.DisplayMemoryTemplate(base.MakeJson("", error, message));
            }
            else if (this.act == "checkcnzz")
            {
                string cnzz = SiteUtils.ReadFileToCnzz();
                string code = RetrunCnzzCode(cnzz);
                string message = "";
                if (cnzz.Contains("@"))
                    message = "账号正确！";
                else
                    message = "<span class=\"label label-danger\">"+code +"</span>请确认域名是否正确或更换时间段获取！";
                base.DisplayMemoryTemplate(base.MakeJson("", 0, message));
            }
            else if (this.act == "zzc")
            {
                //检测权限
                this.IsChecked("zzc");
                IDictionary context = new Hashtable();
                base.DisplayTemplate(context, "seo/zzc");
            }
            else
            {
                //检测权限
                this.IsChecked("seo_page");
                IDictionary context = new Hashtable();
                //context.Add("pages", "index");
                base.DisplayTemplate(context, "seo/seo_page");
            }
        }

        protected string RetrunCnzzCode(string cnzz)
        {
            string str = string.Empty;
            switch (cnzz)
            {
                case "-1": str = "key输入有误"; break;
                case "-2": str = "域名长度有误"; break;
                case "-3": str = "域名输入有误"; break;
                case "-4": str = "域名插入数据库有误"; break;
                case "-5": str = "同一个IP用户调用页面超过阀值"; break;
            }
            return str;
        }
    }
}
