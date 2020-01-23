/**
 * 功能描述：首页
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

namespace DY.Web
{
    public partial class _user : WebPage
    {
        protected IDictionary context = new Hashtable();
        UsersInfo userinfo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.userid <= 0)
                Response.Redirect("/user-login.htm");
            else
                userinfo = SiteBLL.GetUsersInfo(base.userid);

            #region 保存用户修改的密码
            if (base.act == "SavePassword")
            {
                SavePassword();
            } 
            #endregion

            #region 保存用户的资料
            else if (base.act == "SaveProfile")
            {
                SaveProfile();
            }
            #endregion

            #region 保存用户的密码提示问题答案
            else if (base.act == "SavePasswordSafe")
            {
                SavePasswordSafe();
            }
            #endregion

            #region 显示用户的收藏夹信息
            else if (base.act == "favorites")
            {
                productFavorites();
            }
            #endregion

            #region 用户订单
            else if (base.act == "order_list")
            {
                this.OrderList();
            }
            #endregion

            #region 分销中心
            else if (base.act == "center")
            {
                this.Center();
            }
            #endregion

            #region 提现
            else if (base.act == "withdrawals")
            {
                this.Withdrawals();
            }
            #endregion

            #region 帐号查询
            else if (base.act == "account")
            {
                Account();
            }
            #endregion

            #region 我的优惠券
            else if (base.act == "coupon")
            {
                this.Coupon();
            }
            #endregion

            #region 我的积分
            else if (base.act == "sign")
            {
                this.Sign();
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "update_field")
            {
                int id = DYRequest.getRequestInt("id");
                object val = DYRequest.getRequest("val");
                string fieldName = DYRequest.getRequest("fieldName");

                if (Utils.StrToInt(val, 0) == 2 || Utils.StrToInt(val, 0) == 0)
                {
                    //执行修改
                    SiteBLL.UpdateOrderInfoFieldValue(fieldName, val, id);
                }

                //输出json数据
                base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
            }
            #endregion

            #region 提交意见建议
            else if (base.act == "feedback")
            {
                this.AddFeedback();
            }
            #endregion

            context.Add("userpage", "user/"+base.act);
            context.Add("userinfo", userinfo);
            context.Add("is_sign", SiteBLL.GetUserSignValue("COUNT(sign_id)", "DateDiff(dd,date,getdate())=0"));
            context.Add("userFields", SiteUser.GetUserRegFields(base.userid).Rows);
            context.Add("this", this);

            base.DisplayTemplate(context, "user",base.isajax);
        }
        /// <summary>
        /// 保存用户信息
        /// </summary>
        protected void SaveProfile()
        {
            if (ispost)
            {
                string message = "";
                string captcha = DYRequest.getFormString("code");
                if (Session["DYSMS"] == null)
                    message = "验证码不存在，请重新获取";
                else
                {
                    if (string.IsNullOrEmpty(captcha))
                        message = "请输入验证码";

                    if (captcha.ToLower() != Session["DYSMS"].ToString().ToLower())
                        message = "你输入的验证码与系统产生的不一致";
                }
                if (!string.IsNullOrEmpty(message))
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
                else
                {
                    UsersInfo entity = new UsersInfo();
                    entity.user_id = userid;
                    entity.email = DYRequest.getFormString("email");
                    entity.user_name = DYRequest.getFormString("user_name");
                    entity.sex = DYRequest.getFormInt("sex");
                    entity.birthday = DYRequest.getFormDateTime("birthday");
                    entity.user_photo = DYRequest.getFormString("user_photo");

                    //entity.mobile = DYRequest.getFormString("mobile");
                    //entity.address = DYRequest.getFormString("address");

                    SiteBLL.UpdateUsersInfo(entity);

                    //保存注册项信息
                    if (Request.Form["reg_field_id"] != null)
                        SiteUser.SaveRegFields(base.userid, Request.Form.GetValues("reg_field_id"));

                    Session["DYSMS"] = null; //请空验证码

                    base.DisplayMemoryTemplate(base.MakeJson("", 0, "修改成功"));
                }
            }
        }
        /// <summary>
        /// 保存用户密码
        /// </summary>
        protected void SavePassword()
        {
            if (ispost)
            {
                //原来密码是否正确
                int uid = DY.Site.SiteUser.CheckUserPassword(base.username, DYRequest.getForm("oldpassword"), true);
                if (uid <= 0)
                {
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, "原来密码不正确！"));
                }
                else
                {
                    UsersInfo entity = new UsersInfo();
                    entity.user_id = userid;
                    entity.password = SiteUtils.Encryption(DYRequest.getFormString("password"));

                    SiteBLL.UpdateUsersInfo(entity);

                    base.DisplayMemoryTemplate(base.MakeJson("", 0, "修改成功，下次记得用新密码登录！"));
                }
            }
        }
        /// <summary>
        /// 保存用户密码保护信息
        /// </summary>
        protected void SavePasswordSafe()
        {
            if (ispost)
            {
                UsersInfo entity = new UsersInfo();
                entity.user_id = userid;
                entity.question = DYRequest.getForm("question");
                entity.answer = SiteUtils.Encryption(DYRequest.getFormString("answer"));

                SiteBLL.UpdateUsersInfo(entity);

                base.DisplayMemoryTemplate(base.MakeJson("", 0, "修改成功！"));
            }
        }

        /// <summary>
        /// 产品收藏夹
        /// </summary>
        protected void productFavorites()
        {
            context.Add("fav_list", Goods.GetGoodsFavorites(base.userid).Rows);
        }

        /// <summary>
        /// 分销中心
        /// </summary>
        protected void Center()
        {
            string pageString = "";
            context.Add("day_money", SiteBLL.GetUserDistributionValue("SUM(amout)", "DateDiff(dd,date,getdate())=0 and user_id=" + base.userid));
            context.Add("week_money", SiteBLL.GetUserDistributionValue("SUM(amout)", "DateDiff(week,date,getdate())=0 and user_id="+base.userid));
            context.Add("month_money", SiteBLL.GetUserDistributionValue("SUM(amout)", "DateDiff(month,date,getdate())=0 and user_id=" + base.userid));
            context.Add("count_money", SiteBLL.GetUserDistributionValue("SUM(amout)", "user_id=" + base.userid));
            context.Add("distribution_list", SiteBLL.GetUserDistributionList(base.pageindex, config.PageSize, "distribution_id asc", "user_id=" + base.userid, out base.ResultCount));
            context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, config.PageSize, base.act + "-p", config.UrlRewriterKzm, 6, out pageString));
            context.Add("result_count", base.ResultCount);
        }

        /// <summary>
        /// 提现
        /// </summary>
        protected void Withdrawals()
        {
            string pageString = "";
            context.Add("withdrawals_list", SiteBLL.GetUserWithdrawalsList(base.pageindex, config.PageSize, "withdrawals_id asc", "user_id=" + base.userid, out base.ResultCount));
            context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, config.PageSize, base.act + "-p", config.UrlRewriterKzm, 6, out pageString));
            context.Add("result_count", base.ResultCount);
        }

        /// <summary>
        /// 用户订单
        /// </summary>
        protected void OrderList()
        {
            int id = DYRequest.getRequestInt("id");

            #region 详情
            if (id > 0)
            {
                base.act = "order_info";

                context.Add("entity", SiteBLL.GetOrderInfoInfo("order_id=" + id + " and user_id="+base.userid));
                context.Add("ordergoodslist", SiteBLL.GetOrderGoodsAllList("rec_id desc", "order_id=" + id));
                context.Add("pre_id", SiteBLL.GetOrderInfoValue("MAX(order_id)", "user_id="+ base.userid +" and order_id<" + id));
                context.Add("next_id", SiteBLL.GetOrderInfoValue("MIN(order_id)", "user_id=" + base.userid + " and order_id>" + id));
            } 
            #endregion

            #region 列表
            else
            {
                string pageString = "";
                string filter = "";
                string order_status = DYRequest.getRequest("order_status");
                string pay_status = DYRequest.getRequest("pay_status");
                string delivery_status = DYRequest.getRequest("delivery_status");

                if (!string.IsNullOrEmpty(order_status))
                {
                    filter += " and order_status=" + order_status;
                }

                if (!string.IsNullOrEmpty(pay_status))
                {
                    filter += " and pay_status=" + pay_status;
                }

                if (!string.IsNullOrEmpty(delivery_status))
                {
                    filter += " and delivery_status=" + delivery_status;
                }

                context.Add("order_list", SiteBLL.GetOrderInfoList(base.pageindex, 10, "order_id desc", "user_id=" + base.userid + filter, out base.ResultCount));
                //context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, 10, base.pageindex, url, config.UrlRewriterKzm, 6));
                context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, 10, base.act +"-p", config.UrlRewriterKzm, 6, out pageString));
                context.Add("order_status", order_status);
                context.Add("pay_status", pay_status);
                context.Add("delivery_status", delivery_status);
                context.Add("result_count", base.ResultCount);
            } 
            #endregion
        }
        /// <summary>
        /// 我的优惠券
        /// </summary>
        protected void Coupon()
        {
            string pageString = "";
            context.Add("coupon_list", SiteBLL.GetBonusList(base.pageindex, 10, "bonus_id asc", "user_id=" + base.userid, out base.ResultCount));
            context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, config.PageSize, base.act + "-p", config.UrlRewriterKzm, 6, out pageString));
            context.Add("result_count", base.ResultCount);
        }

        /// <summary>
        /// 我的积分
        /// </summary>
        protected void Sign()
        {
            string pageString = "";
            context.Add("sign_list", SiteBLL.GetUserSignList(base.pageindex, 10, "sign_id asc", "userid=" + base.userid, out base.ResultCount));
            context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, config.PageSize, base.act + "-p", config.UrlRewriterKzm, 6, out pageString));
            context.Add("result_count", base.ResultCount);
        }

        /// <summary>
        /// 帐号查询
        /// </summary>
        [ObsoleteAttribute("已更新为支付完成后获取积分，此获取方法暂时已经失效",false)]
        protected void Account()
        {
            UserIntegralInfo uil = SiteBLL.GetUserIntegralInfo("user_id=" + base.userid + "");
           // context.Add("inter", uil.integral);
        }

        /// <summary>
        /// 保存用户反馈内容
        /// </summary>
        protected void AddFeedback()
        {
            if (base.ispost)
            {
                FeedbackInfo fbinfo = new FeedbackInfo();
                fbinfo.msg_content = DYRequest.getForm("content");
                fbinfo.msg_title = base.username + "提交投诉建议内容";
                fbinfo.user_email = userinfo.email;
                fbinfo.user_id = base.userid;
                fbinfo.user_name = base.username;
                int id = SiteBLL.InsertFeedbackInfo(fbinfo);
                if (id > 0)
                {
                    base.DisplayJsonMessage(0,"");
                }
                else
                {
                    base.DisplayJsonMessage("递交失败");
                }
            }
        }
        /// <summary>
        /// 取得优惠券类型信息
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public BonusTypeInfo bonusType(int type_id)
        {
            return SiteBLL.GetBonusTypeInfo(type_id);
        }
    }
}
