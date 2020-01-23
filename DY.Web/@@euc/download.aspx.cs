/**
 * 功能描述：download管理类
 * 创建时间：2010-5-20 11:59:22
 * 最后修改时间：2010-5-20 11:59:22
 * 作者：gudufy
 * 文件标识：271c9a54-5a4c-4387-b172-3c96e9414a72
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

namespace DY.Web.admin
{
    public partial class download : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("download_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("download_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertDownloadInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加download");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("下载内容添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("cat_count", Download.GetDownloadCatAllList().Rows);
                base.DisplayTemplate(context, "download/download_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("download_edit");

                if (ispost)
                {

                    SiteBLL.UpdateDownloadInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改下载内容");

                    base.DisplayMessage("修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDownloadInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("cat_count", Download.GetDownloadCatAllList().Rows);
                base.DisplayTemplate(context, "download/download_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("download_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateDownloadFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改下载内容");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("download_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateDownloadFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("download_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteDownloadInfo("down_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除下载内容");
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
                this.IsChecked("download_del", true);

                //执行删除
                SiteBLL.DeleteDownloadInfo(base.id);

                //日志记录
                base.AddLog("删除下载内容");

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
            if (DYRequest.getRequestInt("cat_id") > 0)
            {
                Download down = new Download();
                filter = " and cat_id in (" + down.GetDownloadCatIds(DYRequest.getRequestInt("cat_id")) + ")";
            }

            this.GetList("download/download_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetDownloadList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("down_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("type", DYRequest.getRequest("type"));
            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected DownloadInfo SetEntity()
        {
            DownloadInfo entity = new DownloadInfo();

            entity.title = DYRequest.getForm("title");
            entity.keywords = DYRequest.getForm("keywords");
            entity.content = DYRequest.getForm("content");
            entity.is_enable = DYRequest.getFormBoolean("is_enable");
            entity.orderid = DYRequest.getFormInt("orderid");
            entity.url = DYRequest.getForm("url");
            entity.rank = DYRequest.getFormInt("rank");
            entity.author = DYRequest.getForm("author");
            entity.source = DYRequest.getForm("source");
            entity.des = DYRequest.getForm("des");
            entity.photo = DYRequest.getForm("photo");
            entity.filesize = DYRequest.getForm("filesize");
            entity.filename = DYRequest.getForm("filename");
            entity.click_count = DYRequest.getFormInt("click_count");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.title, 7);
            entity.cat_id = DYRequest.getFormInt("cat_id");
            entity.is_top = DYRequest.getFormBoolean("is_top");
            entity.seokeyword = DYRequest.getForm("seokeyword");
            entity.seodesc = DYRequest.getForm("seodesc");
            entity.template_file = DYRequest.getForm("template_file");
            entity.showtime = DYRequest.getFormString("showtime") == "" ? DateTime.Now : DYRequest.getFormDateTime("showtime");
            entity.soft_lan = DYRequest.getForm("soft_lan");
            entity.provider = DYRequest.getForm("provider");
            entity.soft_authorize = DYRequest.getForm("soft_authorize");
            entity.platform = DYRequest.getForm("platform");
            entity.seo_title = DYRequest.getForm("seo_title");
            entity.down_id = base.id;

            return entity;
        }
    }
}

