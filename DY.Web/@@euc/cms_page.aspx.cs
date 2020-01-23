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
    public partial class cms_page : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("cms_page_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                string message = "页面添加成功";
                //检测权限
                this.IsChecked("cms_page_add");

                if (ispost)
                {
                    CmsPageInfo pageinfo = this.SetEntity();
                    base.id = SiteBLL.InsertCmsPageInfo(pageinfo);

                    #region 发送微博
                    string des = string.IsNullOrEmpty(pageinfo.des) ? new SiteUtils().GetTopic(SiteUtils.NoHTML(pageinfo.content), 200) : new SiteUtils().GetTopic(SiteUtils.NoHTML(pageinfo.des), 200);
                    if (config.Is_oauth)
                        base.AddLog("发送微博：" + SiteUtils.SendWeibo(des + urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.page + base.id + config.UrlRewriterKzm, urlrewrite.http + new SiteUtils().GetDomain() + pageinfo.photo));
                    #endregion

                    #region 启用百度ping服务
                    if (config.Is_BaiduPing)
                        message += siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.page + base.id + config.UrlRewriterKzm);
                    #endregion

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (pageinfo != null)
                        {
                            string id = pageinfo.urlrewriter;
                            if (id == "") { id = pageinfo.page_id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms-detail.aspx?code=" + id,
                                    Server.MapPath(urlrewrite.html + urlrewrite.page + id + urlrewrite.html));
                        }
                    }


                    //日志记录
                    base.AddLog("添加页面");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage(message, 2, "?act=list", links);
                }

                base.DisplayTemplate("cms/cms_page_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                string message = "页面修改成功";
                //检测权限
                this.IsChecked("cms_page_edit");

                if (ispost)
                {
                    CmsPageInfo pageinfo = this.SetEntity();
                    SiteBLL.UpdateCmsPageInfo(pageinfo);

                    #region 启用百度ping服务
                    if (config.Is_BaiduPing)
                        message += siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.page + base.id + config.UrlRewriterKzm);
                    #endregion


                    //日志记录
                    base.AddLog("修改页面");

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (pageinfo != null)
                        {
                            string id = pageinfo.urlrewriter;
                            if (id == "") { id = pageinfo.page_id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms-detail.aspx?code=" + id,
                                    Server.MapPath(urlrewrite.html + urlrewrite.page + id + urlrewrite.html_suffix));
                        }
                    }

                    //显示提示信息
                    base.DisplayMessage(message, 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetCmsPageInfo(base.id));

                base.DisplayTemplate(context, "cms/cms_page_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("cms_page_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("更新页面");

                    #region 添加到导航栏\从导航栏删除
                    if (fieldName == "show_in_nav")
                    {
                        CmsPageInfo pageinfo = SiteBLL.GetCmsPageInfo(base.id);
                        string url = string.IsNullOrEmpty(pageinfo.urlrewriter) ? pageinfo.page_id.ToString() : pageinfo.urlrewriter;
                        url = urlrewrite.page + url + config.UrlRewriterKzm;
                        if (config.EnableHtml)
                            url = urlrewrite.html + url + urlrewrite.html_suffix;

                        MenuManage.AddToNav(url, pageinfo.title, Convert.ToInt16(val) == 0 ? false : true);
                    }
                    #endregion

                    //执行修改
                    SiteBLL.UpdateCmsPageFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("cms_page_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("更新页面");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        #region 添加到导航栏\从导航栏删除
                        if (fieldName == "show_in_nav")
                        {
                            AddNav(ids, Convert.ToInt16(val) == 0 ? false : true);
                        }
                        #endregion

                        //执行修改
                        SiteBLL.UpdateCmsPageFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("cms_page_del", true);

                //日志记录
                base.AddLog("删除页面");

                //执行删除
                SiteBLL.DeleteCmsPageInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("cms_page_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除页面");

                        //执行删除
                        SiteBLL.DeleteCmsPageInfo("page_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            IDictionary context = new Hashtable();
            //context.Add("list", SiteBLL.GetCmsPageList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("order_id desc,page_id asc"), "", out base.ResultCount));
            //context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            ////context.Add("isajax", base.isajax);
            ////to json
            //context.Add("sort_by", DYRequest.getRequest("sort_by"));
            //context.Add("sort_order", DYRequest.getRequest("sort_order"));
            //context.Add("page", base.pageindex);
            context.Add("list", caches.CMSPageFormat(0).Rows);
            base.DisplayTemplate(context, "cms/cms_page_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected CmsPageInfo SetEntity()
        {
            CmsPageInfo entity = new CmsPageInfo();

            entity.page_id = DYRequest.getFormInt("page_id");
            entity.parent_id = DYRequest.getFormInt("parent_id") < 0 ? 0 : DYRequest.getFormInt("parent_id");
            entity.title = DYRequest.getFormString("title");
            entity.des = DYRequest.getFormString("des");
            entity.content = DYRequest.getFormString("content");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.show_in_nav = DYRequest.getFormBoolean("show_in_nav");
            entity.tag = FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag"));
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.title, 3);
            entity.add_time = DateTime.Now;
            entity.click_count = DYRequest.getFormInt("click_count");
            entity.pagetitle = DYRequest.getFormString("pagetitle");
            entity.pagekeywords = FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords"));
            entity.pagedesc = DYRequest.getFormString("pagedesc");
            entity.info_tlp = DYRequest.getForm("info_tlp");
            entity.entitle = DYRequest.getFormString("entitle");
            entity.mobile_content = string.IsNullOrEmpty(DYRequest.getFormString("mobile_content")) ? DYRequest.getFormString("content") : DYRequest.getFormString("mobile_content");
            entity.photo = DYRequest.getFormString("photo");
            entity.ad_id = DYRequest.getFormInt("ad_id");
            entity.page_id = base.id;

            return entity;
        }
        protected void AddNav(string ids, object val)
        {
            try
            {
                foreach (string str in ids.Split(','))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        CmsPageInfo pageinfo = SiteBLL.GetCmsPageInfo(Utils.StrToInt(str, 0));
                        if (pageinfo != null)
                        {
                            string url = urlrewrite.page + pageinfo.urlrewriter + config.UrlRewriterKzm;
                            if (config.EnableHtml)
                                url = urlrewrite.html + "/" + pageinfo.urlrewriter + urlrewrite.html_suffix;

                            MenuManage.AddToNav(url, pageinfo.title, Convert.ToBoolean(val));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //输出json数据
                base.DisplayMemoryTemplate(base.MakeJson("", 1, ex.Message));
            }
        }
    }
}


