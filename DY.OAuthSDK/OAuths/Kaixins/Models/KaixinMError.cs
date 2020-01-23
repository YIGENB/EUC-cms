using System;
namespace DY.OAuthV2SDK.OAuths.Kaixins.Models
{
    /// <summary>
    /// 错误代码说明
    /// </summary>
    [Serializable]
    public class KaixinMError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string error { set; get; }

        /// <summary>
        /// 错误的内部编号
        /// </summary>
        public int error_code { set; get; }

        /// <summary>
        /// 错误的错误信息
        /// </summary>
        public string message_code { set; get; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string request { set; get; }


    }
}