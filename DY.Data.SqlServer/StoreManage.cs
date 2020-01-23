using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Config;
using DY.Data;
using DY.Entity;
using System.Collections;

namespace DY.Data.SqlServer
{
    /// <summary>
    /// 购物数据管理类
    /// </summary>
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 更新订单商品数量（在原来的数量上加一）
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        public void UpdateOrderGoodsNumber(int order_id, int goods_id)
        {
            string SQL = string.Format("UPDATE {0}order_goods SET goods_number=goods_number+1 WHERE [goods_id]=@goods_id and order_id=@order_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@order_id", (DbType)SqlDbType.Int, 4, order_id)
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 更新购物车商品数量（在原来的数量上加一）
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="session_id"></param>
        /// <param name="goods_attrs"></param>
        /// <param name="goods_attr_ids"></param>
        public void UpdateCartGoodsNumber(int goods_id, string session_id, string goods_attrs, string goods_attr_ids)
        {
            string SQL = string.Format("UPDATE {0}cart SET goods_number=goods_number+1 WHERE [goods_id]=@goods_id and session_id=@session_id and goods_attr_id=@goods_attr_id and cast(goods_attr as varchar(225))=@goods_attr", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@session_id", (DbType)SqlDbType.NVarChar, 50, session_id),
                DbHelper.MakeInParam("@goods_attr_id", (DbType)SqlDbType.NVarChar, 50, goods_attr_ids),
                DbHelper.MakeInParam("@goods_attr", (DbType)SqlDbType.NVarChar, 225, goods_attrs),
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="session_id"></param>
        /// <param name="goods_attrs"></param>
        /// <param name="goods_attr_ids"></param>
        public void UpdateCartGoodsNumber(int goods_id, string session_id, string goods_attrs, string goods_attr_ids, int goods_number)
        {
            string SQL = string.Format("UPDATE {0}cart SET goods_number=goods_number+@goods_number WHERE [goods_id]=@goods_id and session_id=@session_id and goods_attr_id=@goods_attr_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@session_id", (DbType)SqlDbType.NVarChar, 50, session_id),
                DbHelper.MakeInParam("@goods_attr_id", (DbType)SqlDbType.NVarChar, 50, goods_attr_ids),
                DbHelper.MakeInParam("@goods_number", (DbType)SqlDbType.Int, 4, goods_number),
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }

        /// <summary>
        /// 获取商品规格
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsSpec(int goods_id)
        {
            string SQL = string.Format("select s.spec_name,s.spec_id,s.spec_type from {0}goods_spec as s where s.spec_id in (select spec_id from {0}goods_spec_index where goods_id=@goods_id)", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取商品规则值列表
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="spec_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsSpecValues(int goods_id, int spec_id)
        {
            string SQL = string.Format("select v.*,i.product_id from {0}goods_spec_index as i,{0}goods_spec_values as v where i.spec_value_id=v.spec_value_id and i.spec_id=@spec_id and i.goods_id=@goods_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@spec_id", (DbType)SqlDbType.Int, 4, spec_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 取得规格产品信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="spec_value_id"></param>
        /// <param name="spec_id"></param>
        /// <returns></returns>
        public DataTable GetProductInfo(int goods_id, int spec_value_id, int spec_id)
        {
            string SQL = string.Format("select top 1 p.* from {0}products as p,{0}goods_spec_index as i where p.product_id=i.product_id and i.spec_value_id=@spec_value_id and i.spec_id=@spec_id and i.goods_id=@goods_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@spec_id", (DbType)SqlDbType.Int, 4, spec_id),
                DbHelper.MakeInParam("@spec_value_id", (DbType)SqlDbType.Int, 4, spec_value_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
    }
}
