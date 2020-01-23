using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

using DY.Common;
using DY.Entity;
using DY.Config;

using NVelocityTemplateEngine;
using NVelocityTemplateEngine.Interfaces;
using Newtonsoft.Json;
using DY.LanguagePack;

namespace DY.Site
{
    /// <summary>
    /// DY页面基类
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        /// <summary>
        /// 安装程序类
        /// </summary>
        protected internal Install install = new Install();
        /// <summary>
        /// 授权程序类
        /// </summary>
        protected internal SoftReg softreg = new SoftReg();
        /// <summary>
        /// 网站基本信息
        /// </summary>
        protected internal BaseConfigInfo config;
        /// <summary>
        /// 网站设置
        /// </summary>
        protected internal SystemConfig systemConfig = new SystemConfig();
        /// <summary>
        /// 网站用户
        /// </summary>
        protected internal SiteUser user = new SiteUser();
        /// <summary>
        /// 商品相关
        /// </summary>
        protected internal Goods goods = new Goods();
        /// <summary>
        /// 重写地址规则库
        /// </summary>
        protected internal UrlrewriteConfigInfo urlrewriteinfo;
        /// <summary>
        /// 下载相关
        /// </summary>
        protected internal Download down = new Download();
        /// <summary>
        /// 商城相关
        /// </summary>
        protected internal Store store = new Store();
        /// <summary>
        /// 内容管理
        /// </summary>
        protected internal CMS cms = new CMS();
        /// <summary>
        /// 数据缓存
        /// </summary>
        protected internal Caches caches = new Caches();
        /// <summary>
        /// 网站工具
        /// </summary>
        protected internal SiteUtils siteUtils = new SiteUtils();
        /// <summary>
        /// 当前查询结果总数
        /// </summary>
        protected internal int ResultCount = 0;
        /// <summary>
        /// 当前在线用户信息
        /// </summary>
        protected internal OnlineUserInfo oluserinfo;
        /// <summary>
        /// 当前用户的用户名
        /// </summary>
        protected internal string username;
        /// <summary>
        /// 当前用户的密码
        /// </summary>
        protected internal string password;
        /// <summary>
        /// 当前用户的用户ID
        /// </summary>
        protected internal int userid;
        /// <summary>
        /// 当前页面是否被POST请求
        /// </summary>
        protected internal bool ispost;
        /// <summary>
        /// 当前页面是否被GET请求
        /// </summary>
        protected internal bool isget;
        /// <summary>
        /// 当前页面执行动作
        /// </summary>
        protected internal string act;
        /// <summary>
        /// 是否为ajax请求
        /// </summary>
        protected internal bool isajax = DYRequest.getRequestInt("is_ajax") == 1 ? true : false;
        /// <summary>
        /// 当前在线人数
        /// </summary>
        protected internal int onlineusercount = 1;
        /// <summary>
        /// 是否为需检测校验码页面
        /// </summary>
        protected bool isseccode = true;
        /// <summary>
        /// 当前页面名称
        /// </summary>
        public string pagename = DYRequest.GetPageName();
        /// <summary>
        /// 微信公众号ID
        /// </summary>
        protected internal int pid = DYRequest.getRequestInt("pid", 1);
        /// <summary>
        /// 当前主键值
        /// </summary>
        protected internal int id = DYRequest.getRequestInt("id");


