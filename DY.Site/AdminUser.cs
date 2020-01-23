using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 网站管理用户类
    /// </summary>
    public class AdminUser
    {
        #region 静态函数
        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="loginTime">登录时间</param>
        /// <param name="loginIp">登录IP</param>
        /// <param name="uid">用户ID</param>
        public static void UpdateAdminLoginInfo(DateTime loginTime,string loginIp,int uid)
        {
            AdminUserInfo userinfo = new AdminUserInfo();
            userinfo.last_ip = loginIp;
            userinfo.last_login = loginTime;
            userinfo.user_id = uid;

            DatabaseProvider.GetInstance().UpdateAdminLoginInfo(userinfo);
        }
        /// <summary>
        /// 判断指定用户名是否已存在
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>如果已存在该用户id则返回true, 否则返回false</returns>
        public static bool Exists(int uid)
        {
            return DatabaseProvider.GetInstance().AdminExists(uid);
        }
        /// <summary>
        /// 判断指定用户名是否已存在.
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
        public static bool Exists(string username)
        {
            return DatabaseProvider.GetInstance().AdminExists(username);
        }
        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <returns>如果正确则返回uid</returns>
        public static int CheckPassword(string username, string password, bool originalpassword)
        {
            return DatabaseProvider.GetInstance().AdminCheckPassword(username, originalpassword ? SiteUtils.Encryption(password) : password);
        }       
        /// <summary>
        /// 返回指定管理员信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public static AdminUserInfo GetUserInfo(int uid)
        {
            AdminUserInfo usrinfo = null;
            using (DbDataReader rdr = DatabaseProvider.GetInstance().GetAdminUserInfo(uid))
            {
                if (rdr.Read())
                {
                    usrinfo = new AdminUserInfo(
                        rdr.GetInt32(0),
                        rdr.GetString(1),
                        rdr.GetString(2),
                        rdr.GetString(3),
                        rdr.GetDateTime(4),
                        rdr.GetDateTime(5),
                        rdr.GetString(6),
                        rdr.GetString(7),
                        rdr.GetString(8),
                        rdr.GetString(9),
                        rdr.GetInt32(10),
                        rdr.GetString(11)
                   );
                }
            }

            return usrinfo;
        }

        /// <summary>
        /// 虚拟管理员信息
        /// </summary>
        /// <returns>用户信息</returns>
        public static AdminUserInfo GetUserInfo()
        {
            AdminUserInfo usrinfo = null;
            usrinfo = new AdminUserInfo(
                0,
                BaseConfig.username,
                "weihu@qq.com",
                SiteUtils.Encryption(BaseConfig.password),
                DateTime.Now,
                DateTime.Now,
                "127.0.0.1",
                "all",
                "",
                "",
                0,
                ""
           );

            return usrinfo;
        }    
        #endregion
    }
}
