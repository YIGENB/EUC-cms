/**
 * 功能描述：Users管理类
 * 创建时间：2010/1/30 14:28:51
 * 最后修改时间：2010/1/30 14:28:51
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
using System.Text;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class users : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("users_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("users_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加会员");

                    base.id = SiteBLL.InsertUsersInfo(this.SetEntity());
                    if (base.id > 0)
                    {
                        this.SaveRegFields(base.id);
                    }

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("用户添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("userFields", SiteUser.GetUserRegFields(base.id).Rows);

                base.DisplayTemplate(context, "users/user_add");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("users_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改会员");

                    //执行修改
                    SiteBLL.UpdateUsersInfo(this.SetEntity());

                    //保存注册项信息
                    this.SaveRegFields(base.id);

                    //显示提示信息
                    base.DisplayMessage("用户修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetUsersInfo(base.id));
                context.Add("userFields", SiteUser.GetUserRegFields(base.id).Rows);

                base.DisplayTemplate(context, "users/user_edit");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("users_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改会员");

                    //执行修改
                    SiteBLL.UpdateUsersFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("users_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("修改会员");

                        //执行修改
                        SiteBLL.UpdateUsersFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("users_del", true);

                //日志记录
                base.AddLog("删除会员");

                //执行删除
                SiteBLL.DeleteUsersInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("users_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除会员");

                        //执行删除
                        SiteBLL.DeleteUsersInfo("user_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region JSON搜索会员
            else if (base.act == "search_users")
            {
                SearchUsers();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = " and user_id > 0";
            if (DYRequest.getRequestInt("is_enabled", -1) >= 0)
                filter += " and is_enabled=" + DYRequest.getRequestInt("is_enabled");

            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetUsersList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("user_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("type", DYRequest.getRequest("type"));

            base.DisplayTemplate(context, "users/user_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected UsersInfo SetEntity()
        {
            UsersInfo entity = new UsersInfo();
            if (base.id > 0)
            {
                if (!string.IsNullOrEmpty(DYRequest.getFormString("password")))
                    entity.password = SiteUtils.Encryption(DYRequest.getFormString("password"));
            }
            else
            {
                entity.user_name = DYRequest.getFormString("user_name");
                entity.password = SiteUtils.Encryption(DYRequest.getFormString("password"));
                entity.reg_time = DateTime.Now;
                entity.last_login = DYRequest.getFormDateTime("last_login");
                entity.last_ip = "";
                entity.is_validated = false;
                entity.is_enabled = true;
                entity.user_photo = "";
            }
            entity.remarks = DYRequest.getFormString("remarks");
            entity.user_rank = DYRequest.getFormInt("user_rank");
            entity.sex = DYRequest.getFormInt("sex");
            if (!string.IsNullOrEmpty(DYRequest.getForm("birthday")))
                entity.birthday = DYRequest.getFormDateTime("birthday");
            entity.email = DYRequest.getFormString("email");
            entity.is_validated = DYRequest.getFormBoolean("is_validated");
            entity.is_enabled = DYRequest.getFormBoolean("is_enabled");
            entity.user_id = base.id;

            return entity;
        }
        /// <summary>
        /// JSON搜索
        /// </summary>
        protected void SearchUsers()
        {
            string filter = "", q = DYRequest.getRequest("q");
            if (!string.IsNullOrEmpty(q))
                filter += "user_name like '%" + q + "%'";

            StringBuilder sb = new StringBuilder();
            foreach (UsersInfo dr in SiteBLL.GetUsersAllList("", filter))
            {
                sb.Append("{\"user_id\":\""+ dr.user_id +"\",\"user_name\":\""+ dr.user_name +"\"},");
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + sb.ToString() + "]}");
        }
        /// <summary>
        /// 保存注册项信息
        /// </summary>
        /// <param name="user_id"></param>
        protected void SaveRegFields(int user_id)
        {
            if (Request.Form["reg_field_id"] != null)
            {
                string[] ids = Request.Form.GetValues("reg_field_id");
                for (int i = 0; i < ids.Length; i++)
                {
                    string val = DYRequest.getForm("userFields[" + ids[i] + "]");

                    if (!string.IsNullOrEmpty(val))
                    {
                        RegFieldsValueInfo valinfo = new RegFieldsValueInfo(0, user_id, Convert.ToInt16(ids[i]), val);

                        if (!SiteBLL.ExistsRegFieldsValue("user_id=" + user_id + " and reg_field_id=" + ids[i] + ""))
                            SiteBLL.InsertRegFieldsValueInfo(valinfo);
                    }
                }
            }
        }
    }
}
