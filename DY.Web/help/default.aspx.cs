/**
 * 功能描述：资讯首页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using CShop.Common;
using CShop.Site;
using CShop.Entity;

namespace CShop.Web.help
{
    public partial class index : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            string code = CShopRequest.getRequest("code");
            string filter = "article_id > 0";
            string tlp = "help-list";
            string catltp = "";
            if (string.IsNullOrEmpty(code))
            {
                code = "help";
            }

            CmsCatInfo catinfo = SiteBLL.GetCmsCatInfo(string.Format("urlrewriter='{0}'", code));
            if (catinfo == null)
            {
                catinfo = SiteBLL.GetCmsCatInfo(string.Format("cat_id={0}", Utils.StrToInt(code, 0)));
            }
            if (catinfo != null)
            {
                catltp += "<a href='/cms/" + catinfo.cat_id.ToString() + ".html'>" + catinfo.cat_name + "</a>";
                #region 分页的页码控制
                if (catinfo.page_size != null && catinfo.page_size.Value != 0)
                {
                    pagesize = catinfo.page_size.Value;
                }
                else
                {
                    pagesize = config.PageSize;
                }
                #endregion

                #region seo
                if (string.IsNullOrEmpty(catinfo.pagetitle))
                {
                    pagetitle = catinfo.cat_name + "-" + config.Title;
                }
                else
                {
                    pagetitle = catinfo.pagetitle;
                }

                if (string.IsNullOrEmpty(catinfo.pagekeywords))
                {
                    pagekeywords = config.Keywords;
                }
                else
                {
                    pagekeywords = catinfo.pagekeywords;
                }

                if (string.IsNullOrEmpty(catinfo.pagedesc))
                {
                    pagedesc = config.Desc;
                }
                else
                {
                    pagedesc = catinfo.pagedesc;
                }
                #endregion
                if (!string.IsNullOrEmpty(catinfo.list_tlp))
                { tlp = catinfo.list_tlp; }
            }

            if (catinfo != null)
            {
                filter = "cat_id in (" + cms.GetCMSCatIds(catinfo.cat_id.Value) + ")";
                context.Add("cmscatinfo", catinfo);
                context.Add("list", SiteBLL.GetCmsList(base.pageindex, pagesize, "article_id,cat_id,title,des,photo,showtime,urlrewriter", SiteUtils.GetSortOrder("is_top desc,article_id desc"), filter, out base.ResultCount));
                context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, pagesize, base.pageindex, "/cms/" + code + "/", ".html", 6));
                context.Add("catltp", catltp);
                base.DisplayTemplate(context, tlp);
            }
        }
    }
}
