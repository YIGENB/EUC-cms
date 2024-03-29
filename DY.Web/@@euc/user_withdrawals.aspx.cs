﻿/**
 * 功能描述：UserWithdrawals管理类
 * 创建时间：2015/5/18 13:57:41
 * 最后修改时间：2015/5/18 13:57:41
 * 作者：gudufy
 * 文件标识：acfba656-7019-4ae5-bd18-80843af680a6
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com
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
    public partial class user_withdrawals : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("user_withdrawals_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("user_withdrawals_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertUserWithdrawalsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加提现记录");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("提现记录添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "users/user_withdrawals_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("user_withdrawals_edit");

                if (ispost)
                {
                    SiteBLL.UpdateUserWithdrawalsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改提现记录");

                    base.DisplayMessage("提现记录修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetUserWithdrawalsInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "users/user_withdrawals_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("user_withdrawals_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateUserWithdrawalsFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改提现记录");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("user_withdrawals_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateUserWithdrawalsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("user_withdrawals_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteUserWithdrawalsInfo("withdrawals_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除提现记录");
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
                this.IsChecked("user_withdrawals_del", true);

                //执行删除
                SiteBLL.DeleteUserWithdrawalsInfo(base.id);

                //日志记录
                base.AddLog("删除提现记录");

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

            this.GetList("users/user_withdrawals_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetUserWithdrawalsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("withdrawals_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            context.Add("users", SiteBLL.GetUsersAllList("", "is_enabled=1 and is_validated=1"));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected UserWithdrawalsInfo SetEntity()
        {
            UserWithdrawalsInfo entity = new UserWithdrawalsInfo();

            entity.name = DYRequest.getForm("name");
            entity.blank = DYRequest.getForm("blank");
            entity.money = DYRequest.getFormDecimal("money");
            entity.account = DYRequest.getForm("account");
            entity.date = DYRequest.getFormDateTime("date");
            entity.is_verified = DYRequest.getFormBoolean("is_verified");
            entity.user_id = DYRequest.getFormInt("user_id");
            entity.admin_id = DYRequest.getFormInt("admin_id");
            entity.state = DYRequest.getFormInt("state");
            entity.des = DYRequest.getForm("des");
            entity.admin_user = DYRequest.getForm("admin_user");
            entity.withdrawals_id = base.id;

            return entity;
        }
    }
}


