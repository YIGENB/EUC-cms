//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Entity;

namespace DY.Data
{
    /// <summary>
    /// 购物类
    /// </summary>
    public partial interface IDataProvider
    {
        /// <summary>
        /// 更新订单商品数量（在原来的数量上加一）
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        void UpdateOrderGoodsNumber(int order_id, int goods_id);
        /// <summary>
        /// 更新购物车商品数量（在原来的数量上加一）
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="session_id"></param>
        /// <param name="goods_attrs"></param>
        /// <param name="goods_attr_ids"></param>
        void UpdateCartGoodsNumber(int goods_id, string session_id, string goods_attrs, string goods_attr_ids);

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="session_id"></param>
        /// <param name="goods_attrs"></param>
        /// <param name="goods_attr_ids"></param>
        void UpdateCartGoodsNumber(int goods_id, string session_id, string goods_attrs, string goods_attr_ids, int goods_number);

        /// <summary>
        /// 获取商品规格
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        DataTable GetGoodsSpec(int goods_id);
        /// <summary>
        /// 获取商品规则值列表
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="spec_id"></param>
        /// <returns></returns>
        DataTable GetGoodsSpecValues(int goods_id, int spec_id);
        /// <summary>
        /// 取得规格产品信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="spec_value_id"></param>
        /// <param name="spec_id"></param>
        /// <returns></returns>
        DataTable GetProductInfo(int goods_id, int spec_value_id, int spec_id);
    }
}
