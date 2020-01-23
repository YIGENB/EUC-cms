using System;
namespace DY.OAuthV2SDK.OAuths.Sinas.Models
{
    /// <summary>
    /// 实体类MPicUrl 。
    /// </summary>
    [Serializable]
    public class SinaMPicUrl : SinaMError
    {
        /// <summary>
        /// 缩略图
        /// </summary>
        public string thumbnail_pic { set; get; }
    }
}