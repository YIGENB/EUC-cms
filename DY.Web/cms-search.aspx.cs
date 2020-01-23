using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class cms_search1 : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filter = "article_id > 0";
            string k = Server.HtmlEncode(DYRequest.getRequest("k"));

            if (!string.IsNullOrEmpty(k))
                filter += " and (title like '%" + k + "%' or tag like '%" + k + "%' or des  like '%" + k + "%')";

            IDictionary context = new Hashtable();

            context.Add("list", SiteBLL.GetCmsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("article_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, pagesize, base.pageindex, "/article/search/" + k + "/p", config.UrlRewriterKzm, 6));
            context.Add("keyword", k);
            context.Add("countPage", (base.ResultCount - 1) / pagesize + 1);
            context.Add("pagesize", pagesize);
            context.Add("ResultCount", base.ResultCount);

            base.DisplayTemplate(context, "cms-search");
        }
    }
}
