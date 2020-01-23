using System;
namespace DY.OAuthV2SDK.OAuths.Toutiaos.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class ToutiaoMToken : ToutiaoMError
    {
        /// <summary>
        /// 访问令牌 
        /// </summary>
        public string access_token { set; get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { set; get; }
        /// <summary>
        /// 头条的用户标示
        /// </summary>
        public string uid { set; get; }
        /// <summary>
        /// 头条的用户类型
        /// </summary>
        public string uid_type { set; get; }
        /// <summary>
        /// 头条用户名
        /// </summary>
        public string screen_name { set; get; }
        /// <summary>
        /// 头条为合作伙伴分配的用户ID
        /// </summary>
        public string open_id { set; get; }
    }
}