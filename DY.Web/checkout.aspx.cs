/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Collections.Generic;
using Com.Alipay;
using tenpay;

namespace DY.Web
{
    public partial class checkout : WebPage
    {
        protected decimal bonus = 0;
        protected decimal delivery_fee = 0;
        protected decimal discount = 0;
        protected decimal goods_amount = 0;
        protected decimal cashcart = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 验证购物车是否有商品，如果没有则跳出
            if (Store.SumCartGoodsPrice() <= 0 && base.act != "payment")
            {
                Response.Write("<script>alert('您的购物车还没有商品。');location.href='cart.aspx';</script>");
                Response.End();
            }
            #endregion

            IDictionary context = new Hashtable();
            this.goods_amount = Store.SumCartGoodsPrice();
            this.discount = goods_amount - new CashFacade().GetFactTotal(Store.SumCartGoodsPrice());

            #region 验证优惠券
            if (base.act == "validata_bonus")
            {
                int state = -1;
                decimal bonus_fee = 0;
                this.ValidataBonus(context, true, out state, out bonus_fee);
            }
            #endregion

            #region 改变配送方式
            else if (base.act == "change_delivery")
            {
                this.ChangeDelivery(context);
            }
            #endregion

            #region 生成订单
            else if (base.act == "creat_order")
            {
                this.CreatOrder(context);
            }
            #endregion

            #region 订单支付
            else if (base.act == "payment")
            {
                this.DispalyPaymentInfo(context, DYRequest.getRequest("order_sn"));
            }
            #endregion

