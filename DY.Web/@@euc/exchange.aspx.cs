/**
 * 功能描述：Exchange管理类
 * 创建时间：2014/3/31 16:28:02
 * 最后修改时间：2014/3/31 16:28:02
 * 作者：gudufy
 * 文件标识：f349cf79-e5f0-4945-bb6d-5fe1a1f5e5cf
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
    public partial class exchange : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("exchange_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("exchange_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertExchangeInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("生成sncode");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("exchange添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "exchange/exchange_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("exchange_edit");

                if (ispost)
                {
                    SiteBLL.UpdateExchangeInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改exchange");

                    base.DisplayMessage("exchange修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetExchangeInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "exchange/exchange_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("exchange_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateExchangeFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改exchange");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("exchange_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateExchangeFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("exchange_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteExchangeInfo("exchange_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除exchange");
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
                this.IsChecked("exchange_del", true);

                //执行删除
                SiteBLL.DeleteExchangeInfo(base.id);

                //日志记录
                base.AddLog("删除exchange");

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
            string filter = "and activities_id=" + base.atype_id;

            this.GetList("exchange/exchange_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();

            context.Add("list", SiteBLL.GetExchangeList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("exchange_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("aid", base.atype_id);
            context.Add("entityinfo", SiteUtils.GetAwardName(base.atype, base.atype_id));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected ExchangeInfo SetEntity()
        {
            ExchangeInfo entity = new ExchangeInfo();

            entity.sncode = DYRequest.getForm("sncode");
            entity.award_id = DYRequest.getFormInt("award_id");
            entity.activities_id = DYRequest.getFormInt("activities_id");
            entity.state = DYRequest.getFormInt("state");
            entity.user_id = DYRequest.getForm("user_id");
            entity.phone = DYRequest.getForm("phone");
            entity.date = DYRequest.getFormDateTime("date");
            entity.des = DYRequest.getForm("des");
            entity.exchange_id = base.id;

            return entity;
        }
    }
}


