using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.Helpers;
using DY.OAuthV2SDK.Entitys;
using System.Web;

namespace DY.OAuthV2SDK.OAuths
{
    /// <summary>
    /// OAuth2.0协议
    /// <para>此类理论上所有基于OAuth2.0的平台都适用，如不同可以实现重写。</para>
    /// <para>按各大平台继承此类，实现重写。</para>
    /// </summary>
    public abstract partial class OAuthBase
    {
        #region 公共属性
        protected const string OAuthClientIdKey = "client_id";
        protected const string OAuthClientSecretKey = "client_secret";
        protected const string OAuthRedirectUriKey = "redirect_uri";
        protected const string OAuthResponseTypeKey = "response_type";
        protected const string OAuthStateKey = "state";
        protected const string OAuthDisplayKey = "display";
        protected const string OAuthAccessTokenKey = "access_token";
        protected const string OAuthExpiresInKey = "expires_in";
        protected const string OAuthRefreshTokenKey = "refresh_token";
        protected const string OAuthCodeKey = "code";
        protected const string OAuthGrantTypeKey = "grant_type";
        protected const string OAuthUserNameKey = "username";
        protected const string OAuthPasswordKey = "password";
        protected const string OAuthScopeKey = "scope";
        protected const string OAuthAppidKey = "appid";
        private E_ResponseType _response_type = E_ResponseType.code;
        private E_GrantType _grant_type = E_GrantType.authorization_code;
        private string _client_id = String.Empty;
        private string _client_secret = String.Empty;
        private string _redirect_uri = String.Empty;
        private string _access_token = String.Empty;
        private int _expires_in = 0;
        private string _refresh_token = String.Empty;
        private string _code = String.Empty;
        private string _state = String.Empty;
        private E_Display _display = E_Display.Default;
        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _scope = String.Empty;
         private string _appid = String.Empty;
        

        /// <summary>
        /// 授权类型，此值默认为“code”。
        /// </summary>
        public E_ResponseType ResponseType { get { return _response_type; } set { _response_type = value; } }
        /// <summary>
        /// 授权类型，此值默认为“authorization_code”。
        /// </summary>
        public E_GrantType GrantType { get { return _grant_type; } set { _grant_type = value; } }
        /// <summary>
        /// 申请应用时分配的AppKey
        /// </summary>
        public string ClientId { get { return _client_id; } set { _client_id = value; } }
        /// <summary>
        /// 申请应用时分配的AppSecret
        /// </summary>
        public string ClientSecret { get { return _client_secret; } set { _client_secret = value; } }
        /// <summary>
        /// 成功授权后的回调地址，建议设置为网站首页或网站的用户中心。
        /// </summary>
        public string RedirectUri { get { return _redirect_uri; } set { _redirect_uri = value; } }
        /// <summary>
        /// 通过Authorization Code获取Access Token
        /// </summary>
        public string AccessToken { get { return _access_token; } set { _access_token = value; } }
        /// <summary>
        /// accesstoken有效期时间,unix timestamp格式
        /// </summary>
        public int ExpiresIn { get { return _expires_in; } set { _expires_in = value; } }
        /// <summary>
        /// 刷新token,如果有获取权限则返回
        /// </summary>
        public string RefreshToken { get { return _refresh_token; } set { _refresh_token = value; } }
        /// <summary>
        /// 上一步返回的authorization code。
        /// </summary>
        public string Code { get { return _code; } set { _code = value; } }
        /// <summary>
        /// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
        /// </summary>
        public string State { get { return _state; } set { _state = value; } }
        /// <summary>
        /// 用于展示的样式。不传则默认展示为为PC下的样式。
        /// </summary>
        public E_Display Display { get { return _display; } set { _display = value; } }
        /// <summary>
        /// 授权用户的用户名
        /// </summary>
        public string UserName { get { return _username; } set { _username = value; } }
        /// <summary>
        /// 授权用户的密码
        /// </summary>
        public string Password { get { return _password; } set { _password = value; } }
        /// <summary>
        /// 请求用户授权时向用户显示的可进行授权的列表。
        /// <para>例如：scope=aa,bb,cc</para>
        /// </summary>
        public string Scope { get { return _scope; } set { _scope = value; } }
        /// <summary>
        /// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。
        /// </summary>
        public string Appid { get { return _appid; } set { _appid = value; } }



        /// <summary>
        /// 授权页面类型 可选范围
        /// </summary>
        public enum E_Display
        {
            /// <summary>
            /// 默认授权页面
            /// </summary>
            Default,
            /// <summary>
            /// 支持html5的手机
            /// </summary>
            Mobile,
            /// <summary>
            /// 弹窗授权页
            /// </summary>
            Popup,
            /// <summary>
            /// wap1.2页面
            /// </summary>
            Wap12,
            /// <summary>
            /// 	wap2.0页面	
            /// </summary>
            Wap20,
            /// <summary>
            /// js-sdk 专用 授权页面是弹窗，返回结果为js-sdk回掉函数		
            /// </summary>
            Js,
            /// <summary>
            /// 站内应用专用,站内应用不传display参数,并且response_type为token时,默认使用改display.授权后不会返回access_token，只是输出js刷新站内应用父框架
            /// </summary>
            Apponweibo

        }

