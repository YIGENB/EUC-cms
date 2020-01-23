using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI;

namespace DY.Site
{
    /// <summary>
    /// 完成功能如下
    /// 1:支付请求
    /// 2:支付结果处理。
    /// 3:查询订单请求.
    /// 4:查询订单结果处理.
    /// </summary>
    public class tenpay
    {
        /// <summary>
        /// 参数附值,很多参数附值后并不能传给财付通前台验证
        /// 这里只是给予一个参照.
        /// </summary>
        private string chnid = "335588";
        /// <summary>
        /// 平台提供者的财付通账号
        /// </summary>
        /// <value>The chnid.</value>
        public string Chnid
        {
            get { return chnid; }
            set { chnid = value; }
        }

        private int cmdno = 12;
        /// <summary>
        /// 任务代码,暂定值:12
        /// </summary>
        /// <value>The cmdno.</value>
        public int Cmdno
        {
            get { return cmdno; }
            set { cmdno = value; }
        }

        private int encode_type = 1;
        /// <summary>
        /// 1：GB2312编码，默认为GB2312编码。2：UTF-8编码。
        /// </summary>
        /// <value>The encode_type.</value>
        public int Encode_type
        {
            get { return encode_type; }
            set { encode_type = value; }
        }

        private string mch_desc = "测试交易描述";
        /// <summary>
        /// 交易说明，不能包含&lt; &gt;’”%特殊字符
        /// </summary>
        /// <value>The mch_desc.</value>
        public string Mch_desc
        {
            get { return mch_desc; }
            set { mch_desc = value; }
        }

        private string mch_name = "测试商品";
        /// <summary>
        /// 商品名称，不能包含&lt; &gt;’”%特殊字符
        /// </summary>
        /// <value>The mch_name.</value>
        public string Mch_name
        {
            get { return mch_name; }
            set { mch_name = value; }
        }

        private int mch_price = 1;
        /// <summary>
        /// 商品总价，单位为分。而财付通界面不再允许选择数量
        /// </summary>
        /// <value>The mch_price.</value>
        public int Mch_price
        {
            get { return mch_price; }
            set { mch_price = value; }
        }

        private int mch_type = 1;
        /// <summary>
        /// 交易类型：1、实物交易，2、虚拟交易
        /// </summary>
        /// <value>The mch_type.</value>
        public int Mch_type
        {
            get { return mch_type; }
            set { mch_type = value; }
        }

        private int need_buyerinfo = 2;
        /// <summary>
        /// 是否需要在财付通填定物流信息，1：需要，2：不需要。
        /// </summary>
        /// <value>The need_buyerinfo.</value>
        public int Need_buyerinfo
        {
            get { return need_buyerinfo; }
            set { need_buyerinfo = value; }
        }

        private string sign = "";
        /// <summary>
        /// 签名
        /// </summary>
        /// <value>The sign.</value>
        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }

        private string seller = "88881491";
        /// <summary>
        /// 收款方财付通账号
        /// </summary>
        /// <value>The seller.</value>
        public string Seller
        {
            get { return seller; }
            set { seller = value; }
        }

        private string transport_desc = "none";
        /// <summary>
        /// 物流公司或物流方式说明
        /// </summary>
        /// <value>The transport_desc.</value>
        public string Transport_desc
        {
            get { return transport_desc; }
            set { transport_desc = value; }
        }

        private string version = "2";
        /// <summary>
        /// 版本号
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private int total_fee = 1;
        /// <summary>
        /// 订单总价,单位为分
        /// </summary>
        /// <value>The total_fee.</value>
        public int Total_fee
        {
            get { return total_fee; }
            set { total_fee = value; }
        }

        private int trade_price = 1;
        /// <summary>
        /// 商品总价格
        /// </summary>
        /// <value>The trade_price.</value>
        public int Trade_price
        {
            get { return trade_price; }
            set { trade_price = value; }
        }


        private int transport_fee = 0;

        /// <summary>
        ///物流价格
        /// </summary>
        /// <value>The transport_fee.</value>
        public int Transport_fee
        {
            get { return transport_fee; }
            set { transport_fee = value; }
        }



