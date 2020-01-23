using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using tenpay;
using System.Collections.Specialized;

namespace DY.Web.PayReturn
{
	/// <summary>
	/// return_url 的摘要说明。
	/// </summary>
    public partial class PaiPai_return_url : DY.Site.WebPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            string title = "";
            string content = "";

            DY.Entity.PaymentInfo payinfo = DY.Site.SiteBLL.GetPaymentInfo("pay_code='tenpay'");
            if (payinfo == null)
            {
                Response.Write("错误提示：支付宝设置不正确！");
                return;
            }

			//密钥
            string key = (payinfo.pay_config + ",,").Split(',')[1];

			//创建PayResponseHandler实例
			MediPayResponse resHandler = new MediPayResponse(Context);

			resHandler.setKey(key);

			//判断签名
			if(resHandler.isTenpaySign()) 
			{
	
				//支付结果
				string retcode = resHandler.getParameter("retcode");

				//支付状态
				string status = resHandler.getParameter("status");

				//商户订单号
				string mch_vno = resHandler.getParameter("mch_vno");

                string trade_price = resHandler.getParameter("trade_price");

				if("0".Equals(retcode)) 
				{
					//Response.Write("status:" + status + "<br/>");

					//------------------------------
					//处理业务开始
					//------------------------------ 
		
					//按状态处理相关业务逻辑
					switch(Int32.Parse(status)) 
					{
						case 1: 
							//交易创建
							break;
						case 2:
							//收获地址填写完毕
							break;
						case 3:
							//买家付款成功，注意判断订单是否重复的逻辑
                            DY.Entity.OrderInfoInfo orderinfo = DY.Site.SiteBLL.GetOrderInfoInfo("order_sn='" + mch_vno + "'");
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
                                //更新会员积分
                                user.UpdateUserIntegral(mch_vno);


                                title = "恭喜您，付款成功！";
                                content += "订 单 号：" + mch_vno + "</br>";
                                content += "成功支付金额：" + trade_price;
                            }
							break;
						case 4:
							//卖家发货成功
							break;
						case 5:
							//买家收货确认，交易成功
							break;
						case 6:
							//交易关闭，未完成超时关闭
							break;
						case 7:
							//修改交易价格成功
							break;
						case 8:
							//买家发起退款
							break;
						case 9:
							//退款成功
							break;
						case 10:
							//退款关闭
							break;
						default:
							//error	
							break;
					}	
					//调用doShow, 打印meta值跟js代码,告诉财付通处理成功,并在用户浏览器显示$show页面.
					//resHandler.doShow();

				} 
				else 
				{
					//当做不成功处理
                    title = "支付失败";  // Response.Write("支付失败");
				}
	
			} 
			else 
			{
                title = "认证签名失败";  // Response.Write("认证签名失败");
				
			}

			//string debugInfo = resHandler.getDebugInfo();
			//Response.Write("<br/>debugInfo:" + debugInfo);
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

    /// <summary>
    /// ResponseHandler 的摘要说明。
    /// </summary>
    public class ResponseHandler
    {
        /** 密钥 */
        private string key;

        /** 应答的参数 */
        protected Hashtable parameters;

        /** debug信息 */
        private string debugInfo;

        protected HttpContext httpContext;

        //获取服务器通知数据方式，进行参数获取
        public ResponseHandler(HttpContext httpContext)
        {
            parameters = new Hashtable();

            this.httpContext = httpContext;
            NameValueCollection collection;
            if (this.httpContext.Request.HttpMethod == "POST")
            {
                collection = this.httpContext.Request.Form;
            }
            else
            {
                collection = this.httpContext.Request.QueryString;
            }

            foreach (string k in collection)
            {
                string v = (string)collection[k];
                this.setParameter(k, v);
            }
        }

        /** 获取密钥 */
        public string getKey()
        { return key; }

        /** 设置密钥 */
        public void setKey(string key)
        { this.key = key; }

        /** 获取参数值 */
        public string getParameter(string parameter)
        {
            string s = (string)parameters[parameter];
            return (null == s) ? "" : s;
        }

        /** 设置参数值 */
        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }

        /** 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。 
         * @return boolean */
        public virtual Boolean isTenpaySign()
        {
            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToLower();

            //debug信息
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").ToLower().Equals(sign);
        }

        /**
        * 显示处理结果。
        * @param show_url 显示处理结果的url地址,绝对url地址的形式(http://www.xxx.com/xxx.aspx)。
        * @throws IOException 
        */
        public void doShow(string show_url)
        {
            string strHtml = "<html><head>\r\n" +
                "<meta name=\"TENCENT_ONLINE_PAYMENT\" content=\"China TENCENT\">\r\n" +
                "<script language=\"javascript\">\r\n" +
                "window.location.href='" + show_url + "';\r\n" +
                "</script>\r\n" +
                "</head><body></body></html>";

            this.httpContext.Response.Write(strHtml);

            this.httpContext.Response.End();
        }

        /** 获取debug信息 */
        public string getDebugInfo()
        { return debugInfo; }

        /** 设置debug信息 */
        protected void setDebugInfo(String debugInfo)
        { this.debugInfo = debugInfo; }

        protected virtual string getCharset()
        {
            return this.httpContext.Request.ContentEncoding.BodyName;

        }

        /** 是否财付通签名,规则是:按参数名称a-z排序,遇到空值的参数不参加签名。 
         * @return boolean */
        public virtual Boolean _isTenpaySign(ArrayList akeys)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = MD5Util.GetMD5(sb.ToString(), getCharset()).ToLower();

            //debug信息
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").ToLower().Equals(sign);
        }
    }

    public class MediPayResponse : ResponseHandler
    {
        public MediPayResponse(HttpContext httpContext)
            : base(httpContext)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /**
            * 是否财付通签名
            * @Override
            * @return boolean
        */

        public override Boolean isTenpaySign()
        {



            StringBuilder sb = new StringBuilder();

            ArrayList akeys = new ArrayList();
            akeys.Add("attach");
            akeys.Add("buyer_id");
            akeys.Add("cft_tid");
            akeys.Add("chnid");
            akeys.Add("cmdno");
            akeys.Add("mch_vno");
            akeys.Add("retcode");
            akeys.Add("seller");
            akeys.Add("status");
            akeys.Add("total_fee");
            akeys.Add("trade_price");
            akeys.Add("transport_fee");
            akeys.Add("version");
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0
                    && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append("key=" + this.getKey());
            string sign = MD5Util.GetMD5(sb.ToString(), getCharset());

            //debug信息
            this.setDebugInfo(sb.ToString() + " => sign:" + sign);
            return getParameter("sign").Equals(sign);

        }


        public void doShow()
        {
            string strHtml = "<html><head>\r\n" +
                "<meta name=\"TENCENT_ONLINE_PAYMENT\" content=\"China TENCENT\">\r\n" +
                "</head><body></body></html>";

            this.httpContext.Response.Write(strHtml);

            this.httpContext.Response.End();
        }
    }
}
