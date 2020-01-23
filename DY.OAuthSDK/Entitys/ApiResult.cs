using System;
using System.Collections.Generic;
using System.Text;

namespace DY.OAuthV2SDK.Entitys
{
    /// <summary>
    /// 错误代码说明
    /// <para>data: 当前的返回数据</para>
    /// </summary>
    [Serializable]
    public class ApiResult : ApiError
    {

        /// <summary>
        /// 当前数据
        /// </summary>
        public string data { set; get; }

        /// <summary>
        /// api返回内容
        /// </summary>
        public string response { get; set; }
    }

}