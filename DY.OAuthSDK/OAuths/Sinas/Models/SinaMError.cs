using System;
using DY.OAuthV2SDK.OAuths.Sinas;
namespace DY.OAuthV2SDK.OAuths.Sinas.Models
{
    /// <summary>
    /// 错误代码说明
    /// </summary>
    [Serializable]
    public class SinaMError
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
        ///  可读的网页URI，带有关于错误的信息，用于为终端用户提供与错误有关的额外信息。
        /// </summary>
        public string error_url { set; get; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string request { set; get; }

        /// <summary>
        /// 错误的描述信息
        /// </summary>
        public string error_description
        {
            get
            {
                return SinaApiError.GetChinese(this.error_code.ToString());
            }
        }

    }
}