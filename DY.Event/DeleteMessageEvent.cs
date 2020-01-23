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
    /// 有关站点留言与邮件垃圾信息删除的计划任务
    /// </summary>
    public class 删除邮件留言垃圾信息 : IEvent
    {
        #region IEvent 成员

        public void Execute(object state)
        {
            this.DeleteMessage();
            EventLogs.WriteFailedLog("自动删除邮件留言垃圾信息");
        }

        #endregion


        /// <summary>
        /// 删除垃圾信息
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

            //执行删除
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
        /// 检测字符串是否存在危险字符
        /// </summary>
        /// <param name="str">要检测的字符串</param>
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
