using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class download_detail : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");
            string template = "download-details";
            DownloadInfo downloadinfo = SiteBLL.GetDownloadInfo("urlrewriter='" + code + "'");
            if (downloadinfo == null)
                downloadinfo = SiteBLL.GetDownloadInfo(Utils.StrToInt(code, 0));

            if (downloadinfo!=null)
            {
                if (string.IsNullOrEmpty(downloadinfo.title))
                {
                    pagetitle = config.Title;
                }
                else
                {
                    pagetitle = downloadinfo.title;
                }

                if (string.IsNullOrEmpty(downloadinfo.seokeyword))
                {
                    pagekeywords =config.Keywords;
                }
                else
                {
                    pagekeywords = downloadinfo.seokeyword;
                }

                if (string.IsNullOrEmpty(downloadinfo.seodesc))
                {
                    pagedesc =config.Desc;
                }
                else
                {
                    pagedesc = downloadinfo.seodesc;
                }

                //获取下载分类详细页模板
                DownloadCategoryInfo catinfo = SiteBLL.GetDownloadCategoryInfo(string.Format("cat_id={0}", Convert.ToInt32(downloadinfo.cat_id)));
                if (catinfo != null)
                {
                    string cms_template_detail = catinfo.template_detail_path.Trim().ToString();
                    if (!string.IsNullOrEmpty(cms_template_detail))
                    {
                        template = cms_template_detail;
                    }
                }

                if (string.IsNullOrEmpty(downloadinfo.template_file)==false) {
                    template = downloadinfo.template_file;
                }
                context.Add("downloadcatinfo", catinfo);
                context.Add("downloadinfo", downloadinfo);
                int total_reco =Convert.ToInt32(downloadinfo.is_reco + downloadinfo.no_reco);
                if (total_reco == 0)
                {
                    context.Add("no_reco_hot", "0");
                    context.Add("is_reco_hot", "0");
                }
                else
                {

                    context.Add("no_reco_hot", Convert.ToInt32(downloadinfo.no_reco * 100 / total_reco).ToString());
                    context.Add("is_reco_hot", Convert.ToInt32((downloadinfo.is_reco * 100 / total_reco)).ToString());
                }
                context.Add("comment_type", 2);
                context.Add("id_value", downloadinfo.down_id);

                //更新访问统计
                SiteBLL.UpdateDownloadFieldValue("click_count", Convert.ToInt32(downloadinfo.click_count) + 1, Convert.ToInt32(downloadinfo.down_id));

             
                base.DisplayTemplate(context, template);
            }
        }
    }
}
