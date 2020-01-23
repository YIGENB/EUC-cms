/**
 * 功能描述：DepartureDate管理类
 * 创建时间：2010-12-6 9:48:44
 * 最后修改时间：2010-12-6 9:48:44
 * 作者：gudufy
 * 文件标识：68c7d6bb-a7d7-4c8a-9807-ab3596d4314a
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
    public partial class departure_date : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("departure_date_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("departure_date_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertDepartureDateInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加出发日期");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    base.DisplayMessage("修改出发日期成功", 2, "?act=list&travel_id=" + DYRequest.getRequestInt("travel_id"));
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "departure_date/departure_date_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("departure_date_edit");

                if (ispost)
                {
                    SiteBLL.UpdateDepartureDateInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改出发日期");
                    base.DisplayMessage("修改出发日期成功", 2, "?act=list&travel_id=" + DYRequest.getRequestInt("travel_id"));
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDepartureDateInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "departure_date/departure_date_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("departure_date_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateDepartureDateFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改departure_date");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("departure_date_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateDepartureDateFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("departure_date_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteDepartureDateInfo("departure_date_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除departure_date");
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
                this.IsChecked("departure_date_del", true);

                //执行删除
                SiteBLL.DeleteDepartureDateInfo(base.id);

                //日志记录
                base.AddLog("删除departure_date");

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
            if (DYRequest.getRequestInt("travel_id") == 0)
            {
                filter = "";
            }
            else
            {
                filter += " and travel_id=" + DYRequest.getRequestInt("travel_id");
            }
            this.GetList("departure_date/departure_date_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetDepartureDateList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("departure_date_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("isajax", base.isajax);
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("travel_id", DYRequest.getRequest("travel_id"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected DepartureDateInfo SetEntity()
        {
            DepartureDateInfo entity = new DepartureDateInfo();

            entity.travel_id = DYRequest.getRequestInt("travel_id");
            entity.start_time = DYRequest.getFormDateTime("start_time");
            entity.adult_price = DYRequest.getFormDecimal("adult_price");
            entity.child_price = DYRequest.getFormDecimal("child_price");
            entity.include_position = DYRequest.getForm("include_position");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.add_time = string.IsNullOrEmpty(DYRequest.getFormString("add_time")) ? DateTime.Now : DYRequest.getFormDateTime("add_time");
            entity.departure_date_id = base.id;

            return entity;
        }
    }
}


