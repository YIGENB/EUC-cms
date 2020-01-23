using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

using DY.Data;
using DY.Entity;
using DY.Config;

namespace DY.Data.SqlServer
{
    public partial class DataProvider : IDataProvider
    {
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool AdminExists(int uid)
        {
            string SQL = string.Format("SELECT COUNT(1) FROM [{0}Admin_User] WHERE [user_id]=@user_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@user_id", (DbType)SqlDbType.Int, 4, uid)
		    };

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, SQL, parms)) >= 1;
        }
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool AdminExists(string username)
        {
            string SQL = string.Format("SELECT COUNT(1) FROM [{0}Admin_User] WHERE [user_name]=@user_name", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@user_name", (DbType)SqlDbType.NVarChar, 20, username)
		    };

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, SQL, parms)) >= 1;
        }
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public int AdminCheckPassword(string userName, string passWord)
        {
            string SQL = string.Format("SELECT user_id FROM {0}Admin_User WHERE user_name=@user_name AND password=@password",BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@user_name", (DbType)SqlDbType.NVarChar, 20, userName),
                DbHelper.MakeInParam("@password",(DbType)SqlDbType.NVarChar,32,passWord)
		    };

            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, SQL, parms));
        }
        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="userinfo"></param>
        public void UpdateAdminLoginInfo(AdminUserInfo userinfo)
        {
            string SQL = string.Format("UPDATE {0}Admin_User SET last_login=@last_login,last_ip=@last_ip WHERE user_id=@user_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@last_login", (DbType)SqlDbType.DateTime, 4, userinfo.last_login),
                DbHelper.MakeInParam("@last_ip",(DbType)SqlDbType.Char,16,userinfo.last_ip),
                DbHelper.MakeInParam("@user_id",(DbType)SqlDbType.Int,4,userinfo.user_id)
		    };

            DbHelper.ExecuteNonQuery(CommandType.Text, SQL, parms);
        }
        /// <summary>
        /// 获取当前在线用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public DbDataReader GetAdminOnlineUser(int userId, string passWord)
        {
            string SQL = string.Format("SELECT user_id,user_name,password,last_ip,action_list FROM {0}Admin_User WHERE user_id=@user_id", BaseConfig.TablePrefix);

            DbParameter[] parms = {
			    DbHelper.MakeInParam("@user_id", (DbType)SqlDbType.Int, 4, userId),
                DbHelper.MakeInParam("@password",(DbType)SqlDbType.NVarChar,32,passWord)
		    };

            return DbHelper.ExecuteReader(CommandType.Text, SQL, parms);
        }
    }
}
