using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

using DY.Common;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 在线用户操作类
    /// </summary>
    public class OnlineAdminUsers
    {
        private static object SynObject = new object();

        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        /// <param name="passwordkey">用户密码</param
        /// <param name="timeout">在线超时时间</param>>
        public static OnlineUserInfo UpdateInfo(string passwordkey)
        {
            return UpdateInfo(passwordkey, -1, "");
        }
        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        /// <param name="passwordkey">论坛passwordkey</param>
        /// <param name="timeout">在线超时时间</param>
        /// <param name="passwd">用户密码</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int uid, string passwd)
        {
            lock (SynObject)
            {
                OnlineUserInfo onlineuser = null;
                string ip = DYRequest.GetIP();
                int userid = Utils.StrToInt(SiteUtils.GetCookie("userid"), uid);
                string password = (Utils.StrIsNullOrEmpty(passwd) ? SiteUtils.GetCookiePassword(passwordkey) : SiteUtils.GetCookiePassword(passwd, passwordkey));

                // 如果密码非Base64编码字符串则怀疑被非法篡改, 直接置身份为游客
                if (password.Length == 0 || !Utils.IsBase64String(password))
                    userid = -1;

                if (userid != -1)
                {
                    onlineuser = GetOnlineUser(userid, password);

                    if (onlineuser != null)
                    {
                        return onlineuser;
                    }
                    #region 登录之后虚拟数据
                    else
                    {
                        onlineuser = new OnlineUserInfo();
                        onlineuser.Userid = userid;
                        onlineuser.Username = DY.Config.BaseConfig.username;
                        onlineuser.Password = password;
                        onlineuser.Ip = "127.0.0.1";
                        onlineuser.Actions = "all";
                    }
                    #endregion
                }
                else
                {
                    onlineuser = new OnlineUserInfo();
                    onlineuser.Userid = userid;
                }

                return onlineuser;
            }
        }
        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        private static OnlineUserInfo GetOnlineUser(int userid, string password)
        {
            OnlineUserInfo userinfo = null;

            using (DbDataReader rdr = DatabaseProvider.GetInstance().GetAdminOnlineUser(userid, password))
            {
                if (rdr.Read())
                {
                    userinfo = new OnlineUserInfo();
                    userinfo.Userid = rdr.GetInt32(0);
                    userinfo.Username = rdr.GetString(1);
                    userinfo.Password = rdr.GetString(2);
                    userinfo.Ip = rdr.GetString(3);
                    userinfo.Actions = rdr.GetString(4);
                }
            }

            return userinfo;
        }
    }
}
