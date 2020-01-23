/**
 * 功能描述：网站地图生成类
 * 创建时间：2010-1-29 15:01:01
 * 最后修改时间：2010-1-29 15:01:01
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 文件名：UserGroup.cs
 * ID：bd53d549-7bf2-4d5a-b017-3aa0668f882e
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;
using System.Collections;
using DY.LanguagePack;

namespace DY.Site
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class SiteMap : PageBase
    {
        string header = "<\x3Fxml version=\"1.0\" encoding=\"UTF-8\"\x3F>\n\t<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
        string footer = "\t</urlset>\n";

        //百度移动端xml
        string mheader = "<\x3Fxml version=\"1.0\" encoding=\"UTF-8\"\x3F>\n\t<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:mobile=\"http://www.baidu.com/schemas/sitemap-mobile/1/\">";
        string mfooter = "\t</urlset>\n";

        DataTable dt;

        public SiteMap()
        {
            dt = new DataTable();
            dt.Columns.Add("loc", typeof(string));
            dt.Columns.Add("lastmod", typeof(string));
            dt.Columns.Add("changefreq", typeof(string));
            dt.Columns.Add("priority", typeof(string));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc">位置</param>
        /// <param name="lastmod">日期格式 YYYY-MM-DD</param>
        /// <param name="changefreq">更新频率的单位 (always, hourly, daily, weekly, monthly, yearly, never)</param>
        /// <param name="priority">更新频率 0-1</param>
        public void add_item(string loc, string lastmod, string changefreq, string priority)
        {
            dt.Rows.Add(new object[] { loc, lastmod, changefreq, priority });
        }

        /// <summary>
        /// 创建sitemap文件
        /// </summary>
        /// <param name="filename">要创建的sitemap文件名</param>
        /// <param name="mobile">百度移动xml</param>
        public string build(string filename, bool mobile)
        {
            try
            {
                string map =mobile?this.mheader:this.header + "\n";

                foreach (DataRow row in dt.Rows)
                {
                    map += "\t\t<url>\n\t\t\t<loc>" + row["loc"] + "</loc>\n";

                    if (mobile)
                        map += "\t\t\t<mobile:mobile/>\n";

                    //lastmod
                    if (!string.IsNullOrEmpty(row["lastmod"].ToString()))
                        map += "\t\t\t<lastmod>" + row["lastmod"] + "</lastmod>\n";

                    //changefreq
                    if (!string.IsNullOrEmpty(row["changefreq"].ToString()))
                        map += "\t\t\t<changefreq>" + row["changefreq"] + "</changefreq>\n";

                    //priority
                    if (!string.IsNullOrEmpty(row["priority"].ToString()))
                        map += "\t\t\t<priority>" + row["priority"] + "</priority>\n";

                    map += "\t\t</url>\n";
                }

                map += mobile?this.mfooter:this.footer + "\n";

                //写入内容
                FileOperate.WriteFile(filename, map);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 自动生成站点地图
        /// </summary>
        public void createSietmap()
        {
            IDictionary context = new Hashtable();

            //生成网站地图
            #region 生成html地图
            context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
            context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
            context.Add("productlist", SiteBLL.GetGoodsAllList("", "is_on_sale=1"));
            context.Add("downloadCats", SiteBLL.GetDownloadCategoryAllList("", ""));
            context.Add("downloadlist", SiteBLL.GetDownloadAllList("", ""));
            context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
            context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
            context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

            string sitemaphtml = base.GetTemplate(context, "systems/sitemap", DY.Config.BaseConfig.AdminSkinPath, false);

            #endregion

            #region 生成txt地图
            context.Clear();
            context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
            context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
            context.Add("productlist", SiteBLL.GetGoodsAllList("", "is_on_sale=1"));
            context.Add("downloadCats", SiteBLL.GetDownloadCategoryAllList("", ""));
            context.Add("downloadlist", SiteBLL.GetDownloadAllList("", ""));
            context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
            context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
            context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

            string txthtml = base.GetTemplate(context, "systems/txt", DY.Config.BaseConfig.AdminSkinPath, false);

            #endregion

            #region 生成sitemapindex地图
            context.Clear();
            context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
            context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
            context.Add("productlist", SiteBLL.GetGoodsAllList("", "is_on_sale=1"));
            context.Add("downloadCats", SiteBLL.GetDownloadCategoryAllList("", ""));
            context.Add("downloadlist", SiteBLL.GetDownloadAllList("", ""));
            context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
            context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
            context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

            string sitemapIndexhtml = base.GetTemplate(context, "systems/sitemapIndex", DY.Config.BaseConfig.AdminSkinPath, false);

            #endregion

            this.createSietmap(sitemaphtml, txthtml, sitemapIndexhtml, config);
        }

        /// <summary>
        /// 自动生成站点地图
        /// </summary>
        /// <param name="htmlTemplate">解析过的html模板</param>
        /// <param name="txtTemplate">解析过的txt模板</param>
        /// <param name="sitemapIndexTemplate">解析过的sitemapIndex模板</param>
        /// <param name="config"></param>
        public void createSietmap(string htmlTemplate, string txtTemplate, string sitemapIndexTemplate, BaseConfigInfo config)
        {
            string filename = System.Web.HttpContext.Current.Server.MapPath("/sitemap.xml");
            string mfilename = System.Web.HttpContext.Current.Server.MapPath("/msitemap.xml");

            IDictionary context = new Hashtable();

            //网站
            add_item(urlrewrite.http + new SiteUtils().GetDomain() + "/", DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.9");
            //商品分类
            foreach (DataRow catinfo in new Caches().GoodsCat().Rows)
            {
                add_item(urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product + (catinfo["urlrewriter"].ToString() == "" ? catinfo["cat_id"] : catinfo["urlrewriter"]) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
            }
            //商品
            foreach (GoodsInfo dr in SiteBLL.GetGoodsAllList("", "is_on_sale=1"))
            {
                add_item(urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + (dr.urlrewriter == "" ? dr.goods_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }
            //文章分类
            foreach (CmsCatInfo catinfo in new CMS().GetCMSCatList(0))
            {
                add_item(urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article + (catinfo.urlrewriter == "" ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
            }
            //文章
            foreach (CmsInfo dr in SiteBLL.GetCmsAllList("", "is_show=1"))
            {
                add_item(urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + (dr.urlrewriter == "" ? dr.article_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }
            //单页面
            foreach (CmsPageInfo dr in SiteBLL.GetCmsPageAllList("", "is_show=1"))
            {
                add_item(urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.page + dr.urlrewriter + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }

            build(filename, false);

            build(mfilename, true);

            #region 生成html地图
            FileOperate.WriteFile(System.Web.HttpContext.Current.Server.MapPath("/sitemap.htm"), htmlTemplate);
            #endregion

            #region 生成txt地图
            FileOperate.WriteFile(System.Web.HttpContext.Current.Server.MapPath("/sitemap.txt"), txtTemplate);
            #endregion

            #region 生成sitemapIndex地图
            FileOperate.WriteFile(System.Web.HttpContext.Current.Server.MapPath("/sitemapindex.xml"), sitemapIndexTemplate);
            #endregion
        }
    }
}
