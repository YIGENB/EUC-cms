/**
 * 功能描述：UserSign管理类
 * 创建时间：2015/5/13 15:42:38
 * 最后修改时间：2015/5/13 15:42:38
 * 作者：gudufy
 * 文件标识：3ad2604c-9a4f-4074-8b53-5807eb34e2f9
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
    public partial class user_sign : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("user_sign_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("user_sign_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertUserSignInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加积分记录");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("积分记录添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "users/user_sign_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("user_sign_edit");

                if (ispost)
                {
                    SiteBLL.UpdateUserSignInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改积分记录");

                    base.DisplayMessage("积分记录修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetUserSignInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "users/user_sign_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("user_sign_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateUserSignFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改积分记录");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("user_sign_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateUserSignFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("user_sign_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteUserSignInfo("sign_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除积分记录");
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
                this.IsChecked("user_sign_del", true);

                //执行删除
                SiteBLL.DeleteUserSignInfo(base.id);

                //日志记录
                base.AddLog("删除积分记录");

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
            string points_type = DYRequest.getRequest("points_type");
            string filter = "";
            if (user_id > 0)
            {
                filter = " and userid=" + user_id;
            }
            if (!string.IsNullOrEmpty(points_type))
                filter = " and points_type=" + points_type;

            this.GetList("users/user_sign_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetUserSignList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("sign_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("points_type", DYRequest.getRequest("points_type"));
            context.Add("user_id", DYRequest.getRequestInt("user_id"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected UserSignInfo SetEntity()
        {
            UserSignInfo entity = new UserSignInfo();

            entity.userid = DYRequest.getFormInt("userid");
            entity.date = DYRequest.getFormDateTime("date");
            entity.ip = DYRequest.getForm("ip");
            entity.des = DYRequest.getForm("des");
            entity.change = DYRequest.getFormInt("change");
            entity.points_type = DYRequest.getFormInt("points_type");
            entity.sign_id = base.id;

            return entity;
        }
    }
}


