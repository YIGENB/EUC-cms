using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DY.Site;
using DY.Entity;
using DY.Config;
using System.Threading;
using DY.ScheduledEvents;
using DY.Common;
using System.Collections;

namespace DY.Web
{
    public class Global : System.Web.HttpApplication
    {
        static Timer eventTimer;
        protected void Application_Start(object sender, EventArgs e)
        {
            //启动计划任务线程
            if (eventTimer == null && ScheduleConfigs.GetConfig().Enabled)
            {
                EventLogs.LogFileName = Utils.GetMapPath(string.Format("{0}log/scheduleeventfaildlog.log", BaseConfigs.GetForumPath));
                EventManager.RootPath = Utils.GetMapPath(BaseConfigs.GetForumPath);
                eventTimer = new Timer(new TimerCallback(ScheduledEventWorkCallback), Context, 60000, EventManager.TimerMinutesInterval * 60000);
            }
        }
        private void ScheduledEventWorkCallback(object sender)
        {
            try
            {
                if (ScheduleConfigs.GetConfig().Enabled)
                {
                    EventManager.Execute();
                }
            }
            catch (Exception e)
            {
                EventLogs.WriteFailedLog(e.ToString());
            }

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                var httpError = lastError as HttpException;
                if (httpError != null)
                {
                    //ASP.NET的400与404错误不记录日志，并都以自定义404页面响应
                    var httpCode = httpError.GetHttpCode();
                    if (httpCode == 400 || httpCode == 404)
                    {
                        Response.StatusCode = 404;//在IIS中配置自定义404页面
                        Server.Execute("/html/404.aspx");
                        Server.ClearError();
                        return;
                    }
                    else if (httpCode == 500)
                    {
                        Response.StatusCode = 500;//在IIS中配置自定义500页面
                        Server.Execute("/html/500.aspx");
                        Server.ClearError();
                        return;
                    }
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}