using System;
namespace DY.OAuthV2SDK.OAuths.Kaixins.Models
{
    /// <summary>
    /// 实体类MStatuses 。
    /// </summary>
    [Serializable]
    public class KaixinMRecord : KaixinMError
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string rid { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string ctime { set; get; }
        /// <summary>
        /// 内容信息
        /// </summary>
        public KaixinMMain main { set; get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public KaixinMUser user { set; get; }

        /// <summary>
        /// 原记录
        /// </summary>
        public object source { set; get; }
        /// <summary>
        /// 转发数
        /// </summary>
        public string rnum { set; get; }
        /// <summary>
        /// 评论数
        /// </summary>
        public string cnum { set; get; }
        /// <summary>
        /// 评论数
        /// </summary>
        public string znum { set; get; }
        public string from { set; get; }
        public KaixinMLocation location { set; get; }
    }
    /// <summary>
    /// 内容信息
    /// </summary>
    public class KaixinMMain
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string content { set; get; }
        /// <summary>
        /// 视频内容
        /// </summary>
        public string[] videos { set; get; }
        /// <summary>
        /// 视频swf地址
        /// </summary>
        public string swf { set; get; }
        /// <summary>
        /// 视频连接 
        /// </summary>
        public string link { set; get; }
        /// <summary>
        /// 视频图片地址
        /// </summary>
        public string img { set; get; }
        /// <summary>
        /// 图片信息
        /// </summary>
        public string[] pics { set; get; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string src { set; get; }
    }
    public class KaixinMLocation
    {
        public string location { set; get; }
        public string lat { set; get; }
        public string lon { set; get; }
    }
}