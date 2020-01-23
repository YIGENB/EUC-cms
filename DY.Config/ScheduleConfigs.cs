using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using DY.Common;

namespace DY.Config
{
	/// <summary>
	/// 基本设置类
	/// </summary>
    public class ScheduleConfigs
	{
        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static ScheduleConfigInfo GetConfig()
        {
            return ScheduleConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public static bool SaveConfig(ScheduleConfigInfo scheduleconfiginfo)
        {
            ScheduleConfigFileManager scfm = new ScheduleConfigFileManager();
            ScheduleConfigFileManager.ConfigInfo = scheduleconfiginfo;
            return scfm.SaveConfig();
        }


	}
}
