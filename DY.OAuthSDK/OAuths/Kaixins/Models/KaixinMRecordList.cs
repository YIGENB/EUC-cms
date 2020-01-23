using System;
using System.Collections.Generic;
namespace DY.OAuthV2SDK.OAuths.Kaixins.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class KaixinMRecordList : KaixinMError
    {
        /// <summary>
        /// 用户列表 
        /// </summary>
        public KaixinMRecord[] data { set; get; }

        /// <summary>
        ///  分页信息
        /// </summary>
        public KaixinMPaging paging { get; set; }

    }
    /// <summary>
    /// 分页信息
    /// </summary>
    public class KaixinMPaging
    {
        /// <summary>
        /// 总数
        /// </summary>
        public string total { set; get; }
        /// <summary>
        /// 上一页的分页起始数	
        /// </summary>
        public string prev { set; get; }
        /// <summary>
        /// 下一页的分页起始数
        /// </summary>
        public string next { set; get; }
    }

}