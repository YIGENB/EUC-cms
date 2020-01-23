/**
 * 功能描述：资讯首页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
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

namespace DY.Web
{
    public partial class cms_search : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filter = "article_id > 0";
            string k = DYRequest.getRequest("k");

            if (!string.IsNullOrEmpty(k))
                filter += " and (title like '%" + k + "%' or tag like '%" + k + "%' or des  like '%" + k + "%')";
            
            IDictionary context = new Hashtable();

            context.Add("list", SiteBLL.GetCmsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("article_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetStaticPageNumbers(base.pageindex, base.ResultCount, base.pagesize, Request.Url.PathAndQuery, "", 5, false));
            context.Add("keyword", k);
            context.Add("ResultCount", base.ResultCount);

            base.DisplayTemplate(context, "cms-search");
        }
    }
}