            #region 显示购物车
            else
            {
                ShowDefault(context);
            }
            #endregion
        }
        /// <summary>
        /// 购物信息页面
        /// </summary>
        /// <param name="context"></param>
        protected void ShowDefault(IDictionary context)
        {
            context.Add("cartlist", Store.GetCartList());
            context.Add("shipping_list", SiteBLL.GetDeliveryAllList("orderid desc,delivery_id asc", "enabled=1"));
            context.Add("delivery_list", SiteBLL.GetDeliveryAddressAllList("is_checked desc,id desc", "userid="+base.userid));
            context.Add("payment_list", SiteBLL.GetPaymentAllList("pay_order desc,pay_id asc", "enabled=1"));
            context.Add("bonus", this.bonus);
            context.Add("delivery_fee", this.delivery_fee);
            context.Add("discount", discount);
            context.Add("goods_amount", goods_amount);
            cashcart = Store.GetCashCart();
            if (cashcart == 0)
            {
                context.Add("show_delivery_fee", false);
            }
            else
            {
                context.Add("show_delivery_fee", this.goods_amount - this.discount >= Store.GetCashCart());
            }
            base.DisplayTemplate(context, "store/checkout", base.isajax);
        }
        /// <summary>
        /// 改变配送方式
        /// </summary>
        /// <param name="context"></param>
        protected void ChangeDelivery(IDictionary context)
        {
            int delivery_id = DYRequest.getRequestInt("delivery_id");
            DeliveryInfo deliinfo = SiteBLL.GetDeliveryInfo(delivery_id);
            if (deliinfo != null)
            {
                cashcart = Store.GetCashCart();
                if (cashcart == 0)
                {
                    this.delivery_fee = deliinfo.delivery_price.Value;
                }
                else
                {
                    if (this.goods_amount - this.discount >= Store.GetCashCart())
                        this.delivery_fee = 0;
                    else
                        this.delivery_fee = deliinfo.delivery_price.Value;
                }
            }
            this.bonus = DYRequest.getRequestDecimal("bonus");

            this.ShowDefault(context);
        }
        /// <summary>
        /// 验证优惠券
        /// </summary>
        /// <param name="context"></param>
        protected void ValidataBonus(IDictionary context, bool write, out int state, out decimal bonus_fee)
        {
            int bonus_sn = DYRequest.getFormInt("bonus_sn") > 0 ? DYRequest.getFormInt("bonus_sn") : DYRequest.getRequestInt("bonus_sn");
            state = 0;
            bonus_fee = 0;

            if (bonus_sn > 0)
            {
                object obj = SiteBLL.GetBonusValue("bonus_type_id", "bonus_sn=" + bonus_sn);
                if (obj != null)
                {
                    int bonus_type_id = Convert.ToInt32(obj);

                    //取得优惠券类型信息
                    BonusTypeInfo typeinfo = SiteBLL.GetBonusTypeInfo(bonus_type_id);
                    //取得优惠券信息
                    BonusInfo bonusinfo = SiteBLL.GetBonusInfo("bonus_sn=" + bonus_sn);

                    //判断是否有效
                    if (typeinfo != null)
                    {
                        //是否启用
                        if (!bonusinfo.is_enbled.Value)
                        {
                            base.DisplayJsonMessage("该提货卷" + bonus_sn + "还未正式启用！");
                            state = -1;
                        }

                        if (typeinfo.type_name == "注册送礼")
                        {
                            if (DateTime.Now < bonusinfo.use_start_date.Value || DateTime.Now > bonusinfo.use_end_date.Value)
                            {
                                base.DisplayJsonMessage("您所输入的优惠券" + bonus_sn + "还未到正式使用时间，请在" + bonusinfo.use_start_date.Value.ToString("yyyy-MM-dd") + "至" + bonusinfo.use_end_date.Value.ToString("yyyy-MM-dd") + "使用！");
                                state = -2;
                            }
                        }
                        else
                        {

                            //是否到期
                            if (typeinfo.use_start_date.Value > DateTime.Now)
                            {
                                base.DisplayJsonMessage("您所输入的优惠券" + bonus_sn + "还未到正式使用时间，请在" + typeinfo.use_start_date.Value.ToString("yyyy-MM-dd") + "至" + typeinfo.use_end_date.Value.ToString("yyyy-MM-dd") + "使用！");
                                state = -2;
                            }

                            //是否过期
                            if (typeinfo.use_end_date.Value < DateTime.Now)
                            {
                                base.DisplayJsonMessage("您所输入的提货卷" + bonus_sn + "在" + typeinfo.use_end_date.Value.ToString("yyyy-MM-dd") + "已经过期！");
                                state = -3;
                            }
                        }

                        //是否已使用
                        if (bonusinfo.used_time != null)
                        {
                            base.DisplayJsonMessage("您所输入的提货卷" + bonus_sn + "已经被使用过了！");
                            state = -4;
                        }

                        //订单商品金额是否达到
                        if (typeinfo.min_goods_amount.Value > 0)
                        {
                            if (Store.SumCartGoodsPrice() < typeinfo.min_goods_amount.Value)
                            {
                                base.DisplayJsonMessage("订单商品金额必须大于等于￥" + typeinfo.min_goods_amount.Value + "元才能使用该优惠券");
                                state = -5;
                            }
                        }

                        state = bonusinfo.bonus_id.Value;
                        bonus_fee = typeinfo.type_money.Value;
                        this.bonus = typeinfo.type_money.Value;
                        this.delivery_fee = DYRequest.getRequestDecimal("delivery_fee");
                        if (write)
                            this.ShowDefault(context);
                     
                    }
                }
            }
            else
            {
                base.DisplayJsonMessage("优惠券号码格式错误！");
            }
        }
        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="context"></param>
        protected void CreatOrder(IDictionary context)
        {
            #region 保存订单信息
            DeliveryInfo deliinfo = SiteBLL.GetDeliveryInfo(DYRequest.getFormInt("shipping"));
            PaymentInfo payinfo = SiteBLL.GetPaymentInfo(DYRequest.getFormInt("payment"));
            OrderInfoInfo entity = new OrderInfoInfo();
            DeliveryAddressInfo deliveryinfo=new DeliveryAddressInfo();

            #region 订单信息
            entity.user_id = base.userid;
            entity.order_sn = SiteUtils.MakeOrderSn();
            entity.order_status = 0;
            entity.add_time = DateTime.Now;
            entity.postscript = DYRequest.getForm("postscript");
            entity.referer = "本站";
            entity.id_delete = false;
            entity.integral_money = discount;
            entity.delivery_address_id = DYRequest.getFormInt("delivery_address_id");
            #endregion

            #region 收货人信息
            if (entity.delivery_address_id > 0)
                deliveryinfo = SiteBLL.GetDeliveryAddressInfo(entity.delivery_address_id.Value);
            entity.consignee = deliveryinfo.consignee;//DYRequest.getForm("consignee");
            entity.province = deliveryinfo.province;//DYRequest.getFormInt("province");
            entity.city = deliveryinfo.city;//DYRequest.getFormInt("city");
            entity.district = deliveryinfo.district;//DYRequest.getFormInt("district");
            entity.address = deliveryinfo.address;//DYRequest.getForm("address");
            entity.zipcode = deliveryinfo.zipcode;//DYRequest.getForm("zipcode");
            entity.tel = deliveryinfo.tel;//DYRequest.getForm("tel");
            entity.mobile = deliveryinfo.mobile;//DYRequest.getForm("mobile");
            entity.email = deliveryinfo.email;//DYRequest.getForm("email");
            entity.best_time = DYRequest.getForm("best_time");
            #endregion

            #region 发货方式
            if (deliinfo != null)
            {
                entity.delivery_id = deliinfo.delivery_id.Value;
                entity.delivery_name = deliinfo.delivery_name;
                cashcart = Store.GetCashCart();

                if (cashcart == 0)
                {
                    entity.delivery_fee = deliinfo.delivery_price.Value;
                }
                else
                {
                    if (this.goods_amount - this.discount >= Store.GetCashCart())
                    {
                        entity.delivery_fee = 0;
                    }
                    else
                    {
                        entity.delivery_fee = deliinfo.delivery_price.Value;
                    }
                }

            }
            #endregion

            #region 支付信息
            if (payinfo != null)
            {
                entity.pay_id = payinfo.pay_id.Value;
                entity.pay_name = payinfo.pay_name;
                entity.pay_fee = 0;
            }
            #endregion

            #region 优惠券
            int bonus_id = -1;
            decimal bonus_fee = 0;
            //验证优惠券

            if (DYRequest.getFormInt("bonus_sn") > 0)
                this.ValidataBonus(context, false, out bonus_id, out bonus_fee);
            if (bonus_id > 0)
            {
                entity.bonus_id = bonus_id;
                entity.bonus = bonus_fee;
            }
            #endregion


            //推广ID
            int pid = Utils.StrToInt(DY.Site.SiteUtils.GetCookie("pid", "DYPromotion"), 0);
            entity.pid = pid;

            //插入订单表
            int order_id = SiteBLL.InsertOrderInfoInfo(entity);

            int integral = 0;

            decimal goods_amount = 0.00m;

            #region 保存订单商品信息
            foreach (CartInfo cartinfo in Store.GetCartList())
            {
                GoodsInfo goods_model = new GoodsInfo();
                goods_model = SiteBLL.GetGoodsInfo(cartinfo.goods_id.Value);
                if (goods == null)
                {
                    return;
                }

                OrderGoodsInfo ordergoods = new OrderGoodsInfo();
                ordergoods.goods_id = goods_model.goods_id.Value;
                ordergoods.goods_name = goods_model.goods_name;
                ordergoods.goods_attr = cartinfo.goods_attr;
                ordergoods.goods_number = cartinfo.goods_number;

                //xfywl.com 2011-11-22 10:49:33
                //string[] cc = SiteBLL.GetGoodsAttrInfo("goods_id=" + ordergoods.goods_id + " and " + "attr_id=42").attr_value.Split(',');
                //string[] xj = SiteBLL.GetGoodsAttrInfo("goods_id=" + ordergoods.goods_id + " and " + "attr_id=44").attr_value.Split(',');
                //string[] attr = ordergoods.goods_attr.Split(',');

                //decimal price = goods_model.shop_price.Value;

                //if (attr.Length > 1 && cc.Length > 0 && xj.Length > 0)
                //{
                //    string cc2 = attr[1];
                //    if (!string.IsNullOrEmpty(cc2))
                //    {
                //        int price_i = 0;
                //        for (int i = 0; i < cc.Length; i++)
                //        {
                //            if (cc2 == cc[i])
                //            {
                //                price_i = i;
                //                break;
                //            }
                //        }
                //        price = Convert.ToDecimal(xj[price_i]);
                //    }
                //}
                ordergoods.goods_price = goods_model.shop_price;
                //


                ordergoods.goods_sn = goods_model.goods_sn;
                ordergoods.is_gift = false;
                ordergoods.market_price = goods_model.market_price;
                ordergoods.order_id = order_id;
                ordergoods.extension_code = cartinfo.measure_unit;
                ordergoods.parent_id = 0;
                integral = integral + goods_model.give_integral.Value * ordergoods.goods_number.Value; //记录下单时准备送的积分
                goods_amount += (decimal)(ordergoods.goods_price * ordergoods.goods_number);
                SiteBLL.InsertOrderGoodsInfo(ordergoods);
            }
            #endregion


            #region 积分
            UserIntegralInfo Ientity = SiteBLL.GetUserIntegralInfo("user_id=" + base.userid + "");
            if (Ientity != null)
            {
                Ientity.integral = Ientity.integral.Value + integral;
                Ientity.change_time = DateTime.Now;
                Ientity.remark = "";
                Ientity.order_id = order_id;
                SiteBLL.UpdateUserIntegralInfo(Ientity);
            }
            else
            {
                Ientity = new UserIntegralInfo();
                Ientity.integral = integral;
                Ientity.change_time = DateTime.Now;
                Ientity.user_id = base.userid;
                Ientity.remark = "";
                Ientity.order_id = order_id;
                int revalue = SiteBLL.InsertUserIntegralInfo(Ientity);
            }
            #endregion

            #region 更新订单价格及其它信息

            //更新订单商品金额
            SiteBLL.UpdateOrderInfoFieldValue("goods_amount", goods_amount, order_id);

            //更新订单总金额
            SiteBLL.UpdateOrderInfoFieldValue("order_amount", SiteBLL.GetOrderInfoValue("goods_amount+delivery_fee", "order_id=" + order_id), order_id);

            //删除购物车中的商品
            SiteBLL.DeleteCartInfo("session_id='" + Utils.GetSessionID() + "'");

            #endregion

            #endregion

            #region 显示支付信息

            this.DispalyPaymentInfo(context, entity.order_sn);
            #endregion
        }

