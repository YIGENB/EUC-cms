using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 错误代码说明
    /// </summary>
    [Serializable]
    public class RenrenMError
    {
        /// <summary>
        /// 含义说明
        /// </summary>
        public RenrenMSubError error { set; get; }
    }
    /// <summary>
    /// 错误代码说明
    /// </summary>
    public class RenrenMSubError
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string code { get; set; }
    }
}