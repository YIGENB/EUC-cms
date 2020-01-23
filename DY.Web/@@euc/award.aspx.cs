/**
 * 功能描述：Award管理类
 * 创建时间：2014/3/31 11:45:32
 * 最后修改时间：2014/3/31 11:45:32
 * 作者：gudufy
 * 文件标识：b4a4a657-aa9c-4c50-8154-09d92accacdb
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
    public partial class award : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("award_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("award_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertAwardInfo(this.SetEntity());

                    #region 插入奖品明细
                    //获取奖品个数
                    string count = DYRequest.getForm("count");
                    for (int i = 0; i < Convert.ToInt32(count); i++)
                    {
                        //插入明细
                        SiteBLL.InsertExchangeInfo(this.SetExchangeEntity());
                    }
                    #endregion

                    //日志记录
                    base.AddLog("添加奖品");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&amp;&aid=" + base.atype_id + "&atype=" + base.atype);

                    //显示提示信息
                    this.DisplayMessage("奖品添加成功", 2, "?act=list&amp;&aid=" + base.atype_id + "&atype=" + base.atype, links);
                }

                IDictionary context = new Hashtable();
                context.Add("aid", base.atype_id);
                base.DisplayTemplate(context, "award/award_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("award_edit");

                if (ispost)
                {
                    SiteBLL.UpdateAwardInfo(this.SetEntity());
                    //#region 插入奖品明细
                    ////获取奖品个数
                    //string count = DYRequest.getForm("count");
                    //for (int i = 0; i < Convert.ToInt32(count); i++)
                    //{
                    //    //插入明细
                    //    SiteBLL.InsertExchangeInfo(this.SetExchangeEntity());
                    //}
                    //#endregion
                    //日志记录
                    base.AddLog("修改奖品");

                    base.DisplayMessage("奖品修改成功", 2, "?act=list&amp;&aid=" + base.atype_id+"&atype="+base.atype);
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetAwardInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("aid", base.atype_id);
                base.DisplayTemplate(context, "award/award_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("award_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateAwardFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改奖品");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("award_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateAwardFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("award_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteAwardInfo("award_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除奖品");
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
                this.IsChecked("award_del", true);

                //执行删除
                SiteBLL.DeleteAwardInfo(base.id);

                //日志记录
                base.AddLog("删除奖品");

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
            string filter = "and parent_id=" + base.atype_id + " and atype="+base.atype;

            this.GetList("award/award_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            
            context.Add("list", SiteBLL.GetAwardList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("award_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected AwardInfo SetEntity()
        {
            AwardInfo entity = new AwardInfo();

            entity.name = DYRequest.getForm("name");
            entity.type = DYRequest.getForm("type");
            entity.count = DYRequest.getForm("count");
            entity.des = DYRequest.getForm("des");
            entity.winning_rate = DYRequest.getFormInt("winning_rate");
            entity.parent_id = base.atype_id;
            entity.atype = base.atype;
            entity.award_id = base.id;

            return entity;
        }

        /// <summary>
        /// 给实体赋值(奖品明细)
        /// </summary>
        protected ExchangeInfo SetExchangeEntity()
        {
            ExchangeInfo entity = new ExchangeInfo();

            entity.sncode = DateTime.Now.ToString("MMddhhmmss") + SiteUtils.GetRandomString(6).ToUpper();
            entity.state = 0;
            entity.activities_id = base.atype_id;
            entity.award_id = base.id;
            entity.atype = base.atype;
            entity.des = "";

            return entity;
        }
    }
}


