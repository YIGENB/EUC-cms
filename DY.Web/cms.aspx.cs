/**
 * 功能描述：资讯首页
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
using DY.LanguagePack;

namespace DY.Web
{
    public partial class cms : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            navid = "88";

            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");
            string filter = "article_id > 0";
            //filter+= SiteUtils.IsMobileDevice() ? " and is_mobile=1" : "";
            string tlp = "cms";

            CmsCatInfo catinfo = SiteBLL.GetCmsCatInfo(string.Format("urlrewriter='{0}'", code));
            if (catinfo == null)
            {
                catinfo = SiteBLL.GetCmsCatInfo(string.Format("cat_id={0}", Utils.StrToInt(code, 0)));
            }

            if (catinfo != null)
            {

                #region 分页的页码控制

                if (catinfo.page_size != null && catinfo.page_size.Value != 0)
                {
                    pagesize = catinfo.page_size.Value;
                }
                else
                {
                    pagesize = config.TopNumber;
                }
                #endregion

                #region SEO
                if (!string.IsNullOrEmpty(cityname) && config.Is_hotcity)
                {
                    #region 显示城市推广信息
                    CityStationInfo citystation = CityStation.GetCityStation();
                    if (string.IsNullOrEmpty(citystation.pagetitle))
                    {
                        pagetitle = catinfo.cat_name + "-" + CityStation.ReplaceCityStationName(pagetitle);
                    }
                    else
                    {
                        pagetitle = catinfo.cat_name + "-" + citystation.pagetitle;
                    }

                    if (string.IsNullOrEmpty(citystation.pagekeywords))
                    {
                        pagekeywords = catinfo.cat_name + "," + CityStation.ReplaceCityStationName(pagekeywords);
                    }
                    else
                    {
                        pagekeywords = catinfo.cat_name + "," + citystation.pagekeywords;
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
                    if (string.IsNullOrEmpty(catinfo.pagetitle))
                    {
                        pagetitle = catinfo.cat_name + "-" + config.ArticleTitle;
                    }
                    else
                    {
                        pagetitle = catinfo.pagetitle;
                    }

                    if (string.IsNullOrEmpty(catinfo.pagekeywords))
                    {
                        pagekeywords = catinfo.cat_name + "," + config.ArticleKeywords;
                    }
                    else
                    {
                        pagekeywords = catinfo.pagekeywords;
                    }

                    if (string.IsNullOrEmpty(catinfo.pagedesc))
                    {
                        pagedesc = config.ArticleDesc;
                    }
                    else
                    {
                        pagedesc = catinfo.pagedesc;
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(catinfo.list_tlp))
                {
                    tlp = SiteUtils.CheckTlp(tlp, catinfo.list_tlp);
                }

                filter = "cat_id in (" + cms.GetCMSCatIds(catinfo.cat_id.Value) + ")";
                filter += " and is_show=1 and showtime<=getdate()";
                #region 是否手机访问，显示相应列表
                //if (SiteUtils.IsMobileDevice())
                //    filter += " and is_mobile=1";
                #endregion

                int cat_id = catinfo.parent_id > 0 ? catinfo.parent_id.Value : catinfo.cat_id.Value;
                //航id
                switch (cat_id)
                {
                    case 32: navid = "36"; break;
                    case 53: navid = "2"; break;
                    case 3: navid = "77"; break;
                }
                #region 获取分类第一条内容信息
                if (SiteBLL.GetCmsList(base.pageindex, pagesize, SiteUtils.GetSortOrder("is_top desc,sort_order desc,article_id desc,showtime desc"), filter, out base.ResultCount).Count <= 1)
                {
                    int id = 0;
                    string first_id = "";
                    if (cms.GetCMSCatIds(catinfo.cat_id.Value).Contains(","))
                    {
                        first_id = cms.GetCMSCatIds(catinfo.cat_id.Value).Substring(0, cms.GetCMSCatIds(catinfo.cat_id.Value).IndexOf(','));
                        id = Utils.StrToInt(first_id, 0);
                    }
                    else
                        id = Utils.StrToInt(code, 0);
                    CmsInfo cmsinfo = SiteBLL.GetCmsInfo("cat_id=" + id);
                    string body = "";
                    body = systemConfig.CreateLiskTextFr(SiteBLL.GetCmsValue("content", "cat_id=" + id).ToString());
                    body = systemConfig.CreateDescPage(body);
                    context.Add("cmsinfo", cmsinfo);
                    context.Add("content", body);
                    //更新访问统计
                    //SiteBLL.UpdateCmsFieldValue("click_count", Convert.ToInt32(cmsinfo.click_count.Value) + 1, Convert.ToInt16(cmsinfo.article_id.Value));
                }
                else
                    context.Add("content", "");
                #endregion
                //filter += " and is_show=1 and showtime<=getdate()";
                context.Add("cat_id", cat_id);
                context.Add("cat_name", SiteBLL.GetCmsCatValue("cat_name", "cat_id=" + cat_id));
                context.Add("catinfo", catinfo);
                context.Add("catnav", Caches.CmsNav(catinfo.cat_id.Value, ""));
                context.Add("this_id", catinfo.cat_id.Value);
                context.Add("pre_cat_id", CMS.GetPrevCMSCat(cat_id).parent_id);
                context.Add("list", SiteBLL.GetCmsList(base.pageindex, pagesize, SiteUtils.GetSortOrder("sort_order desc,showtime desc,is_top desc,article_id desc"), filter, out base.ResultCount));
                context.Add("countPage", (base.ResultCount - 1) / pagesize + 1);
                context.Add("pagesize", pagesize);

                //string suffix = config.EnableHtml ? ".html" : ".aspx";
                string url = string.IsNullOrEmpty(code) ? urlrewrite.article + "p" : urlrewrite.article + code + "/p";

                context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, pagesize, base.pageindex, url, config.UrlRewriterKzm, 6));


                base.DisplayTemplate(context, tlp);
            }
            else
            {
                Response.StatusCode = 404;
                Server.Execute("/html/404.aspx");
                Server.ClearError();
            }
        }
    }
}
