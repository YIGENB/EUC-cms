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
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoodsCat()
        {
            string SQL = string.Format("SELECT c.cat_id, c.cat_name, c.measure_unit, c.parent_id, c.is_show, c.show_in_nav, c.sort_order,c.cat_level,c.cat_ico,c.urlrewriter, COUNT(s.cat_id) AS has_children FROM {0}goods_category AS c LEFT JOIN {0}goods_category AS s ON s.parent_id=c.cat_id GROUP BY c.cat_id, c.cat_name, c.measure_unit, c.parent_id, c.is_show, c.show_in_nav, c.sort_order,c.cat_level,c.cat_ico,c.urlrewriter ORDER BY c.parent_id, c.sort_order ASC",BaseConfig.TablePrefix);

            return DbHelper.ExecuteDataset(CommandType.Text, SQL).Tables[0];
        }
        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        public void UpdateGoodsGallery(int goods_id, string img_desc, int img_id)
        {
            string SQL = string.Format("UPDATE {0}goods_gallery SET [goods_id]=@goods_id,[img_desc]=@img_desc WHERE img_id=@img_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@goods_id",(DbType)SqlDbType.Int,4,goods_id),
			    DbHelper.MakeInParam("@img_desc", (DbType)SqlDbType.NVarChar, 200, img_desc),
                DbHelper.MakeInParam("@img_id",(DbType)SqlDbType.Int,4,img_id)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }

        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        /// <param name="order_id"></param>
        public void UpdateGoodsGallery(int goods_id, string img_desc, int img_id,int order_id)
        {
            string SQL = string.Format("UPDATE {0}goods_gallery SET [goods_id]=@goods_id,[img_desc]=@img_desc,[order_id]=@order_id WHERE img_id=@img_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@goods_id",(DbType)SqlDbType.Int,4,goods_id),
			    DbHelper.MakeInParam("@img_desc", (DbType)SqlDbType.NVarChar, 200, img_desc),
                DbHelper.MakeInParam("@img_id",(DbType)SqlDbType.Int,4,img_id),
                DbHelper.MakeInParam("@order_id",(DbType)SqlDbType.Int,4,order_id)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }

        /// <summary>
        /// 删除临时上传且未更新所属商品的图片信息
        /// </summary>
        /// <param name="user_id"></param>
        public void DeleteGoodsGalleryByTemp(int user_id)
        {
            string SQL = string.Format("DELETE FROM {0}goods_gallery WHERE [admin_user_id]=@admin_user_id and goods_id=0", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@admin_user_id", (DbType)SqlDbType.Int, 4, user_id)
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL,parms);
        }
        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsAttr(int goods_id,int type_id)
        {
            string SQL = string.Format("SELECT a.attr_id,a.attr_name,a.attr_input_type,a.attr_values,v.attr_value,v.goods_attr_id FROM {0}goods_attribute as a left join {0}goods_attr as v on  v.goods_id=@goods_id and v.attr_id = a.attr_id WHERE a.type_id=@type_id order by a.sort_order desc,a.attr_id asc", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@type_id", (DbType)SqlDbType.Int, 4, type_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取资讯属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public DataTable GetNewsAttr(int goods_id, int type_id)
        {
            string SQL = string.Format("SELECT a.attr_id,a.attr_name,a.attr_input_type,a.attr_values,v.attr_value,v.goods_attr_id,v.article_id FROM {0}goods_attribute as a left join {0}goods_attr as v on  v.article_id=@goods_id and v.attr_id = a.attr_id WHERE a.type_id=@type_id order by a.sort_order desc,a.attr_id asc", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id),
                DbHelper.MakeInParam("@type_id", (DbType)SqlDbType.Int, 4, type_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="attr_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsAttribute(int attr_id)
        {
            string SQL = string.Format("SELECT a.attr_id,a.attr_name,a.attr_input_type,a.attr_values FROM {0}goods_attribute as a WHERE a.attr_id=@attr_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@attr_id", (DbType)SqlDbType.Int, 4, attr_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 删除商品相关属性值
        /// </summary>
        /// <param name="goods_id"></param>
        public void DeleteGoodsAttrValueByGoodsId(int goods_id)
        {
            string SQL = string.Format("DELETE FROM {0}goods_attr WHERE [goods_id]=@goods_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id)
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }

        /// <summary>
        /// 删除资讯相关属性值
        /// </summary>
        /// <param name="article_id"></param>
        public void DeleteGoodsAttrValueByNewsId(int article_id)
        {
            string SQL = string.Format("DELETE FROM {0}goods_attr WHERE [article_id]=@article_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@article_id", (DbType)SqlDbType.Int, 4, article_id)
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 获取商品会员等级价格信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsUserRankPrice(int goods_id)
        {
            string SQL = string.Format("SELECT r.rank_name,r.rank_id,p.user_price FROM {0}user_rank as r left join {0}goods_user_price as p on r.rank_id=p.user_rank and p.goods_id=@goods_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 删除商品相关会员等级价格
        /// </summary>
        /// <param name="goods_id"></param>
        public void DeleteGoodsUserRankPriceByGoodsId(int goods_id)
        {
            string SQL = string.Format("DELETE FROM {0}goods_user_price WHERE [goods_id]=@goods_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id)
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 获取goods信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoodsFullInfo(int goods_id)
        {
            
            //string SQL = string.Format("select p.*,c.cat_name,c.urlrewriter as cat_urlrewriter from {0}goods_category as c,{0}goods as p where p.cat_id=c.cat_id and p.goods_id=@goods_id", BaseConfig.TablePrefix);
            string SQL = string.Format("select p.*,c.cat_name,c.urlrewriter as cat_urlrewriter,b.brand_name from {0}goods as p left join {0}goods_category as c on p.cat_id=c.cat_id left join {0}goods_brand as b on b.brand_id=p.brand_id where p.goods_id=@goods_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取goods信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public DataTable GetGoodsFullInfo(string urlrewriter)
        {
            string SQL = string.Format("select p.*,c.cat_name,c.urlrewriter as cat_urlrewriter,b.brand_name from {0}goods as p left join {0}goods_category as c on p.cat_id=c.cat_id left join {0}goods_brand as b on b.brand_id=p.brand_id where p.urlrewriter=@urlrewriter", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@urlrewriter", (DbType)SqlDbType.NVarChar, 100, urlrewriter)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取商品属性
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="spec_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsSpecs(int spec_id, int goods_id)
        {
            string SQL = string.Format("SELECT v.*,i.id FROM {0}goods_spec_values as v left join {0}goods_spec_index as i on  v.spec_value_id=i.spec_value_id and i.goods_id=@goods_id where v.spec_id=@spec_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@spec_id", (DbType)SqlDbType.Int, 4, spec_id),
	            DbHelper.MakeInParam("@goods_id", (DbType)SqlDbType.Int, 4, goods_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 插入商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        public int InsertGoodsCategory(GoodsCategoryInfo catinfo)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@parent_id", (DbType)SqlDbType.Int, 4, catinfo.parent_id.Value),
	            DbHelper.MakeInParam("@cat_name", (DbType)SqlDbType.NVarChar, 50, catinfo.cat_name),
                DbHelper.MakeInParam("@cat_desc", (DbType)SqlDbType.NVarChar, 1000, catinfo.cat_desc),
                DbHelper.MakeInParam("@measure_unit", (DbType)SqlDbType.NVarChar, 1000, catinfo.measure_unit),
                DbHelper.MakeInParam("@is_show", (DbType)SqlDbType.Bit, 0, catinfo.is_show.Value),
                DbHelper.MakeInParam("@cat_ico", (DbType)SqlDbType.NVarChar, 150, catinfo.cat_ico),
                DbHelper.MakeInParam("@list_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.list_tlp),
                DbHelper.MakeInParam("@info_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.info_tlp),
                DbHelper.MakeInParam("@page_size", (DbType)SqlDbType.Int, 4, catinfo.page_size.Value),
                DbHelper.MakeInParam("@pagetitle", (DbType)SqlDbType.NVarChar, 50, catinfo.pagetitle),
                DbHelper.MakeInParam("@pagekeywords", (DbType)SqlDbType.NVarChar, 150, catinfo.pagekeywords),
                DbHelper.MakeInParam("@pagedesc", (DbType)SqlDbType.NVarChar, 300, catinfo.pagedesc),
                //DbHelper.MakeInParam("@pricearea",(DbType)SqlDbType.NVarChar,4000,catinfo.pricearea),sort_order
                DbHelper.MakeInParam("@cat_name_en", (DbType)SqlDbType.NVarChar, 50, catinfo.cat_name_en),
                DbHelper.MakeInParam("@is_mobile", (DbType)SqlDbType.Bit, 0, catinfo.is_mobile.Value),
                DbHelper.MakeInParam("@urlrewriter", (DbType)SqlDbType.NVarChar, 50, catinfo.urlrewriter),
                DbHelper.MakeInParam("@cat_h_ico", (DbType)SqlDbType.NVarChar, 300, catinfo.cat_h_ico),
                DbHelper.MakeInParam("@sort_order", (DbType)SqlDbType.Int, 4, catinfo.sort_order)
            };

            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}goods_category_insert",BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 更新商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        public int UpdateGoodsCategory(GoodsCategoryInfo catinfo)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@cat_id", (DbType)SqlDbType.Int, 4, catinfo.cat_id.Value),
                DbHelper.MakeInParam("@parent_id", (DbType)SqlDbType.Int, 4, catinfo.parent_id.Value),
	            DbHelper.MakeInParam("@cat_name", (DbType)SqlDbType.NVarChar, 50, catinfo.cat_name),
                DbHelper.MakeInParam("@cat_desc", (DbType)SqlDbType.NVarChar, 1000, catinfo.cat_desc),
                DbHelper.MakeInParam("@measure_unit", (DbType)SqlDbType.NVarChar, 1000, catinfo.measure_unit),
                DbHelper.MakeInParam("@is_show", (DbType)SqlDbType.Bit, 0, catinfo.is_show.Value),
                DbHelper.MakeInParam("@cat_ico", (DbType)SqlDbType.NVarChar, 150, catinfo.cat_ico),
                DbHelper.MakeInParam("@list_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.list_tlp),
                DbHelper.MakeInParam("@info_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.info_tlp),
                DbHelper.MakeInParam("@page_size", (DbType)SqlDbType.Int, 4, catinfo.page_size.Value),
                DbHelper.MakeInParam("@pagetitle", (DbType)SqlDbType.NVarChar, 50, catinfo.pagetitle),
                DbHelper.MakeInParam("@pagekeywords", (DbType)SqlDbType.NVarChar, 150, catinfo.pagekeywords),
                DbHelper.MakeInParam("@pagedesc", (DbType)SqlDbType.NVarChar, 300, catinfo.pagedesc),
                //DbHelper.MakeInParam("@pricearea",(DbType)SqlDbType.NVarChar,4000,catinfo.pricearea),
                DbHelper.MakeInParam("@cat_name_en", (DbType)SqlDbType.NVarChar, 50, catinfo.cat_name_en),
                DbHelper.MakeInParam("@is_mobile", (DbType)SqlDbType.Bit, 0, catinfo.is_mobile.Value),
                DbHelper.MakeInParam("@urlrewriter", (DbType)SqlDbType.NVarChar, 50, catinfo.urlrewriter),
                DbHelper.MakeInParam("@cat_h_ico", (DbType)SqlDbType.NVarChar, 300, catinfo.cat_h_ico),
                DbHelper.MakeInParam("@sort_order", (DbType)SqlDbType.Int, 4, catinfo.sort_order)
            };

            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}goods_category_update", BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="cat_id"></param>
        public int DeleteGoodsCategory(int cat_id)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@cat_id", (DbType)SqlDbType.Int, 4, cat_id)
            };

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}goods_category_delete", BaseConfig.TablePrefix), parms));
        }
        /// <summary>
        /// 获取当前分类的所有父类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public DbDataReader GetPrevGoodsCat(int cat_id)
        {
            DbParameter[] parms = {
			    DbHelper.MakeInParam("@cat_id", (DbType)SqlDbType.Int, 4, cat_id)
		    };

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}get_goods_prevcat", BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 获取商品收藏夹
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsFavorites(int user_id)
        {
            string SQL = string.Format("select g.goods_id,g.goods_name,g.goods_thumb,g.goods_img,g.shop_price,g.urlrewriter,f.add_time,f.id from {0}goods as g,{0}favorites as f where f.id_value=g.goods_id and f.type= 1 and f.user_id=@user_id order by f.add_time desc", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@user_id", (DbType)SqlDbType.Int, 4, user_id)
		    };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 移动商品分类位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public int MoveGoodsCategoryPos(string act, int cat_id)
        {
            int state = 0;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DbHelper.ConnectionString);
            conn.Open();

            using (System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DataRow curcat = DbHelper.ExecuteDataset(trans, CommandType.Text, string.Format("SELECT TOP 1 * FROM {0}goods_category WHERE cat_id={1}", BaseConfig.TablePrefix, cat_id)).Tables[0].Rows[0];

                    #region 向上移动
                    if (act == "up")
                    {
                        //更新其它分类位置
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE {0}goods_category SET sort_order=sort_order-1 WHERE sort_order<{1} and cat_level={2} and parent_id={3}", BaseConfig.TablePrefix, Convert.ToInt32(curcat["sort_order"]), Convert.ToInt32(curcat["cat_level"]), Convert.ToInt32(curcat["parent_id"])));

                        //更新当前分类位置
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE {0}goods_category SET sort_order=sort_order+1 WHERE cat_id={1}", BaseConfig.TablePrefix, cat_id));
                    }
                    #endregion

                    #region 向下移动
                    else if (act == "down")
                    {
                        //更新当前分类位置
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE {0}goods_category SET sort_order=sort_order-1 WHERE cat_id={1}", BaseConfig.TablePrefix, cat_id));
                    }
                    #endregion

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }

            return state;
        }

        /// <summary>
        /// 移动商品位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public int MoveGoodsPos(string act, int goods_id)
        {
            int state = 0;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(DbHelper.ConnectionString);
            conn.Open();

            using (System.Data.SqlClient.SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DataRow curcat = DbHelper.ExecuteDataset(trans, CommandType.Text, string.Format("SELECT TOP 1 * FROM {0}goods WHERE goods_id={1}", BaseConfig.TablePrefix, goods_id)).Tables[0].Rows[0];

                    #region 向上移动
                    if (act == "up")
                    {
                        //更新其它分类位置
                        //DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE {0}cms SET sort_order=sort_order+1 WHERE sort_order<{1} and cat_level={2} and parent_id={3}", BaseConfig.TablePrefix, Convert.ToInt32(curcat["sort_order"]), Convert.ToInt32(curcat["cat_level"]), Convert.ToInt32(curcat["parent_id"])));

                        //更新当前位置
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE {0}goods SET sort_order=sort_order+1 WHERE goods_id={1}", BaseConfig.TablePrefix, goods_id));
                    }
                    #endregion

                    #region 向下移动
                    else if (act == "down")
                    {
                        //更新当前位置
                        DbHelper.ExecuteNonQuery(trans, CommandType.Text, string.Format("UPDATE {0}goods SET sort_order=sort_order-1 WHERE goods_id={1}", BaseConfig.TablePrefix, goods_id));
                    }
                    #endregion

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                conn.Close();
            }

            return state;
        }

    }
}
