using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Site;
using System.Collections;
using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;
using DY.LanguagePack;
using NVelocityTemplateEngine.Interfaces;
using NVelocityTemplateEngine;
using System.Web;
using System.Data;

namespace DY.Event
{
    /// <summary>
    /// 有关自定义错误页的计划任务
    /// </summary>
    public class 更新自定义错误页: IEvent
    {
        #region IEvent 成员

        void IEvent.Execute(object state)
        {
            try
            {
                this.UpdateError();
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog(e.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 更新自定义错误页
        /// </summary>
        private void UpdateError()
        {
            SiteUtils.MakeHtml(urlrewrite.http + new SiteUtils().GetDomain() + "/error.aspx?act=404",
            System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/html/404.aspx");

            SiteUtils.MakeHtml(urlrewrite.http + new SiteUtils().GetDomain() + "/error.aspx?act=500",
            System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "/html/500.aspx");
        }
    }
}
