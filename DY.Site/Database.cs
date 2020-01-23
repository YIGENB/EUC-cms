using System;
using System.Collections.Generic;
using System.Text;

using DY.Data;

namespace DY.Site
{
    /// <summary>
    /// 数据库相关
    /// </summary>
    public class Database
    {
        /// <summary>
        /// 是否支持数据在线备份
        /// </summary>
        /// <returns></returns>
        public static bool IsBackup()
        {
            if (Version.IndexOf("8.0") >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// 是否支持还原
        /// </summary>
        /// <returns></returns>
        public static bool IsRestore()
        {
            if (Version.IndexOf("8.0") >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// 取得当前数据库版本号
        /// </summary>
        private static string Version
        {
            get
            {
                return DatabaseProvider.GetInstance().GetDatabaseVersion();
            }
        }
    }
}
