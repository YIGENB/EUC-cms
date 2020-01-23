using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DY.OAuthV2SDK;
using System.Xml;
using DY.OAuthV2SDK.OAuths;
using DY.Entity;
using DY.Common;
using DY.Site;
using DY.Config;
using DY.LanguagePack;

namespace DY.Web.api
{
    public partial class data : DY.Site.WebPage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           switch (base.act)
            {
                case "incms":
                    if (ispost)
                        InsertCms();
                    break;
                case "selectcmscat":
                        SelectCmsCat();
                    break;
                case "autocms":
                    AutoCMS();
                    break;
           }
        }

        /// <summary>
        /// 采集数据入库
        /// </summary>
        protected void InsertCms()
        {
            string username = DYRequest.getForm("username"), password = DYRequest.getForm("password");

            if (username == BaseConfig.username && password == BaseConfig.password)
            {
                if (!string.IsNullOrEmpty(DYRequest.getForm("title")))
                {
                    if (SiteBLL.ExistsCms(" title='" + DYRequest.getForm("title") + "'"))
                    {

                        //显示提示信息
                        base.DisplayMemoryTemplate(base.MakeJson("", 2, "失败，文章添加失败,标题重复存在！"));
                        return;
                    }
                }

                CmsInfo cmsinfo = this.SetEntity();
                int cms_id = SiteBLL.InsertCmsInfo(cmsinfo);
                //采集数据不加入搜索库
                //Search.ChangeSearch(cmsinfo.title, SiteUtils.NoHTML(cmsinfo.des), SiteUtils.NoHTML(cmsinfo.content), cmsinfo.tag, cmsinfo.photo, 2, cmsinfo.click_count.Value, cms_id);
                base.DisplayMemoryTemplate(base.MakeJson("", 2, "成功"));
            }
            else
                base.DisplayMemoryTemplate(base.MakeJson("", 2, "失败，无权限操作！"));

        }

        /// <summary>
        /// 查询咨询分类
        /// </summary>
        protected void SelectCmsCat()
        {
            string username = DYRequest.getRequest("username"), password = DYRequest.getRequest("password");
            string goodsLink="";

            if (username == BaseConfig.username && password == BaseConfig.password)
            {
                foreach (System.Data.DataRow row in caches.CMSCatFormat(0).Rows)
                {
                    goodsLink += "<option(*)value='"+row["cat_id"] +"'(*)>"+row["cat_name"]+"</option>";
                }
                base.DisplayMemoryTemplate(goodsLink);
          }
           else
                base.DisplayMemoryTemplate(base.MakeJson("", 2, "失败，无权限操作！"));

        }


        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected CmsInfo SetEntity()
        {
            CmsInfo entity = new CmsInfo();

            entity.cat_id = DYRequest.getFormInt("cat_id");
            entity.title = DYRequest.getForm("title");
            entity.fu_title = "";//DYRequest.getForm("fu_title");
            entity.title_style = "";
            entity.content = DYRequest.getForm("content");
            entity.author = new SiteUtils().DefaultValue(DYRequest.getForm("author"), "易优创");
            entity.source = new SiteUtils().DefaultValue(DYRequest.getForm("source"), "易优创");
            entity.editor = new SiteUtils().DefaultValue(DYRequest.getForm("editor"), "eyouc");
            entity.is_newopen = false;
            entity.is_show = false;
            entity.is_top = true;
            entity.is_best = false;
            entity.info_tlp = "";//DYRequest.getForm("info_tlp");
            entity.tag = ""; //FunctionUtils.Text.ToDBC(DYRequest.getForm("tag"));
            entity.link = new SiteUtils().DefaultValue(DYRequest.getForm("link"),"");
            entity.photo = "";//DYRequest.getForm("photo");
            entity.des = string.IsNullOrEmpty(DYRequest.getForm("des")) ? new SiteUtils().GetTopic(Utils.RemoveHtml(DYRequest.getForm("content")), 250).Replace("...", "") : DYRequest.getForm("des");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.title, 2);
            entity.add_time = string.IsNullOrEmpty(DYRequest.getFormString("add_time")) ? DateTime.Now : DYRequest.getFormDateTime("add_time");
            entity.showtime = string.IsNullOrEmpty(DYRequest.getFormString("showtime")) ? DateTime.Now : DYRequest.getFormDateTime("showtime");
            entity.click_count = DYRequest.getFormInt("click_count");
            entity.pagetitle = new SiteUtils().DefaultValue(DYRequest.getForm("pagetitle"), "");
            entity.pagekeywords = "";//FunctionUtils.Text.ToDBC(DYRequest.getForm("pagekeywords"));
            entity.pagedesc = new SiteUtils().DefaultValue(DYRequest.getForm("pagedesc"), "");
            entity.sort_order = 0;
            entity.is_hot = false;
            entity.is_mobile = true;
            entity.is_oauth = false;
            entity.cms_type = 0;
            entity.down_id = "";
            entity.article_id = base.id;

            return entity;
        }


        /// <summary>
        /// 采集文章自动发布（采集的数据默认不显示，自动执行文章时间改为当天。每次更新文章为3篇）
        /// </summary>
        protected void AutoCMS()
        {
            string domain = new SiteUtils().GetDomain();
            DY.ScheduledEvents.EventLogs.WriteFailedLog(DYRequest.GetIP());
            if (DYRequest.GetIP() == "127.0.0.1" || domain.Contains("localhost") || domain.Contains("127.0.0.1") || domain.Contains("eyouc.com"))
            {

                BaseConfigInfo config = BaseConfig.Get();
                int ResultCount = 0;
                //查询未显示的文章
                foreach (CmsInfo cms in SiteBLL.GetCmsList(1, config.Autocms_num, "sort_order desc,newid()", "is_show=0", out ResultCount))
                {
                    cms.is_show = true;
                    cms.is_best = true;
                    cms.urlrewriter = "";
                    cms.showtime = DateTime.Now;
                    cms.photo = string.IsNullOrEmpty(cms.photo) ? SiteUtils.GetContentImgUrl(cms.content) : cms.photo;
                    cms.pagekeywords = FunctionUtils.Text.ToDBC(SiteUtils.GetRelateKeyword(cms.title, cms.content)).Replace("重庆,", "重庆网站建设,").Replace("做网站,", "重庆做网站,");
                    cms.tag = cms.pagekeywords;

                    SiteBLL.UpdateCmsInfo(cms);

                    //加入搜索库
                    Search.ChangeSearch(cms.title, SiteUtils.NoHTML(cms.des), SiteUtils.NoHTML(cms.content), cms.tag, cms.photo, 2, cms.click_count.Value, cms.article_id.Value);

                    //添加标签到标签库
                    CMS.SaveTag(cms.tag.Split(','));

                    #region 发送微博
                    string title = SiteUtils.GetKeyToWeibo(FunctionUtils.Text.ToDBC(cms.tag).Split(','), FunctionUtils.Text.ToDBC(cms.pagekeywords).Split(',')) + "【" + cms.title + "】";
                    string des = title + " " + SiteUtils.GetDes(cms.des, cms.content);
                    string photo = string.IsNullOrEmpty(cms.photo) ? SiteUtils.GetContentImgUrl(cms.content) : cms.photo;
                    if (config.Is_oauth)
                    {
                        DY.ScheduledEvents.EventLogs.WriteFailedLog("发送微博：" + SiteUtils.SendWeibo(new SiteUtils().GetTopic(des, 160) + urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + cms.article_id + config.UrlRewriterKzm, System.AppDomain.CurrentDomain.BaseDirectory.ToString() + photo));

                        DY.ScheduledEvents.EventLogs.WriteFailedLog("发送文章（头条）：" + SiteUtils.SendArticle(cms.title, SiteUtils.GetDes(cms.des, cms.content), cms.content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                    }
                    #endregion

                    #region 同步到博客
                    DY.ScheduledEvents.EventLogs.WriteFailedLog("同步博客：" + SiteUtils.SendWeblog(cms.title, cms.content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                    #endregion
                }
                base.DisplayMemoryTemplate(base.MakeJson("ok", 1, "执行成功"));
            }
        }
        
    }
}
