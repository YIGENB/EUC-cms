using System;
namespace DY.OAuthV2SDK.OAuths.Qzones.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class QzoneMToken : QzoneMError
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
        /// 个人描述
        /// </summary>
        public string refresh_token { set; get; }
        /// <summary>
        /// 用户的ID，与QQ号码一一对应。
        /// </summary>
        public string openid { set; get; }
    }
}