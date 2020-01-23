/**
 * 功能描述：JobPosition管理类
 * 创建时间：2014/6/25 15:29:15
 * 最后修改时间：2014/6/25 15:29:15
 * 作者：gudufy
 * 文件标识：8034e756-3353-46fd-8396-08cf34ade2a6
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
    public partial class job_position : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("job_position_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("job_position_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertJobPositionInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加职位");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("职位添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("departmentlist", SiteBLL.GetJobDepartmentAllList("department_id desc", ""));

                base.DisplayTemplate(context, "job/job_position_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("job_position_edit");

                if (ispost)
                {
                    SiteBLL.UpdateJobPositionInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改职位");

                    base.DisplayMessage("职位修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("departmentlist", SiteBLL.GetJobDepartmentAllList("department_id desc", ""));
                context.Add("entity", SiteBLL.GetJobPositionInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "job/job_position_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("job_position_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateJobPositionFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改职位");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("job_position_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateJobPositionFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("job_position_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteJobPositionInfo("position_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除职位");
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
                this.IsChecked("job_position_del", true);

                //执行删除
                SiteBLL.DeleteJobPositionInfo(base.id);

                //日志记录
                base.AddLog("删除职位");

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
            string did = DYRequest.getRequest("cat_id");
            string val = DYRequest.getRequest("val");
            if (!string.IsNullOrEmpty(did))
                filter += " and department_id=" + did;
            else if (!string.IsNullOrEmpty(val))
                filter += " and name like '%" + val+"%'";

            this.GetList("job/job_position_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetJobPositionList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("position_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected JobPositionInfo SetEntity()
        {
            JobPositionInfo entity = new JobPositionInfo();

            entity.name = DYRequest.getForm("name");
            entity.number = DYRequest.getFormInt("number");
            entity.content = DYRequest.getForm("content");
            entity.department_id = DYRequest.getFormInt("department_id");
            entity.en_time = DYRequest.getFormDateTime("en_time");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.position_id = base.id;

            return entity;
        }
    }
}


