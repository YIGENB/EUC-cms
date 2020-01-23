/**
 * 功能描述：AdminMenu管理类
 * 创建时间：2014/3/19 17:29:22
 * 最后修改时间：2014/3/19 17:29:22
 * 作者：gudufy
 * 文件标识：4873a9e5-5e4b-4af1-b4f5-71c9d79daa4f
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
    public partial class admin_menu : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("admin_menu_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 搜索
            if (this.act == "sreach")
            {
                //检测权限
                this.IsChecked("admin_menu_sreach");

                //显示列表数据
                this.GetSreachList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("admin_menu_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertAdminMenuInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加admin_menu");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("admin_menu添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "admin_menu/admin_menu_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("admin_menu_edit");

                if (ispost)
                {
                    SiteBLL.UpdateAdminMenuInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改admin_menu");

                    base.DisplayMessage("admin_menu修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetAdminMenuInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "admin_menu/admin_menu_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("admin_menu_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateAdminMenuFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改admin_menu");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("admin_menu_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateAdminMenuFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("admin_menu_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteAdminMenuInfo("menu_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除admin_menu");
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
                this.IsChecked("admin_menu_del", true);

                //执行删除
                SiteBLL.DeleteAdminMenuInfo(base.id);

                //日志记录
                base.AddLog("删除admin_menu");

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

            this.GetList("admin_menu/admin_menu_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetSreachList()
        {
            string title = DYRequest.getForm("top-search");
            string filter = "name like '%" + title + "%'";

            this.GetList("admin_menu/admin_menu_sreach", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("title", DYRequest.getForm("top-search"));
            context.Add("list", SiteBLL.GetAdminMenuAllList("", filter));

            //context.Add("list", caches.AdminMenuFormat(0).Rows);
            //context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            ////to json
            //context.Add("sort_by", DYRequest.getRequest("sort_by"));
            //context.Add("sort_order", DYRequest.getRequest("sort_order"));
            //context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected AdminMenuInfo SetEntity()
        {
            AdminMenuInfo entity = new AdminMenuInfo();

            entity.parent_id = DYRequest.getFormInt("parent_id") < 0 ? 0 : DYRequest.getFormInt("parent_id");
            entity.name = DYRequest.getForm("name");
            entity.link = DYRequest.getForm("link");
            entity.type = DYRequest.getFormInt("type");
            entity.isshow = DYRequest.getFormBoolean("isshow");
            entity.menu_id = base.id;

            return entity;
        }
    }
}


