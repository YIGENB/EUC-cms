using System;
namespace DY.OAuthV2SDK.OAuths.Toutiaos.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class ToutiaoMUser : ToutiaoMError
    {
        /// <summary>
        /// 头条分配给第三方的用户ID 
        /// </summary>
        public string open_id { set; get; }

        /// <summary>
        /// 头条的用户标示
        /// </summary>
        public string uid { set; get; }

        /// <summary>
        /// 头条的用户类型
        /// </summary>
        public string uid_type { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string screen_name { set; get; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string avatar_url { set; get; }
        /// <summary>
        /// 是否加V
        /// </summary>
        public string user_verified { set; get; }
        /// <summary>
        /// 用户描述
        /// </summary>
        public string description { set; get; }
        /// <summary>
        /// 用户的手机号
        /// </summary>
        public string mobile { set; get; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { set; get; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string age { set; get; }
        /// <summary>
        /// 用户兴趣特征
        /// </summary>
        public string category { set; get; }

    }

}