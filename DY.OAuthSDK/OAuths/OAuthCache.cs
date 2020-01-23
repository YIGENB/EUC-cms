using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using DY.OAuthV2SDK.Entitys;

namespace DY.OAuthV2SDK.OAuths
{
    public abstract partial class OAuthBase
    {
        #region 缓存操作
        /// <summary>
        /// 缓存前缀
        /// </summary>
        protected const string CACHE_NAME = "DY_oauthv2";
        protected const string CACHE_OAUTH_NAME = "oauth";
        protected const string CACHE_APP_NAME = "app";
        protected const string CACHE_TOKEN = "token";

        /// <summary>
        /// 更新Cooike缓存
        /// </summary>
        public virtual void UpdateCache()
        {
            this.UpdateCache(DateTime.Now.AddSeconds(this.ExpiresIn));
        }

        /// <summary>
        /// 更新Cooike缓存
        /// </summary>
        /// <param name="dtExpires">缓存时间</param>
        public virtual void UpdateCache(DateTime dtExpires)
        {
            UpdateCookie(dtExpires, string.Empty, string.Empty);
        }

        /// <summary>
        /// 更新缓存 cookie|session
        /// </summary>
        /// <param name="dtExpires">缓存时间</param>
        /// <param name="cookieDomain">Cooike缓存域名</param>
        /// <param name="cookiePath">Cooike缓存路径</param>
        public virtual void UpdateCache(DateTime dtExpires, string cookieDomain, string cookiePath)
        {
            if (OAuthConfig.CacheMode == "session")
            {
                UpdateSession();
            }
            else
            { //cookie
                UpdateCookie(dtExpires, cookieDomain, cookiePath);
            }
        }

        /// <summary>
        /// 是否拥有缓存 cookie|session
        /// </summary>
        public virtual bool HasCache
        {
            get
            {
                if (OAuthConfig.CacheMode == "session")
                {
                    return HasSession;
                }
                else
                { //cookie
                    return HasCookie;
                }
            }
        }

        /// <summary>
        /// 清除缓存 cookie|session
        /// </summary>
        public virtual void ClearCache()
        {
            if (OAuthConfig.CacheMode == "session")
            {
                ClearSession();
            }
            else
            { //cookie
                ClearCookie();
            }

        }

        /// <summary>
        /// 更新Cooike缓存
        /// </summary>
        /// <param name="dtExpires">缓存时间</param>
        /// <param name="cookieDomain">Cooike缓存域名</param>
        /// <param name="cookiePath">Cooike缓存路径</param>
        protected virtual void UpdateCookie(DateTime dtExpires, string cookieDomain, string cookiePath)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CACHE_NAME];
            if (cookie == null) { cookie = new HttpCookie(CACHE_NAME); }
            cookie.Values[CACHE_OAUTH_NAME] = OAuthName.ToLower();
            cookie.Values[CACHE_APP_NAME] = App.AppName.ToLower();
            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                cookie.Values[CACHE_TOKEN] = Helpers.SecurityHelper.DesEncrypt(this.AccessToken, OAuthConfig.DesKey);
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
        protected virtual void UpdateSession()
        {
            object[] values = new object[3];
            values[0] = OAuthName.ToLower();
            values[1] = App.AppName.ToLower();
            values[2] = this.AccessToken;
            HttpContext.Current.Session[CACHE_NAME] = values;
        }

        /// <summary>
        /// 是否拥有Cookie缓存
        /// </summary>
        protected virtual bool HasCookie
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CACHE_NAME];
                if (cookie != null && cookie.HasKeys && !string.IsNullOrEmpty(cookie.Values[CACHE_TOKEN]))
                {
                    try
                    {
                        string value = Helpers.SecurityHelper.DesDecrypt(cookie.Values[CACHE_TOKEN], OAuthConfig.DesKey);
                        this.AccessToken = value;
                        return true;
                    }
                    catch
                    {
                        //密文不正确
                        this.ClearCookie();
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 是否拥有Session缓存
        /// </summary>
        protected virtual bool HasSession
        {
            get
            {
                object session = HttpContext.Current.Session[CACHE_NAME];
                if (session != null && !string.IsNullOrEmpty(session.ToString()))
                {
                    object[] values = (object[])session;
                    this.AccessToken = Convert.ToString(values[2]);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 清除Cookie缓存
        /// </summary>
        protected virtual void ClearCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CACHE_NAME];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 清除Session缓存
        /// </summary>
        protected virtual void ClearSession()
        {
            HttpContext.Current.Session[CACHE_NAME] = string.Empty;
        }

        #endregion

    }
}