using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Site;
using DY.Config;

namespace DY.Event
{
    /// <summary>
    /// �й�վ���ͼ�ļƻ�����
    /// </summary>
    public class ɾ��֩���¼ : IEvent
    {
        #region IEvent ��Ա

        public void Execute(object state)
        {
            string sql = "delete from " + BaseConfig.TablePrefix + "robots where datediff(day,date,getdate())>15";
            SystemConfig.SqlProcess(sql);
            //EventLogs.WriteFailedLog("�ɹ�");
        }

        #endregion
    }
}
