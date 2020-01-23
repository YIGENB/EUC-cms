using System;
using System.Collections.Generic;
namespace DY.OAuthV2SDK.OAuths.Tencs.Models
{
    /// <summary>
    /// 微博数据信息实体类
    /// </summary>
    [Serializable]
    public class TencMTweetData : TencMError
    {
        /// <summary>
        /// 微博数据
        /// </summary>
        public TencMTweet data { set; get; }       
    }

}