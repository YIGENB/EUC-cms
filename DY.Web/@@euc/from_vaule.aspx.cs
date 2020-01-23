using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;


namespace DY.Web.admin
{
    public partial class from_vaule : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("fromvalue_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("fromvalue_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertFromvalueInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加fromvalue");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("fromvalue添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "fromvalue/fromvalue_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("fromvalue_edit");

                if (ispost)
                {
                    SiteBLL.UpdateFromvalueInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改fromvalue");

                    base.DisplayMessage("fromvalue修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetFromvalueInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "fromvalue/fromvalue_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("fromvalue_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateFromvalueFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改fromvalue");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("fromvalue_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateFromvalueFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("fromvalue_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteFromvalueInfo("fv_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除fromvalue");
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
                this.IsChecked("fromvalue_del", true);

                //执行删除
                //SiteBLL.DeleteFromvalueInfo(base.id);
                SiteBLL.DeleteFromvalueInfo("session_id='" + DYRequest.getRequest("session_id") + "'");

                //日志记录
                base.AddLog("删除万能表单值");

                base.DisplayMessage("删除万能表单值成功", 2, "?act=list&position_id=" + DYRequest.getRequest("position_id"));
                //显示列表数据
                //this.GetList();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";
            if (!string.IsNullOrEmpty(DYRequest.getRequest("position_id")))
            {
                filter += "and position_id=" + DYRequest.getRequest("position_id");
            }

            this.GetList("fromvalue/fromvalue_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            //插入数据库
            //int count=SiteBLL.GetFromvalueAllList(SiteUtils.GetSortOrder("fv_id asc"), SiteUtils.GetFilter(context) + filter).Count/SiteBLL.GetFormAllAllList("", "parent_id=" + DYRequest.getRequest("position_id")).Count;
            //base.ResultCount=count;
            
            context.Add("list", SiteBLL.GetFromvalueList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("fv_id asc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            //context.Add("list", SiteBLL.GetFromvalueAllList(SiteUtils.GetSortOrder("fv_id asc"), SiteUtils.GetFilter(context) + filter));

            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("position_id", DYRequest.getRequest("position_id"));
            context.Add("namelist", SiteBLL.GetFormAllAllList("", "parent_id=" + DYRequest.getRequest("position_id")));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected FromvalueInfo SetEntity()
        {
            FromvalueInfo entity = new FromvalueInfo();

            entity.allform_id = DYRequest.getFormInt("allform_id");
            entity.position_id = DYRequest.getFormInt("position_id");
            entity.session_id = DYRequest.getForm("session_id");
            entity.value = DYRequest.getForm("value");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.isshow = DYRequest.getFormBoolean("isshow");
            entity.is_best = DYRequest.getFormBoolean("is_best");
            entity.fv_id = base.id;

            return entity;
        }
    }
}