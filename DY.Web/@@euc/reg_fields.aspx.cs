/**
 * 功能描述：RegFields管理类
 * 创建时间：2010-3-17 上午 10:08:20
 * 最后修改时间：2010-3-17 上午 10:08:20
 * 作者：gudufy
 * 文件标识：7ab445f9-d609-4462-9570-33fba9480e47
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
    public partial class reg_fields : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("reg_fields_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("reg_fields_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertRegFieldsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加会员注册项：" + DYRequest.getForm("title"));

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("会员注册项添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "reg_fields/reg_fields_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("reg_fields_edit");

                if (ispost)
                {
                    SiteBLL.UpdateRegFieldsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改会员注册项：" + DYRequest.getForm("title"));

                    base.DisplayMessage("会员注册项修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetRegFieldsInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "reg_fields/reg_fields_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("reg_fields_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateRegFieldsFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改会员注册项：" + val);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("reg_fields_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateRegFieldsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("reg_fields_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteRegFieldsInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除会员注册项");
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
                this.IsChecked("reg_fields_del", true);

                //执行删除
                SiteBLL.DeleteRegFieldsInfo(base.id);

                //日志记录
                base.AddLog("删除会员注册项");

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

            this.GetList("reg_fields/reg_fields_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetRegFieldsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("cat_id", DYRequest.getRequestInt("cat_id"));

            base.DisplayTemplate(context, "reg_fields/reg_fields_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected RegFieldsInfo SetEntity()
        {
            RegFieldsInfo entity = new RegFieldsInfo();

            entity.reg_field_name = DYRequest.getForm("reg_field_name");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.type = DYRequest.getFormInt("type");
            entity.is_need = DYRequest.getFormBoolean("is_need");
            entity.id = base.id;

            return entity;
        }
    }
}
