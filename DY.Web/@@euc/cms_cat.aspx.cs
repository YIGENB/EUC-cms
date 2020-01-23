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
    public partial class cms_cat : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("cms_cat_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                

                //检测权限
                this.IsChecked("cms_cat_add");

                if (ispost)
                {
                    CmsCatInfo cmscatinfo = this.SetEntity();
                    CMS.InsertCategory(cmscatinfo);
                    //移除资讯缓存
                    caches.CMSCatRemove();

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        base.id = SiteBLL.GetCmsCatInfo("cat_id=(select IDENT_CURRENT('" + DY.Config.BaseConfig.TablePrefix + "cms_cat'))").cat_id.Value;

                        if (base.id > 0)
                        {
                            string id = cmscatinfo.urlrewriter;
                            if (id == "") { id = base.id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms.aspx?code=" + id,
                                    Server.MapPath(urlrewrite.html + urlrewrite.article + id + urlrewrite.html_suffix));
                        }
                    }

                    //日志记录
                    base.AddLog("添加文章分类");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("资讯分类添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("cms/cms_cat_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("cms_cat_edit");

                if (ispost)
                {
                    CmsCatInfo cmscatinfo = this.SetEntity();
                    CMS.UpdateCategory(cmscatinfo);

                    //移除资讯缓存
                    caches.CMSCatRemove();

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (base.id > 0)
                        {
                            string id = cmscatinfo.urlrewriter;
                            if (id == "") { id = base.id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms.aspx?code=" + id,
                                    Server.MapPath(urlrewrite.html + urlrewrite.article + id + urlrewrite.html_suffix));
                        }
                    }

                    //日志记录
                    base.AddLog("修改文章分类");

                    //显示提示信息
                    base.DisplayMessage("资讯分类修改成功", 2, "?act=list");
                }
                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetCmsCatInfo(base.id));

                base.DisplayTemplate(context, "cms/cms_cat_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("cms_cat_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    #region 添加到导航栏\从导航栏删除
                    if (fieldName == "show_in_nav")
                    {
                        CmsCatInfo catinfo = SiteBLL.GetCmsCatInfo(base.id);
                        string url = string.IsNullOrEmpty(catinfo.urlrewriter) ? catinfo.cat_id.ToString() : catinfo.urlrewriter;
                        url = urlrewrite.article + url + config.UrlRewriterKzm;
                        if (config.EnableHtml)
                            url = urlrewrite.html + url + urlrewrite.html_suffix;

                        MenuManage.AddToNav(url, catinfo.cat_name, Convert.ToInt16(val) == 0 ? false : true);
                    }
                    #endregion

                    //执行修改
                    SiteBLL.UpdateCmsCatFieldValue(fieldName, val, base.id);
                    //移除资讯缓存
                    caches.CMSCatRemove();
                    //日志记录
                    base.AddLog("更新文章分类");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("cms_cat_del", true);

                //日志记录
                base.AddLog("删除文章分类");

                //执行删除
                CMS.DeleteCategory(base.id);

                //移除资讯缓存
                caches.CMSCatRemove();
                //显示列表数据
                this.GetList();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            IDictionary context = new Hashtable();
            context.Add("list", caches.CMSCatFormat(0).Rows);
            // context.Add("isajax", base.isajax);

            base.DisplayTemplate(context, "cms/cms_cat_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected CmsCatInfo SetEntity()
        {
            CmsCatInfo entity = new CmsCatInfo();
            entity.cat_id = DYRequest.getFormInt("cat_id");
            entity.parent_id = DYRequest.getFormInt("parent_id") < 0 ? 0 : DYRequest.getFormInt("parent_id");
            entity.cat_name = DYRequest.getForm("cat_name");
            entity.cat_type = 0;
            entity.show_in_nav = DYRequest.getFormBoolean("show_in_nav");
            entity.list_tlp = DYRequest.getFormString("list_tlp");
            entity.info_tlp = DYRequest.getFormString("info_tlp");
            entity.is_review = DYRequest.getFormBoolean("is_review");
            entity.cat_level = 1;
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.cat_name, 1);
            entity.pagetitle = DYRequest.getForm("pagetitle");
            entity.pagekeywords =FunctionUtils.Text.ToDBC(DYRequest.getForm("pagekeywords"));
            entity.pagedesc = DYRequest.getForm("pagedesc");
            entity.page_size = DYRequest.getFormInt("page_size");
            entity.pic = DYRequest.getForm("pic");
            entity.ms = DYRequest.getForm("ms");
            entity.en_cat_name = DYRequest.getForm("en_cat_name");
            entity.cat_id = base.id;

            return entity;
        }
    }
}


