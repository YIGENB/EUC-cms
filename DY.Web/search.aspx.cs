using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using CsQuery;
using DY.Cache;

namespace DY.Web
{
    public partial class search : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            string filter = "is_delete=0";
            string wd = HtmlToTxt(Server.HtmlEncode(DYRequest.getRequest("wd")));
            int time = DYRequest.getRequestInt("time");
            string url = "";
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Search/Word" + wd + "/Top" + pagesize) as ArrayList;
            if (!string.IsNullOrEmpty(wd))
            {
                //filter += " and (";
                //string segment = SiteUtils.Segment(wd);
                //if (segment.Split(',').Length > 0)
                //{
                //    for (int i = 0; i < segment.Split(',').Length; i++)
                //    {
                //        filter += " title like '%" + segment.Split(',')[i].Trim() + "%' or ";
                //        //filter += " Contains(title,'" + segment.Split(',')[i].Trim() + "') or ";
                //    }
                //}
                //filter += "1<>1";
                //filter += ")";
                filter += " and title like '%" + wd.Trim() + "%'";
                url = "/search/" + wd + "/";
                //标签库
                TagInfo taginfo = SiteBLL.GetTagInfo(string.Format("urlrewriter='{0}'", wd));
                if (taginfo != null)
                {
                    filter += " or (tag like '%" + taginfo.tag_name + "%')";
                    wd = taginfo.tag_name;
                }
                context.Add("word", GetWord(wd));
                context.Add("wordlist", GetWordBaidu(wd));
            }
            else if (time > 0)
            {
                if (time <= 604800)
                    filter += " and datediff(hour,date,getdate())<=" + (ToDate(ToDate(time)));
                else if (time > 604800 && time <= 15552000)
                    filter += " and datediff(day,date,getdate())<=" + (ToDate(ToDate(ToDate(time))));
                else
                    filter += " and datediff(month,date,getdate())<=" + (ToDate(ToDate(ToDate(time))) / 30);
                url = "/search-" + time + "/";
            }
            else
                return;
            if (data == null)
            {
                data = SiteBLL.GetSearchList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("date desc"), filter, out base.ResultCount);
            }

            cache.AddObject("/DY/Web/Search/Word" + wd + "/Top" + pagesize, data);

            context.Add("list", data);
            context.Add("pager", Utils.GetSearchPageNumbers(base.ResultCount, pagesize, base.pageindex, url, config.UrlRewriterKzm, 6));
            context.Add("keyword", wd);
            context.Add("countPage", (base.ResultCount - 1) / pagesize + 1);
            context.Add("pagesize", pagesize);
            context.Add("ResultCount", base.ResultCount);
            context.Add("tongjiCode", "<script src='http://pw.cnzz.com/c.php?id=" + SiteUtils.ReadFileToCnzz().Split('@')[0] + "&l=2' language='JavaScript' charset='gb2312'></script>");

            base.DisplayTemplate(context, SiteUtils.IsMobileDevice() ? "msearch" : "search", "static/template", false);
        }
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int ToDate(int date)
        {
            decimal mm = (decimal)((decimal)date / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        /// <summary>
        /// 获取单个搜索词的长尾作为相关搜索（词库）
        /// </summary>
        /// <param name="kw">关键词</param>
        /// <returns></returns>
        protected static string GetWord(string kw)
        {
            string pattern = string.Format("(?is){0}(.*?){1}", "rel=\"nofollow\">", "</a>");
            try
            {
                string word = SiteUtils.NoHTML(SiteUtils.GetWordMatch("http://ci.aizhan.com/" + kw + "/", pattern, 9));
                return !string.IsNullOrEmpty(word) ? word.Substring(0, word.LastIndexOf(',')) : "";
            }
            catch
            {
                return "0";
            }
        }
        /// <summary>
        /// 获取单个搜索词的长尾作为相关搜索（百度相关）
        /// </summary>
        /// <param name="kw">关键词</param>
        /// <returns></returns>
        protected static ArrayList GetWordBaidu(string kw)
        {
            ArrayList list = new ArrayList();
            Random r = new Random();
            DY.Cache.DYCache cache = DY.Cache.DYCache.GetCacheService();
            CQ html = cache.RetrieveObject("/DY/Baidu/Word/t" + kw) as CQ;
            if (html == null)
            {
                html = CQ.CreateFromUrl("https://www.so.com/s?q=" + kw);
                cache.AddObject("/DY/Baidu/Word/t" + kw, html);
            }

            CQ rsHtml = html["div[id=\"rs\"] a"];

            foreach (var link in rsHtml)
            {
                list.Add(link.Cq().Text());
            };

            #region 处理关键词展示
            if (kw.Contains("易优创"))
            {
                list[r.Next(0, list.Count)] = "易优创全网营销";
                list[r.Next(0, list.Count)] = "营销型网站建设";
                list[r.Next(0, list.Count)] = "全网营销";
                list[r.Next(0, list.Count)] = "易优创官网";
            }
            #endregion

            return list;
        }


        /// <summary>
        /// 对字符串进行检查和替换其中的特殊字符
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
                        @"<script[^>]*?>.*?</script>",
                        @"<(///s*)?!?((/w+:)?/w+)(/w+(/s*=?/s*(([""'])(//[""'tbnr]|[^/7])*?/7|/w+)|.{0})|/s)*?(///s*)?>",
                        @"([/r/n])[/s]+",
                        @"&(quot|#34);",
                        @"&(amp|#38);",
                        @"&(lt|#60);",
                        @"&(gt|#62);", 
                        @"&(nbsp|#160);", 
                        @"&(iexcl|#161);",
                        @"&(cent|#162);",
                        @"&(pound|#163);",
                        @"&(copy|#169);",
                        @"&#(/d+);",
                        @"-->",
                        @"<!--.*/n"
                        };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(aryReg[i], System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("/r/n", "");

            return strOutput;
        }
    }
}
