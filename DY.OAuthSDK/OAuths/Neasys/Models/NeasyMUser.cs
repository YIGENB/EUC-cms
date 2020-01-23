using System;
namespace DY.OAuthV2SDK.OAuths.Neasys.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class NeasyMUser : NeasyMError
    {
        /// <summary>
        /// 用户UID 
        /// </summary>
        public long id { set; get; }
        /// <summary>
        /// 微博昵称 
        /// </summary>
        public string screen_name { set; get; }
        /// <summary>
        /// 友好显示名称，如Bill Gates
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 地址
        /// </summary>
        public string location { set; get; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string description { set; get; }

        /// <summary>
        /// 用户博客地址
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 自定义图像
        /// </summary>
        public string profile_image_url { set; get; }
        /// <summary>
        /// 性别，0为保密，1为男性，2为女性 
        /// </summary>
        public int gender { set; get; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public long followers_count { set; get; }
        /// <summary>
        /// 关注数
        /// </summary>
        public long friends_count { set; get; }
        /// <summary>
        /// 微博数
        /// </summary>
        public long statuses_count { set; get; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public long favourites_count { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string created_at { set; get; }
        /// <summary>
        /// 当前登录用户是否已关注该用户
        /// </summary>
        public bool following { set; get; }
        /// <summary>
        /// 当前登录用户是否黑名单该用户
        /// </summary>
        public bool blocking { set; get; }
        /// <summary>
        /// 加V标示，是否微博认证用户
        /// </summary>
        public bool verified { set; get; }
        /// <summary>
        /// 用户的最近一条微博信息字段
        /// </summary>
        public NeasyMStatus status { set; get; }
    }
}