using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

namespace DY.Site
{
    /// <summary>
    /// 快钱支付接口
    /// </summary>
    public class kuaiqian
    {
        #region 参数说明
        //字符集  长度2
        //可为空  固定选择值：1,2,3 默认值为1
        //1 代表UTF-8 
        //2 代表GBK
        //3 代表GB2312
        public string inputCharset = "";
        //接受支付结果的页面地址  长度256
        //可为空  需要绝对地址，与bgUrl不能同时为空，当bgUrl为空时，快钱直接将支付结果GET到pageUrl，当bgUrl不为空时，按照bgUrl的方式返回
        public string pageUrl = "";
        //服务器接受支付结果的后台地址 长度256
        //可为空 需呀绝对地址，与pageUrl不能同时为空，快钱将支付结果发送到bgUrl对应的地址，并且获得商户按照约定格式输出的地址，显示页面给用户
        public string bgUrl { get; set; }
        //网关版本 长度10
        //不可空 固定值：v2.0
        public string version = "";
        //网关页面显示语言种类 长度2
        //不可空  固定值：1(代表中文显示)
        public string language = "";

        //签名类型 长度2
        //不可空  固定值：1
        // 1 代表MD5加密签名方式
        // 4 代表PKI证书签名方式
        public String signType = "";

        //人民币账号 长度30
        //不可空  数字串，用来指定接收款项的人民币账号
        public String merchantAcctId = "";

        //支付人姓名  长度32
        //可为空 英文或中文字符
        public String payerName = "";

        //支付人联系方式类型 长度2
        //可为空  固定值：1(代表电子邮件方式)
        public String payerContactType = "";

        //支付人联系方式  长度50
        //可为空 字符串根据payerContactTpye的方式填写对应字符
        public String payerContact = "";

        //商户订单号 长度50
        //不可为空 字符串只允许使用字母，数字，-，_，并以字母或数字开头每商户提交的订单号，必须在自身账户交易中唯一
        public String orderId = "";

        // 商户订单金额 长度10
        //不可为空 以分为单位，比方10元，提交时金额应为1000
        public String orderAmount = "";

        //商户订单提交时间 长度14
        //不可为空 数字串，一共14位。
        //格式为:年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]
        //例如：20071117020101
        public String orderTime = "";

        //商品名称 长度256
        // 可为空
        public String productName = "";

        //商品数量 长度8
        //可为空  整型数字
        public String productNum = "";

        //商品代码 长度 20
        //可为空 
        //必须为字母，数字或-,_的组合，如果商户发布了优惠券，并指向对指定的某商品或某类商品进行优惠时，请将此参数与发布优惠券
        //时设置的“使用商品”保持一致。只可填写一个代码。如果不使用优惠券，本参数不用填写
        public String productId = "";

        //商品描述 长度400
        //可为空
        public String productDesc = "";

        //扩展字段1 长度128
        //可为空
        public String ext1 = "";

        //扩展字段2 长度128
        //可以空 
        public String ext2 = "";

        //支付方式 长度
        //不可空 固定选择值:00,10,11,12,13,14,15,17
        //00：其他支付
        //10：银行卡网银支付
        //11：电话支付
        //12：快钱账户支付
        //13：线下支付
        //14：企业网银在线支付
        //15：信用卡在线支付
        //17: 预付卡支付
        public String payType = "";

        //银行代码 长度8
        //可为空 银行的代码，仅在银行直连时使用，银行代码表参见资料
        //银行直连功能需单独申请，默认不开通
        public String bankId = "";

        //同一订单禁止重复提交标志  长度1
        //可为空  固定选择值1,0  默认值为0
        public String redoFlag = "";

        //合作伙伴在快钱的用户编号 长度30
        //可以为空
        public String pid = "";

        //签名字符串 长度256
        //不可以为空
        public String signMsg = "";
        #endregion


