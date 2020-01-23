/**
 * 功能描述：main
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
using DY.Config;
using DY.LanguagePack;

namespace DY.Web.admin
{
    public partial class make_html : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //检测权限 
            this.IsChecked("make_html");
            if (ispost)
            {
                //产品
                if (Request.Form["goods_cat"] != null)
                {

                    foreach (GoodsInfo dr in SiteBLL.GetGoodsAllList("", "cat_id in (" + DYRequest.getForm("goods_cat") + ")"))
                    {

                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.goods_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/goods-detail.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html+urlrewrite.product_detail + id + urlrewrite.html_suffix));

                    }
                }

                //产品分类
                if (Request.Form["goods_cat"] != null)
                {
                    foreach (GoodsCategoryInfo dr in SiteBLL.GetGoodsCategoryAllList("", "cat_id in (" + DYRequest.getForm("goods_cat") + ")"))
                    {
                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.cat_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/goods.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html + urlrewrite.product + id + urlrewrite.html_suffix));

                    }
                }


                //新闻
                if (Request.Form["cms_cat"] != null)
                {

                    foreach (CmsInfo dr in SiteBLL.GetCmsAllList("", "cat_id in (" + DYRequest.getForm("cms_cat") + ")"))
                    {
                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.article_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms-detail.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html + urlrewrite.article_detail + id + urlrewrite.html_suffix));
                    }

                }

                //新闻分类
                if (Request.Form["cms_cat"] != null)
                {

                    foreach (CmsCatInfo dr in SiteBLL.GetCmsCatAllList("", "cat_id in (" + DYRequest.getForm("cms_cat") + ")"))
                    {
                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.cat_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html + urlrewrite.article + id + urlrewrite.html_suffix));
                    }

                }

                //页面
                if (Request.Form["page_id"] != null)
                {
                    foreach (CmsPageInfo dr in SiteBLL.GetCmsPageAllList("", "page_id in (" + DYRequest.getForm("page_id") + ")"))
                    {
                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.page_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/page.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html + urlrewrite.page + id + urlrewrite.html_suffix));
                    }
                }

                //下载分类
                if (Request.Form["download_cat"] != null)
                {

                    foreach (DownloadCategoryInfo dr in SiteBLL.GetDownloadCategoryAllList("", "cat_id in (" + DYRequest.getForm("download_cat") + ")"))
                    {

                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.cat_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/download.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html + urlrewrite.download + id + urlrewrite.html_suffix));

                    }
                }

                //下载
                if (Request.Form["download_cat"] != null)
                {
                    int ddd = SiteBLL.GetDownloadAllList("", "cat_id in (" + DYRequest.getForm("download_cat") + ")").Count;
                    foreach (DownloadInfo dr in SiteBLL.GetDownloadAllList("", "cat_id in (" + DYRequest.getForm("download_cat") + ")"))
                    {
                        string id = dr.urlrewriter;
                        if (string.IsNullOrEmpty(id)) { id = dr.down_id.ToString(); }

                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/download-detail.aspx?code=" + id,
                        Server.MapPath(urlrewrite.html + urlrewrite.download_detail + id + urlrewrite.html_suffix));

                    }
                }

                //首页
                if (DYRequest.getForm("is_index") == "true")
                {

                    SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/default.aspx",
                            Server.MapPath(urlrewrite.html_index + urlrewrite.html_suffix));
                }
                else
                {
                    FileOperate.Delete(Server.MapPath(urlrewrite.html_index + urlrewrite.html_suffix), FileOperate.FsoMethod.File);
                }


                //brand
                //if (Request.Form["brand"] != null)
                //{
                //    SiteUtils.MakeHtml("http://" + siteUtils.GetDomain() + "/brand.aspx",
                //    Server.MapPath("/html/brand.html"));

                //    foreach (GoodsBrandInfo dr in SiteBLL.GetGoodsBrandAllList("", ""))
                //    {
                //        string id = dr.brand_id.ToString();

                //        SiteUtils.MakeHtml("http://" + siteUtils.GetDomain() + "/brand-detail.aspx?code=" + id,
                //        Server.MapPath("/html/brand/detail/" + id + ".html"));
                //    }
                //}

                 //在线咨询
                if (Request.Form["feedback"] != null)
                {
                    SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/feedback.aspx",
                    Server.MapPath("/html/feedback" + urlrewrite.html_suffix));
                }


                //login
                if (Request.Form["login"] != null)
                {
                    foreach (string id in Request.Form["login"].Split(','))
                    {
                        SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/login.aspx?act=" + id,
                        Server.MapPath("/html/login/" + id + urlrewrite.html_suffix));
                    }
                }

                base.DisplayMessage("已经成功生成静态文件。", 2);
            }


            IDictionary context = new Hashtable();

            Caches cach = new Caches();

            context.Add("goodscat", cach.GoodsCat());
            context.Add("cmscat", cach.CMSCat());
            context.Add("downloadcat", cach.DownloadcatTable());
            context.Add("pages", SiteBLL.GetCmsPageAllList("", ""));
            context.Add("haveindex", FileOperate.IsExist(Server.MapPath(urlrewrite.html_index + urlrewrite.html_suffix), FileOperate.FsoMethod.File));

            base.DisplayTemplate(context, "systems/make_html");
        }
    }
}
