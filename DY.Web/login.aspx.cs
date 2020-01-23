/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
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
using DY.Site;
using DY.Entity;
using DY.Config;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using DY.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.Exceptions;

namespace DY.Web
{
    public partial class login : WebPage
    {
        IDictionary context = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string openid = base.openid;
            switch (base.act)
            {
                case "login": Login(); break;
                case "logout": Logout(); break;
                case "reg": Reg(); break;
                case "RegCheck": RegCheck(); break;
                case "forget": Forget(); break;
                case "checkuser": CheckUser(); break;
            }
        }

        protected void CheckUser()
        {
            string username = DYRequest.getRequest("username");
            int msg = 1;
            if (string.IsNullOrEmpty(username))
            {
                msg = 0;
            }
            else if (DY.Site.SiteUser.UserExists(username))
            {
                msg = 0;
            }
            base.DisplayMemoryTemplate(msg.ToString());
        }


        protected void Reg()
        {
                base.DisplayTemplate("public/reg");
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        protected void Login()
        {


            if (ispost)
            {
                string username = DYRequest.getForm("username"),
                    password = DYRequest.getForm("password"),
                    bindwecat = DYRequest.getForm("bindwecat"),
                    message = "";

                if (string.IsNullOrEmpty(username))
                    message = "请输入登录名";
                else if (string.IsNullOrEmpty(password))
                    message = "请输入登录密码";
                else if (!DY.Site.SiteUser.UserExists(username))
                    message = "用户不存在";

                //if (config.CaptchaLogin)
                //{
                //    if (Session["DYCaptcha"] == null)
                //        message = "验证码不存在，请刷新验证码";

                //    if (string.IsNullOrEmpty(captcha) && config.CaptchaLogin == true)
                //        message = "请输入验证码";

                //    if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
                //        message = "你输入的验证码与系统产生的不一致";
                //}

                if (!string.IsNullOrEmpty(message))
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
                else
                {
                    UsersInfo userinfo = GetUserInfo(username, password);

                    if (userinfo != null)
                    {
                        SiteUtils.WriteUserLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                        DY.Site.SiteUser.UpdateUserLoginInfo(DateTime.Now, DYRequest.GetIP(), userinfo.user_id.Value);
                        #region 绑定微信
                        if (!string.IsNullOrEmpty(bindwecat))
                            DY.Site.SiteUser.UpdateUserOpenidInfo(base.openid, userinfo.user_id.Value);
                        #endregion
                        if (config.CaptchaLogin)
                            Session["DYCaptcha"] = null; //请空验证码

                        base.DisplayMemoryTemplate(base.MakeJson(username, 0, "恭喜你，登录成功！"));
                    }
                    else
                    {
                        base.DisplayMemoryTemplate(base.MakeJson(username, 1, "密码错误！！！"));
                    }
                }
            }

            if (!string.IsNullOrEmpty(base.openid))
            {
                if (!DY.Site.SiteUser.UserOpenidExists(base.openid))
                {
                    UsersInfo userinfo = SiteBLL.GetUsersInfo("openid='" + base.openid + "'");

                    if (userinfo != null)
                    {
                        SiteUtils.WriteUserLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                        DY.Site.SiteUser.UpdateUserLoginInfo(DateTime.Now, DYRequest.GetIP(), userinfo.user_id.Value);
                        if (config.CaptchaLogin)
                            Session["DYCaptcha"] = null; //请空验证码

                        Response.Redirect("/user/account"+ config.UrlRewriterKzm);
                    }
                }
            }
            else
            {
                //登录跳转
                string url = Request.ServerVariables["HTTP_REFERER"];
                base.DisplayTemplate(context, "public/login");
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected UsersInfo GetUserInfo(string username, string password)
        {
            int uid = DY.Site.SiteUser.CheckUserPassword(username, password, true);

            return uid > 0 ? SiteBLL.GetUsersInfo(uid) : null;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        protected void Logout()
        {
            SiteUtils.ClearUserCookie("DYUser");

            base.DisplayMemoryTemplate(base.MakeJson(username, 0, "成功退出"));
        }
        /// <summary>
        /// 提交注册信息
        /// </summary>
        protected void RegCheck()
        {
            string username = DYRequest.getForm("username"),
                password = DYRequest.getForm("password"),
                captcha = DYRequest.getForm("code"),
                message = "";

            if (string.IsNullOrEmpty(username))
                message = "请输入用户名";
            else if (string.IsNullOrEmpty(password))
                message = "请输入用户密码";
            else if (DY.Site.SiteUser.UserExists(username))
                message = "用户已存在";

            if (config.CaptchaReg)
            {
               // if (Session["DYCaptcha"] == null)
                if (Session["DYSMS"] == null)
                    message = "验证码不存在，请重新获取";

                if (string.IsNullOrEmpty(captcha) && config.CaptchaLogin == true)
                    message = "请输入验证码";

                if (captcha.ToLower() != Session["DYSMS"].ToString().ToLower())
                    message = "你输入的验证码与系统产生的不一致";
            }

            if (!string.IsNullOrEmpty(message))
                base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
            else
            {
                    UsersInfo userinfo = new UsersInfo();
                    userinfo.user_name = DYRequest.getFormString("username");
                    userinfo.password = SiteUtils.Encryption(DYRequest.getFormString("password"));
                    userinfo.email = DYRequest.getFormString("email");
                    userinfo.question = "";
                    userinfo.answer = "";
                    userinfo.sex = 0;
                    userinfo.birthday = DYRequest.getFormDateTime("birthday");
                    userinfo.user_money = 0;
                    userinfo.frozen_money = 0;
                    userinfo.pay_points = 0;
                    userinfo.rank_points = 0;
                    userinfo.address_id = 0;
                    userinfo.reg_time = DateTime.Now;
                    userinfo.last_login = DYRequest.getFormDateTime("last_login");
                    userinfo.last_ip = "";
                    userinfo.login_count = 0;
                    userinfo.user_rank = 0;
                    userinfo.parent_id = DYRequest.getFormInt("puid", 0);
                    userinfo.is_validated = config.Reg_shenhe;
                    userinfo.is_enabled = config.Reg_shenhe;
                    userinfo.user_photo = DYRequest.getFormString("headimgurl");
                    userinfo.remarks = DYRequest.getFormString("qq");
                    //userinfo.address = DYRequest.getFormString("address");
                    userinfo.openid = DYRequest.getFormString("openid");
                    userinfo.distribution_level = DYRequest.getFormInt("dlevel", 0);
                    int user_id = SiteBLL.InsertUsersInfo(userinfo);
                    if (user_id <= 0)
                    {
                        return;
                    }
                    else
                    {
                        userinfo.user_id = user_id;
                        SiteUtils.WriteUserLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                        DY.Site.SiteUser.UpdateUserLoginInfo(DateTime.Now, DYRequest.GetIP(), user_id);


                        //积分
                        int jf = Utils.StrToInt(config.RegisterPoints, 0);
                        if (jf > 0)
                        {
                            UserSignInfo entity = new UserSignInfo();
                            entity.userid = user_id;
                            entity.change = jf;
                            //现改为积分获得时间
                            entity.date = DateTime.Now;
                            entity.points_type = 0;
                            entity.des = "注册赠送" + jf + "消费积分";
                            SiteBLL.InsertUserSignInfo(entity);
                            //修改会员积分
                            UsersInfo users = SiteBLL.GetUsersInfo(user_id);
                            users.pay_points += jf;
                            SiteBLL.UpdateUsersInfo(users);
                        }

                        base.DisplayMemoryTemplate(base.MakeJson("", 2, "注册成功！"));
                    }


                    //string card_num = DYRequest.getFormString("card_num");
                    //if (!string.IsNullOrEmpty(card_num))
                    //{
                    //    CardInfo cardinfo = SiteBLL.GetCardInfo("is_enabled=1 and is_validated=0 and card_num='" + card_num + "'");
                    //    if (cardinfo != null)
                    //    {
                    //        BonusTypeInfo bonustype = SiteBLL.GetBonusTypeInfo("type_name='注册送礼' and getdate() between send_start_date and send_end_date");
                    //        if (bonustype != null)
                    //        {
                    //            for (int i = 0; i < 12; i++)
                    //            {
                    //                //生成优惠券序列号
                    //                object val = SiteBLL.GetBonusValue("MAX(bonus_sn)", "bonus_type_id=" + bonustype.type_id);

                    //                double num = !string.IsNullOrEmpty(val.ToString()) ? Math.Floor(Convert.ToDouble(val) / 10000) : 100000;

                    //                Random rnd = new Random();

                    //                //向会员优惠券表加入数据
                    //                string bonus_sn = (num + i) + rnd.Next(0, 9999).ToString().PadLeft(4, '0');

                    //                BonusInfo bonusinfo = new BonusInfo();
                    //                bonusinfo.bonus_sn = Convert.ToInt32(bonus_sn);
                    //                bonusinfo.user_id = user_id;
                    //                bonusinfo.user_name = userinfo.user_name;
                    //                bonusinfo.emailed = 0;
                    //                bonusinfo.bonus_type_id = bonustype.type_id;
                    //                bonusinfo.is_enbled = true;
                    //                bonusinfo.use_start_date = DateTime.Now.AddMonths(i);
                    //                bonusinfo.use_end_date = DateTime.Now.AddMonths(i + 1);

                    //                SiteBLL.InsertBonusInfo(bonusinfo);
                    //            }
                    //        }

                    //        cardinfo.user_id = user_id;
                    //        cardinfo.is_validated = true;
                    //        cardinfo.use_time = DateTime.Now;
                    //        SiteBLL.UpdateCardInfo(cardinfo);
                    //    }
                    //}

                this.Login();

                if (config.CaptchaReg)
                    Session["DYSMS"] = null; //请空验证码

                base.DisplayMemoryTemplate(base.MakeJson(username, 0, ""));
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        protected void Forget()
        {
            if (ispost)
            {
                string username = DYRequest.getForm("username"),
                   email = DYRequest.getForm("email"),
                   captcha = DYRequest.getForm("captcha"),
                   message = "";

                if (Session["DYCaptcha"] == null)
                    message = "验证码不存在，请刷新验证码";
                else if (string.IsNullOrEmpty(username))
                    message = "请输入用户名";
                else if (!DY.Site.SiteUser.UserExists(username))
                    message = "用户不存在";
                else if (string.IsNullOrEmpty(email))
                    message = "请输入邮件地址";
                else if (!Utils.IsValidEmail(email))
                    message = "邮件地址格式不正确";
                else if (SiteBLL.GetUsersAllList("", string.Format("user_name='{0}' and email='{1}'", username.Trim(), email.Trim())).Count <= 0)
                    message = "您输入的邮件地址与用户名不匹配";
                else if (string.IsNullOrEmpty(captcha))
                    message = "请输入验证码";
                else if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
                    message = "你输入的验证码与系统产生的不一致";

                if (!string.IsNullOrEmpty(message))
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
                else
                {

                    string pwd = SiteBLL.GetUsersInfo(string.Format("user_name='{0}' and email='{1}'", username.Trim(), email.Trim())).password;

                    //重置密码
                    UsersInfo user = new UsersInfo();
                    user = SiteBLL.GetUsersInfo(string.Format("user_name='{0}' and email='{1}'", username.Trim(), email.Trim()));

                    Random r = new Random();
                    int password = r.Next(1000000);
                    user.password = SiteUtils.Encryption(password.ToString());
                    context.Add("password", password);

                    SiteBLL.UpdateUsersInfo(user);

                    //登录跳转
                    string url = Request.ServerVariables["HTTP_REFERER"];
                    context.Add("url", url);

                    string mailbody = base.GetTemplate(context, "tlp/email/forget", BaseConfig.WebSkinPath, false);

                    bool isok = SiteUtils.SendMailUseGmail(email, config.Name + " 密码找回信息", mailbody.Replace("{info}", Server.UrlEncode(DES.Encode(username + "," + DateTime.Now, BaseConfig.WebEncrypt))).Replace("{domain}", siteUtils.GetDomain()));

                    if (isok)
                    {
                        base.DisplayMemoryTemplate(base.MakeJson("", 0, "密码找回信息已经发送到" + email + "，请注意查收"));
                    }
                    else
                        base.DisplayMemoryTemplate(base.MakeJson("", 1, "邮件发送错误"));

                    Session["DYCaptcha"] = null;
                }
            }
            base.DisplayTemplate(context, "public/forget");
        }
    }
}
