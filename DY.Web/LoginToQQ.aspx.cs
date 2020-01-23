using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DY.OAuthSDK.Login;
using DY.OAuthSDK.Connect.Util;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP;


namespace DY.Web
{
    public partial class LoginToQQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetRequestToken();
        }
        private void GetRequestToken()
        {
            string appId = "";
            //第一个参数是微信appid，第二个是返回地址，第三个是返回的状态，第四个应用授权作用域获取用户信息
            OAuthApi.GetAuthorizeUrl(appId, "http://weixin.senparc.com/oauth2/UserInfoCallback", "JeffreySu", OAuthScope.snsapi_userinfo);//微信登录api
            //这个是QQ接口地址
            Response.Redirect(CreateConnect.CreateConnectFactory(QQSite.Tentent));
        }
    }
}