/**
 * 功能描述：main
 * 创建时间：2010-1-29 12:43:46
 * 最后修改时间：2010-1-29 12:43:46
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Text;

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.Config;

namespace DY.Web.admin
{
    public partial class main : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DYRequest.getRequest("act") == "get_tip")
            {
                base.DisplayMemoryTemplate("{message:{wck:" + SiteBLL.GetFeedbackAllList("", "is_show=0").Count + ",whf:" + SiteBLL.GetFeedbackAllList("", "(msg_id NOT IN (SELECT parent_id FROM " + BaseConfig.TablePrefix + "feedback WHERE parent_id != 0) and parent_id=0)").Count + "},users:{wsh:" + SiteBLL.GetUsersAllList("", "is_enabled=0").Count + "},review:{wck:" + SiteBLL.GetCommentAllList("", "is_read=0").Count + ",whf:0},email:{wsh:" + SiteBLL.GetEmailListAllList("", "stat=0").Count + "}}");
            }
            else
            {
                IDictionary context = new Hashtable();
                StringBuilder sb = new StringBuilder();
                StringBuilder order_data = new StringBuilder();
                StringBuilder user_data = new StringBuilder();
                //StringBuilder weixin_user_data = new StringBuilder();
                int d = DYRequest.getRequestInt("d", 15);
                base.pid = GetWeixinUserFirst();
                for (int i = d-1; i >= 0; i--)
                {
                    if (DateTime.Now.AddDays(-i).Day == DateTime.Now.Day)
                    {
                        sb.Append("'今天',");
                        order_data.Append("" + this.GetOrderCount(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ",");
                        user_data.Append("" + this.GetUserCount(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ",");
                        //weixin_user_data.Append("" + this.GetWeixinUserCount(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ",");
                    }
                    else
                    {
                        sb.Append("'" + DateTime.Now.AddDays(-i).ToString("M月d") + "',");
                        order_data.Append("" + this.GetOrderCount(DateTime.Now.AddDays(-i).Year, DateTime.Now.AddDays(-i).Month, DateTime.Now.AddDays(-i).Day) + ",");
                        user_data.Append("" + this.GetUserCount(DateTime.Now.AddDays(-i).Year, DateTime.Now.AddDays(-i).Month, DateTime.Now.AddDays(-i).Day) + ",");
                        //weixin_user_data.Append("" + this.GetWeixinUserCount(DateTime.Now.AddDays(-i).Year, DateTime.Now.AddDays(-i).Month, DateTime.Now.AddDays(-i).Day) + ",");
                    }
                }

                context.Add("file_authority", FileOperate.get_file_authority());
                context.Add("days", sb.Remove(sb.Length - 1, 1));
                context.Add("order_datas", order_data.Remove(order_data.Length - 1, 1));
                context.Add("user_data", user_data.Remove(user_data.Length - 1, 1));
                //context.Add("weixin_user_data", weixin_user_data.Remove(weixin_user_data.Length - 1, 1));
                context.Add("d", d);
                context.Add("goods_count", SiteBLL.GetGoodsAllList("", "is_delete=0").Count);
                context.Add("cms_count", SiteBLL.GetCmsAllList("", "").Count);
                context.Add("siteTime", SiteUtils.DateDiff2(DateTime.Now, Convert.ToDateTime(ReadFile().Split('@')[0])).Days);
                context.Add("siteSize", ReadFile().Split('@')[1]);
                //活跃用户
                context.Add("user_ActiveCount", GetActiveCount(30));
                context.Add("thisMonth", DateTime.Now.Month);

                base.DisplayTemplate(context, "main");
            }
        }

        /// <summary>
        /// 取得指定日期制定公众号的关注人数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetWeixinUserCount(int year, int month, int day)
        {
            object obj = SiteBLL.GetWeixinUserValue("COUNT(user_id)", "YEAR(subscribe_time)=" + year + " and MONTH(subscribe_time)=" + month + " and DAY(subscribe_time)=" + day + "");

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 取得多公众号默认第一个
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetWeixinUserFirst()
        {
            object obj = SiteBLL.GetWeixinValue("max(mp_id)", "");

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 取得指定日期的订单数量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetOrderCount(int year, int month, int day)
        {
            object obj = SiteBLL.GetOrderInfoValue("COUNT(order_id)", "id_delete=0 and YEAR(add_time)=" + year + " and MONTH(add_time)=" + month + " and DAY(add_time)=" + day + "");

            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 取得指定日期的注册会员数量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetUserCount(int year, int month, int day)
        {
            object obj = SiteBLL.GetUsersValue("COUNT(user_id)", "YEAR(reg_time)=" + year + " and MONTH(reg_time)=" + month + " and DAY(reg_time)=" + day + "");

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 取得最近一个月活跃用户数量
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetActiveCount(int day)
        {
            object obj = SiteBLL.GetUsersValue("COUNT(user_id)", "datediff(DAY,last_login,getdate())<=" + day);

            return Convert.ToInt32(obj);
        }

        #region 读取网站信息
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        protected string ReadFile()
        {
            string str = "";
            string path = System.Web.HttpContext.Current.Server.MapPath("/site/siteinfo.txt");
            if (FileOperate.IsExist(path, DY.Common.FileOperate.FsoMethod.File))
                str = FileOperate.ReadFile(path);
            return str;
        }
        #endregion
    }
}
