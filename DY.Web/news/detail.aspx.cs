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
using DY.Cache;

namespace DY.Web
{
    public partial class detail : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            RollNewsDetailInfo entity = new RollNewsDetailInfo();
            ArrayList otherlist = new ArrayList();


            string site = DYRequest.getRequest("site").Replace("%2B", "+").Replace("%2F", "/").Replace("%3D", "=").Replace(" ","+");
            string thisurl = SiteUtils.IgetNumber(site); //AESEncrypt.Decode(site, BaseConfig.WebEncrypt);
            if (!string.IsNullOrEmpty(thisurl))
            {
                DYCache cache = DYCache.GetCacheService();
                CQ html = cache.RetrieveObject("/DY/RollNews/Detail/t" + DYRequest.getRequest("site")) as CQ;
                if (html == null)
                 {
                     html = CQ.CreateFromUrl(thisurl);
                     cache.AddObject("/DY/RollNews/Detail/t" + DYRequest.getRequest("site"), html);
                 }

                entity.title = html["h1"].Text();

                entity.pagekeywords = html["meta[name=\"keywords\"]"].Attr("content");
                entity.pagedesc = html["meta[name=\"description\"]"].Attr("content");


                entity.content = Server.HtmlDecode(System.Text.RegularExpressions.Regex.Replace(html["div[class='content fontsmall']"].Html(), @"<[a|A]\s*[^>]*>(.*?)</[a|A]>", "$1"));
                entity.content = entity.content.Replace("src=\"", string.Format("src=\"{0}", "/tools/ajax.aspx?act=CrackImg&imgurl="));


                entity.source = html["span[class=\"author\"]"].Text();
                entity.time = html["span[class=\"time\"]"].Text();


                CQ otherHtml = html[".relatedlist a"];

                foreach (var link in otherHtml)
                {
                    OtherNews other = new OtherNews();
                    other.name = link.Cq().Text();
                    //other.url = "/news/detail/" + AESEncrypt.Encode(link["href"], BaseConfig.WebEncrypt).Replace("+", "%2B").Replace("/", "%2F").Replace("=", "%3D") + config.UrlRewriterKzm;
                    other.url = "/news/detail/" + SiteUtils.IgetNumber(link["href"],"") + config.UrlRewriterKzm;
                    //other.time = link.Cq().Next("span").Text();
                    other.source = link.Cq().Children("p").Children("span.source").Text();
                    otherlist.Add(other);
                };



                context.Add("RollNewsDetailInfo", entity);
                context.Add("otherlist", otherlist);
            }
            context.Add("tongjiCode", "<script src='http://pw.cnzz.com/c.php?id=" + SiteUtils.ReadFileToCnzz().Split('@')[0] + "&l=2' language='JavaScript' charset='gb2312'></script>");
            base.DisplayTemplate(context, SiteUtils.IsMobileDevice() ? "mdetail" : "detail", "/static/template/news", false);
        }


        public class RollNewsDetailInfo
        {
            // Internal member variables
            public string title { get; set; }
            public string content { get; set; }
            public string time { get; set; }
            public string source { get; set; }

            public string pagekeywords { get; set; }
            public string pagedesc { get; set; }

        }

        public class OtherNews
        {
            public string url { get; set; }
            public string name { get; set; }
            public string time { get; set; }
            public string source { get; set; }

        }
    }
}
