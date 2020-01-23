using System;
using System.Collections.Generic;
namespace DY.OAuthV2SDK.OAuths.Sinas.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class SinaMUserList : SinaMError
    {

        /// <summary>
        /// 用户列表 
        /// </summary>
        public List<SinaMUser> users { set; get; }

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