using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 响应用户
    /// </summary>
    public class RenrenMRsUser : RenrenMError
    {
        /// <summary>
        /// 响应
        /// </summary>
        public RenrenMUser response { get; set; }
    }
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class RenrenMUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string id { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 用户头像列表
        /// </summary>
        public RenrenMImage[] avatar { set; get; }
        /// <summary>
        /// 表示是否为星级用户，值“1”表示“是”；值“0”表示“不是”
        /// </summary>
        public string star { set; get; }
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public RenrenMBasicInfo basicInformation { set; get; }
        /// <summary>
        /// 用户学校信息
        /// </summary>
        public RenrenMSchool[] education { set; get; }
        /// <summary>
        /// 工作信息
        /// </summary>
        public RenrenMWork[] work { get; set; }
        /// <summary>
        /// 喜欢
        /// </summary>
        public RenrenMLike[] like { get; set; }
        /// <summary>
        /// 感情状态
        /// </summary>
        public string emotionalState { get; set; }
    }

}