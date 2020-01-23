using System;
using System.Collections.Generic;
namespace DY.OAuthV2SDK.OAuths.Toutiaos.Models
{
    /// <summary>
    /// 头条信息实体类
    /// </summary>
    [Serializable]
    public class ToutiaoMTweet : ToutiaoMError
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public string message { set; get; }
        /// <summary>
        /// 	失败原因
        /// </summary>
        public string data { set; get; }
    }

}