        /// <summary>
        /// BasePage类构造函数
        /// </summary>
        public PageBase()
        {
            //统计蜘蛛爬行记录
            SiteUtils.SpiderBot();

            config = BaseConfig.Get();

            ispost = DYRequest.IsPost;
            isget = DYRequest.IsGet;
            act = DYRequest.getAction();
            urlrewriteinfo = UrlrewriteConfig.Get();

            #region 如果IP访问列表有设置则进行判断
            if (config.BanIp.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.BanIp, "\n");
                if (Utils.InIPArray(DYRequest.GetIP(), regctrl))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<br /><br /><div style=\"width:100%\" align=\"center\"><div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\">");
                    sb.Append("<img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" />&nbsp; 您的IP地址不在系统允许的范围之内</div></div>");
                    Context.Response.Write(sb.ToString());
                    Context.Response.End();
                    return;
                }
            }
            #endregion
        }
        /// <summary>
        /// 模板输出
        /// </summary>
        /// <param name="context">Hashtable</param>
        /// <param name="json"></param>
        /// <param name="tlpPath"></param>
        /// <param name="templateName"></param>
        protected void DisplayTemplate(IDictionary context, string templateName,string tlpPath,bool json)
        {
            Response.Write(GetTemplate(context, templateName, tlpPath, json));
            Response.End();
        }
        /// <summary>
        /// string类模板
        /// </summary>
        /// <param name="templateString"></param>
        /// <returns></returns>
        protected void DisplayMemoryTemplate(string templateString)
        {
            this.DisplayMemoryTemplate(new Hashtable(), templateString);
        }
        /// <summary>
        /// string类模板
        /// </summary>
        /// <param name="context">Hashtable</param>
        /// <param name="templateString"></param>
        protected void DisplayMemoryTemplate(IDictionary context, string templateString)
        {
            INVelocityEngine memoryEngine =
                NVelocityEngineFactory.CreateNVelocityMemoryEngine(true);

            Response.Write(memoryEngine.Process(context, templateString));
            Response.End();
        }
        /// <summary>
        /// 获取解析过后的模板数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <param name="tlpPath"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string GetTemplate(IDictionary context, string templateName, string tlpPath, bool json)
        {
            INVelocityEngine fileEngine =
                NVelocityEngineFactory.CreateNVelocityFileEngine(Server.MapPath(tlpPath), true);

            context.Add("config", this.config);
            context.Add("SiteUtils", this.siteUtils);
            context.Add("SystemConfig", this.systemConfig);
            context.Add("User", this.user);
            context.Add("Goods", this.goods);
            context.Add("Download", this.down);
            context.Add("Store", this.store);
            context.Add("CMS", this.cms);
            context.Add("SessionID", Session.SessionID);
            context.Add("Caches", this.caches);
            context.Add("act", this.act);
            context.Add("html", config.UrlRewriterKzm);//伪静态后缀

            string html = SiteUtils.ReplacePath(context, fileEngine.Process(context, templateName + BaseConfig.WebSkinSuffix), tlpPath);

            return json ? this.MakeJson(html, 0, "", context) : html;
        }
        /// <summary>
        /// 创建一个JSON格式的数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string MakeJson(IDictionary context)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.None;

                if (context != null)
                {
                    if (context.Count > 0)
                    {
                        jsonWriter.WriteStartObject();
                        foreach (DictionaryEntry de in context)
                        {
                            jsonWriter.WritePropertyName(de.Key.ToString());
                            if (de.Value.GetType().ToString() == "System.Int32")
                                jsonWriter.WriteValue(Convert.ToInt32(de.Value.ToString()));
                            else
                                jsonWriter.WriteValue(de.Value.ToString());
                        }
                        jsonWriter.WriteEndObject();
                    }
                }
            }

            return sb.ToString();
        }
        /// <summary>
        /// 创建一个JSON格式的数据
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="error">错误代码</param>
        /// <param name="message">信息</param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string MakeJson(string content, int error, string message, IDictionary context)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.None;

                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("error");
                jsonWriter.WriteValue(error);
                jsonWriter.WritePropertyName("message");
                jsonWriter.WriteValue(message);
                jsonWriter.WritePropertyName("content");
                jsonWriter.WriteValue(content);
                if (context != null)
                {
                    if (context.Count > 0)
                    {
                        jsonWriter.WritePropertyName("filter");
                        jsonWriter.WriteStartObject();
                        foreach (DictionaryEntry de in context)
                        {
                            if (de.Key.ToString() != "list" && de.Key.ToString() != "act" && de.Key.ToString() != "pager" && de.Key.ToString() != "isajax" && de.Key.ToString() != "config" && de.Key.ToString() != "SiteUtils" && de.Key.ToString() != "SystemConfig" && de.Key.ToString() != "id" && de.Key.ToString() != "entity" && de.Key.ToString() != "Goods" && de.Key.ToString() != "SessionID" && de.Key.ToString() != "User")
                            {
                                jsonWriter.WritePropertyName(de.Key.ToString());
                                if (de.Value.GetType().ToString() == "System.Int32" || de.Value.GetType().ToString() == "System.Int16" || de.Value.GetType().ToString() == "System.Int64")
                                    jsonWriter.WriteValue(Convert.ToInt32(de.Value.ToString()));
                                else
                                    jsonWriter.WriteValue(de.Value.ToString());
                            }
                        }
                        jsonWriter.WriteEnd();
                    }
                }
                jsonWriter.WriteEndObject();
            }

            return sb.ToString();
        }
        /// <summary>
        /// 创建一个JSON格式的数据
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="error">错误代码(1success,2info,3warning,4error)</param>
        /// <param name="message">信息</param>
        /// <returns></returns>
        protected string MakeJson(string content, int error, string message)
        {
            return this.MakeJson(content, error, message, null);
        }
        /// <summary>
        /// 输出json提示信息
        /// </summary>
        /// <param name="error"></param>
        /// <param name="message"></param>
        protected void DisplayJsonMessage(int error, string message)
        {
            this.DisplayMemoryTemplate(this.MakeJson("", error, message));
        }
        /// <summary>
        /// 输出json提示信息
        /// </summary>
        /// <param name="message"></param>
        protected void DisplayJsonMessage(string message)
        {
            this.DisplayJsonMessage(1,message);
        }
     }
}
