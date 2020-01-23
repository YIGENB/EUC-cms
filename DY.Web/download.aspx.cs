using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.LanguagePack;

namespace DY.Web
{
    public partial class download : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");
            string filter = "is_enable=1";
            string template = "download";
            string catnav = "&raquo; 下载中心";
            string cat_name = "下载中心";

            DownloadCategoryInfo download = SiteBLL.GetDownloadCategoryInfo(string.Format("urlrewriter='{0}'", code));
            if (download == null)
            {
                download = SiteBLL.GetDownloadCategoryInfo(string.Format("cat_id={0}", Utils.StrToInt(code, 0)));
            }

            if (download != null)
            {
                if (!string.IsNullOrEmpty(download.cat_name))
                {
                    pagetitle = download.cat_name;
                }
                else
                {
                    pagetitle = config.Title;
                }

                if (!string.IsNullOrEmpty(download.keywords))
                {
                    pagekeywords = download.keywords;
                }
                else
                {
                    pagekeywords = config.Keywords;
                }

                if (!string.IsNullOrEmpty(download.cat_desc))
                {
                    pagedesc = download.cat_desc;
                }
                else
                {
                    pagedesc = config.Desc;
                }

                filter = "cat_id in (" + down.GetDownloadCatIds(download.cat_id.Value) + ")";

                if (!string.IsNullOrEmpty(download.template_file))
                {
                    template = download.template_file;
                }
                if (download.pagesize > 0)
                {
                    pagesize = Convert.ToInt32(download.pagesize);
                }

                int catid = Caches.DownloadCatID(download.parent_id.Value, download.cat_id.Value);//catinfo.parent_id > 0 ? catinfo.parent_id.Value : catinfo.cat_id.Value;

                cat_name = SiteBLL.GetDownloadCategoryValue("cat_name", "cat_id=" + catid).ToString();

                context.Add("downloadcatinfo", download);
                context.Add("this_id", download.cat_id.Value);
                catnav = Caches.DownLoadNav(download.cat_id.Value, "");
            }

            context.Add("catnav", catnav);
            context.Add("list", SiteBLL.GetDownloadList(base.pageindex, pagesize, SiteUtils.GetSortOrder("orderid desc,cat_id desc"), filter, out base.ResultCount));

            string url = string.IsNullOrEmpty(code) ? urlrewrite.download + "p" : urlrewrite.download+ code + "/p";
            context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, pagesize, base.pageindex, url, config.UrlRewriterKzm, 6));

            context.Add("cat_name", cat_name);

            base.DisplayTemplate(context, template);
        }
    }
}
