using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.Helpers;

namespace DY.OAuthV2SDK
{
    /// <summary>
    /// 配置文件类
    /// </summary>
    public class OAuthConfig
    {
        public const string configPath = "~/config/OAuthV2.config";
        public const string CONFIG_APPLICATION_KEY = "/clientId";
        public const string CONFIG_APPLICATION_SECRET = "/clientSecret";
        public const string CONFIG_APPLICATION_REDIRECTURI = "/redirectUri";
        public const string CONFIG_APPLICATION_USERNAME = "/userName";
        public const string CONFIG_APPLICATION_PASSWORD = "/password";
        public const string CONFIG_APPLICATION_SCOPE = "/scope";
        public const string CONFIG_APPLICATION_access_token = "/access_token";
        public const string CONFIG_APPLICATION_uid = "/uid";
        public const string CONFIG_APPLICATION_AT = "/randomAT";
        public const string CONFIG_ROOT = "/configuration";
        public const string CONFIG_OAUTH = "/oauth";
        public const string CONFIG_BASE = "/base";
        public const string CONFIG_APP = "/app";
        public const string CONFIG_API = "/api";
        public const string CONFIG_APPLICATION_APPID = "/appid";
        /// <summary>
        /// 获取配置文件
        /// </summary>
        public static XmlHelper XmlConfig
        {
            get
            {
                const string CONFIG_PATH = configPath;
                const string CONFIG_NAME = "DY_OAuthV2SDK_CONFIG";

                try
                {
                    XmlHelper xml = null;
                    object cache = System.Web.HttpContext.Current.Cache[CONFIG_NAME];
                    if (cache == null)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath(CONFIG_PATH);
                        if (!System.IO.File.Exists(path)) { throw new System.IO.FileNotFoundException(string.Format("{0}配置文件不存在", CONFIG_PATH)); }

                        xml = new XmlHelper();
                        xml.Load(path);

                        System.Web.Caching.CacheDependency depe = new System.Web.Caching.CacheDependency(path);
                        System.Web.HttpContext.Current.Cache.Insert(CONFIG_NAME, xml, depe);
                    }
                    else
                    {
                        xml = (XmlHelper)cache;
                    }

                    return xml;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取配置文件(DY.OAuthV2.config)的oauth子节点
        /// </summary>
        /// <returns></returns>
        public static List<OAuthEntity> GetConfigOAuths()
        {
            string xpath = CONFIG_ROOT + CONFIG_OAUTH;
            XmlNode oauth_node = XmlConfig.SelectSingleNode(xpath);
            List<OAuthEntity> list = new List<OAuthEntity>();
            if (oauth_node != null)
            {
                foreach (XmlNode item in oauth_node.ChildNodes)
                {
                    if (item.NodeType == XmlNodeType.Element)
                    {
                        OAuthEntity oauth = new OAuthEntity();
                        oauth.name = item.Name;
                        if (item.Attributes["desc"] != null)
                        {
                            oauth.desc = item.Attributes["desc"].Value;
                        }
                        if (item.Attributes["cnname"] != null)
                        {
                            oauth.cnname = item.Attributes["cnname"].Value;
                        }
                        if (item.Attributes["isat"] != null)
                        {
                            oauth.isAt = item.Attributes["isat"].Value;
                        }
                        list.Add(oauth);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取配置文件(DY.OAuthV2.config)的app节点
        /// </summary>
        /// <param name="cfgOAuthName">oauth节点名称</param>
        /// <param name="cfgAppName">app子节点名称</param>
        /// <returns>返回app实体</returns>
        public static AppEntity GetConfigApp(string cfgOAuthName, string cfgAppName)
        {
            if (string.IsNullOrEmpty(cfgAppName))
            {//获取第一个节点
                string oauth_path = CONFIG_ROOT + CONFIG_OAUTH + "/" + cfgOAuthName + CONFIG_APP;
                XmlNode oauth_node = XmlConfig.SelectSingleNode(oauth_path);
                if (oauth_node != null && oauth_node.HasChildNodes)
                {
                    XmlNode first_node = null;
                    foreach (System.Xml.XmlNode item in oauth_node.ChildNodes)
                    {
                        if (item.NodeType == XmlNodeType.Element)
                        {
                            first_node = item;
                            break;
                        }
                    }
                    if (first_node != null)
                    {//第一节点名称
                        cfgAppName = first_node.Name;
                    }
                    else
                    {
                        throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", oauth_path));
                    }
                }
                else
                {
                    throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", oauth_path));
                }
            }

            if (!string.IsNullOrEmpty(cfgAppName))
            {
                //获取应用信息
                string xpath = CONFIG_ROOT + CONFIG_OAUTH + "/" + cfgOAuthName + CONFIG_APP + "/" + cfgAppName;
                string key = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_KEY);
                string secret = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_SECRET);
                string redirecturi = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_REDIRECTURI);
                string username = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_USERNAME);
                string password = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_PASSWORD);
                string scope = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_SCOPE);
                string appid = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_APPID);
                string access_token = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_access_token);
                string uid = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_uid);
                string randomAT = XmlConfig.SelectSingleNodeText(xpath + CONFIG_APPLICATION_AT);
                AppEntity app = new AppEntity();
                if (!string.IsNullOrEmpty(key))
                {
                    if (!string.IsNullOrEmpty(secret))
                    {
                        if (!string.IsNullOrEmpty(redirecturi))
                        {
                            //设置应用信息
                            app.AppName = cfgAppName;
                            app.AppKey = key;
                            app.AppSecret = secret;
                            app.RedirectUri = redirecturi;
                            app.UserName = username;
                            app.Password = password;
                            app.Scope = scope;
                            app.Appid = appid;
                            app.Access_token = access_token;
                            app.Uid = uid;
                            app.RandomAT = randomAT;
                        }
                        else
                        {
                            throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", xpath + CONFIG_APPLICATION_REDIRECTURI));
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", xpath + CONFIG_APPLICATION_SECRET));
                    }
                }
                else
                {
                    throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", xpath + CONFIG_APPLICATION_KEY));
                }
                return app;
            }
            else
            {
                throw new ArgumentNullException(string.Format("app子节点名称"));

            }

        }

        /// <summary>
        /// 获取配置文件(DY.OAuthV2.config)的api节点
        /// </summary>
        /// <param name="cfgApiNodeName">api子节点的名称</param>
        /// <returns></returns>
        public static string GetConfigAPI(string cfgOAuthName, string cfgAppName, string cfgApiName)
        {
            if (!string.IsNullOrEmpty(cfgApiName))
            {
                string xpath = CONFIG_ROOT + CONFIG_OAUTH + "/" + cfgOAuthName + CONFIG_API + "/" + cfgApiName;
                string api = XmlConfig.SelectSingleNodeText(xpath);
                if (!string.IsNullOrEmpty(api))
                {
                    return api;
                }
                else
                {
                    throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", xpath));
                }
            }
            else
            {
                throw new ArgumentNullException(string.Format("api子节点的名称为空"));
            }
        }


        /// <summary>
        /// DES加密/解密KEY
        /// </summary>
        public static string DesKey
        {
            get
            {
                string xpath = CONFIG_ROOT + CONFIG_BASE + "/desKey";
                string value = XmlConfig.SelectSingleNodeText(xpath);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", xpath));
                }
            }
        }

        /// <summary>
        /// 缓存模式 cookie|session
        /// </summary>
        public static string CacheMode
        {
            get
            {
                string xpath = CONFIG_ROOT + CONFIG_BASE + "/cacheMode";
                string value = XmlConfig.SelectSingleNodeText(xpath);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    throw new ArgumentNullException(string.Format("{0}节点：找不到该节点或该值为空", xpath));
                }
            }
        }

    }
}