        /// <summary>
        /// 显示订单支付信息
        /// </summary>
        /// <param name="orderinfo"></param>
        protected void DispalyPaymentInfo(IDictionary context, string order_sn)
        {
            OrderInfoInfo orderinfo = SiteBLL.GetOrderInfoInfo("order_sn='" + order_sn + "'");
            //OrderGoodsInfo ordergoods = SiteBLL.GetOrderGoodsInfo("order_id=" + orderinfo.order_id);
            PaymentInfo payinfo = SiteBLL.GetPaymentInfo(orderinfo.pay_id.Value);
            string pay_content = "", domain = "http://" + siteUtils.GetDomain();

            string kqbank = "";

            #region 支付宝
            if (payinfo.pay_code == "alipay")
            {
                string out_trade_no = order_sn;
                //业务参数赋值；
                string gateway = "https://www.alipay.com/cooperate/gateway.do?";	//'支付接口
                //string service = "create_partner_trade_by_buyer";
                string service = "create_direct_pay_by_user";

                string partner = (payinfo.pay_config + ",,").Split(',')[2];		//partner		合作伙伴ID			保留字段
                string sign_type = "MD5";
                string subject = order_sn;	//subject		商品名称
                string body = "来自" + config.Name + "的网站(" + domain + ")的订单" + order_sn;		//body			商品描述    
                string payment_type = "1";                  //支付类型
                decimal strprice = orderinfo.goods_amount.Value + orderinfo.delivery_fee.Value - orderinfo.bonus.Value - orderinfo.integral_money.Value;
                string price = strprice.ToString();
                string quantity = "1";
                string show_url = domain;
                string seller_email = (payinfo.pay_config + ",,").Split(',')[0];             //卖家账号
                string key = (payinfo.pay_config + ",,").Split(',')[1];              //partner账户的支付宝安全校验码
                string return_url = domain + "/PayReturn/Alipay_Return.aspx"; //服务器通知返回接口
                string notify_url = domain + "/PayReturn/Alipay_Notify.aspx"; //服务器通知返回接口

                string _input_charset = "utf-8";
                //string logistics_type = "POST";
                //string logistics_fee = "0";
                //string logistics_payment = "BUYER_PAY";
                //string logistics_type_1 = "EXPRESS";
                //string logistics_fee_1 = orderinfo.delivery_fee.Value.ToString("f2");
                //string logistics_payment_1 = "BUYER_PAY";
                payway ap = new payway();
                string aliay_url = ap.CreatUrl2(
                   gateway,
                   service,
                   partner,
                   sign_type,
                   out_trade_no,
                   subject,
                   body,
                   payment_type,
                   price,
                   show_url,
                   seller_email,
                   key,
                   return_url,
                   _input_charset,
                   notify_url
                   );

                //string aliay_url = ap.CreatUrl(
                //    gateway,
                //    service,
                //    partner,
                //    sign_type,
                //    out_trade_no,
                //    subject,
                //    body,
                //    payment_type,
                //    price,
                //    show_url,
                //    seller_email,
                //    key,
                //    return_url,
                //    _input_charset,
                //    notify_url,
                //    logistics_type,
                //    logistics_fee,
                //    logistics_payment,
                //    logistics_type_1,
                //    logistics_fee_1,
                //    logistics_payment_1,
                //    quantity
                //    );

                pay_content = "<a href=\"" + aliay_url + "\" target=\"_blank\"><img src=\"/include/images/payment/alipay.gif\" /><br><img src=\"/include/images/l_zfb.gif\"></a>";
            }
            #endregion

            #region 财付通
            else if (payinfo.pay_code == "tenpay")
            {

                //商户号
                string bargainor_id = (payinfo.pay_config + ",,").Split(',')[0]; ;

                //密钥
                string key = (payinfo.pay_config + ",,").Split(',')[1];

                //当前时间 yyyyMMdd
                string date = DateTime.Now.ToString("yyyyMMdd");

                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                string strReq = "" + DateTime.Now.ToString("HHmmss") + TenpayUtil.BuildRandomStr(4);

                //商户订单号，不超过32位，财付通只做记录，不保证唯一性
                string sp_billno = orderinfo.order_sn;

                //财付通订单号，10位商户号+8位日期+10位序列号，需保证全局唯一
                string transaction_id = bargainor_id + date + strReq;
                string return_url = domain + "/PayReturn/tenpay_return_url.aspx";

                //创建PayRequestHandler实例
                PayRequestHandler reqHandler = new PayRequestHandler(Context);

                //设置密钥
                reqHandler.setKey(key);

                //初始化
                reqHandler.init();

                decimal strprice = orderinfo.goods_amount.Value + orderinfo.delivery_fee.Value - orderinfo.bonus.Value - orderinfo.integral_money.Value;

                //-----------------------------
                //设置支付参数
                //-----------------------------
                reqHandler.setParameter("bargainor_id", bargainor_id);			//商户号
                reqHandler.setParameter("sp_billno", sp_billno);				//商家订单号
                reqHandler.setParameter("transaction_id", transaction_id);		//财付通交易单号
                reqHandler.setParameter("return_url", return_url);				//支付通知url
                reqHandler.setParameter("desc", "订单号：" + transaction_id);	//商品名称
                reqHandler.setParameter("total_fee", Convert.ToInt32(strprice * 100).ToString());//商品金额,以分为单位


                //用户ip,测试环境时不要加这个ip参数，正式环境再加此参数
                reqHandler.setParameter("spbill_create_ip", Page.Request.UserHostAddress);

                //获取请求带参数的url
                string requestUrl = reqHandler.getRequestURL();

                pay_content = "<a target=\"_blank\" href=\"" + requestUrl + "\"><img src=\"/include/images/payment/TenPay.jpg\" /></a>";

                //post实现方式
                /*
                reqHandler.getRequestURL();
                Response.Write("<form method=\"post\" action=\""+ reqHandler.getGateUrl() + "\" >\n");
                Hashtable ht = reqHandler.getAllParameters();
                foreach(DictionaryEntry de in ht) 
                {
                    Response.Write("<input type=\"hidden\" name=\"" + de.Key + "\" value=\"" + de.Value + "\" >\n");
                }
                Response.Write("<input type=\"submit\" value=\"财付通支付\" >\n</form>\n");
                */

                //获取debug信息
                //string debuginfo = reqHandler.getDebugInfo();
                //Response.Write("<br/>" + debuginfo + "<br/>");

                //重定向到财付通支付
                //reqHandler.doSend();

            }
            #endregion

            #region 网银
            else if (payinfo.pay_code == "chinabank")
            {
                //必要的交易信息
                decimal strprice = orderinfo.goods_amount.Value + orderinfo.delivery_fee.Value - orderinfo.bonus.Value - orderinfo.integral_money.Value;
                string price = strprice.ToString("f2");
                string v_amount = price;       // 订单金额
                string v_moneytype = "CNY";    // 币种
                string v_md5info;      // 对拼凑串MD5私钥加密后的值
                string v_mid = (payinfo.pay_config + ",,").Split(',')[0];		 // 商户号
                string v_key = (payinfo.pay_config + ",,").Split(',')[1];		 // 商户号
                string v_url = domain + "/PayReturn/CBReceive.aspx";     // 返回页地址
                string v_oid = orderinfo.order_sn;		 // 推荐订单号构成格式为 年月日-商户号-小时分钟秒

                string text = v_amount + v_moneytype + v_oid + v_mid + v_url + v_key; // 拼凑加密串

                v_md5info = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text, "md5").ToUpper();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<form action=\"https://pay3.chinabank.com.cn/PayGate\" target=\"_blank\"  method=\"post\" name=\"E_FORM\">");
                sb.Append("<input type=\"hidden\" name=\"v_md5info\"    value=\"" + v_md5info + "\" size=\"100\" />");
                sb.Append("<input type=\"hidden\" name=\"v_mid\"        value=\"" + v_mid + "\" />");
                sb.Append("<input type=\"hidden\" name=\"v_oid\"        value=\"" + v_oid + "\" />");
                sb.Append("<input type=\"hidden\" name=\"v_amount\"     value=\"" + v_amount + "\" />");
                sb.Append("<input type=\"hidden\" name=\"v_moneytype\"  value=\"" + v_moneytype + "\" />");
                sb.Append("<input type=\"hidden\" name=\"v_url\"        value=\"" + v_url + "\" />");
                sb.Append("<input type=\"image\" src=\"/include/images/payment/chinabank_01.gif\" />");
                sb.Append("</form>");

