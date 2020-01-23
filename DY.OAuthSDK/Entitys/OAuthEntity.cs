using System;
using System.Collections.Generic;
using System.Text;

namespace DY.OAuthV2SDK.Entitys
{
    /// <summary>
    /// 协议实体
    /// </summary>
    [Serializable]
    public class OAuthEntity
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string desc { set; get; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string cnname { set; get; }

        /// <summary>
        /// 是否使用@
        /// </summary>
        public string isAt { set; get; }
    }

}