using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Common;

namespace DY.Event
{
    /// <summary>
    /// 有关站点iis的计划任务
    /// 通过更新web.config文件方式来重启IIS进程池（注：iis中web园数量须大于1,且为非虚拟主机用户才可调用该方法）
    /// </summary>
    public class 重启IIS进程 : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            Utils.RestartIISProcess();
            EventLogs.WriteFailedLog("自动重启IIS进程");
        }

        #endregion
    }
}
