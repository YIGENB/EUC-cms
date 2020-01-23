using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Site;
using DY.Config;

namespace DY.Event
{
    /// <summary>
    /// 有关站点地图的计划任务
    /// </summary>
    public class 删除蜘蛛记录 : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            string sql = "delete from " + BaseConfig.TablePrefix + "robots where datediff(day,date,getdate())>15";
            SystemConfig.SqlProcess(sql);
            //EventLogs.WriteFailedLog("成功");
        }

        #endregion
    }
}
