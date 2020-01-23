using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Config;
using DY.Entity;
using DY.Data;
using DY.Cache;

namespace DY.Site
{
    /// <summary>
    /// 页面操作类
    /// </summary>
    public partial class Caches
    {
        string content = SiteUtils.IsMobileDevice() ? "mobile_content" : "content"; //手机访问调取

        #region 页面内容
        /// <summary>
        /// 取得单个页面内容
        /// </summary>
        /// <returns></returns>
        public object Content(string urlrewriter)
        {
            return SiteBLL.GetCmsPageValue(content, "urlrewriter='" + urlrewriter + "'");
        }
        /// <summary>
        /// 取得单个页面字段
        /// </summary>
        /// <returns></returns>
        public object Content(string content, string urlrewriter)
        {
            return SiteBLL.GetCmsPageValue(content, "urlrewriter='" + urlrewriter + "'");
        }
        #endregion

        #region 取得指定页面下的页面
        /// <summary>
        /// 取得指定页面下的页面
        /// </summary>
        /// <returns></returns>
        public ArrayList AllPage(int page_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Page" + page_id) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetCmsPageAllList("order_id desc", "is_show=1 and parent_id=" + page_id);
                cache.AddObject("/DY/Web/Page" + page_id, data);
            }
            return data;
        }
        #endregion

        #region 取得页面分类
        /// <summary>
        /// 取得页面分类
        /// </summary>
        /// <returns></returns>
        //public ArrayList Pages()
        //{
        //    DYCache cache = DYCache.GetCacheService();
        //    ArrayList data = cache.RetrieveObject(CacheKeys.前台资讯分类 + "/Page") as ArrayList;
        //    if (data == null)
        //    {
        //        data = SiteBLL.GetCmsPageAllList("order_id desc,page_id desc", "title,parent_id,urlrewriter,page_id", "is_show=1");

        //        cache.AddObject(CacheKeys.前台资讯分类 + "/Page", data);
        //    }
        //    return data;
        //}
        #endregion

        #region 取得页面分类名称

        public CmsPageInfo PageInfos(int id)
        {
            CmsPageInfo pageinfo = new CmsPageInfo();
            pageinfo = SiteBLL.GetCmsPageInfo(id);
            return pageinfo;
        }
        #endregion

    }
}
