/**
 * 功能描述：InternalLinks管理类
 * 创建时间：2010-6-2 15:08:01
 * 最后修改时间：2010-6-2 15:08:01
 * 作者：gudufy
 * 文件标识：8612a35a-da59-4516-a373-ff77fa051c3c
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

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.Config;

namespace DY.Web.admin
{
    public partial class robots : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("robots_list");

                //显示列表数据
                this.GetList();
                //删除一周前数据
                //TimingDelete();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("robots_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteRobotsInfo("ID in (" + ids.Remove(ids.Length - 1, 1) + ")");
                        //日志记录
                        base.AddLog("删除蜘蛛记录");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 清空记录
            else if (this.act == "empty")
            {
                //检测权限
                this.IsChecked("robots_del", true);
                string sql = "truncate table " + DY.Config.BaseConfig.TablePrefix + "robots";

                SystemConfig.SqlProcess(sql);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("robots_del", true);

                //执行删除
                SiteBLL.DeleteRobotsInfo(base.id);
                //日志记录
                base.AddLog("删除蜘蛛记录");

                //显示列表数据
                this.GetList();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";

            this.GetList("robots/robots_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetRobotsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("ID desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            //context.Add("sort_by", DYRequest.getRequest("sort_by"));
            //context.Add("sort_order", DYRequest.getRequest("sort_order"));

            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }

        /// <summary>
        /// 定期删除15天前数据
        /// </summary>
        /// <param name="id"></param>
        protected void TimingDelete()
        {
            string sql = "delete from " + BaseConfig.TablePrefix + "robots where datediff(day,date,getdate())>15";
            SystemConfig.SqlProcess(sql);
        }

    }
}