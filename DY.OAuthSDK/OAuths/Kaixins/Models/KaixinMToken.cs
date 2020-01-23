using System;
namespace DY.OAuthV2SDK.OAuths.Kaixins.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class KaixinMToken : KaixinMError
    {
        /// <summary>
        /// 访问令牌 
        /// </summary>
        public string access_token { set; get; }
        /// <summary>
        /// 访问令牌 
        /// </summary>
        public string oauth_token { set; get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { set; get; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string refresh_token { set; get; }
    }
}