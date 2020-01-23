using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class RenrenMLike
    {
        /// <summary>
        /// 表示用户id
        /// </summary>
        public string catagory { set; get; }
        /// <summary>
        /// 表示用户名
        /// </summary>
        public string name { set; get; }

    }
    /// <summary>
    /// 兴趣的类型
    /// </summary>
    public enum RenrenMLikeCatagory
    {
        /// <summary>
        /// 运动
        /// </summary>
        SPORT,
        /// <summary>
        /// 电影
        /// </summary>
        MOVIE,
        /// <summary>
        /// 动漫
        /// </summary>
        CARTOON,
        /// <summary>
        /// 游戏
        /// </summary>
        GAME,
        /// <summary>
        /// 音乐
        /// </summary>
        MUSIC,
        /// <summary>
        /// 书籍
        /// </summary>
        BOOK,
        /// <summary>
        /// 爱好
        /// </summary>
        INTEREST
    }

    /// <summary>
    /// 感情状态
    /// </summary>
    public enum RenrenMEmotionalState
    {
        /// <summary>
        /// 恋爱中
        /// </summary>
        INLOVE,
        /// <summary>
        /// 其他
        /// </summary>
        OTHER,
        /// <summary>
        /// 单身
        /// </summary>
        SINGLE,
        /// <summary>
        /// 已婚
        /// </summary>
        MARRIED,
        /// <summary>
        /// 暗恋
        /// </summary>
        UNOBVIOUSLOVE,
        /// <summary>
        /// 离异
        /// </summary>
        DIVORCE,
        /// <summary>
        /// 订婚
        /// </summary>
        ENGAGE,
        /// <summary>
        /// 失恋
        /// </summary>
        OUTLOVE,
    }
}