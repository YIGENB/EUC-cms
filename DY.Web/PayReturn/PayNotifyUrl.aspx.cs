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

namespace DY.Web.PayReturn
{
    public partial class PayNotifyUrl : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentInfo pinfo = SiteBLL.GetPaymentInfo(12);

            string[] wxkey = new SiteUtils().Split(pinfo.pay_config, ",");
            TenPayInfo tpinfo = new TenPayInfo("", wxkey[0], wxkey[1], wxkey[2], "");//string partnerId, string key, string appId, string appKey, string tenPayNotify
         
            Senparc.Weixin.MP.TenPayLib.ResponseHandler resHandler = new Senparc.Weixin.MP.TenPayLib.ResponseHandler(null);
            resHandler.Init();
            resHandler.SetKey(tpinfo.Key, tpinfo.AppKey);

            string message;

            //判断签名
            if (resHandler.IsTenpaySign())
            {
                if (resHandler.IsWXsign())
                {
                    //商户在收到后台通知后根据通知ID向财付通发起验证确认，采用后台系统调用交互模式
                    string notify_id = resHandler.GetParameter("notify_id");
                    //取结果参数做业务处理
                    string out_trade_no = resHandler.GetParameter("out_trade_no");
                    //财付通订单号
                    string transaction_id = resHandler.GetParameter("transaction_id");
                    //金额,以分为单位
                    string total_fee = resHandler.GetParameter("total_fee");
                    //如果有使用折扣券，discount有值，total_fee+discount=原请求的total_fee
                    string discount = resHandler.GetParameter("discount");
                    //支付结果
                    string trade_state = resHandler.GetParameter("trade_state");

                    string payMessage = null;

                    //即时到账
                    if ("0".Equals(trade_state))
                    {
                        //------------------------------
                        //处理业务开始
                        //------------------------------

                        //处理数据库逻辑
                        //注意交易单不要重复处理
                        //注意判断返回金额

                        //------------------------------
                        //处理业务完毕
                        //------------------------------

                        //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知
                        payMessage = "success 后台通知成功";
                    }
                    else
                    {
                        payMessage = "支付失败";
                    }
                    //ViewData["payMessage"] = payMessage;
                    //回复服务器处理成功
                    message = "success";
                }

                else
                {//SHA1签名失败
                    message = "SHA1签名失败" + resHandler.GetDebugInfo();
                }
            }

            else
            {//md5签名失败
                message = "md5签名失败" + resHandler.GetDebugInfo();
            }
            //ViewData["message"] = message;
        }
    }
}