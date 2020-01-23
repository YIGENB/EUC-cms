using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DY.Common;
using DY.Config;
using DY.Entity;
using RUNWINZIP;

namespace DY.Site
{
    /// <summary>
    /// DY后台页面基类
    /// </summary>
    public class AdminPage : PageBase
    {
        /// <summary>
        /// 当前用户所拥有的权限列表
        /// </summary>
        protected internal string actionlist;
        /// <summary>
        /// 当前页码
        /// </summary>
        protected internal int pageindex = DYRequest.getRequestInt("page", 1);
        /// <summary>
        /// 当前列表每页显示记录数
        /// </summary>
        protected internal int pagesize = SiteUtils.GetPageSize();
        /// <summary>
        /// 页面请求方式
        /// </summary>
        protected internal string target = DYRequest.getRequest("t");
        /// <summary>
        /// 活动类型
        /// </summary>
        protected internal int atype = DYRequest.getRequestInt("atype",0);
        /// <summary>
        /// 活动类型ID
        /// </summary>
        protected internal int atype_id = DYRequest.getRequestInt("aid",0);



        /// <summary>
        /// AdminPage类构造函数
        /// </summary>
        public AdminPage()
        {
            #region 如果IP访问列表有设置则进行判断
            if (config.Admin_allowIp.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Admin_allowIp, "\n");
                if (!Utils.InIPArray(DYRequest.GetIP(), regctrl))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<br /><br /><div style=\"width:100%\" align=\"center\"><div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\">");
                    sb.Append("<img src=\"/static/images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" />&nbsp; 您的IP地址不在系统允许的范围之内</div></div>");
                    Context.Response.Write(sb.ToString());
                    Context.Response.End();
                    return;
                }
            }
            #endregion

            oluserinfo = OnlineAdminUsers.UpdateInfo(BaseConfig.WebEncrypt);

            if (oluserinfo != null)
            {
                if (oluserinfo.Userid < 0 && DYRequest.GetPageName() != "login.aspx")
                {
                    Context.Response.Redirect("login.aspx");
                    return;
                }
                userid = oluserinfo.Userid;
                username = oluserinfo.Username;
                password = oluserinfo.Password;
                actionlist = oluserinfo.Actions;
            }
        }
        /// <summary>
        /// 输出模板
        /// </summary>
        /// <param name="templateName">模板名称（不带后辍）</param>
        protected void DisplayTemplate(string templateName)
        {
            this.DisplayTemplate(new Hashtable(), templateName, false);
        }
        /// <summary>
        /// 输出模板
        /// </summary>
        /// <param name="context">供模板调用的数据，IDictionary类型</param>
        /// <param name="templateName">模板名称（不带后辍）</param>
        protected void DisplayTemplate(IDictionary context, string templateName)
        {
            this.DisplayTemplate(context, templateName, false);
        }
        /// <summary>
        /// 输出模板
        /// </summary>
        /// <param name="context">供模板调用的数据，IDictionary类型</param>
        /// <param name="templateName">模板名称（不带后辍）</param>
        /// <param name="json">是否为json输出</param>
        protected void DisplayTemplate(IDictionary context, string templateName, bool json)
        {
            context.Add("id", this.id);
            context.Add("t", this.target);
            context.Add("isajax", base.isajax);
            context.Add("visualization", DYRequest.getRequestInt("visualization"));
            context.Add("pid", this.pid);//微信公众号id
            //context.Add("ver", RunZIP.CheckedVer());//检测版本
            context.Add("atype", this.atype);
            context.Remove("userid");
            context.Add("userid", this.userid);
            context.Add("skin", BaseConfig.AdminSkin);
            //context.Add("username", this.username);

            base.DisplayTemplate(context, templateName, BaseConfig.AdminSkinPath, json);
        }
        /// <summary>
        /// 获取解析过后的模板数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="templateName"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        protected string GetTemplate(IDictionary context, string templateName, bool json)
        {
            return base.GetTemplate(context, templateName, BaseConfig.AdminSkinPath, json);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="msg">提示内容</param>
        /// <param name="type">0为普通，1为错误，2为正确</param>
        /// <returns></returns>
        protected void DisplayMessage(string msg, int type)
        {
            this.DisplayMessage(msg, type, null, null);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="msg">提示内容</param>
        /// <param name="type">0为普通，1为错误，2为正确</param>
        /// <param name="links">候选url</param>
        protected void DisplayMessage(string msg, int type, Hashtable links)
        {
            this.DisplayMessage(msg, type, null, links);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="msg">提示内容</param>
        /// <param name="type">0为普通，1为错误，2为正确</param>
        /// <param name="url">候选url</param>
        protected void DisplayMessage(string msg, int type, string url)
        {
            this.DisplayMessage(msg, type, url, null);
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="msg">提示内容</param>
        /// <param name="type">0为普通，1为错误，2为正确</param>
        /// <param name="url">要跳转到的url</param>
        protected void DisplayMessage(string msg, int type, string url, Hashtable links)
        {
            string msg_img = "";

            if (type == 0)
                msg_img = "info";
            else if (type == 1)
                msg_img = "danger";
            else if(type==2)
                msg_img = "success";
            else
                msg_img = "warning";

            IDictionary context = new Hashtable();
            context.Add("msg_type", msg_img);
            context.Add("msg_detail", msg);
            context.Add("default_url", string.IsNullOrEmpty(url) == true ? Request.UrlReferrer.PathAndQuery : url);
            context.Add("links", links);

            this.DisplayTemplate(context, "systems/message");
        }
        /// <summary>
        /// 检测权限
        /// </summary>
        /// <param name="ActionCode"></param>
        protected void IsChecked(string ActionCode)
        {
            this.IsChecked(ActionCode, false);
        }
        /// <summary>
        /// 检测是否具有权限，没有则提示
        /// </summary>
        /// <param name="ActionCode">权限代码</param>
        /// <param name="isJson">是否以json形式输出</param>
        /// <returns></returns>
        protected void IsChecked(string ActionCode, bool isJson)
        {
            bool ischeck = true;

            if (this.actionlist == null)
                ischeck = false;
            else
            {
                if (this.actionlist == "all")
                    ischeck = true;
                else
                {
                    if (this.actionlist.IndexOf(ActionCode) != -1)
                        ischeck = true;
                    else
                        ischeck = false;
                }
            }

            if (!ischeck)
            {
                if (!isJson)
                    this.DisplayMessage("您没有执行本操作的权限", 1, "main.aspx");
                else
                    this.DisplayMemoryTemplate(base.MakeJson(null, 1, "您没有执行本操作的权限"));
            }
        }

        /// <summary>
        /// 检测是否存在公众号id，没有则提示
        /// </summary>
        /// <param name="mp_id">mp_id</param>
        /// <returns></returns>
        protected void IsCheckedWeCat(int mp_id)
        {
            bool ischeck = true;

            if (mp_id>0)
                ischeck = false;

            if (ischeck)
                base.DisplayMemoryTemplate(base.MakeJson("请求错误，请点击公众号管理", 1, null));
        }
        /// <summary>
        /// 添加后台操作日志
        /// </summary>
        /// <param name="log_info">日志信息</param>
        protected void AddLog(string log_info)
        {
            if (userid>0)
                AdminLog.AddLog(userid, username, log_info);
            else
                WriteFile(log_info+DateTime.Now);
        }

        /// <summary>
        ///日志文件保存
        /// </summary>
        /// <returns></returns>
        protected void WriteFile(string str)
        {
            AdminLog.WriteFile(str);
        }
    }
}
