/**
 * 功能描述：Scheduledevents管理类
 * 创建时间：2016/3/28 11:26:44
 * 最后修改时间：2016/3/28 11:26:44
 * 作者：gudufy
 * 文件标识：56c163aa-06b4-4086-a709-b05cd1469ca8
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
using DY.Config;

namespace DY.Web.admin
{
    public partial class schedule : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("schedule_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("schedule_add");

                if (ispost)
                {

                    this.AddSchedule();

                    //日志记录
                    base.AddLog("添加计划任务");
                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("计划任务添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "schedule/schedule_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("schedule_edit");
                string key = DYRequest.getRequest("key");
                if (ispost)
                {
                    this.UpdateSchedule();

                    //日志记录
                    base.AddLog("修改计划任务");

                    base.DisplayMessage("计划任务修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", this.GetScheduleInfo(key));
                context.Add("oldkey", key);

                base.DisplayTemplate(context, "schedule/schedule_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("schedule_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    //SiteBLL.UpdateScheduledeventsFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改计划任务");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("schedule_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        //SiteBLL.UpdateScheduledeventsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("schedule_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        //SiteBLL.DeleteScheduledeventsInfo("scheduleID in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除计划任务");
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
                this.IsChecked("schedule_del", true);

                //执行删除
                //SiteBLL.DeleteScheduledeventsInfo(base.id);

                //日志记录
                base.AddLog("删除计划任务");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 立即执行
            if (this.act == "exec")
            {
                //检测权限
                this.IsChecked("schedule_list");
                string key = DYRequest.getRequest("key");

                DY.Config.Event[] events = ScheduleConfigs.GetConfig().Events;
                foreach (DY.Config.Event ev in events)
                {
                    if (ev.Key == key)
                    {
                        ((ScheduledEvents.IEvent)Activator.CreateInstance(Type.GetType(ev.ScheduleType))).Execute(HttpContext.Current);
                        ScheduledEvents.Event.SetLastExecuteScheduledEventDateTime(ev.Key, Environment.MachineName, DateTime.Now);
                        break;
                    }
                }
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

            this.GetList("schedule/schedule_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            DataTable dt = new DataTable();
            dt.Columns.Add("key");
            dt.Columns.Add("scheduletype");
            dt.Columns.Add("exetime");
            dt.Columns.Add("lastexecute");
            dt.Columns.Add("issystemevent");
            dt.Columns.Add("enable");
            dt.Columns.Add("boolenable");
            DY.Config.Event[] events = ScheduleConfigs.GetConfig().Events;
            foreach (DY.Config.Event ev in events)
            {
                DataRow dr = dt.NewRow();
                dr["key"] = ev.Key;
                dr["scheduletype"] = ev.ScheduleType;
                if (ev.TimeOfDay != -1)
                {
                    dr["exetime"] = "定时执行:" + (ev.TimeOfDay / 60) + "时" + (ev.TimeOfDay % 60) + "分";
                }
                else
                {
                    dr["exetime"] = "周期执行:" + ev.Minutes + "分钟";
                }
                DateTime lastExecute = ScheduledEvents.Event.GetLastExecuteScheduledEventDateTime(ev.Key, Environment.MachineName);
                if (lastExecute == DateTime.MinValue)
                {
                    dr["lastexecute"] = "从未执行";
                }
                else
                {
                    dr["lastexecute"] = lastExecute.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dr["issystemevent"] = ev.IsSystemEvent ? "系统级" : "非系统级";
                dr["enable"] = ev.Enabled ? "启用" : "禁用";
                dr["boolenable"] = ev.Enabled;
                dt.Rows.Add(dr);
            }

            context.Add("list", dt);

            base.DisplayTemplate(context, tpl, base.isajax);
        }


        /// <summary>
        /// 任务数据日添加
        /// </summary>
        protected void AddSchedule()
        {
            ScheduleConfigInfo sci = ScheduleConfigs.GetConfig();
            foreach (DY.Config.Event ev1 in sci.Events)
            {
                if (ev1.Key == DYRequest.getFormString("key").Trim())
                {
                    //显示提示信息
                    this.DisplayMessage("计划任务名称已经存在", 3, "");
                    return;
                }
            }
            DY.Config.Event ev = new DY.Config.Event();
            ev.Key = DYRequest.getFormString("key");
            ev.Enabled = DYRequest.getFormBoolean("enabled");
            ev.IsSystemEvent = false;
            ev.ScheduleType = DYRequest.getFormString("scheduletype");
            int type = DYRequest.getFormInt("type");
            string timeofday = DYRequest.getForm("timeofday");
            int hour = !string.IsNullOrEmpty(timeofday) ? int.Parse(timeofday.Split(':')[0]) : 0;
            int minute = !string.IsNullOrEmpty(timeofday) ? int.Parse(timeofday.Split(':')[1]) : 0;

            if (type == 1)
            {
                ev.TimeOfDay = hour * 60 + minute;
                ev.Minutes = sci.TimerMinutesInterval;
            }
            else
            {
                ev.Minutes = DYRequest.getFormInt("timeserval");
                ev.TimeOfDay = -1;
            }
            DY.Config.Event[] es = new DY.Config.Event[sci.Events.Length + 1];
            for (int i = 0; i < sci.Events.Length; i++)
            {
                es[i] = sci.Events[i];
            }
            es[es.Length - 1] = ev;
            sci.Events = es;

            ScheduleConfigs.SaveConfig(sci);
        }

        /// <summary>
        /// 任务数据日修改
        /// </summary>
        protected void UpdateSchedule()
        {
            ScheduleConfigInfo sci = ScheduleConfigs.GetConfig();
            //foreach (Ctmon.Config.Event ev1 in sci.Events)
            //{
            //    if (ev1.Key == CtmonRequest.getFormString("key").Trim())
            //    {
            //        //显示提示信息
            //        this.DisplayMessage("计划任务名称已经存在", 3, "");
            //        return;
            //    }
            //}
            foreach (DY.Config.Event ev1 in sci.Events)
            {
                if (ev1.Key == DYRequest.getForm("oldkey"))
                {
                    ev1.Key = DYRequest.getFormString("key").Trim();
                    ev1.ScheduleType = DYRequest.getFormString("scheduletype").Trim();
                    ev1.Enabled = DYRequest.getFormBoolean("enabled");
                    int type = DYRequest.getFormInt("type");
                    string timeofday = DYRequest.getForm("timeofday");
                    int hour = !string.IsNullOrEmpty(timeofday) ? int.Parse(timeofday.Split(':')[0]) : 0;
                    int minute = !string.IsNullOrEmpty(timeofday) ? int.Parse(timeofday.Split(':')[1]) : 0;

                    if (type == 1)
                    {
                        ev1.TimeOfDay = hour * 60 + minute;
                        ev1.Minutes = sci.TimerMinutesInterval;
                    }
                    else
                    {
                        if (DYRequest.getFormInt("timeserval") < sci.TimerMinutesInterval)
                            ev1.Minutes = sci.TimerMinutesInterval;
                        else
                            ev1.Minutes = DYRequest.getFormInt("timeserval");
                        ev1.TimeOfDay = -1;
                    }
                    break;
                }
            }

            ScheduleConfigs.SaveConfig(sci);
        }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="key">任务key</param>
        protected DY.Config.Event GetScheduleInfo(string key)
        {
            ScheduleConfigInfo sci = ScheduleConfigs.GetConfig();
            DY.Config.Event ev = new DY.Config.Event();
            foreach (DY.Config.Event ev1 in sci.Events)
            {
                if (ev1.Key == key)
                {
                    ev.Key = ev1.Key;
                    ev.Enabled = ev1.Enabled;
                    ev.ScheduleType = ev1.ScheduleType;
                    ev.IsSystemEvent = ev1.IsSystemEvent;
                    ev.Minutes = ev1.Minutes;
                    ev.TimeOfDay = ev1.TimeOfDay;
                }
            }
            return ev;
        }
    }
}


