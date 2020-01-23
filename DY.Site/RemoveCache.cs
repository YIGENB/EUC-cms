using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

using DY.Cache;
using DY.Common;

namespace DY.Site
{
    /// <summary>
    /// 移除缓存类
    /// </summary>
    public class RemoveCache
    {
        private static readonly DYCache cache = DYCache.GetCacheService();
        /// <summary>
        /// 移除网站设置缓存
        /// </summary>
        public static void Config()
        {
            cache.RemoveObject(CacheKeys.网站设置);
        }
        /// <summary>
        /// 移除前台底部导航缓存
        /// </summary>
        public static void FootNav()
        {
            cache.RemoveObject(CacheKeys.前台底部导航);
        }
        /// <summary>
        /// 移除前台商品分类缓存
        /// </summary>
        public static void GoodsCat()
        {
            cache.RemoveObject(CacheKeys.前台商品分类);
        }
        /// <summary>
        /// 移除前台主导航缓存
        /// </summary>
        public static void MainNav()
        {
            cache.RemoveObject(CacheKeys.前台主导航);
        }
        /// <summary>
        /// 移除前台资讯分类缓存
        /// </summary>
        public static void CMSCat()
        {
            cache.RemoveObject(CacheKeys.前台资讯分类);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static int All()
        {
            int count = HttpRuntime.Cache.Count;

            IDictionaryEnumerator CacheIDE = HttpRuntime.Cache.GetEnumerator();
            while (CacheIDE.MoveNext())
            {
                HttpContext.Current.Cache.Remove(CacheIDE.Key.ToString());
            }

            return count;
        }
    }
}
