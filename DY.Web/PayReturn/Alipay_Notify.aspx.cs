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

namespace DY.Web.PayReturn
{
    public partial class Alipay_Notify : DY.Site.WebPage //System.Web.UI.Page
    {
        /// <summary>
        /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
        /// </summary>

        public static string GetMD5(string s)
        {

            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(s));
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
            //*****************************************************************************************
            ///当不知道https的时候，请使用http
            string alipayNotifyURL = "https://www.alipay.com/cooperate/gateway.do?";
            //string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";

            DY.Entity.PaymentInfo payinfo = DY.Site.SiteBLL.GetPaymentInfo("pay_code='alipay'");
            if (payinfo == null)
            {
                Response.Write("错误提示：支付宝设置不正确！");
                return;
            }

            string partner = (payinfo.pay_config + ",,").Split(',')[2];			//partner	合作伙伴ID			保留字段
            string key = (payinfo.pay_config + ",,").Split(',')[1];   //partner  账户的支付宝安全校验码

            //string partner = "2088101056860454"; 		//*********partner合作伙伴id（必须填写）
            //string key = "jg61wgyirqxlgn17c2k8lz6yczxjiim2"; //************partner 的对应交易安全校验码（必须填写）

            alipayNotifyURL = alipayNotifyURL + "service=notify_verify" + "&partner=" + partner + "&notify_id=" + Request.Form["notify_id"];
            // alipayNotifyURL = alipayNotifyURL + "&partner=" + partner + "&notify_id=" + Request.Form["notify_id"];

            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            string responseTxt = Get_Http(alipayNotifyURL, 120000);


            //****************************************************************************************
            int i;
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestarr = coll.AllKeys;



            //进行排序；
            string[] Sortedstr = BubbleSort(requestarr);

            //for (i = 0; i < Sortedstr.Length; i++)
            //{
            //  Response.Write("Form: " + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]] + "<br>");
            //}

            //构造待md5摘要字符串 ；
            string prestr = "";
            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (Request.Form[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
                {
                    if (i == Sortedstr.Length - 1)
                    {
                        prestr = prestr + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]];
                    }
                    else
                    {
                        prestr = prestr + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]] + "&";
                    }
                }

            }
            prestr = prestr + key;

            string mysign = GetMD5(prestr);


            string sign = Request.Form["sign"];



            if (mysign == sign && responseTxt == "true")   //验证支付发过来的消息，签名是否正确
            {

                //更新自己数据库的订单语句，请自己填写一下
               // operate.ExecuteSQL("exec CradOrderUpdate '" + HttpContext.Current.Request.Form["out_trade_no"].ToString() + "'");
                Response.Write("success");     //返回给支付宝消息，成功
            }
            else
            Response.Write("success");
            string TOEXCELLR = "MD5结果:mysign=" + mysign + ",sign=" + sign + ",responseTxt=" + responseTxt;
            TOEXCELLR = TOEXCELLR + "post:" + HttpContext.Current.Request.Form["out_trade_no"].ToString();
            //写文本，纪录支付宝返回消息，比对md5计算结果（如网站不支持写txt文件，可改成写数据库）
            StreamWriter fs = new StreamWriter(Server.MapPath("Notify_DATA/" + DateTime.Now.ToString().Replace(":", "")) + ".txt", false, System.Text.Encoding.Default);
            fs.Write(TOEXCELLR);
            fs.Close();
        }
    }
}