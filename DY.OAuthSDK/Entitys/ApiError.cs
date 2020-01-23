using System;
using System.Collections.Generic;
using System.Text;

namespace DY.OAuthV2SDK.Entitys
{
    /// <summary>
    /// 错误代码说明
    /// </summary>
    [Serializable]
    public class ApiError
    {
        /// <summary>
        /// 错误码(无错误时为0)
        /// </summary>
        public int ret { set; get; }

        /// <summary>
        /// 含义说明
        /// </summary>
        public string msg { set; get; }

        /// <summary>
        /// 返回错误码
        /// </summary>
        public string errcode { set; get; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string request { set; get; }
    }
}