using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CShop.Web
{
    public partial class PayPal : System.Web.UI.Page
    {
        protected string amount;//金额
        protected string business;//paypal帐户
        protected string cancel_url;//取消后返回的页面
        protected string cmd;//不可更改
        protected string currency_code;
        protected string item_name;//名称
        protected string no_shipping;
        protected string notify_url;//处理页面
        protected string request_id;
        protected string return_url;//返回页面
        protected string rm;
        protected string URL;

        public PayPal()
        {
            base.Load += new EventHandler(this.Page_Load);
            this.cmd = "_xclick";
            this.business = ConfigurationManager.AppSettings["BusinessEmail"];//paypal帐户
            this.item_name = "Payment for goods";//产品名称
            this.return_url = ConfigurationManager.AppSettings["ReturnUrl"];//处理页面
            this.notify_url = ConfigurationManager.AppSettings["NotifyUrl"];//返回页面
            this.cancel_url = ConfigurationManager.AppSettings["CancelPurchaseUrl"];
            this.currency_code = ConfigurationManager.AppSettings["CurrencyCode"];
            this.no_shipping = "1";
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // determining the URL to work with depending on whether sandbox or a real PayPal account should be used
            if (String.Compare(ConfigurationManager.AppSettings["UseSandbox"].ToString(), "true", false) == 0)
            {
                this.URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                this.URL = "https://www.paypal.com/cgi-bin/webscr";
            }

            // This parameter determines the was information about successfull transaction will be passed to the script
            // specified in the return_url parameter.
            // "1" - no parameters will be passed.
            // "2" - the POST method will be used.
            // "0" - the GET method will be used. 
            // The parameter is "0" by deault.
            if (String.Compare(ConfigurationManager.AppSettings["SendToReturnURL"].ToString(), "true", false) == 0)
            {
                this.rm = "2";
            }
            else
            {
                this.rm = "1";
            }

            // the total cost of the cart该车的总成本
            this.amount = this.Session["Amount"].ToString();
            // the identifier of the payment request对支付请求标识符
            this.request_id = this.Session["request_id"].ToString();
        }
    }
}