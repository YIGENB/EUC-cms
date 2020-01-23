/**
 * 功能描述：download_category管理类
 * 创建时间：2010-5-20 11:59:22
 * 最后修改时间：2010-5-20 11:59:22
 * 作者：gudufy
 * 文件标识：d4684083-8477-4bbd-93bb-5eb15f468817
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
    public partial class download_category : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("download_category_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("download_category_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertDownloadCategoryInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加下载目录");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("下载目录添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                context.Add("cat_count", Download.GetDownloadCatAllList().Rows);

                base.DisplayTemplate(context, "download/download_category_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("download_category_edit");

                if (ispost)
                {
                    SiteBLL.UpdateDownloadCategoryInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改下载目录");

                    base.DisplayMessage("下载目录修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDownloadCategoryInfo(base.id));
                context.Add("cat_count", Download.GetDownloadCatAllList().Rows);
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "download/download_category_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("download_category_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");


                    #region 添加到导航栏\从导航栏删除
                    if (fieldName == "show_in_nav")
                    {
                        DownloadCategoryInfo catinfo = SiteBLL.GetDownloadCategoryInfo(base.id);
                        string url = string.IsNullOrEmpty(catinfo.urlrewriter) ? catinfo.cat_id.ToString() : catinfo.urlrewriter;
                        url = urlrewrite.download + url + config.UrlRewriterKzm;
                        if (config.EnableHtml)
                            url = urlrewrite.html + url + urlrewrite.html_suffix;

                        MenuManage.AddToNav(url, catinfo.cat_name, Convert.ToInt16(val) == 0 ? false : true);
                    }
                    #endregion

                    //执行修改
                    SiteBLL.UpdateDownloadCategoryFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改下载目录");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("download_category_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateDownloadCategoryFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("download_category_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteDownloadCategoryInfo("cat_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除下载目录");
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
                this.IsChecked("download_category_del", true);

                //执行删除
                SiteBLL.DeleteDownloadCategoryInfo(base.id);

                //日志记录
                base.AddLog("删除下载目录");

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
            string filter = "";
           
            this.GetList("download/download_category_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetDownloadCategoryList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("cat_id asc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected DownloadCategoryInfo SetEntity()
        {
            DownloadCategoryInfo entity = new DownloadCategoryInfo();
            entity.parent_id = DYRequest.getFormInt("parent_id") < 0 ? 0 : DYRequest.getFormInt("parent_id");
            entity.cat_type = DYRequest.getFormInt("cat_type");
            entity.cat_name = DYRequest.getFormString("cat_name");
            entity.keywords = DYRequest.getFormString("keywords");
            entity.cat_desc = DYRequest.getFormString("cat_desc");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.cat_level = DYRequest.getFormInt("cat_level") + 1;
            entity.template_file = DYRequest.getFormString("template_file");
            entity.template_detail_path = DYRequest.getFormString("template_detail_path");
            entity.show_in_nav = DYRequest.getFormBoolean("show_in_nav");
            entity.is_review = DYRequest.getFormBoolean("is_review");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.grade = DYRequest.getFormInt("grade");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.cat_name, 6);
            entity.child_open = DYRequest.getFormBoolean("child_open");
            entity.ico = DYRequest.getFormString("ico");
            entity.article_photo_height = DYRequest.getFormInt("article_photo_height");
            entity.article_photo_width = DYRequest.getFormInt("article_photo_width");
            entity.link_url = DYRequest.getFormString("link_url");
            entity.pagesize = DYRequest.getFormInt("pagesize");
            entity.cat_id = base.id;

            return entity;
        }
    }
}
