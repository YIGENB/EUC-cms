using System;
namespace DY.OAuthV2SDK.OAuths.Tencs.Models
{
    /// <summary>
    /// 教育信息实体类
    /// </summary>
    [Serializable]
    public class TencMEdu 
    {
        /// <summary>
        /// 院系id
        /// </summary>
        public int departmentid { set; get; }

        /// <summary>
        /// 教育信息记录id
        /// </summary>
        public int id { set; get; }

        /// <summary>
        /// 学历级别
        /// </summary>
        public int level { set; get; }

        /// <summary>
        /// 学校id
        /// </summary>
        public int schoolid { set; get; }

        /// <summary>
        /// 入学年
        /// </summary>
        public int year { set; get; }

    }

}