using System;
using System.Collections.Generic;
namespace DY.OAuthV2SDK.OAuths.Neasys.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class NeasyMStatusList : NeasyMError
    {
        /// <summary>
        /// 用户列表 
        /// </summary>
        public List<NeasyMStatus> statuses { set; get; }

        /// <summary>
        /// 下一页用返回值里的next_cursor
        /// </summary>
        public int next_cursor { set; get; }

        /// <summary>
        /// 上一页用previous_cursor
        /// </summary>
        public int previous_cursor { set; get; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int total_number { set; get; }

    }
}