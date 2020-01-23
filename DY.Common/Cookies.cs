using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DY.Entity;

namespace DY.Common
{
    [Serializable]
    public class Cookies
    {
        #region 将ShopViewInfo写入Cookie
        /// <summary>
        /// 将ShopViewInfo写入Cookie
        /// </summary>
        /// <param name="sV"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static void ShopViewToCookie(ShowViewCollection sV, string cookieName)
        {
            if (HasCookie(cookieName))
                cookieName = SetShopViewCookie();

            IFormatter fm = new BinaryFormatter();
            string StrCartNew = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                fm.Serialize(ms, sV);
                byte[] byt = new byte[ms.Length];
                byt = ms.ToArray();
                StrCartNew = Convert.ToBase64String(byt);
                ms.Flush();
            }
            WriteCookie(cookieName, StrCartNew, 30);
        }
        #endregion

        #region 将Cookie反序列化为ShopMobileView
        /// <summary>
        /// 将Cookie反序列化为ShopMobileView
        /// </summary>
        /// <param name="cookieName">CookieName</param>
        public static ShowViewCollection CookieToShopView(string cookieName)
        {
            if (HasCookie(cookieName))
                cookieName = SetShopViewCookie();

            string StrCart = GetCookie(cookieName);
            if (StrCart == "" || StrCart == string.Empty)
                return null;

            string StrViewNew = GetCookie(cookieName);

            byte[] byt = Convert.FromBase64String(StrViewNew);

            ShowViewCollection SvNew = null;

            using (Stream smNew = new MemoryStream(byt, 0, byt.Length))
            {
                IFormatter fmNew = new BinaryFormatter();
                SvNew = (ShowViewCollection)fmNew.Deserialize(smNew);

            }
            if (SvNew == null)
                return null;
            else
                return SvNew;
        }
        #endregion

        #region 判断是否存在Cookie表
        /// <summary>   
        /// 判断是否存在Cookie表   
        /// </summary>   
        /// <param name="cookieName">Cookie名称</param>   
        /// <returns></returns>   
        public static bool HasCookie(string cookieName)
        {
            bool BoolReturnValue = false;
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
                BoolReturnValue = true;

            return BoolReturnValue;
        }
        #endregion

        #region 为Cookie赋值方法
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(天)</param>
        public static void WriteCookie(string cookieName, string cookieValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null)
                cookie = new HttpCookie(cookieName);

            cookie.Value = cookieValue;
            cookie.Expires = DateTime.Now.AddDays(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 读cookie值
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[cookieName] != null)
                return HttpContext.Current.Request.Cookies[cookieName].Value.ToString();

            return "";
        }
        #endregion

        #region 删除Cookies
        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="strName">主键</param>
        /// <returns></returns>
        public static bool DelCookie(string cookieName)
        {
            try
            {
                HttpCookie Cookie = new HttpCookie(cookieName);
                Cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 设置Cookie的名称
        public static string SetShopViewCookie()
        {
            return "xcxy_ViewShopCookie" ;
        }
        #endregion
    }
}
