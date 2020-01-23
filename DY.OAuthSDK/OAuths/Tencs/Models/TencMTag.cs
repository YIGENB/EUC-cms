using System;
namespace DY.OAuthV2SDK.OAuths.Tencs.Models
{
    /// <summary>
    /// 标签信息实体类
    /// </summary>
    [Serializable]
    public class TencMTag 
    {
        /// <summary>
        /// 个人标签id
        /// </summary>
        public string id { set; get; }

        /// <summary>
        /// 标签名
        /// </summary>
        public string name { set; get; }

    }

}