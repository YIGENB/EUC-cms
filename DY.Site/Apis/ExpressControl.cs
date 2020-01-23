using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DY.Site
{
    /// <summary>
    /// 快递控制器
    /// </summary>
    public class ExpressControl
    {

        /// <summary>
        /// 快递接口的种类，用户根据选择种类而得到类
        /// </summary>
        public enum ExpressType
        {
            /// <summary>
            /// 快递100
            /// </summary>
            Express100,
            /// <summary>
            /// 百度apistore快递接口http://apistore.baidu.com/apiworks/servicedetail/1727.html
            /// </summary>
            Expressjishu,
            /// <summary>
            /// 超级站长蜘蛛池提交入口http://zzuser.chaojirj.com/
            /// </summary>
            SubmitZhiZhuChi,
            /// <summary>
            /// 超级站长蜘蛛池积分查询http://zzuser.chaojirj.com/
            /// </summary>
            SelectZhiZhuChi,
            /// <summary>
            /// 腾讯滚动新闻接口
            /// </summary>
            RollNewsFromQQ
        }
        /// <summary>
        /// 通过传入的enum得到想要得到的类
        /// </summary>
        /// <param name="type">快递接口的种类</param>
        /// <returns></returns>
        public Expressabstract Get(ExpressType type)
        {
            Expressabstract fruitage = null;
            switch (type)
            {
                case ExpressType.Express100:
                    fruitage = new Express100();
                    break;
                case ExpressType.Expressjishu:
                    fruitage = new Expressjishu();
                    break;
                case ExpressType.SubmitZhiZhuChi:
                    fruitage = new SubmitZhiZhuChi();
                    break;
                case ExpressType.SelectZhiZhuChi:
                    fruitage = new SelectZhiZhuChi();
                    break;
                    case ExpressType.RollNewsFromQQ:
                    fruitage = new RollNewsFromQQ();
                    break;

            }
            return fruitage;

        }
    }
}
