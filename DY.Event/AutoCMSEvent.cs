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
    /// �й����·����ļƻ�����
    /// </summary>
    public class �ɼ����·��� : IEvent
    {
        #region IEvent ��Ա

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
        /// �ɼ������Զ��������ɼ�������Ĭ�ϲ���ʾ���Զ�ִ������ʱ���Ϊ���졣ÿ�θ�������Ϊ3ƪ��
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
