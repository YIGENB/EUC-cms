/**
 * 功能描述：UserIntegral管理类
 * 创建时间：2010-3-24 下午 15:53:02
 * 最后修改时间：2010-3-24 下午 15:53:02
 * 作者：gudufy
 * 文件标识：ecb2a686-df2b-49e0-9939-81230390920e
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
    public partial class points : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("user_integral_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("user_integral_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertUserIntegralInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加user_integral：" + DYRequest.getForm("title"));

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("user_integral添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "user_integral/user_integral_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("user_integral_edit");

                if (ispost)
                {
                    SiteBLL.UpdateUserIntegralInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改user_integral：" + DYRequest.getForm("title"));

                    base.DisplayMessage("user_integral修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetUserIntegralInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "user_integral/user_integral_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("user_integral_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateUserIntegralFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改user_integral：" + val);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("user_integral_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateUserIntegralFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("user_integral_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteUserIntegralInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除user_integral");
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
                this.IsChecked("user_integral_del", true);

                //执行删除
                SiteBLL.DeleteUserIntegralInfo(base.id);

                //日志记录
                base.AddLog("删除user_integral");

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
            int user_id = DYRequest.getRequestInt("user_id");
            string account_type = DYRequest.getRequest("account_type");
            string filter = "";
            if (user_id>0)
            {
                filter = " and user_id=" + user_id;
            }
            if (!string.IsNullOrEmpty(account_type))
                filter = " and user_id=" + user_id;
            this.GetList("users/user_integral_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetUserIntegralList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected UserIntegralInfo SetEntity()
        {
            UserIntegralInfo entity = new UserIntegralInfo();

            entity.user_id = DYRequest.getFormInt("user_id");
            entity.integral = DYRequest.getFormInt("integral");
            entity.change_time = DYRequest.getFormDateTime("change_time");
            entity.remark = DYRequest.getForm("remark");
            entity.id = base.id;

            return entity;
        }
    }
}


