/**
 * 功能描述：Appstore管理类
 * 创建时间：2014/3/28 15:34:03
 * 最后修改时间：2014/3/28 15:34:03
 * 作者：gudufy
 * 文件标识：11649b38-eb3d-4c1c-9bd7-ee164d268890
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
    public partial class appstore : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("appstore_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("appstore_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertAppstoreInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加appstore");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("appstore添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "appstore/appstore_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("appstore_edit");

                if (ispost)
                {
                    SiteBLL.UpdateAppstoreInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改appstore");

                    base.DisplayMessage("appstore修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetAppstoreInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "appstore/appstore_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("appstore_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateAppstoreFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改appstore");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("appstore_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateAppstoreFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("appstore_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteAppstoreInfo("app_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除appstore");
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
                this.IsChecked("appstore_del", true);

                //执行删除
                SiteBLL.DeleteAppstoreInfo(base.id);

                //日志记录
                base.AddLog("删除appstore");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 查看
            else if (this.act == "look")
            {
                //检测权限
                this.IsChecked("appstore_list");

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetAppstoreInfo(base.id));

                base.DisplayTemplate(context, "appstore/appstore_look");
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";

            this.GetList("appstore/appstore_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetAppstoreList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("app_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected AppstoreInfo SetEntity()
        {
            AppstoreInfo entity = new AppstoreInfo();

            entity.name = DYRequest.getForm("name");
            entity.typename = DYRequest.getForm("typename");
            entity.developers = DYRequest.getForm("developers");
            entity.language = DYRequest.getForm("language");
            entity.des = DYRequest.getForm("des");
            entity.insession = DYRequest.getForm("insession");
            entity.extsession = DYRequest.getForm("extsession");
            entity.stoptime = DYRequest.getForm("stoptime");
            entity.keyword = DYRequest.getForm("keyword");
            entity.parameters_exp = DYRequest.getForm("parameters_exp");
            entity.is_system = DYRequest.getFormBoolean("is_system");
            entity.is_buy = DYRequest.getFormBoolean("is_buy");
            entity.date = DateTime.Now;//DYRequest.getFormDateTime("date");
            entity.photo = DYRequest.getForm("photo");
            entity.app_id = base.id;

            return entity;
        }
    }
}


