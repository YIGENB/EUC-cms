using System;
namespace DY.OAuthV2SDK.OAuths.Qzones.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class QzoneMUser : QzoneMError
    {
        /// <summary>
        /// 昵称 
        /// </summary>
        public string nickname { set; get; }

        /// <summary>
        /// 自定义图像
        /// </summary>
        public string figureurl { set; get; }

        /// <summary>
        /// 自定义图像1
        /// </summary>
        public string figureurl_1 { set; get; }

        /// <summary>
        /// 自定义图像2
        /// </summary>
        public string figureurl_2 { set; get; }

        /// <summary>
        /// 性别, 男，女
        /// </summary>
        public string gender { set; get; }
        /// <summary>
        /// vip[
        /// </summary>
        public string vip { set; get; }
        /// <summary>
        /// 等级
        /// </summary>
        public string level { set; get; }

    }

}