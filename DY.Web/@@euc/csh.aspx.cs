using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.IO;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Text;

namespace DY.Web.admin
{
    public partial class csh : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("csh_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("csh_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertCshInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加客服");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("客服添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("cshtypelist", SiteBLL.GetCshTypeAllList("type_id desc", ""));
                base.DisplayTemplate(context, "csh/csh_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("csh_edit");

                if (ispost)
                {
                    SiteBLL.UpdateCshInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改客服");

                    base.DisplayMessage("客服修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetCshInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("cshtypelist", SiteBLL.GetCshTypeAllList("type_id desc", ""));
                base.DisplayTemplate(context, "csh/csh_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("csh_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateCshFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改客服");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("csh_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateCshFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("csh_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteCshInfo("csh_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除客服");
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
                this.IsChecked("csh_del", true);

                //执行删除
                SiteBLL.DeleteCshInfo(base.id);

                //日志记录
                base.AddLog("删除客服");

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
            int pid = DYRequest.getRequestInt("cat_id", 0);
            if (pid != 0)
                filter += " and csh_type=" + pid;
            if (!string.IsNullOrEmpty(DYRequest.getRequest("val")))
                filter += " and csh_title like '%" + DYRequest.getRequest("val") + "%'";

            this.GetList("csh/csh_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetCshList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("csh_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("cshtypelist", SiteBLL.GetCshTypeAllList("type_id desc", ""));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected CshInfo SetEntity()
        {
            CshInfo entity = new CshInfo();

            entity.csh_title = DYRequest.getForm("csh_title");
            entity.csh_con = DYRequest.getForm("csh_con");
            entity.csh_pic = DYRequest.getForm("csh_pic");
            entity.csh_type = DYRequest.getFormInt("csh_type");
            entity.csh_order = DYRequest.getFormInt("csh_order");
            entity.csh_id = base.id;

            return entity;
        }
    }
}