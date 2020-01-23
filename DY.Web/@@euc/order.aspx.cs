/**
 * 功能描述：OrderInfo管理类
 * 创建时间：2010/2/7 12:56:14
 * 最后修改时间：2010/2/7 12:56:14

 * ============================================================================


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
using DY.Config;

namespace DY.Web.admin
{
    public partial class order : AdminPage
    {
        protected string step;  //当前步骤
        protected int order_id;
        protected IDictionary context = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.step = DYRequest.getRequest("step");
            this.order_id = DYRequest.getRequestInt("order_id");

            if (this.order_id == 0) { this.order_id = DYRequest.getRequestInt("id"); }

            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("order_info_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 快递查询
            if (this.act == "kuaidi100")
            {
                context.Add("com", new Caches().GetDelivery_Info(DYRequest.getRequestInt("delivery_id")).com);
                context.Add("nu", DYRequest.getRequest("nu"));
                base.DisplayTemplate(context, "orders/kuaidi100");
            }
            #endregion


            #region 回收站
            else if (this.act == "trash")
            {
                //检测权限
                this.IsChecked("orders_trash");

                //显示列表数据
                this.GetList("orders/order_trash", " and id_delete=1");
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("order_info_add");

                IDictionary context = new Hashtable();
                context.Add("order_id", order_id);

                switch (this.step)
                {
                    case "goods":
                        this.AddGoods(context);
                        break;
                    case "add_goods":
                        SaveOrderGoods(context);
                        break;
                    case "delete_order_goods":
                        SiteBLL.DeleteOrderGoodsInfo(DYRequest.getRequestInt("rec_id"));

                        this.AddGoods(context);
                        break;
                    case "consignee":
                        #region 设置收货人信息
                        if (base.ispost)
                        {
                            OrderInfoInfo entity = new OrderInfoInfo();
                            entity.consignee = DYRequest.getForm("consignee");
                            entity.country = DYRequest.getFormInt("country");
                            entity.province = DYRequest.getFormInt("province");
                            entity.city = DYRequest.getFormInt("city");
                            entity.district = DYRequest.getFormInt("district");
                            entity.address = DYRequest.getForm("address");
                            entity.zipcode = DYRequest.getForm("zipcode");
                            entity.tel = DYRequest.getForm("tel");
                            entity.mobile = DYRequest.getForm("mobile");
                            entity.email = DYRequest.getForm("email");
                            entity.best_time = DYRequest.getForm("best_time");
                            entity.sign_building = DYRequest.getForm("sign_building");
                            entity.order_id = order_id;
                            SiteBLL.UpdateOrderInfoInfo(entity);

                            Response.Redirect("order.aspx?act=add&step=shipping&order_id=" + order_id);
                        }
                        #endregion

                        base.DisplayTemplate(context, "orders/order_add_consignee");
                        break;
                    case "shipping":
                        #region 设置收货方式
                        if (base.ispost)
                        {
                            DeliveryInfo deliinfo = SiteBLL.GetDeliveryInfo(DYRequest.getFormInt("shipping"));
                            OrderInfoInfo entity = new OrderInfoInfo();
                            entity.delivery_id = deliinfo.delivery_id;
                            entity.delivery_name = deliinfo.delivery_name;
                            entity.delivery_fee = deliinfo.delivery_price.Value;
                            entity.order_id = order_id;
                            SiteBLL.UpdateOrderInfoInfo(entity);

                            //统计商品金额+配送金额
                            SiteBLL.UpdateOrderInfoFieldValue("order_amount", SiteBLL.GetOrderInfoValue("SUM(goods_amount+delivery_fee)", "order_id=" + this.order_id), this.order_id);

                            Response.Redirect("order.aspx?act=add&step=payment&order_id=" + order_id);
                        }
                        #endregion

                        context.Add("shipping_list", SiteBLL.GetDeliveryAllList("", ""));

                        base.DisplayTemplate(context, "orders/order_add_shipping");
                        break;
                    case "payment":
                        #region 设置支付方式
                        if (base.ispost)
                        {
                            PaymentInfo payinfo = SiteBLL.GetPaymentInfo(DYRequest.getFormInt("payment"));
                            OrderInfoInfo entity = new OrderInfoInfo();
                            entity.pay_id = payinfo.pay_id;
                            entity.pay_name = payinfo.pay_name;
                            entity.pay_fee = 0;
                            entity.order_id = order_id;
                            entity.id_delete = false;
                            SiteBLL.UpdateOrderInfoInfo(entity);

                            //添加记录
                            AddOrderActionLog("添加订单", 0, 0, 0, order_id);

                            Response.Redirect("order.aspx?act=info&order_id=" + order_id);
                        }
                        #endregion

                        context.Add("payment_list", SiteBLL.GetPaymentAllList("", ""));

                        base.DisplayTemplate(context, "orders/order_add_payment");
                        break;
                    default:
                        if (base.ispost)
                            SaveOrderInfo();

                        base.DisplayTemplate("orders/order_add");
                        break;
                }
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                OrderEdit(this.order_id);
            }
            #endregion

            #region 查看
            else if (base.act == "info")
            {
                OrderInfo(this.order_id);
            }
            #endregion

            #region 订单操作
            else if (base.act == "order_state")
            {
                OrderState();
            }
            #endregion

            #region 重置订单状态

            else if (base.act == "order_reset")
            {
                OrderReset();
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("order_info_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateOrderInfoFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("order_info_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateOrderInfoFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("order_info_del", true);

                //执行删除
                SiteBLL.DeleteOrderInfoInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("order_info_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除订单");

                        //执行删除
                        SiteBLL.DeleteOrderInfoInfo("order_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 订单打印模板编辑
            else if (base.act == "template")
            {
                //权限检测
                this.IsChecked("edit_order_print_tlp");

                string tlpFile = Server.MapPath(BaseConfig.AdminSkinPath + "template/order_print" + BaseConfig.WebSkinSuffix);
                if (base.ispost)
                {
                    FileOperate.WriteFile(tlpFile, DYRequest.getForm("temp_content"));

                    //显示提示信息
                    base.DisplayMessage("订单打印模板修改成功", 2, "?act=template");
                }

                IDictionary context = new Hashtable();
                context.Add("temp_content", FileOperate.ReadFile(tlpFile));

                base.DisplayTemplate(context, "orders/order_template");
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = " and id_delete=0";
            string field = DYRequest.getRequest("state"), val = DYRequest.getRequest("val");


            string pid = Request.QueryString["pid"];
            string date = Request.QueryString["date"];

            if (!string.IsNullOrEmpty(pid))
            {
                filter += " and pid=" + pid;
            }

            if (!string.IsNullOrEmpty(date))
            {
                if (date == "1")
                {
                    filter += " and datediff(day,add_time,getdate())=0";
                }
                else if (date == "2")
                {
                    filter += " and datediff(week,add_time,getdate())=0";
                }
                else if (date == "3")
                {
                    filter += " and datediff(Month,add_time,getdate())=0";
                }
            }

            int user_id = DYRequest.getRequestInt("user_id");
            if (user_id > 0)
            {
                filter += " and user_id=" + user_id;
            }


            if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(val))
                filter += " and " + field + "=" + val;
            if (!string.IsNullOrEmpty(DYRequest.getRequest("ids")))
                filter += " and order_id in (" + DYRequest.getRequest("ids").Remove(DYRequest.getRequest("ids").Length - 1, 1) + ")";

            this.GetList("orders/order_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();

            #region 导出
            if (DYRequest.getRequest("import") == "true")
            {
                Response.ContentType = "application/vnd.ms-excel; charset=utf-8";
                Response.AddHeader("Content-Disposition", "attachment;filename=order-" + SiteUtils.MakeOrderSn() + ".xls");
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                context.Add("list", SiteBLL.GetOrderInfoAllList(SiteUtils.GetSortOrder("order_id desc,order_amount desc"), SiteUtils.GetFilter(context) + filter));

                base.DisplayTemplate(context, "import/order_list_xls");
            }
            #endregion

            context.Add("list", SiteBLL.GetOrderInfoList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("order_id desc,order_amount desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 保存订单信息
        /// </summary>
        protected void SaveOrderInfo()
        {
            OrderInfoInfo orderinfo = new OrderInfoInfo();
            orderinfo.user_id = DYRequest.getFormInt("user");
            orderinfo.order_sn = SiteUtils.MakeOrderSn();
            orderinfo.order_status = 0;
            orderinfo.id_delete = true;

            order_id = SiteBLL.InsertOrderInfoInfo(orderinfo);

            Response.Redirect("order.aspx?act=add&step=goods&order_id=" + order_id);
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        protected void AddGoods(IDictionary context)
        {
            context.Add("ordergoodslist", SiteBLL.GetOrderGoodsAllList("rec_id desc", "order_id=" + this.order_id));
            context.Add("order_price", SiteBLL.GetOrderGoodsValue("SUM(goods_number*goods_price)", "order_id=" + this.order_id));

            base.DisplayTemplate(context, "orders/order_add_goods", base.isajax);
        }
        /// <summary>
        /// 保存订单商品信息
        /// </summary>
        protected void SaveOrderGoods(IDictionary context)
        {
            if (ispost)
            {
                int goods_id = DYRequest.getFormInt("goodslist");
                GoodsInfo goodsinfo = SiteBLL.GetGoodsInfo(goods_id);
                OrderGoodsInfo ordergoods = new OrderGoodsInfo();

                if (!SiteBLL.ExistsOrderGoods("order_id=" + this.order_id + " and goods_id=" + goods_id))
                {
                    ordergoods.goods_id = goods_id;
                    ordergoods.goods_name = goodsinfo.goods_name;
                    ordergoods.goods_number = DYRequest.getFormInt("add_number");
                    ordergoods.goods_price = DYRequest.getForm("add_price") == "user_input" ? DYRequest.getFormDecimal("input_price") : DYRequest.getFormDecimal("add_price");
                    ordergoods.goods_sn = goodsinfo.goods_sn;
                    ordergoods.is_gift = false;
                    ordergoods.market_price = goodsinfo.market_price;
                    ordergoods.order_id = this.order_id;
                    ordergoods.extension_code = goodsinfo.weight_unit;
                    ordergoods.parent_id = 0;
                    SiteBLL.InsertOrderGoodsInfo(ordergoods);
                }
                else
                {
                    Goods.UpdateOrderGoodsNumber(this.order_id, goods_id);
                }

                //统计商品金额
                SiteBLL.UpdateOrderInfoFieldValue("goods_amount", SiteBLL.GetOrderGoodsValue("sum(goods_price*goods_number)", "order_id=" + this.order_id), this.order_id);
            }

            AddGoods(context);
        }
        /// <summary>
        /// 订单查看
        /// </summary>
        protected void OrderInfo(int order_id)
        {
            //检测权限
            this.IsChecked("order_info_info");

            IDictionary context = new Hashtable();
            context.Add("entity", SiteBLL.GetOrderInfoInfo(order_id));
            context.Add("ordergoodslist", SiteBLL.GetOrderGoodsAllList("rec_id desc", "order_id=" + order_id));
            context.Add("orderlogs", SiteBLL.GetOrderActionAllList("action_id desc", "order_id=" + order_id));
            context.Add("pre_id", SiteBLL.GetOrderInfoValue("MAX(order_id)", "order_id<" + order_id));
            context.Add("next_id", SiteBLL.GetOrderInfoValue("MIN(order_id)", "order_id>" + order_id));
            context.Add("datetime", DateTime.Now);
            context.Add("username", base.username);

            base.DisplayTemplate(context, DYRequest.getRequestInt("print") == 1 ? "template/order_print" : "orders/order_info", base.isajax);
        }

        /// <summary>
        /// 订单修改
        /// </summary>
        protected void OrderEdit(int order_id)
        {
            //检测权限
            this.IsChecked("order_info_edit");

            Entity.OrderInfoInfo model = SiteBLL.GetOrderInfoInfo(order_id);
            //付款
            ArrayList payment = SiteBLL.GetPaymentAllList("pay_order desc,pay_id asc", "enabled=1");
            //物流
            ArrayList delivery = SiteBLL.GetDeliveryAllList("orderid desc,delivery_id asc", "enabled=1");
            if (ispost)
            {
                //付款信息
                model.pay_id = DYRequest.getFormInt("payment");
                model.pay_fee = 0;
                model.pay_name = "";
                model.pay_note = "";
                model.pay_status = DYRequest.getFormInt("pay_status");
                string pay_time = DYRequest.getForm("pay_time");
                if (string.IsNullOrEmpty(pay_time))
                {
                    model.pay_time = null;
                }
                else
                {
                    try { model.pay_time = Convert.ToDateTime(pay_time); }
                    catch { model.pay_time = null; }
                }
                //物流
                model.delivery_id = DYRequest.getFormInt("delivery");
                model.delivery_name = "";
                model.delivery_status = DYRequest.getFormInt("delivery_status");
                model.invoice_no = DYRequest.getForm("invoice_no");
                model.referer = DYRequest.getForm("referer");
                string delivery_time = DYRequest.getForm("delivery_time");
                if (string.IsNullOrEmpty(delivery_time))
                {
                    model.delivery_time = null;
                }
                else
                {
                    try { model.delivery_time = Convert.ToDateTime(delivery_time); }
                    catch { model.delivery_time = null; }
                }
                // 收货人信息
                model.consignee = DYRequest.getForm("consignee");
                model.email = DYRequest.getForm("email");
                model.address = DYRequest.getForm("address");
                model.city = DYRequest.getFormInt("city");
                model.province = DYRequest.getFormInt("province");
                model.zipcode = DYRequest.getForm("zipcode");
                model.tel = DYRequest.getForm("tel");
                model.mobile = DYRequest.getForm("mobile");
                model.sign_building = DYRequest.getForm("sign_building");
                model.best_time = DYRequest.getForm("best_time");
                model.postscript = DYRequest.getForm("postscript");
                model.to_buyer = DYRequest.getForm("to_buyer");
                //金额
                model.goods_amount = DYRequest.getFormDecimal("goods_amount");
                model.delivery_fee = DYRequest.getFormDecimal("delivery_fee");
                model.order_amount = DYRequest.getFormDecimal("order_amount");

                model.add_time = Utils.StrToDataTime(DYRequest.getForm("add_time"), DateTime.Now);
                //model.action_note = DYRequest.getForm("action_note");
                SiteBLL.UpdateOrderInfoInfo(model);
            }

            IDictionary context = new Hashtable();
            context.Add("entity", model);
            context.Add("payment", payment);
            context.Add("delivery", delivery);
            context.Add("ordergoodslist", SiteBLL.GetOrderGoodsAllList("rec_id desc", "order_id=" + order_id));
            context.Add("orderlogs", SiteBLL.GetOrderActionAllList("action_id desc", "order_id=" + order_id));
            context.Add("pre_id", SiteBLL.GetOrderInfoValue("MAX(order_id)", "order_id<" + order_id));
            context.Add("next_id", SiteBLL.GetOrderInfoValue("MIN(order_id)", "order_id>" + order_id));
            context.Add("datetime", DateTime.Now);
            context.Add("username", base.username);

            base.DisplayTemplate(context, DYRequest.getRequestInt("print") == 1 ? "template/order_print" : "orders/order_edit", base.isajax);
        }

        /// <summary>
        /// 订单状态操作
        /// </summary>
        protected void OrderState()
        {
            string fieldName = DYRequest.getRequest("field"), action_note = "";
            int val = DYRequest.getRequestInt("val");
            int order_status = val, pay_status = val, delivery_status = val;
            OrderInfoInfo orderinfo = SiteBLL.GetOrderInfoInfo(base.id);

            //执行修改
            SiteBLL.UpdateOrderInfoFieldValue(fieldName, val, base.id);

            if (fieldName == "order_status")
            {
                delivery_status = orderinfo.delivery_status.Value;
                pay_status = orderinfo.pay_status.Value;
                if (val == 1)
                    action_note = "确认订单";
                else if (val == 2)
                    action_note = "取消订单";
                else if (val == 3)
                    action_note = "将订单标记为无效";
                else if (val == 4)
                {
                    action_note = "订单完成";

                    if (orderinfo.user_id > 0)
                    {
                        //decimal d = Utils.StrToDecimal(config.GoodsInteger, 0);
                        decimal d = Utils.StrToDecimal(config.GoodsInteger, 0);
                        if (config.GoodsInteger != "" && d != 0)
                        {
                            decimal price = orderinfo.goods_amount.Value - orderinfo.integral_money.Value + orderinfo.delivery_fee.Value - orderinfo.bonus.Value;

                            UserIntegralInfo entity = new UserIntegralInfo();
                            entity.user_id = orderinfo.user_id;
                            entity.integral = Convert.ToInt16(price * d);
                            entity.order_id = orderinfo.order_id;
                            //现改为积分获得时间
                            entity.change_time = DateTime.Now;
                            entity.remark = "订单完成获得" + entity.integral + "积分";
                            SiteBLL.InsertUserIntegralInfo(entity);
                        }
                    }
                }


            }
            else if (fieldName == "delivery_status")
            {
                pay_status = orderinfo.pay_status.Value;
                order_status = orderinfo.order_status.Value;
                action_note = val == 1 ? "标记为发货" : "标记为退货";
                if (val == 1)
                {
                    //执行修改
                    SiteBLL.UpdateOrderInfoFieldValue("delivery_time", DateTime.Now, base.id);
                    //减少库存
                    ReduceInventory(base.id);
                }
            }
            else if (fieldName == "pay_status")
            {
                order_status = orderinfo.order_status.Value;
                delivery_status = orderinfo.delivery_status.Value;
                action_note = val == 1 ? "标记为已付款" : "标记为退款";
            }

            if (!string.IsNullOrEmpty(DYRequest.getRequest("note")))
                action_note = action_note + "：" + DYRequest.getRequest("note");

            this.AddOrderActionLog(action_note, (byte)order_status, (byte)pay_status, (byte)delivery_status, base.id);

            //重新显示数据
            this.OrderInfo(base.id);
        }
        /// <summary>
        /// 重置订单状态
        /// </summary>
        protected void OrderReset()
        {
            int order_status = 0;
            int delivery_status = 0;
            string action_note = "重置订单状态";
            int pay_status = 0;
            //检测权限
            this.IsChecked("order_info_edit");
            //执行修改
            OrderInfoInfo orderinfo = SiteBLL.GetOrderInfoInfo(base.id);
            orderinfo.order_status = 0;
            orderinfo.pay_status = 0;
            orderinfo.delivery_status = 0;
            SiteBLL.UpdateOrderInfoInfo(orderinfo);
            //SiteBLL.UpdateOrderInfoFieldValue("order_status", "0", base.id);
            if (!string.IsNullOrEmpty(DYRequest.getRequest("note")))
                action_note = action_note + "：" + DYRequest.getRequest("note");
            this.AddOrderActionLog(action_note, (byte)order_status, (byte)pay_status, (byte)delivery_status, base.id);

            SiteBLL.DeleteUserIntegralInfo("order_id=" + orderinfo.order_id);


            //重新显示数据
            this.OrderInfo(base.id);
        }

        /// <summary>
        /// 添加操作记录
        /// </summary>
        protected void AddOrderActionLog(string note, int order_status, int pay_status, int delivery_status, int order_id)
        {
            OrderActionInfo actinfo = new OrderActionInfo();
            actinfo.action_note = note;
            actinfo.action_user = base.username;
            actinfo.delivery_status = (byte)delivery_status;
            actinfo.log_time = DateTime.Now;
            actinfo.order_id = order_id;
            actinfo.order_status = (byte)order_status;
            actinfo.pay_status = (byte)pay_status;
            SiteBLL.InsertOrderActionInfo(actinfo);
        }

        /// <summary>
        /// 减少库存
        /// </summary>
        /// <param name="order_id">订单ID</param>
        private void ReduceInventory(int order_id)
        {
            ArrayList goodslist = SiteBLL.GetOrderGoodsAllList("rec_id asc", "order_id=" + order_id);
            Entity.GoodsInfo goodsmodel = new GoodsInfo();
            foreach (Entity.OrderGoodsInfo model in goodslist)
            {
                goodsmodel = SiteBLL.GetGoodsInfo((int)model.goods_id);
                if (goodsmodel != null)
                {
                    goodsmodel.goods_number = goodsmodel.goods_number - model.goods_number;
                    SiteBLL.UpdateGoodsInfo(goodsmodel);
                }
            }
        }
    }
}
