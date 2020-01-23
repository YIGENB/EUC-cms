using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DY.OAuthSDK.Connect.Util
{
    internal class SkyUtility
    {

        internal static String BuildQueryString(params WeiboParaeter[] parameter)
        {
            List<String> list = new List<String>();
            foreach (WeiboParaeter item in parameter)
            {
                String value = String.Format("{0}", item.Value);
                if (!String.IsNullOrEmpty(value))
                {
                    list.Add(String.Format("{0}={1}", Uri.EscapeDataString(item.Name),
                        Uri.EscapeDataString(value)));
                }
            }
            return String.Join("&", list.ToArray());
        }

        internal static String BuildQueryString(Dictionary<String, String> kvp)
        {
            List<String> list = new List<String>();
            foreach (var item in kvp)
            {
                String value = String.Format("{0}", item.Value);
                if (!String.IsNullOrEmpty(value))
                {
                    list.Add(String.Format("{0}={1}", Uri.EscapeDataString(item.Key),
                        Uri.EscapeDataString(value)));
                }
            }
            return String.Join("&", list.ToArray());
        }
    }
    /// <summary>
    /// 请求类型
    /// </summary>
    internal enum RequestMethod
    {
        Get,
        Post
    }
    /// <summary>
    /// 获取类型
    /// </summary>
    public enum ResponseType
    { 
        Code,
        Token
    }
    /// <summary>
    /// 平台类型
    /// </summary>
    public enum DisplayType
    {
        Default,
        Mobile,
        Popup
    }
    /// <summary>
    /// 授权类型
    /// </summary>
    internal enum GrantType
    {
        AuthorizationCode,
        Password,
        RefreshToken
    }
    /// <summary>
    /// 站点类型
    /// </summary>
    public enum QQSite
    { 
        Tentent,
        Weibo
    }
}
