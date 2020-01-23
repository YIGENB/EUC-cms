using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

using DY.Data;
using DY.Entity;
using DY.Config;

namespace DY.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 获取网站配置数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public System.Data.DataTable GetConfigList(string filter)
        {
            string whereSql = "";
            if (!string.IsNullOrEmpty(filter))
                whereSql = " where " + filter;
            string SQL = string.Format("SELECT * FROM {0}Config{1}", BaseConfig.TablePrefix, whereSql);

            using (DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, SQL, null).Tables[0])
            {
                return dt;
            }
        }
        /// <summary>
        /// 获取网站设置数据
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DbDataReader GetConfigData(int parent_id)
        {
            string SQL = string.Format("SELECT id,parent_id,name,code,type,tip,size,store_range,store_dir,value,sort_order,isshow FROM {0}Config WHERE parent_id=@parent_id and isshow=@isshow order by sort_order desc", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@parent_id", (DbType)SqlDbType.Int, 4, parent_id),
                DbHelper.MakeInParam("@isshow", (DbType)SqlDbType.Int, 4, 1)
		    };

            return DbHelper.ExecuteReader(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 根据IP获取错误登录记录
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int GetErrLoginCountByIP(string ip)
        {
            string SQL = string.Format("SELECT count(log_id) as errcount FROM [{0}Admin_Log] WHERE [ip_address]=@ip_address AND [log_type]=1", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@ip_address", (DbType)SqlDbType.Char, 15, ip)
		    };

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, SQL, parms));
        }
        /// <summary>
        /// 删除指定ip地址的登录错误日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        public void DeleteErrLoginRecord(string ip)
        {
            string SQL = string.Format("DELETE FROM {0}Admin_Log WHERE [ip_address]=@ip and log_type=1",BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@ip", (DbType)SqlDbType.Char, 15, ip)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 更新网站配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public void UpdateConfigInfo(int id, string val)
        {
            string SQL = string.Format("UPDATE {0}Config SET [value]=@value WHERE id=@id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@value", (DbType)SqlDbType.NVarChar, 4000, val),
                DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int,4,id)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 根据存储过程查询数据
        /// </summary>
        /// <param name="TableName">要分页显示的表名</param>
        /// <param name="FieldKey">用于定位记录的主键(惟一键)字段,可以是逗号分隔的多个字段</param>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="FieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public DbDataReader GetPagerData(string TableName, string FieldKey, int PageCurrent, int PageSize, string FieldShow, string FieldOrder, string Where, out int ResultCount)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@tbname", (DbType)SqlDbType.NVarChar, 50, string.Format("{0}" + TableName, BaseConfig.TablePrefix)),
                DbHelper.MakeInParam("@FieldKey", (DbType)SqlDbType.NVarChar, 100, FieldKey),
                DbHelper.MakeInParam("@PageCurrent", (DbType)SqlDbType.Int, 4, PageCurrent),
                DbHelper.MakeInParam("@PageSize", (DbType)SqlDbType.Int, 4, PageSize),
                DbHelper.MakeInParam("@FieldShow", (DbType)SqlDbType.NVarChar, 0, FieldShow),
                DbHelper.MakeInParam("@FieldOrder", (DbType)SqlDbType.NVarChar, 200, FieldOrder),
                DbHelper.MakeInParam("@Where", (DbType)SqlDbType.VarChar, 8000, Where),
                DbHelper.MakeParam("@ResultCount", (DbType)SqlDbType.Int, 4, ParameterDirection.Output, null)
            };
 
            DbConnection connection = DbHelper.Factory.CreateConnection();
            connection.ConnectionString = DbHelper.ConnectionString;
            connection.Open();

            DbCommand command = DbHelper.Factory.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;
            command.CommandText = string.Format("{0}PageView", BaseConfig.TablePrefix);
            foreach (DbParameter p in parms)
            {
                if (p != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    command.Parameters.Add(p);
                }
            }

            DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            //reader.Dispose();
            ResultCount = Convert.ToInt32(command.Parameters["@ResultCount"].Value);

            return reader;
        }
        /// <summary>
        /// 更改指定表的某个字段的值
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="Value">值</param>
        /// <param name="key">主键名</param>
        /// <param name="id">主键值</param>
        /// <returns>返回已更改的标识列</returns>
        public void UpdateFieldValue(string TableName, string Field, object Value,string key,int id)
        {
            string SQL = string.Format("UPDATE {0}{1} SET [{2}]=@value WHERE {3}=@id", BaseConfig.TablePrefix, TableName, Field, key);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@value", DbHelper.MakeDbType(Value) , 0, Value),
                DbHelper.MakeInParam("@id",(DbType)SqlDbType.Int,4,id)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 更改指定表的某个字段的值
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="Value">值</param>
        /// <param name="key">主键名</param>
        /// <param name="ids">主键值</param>
        /// <returns>返回已更改的标识列</returns>
        public void UpdateFieldValue(string TableName, string Field, object Value, string key, string ids)
        {
            string SQL = string.Format("UPDATE {0}{1} SET [{2}]=@value WHERE {3} in ({4})", BaseConfig.TablePrefix, TableName, Field, key,ids);
            //throw new Exception(SQL);
            DbParameter[] parms = {
			    DbHelper.MakeInParam("@value", Value.GetType() == typeof(string) ? (DbType)SqlDbType.NVarChar : Value.GetType() == typeof(DateTime)? (DbType)SqlDbType.DateTime : Value.GetType() == typeof(Boolean)? (DbType)SqlDbType.Bit : (DbType)SqlDbType.Int , 0, Value)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="TableName">要分页显示的表名</param>
        /// <param name="FieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public DbDataReader GetAllData(string TableName, string FieldShow, string FieldOrder, string Where)
        {
            DbParameter[] parms = {
			    DbHelper.MakeInParam("@tbname", (DbType)SqlDbType.NVarChar, 50, string.Format("{0}" + TableName, BaseConfig.TablePrefix)),
                DbHelper.MakeInParam("@FieldShow", (DbType)SqlDbType.NVarChar, 500, FieldShow),
                DbHelper.MakeInParam("@FieldOrder", (DbType)SqlDbType.NVarChar, 200, FieldOrder),
                DbHelper.MakeInParam("@Where", (DbType)SqlDbType.NVarChar, 2000, Where)
		    };

            return DbHelper.ExecuteReader(CommandType.StoredProcedure, string.Format("{0}GetAllData",BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="TableName">要分页显示的表名</param>
        /// <param name="FieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public DataTable GetAllDataToDataTable(string TableName, string FieldShow, string FieldOrder, string Where)
        {
            DbParameter[] parms = {
			    DbHelper.MakeInParam("@tbname", (DbType)SqlDbType.NVarChar, 50, string.Format("{0}" + TableName, BaseConfig.TablePrefix)),
                DbHelper.MakeInParam("@FieldShow", (DbType)SqlDbType.NVarChar, 500, FieldShow),
                DbHelper.MakeInParam("@FieldOrder", (DbType)SqlDbType.NVarChar, 200, FieldOrder),
                DbHelper.MakeInParam("@Where", (DbType)SqlDbType.NVarChar, 2000, Where)
		    };

            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}GetAllData", BaseConfig.TablePrefix), parms).Tables[0];
        }
        /// <summary>
        /// 按自定义条件删除指定表中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public void DeleteData(string tbname, string filter)
        {
            DbParameter[] parms = {
			    DbHelper.MakeInParam("@tbname", (DbType)SqlDbType.NVarChar, 50, string.Format("{0}" + tbname, BaseConfig.TablePrefix)),
                DbHelper.MakeInParam("@Where", (DbType)SqlDbType.NVarChar, 0, filter)
		    };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}deletedata",BaseConfig.TablePrefix), parms);
        }
        /// <summary>
        /// 获取指定所的所有字段
        /// </summary>
        /// <param name="tbname"></param>
        /// <returns></returns>
        public DataTable GetTableColumns(string tbname)
        {
            string SQL = string.Format("SELECT * FROM {0}{1} WHERE 1=2", BaseConfig.TablePrefix, tbname);

            return DbHelper.ExecuteDataset(CommandType.Text, SQL).Tables[0];
        }
        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseVersion()
        {
            string SQL = "SELECT SERVERPROPERTY('productversion') as version";

            return DbHelper.ExecuteScalar(CommandType.Text, SQL).ToString();
        }

        /// <summary>
        /// 获取指定广告位的广告信息
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        public DataTable GetAds(int pos_id)
        {
            string SQL = string.Format("select * from {0}ad as a,{0}ad_position as p where a.position_id=p.position_id and p.position_id = @position_id and a.enabled=1  order by orderid asc", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@position_id", (DbType)SqlDbType.Int, 4, pos_id)
		    };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }

        /// <summary>
        /// 获取指定表单的控件列表
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        public DataTable GetForm(int pos_id)
        {
            string SQL = string.Format("select * from {0}form_all as a,{0}form_position as p where a.parent_id=p.position_id and p.position_id = @position_id and a.isshow=1  order by sort_order asc", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@position_id", (DbType)SqlDbType.Int, 4, pos_id)
		    };

            return DbHelper.ExecuteDataset(CommandType.Text, SQL, parms).Tables[0];
        }
        /// <summary>
        /// 获取指定表单的不重复session_id列表
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        public DataTable GetFormvalueSessionId(int position_id)
        {
            string SQL = string.Format("select session_id from {0}fromvalue  where position_id=" + position_id+" group by session_id having count(1) >= 2", BaseConfig.TablePrefix);

            return DbHelper.ExecuteDataset(CommandType.Text, SQL).Tables[0];
        }

        /// <summary>
        /// 添加到收藏夹
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <param name="id_value"></param>
        public int SaveFavorites(int user_id, int type, int id_value)
        {
            string SQL = string.Format("INSERT INTO {0}favorites (user_id,type,id_value) VALUES (@user_id,@type,@id_value);SELECT SCOPE_IDENTITY()", BaseConfig.TablePrefix);

            DbParameter[] parms = {
                DbHelper.MakeInParam("@user_id", (DbType)SqlDbType.Int, 4, user_id),
                DbHelper.MakeInParam("@type", (DbType)SqlDbType.NVarChar, 120, type),
                DbHelper.MakeInParam("@id_value", (DbType)SqlDbType.Int, 4, id_value)
            };

            object val = DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(id) FROM {0}favorites WHERE user_id=@user_id and type=@type and id_value=@id_value",BaseConfig.TablePrefix), parms);

            if (Convert.ToInt32(val) > 0)
                return -1;

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, SQL, parms));
        }
        /// <summary>
        /// 删除收藏夹信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        public void DeleteFavorites(int user_id, int id)
        {
            string SQL = string.Format("DELETE FROM {0}favorites WHERE [user_id]=@user_id and id=@id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@user_id", (DbType)SqlDbType.Int, 4, user_id),
                DbHelper.MakeInParam("@id", (DbType)SqlDbType.Int, 4, id)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }

        /// <summary>
        /// 相关联的商品
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int SqlProcess(string sql)
        {
            string SQL = string.Format(sql);

            return DbHelper.ExecuteNonQuery(SQL);
        }

        #region 计划任务
        public void SetLastExecuteScheduledEventDateTime(string key, string serverName, DateTime lastExecuted)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)SqlDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)SqlDbType.VarChar, 100, serverName),
                DbHelper.MakeInParam("@lastexecuted", (DbType)SqlDbType.DateTime, 8, lastExecuted)
            };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                     string.Format("{0}setlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix),
                                     parms);
        }

        public DateTime GetLastExecuteScheduledEventDateTime(string key, string serverName)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@key", (DbType)SqlDbType.VarChar, 100, key),
                DbHelper.MakeInParam("@servername", (DbType)SqlDbType.VarChar, 100, serverName),
                DbHelper.MakeOutParam("@lastexecuted", (DbType)SqlDbType.DateTime, 8)
            };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure,
                                     string.Format("{0}getlastexecutescheduledeventdatetime", BaseConfigs.GetTablePrefix),
                                     parms);

            return Convert.IsDBNull(parms[2].Value) ? DateTime.MinValue : Convert.ToDateTime(parms[2].Value);
        }
        #endregion

     }
}
