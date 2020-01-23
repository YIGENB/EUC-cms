using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Site;
using System.Collections;
using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;
using DY.LanguagePack;
using NVelocityTemplateEngine.Interfaces;
using NVelocityTemplateEngine;
using System.Web;
using System.Data;

namespace DY.Event
{
    /// <summary>
    /// 有关站点地图的计划任务
    /// </summary>
    public class 更新站点地图 : IEvent
    {
        #region IEvent 成员

        void IEvent.Execute(object state)
        {
            try
            {
                this.createSietmap();
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog(e.ToString());
            }
        }

        #endregion

        public 更新站点地图()
        {
            dt = new DataTable();
            dt.Columns.Add("loc", typeof(string));
            dt.Columns.Add("lastmod", typeof(string));
            dt.Columns.Add("changefreq", typeof(string));
            dt.Columns.Add("priority", typeof(string));
        }

        /// <summary>
        /// 获取解析过后的模板数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <param name="tlpPath"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string GetTemplate(IDictionary context, string templateName, string tlpPath)
        {
            //多线程下面无法使用HttpContext.Current
            INVelocityEngine fileEngine =
                NVelocityEngineFactory.CreateNVelocityFileEngine(System.AppDomain.CurrentDomain.BaseDirectory.ToString()+tlpPath, true);

            BaseConfigInfo config = BaseConfig.Get();
            UrlrewriteConfigInfo urlrewriteinfo = UrlrewriteConfig.Get();
            context.Add("config", config);
            context.Add("urlrewriteinfo", urlrewriteinfo);
            context.Add("html", config.UrlRewriterKzm);//伪静态后缀


            string html = fileEngine.Process(context, templateName + BaseConfig.WebSkinSuffix);

            return html;
        }


        /// <summary>
        /// 自动生成站点地图
        /// </summary>
        public void createSietmap()
        {
            BaseConfigInfo config = BaseConfig.Get();
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

            string sitemaphtml = GetTemplate(context, "systems/sitemap", DY.Config.BaseConfig.AdminSkinPath);

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

            string txthtml = GetTemplate(context, "systems/txt", DY.Config.BaseConfig.AdminSkinPath);

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

            string sitemapIndexhtml = GetTemplate(context, "systems/sitemapIndex", DY.Config.BaseConfig.AdminSkinPath);

            #endregion

            createSietmap(sitemaphtml, txthtml, sitemapIndexhtml, config);
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
            #region 生成html地图
            FileOperate.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"/sitemap.htm", htmlTemplate);
            #endregion

            #region 生成txt地图
            FileOperate.WriteFile( System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"/sitemap.txt", txtTemplate);
            #endregion

            #region 生成sitemapIndex地图
            FileOperate.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"/sitemapindex.xml", sitemapIndexTemplate);
            #endregion

            #region 生成xml地图
            FileOperate.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sitemapindex.xml", sitemapIndexTemplate);
            #endregion

            createSitemaiXml();

        }

        #region XML地图处理
        string header = "<\x3Fxml version=\"1.0\" encoding=\"UTF-8\"\x3F>\n\t<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
        string footer = "\t</urlset>\n";

        //百度移动端xml
        string mheader = "<\x3Fxml version=\"1.0\" encoding=\"UTF-8\"\x3F>\n\t<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:mobile=\"http://www.baidu.com/schemas/sitemap-mobile/1/\">";
        string mfooter = "\t</urlset>\n";

        DataTable dt;


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
        public string build(string filename,bool mobile)
        {
            try
            {
                string map = mobile ? this.mheader : this.header + "\n";

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

                map += mobile ? this.mfooter : this.footer + "\n";

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
        /// 生成XML地图
        /// </summary>
        private void createSitemaiXml()
        {
            string filename = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sitemap.xml";
            string mfilename = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/msitemap.xml";

            更新站点地图 sitemap = new 更新站点地图();
            BaseConfigInfo config = BaseConfig.Get();
            //网站
            add_item(urlrewrite.http + config.Sitedomain + "/", DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.9");
            //商品分类
            foreach (GoodsCategoryInfo catinfo in SiteBLL.GetGoodsCategoryAllList("",""))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.product + (catinfo.urlrewriter == "" ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
            }
            //商品
            foreach (GoodsInfo dr in SiteBLL.GetGoodsAllList("", "is_on_sale=1"))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.product_detail + (dr.urlrewriter == "" ? dr.goods_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }
            //文章分类
            foreach (CmsCatInfo catinfo in SiteBLL.GetCmsCatAllList("",""))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.article + (catinfo.urlrewriter == "" ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
            }
            //文章
            foreach (CmsInfo dr in SiteBLL.GetCmsAllList("", "is_show=1"))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.article_detail + (dr.urlrewriter == "" ? dr.article_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }
            //单页面
            foreach (CmsPageInfo dr in SiteBLL.GetCmsPageAllList("", "is_show=1"))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.page + dr.urlrewriter + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }

            build(filename,false);

            build(mfilename, true);

        }

        #endregion
    }
}
