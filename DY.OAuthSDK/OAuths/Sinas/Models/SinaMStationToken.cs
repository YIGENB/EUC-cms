using System;
namespace DY.OAuthV2SDK.OAuths.Sinas.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class SinaMStationToken : SinaMError
    {
        /// <summary>
        /// 用户
        /// </summary>
        public SinaMUser user { set; get; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string algorithm { set; get; }
        /// <summary>
        /// 发行时间
        /// </summary>
        public int issued_at { set; get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires { set; get; }
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string oauth_token { set; get; }
        /// <summary>
        /// 用户id
        /// </summary>
        public long user_id { set; get; }
        /// <summary>
        /// 来源
        /// </summary>
        public string referer { set; get; }
    }
}