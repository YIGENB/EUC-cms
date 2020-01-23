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
    /// 有关文章发布的计划任务
    /// </summary>
    public class 采集文章发布 : IEvent
    {
        #region IEvent 成员

        void IEvent.Execute(object state)
        {
            try
            {
                this.AutoCMS();
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog(e.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 采集文章自动发布（采集的数据默认不显示，自动执行文章时间改为当天。每次更新文章为3篇）
        /// </summary>
        private void AutoCMS()
        {
            BaseConfigInfo config = BaseConfig.Get();
                if (config.Is_oauth)
                {
                    EventLogs.WriteFailedLog(CommonUtils.LoadURLString(urlrewrite.http+config.Sitedomain+"/api/data.aspx?act=autocms"));
                }
            }
    }
}
