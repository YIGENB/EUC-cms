using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 响应状态
    /// </summary>
    public class RenrenMRsStatus : RenrenMError
    {
        /// <summary>
        /// 响应
        /// </summary>
        public RenrenMStatus response { get; set; }
    }
    /// <summary>
    /// 响应状态
    /// </summary>
    public class RenrenMRsStatusList : RenrenMError
    {
        /// <summary>
        /// 响应
        /// </summary>
        public RenrenMStatus[] response { get; set; }
    }
    /// <summary>
    /// 状态实体类
    /// </summary>
    [Serializable]
    public class RenrenMStatus
    {
        /// <summary>
        /// 状态的ID
        /// </summary>
        public long id { set; get; }
        /// <summary>
        /// 状态所有者的用户ID
        /// </summary>
        public string ownerId { set; get; }
        /// <summary>
        /// 状态的内容，未处理过ubb的状态原文本
        /// </summary>
        public string content { set; get; }
        /// <summary>
        /// 状态发布的时间
        /// </summary>
        public string createTime { set; get; }
        /// <summary>
        /// 状态被转发的次数
        /// </summary>
        public int shareCount { set; get; }
        /// <summary>
        /// 状态被回复的次数
        /// </summary>
        public int commentCount { set; get; }
        /// <summary>
        /// 被分享状态的状态ID（分享的状态才会有）
        /// </summary>
        public long sharedStatusId { set; get; }
        /// <summary>
        /// 被分享的根状态的用户ID（分享的状态才会有）
        /// </summary>
        public long sharedUserId { set; get; }

    }
}