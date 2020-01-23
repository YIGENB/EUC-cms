using System;
namespace DY.OAuthV2SDK.OAuths.Renrens.Models
{
    /// <summary>
    /// 实体类MUsers 。
    /// </summary>
    [Serializable]
    public class RenrenMSchool
    {
        /// <summary>
        /// 学校名称
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 入学日期
        /// </summary>
        public string year { set; get; }
        /// <summary>
        /// 学历
        /// </summary>
        public string educationBackground { set; get; }
        /// <summary>
        /// 所在专业
        /// </summary>
        public string department { set; get; }
    }

    /// <summary>
    /// 学历的类型
    /// </summary>
    public enum RenrenMEducationBg
    {
        /// <summary>
        /// 博士
        /// </summary>
        DOCTOR,
        /// <summary>
        /// 本科
        /// </summary>
        COLLEGE,
        /// <summary>
        /// 校工
        /// </summary>
        GVY,
        /// <summary>
        /// 小学
        /// </summary>
        PRIMARY,
        /// <summary>
        /// 其他
        /// </summary>
        OTHER,
        /// <summary>
        /// 教师
        /// </summary>
        TEACHER,
        /// <summary>
        /// 硕士
        /// </summary>
        MASTER,
        /// <summary>
        /// 高中
        /// </summary>
        HIGHSCHOOL,
        /// <summary>
        /// 中专技校
        /// </summary>
        TECHNICAL,
        /// <summary>
        /// 初中
        /// </summary>
        JUNIOR,
        /// <summary>
        /// 保密
        /// </summary>
        SECRET
    }

}