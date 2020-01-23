using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DY.Common;
using DY.OAuthV2SDK.OAuths;

namespace DY.Web.PayReturn
{
    public partial class oauth_login : DY.Site.WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var oauth_name = DYRequest.getRequest("oauth");
            int home = DYRequest.getRequestInt("home");
            if (home == 1)
                Session["home"] = home;
            if (!string.IsNullOrEmpty(oauth_name))
            {
                var oauth = OAuthBase.CreateInstance(oauth_name);
                if (oauth != null)
                {
                    oauth.UpdateCache(DateTime.Now.AddMonths(1), siteUtils.GetDomain(), "/"); //缓存当前协议 (建议使用301重定向到同一域名，不然会导致域名不一致找不到缓存错误)
                    var oauth_url = oauth.GetAuthorize(); //获取用户认证地址
                    Response.Redirect(oauth_url);
                }
                else
                {
                    Response.Write("登录失败，找不到相对应的接口");
                }
            }
            else
            {
                Response.Write("登录失败，找不到相对应的接口");
            }
        }
    }
}