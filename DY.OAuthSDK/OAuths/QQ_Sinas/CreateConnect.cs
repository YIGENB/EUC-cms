using System;
using System.Collections.Generic;
using System.Text;
using DY.OAuthSDK.Connect.Util;
using DY.OAuthSDK.Connect.Model;

namespace DY.OAuthSDK.Login
{
    public class CreateConnect
    {

        private static Connect connect = null;

        public static String CreateConnectFactory(QQSite site)
        {
            String authorize_url = String.Empty;
            Connect connect = null;
            String state = getGuid;
            switch (site)
            {
                case QQSite.Weibo:
                    connect = new DY.OAuthSDK.Login.WeiBo(site);
                    authorize_url = connect.GetAuthorizeURL(ResponseType.Code, getGuid);
                    break;
                case QQSite.Tentent:
                    connect = new DY.OAuthSDK.Login.Tencent(site);
                    authorize_url = connect.GetAuthorizeURL(ResponseType.Code, getGuid);
                    break;
            }
            return authorize_url;
        }

        public static AccessToken CreateConnectAccessToken(QQSite site, String code)
        {
            AccessToken accessToken = null;
            switch (site)
            {
                case QQSite.Tentent:
                    connect = new DY.OAuthSDK.Login.Tencent(site);
                    break;
                case QQSite.Weibo:
                    connect = new DY.OAuthSDK.Login.WeiBo(site);
                    break;
            }
            if (connect != null)
            {
                accessToken = connect.GetAccessTokenByAuthorizationCode(code);
            }
            return accessToken;
        }

        public static String GetAppk
        {
            get { return connect.GetAppKey; }
        }

        public static String GetCommand(String url, params WeiboParaeter[] parameter)
        {
            return connect.Request(url, RequestMethod.Get, parameter);
        }

        public static String PostCommand(String url, params WeiboParaeter[] parameter)
        {
            return connect.Request(url, RequestMethod.Post, parameter);
        }

        /// <summary>
        /// 此属性作用随机生成一个guid值
        /// </summary>
        private static String getGuid
        {
            get
            {
                String guide =
                    Guid.NewGuid().ToString();
                //建议最好加密，我这里使用的是md5,你们自行修改。
                //guide = CMS.Common.SecurityObject.PassEncrypt(guide, 1);
                System.Web.HttpContext.Current.Session["ConnectGuid"] = guide;
                return guide;
            }
        }
    }
}
