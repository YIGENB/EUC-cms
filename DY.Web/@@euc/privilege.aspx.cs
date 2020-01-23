/**
 * 功能描述：AdminUser管理类
 * 创建时间：2010-1-29 12:55:05
 * 最后修改时间：2010-1-29 12:55:05
 * 作者：gudufy
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
    public partial class privilege : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //权限检测
                this.IsChecked("privilege_manager_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //权限检测
                this.IsChecked("privilege_manager_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加管理员");

                    int IDENTITY = SiteBLL.InsertAdminUserInfo(this.SetEntity());

                    if (this.SetEntity().action_list == "all")
                    {
                        //显示提示信息
                        base.DisplayMessage("管理员添加成功", 2, "?act=list");
                        return;
                    }
                    //显示提示信息
                    base.DisplayMessage("管理员添加成功，下面将转到权限分配界面", 2, "?act=allot&id=" + IDENTITY + "&user=" + this.SetEntity().user_name);
                }

                base.DisplayTemplate("privileges/privilege_add");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                if (base.userid != base.id)
                {
                    //权限检测
                    this.IsChecked("privilege_manager_edit");
                }

                if (ispost)
                {
                    AdminUserInfo userinfo = SiteBLL.GetAdminUserInfo(base.id);
                    string old_pwd = DYRequest.getForm("old_password");
                    if (userinfo != null)
                    {
                        if (userinfo.password == SiteUtils.Encryption(old_pwd))
                        {
                            //日志记录
                            base.AddLog("修改管理员");

                            SiteBLL.UpdateAdminUserInfo(this.SetEntity());

                            //显示提示信息
                            base.DisplayMessage("用户信息修改成功", 2, "?act=list");
                        }
                        else if (string.IsNullOrEmpty(old_pwd))
                        {
                            //显示提示信息
                            base.DisplayMessage("用户密码未更改", 2, "?act=list");
                        }
                        else
                            //显示提示信息
                            base.DisplayMessage("原始密码错误", 1, "?act=list");
                    }
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetAdminUserInfo(base.id));

                base.DisplayTemplate(context, "privileges/privilege_edit");
            }
            #endregion

            #region 分派权限
            else if (this.act == "allot")
            {
                //检测权限
                this.IsChecked("privilege_manager_allot");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("给管理员分派权限");

                    SiteBLL.UpdateAdminUserFieldValue("action_list", string.IsNullOrEmpty(DYRequest.getForm("action_code[]")) ? "" : DYRequest.getForm("action_code[]"), base.id);

                    //显示提示信息
                    base.DisplayMessage("权限信息分配完成", 2, "?act=list");
                }

                AdminUserInfo userinfo = SiteBLL.GetAdminUserInfo(base.id);

                if (userinfo.action_list == "all")
                    base.DisplayMessage("您不能修改该管理员的权限信息", 1);

                if (DYRequest.getRequestInt("id") == base.userid)
                    base.DisplayMessage("您不能修改自己的权限信息", 1);

                IDictionary context = new Hashtable();
                context.Add("entity", userinfo);

                base.DisplayTemplate(context, "privileges/privilege_allot");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("privilege_manager_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改管理员");

                    //执行修改
                    SiteBLL.UpdateAdminUserFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("privilege_manager_del", true);

                //日志记录
                base.AddLog("删除管理员");

                //执行删除
                SiteBLL.DeleteAdminUserInfo(base.id);

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
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetAdminUserList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("user_id desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));

            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "privileges/privilege_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected AdminUserInfo SetEntity()
        {
            AdminUserInfo userinfo = SiteBLL.GetAdminUserInfo(base.id);
            if (userinfo != null)
            {
                AdminUserInfo entity = new AdminUserInfo();
                entity.user_name = DYRequest.getForm("user_name");
                entity.email = DYRequest.getForm("email");
                if (!string.IsNullOrEmpty(DYRequest.getForm("old_password")))
                    entity.password = SiteUtils.Encryption(DYRequest.getForm("password"));
                else
                    entity.password = userinfo.password;
                entity.add_time = userinfo.add_time;
                entity.last_login = userinfo.last_login;
                entity.action_list = userinfo.action_list;
                entity.last_ip = userinfo.last_ip;
                entity.nav_list = userinfo.nav_list;
                entity.lang_type = userinfo.lang_type;
                entity.todolist = userinfo.todolist;
                entity.agency_id = userinfo.agency_id;
                entity.user_id = base.id;

                return entity;
            }
            else
            {
                AdminUserInfo entity = new AdminUserInfo();
                entity.user_name = DYRequest.getForm("user_name");
                entity.email = DYRequest.getForm("email");
                entity.password = SiteUtils.Encryption(DYRequest.getForm("password"));
                entity.add_time = DateTime.Now;
                entity.last_login = Convert.ToDateTime("1949-10-01");
                entity.action_list = DYRequest.getForm("action_list");
                entity.last_ip = "";
                entity.nav_list = "";
                entity.lang_type = "";
                entity.todolist = "";
                entity.agency_id = 0;
                entity.user_id = base.id;

                return entity;
            }
        }
    }
}
