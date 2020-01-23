using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Text;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class submit_inquiry : WebPage
    {
        IDictionary context = new Hashtable();
        protected void Page_Load(object sender, EventArgs e)
        {
            string openid = base.openid;
            switch (base.act)
            {
                case "check": Check(); break;
                case "cartsubmit": CartSubmit(); break;
                //case "logout": Logout(); break;
 
            }
        }
        public void Check()
        {
            context.Add("cartlist", Store.GetCartList());
            base.DisplayTemplate(context, "public/submit_inquiry");
        }
        public void CartSubmit()
        {
            if (ispost)
            {
                string username = DYRequest.getForm("username"),
              email = DYRequest.getForm("email"),
              captcha = DYRequest.getForm("captcha"),
              message = "";
                if (Session["DYCaptcha"] == null)
                    message = "验证码不存在，请刷新验证码";
                else if (string.IsNullOrEmpty(captcha))
                    message = "请输入验证码";
                else if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
                    message = "你输入的验证码与系统产生的不一致";
                if (!string.IsNullOrEmpty(message))
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
                else
                {
                    GoodsInquiryInfo entity = new GoodsInquiryInfo();

                    entity.name = DYRequest.getForm("name");
                    entity.company = DYRequest.getForm("company");
                    entity.tel = DYRequest.getForm("tel");
                    entity.address = DYRequest.getForm("address");
                    entity.email = DYRequest.getForm("email");
                    entity.advice = DYRequest.getForm("advice");
                    entity.goods_id = DYRequest.getForm("goods_id");
                    entity.goods_number = DYRequest.getForm("goods_number");
                    entity.userid = userid;
                    entity.username = username;
                    if (DYRequest.getForm("goods_id") != "")
                    SiteBLL.InsertGoodsInquiryInfo(entity);
                    context.Add("cartlist", Store.GetCartList());
                    Session["DYCaptcha"] = null;
                   
                }

            }
            Response.Write("<script>window.location='/cart.htm'</script>");
        }
    }
}