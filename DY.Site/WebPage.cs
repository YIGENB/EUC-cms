using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using DY.Common;
using DY.Config;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 前台页面类
    /// </summary>
    public class WebPage : PageBase
    {
        /// <summary>
        /// 导航id
        /// </summary>
        public string navid = "1";

        private string _pagetitle;
        /// <summary>
        /// SEO标题
        /// </summary>
        public string pagetitle
        {
            get { return _pagetitle; }
            set { _pagetitle = value; }
        }

        private string _pagekeywords;
        /// <summary>
        /// SEO关键字
        /// </summary>
        public string pagekeywords
        {
            get { return _pagekeywords; }
            set { _pagekeywords = value; }
        }

        private string _pagedesc;
        /// <summary>
        /// SEO描述
        /// </summary>
        public string pagedesc
        {
            get { return _pagedesc; }
            set { _pagedesc = value; }
        }


        /// <summary>
        /// 当前页码
        /// </summary>
        protected internal int pageindex = DYRequest.getRequestInt("page", 1);
        /// <summary>
        /// 当前列表每页显示记录数
        /// </summary>
        protected internal int pagesize = DYRequest.getRequestInt("pagesize") < 1 ? 14 : DYRequest.getRequestInt("pagesize", 14);

        /// <summary>
        /// 当前城市分站名
        /// </summary>
        protected internal string cityname = "";

        /// <summary>
        /// 当前微信用户openid
        /// </summary>
        protected internal string openid = DYRequest.getRequest("openid");

        /// <summary>
        /// 上级用户id
        /// </summary>
        protected internal int puid = DYRequest.getRequestInt("puid", 0);

        /// <summary>
        /// 当前用户分销等级
        /// </summary>
        protected internal int dlevel = DYRequest.getRequestInt("dlevel", 0);

        /// <summary>
        /// 当前模板路径
        /// </summary>
        protected internal string skinPath;
        

        /// <summary>
        /// WebPage类构造函数
        /// </summary>
        public WebPage()
        {
            #region 如果IP访问列表有设置则进行判断
            if (config.Web_allowIp.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Web_allowIp, "\n");
                if (!Utils.InIPArray(DYRequest.GetIP(), regctrl))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<br /><br /><div style=\"width:100%\" align=\"center\"><div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\">");
                    sb.Append("<img src=\"/static/images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" />&nbsp; 您的IP地址不在系统允许的范围之内</div></div>");
                    Context.Response.Write(sb.ToString());
                    Context.Response.End();
                    return;
                }
            }
            #endregion

            #region 分站城市名
            if (config.Is_hotcity)
                cityname = CityStation.GetCityStationName();
            #endregion
            
            oluserinfo = OnlineUsers.UpdateInfo(BaseConfig.WebEncrypt);

            if (oluserinfo.Userid <= 0 && DYRequest.GetPageName() == "user.aspx")
            {
                string url="";
                if (puid > 0)
                {
                    url = "/login.aspx?act=reg&puid=" + puid + "&dlevel=" + dlevel;
                }
                else
                    url = config.EnableHtml ? "/html/login/login.html" : "/login/login" + config.UrlRewriterKzm;
                Context.Response.Redirect(url);
                return;
            }

            userid = oluserinfo.Userid;
            username = oluserinfo.Username;
            password = oluserinfo.Password;

            if (!string.IsNullOrEmpty(cityname) && config.Is_hotcity)
            {
                #region 显示城市推广信息
                CityStationInfo citystation = CityStation.GetCityStation();
                if (string.IsNullOrEmpty(citystation.pagetitle)) 
                {
                    pagetitle = CityStation.ReplaceCityStationName(pagetitle);
                }
                else
                {
                    pagetitle = citystation.pagetitle;
                }

                if (string.IsNullOrEmpty(citystation.pagekeywords))
                {
                    pagekeywords = CityStation.ReplaceCityStationName(pagekeywords);
                }
                else
                {
                    pagekeywords = citystation.pagekeywords;
                }

                if (string.IsNullOrEmpty(citystation.pagedesc))
                {
                    pagedesc = CityStation.ReplaceCityStationName(pagedesc);
                }
                else
                {
                    pagedesc = citystation.pagedesc;
                }
                #endregion
            }
                if (string.IsNullOrEmpty(pagetitle))
                {
                    pagetitle = config.Title;
                }

                if (string.IsNullOrEmpty(pagekeywords))
                {
                    pagekeywords = config.Keywords;
                }

                if (string.IsNullOrEmpty(pagedesc))
                {
                    pagedesc = config.Desc;
                }
        }
        /// <summary>
        /// 输出模板
        /// </summary>
        /// <param name="templateName"></param>
        protected void DisplayTemplate(string templateName)
        {
            this.DisplayTemplate(new Hashtable(), templateName, false);
        }
        /// <summary>
        /// 输出模板
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        protected void DisplayTemplate(IDictionary context, string templateName)
        {
            this.DisplayTemplate(context, templateName, false);
        }
        /// <summary>
        /// 输出模板
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <param name="json"></param>
        protected void DisplayTemplate(IDictionary context, string templateName, bool json)
        {
            context.Add("mstyle", SiteUtils.GetStyle());//手机网站风格
            context.Add("userid", base.userid);
            context.Add("dlevel", dlevel);
            context.Add("puid", puid);
            context.Add("openid", openid);
            context.Add("username", base.username);
            context.Add("isajax", base.isajax);
            context.Add("navid", navid);
            context.Add("page", this.pageindex);
            context.Add("pagetitle", pagetitle);
            context.Add("pagekeywords", pagekeywords);
            context.Add("pagedesc", pagedesc);
            context.Add("admin_userid", SiteUtils.GetCookie("userid"));
            context.Add("uc_ua", GetUcBorwserUA());
            context.Add("cityname", cityname);//分站城市名
            context.Add("tongjiCode", "<script src='http://pw.cnzz.com/c.php?id=" + SiteUtils.ReadFileToCnzz().Split('@')[0] + "&l=2' language='JavaScript' charset='gb2312'></script>");
            this.skinPath = SiteUtils.SkinPath(context);


            #region 加密文件
            context.Add("js", Caches.SiteBundles(this.skinPath, "js"));
            context.Add("css", Caches.SiteBundles(this.skinPath, "css"));
            #endregion

            base.DisplayTemplate(context, templateName, this.skinPath, json);
        }

        #region 获取UC浏览器极速模式
        /// <summary>
        /// 是否打开uc浏览器极速模式
        /// </summary>
        /// <returns></returns>
        public bool GetUcBorwserUA()
        {
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"]))
            {
                string agent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"];
                return agent.Contains("UCWEB") ? true : false;
            }
            else
                return false;
        }
        #endregion



    }
}
