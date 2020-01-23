using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class RenrenMImage 
    {
        /// <summary>
        /// 图片的大小。
        /// </summary>
        public RenrenMImageSize size { set; get; }

        /// <summary>
        /// 图片的URL
        /// </summary>
        public string url { set; get; }
    }

    /// <summary>
    /// 图片大小的枚举。
    /// </summary>
    public enum RenrenMImageSize
    {
        /// <summary>
        /// 200pt x 600pt
        /// </summary>
        MAIN,
        /// <summary>
        /// 50pt x 50pt
        /// </summary>
        TINY,
        /// <summary>
        /// 720pt x 720pt
        /// </summary>
        LARGE,
        /// <summary>
        /// 100pt x 300pt
        /// </summary>
        HEAD
    }

}