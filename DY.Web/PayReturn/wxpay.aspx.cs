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
using System.Text;
using Senparc.Weixin.MP.TenPayLib;

namespace DY.Web
{
    public partial class wxpay : WebPage
    {
        public String parm;
        //url编码，添加空格转成%20
        public string UrlEncode1(string con)
        {
            string UrlEncode = "";
            UrlEncode = HttpUtility.UrlEncode(con, Encoding.UTF8);
            UrlEncode = UrlEncode.Replace("+", "%20");
            return UrlEncode;
        }
        //' * google api 二维码生成【QRcode可以存储最多4296个字母数字类型的任意文本，具体可以查看二维码数据格式】
        //' * @param string $chl 二维码包含的信息，可以是数字、字符、二进制信息、汉字。不能混合数据类型，数据必须经过UTF-8 URL-encoded.如果需要传递的信息超过2K个字节请使用POST方式
        //' * @param int $widhtHeight 生成二维码的尺寸设置
        //' * @param string $EC_level 可选纠错级别，QR码支持四个等级纠错，用来恢复丢失的、读错的、模糊的、数据。
        //' *                         L-默认：可以识别已损失的7%的数据
        //' *                         M-可以识别已损失15%的数据
        //' *                         Q-可以识别已损失25%的数据
        //' *                         H-可以识别已损失30%的数据
        //' * @param int $margin 生成的二维码离图片边框的距离
        public string QRfromGoogle(string chl)
        {
            int widhtHeight = 300;
            string EC_level = "L";
            int margin = 0;
            string QRfromGoogle;
            chl = UrlEncode1(chl);
            QRfromGoogle = "http://chart.apis.google.com/chart?chs=" + widhtHeight + "x" + widhtHeight + "&cht=qr&chld=" + EC_level + "|" + margin + "&chl=" + chl;
            return QRfromGoogle;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string domain = "http://" + siteUtils.GetDomain() + "/PayReturn/PayNotifyUrl.aspx"; //返回页面
            string sp_billno = Request["order_no"];//订单号
            PaymentInfo pinfo = SiteBLL.GetPaymentInfo(12);
            
            string[] wxkey = new SiteUtils().Split(pinfo.pay_config, ",");
            TenPayInfo tpinfo = new TenPayInfo(sp_billno, wxkey[0], wxkey[1], wxkey[2], domain);//string partnerId, string key, string appId, string appKey, string tenPayNotify
            
            //当前时间 yyyyMMdd
            string date = DateTime.Now.ToString("yyyyMMdd");
            //订单号，此处用时间和随机数生成，商户根据自己调整，保证唯一
            string out_trade_no = "" + DateTime.Now.ToString("HHmmss") + TenPayUtil.BuildRandomStr(4);

            if (null == sp_billno)
            {
                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                sp_billno = DateTime.Now.ToString("HHmmss") + TenPayUtil.BuildRandomStr(4);
            }
            else
            {
                sp_billno = Request["order_no"].ToString();
            }

            sp_billno = tpinfo.PartnerId + sp_billno;



            //创建RequestHandler实例
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            packageReqHandler.SetKey(tpinfo.Key);

            //设置package订单参数
            packageReqHandler.SetParameter("partner", tpinfo.PartnerId);		  //商户号
            packageReqHandler.SetParameter("bank_type", "WX");		                      //银行类型
            packageReqHandler.SetParameter("fee_type", "1");                    //币种，1人民币
            packageReqHandler.SetParameter("input_charset", "GBK");
            packageReqHandler.SetParameter("out_trade_no", sp_billno);		//商家订单号
            packageReqHandler.SetParameter("total_fee", "1");			        //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.SetParameter("notify_url", tpinfo.TenPayNotify);		    //接收财付通通知的URL
            packageReqHandler.SetParameter("body", "nativecall");	                    //商品描述
            packageReqHandler.SetParameter("spbill_create_ip", "8.8.8.8"/*Page.Request.UserHostAddress*/);   //用户的公网ip，不是商户服务器IP

            //获取package包
            string packageValue = packageReqHandler.GetRequestURL();

            //调起微信支付签名
            string timeStamp = TenPayUtil.GetTimestamp();
            string nonceStr = TenPayUtil.GetNoncestr();

            //设置支付参数
            RequestHandler payHandler = new RequestHandler(null);
            payHandler.SetParameter("appid", tpinfo.AppId);
            payHandler.SetParameter("noncestr", nonceStr);
            payHandler.SetParameter("timestamp", timeStamp);
            payHandler.SetParameter("package", packageValue);
            payHandler.SetParameter("RetCode", "0");
            payHandler.SetParameter("RetErrMsg", "成功");
            string paySign = payHandler.CreateSHA1Sign();
            payHandler.SetParameter("app_signature", paySign);
            payHandler.SetParameter("sign_method", "SHA1");


            Response.ContentType = "text/xml";
            Response.Clear();
            Response.Write(payHandler.ParseXML());
            
        }
    }
}