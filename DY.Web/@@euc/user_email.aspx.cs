using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class user_email : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("user_email_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("user_email_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertUserEmailInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("会员关怀");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("会员关怀添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("userlist", SiteBLL.GetUsersAllList("", ""));
                base.DisplayTemplate(context, "users/user_email_add");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("user_email_edit");

                if (ispost)
                {
                    SiteBLL.UpdateUserEmailInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改user_email");

                    base.DisplayMessage("user_email修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetUserEmailInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "users/user_email_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("user_email_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateUserEmailFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改user_email");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("user_email_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateUserEmailFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("user_email_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteUserEmailInfo("email_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除user_email");
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
                this.IsChecked("user_email_del", true);

                //执行删除
                SiteBLL.DeleteUserEmailInfo(base.id);

                //日志记录
                base.AddLog("删除user_email");

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

            this.GetList("users/user_email_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetUserEmailList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("email_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected UserEmailInfo SetEntity()
        {
            UserEmailInfo entity = new UserEmailInfo();

            entity.email_title = DYRequest.getForm("email_title");
            entity.email_content = DYRequest.getForm("email_content");
            //entity.email_state = "准备发送";//DYRequest.getForm("email_state");
            entity.email_users = DYRequest.getForm("email_users");
            entity.date = DateTime.Now;
            entity.email_id = base.id;
            try
            {
                 SiteUtils.SendEmail(entity.email_title, entity.email_content, entity.email_users);
                 entity.email_state = "已发送";
            }
            catch
            {
                entity.email_state = "发送失败";
            }
            return entity;
        }
    }
}


