using System;
using System.Collections.Generic;
using System.Text;

using DY.Common;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 登录日志类
    /// </summary>
    public class AdminLog
    {
        #region 静态函数
        /// <summary>
		/// 增加错误次数并返回错误次数, 如不存在登录错误日志则建立
		/// </summary>
		/// <param name="ip">ip地址</param>
        /// <returns>int</returns>
        public static int UpdateLoginLog(string ip, bool add)
        {
            AdminLogInfo loginfo = new AdminLogInfo(0,DateTime.Now,0,"","尝试登录失败",DYRequest.GetIP(),1);

            int errcount = DatabaseProvider.GetInstance().GetErrLoginCountByIP(loginfo.ip_address)+1;

            if ((errcount > 3) || (!add))
			{
				return errcount;
			}
            else
            {
                DatabaseProvider.GetInstance().InsertAdminLogInfo(loginfo);
            }

            return errcount;
        }
        /// <summary>
        /// 删除指定ip地址的登录错误日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>int</returns>
        public static void DeleteLoginLog(string ip)
        {
            DatabaseProvider.GetInstance().DeleteErrLoginRecord(ip);
        }        
        /// <summary>
        /// 添加管理员登录成功日志
        /// </summary>
        /// <param name="uid">管理员ID</param>
        /// <param name="user_name">管理员用户名</param>
        public static void AddLoginLog(int uid,string user_name)
        {
            DatabaseProvider.GetInstance().InsertAdminLogInfo(new AdminLogInfo(0, DateTime.Now, uid, user_name, "成功登录后台", DYRequest.GetIP(), 0));
        }
        /// <summary>
        /// 添加后台操作日志
        /// </summary>
        /// <param name="uid">管理员ID</param>
        /// <param name="user_name">管理员用户名</param>
        /// <param name="log_info">日志信息</param>
        public static void AddLog(int uid, string user_name,string log_info)
        {
            DatabaseProvider.GetInstance().InsertAdminLogInfo(new AdminLogInfo(0, DateTime.Now, uid, user_name, log_info, DYRequest.GetIP(), 0));
        }

        /// <summary>
        ///日志文件保存
        /// </summary>
        /// <returns></returns>
        public static void WriteFile(string str)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(DY.Config.BaseConfig.LogPath + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            FileOperate.WriteFile(path, str + "\r\n", true);
        }
        #endregion
    }
}
