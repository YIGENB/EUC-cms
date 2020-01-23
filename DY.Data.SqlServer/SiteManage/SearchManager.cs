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
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Reflection;

using DY.Data;
using DY.Entity;
using DY.Config;

namespace DY.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 插入Search
        /// </summary>
        /// <param name="adinfo"></param>
        public int InsertSearchInfo(SearchInfo entity)
        {
            Type entityType = entity.GetType();
            PropertyInfo[] propertyInfos = entityType.GetProperties();
            StringBuilder sb = new StringBuilder("INSERT INTO {0}search ("), sbParamer = new StringBuilder(" (");
            List<DbParameter> paramerList = new List<DbParameter>(); //参数集合
            foreach (PropertyInfo property in propertyInfos)
            {
                object oval = property.GetValue(entity, null);
                oval = oval == null ? DBNull.Value : oval;

                if (oval.GetType() != typeof(System.DBNull))
                {
                    if (property.Name != "search_id")
                    {
                        sb.Append(property.Name);
                        sb.Append(",");

                        sbParamer.Append("@" + property.Name);
                        sbParamer.Append(",");

                        paramerList.Add(DbHelper.MakeInParam("@" + property.Name, DbHelper.MakeDbType(oval), 0, oval));
                    }
                }
            }

            //截取掉最后一个多于的参数分隔','符号
            sb.Remove(sb.Length - 1, 1);
            sbParamer.Remove(sbParamer.Length - 1, 1);

            //拼接最终SQL
            sb.Append(") VALUES ");
            sb.Append(sbParamer.ToString() + ")");
            sb.Append(";SELECT SCOPE_IDENTITY()");

            string SQL = string.Format(sb.ToString(), BaseConfig.TablePrefix);
            //throw new Exception(SQL);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, SQL, paramerList.ToArray()));
        }
        /// <summary>
        /// 更新Search
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public void UpdateSearchInfo(SearchInfo entity)
        {
            Type entityType = entity.GetType();
            PropertyInfo[] propertyInfos = entityType.GetProperties();
            StringBuilder sb = new StringBuilder("UPDATE {0}search SET ");
            List<DbParameter> paramerList = new List<DbParameter>(); //参数集合
            foreach (PropertyInfo property in propertyInfos)
            {
                object oval = property.GetValue(entity, null);
                oval = oval == null ? DBNull.Value : oval;

                if (oval.GetType() != typeof(System.DBNull))
                {
                    if (property.Name != "search_id")
                    {
                        sb.Append(property.Name + "=@" + property.Name);
                        sb.Append(",");
                    }

                    paramerList.Add(DbHelper.MakeInParam("@" + property.Name, DbHelper.MakeDbType(oval),0, oval));
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE search_id=@search_id");

            string SQL = string.Format(sb.ToString(),BaseConfig.TablePrefix);

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, paramerList.ToArray());
        }
        /// <summary>
        /// 获取Search信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public DbDataReader GetSearchInfo(int search_id)
        {
            string SQL = string.Format("SELECT * FROM {0}search WHERE [search_id]=@search_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@search_id", (DbType)SqlDbType.Int, 4, search_id)
            };

            return DbHelper.ExecuteReader(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 获取Search信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DbDataReader GetSearchInfo(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
                filter = "and " + filter;

            string SQL = string.Format("SELECT * FROM {0}search WHERE [search_id]>0 {1}", BaseConfig.TablePrefix, filter);

            return DbHelper.ExecuteReader(CommandType.Text, SQL);
        }
        /// <summary>
        /// 删除Search信息（按主键删除）
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public void DeleteSearchInfo(int search_id)
        {
            string SQL = string.Format("DELETE FROM {0}search WHERE [search_id]=@search_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
	            DbHelper.MakeInParam("@search_id", (DbType)SqlDbType.Int, 4, search_id)
            };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool ExistsSearch(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
                filter = "and " + filter;

            string SQL = string.Format("SELECT COUNT(search_id) FROM {0}search WHERE [search_id]>0 {1}", BaseConfig.TablePrefix, filter);

            return Convert.ToInt32(DbHelper.ExecuteScalarToStr(CommandType.Text, SQL)) > 0 ? true : false;
        }
        /// <summary>
        /// 获取Search单个值
        /// </summary>
        /// <param name="Fields"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public object GetSearchValue(string Fields, string filter)
        {
            if (!string.IsNullOrEmpty(filter))
                filter = "and " + filter;

            string SQL = string.Format("SELECT {2} FROM {0}search WHERE [search_id]>0 {1}", BaseConfig.TablePrefix, filter,Fields);

            return DbHelper.ExecuteScalarToStr(CommandType.Text, SQL);
        }
    }
}