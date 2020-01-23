/**
 * 功能描述：FormAll管理类
 * 创建时间：2014/6/30 10:15:46
 * 最后修改时间：2014/6/30 10:15:46
 * 作者：gudufy
 * 文件标识：247f2bf6-e348-48be-8ad9-4ce25e367e0e
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
    public partial class form_all : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("form_all_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("form_all_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertFormAllInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加表单模型");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("表单模型添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "form_all/form_all_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("form_all_edit");

                if (ispost)
                {
                    SiteBLL.UpdateFormAllInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改表单模型");

                    base.DisplayMessage("表单模型修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetFormAllInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "form_all/form_all_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("form_all_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateFormAllFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改表单模型");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("form_all_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateFormAllFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("form_all_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteFormAllInfo("allform_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除表单模型");
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
                this.IsChecked("form_all_del", true);

                //执行删除
                SiteBLL.DeleteFormAllInfo(base.id);

                //日志记录
                base.AddLog("删除表单模型");

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
            if (!string.IsNullOrEmpty(DYRequest.getRequest("val")))
            {
                filter += "and name like '%" + DYRequest.getRequest("val") + "%'";
            }
            if (!string.IsNullOrEmpty(DYRequest.getRequest("position_id")))
            {
                filter += "and parent_id=" + DYRequest.getRequest("position_id");
            }

            this.GetList("form_all/form_all_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetFormAllList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("allform_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("position_id", DYRequest.getRequest("position_id"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected FormAllInfo SetEntity()
        {
            FormAllInfo entity = new FormAllInfo();

            entity.name = DYRequest.getForm("name");
            entity.type = DYRequest.getForm("type");
            entity.tip = DYRequest.getForm("tip");
            entity.size = DYRequest.getFormInt("size");
            entity.store_range = DYRequest.getForm("store_range");
            entity.store_dir = DYRequest.getForm("store_dir");
            entity.value = DYRequest.getForm("value");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.isshow = DYRequest.getFormBoolean("isshow");
            entity.is_validate = DYRequest.getFormBoolean("is_validate");
            entity.is_required = DYRequest.getFormBoolean("is_required");
            entity.parent_id = DYRequest.getFormInt("parent_id");
            entity.class_name = DYRequest.getForm("class_name");
            entity.allform_id = base.id;

            return entity;
        }
    }
}

