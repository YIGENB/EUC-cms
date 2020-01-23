using System;
namespace DY.OAuthV2SDK.OAuths.Neasys.Models
{
    /// <summary>
    /// 实体类MStatuses 。
    /// </summary>
    [Serializable]
    public class NeasyMStatus : NeasyMError
    {
        /// <summary>
        /// 微博ID 
        /// </summary>
        public long id { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string created_at { set; get; }
        /// <summary>
        /// 微博信息内容
        /// </summary>
        public string text { set; get; }
        /// <summary>
        /// 微博来源
        /// </summary>
        public string source { set; get; }
        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool favorited { set; get; }
        /// <summary>
        /// 是否被截断
        /// </summary>
        public bool truncated { set; get; }
        /// <summary>
        /// 回复ID 
        /// </summary>
        public string in_reply_to_status_id { set; get; }
        /// <summary>
        /// 回复人UID 
        /// </summary>
        public string in_reply_to_user_id { set; get; }
        /// <summary>
        /// 回复人昵称
        /// </summary>
        public string in_reply_to_screen_name { set; get; }
        /// <summary>
        /// 回复微博内容
        /// </summary>
        public string in_reply_to_status_text { set; get; }
        /// <summary>
        /// 转发的微博id
        /// </summary>
        public string root_in_reply_to_status_id { set; get; }
        /// <summary>
        /// 是否被转发
        /// </summary>
        public bool is_retweet_by_user { set; get; }
        /// <summary>
        /// 微博作者的用户信息字段
        /// </summary>
        public NeasyMUser user { set; get; }

    }
}