        //商户定单号,只能为数字
        private string mch_vno = "1122334455";
        /// <summary>
        /// 商家定单号，此参数仅在对账时提供。YYYYMMDDXXXX
        /// </summary>
        /// <value>The mch_vno.</value>
        public string Mch_vno
        {
            get { return mch_vno; }
            set { mch_vno = value; }
        }

        private int retcode = 0;
        /// <summary>
        /// 0,交易成功，其它值，标识交易失败的状态。
        /// </summary>
        /// <value>The retcode.</value>
        public int Retcode
        {
            get { return retcode; }
            set { retcode = value; }
        }

        private string status = "3";
        /// <summary>
        /// 交易状态：1交易创建 2收获地址填写完毕 3买家付款成功 4卖家发货成功
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string buyer_id = "40669760";
        /// <summary>
        /// 买家财付通帐号
        /// </summary>
        /// <value>The buyer_id.</value>
        public string Buyer_id
        {
            get { return buyer_id; }
            set { buyer_id = value; }
        }

        private string cft_tid = "";
        /// <summary>
        /// 财付通交易单号
        /// </summary>
        /// <value>The cft_tid.</value>
        public string Cft_tid
        {
            get { return cft_tid; }
            set { cft_tid = value; }
        }

        private string key = "88881491";
        /// <summary>
        /// 商户KEY（替换为自已的KEY）
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string paygateurl = "https://www.tenpay.com/cgi-bin/med/show_opentrans.cgi";/// 财付通支付网关URL

        private string querygateurl = "https://www.tenpay.com/cgi-bin/med/query_opentrans.cgi";/// 财付通查询请求URL

        /// <summary>
        /// 支付结果回跳页面
        /// 推荐使用ip地址的方式(最长255个字符)
        /// 可以使用相对地址或配置,在使用前拼装全地址就行.这样方便部署.
        /// </summary>
        private string mch_returl = "";

        /// <summary>
        /// 支付结果回跳页面
        /// </summary>
        public string Mch_returl {
            get { return mch_returl; }
            set { mch_returl = value; }
        }

        /// <summary>
        /// 查询结果回跳页面
        /// 推荐使用ip地址的方式(最长255个字符)
        /// 可以使用相对地址或配置,在使用前拼装全地址就行.这样方便部署.
        /// </summary>
        private string show_url = "";

        /// <summary>
        /// 查询结果回跳页面
        /// </summary>
        public string Show_url {
            get { return show_url; }
            set { show_url = value; }
        }

        private const int querycmdno = 2;/// 查询命令.2

        private string date;//日期

        #region 日期字段设置,格式为yyyyMMdd

        /// <summary>
        /// 支付日期,yyyyMMdd
        /// </summary>
        public string Date
        {
            get
            {
                if (date == null)
                {
                    date = DateTime.Now.ToString("yyyyMMdd");
                }

                return date;
            }
            set
            {
                if (value == null || value.Trim().Length != 8)
                {
                    date = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    try
                    {
                        string strTmp = value.Trim();
                        date = DateTime.Parse(strTmp.Substring(0, 4) + "-" + strTmp.Substring(4, 2) + "-"
                            + strTmp.Substring(6, 2)).ToString("yyyyMMdd");
                    }
                    catch
                    {
                        date = DateTime.Now.ToString("yyyyMMdd");
                    }

                }
            }
        }

        #endregion

        private string attach = "test11";
        /// <summary>
        /// 指令标识,每次指令都会有这个字段,财付通在处理完成后会原样返回.
        /// </summary>
        public string Attach
        {
            get { return UrlDecode(attach); }
            set { attach = UrlEncode(value); }
        }

        private string payerrmsg = "";
        /// <summary>
        /// 如果为支付失败时,财付通返回的错误信息
        /// </summary>
        public string PayErrMsg
        {
            get { return payerrmsg; }
        }

