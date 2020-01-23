using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using DY.OAuthV2SDK.OAuths;
using DY.OAuthV2SDK.Helpers;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.OAuths.Weixins.Models;

namespace DY.OAuthV2SDK.OAuths.Weixins
{
    /// <summary>
    /// QQ空间协议
    /// </summary>
    public class WeixinOAuth : OAuthBase
    {
        #region 属性
        private const string OAuthConsumerKey = "oauth_consumer_key";
        private const string OAuthOpenIdKey = "openid";
        private const string OAuthClientIpKey = "clientip";
        private const string OAuthOauthVersionKey = "oauth_version";
        private const string OAuthAppidKey = "appid";
        private string _openid = String.Empty;
        private string _clientip = "127.0.0.1";
        private string _oauth_version = "2.a";
       
        /// <summary>
        /// 用户的ID，与QQ号码一一对应。
        /// </summary>
        public string OpenId { get { return _openid; } set { _openid = value; } }
        /// <summary>
        /// 客户端的ip
        /// </summary>
        public string ClientIp { get { return _clientip; } set { _clientip = value; } }
        /// <summary>
        /// 版本号，必须为2.a
        /// </summary>
        public string OauthVersion { get { return _oauth_version; } set { _oauth_version = value; } }
        /// <summary>
        /// 协议节点名称(区分大小写)
        /// </summary>
        public override string OAuthName { get { return "weixin"; } }
        /// <summary>
        /// 协议节点描述
        /// </summary>
        public override string OAuthDesc { get { return "微信"; } }
        protected const string CACHE_OPENID = "opid";
        #endregion


        public WeixinOAuth()
        {
            this.ClientIp = UtilHelper.GetClientIP();
        }

        public WeixinOAuth(string cfgAppName)
        {
            this.ClientIp = UtilHelper.GetClientIP();
        }

        /// <summary>
        /// 用户id(openid)
        /// </summary>
        public override string Uid
        {
            get
            {
                return this.OpenId;
            }
            set
            {
                this.OpenId = value;
            }
        }

        /// <summary>
        /// 获取token参数
        /// </summary>
        /// <returns></returns>
        public override NameValueCollection GetTokenParas()
        {
            NameValueCollection paras = new NameValueCollection();
            paras.Add(OAuthConsumerKey, this.ClientId);
            paras.Add(OAuthAccessTokenKey, this.AccessToken);
            paras.Add(OAuthOpenIdKey, this.OpenId);
            paras.Add(OAuthAppidKey, this.Appid);
            paras.Add(OAuthOauthVersionKey, this.OauthVersion);
            paras.Add(OAuthClientIpKey, this.ClientIp);
            return paras;
        }

        /// <summary>
        /// 获取授权过的Access Token
        /// <param name="accessTokenUrl">Access Url</param>
        /// </summary>
        public override string GetAccessToken(string accessTokenUrl)
        {
            if (string.IsNullOrEmpty(accessTokenUrl))
            {
                throw new ArgumentNullException(string.Format("accessTokenUrl 为空值"));
            }
            return base.GetAccessToken(accessTokenUrl);
        }

        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        public override ApiToken GetAccessToken()
        {
            string accessTokenUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "access_token");
            string response = GetAccessToken(accessTokenUrl);
            ApiToken api = new ApiToken();
            if (!string.IsNullOrEmpty(response))
            {
                NameValueCollection qs = HttpHelper.GetParasFromQuery(response);

                if (qs[OAuthAccessTokenKey] != null)
                {
                    this.AccessToken = qs[OAuthAccessTokenKey].ToString();
                    api.access_token = this.AccessToken;
                }

                if (qs[OAuthExpiresInKey] != null)
                {
                    int _expires_in = 0;
                    this.ExpiresIn = int.TryParse(qs[OAuthExpiresInKey].ToString(), out _expires_in) ? _expires_in : 0;

                    api.expires_in = this.ExpiresIn;
                }

                if (qs[OAuthRefreshTokenKey] != null)
                {
                    this.RefreshToken = qs[OAuthRefreshTokenKey].ToString();
                    api.refresh_token = this.RefreshToken;
                }
                GetOpenId(this.AccessToken);
            }
            return api;
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <param name="files">文件路径集合</param>
        /// <returns></returns>
        public override string ApiByHttpPostWithPic(string apiName, NameValueCollection queryParas, NameValueCollection files)
        {
            return base.ApiByHttpPostWithPic(apiName, queryParas, files, true);
        }

