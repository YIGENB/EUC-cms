using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Data;

using DY.Common;
using DY.Cache;
using DY.Data;
using DY.Entity;
using DY.LanguagePack;

namespace DY.Config
{
    /// <summary>
    /// URL资源文件初始化类
    /// </summary>
    public class UrlrewriteConfig
    {
        /// <summary>
        /// 读取资源文件配置信息
        /// </summary>
        /// <returns></returns>
        public static UrlrewriteConfigInfo Get()
        {
            UrlrewriteConfigInfo urlrewriteinfo = new UrlrewriteConfigInfo();
            urlrewriteinfo.article = urlrewrite.article;
            urlrewriteinfo.article_detail = urlrewrite.article_detail;
            urlrewriteinfo.download = urlrewrite.download;
            urlrewriteinfo.download_detail = urlrewrite.download_detail;
            urlrewriteinfo.page = urlrewrite.page;
            urlrewriteinfo.product = urlrewrite.product;
            urlrewriteinfo.product_detail = urlrewrite.product_detail;
            urlrewriteinfo.search = urlrewrite.search;
            urlrewriteinfo.html = urlrewrite.html;
            urlrewriteinfo.html_article = urlrewrite.html_article;
            urlrewriteinfo.html_article_detail = urlrewrite.html_article_detail;
            urlrewriteinfo.html_download = urlrewrite.html_download;

            urlrewriteinfo.html_download_detail = urlrewrite.html_download_detail;
            urlrewriteinfo.html_index = urlrewrite.html_index;
            urlrewriteinfo.html_page = urlrewrite.html_page;
            urlrewriteinfo.html_product = urlrewrite.html_product;
            urlrewriteinfo.html_product_detail = urlrewrite.html_product_detail;
            urlrewriteinfo.html_suffix = urlrewrite.html_suffix;
            urlrewriteinfo.http = urlrewrite.http;

            return urlrewriteinfo;
        }
    }
}