        /// <summary>
        /// 对字符串进行URL编码
        /// </summary>
        /// <param name="instr">待编码的字符串</param>
        /// <returns>编码结果</returns>
        private static string UrlEncode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {

                return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26").
                       Replace("\"", "%22").Replace("?", "%3f").Replace("'", "%27").Replace(" ", "%20");
            }
        }

        /// <summary>
        /// 对字符串进行URL解码
        /// </summary>
        /// <param name="instr">待解码的字符串</param>
        /// <returns>解码结果</returns>
        private static string UrlDecode(string instr)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                return instr.Replace("%3d", "=").Replace("%26", "&").Replace("%22", "\"").Replace("%3f", "?")
                    .Replace("%27", "'").Replace("%20", " ").Replace("%25", "%");
            }
        }

        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        private static string GetMD5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("gb2312").GetBytes(encypStr);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public tenpay()
        {
            //payment = (DY_Payment)(new config().bll_payment.GetEntity(DY_Payment._.pay_code == "TenPay"));

            //if (payment != null)
            //{
            //    this.Seller = (payment.pay_config + ",,").Split(',')[0];
            //    this.Key = (payment.pay_config + ",,").Split(',')[1];
            //}
        }

        /// <summary>
        /// 添加参数,惹参数值不为空串,则添加。反之,不添加。
        /// </summary>
        protected StringBuilder AddParameter(StringBuilder buf,
            String parameterName,
            String parameterValue)
        {

            if (null == parameterValue || "".Equals(parameterValue))
            {
                return buf;
            }

            if ("".Equals(buf.ToString()))
            {
                buf.Append(parameterName);
                buf.Append("=");
                buf.Append(parameterValue);
            }
            else
            {
                buf.Append("&");
                buf.Append(parameterName);
                buf.Append("=");
                buf.Append(parameterValue);
            }
            return buf;
        }

        /// <summary>
        /// 获取支付签名
        /// </summary>
        /// <returns>根据参数得到签名</returns>
        public string GetPaySign()
        {
            StringBuilder buf = new StringBuilder();
            AddParameter(buf, "attach", attach);
            AddParameter(buf, "chnid", chnid);
            AddParameter(buf, "cmdno", (cmdno).ToString());
            AddParameter(buf, "encode_type", (encode_type).ToString());
            AddParameter(buf, "mch_desc", mch_desc);
            AddParameter(buf, "mch_name", mch_name);
            AddParameter(buf, "mch_price", (mch_price).ToString());
            AddParameter(buf, "mch_returl", mch_returl);
            AddParameter(buf, "mch_type", (mch_type).ToString());
            AddParameter(buf, "mch_vno", mch_vno);
            AddParameter(buf, "need_buyerinfo", (need_buyerinfo).ToString());
            AddParameter(buf, "seller", seller);
            AddParameter(buf, "show_url", show_url);
            AddParameter(buf, "transport_desc", transport_desc);
            AddParameter(buf, "transport_fee", (transport_fee).ToString());
            AddParameter(buf, "version", version);
            AddParameter(buf, "key", key);

            return GetMD5(buf.ToString());
        }

        /// <summary>
        /// 获取支付结果签名
        /// </summary>
        /// <returns>根据参数得到签名</returns>
        public string GetPayResultSign()
        {
            StringBuilder buf = new StringBuilder();
            AddParameter(buf, "attach", attach);
            AddParameter(buf, "buyer_id", buyer_id);
            AddParameter(buf, "cft_tid", (cft_tid).ToString());
            AddParameter(buf, "chnid", (chnid).ToString());
            AddParameter(buf, "cmdno", (cmdno).ToString());
            AddParameter(buf, "mch_vno", mch_vno);
            AddParameter(buf, "retcode", (retcode).ToString());
            AddParameter(buf, "seller", seller);
            AddParameter(buf, "status", (status).ToString());
            AddParameter(buf, "total_fee", (total_fee).ToString());
            AddParameter(buf, "trade_price", (trade_price).ToString());
            AddParameter(buf, "transport_fee", (transport_fee).ToString());
            AddParameter(buf, "version", version);
            AddParameter(buf, "key", key);

            return GetMD5(buf.ToString());
        }

        /// <summary>
        /// 获取支付页面URL
        /// </summary>
        /// <param name="url">如果函数返回真,是支付URL,如果函数返回假,是错误信息</param>
        /// <returns>函数执行是否成功</returns>
        public bool GetPayUrl(out string url)
        {
            try
            {
                string sign = GetPaySign();

                url = paygateurl + "?attach=" + attach + "&chnid=" + chnid + "&cmdno=" + cmdno + "&encode_type=" + encode_type + "&mch_desc=" + mch_desc
                + "&mch_name=" + mch_name + "&mch_price=" + mch_price + "&mch_returl="
                + mch_returl + "&mch_type=" + mch_type + "&mch_vno=" + mch_vno + "&need_buyerinfo=" + need_buyerinfo + "&seller=" + seller
                + "&show_url=" + show_url + "&transport_desc=" + transport_desc + "&transport_fee=" + transport_fee + "&version=" + version + "&sign=" + sign;

                return true;
            }
            catch (Exception err)
            {
                url = "创建URL时出错,错误信息:" + err.Message;
                return false;
            }
        }

        /// <summary>
        /// 从支付结果页面的URL请求参数中获取结果信息
        /// </summary>
        /// <param name="querystring">支付结果页面的URL请求参数</param>
        /// <param name="errmsg">函数执行不成功的话,返回错误信息</param>
        /// <returns>函数执行是否成功</returns>
        public bool GetPayValueFromUrl(NameValueCollection querystring, out string errmsg)
        {

            //结果URL参数样例如下
            /*
            ?cmdno=1&pay_result=0&pay_info=OK&date=20070423&seller=1201143001&transaction_id=1201143001200704230000000013
            &sp_billno=13&mch_price=1&fee_type=1&attach=%D5%E2%CA%C7%D2%BB%B8%F6%B2%E2%CA%D4%BD%BB%D2%D7%B5%A5				
            &sign=ADD7475F2CAFA793A3FB35051869E301
            */
            #region 进行参数校验

            if (querystring == null || querystring.Count == 0)
            {
                errmsg = "参数为空";
                return false;
            }

            if (querystring["cmdno"] == null || querystring["cmdno"].ToString().Trim() != cmdno.ToString())
            {
                errmsg = "没有cmdno参数或cmdno参数不正确";
                return false;
            }


            if (querystring["seller"] == null)
            {
                errmsg = "没有seller参数";
                return false;
            }

            if (querystring["buyer_id"] == null)
            {
                errmsg = "没有buyer_id参数";
                return false;
            }

            if (querystring["cft_tid"] == null)
            {
                errmsg = "没有cft_tid参数";
                return false;
            }

            if (querystring["mch_vno"] == null)
            {
                errmsg = "没有mch_vno参数";
                return false;
            }

            if (querystring["retcode"] == null)
            {
                errmsg = "没有retcode参数";
                return false;
            }

            if (querystring["status"] == null)
            {
                errmsg = "没有status参数";
                return false;
            }

            if (querystring["total_fee"] == null)
            {
                errmsg = "没有total_fee参数";
                return false;
            }

            if (querystring["attach"] == null)
            {
                errmsg = "没有attach参数";
                return false;
            }

            if (querystring["trade_price"] == null)
            {
                errmsg = "没有trade_price参数";
                return false;
            }

            if (querystring["transport_fee"] == null)
            {
                errmsg = "没有transport_fee参数";
                return false;
            }

            //			if(querystring["version"] == null)
            //			{
            //				errmsg = "没有version参数";
            //				return false;
            //			}

            if (querystring["chnid"] == null)
            {
                errmsg = "没有chnid参数";
                return false;
            }
            if (querystring["sign"] == null)
            {
                errmsg = "没有sign参数";
                return false;
            }

            #endregion
            errmsg = "";
            try
            {
                attach = querystring["attach"];
                buyer_id = querystring["buyer_id"];
                cft_tid = querystring["cft_tid"];
                chnid = querystring["chnid"];
                cmdno = Int32.Parse(querystring["cmdno"]);
                mch_vno = querystring["mch_vno"];
                retcode = Int32.Parse(querystring["retcode"]);
                seller = querystring["seller"];
                status = querystring["status"];
                total_fee = Int32.Parse(querystring["total_fee"]);
                trade_price = Int32.Parse(querystring["trade_price"]);
                transport_fee = Int32.Parse(querystring["transport_fee"]);
                version = querystring["version"];

                string strsign = querystring["sign"];
                string sign = GetPayResultSign();


                if (sign != strsign)//验证签名
                {
                    errmsg = "验证签名失败";
                    return false;
                }

                return true;
            }
            catch (Exception err)
            {
                errmsg = "解析参数出错:" + err.Message;
                return false;
            }
        }
    }
}
