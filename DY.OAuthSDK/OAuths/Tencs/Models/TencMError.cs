﻿using System;
namespace DY.OAuthV2SDK.OAuths.Tencs.Models
{
    /// <summary>
    /// 错误代码说明
    /// </summary>
    [Serializable]
    public class TencMError
    {
        /// <summary>
        /// 错误码
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
    }
}