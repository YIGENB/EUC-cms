/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class page : WebPage
    {
        protected string body = "";
        protected string mbody = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //IDictionary context = new Hashtable();
            //string tlp = "page";
            //string code = DYRequest.getRequest("code");
            //CmsPageInfo pageinfo = SiteBLL.GetCmsPageInfo(string.Format("urlrewriter='{0}'", code));
            //if (code != "")
            //{
            //    tlp = code;
            //}
            //if (pageinfo != null)
            //{
            //    context.Add("pageinfo", pageinfo);
            //    base.DisplayTemplate(context,tlp);
            //}
            //else  //页面不存在
            //{

            //}
            string tlp = "page";
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");

            switch (code)
            {
                //case "about": navid = "42"; break;
                case "lxwm": navid = "43"; break;
                case "jztc": navid = "76"; break;
                case "wyjz": navid = "77"; break;
                case "fwfw": navid = "43"; break;
                case "zhishixueyuan": navid = "2"; break;
            }
            if (Request.QueryString["alid"] != null)
            {
                //跳转url
                string anli_id = DYRequest.getRequest("alid");
                CmsInfo cmsinfo = SiteBLL.GetCmsInfo("article_id=" + anli_id);
                //CmsInfo cmsinfo = SiteBLL.GetCmsInfo(string.Format("cat_id={0}", Utils.StrToInt(anli_id, 0)));
                if (cmsinfo != null)
                {
                    context.Add("url", cmsinfo.fu_title);
                }
            }



            CmsPageInfo pageinfo = SiteBLL.GetCmsPageInfo(string.Format("urlrewriter='{0}'", code));
            if (pageinfo != null)
            {
                if (!string.IsNullOrEmpty(cityname) && config.Is_hotcity)
                {
                    #region 显示城市推广信息
                    CityStationInfo citystation = CityStation.GetCityStation();
                    if (string.IsNullOrEmpty(citystation.pagetitle))
                    {
                        pagetitle = pageinfo.title + "-" + CityStation.ReplaceCityStationName(pagetitle);
                    }
                    else
                    {
                        pagetitle = pageinfo.title + "-" + citystation.pagetitle;
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
                else
                {
                    if (string.IsNullOrEmpty(pageinfo.pagetitle))
                    {
                        pagetitle = pageinfo.title + "-" + config.Title;
                    }
                    else
                    {
                        pagetitle = pageinfo.pagetitle;
                    }

                    if (string.IsNullOrEmpty(pageinfo.pagekeywords))
                    {
                        pagekeywords = config.Keywords;
                    }
                    else
                    {
                        pagekeywords = pageinfo.pagekeywords;
                    }

                    if (string.IsNullOrEmpty(pageinfo.pagedesc))
                    {
                        pagedesc = config.Desc;
                    }
                    else
                    {
                        pagedesc = pageinfo.pagedesc;
                    }
                }
                if (!string.IsNullOrEmpty(pageinfo.info_tlp))
                {
                    tlp = SiteUtils.CheckTlp(tlp, pageinfo.info_tlp);

                }

                if (pageinfo.content != null)
                {
                    body = systemConfig.CreateLiskTextFr(pageinfo.content.ToString());
                    body = systemConfig.CreateDescPage(body);
                }
                if (pageinfo.mobile_content != null)
                {
                    mbody = systemConfig.CreateLiskTextFr(pageinfo.mobile_content.ToString());
                    mbody = systemConfig.CreateDescPage(mbody);
                }


                int cat_id = Caches.PageCatID(pageinfo.parent_id.Value, pageinfo.page_id.Value);//pageinfo.parent_id > 0 ? pageinfo.parent_id.Value : pageinfo.page_id.Value;
                //航id
                switch (cat_id)
                {
                    case 15: navid = "42"; break;
                }

                context.Add("cat_id", cat_id);
                context.Add("this_id", pageinfo.page_id);
                context.Add("cat_name", SiteBLL.GetCmsPageValue("title", "page_id=" + cat_id));
                context.Add("en_cat_name", SiteBLL.GetCmsPageValue("entitle", "page_id=" + cat_id));
                context.Add("body", body);
                context.Add("code", code);
                context.Add("mbody", mbody);
                context.Add("pageinfo", pageinfo);
                context.Add("catnav", Caches.PageNav(pageinfo.page_id.Value, ""));

                

                base.DisplayTemplate(context, tlp);
            }
            else  //页面不存在
            {
                Response.StatusCode = 404;
                Server.Execute("/html/404.aspx");
                Server.ClearError();
            }
        }
    }
}
