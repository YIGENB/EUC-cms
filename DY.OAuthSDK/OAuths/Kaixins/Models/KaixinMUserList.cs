using System;
using System.Collections.Generic;
namespace DY.OAuthV2SDK.OAuths.Kaixins.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class KaixinMUserList : KaixinMError
    {
        /// <summary>
        /// 用户列表 
        /// </summary>
        public KaixinMUser[] users { set; get; }
    }
}