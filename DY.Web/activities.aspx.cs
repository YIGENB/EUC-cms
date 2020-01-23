/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
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
using System.Collections.Generic;

namespace DY.Web
{
    public partial class activities : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tlp = "activities/";
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");
            string aid = DYRequest.getRequest("aid");
            int atype = 0;
            if (!string.IsNullOrEmpty(code))
            {
                CardInfo entity = SiteBLL.GetCardInfo("is_enabled=1 and card_id=" + aid);
                if (entity != null)
                {
                    if (entity.start_time.Value >= DateTime.Now)
                    {
                        Response.Write("<h1>活动还没开始</h1>");
                        return;
                    }
                    else if (DateTime.Now >= entity.end_time.Value)
                    {
                        Response.Write("<h1>活动已经结束</h1>");
                        return;
                    }

                    switch (code)
                    {
                        case "ggk": atype = 0; break;
                        case "dzp": atype = 1; break;
                    }
                    switch (code)
                    {
                        case "ggk": tlp += code; break;
                        case "dzp": tlp += code; break;
                    }
                    if (entity.user_day_count > SumExchangeLog(DYRequest.GetIP()))
                    {

                        //取得1个随机数
                        RandomController rc = new RandomController(1);
                        Random r = new Random();
                        foreach (AwardInfo award in SiteBLL.GetAwardAllList("", "atype=" + atype + " and parent_id=" + aid))
                        {
                            int count = SiteBLL.GetExchangeAllList("", "state=0 and award_id=" + award.award_id.Value).Count;
                            if (count > 0)
                            {
                                //添加奖品id到随机数
                                rc.datas.Add(award.award_id.Value);
                                //添加奖品中奖率
                                rc.weights.Add(Convert.ToUInt16(award.winning_rate.Value));
                            }
                        }

                        #region 默认添加谢谢参与
                        //添加奖品id到随机数
                        rc.datas.Add(0);
                        //添加奖品中奖率
                        rc.weights.Add(80);
                        #endregion
                        int awardid = 0;
                        string sncode = "谢谢参与";
                        int exchangeid = 0;
                        if (rc.ControllerRandomExtract(r) > 0)
                        {
                            awardid = rc.ControllerRandomExtract(r);
                            context.Add("awardinfo", SiteBLL.GetAwardInfo(awardid));
                            AwardInfo award = SiteBLL.GetAwardInfo(awardid);
                            if (award != null)
                                context.Add("awardtype", GetAwardType(award.type));
                            ExchangeInfo exchange = SiteBLL.GetExchangeInfo("state=0 and award_id=" + awardid);
                            if (exchange != null)
                            {
                                sncode = exchange.sncode;
                                exchangeid = exchange.exchange_id.Value;
                            }
                        }

                        context.Add("exchangeid", exchangeid);
                        context.Add("sncode", sncode);
                        context.Add("awardid", awardid);
                    }
                    context.Add("entity", entity);
                    context.Add("award", SiteBLL.GetAwardAllList("", "atype=" + atype + " and parent_id=" + aid));
                    context.Add("count_user_log", SumExchangeLog(DYRequest.GetIP()));
                    context.Add("user_day_count", entity.user_day_count);
                    context.Add("countlog", SumExchangeLog());
                    context.Add("day_users", entity.day_users);

                    //添加记录
                    if (SumExchangeLog(DYRequest.GetIP()) < entity.user_day_count)
                        AddExchangeLog(entity.name, entity.card_id.Value);

                    base.DisplayTemplate(context, tlp, "static/template", false);
                }
                else
                    Response.Write("活动未开启");
            }
            else  //页面不存在
            {
                Response.Write("页面不存在");
            }
        }

        /// <summary>
        /// 匹配奖品
        /// </summary>
        /// <param name="name">奖品名称</param>
        /// <returns></returns>
        protected int GetAwardType(string name)
        {
            int arard = 0;
            switch (name)
            {
                case "一等奖": arard = 1; break;
                case "二等奖": arard = 2; break;
                case "三等奖": arard = 3; break;
                case "四等奖": arard = 4; break;
                case "五等奖": arard = 5; break;
                case "六等奖": arard = 6; break;
            }
            return arard;
        }
        /// <summary>
        /// 添加参与记录日志
        /// </summary>
        /// <param name="activitiesname">活动名称</param>
        /// <param name="card_id">活动ID</param>
        public void AddExchangeLog(string activitiesname, int card_id)
        {
            //删除一个月以前的记录
            string sql = "delete from " + DY.Config.BaseConfig.TablePrefix + "exchange_log where datediff(day,date,getdate())>30";
            SystemConfig.SqlProcess(sql);

            ExchangeLogInfo log = new ExchangeLogInfo();
            log.date = DateTime.Now;
            log.ip = DYRequest.GetIP();
            log.name = activitiesname;
            log.card_id = card_id;

            SiteBLL.InsertExchangeLogInfo(log);
        }
        /// <summary>
        /// 查询参与次数-
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>int</returns>
        public int SumExchangeLog(string ip)
        {
            int errcount = Convert.ToInt32(SiteBLL.GetExchangeLogValue("count(log_id)", string.Format("datediff(day,date,getdate())=0 and ip='{0}'", ip)));

            return errcount;
        }
        /// <summary>
        /// 查询参与次数
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>int</returns>
        public int SumExchangeLog()
        {
            int errcount = Convert.ToInt32(SiteBLL.GetExchangeLogValue("count(log_id)", "datediff(day,date,getdate())=0"));

            return errcount;
        }
    }
}
