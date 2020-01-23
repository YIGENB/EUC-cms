/**
 * 功能描述：sitemap管理类
 * 创建时间：2010-1-29 12:43:46
 * 最后修改时间：2010-1-29 12:43:46
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

namespace DY.Web.admin
{
    public partial class sitemap : AdminPage
    {
        string filename = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            filename = Server.MapPath("/sitemap.xml");
            string mfilename = System.Web.HttpContext.Current.Server.MapPath("/msitemap.xml");

            //检测权限
            this.IsChecked("sitemap");

            IDictionary context = new Hashtable();
            if (ispost)
            {
                //生成网站地图
                DY.Site.SiteMap sitemap = new DY.Site.SiteMap();
                //网站
                sitemap.add_item(urlrewrite.http + Request.Url.Host + "/", DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.9");
                //商品分类
                foreach (DataRow catinfo in new Caches().GoodsCat().Rows)
                {
                    sitemap.add_item(urlrewrite.http + Request.Url.Host + urlrewrite.product + (catinfo["urlrewriter"].ToString() == "" ? catinfo["cat_id"] : catinfo["urlrewriter"]) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
                }
                //商品
                foreach (GoodsInfo dr in SiteBLL.GetGoodsAllList("", "is_on_sale=1"))
                {
                    sitemap.add_item(urlrewrite.http + Request.Url.Host + urlrewrite.product_detail + (dr.urlrewriter == "" ? dr.goods_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.8");
                }
                //文章分类
                foreach (CmsCatInfo catinfo in cms.GetCMSCatList(0))
                {
                    sitemap.add_item(urlrewrite.http + Request.Url.Host + urlrewrite.article + (catinfo.urlrewriter == "" ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.6");
                }
                //文章
                foreach (CmsInfo dr in SiteBLL.GetCmsAllList("", "is_show=1"))
                {
                    sitemap.add_item(urlrewrite.http + Request.Url.Host + urlrewrite.article_detail + (dr.urlrewriter == "" ? dr.article_id.Value.ToString() : dr.urlrewriter) + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.8");
                }
                //单页面
                foreach (CmsPageInfo dr in SiteBLL.GetCmsPageAllList("", "is_show=1"))
                {
                    sitemap.add_item(urlrewrite.http + Request.Url.Host + urlrewrite.page + dr.urlrewriter + config.UrlRewriterKzm, DateTime.Now.ToString("yyyy-MM-dd"), "hourly", "0.8");
                }
                sitemap.build(filename,false);

                sitemap.build(filename,true);

                #region 生成htm地图
                context.Add("pages", SiteBLL.GetCmsPageAllList("",""));
                context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
                context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));

                string sitemaphtml = base.GetTemplate(context, "systems/sitemap", DY.Config.BaseConfig.AdminSkinPath, false);

                FileOperate.WriteFile(Server.MapPath("/sitemap.htm"), sitemaphtml);
                #endregion

                //日志记录
                base.AddLog("生成网站地图");

                //显示提示信息
                base.DisplayMessage("已经成功更新网站地图内容", 2);
            }

            context.Add("sitemap", FileOperate.IsExist(filename, FileOperate.FsoMethod.File) ? 1 : 0);

            base.DisplayTemplate(context, "systems/sitemap_info");
        }
    }
}