        /**
        * @Description: 快钱人民币支付网关接口范例
        * @Copyright (c) 上海快钱信息服务有限公司
        * @version 2.0
        */
        /// <summary>
        /// 构造快钱支付方法
        /// </summary>
        /// <param name="partner">账户号</param>
        /// <param name="partnerkey">密钥</param>
        public string KuiQianMethod(string partner, string partnerkey)
        {
            inputCharset = "1";
            pageUrl = "";
            //bgUrl = "http://www.wlyx365.com/PayReturn/KQReceive.aspx";
            version = "v2.0";
            language = "1";
            signType = "1";
            merchantAcctId = partner;
            //payerName = "";
            //productNum = "";
            payerContactType = "";
            //orderId = DateTime.Now.ToString("yyyyMMddHHmmss");
            //orderAmount = "5";
            orderTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //payType = "00";
            string key = partnerkey;
            string signMsgVal = "";
            signMsgVal = appendParam(signMsgVal, "inputCharset", inputCharset);
            signMsgVal = appendParam(signMsgVal, "bgUrl", bgUrl);
            signMsgVal = appendParam(signMsgVal, "pageUrl", pageUrl);
            signMsgVal = appendParam(signMsgVal, "version", version);
            signMsgVal = appendParam(signMsgVal, "language", language);
            signMsgVal = appendParam(signMsgVal, "signType", signType);
            signMsgVal = appendParam(signMsgVal, "merchantAcctId", merchantAcctId);
            signMsgVal = appendParam(signMsgVal, "payerName", payerName);
            signMsgVal = appendParam(signMsgVal, "payerContactType", payerContactType);
            signMsgVal = appendParam(signMsgVal, "payerContact", payerContact);
            signMsgVal = appendParam(signMsgVal, "orderId", orderId);
            signMsgVal = appendParam(signMsgVal, "orderAmount", orderAmount);
            signMsgVal = appendParam(signMsgVal, "orderTime", orderTime);
            signMsgVal = appendParam(signMsgVal, "productName", productName);
            signMsgVal = appendParam(signMsgVal, "productNum", productNum);
            signMsgVal = appendParam(signMsgVal, "productId", productId);
            signMsgVal = appendParam(signMsgVal, "productDesc", productDesc);
            signMsgVal = appendParam(signMsgVal, "ext1", ext1);
            signMsgVal = appendParam(signMsgVal, "ext2", ext2);
            signMsgVal = appendParam(signMsgVal, "payType", payType);
            signMsgVal = appendParam(signMsgVal, "bankId", bankId);
            signMsgVal = appendParam(signMsgVal, "redoFlag", redoFlag);
            signMsgVal = appendParam(signMsgVal, "pid", pid);
            signMsgVal = appendParam(signMsgVal, "key", key);

            //如果在web.config文件中设置了编码方式，例如<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>（如未设则默认为utf-8），
            //那么，inputCharset的取值应与已设置的编码方式相一致；
            //同时，GetMD5()方法中所传递的编码方式也必须与此保持一致。
            string signMsg = GetMD5(signMsgVal, "utf-8").ToUpper();
            // string signMsg = FormsAuthentication.HashPasswordForStoringInConfigFile(signMsgVal, "MD5").ToUpper();
            string myurl = "https://www.99bill.com/gateway/recvMerchantInfoAction.htm?" + signMsgVal + "&signMsg=" + signMsg;
            return myurl.ToString();
        }


        //功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。开始
        #region   MD5加密算法
        private static string GetMD5(string dataStr, string codeType)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(codeType).GetBytes(dataStr));
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        #endregion

        //功能函数。将变量值不为空的参数组成字符串
        #region 字符串串联函数
        public string appendParam(string returnStr, string paramId, string paramValue)
        {
            if (returnStr != "")
            {
                if (paramValue != "")
                {
                    returnStr += "&" + paramId + "=" + paramValue;
                }
            }
            else
            {
                if (paramValue != "")
                {
                    returnStr = paramId + "=" + paramValue;
                }
            }
            return returnStr;
        }
        #endregion
    }
}
