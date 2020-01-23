using System;
using System.Collections;
using System.Web;
namespace tenpay
{

    public partial class tenpay_return_url : DY.Site.WebPage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            string title = "";
            string content = "";

            DY.Entity.PaymentInfo payinfo = DY.Site.SiteBLL.GetPaymentInfo("pay_code='tenpay'");
            if (payinfo == null)
            {
                Response.Write("错误提示：财付通设置不正确！");
                return;
            }

            //密钥
            //string key = "8934e7d15453e97507ef794cf7b0519d";
            string key = (payinfo.pay_config + ",,").Split(',')[1];

            //创建PayResponseHandler实例
            PayResponseHandler resHandler = new PayResponseHandler(Context);

            resHandler.setKey(key);

            //判断签名
            if (resHandler.isTenpaySign())
            {
                //交易单号
                string transaction_id = resHandler.getParameter("transaction_id");

                string sp_billno = resHandler.getParameter("sp_billno");

                //金额金额,以分为单位
                string total_fee = resHandler.getParameter("total_fee");

                //支付结果
                string pay_result = resHandler.getParameter("pay_result");

                if ("0".Equals(pay_result))
                {
                    //------------------------------
                    //处理业务开始
                    //------------------------------ 

                    //注意交易单不要重复处理
                    //注意判断返回金额


                    //买家付款成功，注意判断订单是否重复的逻辑
                    DY.Entity.OrderInfoInfo orderinfo = DY.Site.SiteBLL.GetOrderInfoInfo("order_sn='" + sp_billno + "'");
                    if (orderinfo == null)
                    {
                        title = "很抱歉，付款失败！";

                        content = "多次提交订单失败,请与我们的客户联系。";
                    }
                    else
                    {
                        orderinfo.pay_status = 1;
                        orderinfo.pay_time = DateTime.Now;
                        //更新数据库付款状态
                        DY.Site.SiteBLL.UpdateOrderInfoInfo(orderinfo);

                        title = "恭喜您，付款成功！";
                        content += "订 单 号：" + sp_billno + "</br>";
                        content += "成功支付金额：" + (0.01* Convert.ToDouble(total_fee));
                    }


                    //------------------------------
                    //处理业务完毕
                    //------------------------------

                    //调用doShow, 打印meta值跟js代码,告诉财付通处理成功,并在用户浏览器显示$show页面.
                    //resHandler.doShow("http://localhost/tenpayApp/show.aspx");
                }
                else
                {
                    //当做不成功处理
                    Response.Write("支付失败");
                }

            }
            else
            {
                Response.Write("认证签名失败");
                //string debugInfo = resHandler.getDebugInfo();
                //Response.Write("<br/>debugInfo:" + debugInfo);
            }

            IDictionary context = new Hashtable();
            context.Add("pay_title", title);
            context.Add("pay_content", content);

            base.DisplayTemplate(context, "store/payreturn");
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
