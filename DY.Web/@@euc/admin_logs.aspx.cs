/**
 * 功能描述：AdminLog管理类
 * 创建时间：2010-1-29 12:51:05
 * 最后修改时间：2010-1-29 12:51:05
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

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class admin_logs : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //列表
            if (this.act == "list")
            {
                //检测是否有浏览日志权限
                this.IsChecked("log_manager_list");

                //显示列表数据
                this.GetList();
            }

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("log_manager_del", true);

                //日志记录
                base.AddLog("删除日志：" + SiteBLL.GetAdminLogInfo(base.id).log_info);

                //执行删除
                SiteBLL.DeleteAdminLogInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 清空记录
            else if (this.act == "empty")
            {
                //检测权限
                this.IsChecked("log_manager_empty", true);

                //执行删除
                SiteBLL.DeleteAdminLogInfo("");

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
            if (DYRequest.getRequestInt("uid") > 0)
                filter += "user_id=" + DYRequest.getRequestInt("uid");

            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetAdminLogList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("log_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, "systems/log_list", base.isajax);
        }
    }
}