        /// <summary>
        /// 请求的类型,可以为:authorization_code ,password,refresh_token
        /// </summary>
        public enum E_GrantType
        {
            /// <summary>
            /// grant_type为authorization_code时
            /// </summary>
            authorization_code,
            /// <summary>
            /// grant_type为password时
            /// </summary>
            password,
            /// <summary>
            /// grant_type为refresh_token时
            /// </summary>
            refresh_token
        }

        /// <summary>
        /// 支持的值包括 code 和token 默认值为code
        /// </summary>
        public enum E_ResponseType
        {
            /// <summary>
            /// response_type为code
            /// </summary>
            code,
            /// <summary>
            /// response_type为token
            /// </summary>
            token
        }
        #endregion

        #region 获取授权

        /// <summary>
        /// 请求用户授权Token
        /// </summary>
        /// <returns></returns>
        public virtual string GetAuthorize()
        {
            string authorizeUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "authorize");
            return GetAuthorize(authorizeUrl);
        }        
        
        /// <summary>
        /// 请求用户授权Token
        /// </summary>
        /// <param name="authorizeUrl">授权Url</param>
        /// <returns></returns>
        public virtual string GetAuthorize(string authorizeUrl)
        {
            if (string.IsNullOrEmpty(authorizeUrl))
            {
                throw new ArgumentNullException(string.Format("authorizeUrl 为空值"));
            }
            else
            {
                NameValueCollection paras = new NameValueCollection();
                paras.Add(OAuthClientIdKey, this.ClientId);
                paras.Add(OAuthRedirectUriKey, this.RedirectUri);
                paras.Add(OAuthResponseTypeKey, this.ResponseType.ToString());
                paras.Add(OAuthDisplayKey, this.Display.ToString().ToLower());
                if (!string.IsNullOrEmpty(this.State))
                {
                    paras.Add(OAuthStateKey, this.State);
                }
                if (!string.IsNullOrEmpty(this.Scope))
                {
                    paras.Add(OAuthScopeKey, this.Scope);
                }
                if (!string.IsNullOrEmpty(this.Appid))
                {
                    paras.Add(OAuthAppidKey, this.Appid);
                }
                return string.Format("{0}?{1}", authorizeUrl, HttpHelper.GetQueryFromParas(paras));
            }

        }

        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        public virtual ApiToken GetAccessToken()
        {
            string accessTokenUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "access_token");
            string response = GetAccessToken(accessTokenUrl);
            return Helpers.UtilHelper.ParseJson<ApiToken>(response);
        }

        /// <summary>
        /// 获取授权过的Access Token
        /// <param name="accessTokenUrl">Access Url</param>
        /// </summary>
        public virtual string GetAccessToken(string accessTokenUrl)
        {
            if (string.IsNullOrEmpty(accessTokenUrl))
            {
                throw new ArgumentNullException(string.Format("accessTokenUrl 为空值"));
            }
            else
            {
                if (HttpContext.Current.Request["error"] != null)
                {
                    throw new Exception(string.Format("error {0}", HttpContext.Current.Request["error"]));
                }
                else
                {
                    if (HttpContext.Current.Request[OAuthStateKey] != null)
                    {
                        this.State = HttpContext.Current.Request[OAuthStateKey];
                    }
                    if (HttpContext.Current.Request[OAuthAppidKey] != null)
                    {
                        this.Appid = HttpContext.Current.Request[OAuthAppidKey];
                    }
                    if (this.ResponseType == E_ResponseType.code)
                    {
                        this.Code = HttpContext.Current.Request[OAuthCodeKey];
                        if (string.IsNullOrEmpty(this.Code))
                        {
                            throw new ArgumentNullException(string.Format("{0} 为空值", OAuthCodeKey));
                        }
                    }
                    else if (this.ResponseType == E_ResponseType.token)
                    {
                        this.AccessToken = HttpContext.Current.Request[OAuthAccessTokenKey];
                        this.ExpiresIn = int.TryParse(HttpContext.Current.Request[OAuthExpiresInKey], out _expires_in) ? _expires_in : 0;
                        this.RefreshToken = HttpContext.Current.Request[OAuthRefreshTokenKey];

                        if (ExpiresIn == 0 || string.IsNullOrEmpty(this.AccessToken))
                        {
                            throw new ArgumentNullException(string.Format("{0}或者{1} 为空值", OAuthAccessTokenKey, OAuthExpiresInKey));
                        }
                    }

                    NameValueCollection paras = new NameValueCollection();
                    paras.Add(OAuthClientIdKey, this.ClientId);
                    paras.Add(OAuthClientSecretKey, this.ClientSecret);
                    paras.Add(OAuthGrantTypeKey, this.GrantType.ToString());

                    if (this.GrantType == E_GrantType.authorization_code)
                    {
                        //grant_type为authorization_code时
                        paras.Add(OAuthCodeKey, this.Code);
                        paras.Add(OAuthRedirectUriKey, this.RedirectUri);
                    }
                    else if (this.GrantType == E_GrantType.password)
                    {
                        //grant_type为password时
                        paras.Add(OAuthUserNameKey, this.UserName);
                        paras.Add(OAuthPasswordKey, this.Password);
                    }
                    else if (this.GrantType == E_GrantType.refresh_token)
                    {
                        //grant_type为refresh_token时
                        paras.Add(OAuthRefreshTokenKey, this.RefreshToken);
                    }

                    return HttpHelper.HttpPost(accessTokenUrl, paras);
                }
            }
        }


        #endregion
    }
}