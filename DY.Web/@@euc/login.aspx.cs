/**
 * 功能描述：登录页
 * 创建时间：2010-1-29 12:43:46
 * 最后修改时间：2010-1-29 12:43:46
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Config;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class login : AdminPage
    {
        IDictionary context = new Hashtable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.userid > 0)
                Response.Redirect("default.aspx");

            //if (AdminLog.UpdateLoginLog(DYRequest.GetIP(), false) > 3)
            //{
            //    Response.Write("您已经多次输入密码错误，如有疑问，请联系管理员。<a href=\"/\">返回首页</a>");
            //    return;
            //}

            if (ispost)
            {
                string username=DYRequest.getForm("username"),password= DYRequest.getForm("password"),captcha = DYRequest.getForm("captcha");
                if (AdminLog.UpdateLoginLog(DYRequest.GetIP(), false) > 3)
                {
                    if (Session["DYCaptcha"] == null)
                        base.DisplayMessage("验证码不存在，请刷新页面", 1);

                    if (string.IsNullOrEmpty(captcha))
                        base.DisplayMessage("请输入验证码", 0);

                    if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
                        base.DisplayMessage("你输入的验证码与系统产生的不一致", 1);
                }

                if (string.IsNullOrEmpty(username))
                    base.DisplayMessage("请输入登录名", 0);

                if (string.IsNullOrEmpty(password))
                    base.DisplayMessage("请输入登录密码", 0);

                AdminUserInfo userinfo = new AdminUserInfo();
                if (username == BaseConfig.username && password == BaseConfig.password)
                {
                    userinfo = AdminUser.GetUserInfo();
                    SiteUtils.WriteAdminLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                }
                else
                {
                    if (!AdminUser.Exists(username))
                        base.DisplayMessage("用户不存在", 1);

                    userinfo = GetUserInfo(username, password);
                }

                if (userinfo != null)
                {
                    SiteUtils.WriteAdminLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                    AdminLog.DeleteLoginLog(DYRequest.GetIP());  //删除错误登录信息
                    AdminLog.AddLoginLog(userinfo.user_id.Value,username);
                    AdminUser.UpdateAdminLoginInfo(DateTime.Now, DYRequest.GetIP(), userinfo.user_id.Value);
                    Session["adminuserid"] = userinfo.user_id.Value;
                    Session["DYCaptcha"] = null; //请空验证码

                    //转到后台首页
                    Response.Redirect("default.aspx");
                }
                else
                { 
                    int errcount = AdminLog.UpdateLoginLog(DYRequest.GetIP(),true);

                    if (errcount > 3)
                        base.DisplayMessage("用户名或密码错误", 1);
                    else
                        base.DisplayMessage(string.Format("第{0}次错误", errcount), 1);
                }
            }
            context.Add("errcount", AdminLog.UpdateLoginLog(DYRequest.GetIP(), false));
            base.DisplayTemplate(context, "login");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected AdminUserInfo GetUserInfo(string username, string password)
        {
            int uid = AdminUser.CheckPassword(username, password, true);

            return uid > 0 ? AdminUser.GetUserInfo(uid) : null;
        }
    }
}
