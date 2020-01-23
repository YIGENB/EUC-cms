using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace DY.OAuthV2SDK.OAuths
{
    public abstract partial class OAuthBase
    {
        #region 静态方法
        /// <summary>
        /// 动态创建实例
        /// <para>找不到类或实例失败返回null</para>
        /// <para>使用当前缓存协议名称</para>
        /// <para>使用app节点第一子节点</para>
        /// </summary>
        /// <returns>找不到类返回null</returns>
        public static OAuthBase CreateInstance()
        {
            string cfgOauthName = CacheOAuthName;
            return CreateInstance(cfgOauthName, string.Empty);
        }
        /// <summary>
        /// 动态创建实例
        /// <para>找不到类或实例失败返回null</para>
        /// <para>使用app节点第一子节点</para>
        /// </summary>
        /// <param name="cfgOauthName">协议名称</param>
        /// <returns>找不到类返回null</returns>
        public static OAuthBase CreateInstance(string cfgOauthName)
        {
            return CreateInstance(cfgOauthName, string.Empty);
        }

        /// <summary>
        /// 动态创建实例
        /// <para>找不到类或实例失败返回null</para>
        /// </summary>
        /// <param name="cfgOauthName">协议名称</param>
        /// <param name="cfgAppName">应用名称</param>
        /// <returns>找不到类返回null</returns>
        public static OAuthBase CreateInstance(string cfgOauthName, string cfgAppName)
        {
            //文件路径和命名空间不允许随意修改
            Type oauth = Type.GetType(string.Format("DY.OAuthV2SDK.OAuths.{0}s.{0}OAuth,DY.OAuthSDK", cfgOauthName), false, true);
            if (oauth != null)
            {
                return (OAuthBase)Activator.CreateInstance(oauth, new object[] { cfgAppName });
            }
            return null;
        }

        /// <summary>
        /// 是否拥有缓存
        /// </summary>
        public static bool HasCacheOAuth
        {
            get
            {
                if (OAuthConfig.CacheMode == "session")
                {
                    return HasSessionOAuth;
                }
                else
                { //cookie
                    return HasCookieOAuth;
                }
            }
        }

        /// <summary>
        /// 获取Cookie缓存协议名称(当前协议)
        /// </summary>
        public static string CacheOAuthName
        {
            get
            {
                if (OAuthConfig.CacheMode == "session")
                {
                    return SessionOAuthName;
                }
                else
                { //cookie
                    return CookieOAuthName;
                }
            }
        }
        /// <summary>
        /// 是否拥有Cookie缓存
        /// </summary>
        protected static bool HasCookieOAuth
        {
            get
            {
                return !string.IsNullOrEmpty(CookieOAuthName);
            }
        }

        /// <summary>
        /// 获取Cookie缓存协议名称(当前协议)
        /// </summary>
        protected static string CookieOAuthName
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CACHE_NAME];
                if (cookie != null)
                {
                    return cookie.Values[CACHE_OAUTH_NAME];
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 是否拥有Session缓存
        /// </summary>
        protected static bool HasSessionOAuth
        {
            get
            {
                return !string.IsNullOrEmpty(SessionOAuthName);
            }
        }

        /// <summary>
        /// 获取Session缓存协议名称(当前协议)
        /// </summary>
        protected static string SessionOAuthName
        {
            get
            {
                object session = HttpContext.Current.Session[CACHE_NAME];
                if (session != null && !string.IsNullOrEmpty(session.ToString()))
                {
                    object[] values = (object[])session;
                    return Convert.ToString(values[0]);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取空参数
        /// </summary>
        /// <returns></returns>
        public static NameValueCollection GetNewParas()
        {
            return new NameValueCollection();
        }
        #endregion
    }
}