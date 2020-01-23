using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using DY.OAuthSDK.Login.Model;
using DY.OAuthSDK.Connect.Model;
using DY.OAuthSDK.Connect.Util;
using DY.OAuthSDK.Login;
using DY.OAuthSDK.Helper;



namespace DY.Web
{
    public partial class Callback : System.Web.UI.Page
    {
        private delegate UserInfo myDelegateUserInfo(AccessToken tokenInfo);

        private static String regexCode = "^[0-9a-zA-Z]{32}$";
        protected void Page_Load(object sender, EventArgs e)
        {
            String state = Request.QueryString["state"];
            String code = Request.QueryString["code"];
            if (!String.IsNullOrEmpty(state) && Regex.IsMatch(state, regexCode))
            {
                #region 处理站点
                object oSite = Session["siteType"];
                if (oSite == null)
                    return;
                byte b = byte.Parse(oSite.ToString());
                Session.Remove("siteType");
                #endregion
                object guid = Session["ConnectGuid"];
                if (guid != null)
                {
                    if (guid.ToString().CompareTo(state) == 0)
                    {
                        Session.Remove("ConnectGuid");
                        if (Regex.IsMatch(code, regexCode))
                        {
                            QQSite site = b == 1 ? QQSite.Weibo : QQSite.Tentent;
                            AccessToken tokenInfo =CreateConnect.CreateConnectAccessToken(site, code);
                            if (tokenInfo != null)
                            {
                                UserInfo info = site == QQSite.Weibo ? getDelegate(tokenInfo, getWeiBo) :
                                    getDelegate(tokenInfo, getTencent);
                                if (info != null)
                                {
                                    Session["userInfo"] = info;
                                    Response.Redirect("/liuhongbing/login.aspx", true);
                                }
                            }
                        }
                        else
                        {
                            // Response.Redirect("/Index.html", true);
                        }
                    }
                }
            }
        }

        private UserInfo getDelegate(AccessToken token, myDelegateUserInfo myDelegate)
        {
            return myDelegate(token);
        }

        private UserInfo getWeiBo(AccessToken tokenInfo)
        {
            UserInfo info = null;
            WeiboParaeter[] parameter = new WeiboParaeter[]{
                new WeiboParaeter{ Name="uid",Value=tokenInfo.uid},
                new WeiboParaeter{ Name="source", Value = CreateConnect.GetAppk},
                new WeiboParaeter{ Name = "access_token", Value = tokenInfo.access_token}
            };
            String result = CreateConnect.
                GetCommand("https://api.weibo.com/2/users/show.json", parameter);
            if (!String.IsNullOrEmpty(result))
            {
                info = JsonHelper.GetJosnModel<UserInfo>(result);
                if (info != null)
                {
                    info.gender = info.gender == "m" ? "男" : "女";
                    info.uType = 1;
                }
            }
            return info;
        }

        private UserInfo getTencent(AccessToken tokenInfo)
        {
            UserInfo info = null;
            WeiboParaeter[] parameter = new WeiboParaeter[]{
                    new WeiboParaeter{ Name="oauth_consumer_key",Value= CreateConnect.GetAppk},
                    new WeiboParaeter{ Name="access_token", Value = tokenInfo.access_token},
                    new WeiboParaeter{ Name="openid", Value = tokenInfo.openid}
              };
            String result = CreateConnect.GetCommand("https://graph.qq.com/user/get_user_info", parameter);
            if (!String.IsNullOrEmpty(result))
            {
                TentenctInfo tentectInfo =
                   JsonHelper.GetJosnModel<TentenctInfo>(result);
                if (tentectInfo != null)
                {
                    info = new UserInfo
                    {
                        gender = tentectInfo.gender,
                        id = tokenInfo.openid,
                        screen_name = tentectInfo.nickname,
                        location = String.Empty,
                        profile_image_url = tentectInfo.figureurl_qq_1,
                        uType = 2
                    };
                }
            }
            return info;
        }

    }
}