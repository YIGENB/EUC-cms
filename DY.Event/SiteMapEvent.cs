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
    /// �й�վ���ͼ�ļƻ�����
    /// </summary>
    public class ����վ���ͼ : IEvent
    {
        #region IEvent ��Ա

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

        public ����վ���ͼ()
        {
            dt = new DataTable();
            dt.Columns.Add("loc", typeof(string));
            dt.Columns.Add("lastmod", typeof(string));
            dt.Columns.Add("changefreq", typeof(string));
            dt.Columns.Add("priority", typeof(string));
        }

        /// <summary>
        /// ��ȡ���������ģ������
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <param name="tlpPath"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string GetTemplate(IDictionary context, string templateName, string tlpPath)
        {
            //���߳������޷�ʹ��HttpContext.Current
            INVelocityEngine fileEngine =
                NVelocityEngineFactory.CreateNVelocityFileEngine(System.AppDomain.CurrentDomain.BaseDirectory.ToString()+tlpPath, true);

            BaseConfigInfo config = BaseConfig.Get();
            UrlrewriteConfigInfo urlrewriteinfo = UrlrewriteConfig.Get();
            context.Add("config", config);
            context.Add("urlrewriteinfo", urlrewriteinfo);
            context.Add("html", config.UrlRewriterKzm);//α��̬��׺


            string html = fileEngine.Process(context, templateName + BaseConfig.WebSkinSuffix);

            return html;
        }


        /// <summary>
        /// �Զ�����վ���ͼ
        /// </summary>
        public void createSietmap()
        {
            BaseConfigInfo config = BaseConfig.Get();
            IDictionary context = new Hashtable();


            //������վ��ͼ
            #region ����html��ͼ
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

            #region ����txt��ͼ
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

            #region ����sitemapindex��ͼ
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
        /// �Զ�����վ���ͼ
        /// </summary>
        /// <param name="htmlTemplate">��������htmlģ��</param>
        /// <param name="txtTemplate">��������txtģ��</param>
        /// <param name="sitemapIndexTemplate">��������sitemapIndexģ��</param>
        /// <param name="config"></param>
        public void createSietmap(string htmlTemplate, string txtTemplate, string sitemapIndexTemplate, BaseConfigInfo config)
        {
            #region ����html��ͼ
            FileOperate.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"/sitemap.htm", htmlTemplate);
            #endregion

            #region ����txt��ͼ
            FileOperate.WriteFile( System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"/sitemap.txt", txtTemplate);
            #endregion

            #region ����sitemapIndex��ͼ
            FileOperate.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"/sitemapindex.xml", sitemapIndexTemplate);
            #endregion

            #region ����xml��ͼ
            FileOperate.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sitemapindex.xml", sitemapIndexTemplate);
            #endregion

            createSitemaiXml();

        }

        #region XML��ͼ����
        string header = "<\x3Fxml version=\"1.0\" encoding=\"UTF-8\"\x3F>\n\t<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
        string footer = "\t</urlset>\n";

        //�ٶ��ƶ���xml
        string mheader = "<\x3Fxml version=\"1.0\" encoding=\"UTF-8\"\x3F>\n\t<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:mobile=\"http://www.baidu.com/schemas/sitemap-mobile/1/\">";
        string mfooter = "\t</urlset>\n";

        DataTable dt;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loc">λ��</param>
        /// <param name="lastmod">���ڸ�ʽ YYYY-MM-DD</param>
        /// <param name="changefreq">����Ƶ�ʵĵ�λ (always, hourly, daily, weekly, monthly, yearly, never)</param>
        /// <param name="priority">����Ƶ�� 0-1</param>
        public void add_item(string loc, string lastmod, string changefreq, string priority)
        {
            dt.Rows.Add(new object[] { loc, lastmod, changefreq, priority });
        }

        /// <summary>
        /// ����sitemap�ļ�
        /// </summary>
        /// <param name="filename">Ҫ������sitemap�ļ���</param>
        /// <param name="mobile">�ٶ��ƶ�xml</param>
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

                //д������
                FileOperate.WriteFile(filename, map);

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// ����XML��ͼ
        /// </summary>
        private void createSitemaiXml()
        {
            string filename = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/sitemap.xml";
            string mfilename = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/msitemap.xml";

            ����վ���ͼ sitemap = new ����վ���ͼ();
            BaseConfigInfo config = BaseConfig.Get();
            //��վ
            add_item(urlrewrite.http + config.Sitedomain + "/", DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.9");
            //��Ʒ����
            foreach (GoodsCategoryInfo catinfo in SiteBLL.GetGoodsCategoryAllList("",""))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.product + (catinfo.urlrewriter == "" ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
            }
            //��Ʒ
            foreach (GoodsInfo dr in SiteBLL.GetGoodsAllList("", "is_on_sale=1"))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.product_detail + (dr.urlrewriter == "" ? dr.goods_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }
            //���·���
            foreach (CmsCatInfo catinfo in SiteBLL.GetCmsCatAllList("",""))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.article + (catinfo.urlrewriter == "" ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
            }
            //����
            foreach (CmsInfo dr in SiteBLL.GetCmsAllList("", "is_show=1"))
            {
                add_item(urlrewrite.http + config.Sitedomain + urlrewrite.article_detail + (dr.urlrewriter == "" ? dr.article_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, dr.add_time.Value.ToString("yyyy-MM-dd"), "hourly", "0.8");
            }
            //��ҳ��
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
