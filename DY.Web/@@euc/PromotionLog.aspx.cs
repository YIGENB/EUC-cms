/**
 * 功能描述：PromotionLog管理类
 * 创建时间：2010-5-13 12:05:25
 * 最后修改时间：2010-5-13 12:05:25
 * 作者：gudufy
 * 文件标识：ee391224-6bce-42d8-91ad-bcf9fce41957
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
    public partial class promotionLog : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("promotion_log");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("promotion_log", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeletePromotionLogInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除promotion_log");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 统计
            else if (this.act == "tj")
            {
                //检测权限
                this.IsChecked("promotion_log");
                GetTJ();
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("promotion_log");

                //执行删除
                SiteBLL.DeletePromotionLogInfo(base.id);

                //日志记录
                base.AddLog("删除promotion_log");

                //显示列表数据
                this.GetList();
            }
            #endregion
        }

        /// <summary>
        /// 推广统计 
        /// </summary>
        protected void GetTJ()
        {
            int pid = DYRequest.getRequestInt("pid",0);
            if (pid < 1)
            {
               return;
            }
            string filter1 = " pid=" + pid + " and datediff(day,input_time,getdate())=0";
            int daycount = SiteBLL.GetPromotionLogAllList("id desc","id", filter1).Count;

            string filter2 = " pid=" + pid + " and datediff(week,input_time,getdate())=0";
            int weekcount = SiteBLL.GetPromotionLogAllList("id desc", "id", filter2).Count;

            string filter3 = " pid=" + pid + " and datediff(Month,input_time,getdate())=0";
            int monthcount = SiteBLL.GetPromotionLogAllList("id desc", "id", filter3).Count;

            string filter4 = " pid=" + pid;
            int allcount = SiteBLL.GetPromotionLogAllList("id desc", "id", filter4).Count;
 
            //当天统计
            ArrayList dayarray = SiteBLL.GetOrderInfoAllList("order_id desc", "order_amount", " pid=" + pid + " and datediff(day,add_time,getdate())=0");
            int dayamountcount = dayarray.Count;
            decimal dayamount=0.00m;
            foreach (Entity.OrderInfoInfo item in dayarray)
            {
                dayamount += (decimal)item.order_amount;
            }
            //本周统计
            ArrayList weekarray = SiteBLL.GetOrderInfoAllList("order_id desc", "order_amount", " pid=" + pid + " and datediff(week,add_time,getdate())=0");
            int weekamountcount = weekarray.Count;
            decimal weekamount = 0.00m;
            foreach (Entity.OrderInfoInfo item in weekarray)
            {
                weekamount += (decimal)item.order_amount;
            }
            //当月统计
            ArrayList montharray = SiteBLL.GetOrderInfoAllList("order_id desc", "order_amount", " pid=" + pid + " and datediff(month,add_time,getdate())=0");
            int monthamountcount = montharray.Count;
            decimal monthamount = 0.00m;
            foreach (Entity.OrderInfoInfo item in montharray)
            {
                monthamount += (decimal)item.order_amount;
            }
            //所有统计
            ArrayList allarray = SiteBLL.GetOrderInfoAllList("order_id desc", "order_amount", " pid=" + pid);
            int allamountcount = allarray.Count;
            decimal allamount = 0.00m;
            foreach (Entity.OrderInfoInfo item in allarray)
            {
                allamount += (decimal)item.order_amount;
            }

            IDictionary context = new Hashtable();
            context.Add("daycount", daycount);
            context.Add("weekcount", weekcount);
            context.Add("monthcount", monthcount);
            context.Add("allcount", allcount);

            context.Add("dayamountcount", dayamountcount);
            context.Add("weekamountcount", weekamountcount);
            context.Add("monthamountcount", monthamountcount);
            context.Add("allamountcount", allamountcount);

            context.Add("dayamount", dayamount);
            context.Add("weekamount", weekamount);
            context.Add("monthamount", monthamount);
            context.Add("allamount", allamount);

            context.Add("ptid", pid);
            base.DisplayTemplate(context, "promotion/promotion");
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";
            string pid = Request.QueryString["pid"];
            string date = Request.QueryString["date"];

            if (!string.IsNullOrEmpty(pid))
            {
                filter += " and pid=" + pid;
            }

            if (!string.IsNullOrEmpty(date))
            {
                if (date == "1")
                {
                    filter += " and datediff(day,input_time,getdate())=0";
                }
                else if (date == "2")
                {
                    filter += " and datediff(week,input_time,getdate())=0";
                }
                else if (date == "3")
                {
                    filter += " and datediff(Month,input_time,getdate())=0";
                }
            }

            this.GetList("promotion/promotion_log_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetPromotionLogList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected PromotionLogInfo SetEntity()
        {
            PromotionLogInfo entity = new PromotionLogInfo();

            entity.input_time = DYRequest.getFormDateTime("input_time");
            entity.website = DYRequest.getForm("website");
            entity.ip = DYRequest.getForm("ip");
            entity.pid = DYRequest.getFormInt("pid");
            entity.id = base.id;

            return entity;
        }
    }
}


