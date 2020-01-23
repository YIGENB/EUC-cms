/**
 * 功能描述：InternalLinks管理类
 * 创建时间：2010-6-2 15:08:01
 * 最后修改时间：2010-6-2 15:08:01
 * 作者：gudufy
 * 文件标识：8612a35a-da59-4516-a373-ff77fa051c3c
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
    public partial class internal_links : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("internal_links_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("internal_links_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertInternalLinksInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加内部链接");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("内部链接添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "internal_links/internal_links_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("internal_links_edit");

                if (ispost)
                {
                    SiteBLL.UpdateInternalLinksInfo(this.SetEntity());
                    //移除缓存
                    caches.InternalLinkRemove();
                    //日志记录
                    base.AddLog("修改内部链接");

                    base.DisplayMessage("内部链接修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetInternalLinksInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "internal_links/internal_links_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("internal_links_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateInternalLinksFieldValue(fieldName, val, base.id);
                    //移除缓存
                    caches.InternalLinkRemove();
                    //日志记录
                    base.AddLog("修改内部链接");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("internal_links_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateInternalLinksFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }
                    //移除缓存
                    caches.InternalLinkRemove();
                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("internal_links_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteInternalLinksInfo("ID in (" + ids.Remove(ids.Length - 1, 1) + ")");
                        //移除缓存
                        caches.InternalLinkRemove();
                        //日志记录
                        base.AddLog("删除内部链接");
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
                this.IsChecked("internal_links_del", true);

                //执行删除
                SiteBLL.DeleteInternalLinksInfo(base.id);
                //移除缓存
                caches.InternalLinkRemove();
                //日志记录
                base.AddLog("删除内部链接");

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

            this.GetList("internal_links/internal_links_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetInternalLinksList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("ID desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected InternalLinksInfo SetEntity()
        {
            InternalLinksInfo entity = new InternalLinksInfo();

            entity.title = DYRequest.getForm("title");
            entity.link = DYRequest.getForm("link");
            entity.remark = DYRequest.getForm("remark");
            entity.frequency = DYRequest.getFormInt("frequency");
            entity.is_enable = DYRequest.getFormBoolean("is_enable");
            entity.ID = base.id;

            return entity;
        }
    }
}