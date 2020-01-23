/**
 * 功能描述：首页
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
using CsQuery;
using DY.Config;

namespace DY.Web.news
{
    public partial class index : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            ArrayList list = new ArrayList();
            string site = new SiteUtils().DefaultValue(DYRequest.getRequest("site"), "news");
            string page = new SiteUtils().DefaultValue(DYRequest.getRequest("page"), "1");
            Rollroot rollnews = SiteUtils.RollNewsFromQQ(site, "", page);

            //CQ html = CQ.CreateDocument("http://www.eyouc.com/apistore/rollnews/" + site + "/" + page + ""+config.UrlRewriterKzm);
            CQ html = CQ.CreateDocument(rollnews.data.article_info);
            foreach (var item in html["li"])
            {
                RollNewsInfo entity = new RollNewsInfo();
                //entity.href = !string.IsNullOrEmpty(item.Cq().Children("a").Attr("href")) ? "/news/detail/" + AESEncrypt.Encode(item.Cq().Children("a").Attr("href"), BaseConfig.WebEncrypt).Replace("+", "%2B").Replace("/", "%2F").Replace("=", "%3D") + config.UrlRewriterKzm : "";
                entity.href = "/news/detail/" + SiteUtils.IgetNumber(item.Cq().Children("a").Attr("href"),"") + config.UrlRewriterKzm;
                entity.name = item.Cq().Children("a").Text();
                entity.time = item.Cq().Children(".t-time").Text();
                entity.catename = item.Cq().Children(".t-tit").Text();
                list.Add(entity);
            }


            context.Add("list", list);
            context.Add("thispage", rollnews.data.page);
            context.Add("count", rollnews.data.count);
            context.Add("site", site);
            context.Add("tongjiCode", "<script src='http://pw.cnzz.com/c.php?id=" + SiteUtils.ReadFileToCnzz().Split('@')[0] + "&l=2' language='JavaScript' charset='gb2312'></script>");
            base.DisplayTemplate(context, SiteUtils.IsMobileDevice() ? "mindex" : "index", "/static/template/news", false);
        }

        public class RollNewsInfo
        {
            // Internal member variables
            public string href { get; set; }
            public string name { get; set; }
            public string time { get; set; }
            public string catename { get; set; }
        }
    }
}
