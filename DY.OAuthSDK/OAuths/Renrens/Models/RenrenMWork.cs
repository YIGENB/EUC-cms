using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 实体类
    /// </summary>
    [Serializable]
    public class RenrenMWork
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public string time { set; get; }
        /// <summary>
        /// 行业
        /// </summary>
        public RenrenMIndustry industry { set; get; }
        /// <summary>
        /// 职位
        /// </summary>
        public RenrenMJob job { set; get; }
    }
    /// <summary>
    /// 行业
    /// </summary>
    [Serializable]
    public class RenrenMIndustry
    {
        /// <summary>
        /// 行业类别
        /// </summary>
        public string industryCategory { get; set; }
        /// <summary>
        /// 行业详情
        /// </summary>
        public string industryDetail { get; set; }
    }

    /// <summary>
    /// 职位
    /// </summary>
    [Serializable]
    public class RenrenMJob
    {
        public string jobCategory { get; set; }
        public string jobDetail { get; set; }
    }

}