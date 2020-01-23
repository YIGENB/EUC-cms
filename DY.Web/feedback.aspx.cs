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

namespace DY.Web
{
    public partial class feedback : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();

            switch (this.act)
            {
                case "Submit":
                    Submit();
                    break;
                default:
                    base.DisplayTemplate(context, "feedback");
                    break;
            }

            //IDictionary context = new Hashtable();
            //context.Add("comment_type", 3);//在线咨询
            //base.DisplayTemplate(context, "feedback");

        }
        protected void Submit()
        {
            string
                    captcha = DYRequest.getForm("captcha"),
                    message = "";

            //if (Session["DYCaptcha"] == null)
            //    message = "验证码不存在，请刷新验证码";
            //else if (string.IsNullOrEmpty(captcha))
            //    message = "请输入验证码";
            //else if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
            //    message = "你输入的验证码与系统产生的不一致";

            if (!string.IsNullOrEmpty(message))
                base.DisplayJsonMessage(message);
            else
            {
                FeedbackInfo feedbackinfo = new FeedbackInfo();
                feedbackinfo.is_show = false;
                feedbackinfo.msg_content = Utils.RemoveHtml(DYRequest.getForm("msg_content"));
                feedbackinfo.msg_file = DYRequest.getForm("msg_file");
                feedbackinfo.msg_time = DateTime.Now;
                feedbackinfo.msg_title = DYRequest.getForm("msg_title");
                feedbackinfo.msg_type = DYRequest.getFormInt("msg_type");
                feedbackinfo.order_id = 0;
                feedbackinfo.parent_id = 0;
                feedbackinfo.user_email = DYRequest.getForm("user_email");
                feedbackinfo.user_id = base.userid;
                feedbackinfo.user_name = DYRequest.getForm("user_name");//base.userid > 0 ? base.username : "";
                feedbackinfo.user_qq = DYRequest.getForm("user_qq");
                SiteBLL.InsertFeedbackInfo(feedbackinfo);

                base.DisplayJsonMessage(0, "恭喜，您的信息已经递交成功，请等待我们的回复。");
            }
        }
    }
}
