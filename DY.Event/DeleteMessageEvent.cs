using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Common;
using System.Collections;
using DY.Entity;
using DY.Site;
using DY.Config;

namespace DY.Event
{
    /// <summary>
    /// �й�վ���������ʼ�������Ϣɾ���ļƻ�����
    /// </summary>
    public class ɾ���ʼ�����������Ϣ : IEvent
    {
        #region IEvent ��Ա

        public void Execute(object state)
        {
            this.DeleteMessage();
            EventLogs.WriteFailedLog("�Զ�ɾ���ʼ�����������Ϣ");
        }

        #endregion


        /// <summary>
        /// ɾ��������Ϣ
        /// </summary>
        private void DeleteMessage()
        {
            ArrayList emaillist = new ArrayList();
            foreach (EmailListInfo emailinfo in SiteBLL.GetEmailListAllList("", ""))
            {
                if (CheckSafe(emailinfo.email) || CheckSafe(emailinfo.remark))
                    emaillist.Add(emailinfo);
            }

            ArrayList feedbacklist = new ArrayList();
            foreach (FeedbackInfo feedbackinfo in SiteBLL.GetFeedbackAllList("", ""))
            {
                if (CheckSafe(feedbackinfo.msg_content) || CheckSafe(feedbackinfo.user_email) || CheckSafe(feedbackinfo.user_email))
                    feedbacklist.Add(feedbackinfo);
            }

            ArrayList fromvaluelist = new ArrayList();
            foreach (FromvalueInfo fromvalueinfo in SiteBLL.GetFromvalueAllList("", ""))
            {
                if (CheckSafe(fromvalueinfo.value))
                    fromvaluelist.Add(fromvalueinfo);
            }

            //ִ��ɾ��
            foreach (EmailListInfo emailinfo in emaillist)
            {
                SiteBLL.DeleteEmailListInfo(emailinfo.id.Value);
            }
            foreach (FeedbackInfo feedbackinfo in feedbacklist)
            {
                SiteBLL.DeleteFeedbackInfo(feedbackinfo.msg_id.Value);
            }
            foreach (FromvalueInfo fromvalueinfo in fromvaluelist)
            {
                SiteBLL.DeleteFromvalueInfo(fromvalueinfo.fv_id.Value);
            }
        }


        /// <summary>
        /// ����ַ����Ƿ����Σ���ַ�
        /// </summary>
        /// <param name="str">Ҫ�����ַ���</param>
        /// <returns></returns>
        private bool CheckSafe(string str)
        { 
            bool flag=false;
            if (Utils.IsSafeSqlString(str) || Utils.IsSafeUserInfoString(str))
                flag = true;
            return flag;
        }
    }
}
