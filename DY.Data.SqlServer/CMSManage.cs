using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Config;
using DY.Data;
using DY.Entity;

namespace DY.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 获取商品分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetCMSCat()
        {
            string SQL = string.Format("SELECT c.cat_id, c.cat_name, c.parent_id, c.show_in_nav,c.is_review, c.sort_order,c.cat_level,c.urlrewriter, COUNT(s.cat_id) AS has_children FROM {0}cms_cat AS c LEFT JOIN {0}cms_cat AS s ON s.parent_id=c.cat_id GROUP BY c.cat_id, c.cat_name, c.parent_id, c.show_in_nav,c.is_review, c.sort_order,c.cat_level,c.urlrewriter ORDER BY c.parent_id, c.sort_order ASC", BaseConfig.TablePrefix);

            return DbHelper.ExecuteDataset(CommandType.Text, SQL).Tables[0];
        }
        /// <summary>
        /// 获取cms信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public DataTable GetCMSFullInfo(string urlrewriter)
        {
            string SQL = string.Format("select p.*,c.cat_name,c.is_review,c.urlrewriter as cat_urlrewriter from {0}cms_cat as c,{0}cms as p where p.cat_id=c.cat_id and p.urlrewriter=@urlrewriter", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@urlrewriter", (DbType)SqlDbType.NVarChar, 100, urlrewriter)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取cms信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public DataTable GetCMSFullInfo(int article_id)
        {
            string SQL = string.Format("select p.*,c.cat_name,c.is_review,c.urlrewriter as cat_urlrewriter from {0}cms_cat as c,{0}cms as p where p.cat_id=c.cat_id and p.article_id=@article_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@article_id", (DbType)SqlDbType.Int, 4, article_id)
            };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 插入文章分类
        /// </summary>
        /// <param name="catinfo"></param>
        public int InsertCMSCategory(CmsCatInfo catinfo)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@parent_id", (DbType)SqlDbType.Int, 4, catinfo.parent_id.Value),
	            DbHelper.MakeInParam("@cat_ame", (DbType)SqlDbType.NVarChar, 50, catinfo.cat_name),
                DbHelper.MakeInParam("@list_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.list_tlp),
                DbHelper.MakeInParam("@info_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.info_tlp),
                DbHelper.MakeInParam("@page_size", (DbType)SqlDbType.Int, 4, catinfo.page_size.Value),
                DbHelper.MakeInParam("@pagetitle", (DbType)SqlDbType.NVarChar, 50, catinfo.pagetitle),
                DbHelper.MakeInParam("@pagekeywords", (DbType)SqlDbType.NVarChar, 150, catinfo.pagekeywords),
                DbHelper.MakeInParam("@pagedesc", (DbType)SqlDbType.NVarChar, 300, catinfo.pagedesc),
                DbHelper.MakeInParam("@pic", (DbType)SqlDbType.Text, 150, catinfo.pic),
                DbHelper.MakeInParam("@ms", (DbType)SqlDbType.Text, 150, catinfo.ms),
                DbHelper.MakeInParam("@en_cat_name", (DbType)SqlDbType.NVarChar, 150, catinfo.en_cat_name),
                DbHelper.MakeInParam("@is_mobile", (DbType)SqlDbType.Int, 4, catinfo.is_mobile),
                DbHelper.MakeInParam("@urlrewriter", (DbType)SqlDbType.NVarChar, 50, catinfo.urlrewriter)
            };

            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}cms_category_insert", BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 更新文章分类
        /// </summary>
        /// <param name="catinfo"></param>
        public int UpdateCMSCategory(CmsCatInfo catinfo)
        {
            DbParameter[] parms = {
               DbHelper.MakeInParam("@cat_id", (DbType)SqlDbType.Int, 4, catinfo.cat_id.Value),
                DbHelper.MakeInParam("@parent_id", (DbType)SqlDbType.Int, 4, catinfo.parent_id.Value),
	            DbHelper.MakeInParam("@cat_name", (DbType)SqlDbType.NVarChar, 50, catinfo.cat_name),
                DbHelper.MakeInParam("@list_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.list_tlp),
                DbHelper.MakeInParam("@info_tlp", (DbType)SqlDbType.NVarChar, 150, catinfo.info_tlp),
                DbHelper.MakeInParam("@page_size", (DbType)SqlDbType.Int, 4, catinfo.page_size.Value),
                DbHelper.MakeInParam("@pagetitle", (DbType)SqlDbType.NVarChar, 50, catinfo.pagetitle),
                DbHelper.MakeInParam("@pagekeywords", (DbType)SqlDbType.NVarChar, 150, catinfo.pagekeywords),
                DbHelper.MakeInParam("@pagedesc", (DbType)SqlDbType.NVarChar, 300, catinfo.pagedesc),
                DbHelper.MakeInParam("@pic", (DbType)SqlDbType.Text, 150, catinfo.pic),
                DbHelper.MakeInParam("@ms", (DbType)SqlDbType.Text, 150, catinfo.ms),
                DbHelper.MakeInParam("@en_cat_name", (DbType)SqlDbType.NVarChar, 150, catinfo.en_cat_name),
                DbHelper.MakeInParam("@is_mobile", (DbType)SqlDbType.Int, 4, catinfo.is_mobile),
                DbHelper.MakeInParam("@urlrewriter", (DbType)SqlDbType.NVarChar, 50, catinfo.urlrewriter),
                DbHelper.MakeInParam("@show_in_nav", (DbType)SqlDbType.Int, 4, catinfo.show_in_nav),
                 DbHelper.MakeInParam("@sort_order", (DbType)SqlDbType.Int, 4, catinfo.sort_order.Value)
            };

            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}cms_category_update", BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="cat_id"></param>
        public int DeleteCMSCategory(int cat_id)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@cat_id", (DbType)SqlDbType.Int, 4, cat_id)
            };

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.StoredProcedure, string.Format("{0}cms_category_delete", BaseConfig.TablePrefix), parms));
        }
    }
}
