using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 商店类，与商品购买相关的功能都放在这里
    /// </summary>
    public class Store
    {
        #region 静态函数
        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="goods_id">商品ID</param>
        /// <param name="parent_id">所商品ID</param>
        /// <param name="goods_attrs">顾客所选的商品属性</param>
        /// <param name="goods_attr_ids">商品属性ID</param>
        /// <param name="goods_number">顾客所购买的商品数量</param>
        /// <param name="user_id">顾客在本站所注册的用户ID，如果未登录则为0</param>
        /// <returns></returns>
        public static int AddToCart(int goods_id, int parent_id, string goods_attrs, string goods_attr_ids, int goods_number, int user_id)
        {
            GoodsInfo goodsinfo = SiteBLL.GetGoodsInfo(goods_id);
            if (goodsinfo != null)
            {
                CartInfo cartinfo = new CartInfo();
                cartinfo.add_time = DateTime.Now;
                cartinfo.goods_attr = goods_attrs;
                cartinfo.goods_attr_id = goods_attr_ids;
                cartinfo.goods_id = goods_id;
                cartinfo.goods_name = goodsinfo.goods_name;
                cartinfo.goods_number = goods_number;
                if (goodsinfo.is_promote == true)
                {
                    SiteUtils su = new SiteUtils();
                    if (su.CompareTime(goodsinfo.promote_start_date.Value) > 0)
                    {
                        cartinfo.goods_price = goodsinfo.shop_price;
                    }
                    else
                    {
                        cartinfo.goods_price = goodsinfo.promote_price;
                    }
                }
                else
                {
                    //string[] cc = SiteBLL.GetGoodsAttrInfo("goods_id=" + goods_id + " and " + "attr_id=42").attr_value.Split(',');
                    //string[] xj = SiteBLL.GetGoodsAttrInfo("goods_id=" + goods_id + " and " + "attr_id=44").attr_value.Split(',');
                    //string[] attr = goods_attrs.Split(',');

                    decimal price = goodsinfo.shop_price.Value;

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
                    cartinfo.goods_price = price;
                }
                cartinfo.goods_sn = goodsinfo.goods_sn;
                cartinfo.goods_weight = goodsinfo.goods_weight;
                cartinfo.goods_img = goodsinfo.goods_thumb;
                cartinfo.give_integral = goodsinfo.give_integral * goods_number;
                cartinfo.is_gift = goodsinfo.is_alone_sale.Value ? false : true;
                cartinfo.is_onlinesale = false;  //是否为在线销售商品，如软件
                cartinfo.market_price = goodsinfo.market_price;
                cartinfo.measure_unit = goodsinfo.weight_unit;
                cartinfo.parent_id = parent_id;
                cartinfo.session_id = Utils.GetSessionID();
                cartinfo.user_id = user_id;
                cartinfo.is_promote = goodsinfo.is_promote;
                cartinfo.promote_price = goodsinfo.promote_price;
                cartinfo.promote_end_date = goodsinfo.promote_end_date;
                cartinfo.promote_start_date = goodsinfo.promote_start_date;

                //判断购物车是否存在相同商品，如果存在，则更新数量，否则插入
                if (!SiteBLL.ExistsCart("goods_id=" + goods_id + " and session_id='" + Utils.GetSessionID() + "' and goods_attr_id='" + goods_attr_ids + "' and cast(goods_attr as varchar(225))='" + goods_attrs + "'"))
                    return SiteBLL.InsertCartInfo(cartinfo);
                else
                    DatabaseProvider.GetInstance().UpdateCartGoodsNumber(goods_id, Utils.GetSessionID(), goods_attrs, goods_attr_ids, goods_number);

                return -1;

            }

            return 0;
        }
        /// <summary>
        /// 取得购物车列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetCartList()
        {
            return SiteBLL.GetCartAllList("rec_id desc", "session_id='" + Utils.GetSessionID() + "'");
        }
        /// <summary>
        /// 取得规格产品信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="spec_value_id"></param>
        /// <param name="spec_id"></param>
        /// <returns></returns>
        public static System.Data.DataRowCollection GetProductInfo(int goods_id, int spec_value_id, int spec_id)
        {
            return DatabaseProvider.GetInstance().GetProductInfo(goods_id, spec_value_id, spec_id).Rows;
        }
        /// <summary>
        /// 统计购物车商品总额
        /// </summary>
        /// <returns></returns>
        public static decimal SumCartGoodsPrice()
        {
            object obj = SiteBLL.GetCartValue("sum(goods_price*goods_number)", "session_id='" + Utils.GetSessionID() + "'");
            if (!string.IsNullOrEmpty(obj.ToString()))
                return Convert.ToDecimal(obj);

            return 0;
        }
        /// <summary>
        /// 取得当前符合条件的满立减
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetDiscountList()
        {
            return SiteBLL.GetDiscountAllList("", "is_enabled=1");
        }
        /// <summary>
        /// 取得免配送费用的购物车金额
        /// </summary>
        /// <returns></returns>
        public static decimal GetCashCart()
        {
            DiscountInfo disinfo = SiteBLL.GetDiscountInfo("discount_class='CashCart' and is_enabled=1");
            if (disinfo != null)
                return Utils.StrToDecimal(disinfo.discount_para, 0);
            return 0;
        }
        /// <summary>
        /// 统计购物车商品数量
        /// </summary>
        /// <returns></returns>
        public static int GetCartSumGoods()
        {
            object obj = SiteBLL.GetCartValue("SUM(goods_number)", "session_id='" + Utils.GetSessionID() + "'");
            if (!string.IsNullOrEmpty(obj.ToString()))
                return Convert.ToInt32(obj);

            return 0;

        }
        #endregion

        #region 非静态函数
        /// <summary>
        /// 取得某商品的规格
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public System.Data.DataRowCollection GetSpecs(int goods_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsSpec(goods_id).Rows;
        }
        /// <summary>
        /// 取得某商品的规格值列表
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public System.Data.DataRowCollection GetSpecValues(int goods_id, int spec_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsSpecValues(goods_id, spec_id).Rows;
        }
        #endregion
    }
}
