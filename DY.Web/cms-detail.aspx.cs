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

namespace DY.Web
{
    public partial class cms_detail : WebPage
    {
        protected string body = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            navid = "88";

            string tlp = "cms-detail";
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");

            int id = 0;
            id = Utils.StrToInt(code, 0);
            DataTable cmsinfo = new DataTable();
            if (id > 0)
            {
                cmsinfo = CMS.GetCMSFullInfo(Utils.StrToInt(code, 0));
            }
            else
            {
                cmsinfo = CMS.GetCMSFullInfo(code);
            }

            if (cmsinfo.Rows.Count > 0)
            {
                DataRow dr = cmsinfo.Rows[0];
                if (!string.IsNullOrEmpty(cityname) && config.Is_hotcity)
                {
                    #region 显示城市推广信息
                    CityStationInfo citystation = CityStation.GetCityStation();
                    if (string.IsNullOrEmpty(citystation.pagetitle))
                    {
                        pagetitle = dr["title"] + "-" + CityStation.ReplaceCityStationName(pagetitle);
                    }
                    else
                    {
                        pagetitle = dr["title"] + "-" + citystation.pagetitle;
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
                        pagedesc = string.IsNullOrEmpty(dr["content"].ToString()) ? config.ArticleDesc : SiteUtils.GetDes(dr["content"].ToString(), 100);
                    }
                    #endregion
                }
                else
                {
                    if (string.IsNullOrEmpty(dr["pagetitle"].ToString()))
                    {
                        pagetitle = dr["title"] + "-" + config.ArticleTitle;
                    }
                    else
                    {
                        pagetitle = dr["pagetitle"].ToString();
                    }

                    if (string.IsNullOrEmpty(dr["pagekeywords"].ToString()))
                    {
                        pagekeywords = config.ArticleKeywords;
                    }
                    else
                    {
                        pagekeywords = dr["pagekeywords"].ToString();
                    }

                    if (string.IsNullOrEmpty(dr["pagedesc"].ToString()))
                    {
                        pagedesc = string.IsNullOrEmpty(dr["des"].ToString()) ? string.IsNullOrEmpty(dr["content"].ToString()) ? config.ArticleDesc : SiteUtils.GetDes(dr["content"].ToString(), 100) : dr["des"].ToString();
                    }
                    else
                    {
                        pagedesc = dr["pagedesc"].ToString();
                    }
                }
                //获取资讯分类详细页模板
                CmsCatInfo catinfo = SiteBLL.GetCmsCatInfo(string.Format("cat_id={0}", Convert.ToInt32(dr["cat_id"])));
                if (catinfo != null)
                {
                    string cms_template_detail = catinfo.info_tlp.Trim().ToString();
                    if (!string.IsNullOrEmpty(cms_template_detail))
                    {
                        tlp = cms_template_detail;
                    }
                }
                if (!string.IsNullOrEmpty(dr["info_tlp"].ToString()))
                {
                    tlp = SiteUtils.CheckTlp(tlp, dr["info_tlp"].ToString());
                }
                if (dr["content"] != null)
                {
                    body = systemConfig.CreateLiskTextFr(dr["content"].ToString());
                    body = systemConfig.CreateDescPage(body);
                }
                
                int this_id = Convert.ToInt32(dr["cat_id"]);
                int cat_id = catinfo.parent_id > 0 ? catinfo.parent_id.Value : catinfo.cat_id.Value;
                //航id
                switch (cat_id)
                {
                    case 32: navid = "36"; break;
                    case 53: navid = "2"; break;
                    case 3: navid = "77"; break;

                }

                context.Add("this_id", this_id);
                context.Add("cat_id", cat_id);

                context.Add("catnav", Caches.CmsNav(Convert.ToInt32(dr["cat_id"]), ""));
                context.Add("cat_name", SiteBLL.GetCmsCatValue("cat_name", "cat_id=" + cat_id));
                context.Add("encat_name", SiteBLL.GetCmsCatValue("en_cat_name", "cat_id=" + cat_id));
                context.Add("body", body);
                context.Add("cmsinfo", dr);
                context.Add("comment_type", 2);
                context.Add("id_value", dr[0]);
                context.Add("catinfo", catinfo);


                //更新访问统计
                SiteBLL.UpdateCmsFieldValue("click_count", Convert.ToInt32(dr["click_count"]) + 1, Convert.ToInt16(dr[0]));

                base.DisplayTemplate(context, tlp);
            }
            else  //页面不存在
            {
                Response.StatusCode = 404;
                Server.Execute("/html/404.aspx");
                Server.ClearError();
            }
        }

        public int GetCat(int cat_id)
        {
            int parent_id = Convert.ToInt32(SiteBLL.GetCmsCatValue("parent_id", "cat_id=" + cat_id));
            if (parent_id > 0)
            {
                return GetCat(parent_id);
            }
            return cat_id;
        }
    }
}
