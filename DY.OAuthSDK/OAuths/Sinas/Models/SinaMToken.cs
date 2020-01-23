using System;
namespace DY.OAuthV2SDK.OAuths.Sinas.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class SinaMToken : SinaMError
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
        /// 刷新令牌
        /// </summary>
        public string refresh_token { set; get; }
    }
}