using System;
namespace DY.OAuthV2SDK.OAuths.Kaixins.Models
{
    /// <summary>
    /// 实体类
    /// </summary>
    [Serializable]
    public class KaixinMUser : KaixinMError
    {
        /// <summary>
        /// 用户UID 
        /// </summary>
        public string uid { set; get; }
        /// <summary>
        /// 用户名 
        /// </summary>
        public string name { set; get; }
        /// <summary>
        /// 性别 0:男 1:女
        /// </summary>
        public string gender { set; get; }
        /// <summary>
        /// 家乡
        /// </summary>
        public string hometown { set; get; }
        /// <summary>
        /// 现居住地
        /// </summary>
        public string city { set; get; }
        /// <summary>
        /// 状态 0:其它 1:学生 2:已工作
        /// </summary>
        public string status { set; get; }
        /// <summary>
        /// 头像120 x 120
        /// </summary>
        public string logo120 { set; get; }
        /// <summary>
        /// 头像50 x 50
        /// </summary>
        public string logo50 { set; get; }
        /// <summary>
        /// 生日
        /// </summary>
        public string birthday { set; get; }
        /// <summary>
        /// 体型 0:保密 1:苗条 2:匀称 3:健壮 4:高大 5:小巧 6:丰满 7:高挑 8:较胖 9:较瘦 10:运动型
        /// </summary>
        public string bodyform { set; get; }
        /// <summary>
        /// 血型 0:没有填写 1:O型血 2:A型血 3:B型血 4:AB型血 5:稀有血型
        /// </summary>
        public string blood { set; get; }
        /// <summary>
        /// 婚姻状态 0:没有填写 1:单身 2:恋爱中 3:订婚 4:已婚 5:离异
        /// </summary>
        public string marriage { set; get; }
        /// <summary>
        /// 希望结交
        /// </summary>
        public string trainwith { set; get; }
        /// <summary>
        /// 兴趣爱好
        /// </summary>
        public string interest { set; get; }
        /// <summary>
        /// 喜欢的书籍
        /// </summary>
        public string favbook { set; get; }
        /// <summary>
        /// 喜欢的电影
        /// </summary>
        public string favmovie { set; get; }
        /// <summary>
        /// 喜欢的电视剧
        /// </summary>
        public string favtv { set; get; }
        /// <summary>
        /// 偶像
        /// </summary>
        public string idol { set; get; }
        /// <summary>
        /// 座右铭
        /// </summary>
        public string motto { set; get; }
        /// <summary>
        /// 愿望列表
        /// </summary>
        public string wishlist { set; get; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string intro { set; get; }
        /// <summary>
        /// 教育经历
        /// </summary>
        public KaixinMEeducation[] education { set; get; }
        /// <summary>
        /// 工作经历
        /// </summary>
        public KaixinMCareer[] career { set; get; }       
        /// <summary>
        /// 是否公共主页 0:否 1:是
        /// </summary>
        public string isStar { set; get; }
        /// <summary>
        /// 用户的姓名拼音
        /// </summary>
        public string pinyin { set; get; }
        /// <summary>
        /// 用户是否在线 0:不在线 1:在线
        /// </summary>
        public string online { set; get; }
    }
    /// <summary>
    /// 教育经历
    /// </summary>
    public class KaixinMEeducation
    {
        /// <summary>
        /// 学校类型
        /// </summary>
        public string schooltype { set; get; }
        /// <summary>
        /// 学校
        /// </summary>
        public string school { set; get; }
        /// <summary>
        /// 班级
        /// </summary>
        public string Class { set; get; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public string year { set; get; }
    }

    public class KaixinMCareer
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string company { set; get; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public string dept { set; get; }
        /// <summary>
        /// 当前工作开始年份
        /// </summary>
        public string beginyear { set; get; }
        /// <summary>
        /// 当前工作开始月份
        /// </summary>
        public string beginmonth { set; get; }
        /// <summary>
        /// 当前工作结束年份
        /// </summary>
        public string endyear { set; get; }
        /// <summary>
        /// 当前工作结束月份
        /// </summary>
        public string endmonth { set; get; }
    }
}