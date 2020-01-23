using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class RenrenMToken
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
        /// <summary>
        /// 错误的内部编码
        /// </summary>
        public int error_code { get; set; }
        /// <summary>
        /// 错误码，有关错误码的详细信息请浏览错误码；
        /// </summary>
        public string error { get; set; }
        /// <summary>
        /// 一段人类可读的文字，用来帮助理解和解决发生的错误；
        /// </summary>
        public string error_description { get; set; }
        /// <summary>
        /// 一个人类可读的网页URI，带有关于错误的信息，用来为终端用户提供与错误有关的额外信息。
        /// </summary>
        public string error_uri { get; set; }
    }
}