        /// <summary>
        /// 更新Cooike缓存
        /// </summary>
        /// <param name="dtExpires">缓存时间</param>
        /// <param name="cookieDomain">Cooike缓存域名</param>
        /// <param name="cookiePath">Cooike缓存路径</param>
        protected override void UpdateCookie(DateTime dtExpires, string cookieDomain, string cookiePath)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CACHE_NAME];
            if (cookie == null) { cookie = new HttpCookie(CACHE_NAME); }
            cookie.Values[CACHE_OAUTH_NAME] = OAuthName.ToLower();
            cookie.Values[CACHE_APP_NAME] = App.AppName.ToLower();
            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                cookie.Values[CACHE_TOKEN] = Helpers.SecurityHelper.DesEncrypt(this.AccessToken, OAuthConfig.DesKey);
            }
            if (!string.IsNullOrEmpty(this.OpenId))
            {
                cookie.Values[CACHE_OPENID] = Helpers.SecurityHelper.DesEncrypt(this.OpenId, OAuthConfig.DesKey);
            }
            cookie.Expires = dtExpires;
            cookie.HttpOnly = true;
            if (!string.IsNullOrEmpty(cookieDomain))
            {
                cookie.Domain = cookieDomain;
            }
            if (!string.IsNullOrEmpty(cookiePath))
            {
                cookie.Path = cookiePath;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 更新session缓存
        /// </summary>
        protected override void UpdateSession()
        {
            object[] values = new object[3];
            values[0] = OAuthName.ToLower();
            values[1] = App.AppName.ToLower();
            values[2] = this.AccessToken;
            values[3] = this.OpenId;
            HttpContext.Current.Session[CACHE_NAME] = values;
        }

        /// <summary>
        /// 是否拥有Cookie缓存
        /// </summary>
        protected override bool HasCookie
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CACHE_NAME];
                if (cookie != null && cookie.HasKeys && !string.IsNullOrEmpty(cookie.Values[CACHE_TOKEN]) && !string.IsNullOrEmpty(cookie.Values[CACHE_OPENID]))
                {
                    try
                    {
                        string token = Helpers.SecurityHelper.DesDecrypt(cookie.Values[CACHE_TOKEN], OAuthConfig.DesKey);
                        string openid = Helpers.SecurityHelper.DesDecrypt(cookie.Values[CACHE_OPENID], OAuthConfig.DesKey);
                        this.AccessToken = token;
                        this.OpenId = openid;
                        return true;
                    }
                    catch
                    {
                        //密文不正确
                        base.ClearCookie();
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 是否拥有Session缓存
        /// </summary>
        protected override bool HasSession
        {
            get
            {
                object session = HttpContext.Current.Session[CACHE_NAME];
                if (session != null && !string.IsNullOrEmpty(session.ToString()))
                {
                    object[] values = (object[])session;
                    this.AccessToken = Convert.ToString(values[2]);
                    this.OpenId = Convert.ToString(values[3]);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        private ApiResult GetOpenId(string accessToken)
        {
            var paras = new NameValueCollection();
            paras.Add(OAuthAccessTokenKey, accessToken);
            string response = ApiByHttpPost("me", paras);
            response = response.Replace("callback(", "").Replace(");", "");
            ApiResult api = new ApiResult();
            var token = Helpers.UtilHelper.ParseJson<WeixinMToken>(response);
            if (token.ret == 0)
            {
                api.data = token.openid;
                this.OpenId = token.openid;
            }
            else
            {
                api.ret = 1;
                api.errcode = token.errcode;
                api.msg = token.msg;
            }
            return api;
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="paras">参数集合</param>
        /// <returns></returns>
        public override ApiResult GetUid(string accessToken)
        {
            ApiResult api = new ApiResult();
            api.data = this.OpenId;
            return api;
        }


        /// <summary>
        /// 发送微博
        ///  <para>data: 当前的返回status_id</para>
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="strText">微博内容</param>
        /// <returns></returns>
        public override ApiResult SendStatus(string accessToken, string strText)
        {
            //官方暂无接口
            ApiResult api = new ApiResult();
            api.ret = 1;
            api.errcode = "1";
            api.msg = "官方暂无接口";
            return api;
        }

        /// <summary>
        /// 发送图片微博
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="strText">微博内容</param>
        /// <param name="strFile">图片绝对路径</param>
        /// <returns></returns>
        public override ApiResult SendStatusWithPic(string accessToken, string strText, string strFile)
        {
            //官方暂无接口
            ApiResult api = new ApiResult();
            api.ret = 1;
            api.errcode = "1";
            api.msg = "官方暂无接口";
            return api;
        }

        /// <summary>
        /// 发送文章类型，如头条
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="title">标题</param>
        /// <param name="des">简介</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public override ApiResult SendStatusToArticle(string accessToken, string title, string des, string content)
        {
            //官方暂无接口
            ApiResult api = new ApiResult();
            api.ret = 1;
            api.errcode = "1";
            api.msg = "官方暂无接口";
            return api;
        }
    }
}