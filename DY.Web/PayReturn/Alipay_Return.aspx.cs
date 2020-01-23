using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using System.Collections;

namespace DY.Web.PayReturn
{
    public partial class Alipay_Return : DY.Site.WebPage //System.Web.UI.Page
    {

        #region 定义公共变量

        //订单号
        private String out_trade_no { get { return Request.QueryString["out_trade_no"]; } }

        //订单金额
        private String total_fee { get { return Request.QueryString["total_fee"]; } }


        //订单信息
        private string _orderInfo = "";
        protected String OrderInfo
        {
            get { return _orderInfo; }
            set { _orderInfo = value; }
        }

        private string _ImgState = "";
        protected String ImgState
        {
            get { return _ImgState; }
            set { _ImgState = value; }
        }

        private string _ddState = "";
        protected String DDState
        {
            get { return _ddState; }
            set { _ddState = value; }
        }
        #endregion

        /// <summary>
        /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
        /// </summary>

        public static string GetMD5(string s)
        {

            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public static string[] BubbleSort(string[] r)
        {
            /// <summary>
            /// 冒泡排序法
            /// </summary>

            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }

        //获取远程服务器ATN结果
        public String Get_Http(String a_strUrl, int timeout)
        {
            string strResult;
            try
            {

                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }

                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {

                strResult = "错误：" + exp.Message;
            }

            return strResult;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string title = "";
            string content = "";

            ///当不知道https的时候，请使用http
            //string alipayNotifyURL = "https://www.alipay.com/cooperate/gateway.do?";
            string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";

            DY.Entity.PaymentInfo payinfo = DY.Site.SiteBLL.GetPaymentInfo("pay_code='alipay'");
            if (payinfo == null)
            {
                Response.Write("错误提示：支付宝设置不正确！");
                return;
            }
            
            string partner = (payinfo.pay_config + ",,").Split(',')[2];			//partner	合作伙伴ID			保留字段
            string key = (payinfo.pay_config + ",,").Split(',')[1];   //partner  账户的支付宝安全校验码
            //alipayNotifyURL = alipayNotifyURL + "service=notify_verify" + "&partner=" + partner + "&notify_id=" + Request.QueryString["notify_id"];
            alipayNotifyURL = alipayNotifyURL + "&partner=" + partner + "&notify_id=" + Request.QueryString["notify_id"];

            string responseTxt = Get_Http(alipayNotifyURL, 120000);
            //*********************************************************************************************
            int i;
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestarr = coll.AllKeys;



            //进行排序；
            string[] Sortedstr = BubbleSort(requestarr);

            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
                {
                    if (i == Sortedstr.Length - 1)
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.QueryString[Sortedstr[i]]);

                    }
                    else
                    {

                        prestr.Append(Sortedstr[i] + "=" + Request.QueryString[Sortedstr[i]] + "&");
                    }
                }
            }

            prestr.Append(key);

            //生成Md5摘要；
            string mysign = GetMD5(prestr.ToString());

            string sign = Request.QueryString["sign"];

            if (mysign == sign && responseTxt == "true")   //验证支付发过来的消息，签名是否正确
            {
                DY.Entity.OrderInfoInfo orderinfo = DY.Site.SiteBLL.GetOrderInfoInfo("order_sn='" + out_trade_no + "'");
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
                    user.UpdateUserIntegral(out_trade_no);

                    title = "恭喜您，付款成功！";
                    content += "订 单 号：" + out_trade_no + "</br>";
                    content += "成功支付金额：" + total_fee;
                }
            }
            else
            {
                title = "很抱歉，付款失败！";
                content = "多次提交订单失败,请与我们的客户联系。";
            }

            IDictionary context = new Hashtable();
            context.Add("pay_title", title);
            context.Add("pay_content", content);

            base.DisplayTemplate(context, "store/payreturn");
           // Session["OrderID"] = null;
           // string TOEXCELLR = "MD5结果:mysign=" + mysign + ",sign=" + sign + ",responseTxt=" + responseTxt;
           // TOEXCELLR = TOEXCELLR + "get:" + HttpContext.Current.Request.QueryString["out_trade_no"].ToString();
            //写文本，纪录支付宝返回消息，比对md5计算结果（如网站不支持写txt文件，可改成写数据库）
          //  StreamWriter fs = new StreamWriter(Server.MapPath("PayReturn/" + DateTime.Now.ToString().Replace(":", "")) + ".txt", false, System.Text.Encoding.Default);
          //  fs.Write(TOEXCELLR);
           // fs.Close();
        }
    }
}