                pay_content = sb.ToString();
            }
            #endregion

            #region 快钱
            else if (payinfo.pay_code == "kuaiqian")
            {
                string v_mid = (payinfo.pay_config + ",,").Split(',')[0];		 // 商户号
                string v_key = (payinfo.pay_config + ",,").Split(',')[1];		 // 商户号

                kuaiqian kq = new kuaiqian();
                decimal strprice = orderinfo.goods_amount.Value + orderinfo.delivery_fee.Value - orderinfo.bonus.Value - orderinfo.integral_money.Value;
                //string price = strprice.ToString("f2");
                string price = Convert.ToInt32(strprice * 100).ToString();
                kq.orderAmount = price;       // 订单金额
                kq.orderId = orderinfo.order_sn;		 // 推荐订单号构成格式为 年月日-商户号-小时分钟秒
                kq.bgUrl = domain + "/PayReturn/Receive.aspx";
                //kq.payerName = "";//orderinfo.consignee;
                //kq.productName = ordergoods.goods_name;
                // kq.productName = config.Name;
                kq.payType = "00";
                pay_content = "<a href=\"" + kq.KuiQianMethod(v_mid, v_key) + "\" target=\"_blank\"><img src=\"/include/images/payment/kuaiqian.gif\" /></a>";

                string[] bank = { "ABC", "BCOM", "BEA", "BJRCB", "BOB", "BOC", "CBHB", "CCB", "CEB", "CIB", "CITIC", "CMB", "CMBC", "GDB", "GZCB", "GZRCC", "HXB", "ICBC", "NBCB", "NJCB", "PAB", "POST", "SDB", "SHRCC", "SPDB", "HSB", "CZB", "SHB", "HZB" };
                foreach (string item in bank)
                {
                    kq.payType = "10";
                    kq.bankId = item;
                    pay_content = "<a href=\"" + kq.KuiQianMethod(v_mid, v_key) + "\" target=\"_blank\"><img src=\"/PayReturn/bank/skin/cwgj/img/bank_" + item.ToLower() + ".gif\" /></a>";
                }
            }
            #endregion

            #region 银行转账
            else if (payinfo.pay_code == "bank")
            {
                pay_content = payinfo.pay_config;
            }
            #endregion

            context.Add("kqbank", kqbank);
            context.Add("orderinfo", orderinfo);
            context.Add("payinfo", payinfo);
            context.Add("pay_content", pay_content);

            base.DisplayTemplate(context, "store/payment");
        }
    }
}
