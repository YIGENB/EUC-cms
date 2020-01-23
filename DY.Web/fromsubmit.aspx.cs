using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class fromsubmit : WebPage
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
                int id = 0;
                string guid = Guid.NewGuid().ToString();
                ArrayList fromlistid = SiteBLL.GetFormAllAllList("","");
                foreach (FormAllInfo fv in fromlistid)
                {
                    if (id == 0)
                        id = DYRequest.getFormInt("parent_id[" + fv.allform_id + "]");
                    else
                        break;
                }

                ArrayList fromlist = SiteBLL.GetFormAllAllList("allform_id asc", "parent_id=" + id);

                foreach (FormAllInfo fv in fromlist)
                {
                    FromvalueInfo feedbackinfo = new FromvalueInfo();
                    feedbackinfo.value = DYRequest.getForm("value["+fv.allform_id+"]");
                    feedbackinfo.position_id = id;
                    feedbackinfo.allform_id = DYRequest.getFormInt("allform_id[" + fv.allform_id + "]");
                    feedbackinfo.is_best = false ;
                    feedbackinfo.isshow = false;
                    feedbackinfo.sort_order = 0;
                    feedbackinfo.session_id = guid;
                    SiteBLL.InsertFromvalueInfo(feedbackinfo);
                }

                base.DisplayJsonMessage(0, "恭喜，您的信息已经递交成功，请等待我们的回复。");
            }
        }
    }
}