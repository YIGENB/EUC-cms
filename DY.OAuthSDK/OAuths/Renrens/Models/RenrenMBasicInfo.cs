using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 实体类
    /// </summary>
    [Serializable]
    public class RenrenMBasicInfo
    {
        /// <summary>
        /// 用户性别
        /// </summary>
        public string sex { set; get; }
        /// <summary>
        /// 用户生日，格式为'yyyy-mm-dd'或'y0后-mm-dd'
        /// </summary>
        public string birthday { set; get; }
        /// <summary>
        /// 家乡信息
        /// </summary>
        public RenrenMHomeTown homeTown { set; get; }
    }

    /// <summary>
    /// 性别的种类
    /// </summary>
    public enum RenrenMSex
    {
        /// <summary>
        /// 女性
        /// </summary>
        FEMALE,
        /// <summary>
        /// 男性
        /// </summary>
        MALE
    }

    /// <summary>
    /// 家乡
    /// </summary>
    [Serializable]
    public class RenrenMHomeTown
    {
        /// <summary>
        /// 所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string city { get; set; }
    }
}