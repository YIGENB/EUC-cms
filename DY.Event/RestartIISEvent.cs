using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Common;

namespace DY.Event
{
    /// <summary>
    /// �й�վ��iis�ļƻ�����
    /// ͨ������web.config�ļ���ʽ������IIS���̳أ�ע��iis��web԰���������1,��Ϊ�����������û��ſɵ��ø÷�����
    /// </summary>
    public class ����IIS���� : IEvent
    {
        #region IEvent ��Ա

        public void Execute(object state)
        {
            Utils.RestartIISProcess();
            EventLogs.WriteFailedLog("�Զ�����IIS����");
        }

        #endregion
    }
}
