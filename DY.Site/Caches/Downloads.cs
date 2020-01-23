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
    /// 下载操作类
    /// </summary>
    public partial class Caches
    {

        #region 所有最新的信息
        /// <summary>
        /// 所有最新的信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList NewDownload(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Download/New") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetDownloadAllList("showtime desc,orderid desc,down_id desc", "");
                cache.AddObject("/DY/Web/Download/New", data);
            }
            return data;
        }
        #endregion

        #region 所有推荐的信息
        /// <summary>
        /// 取得所有推荐的信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList BestDownload(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Download/Best/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;
                data = SiteBLL.GetDownloadList(1, topN, "orderid desc,showtime desc,down_id desc", "is_enable=1 and is_top=1", out ResultCount);
                cache.AddObject("/DY/Web/Download/Best/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 指定分类下的推荐下载
        /// <summary>
        /// 取得指定分类下的推荐下载
        /// </summary>
        /// <param name="cat_id"></param>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList BestDownload(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            Download download = new Download();
            ArrayList data = cache.RetrieveObject("/DY/Web/Download/Best" + cat_id + "/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetDownloadList(1, topN, "orderid desc,showtime desc,down_id desc", "is_enable=1 and is_top=1 and cat_id in (" + download.GetDownloadCatAllIds(cat_id) + ")", out ResultCount);

                cache.AddObject("/DY/Web/Download/Best" + cat_id + "/Top" + topN, data);
            }
            return data;
        }


        #endregion

        #region 指定分类下最新的信息
        /// <summary>
        /// 指定分类下最新的信息
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public ArrayList NewDownload(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            Download download = new Download();
            ArrayList data = cache.RetrieveObject("/DY/Web/Download/New" + topN + "/Cat" + cat_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetDownloadList(1, topN, "showtime desc", "is_enable=1 and cat_id in (" + download.GetDownloadCatAllIds(cat_id) + ")", out ResultCount);

                cache.AddObject("/DY/Web/Download/New" + topN + "/Cat" + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 取得指定下载分类下的子类
        /// <summary>
        /// 取得指定下载分类下的子类
        /// </summary>
        /// <returns></returns>
        public ArrayList GetDownloadCat(int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台下载分类 + cat_id) as ArrayList;
            if (data == null)
            {
                data = new Download().GetDownloadCatList(cat_id);

                cache.AddObject(CacheKeys.前台下载分类 + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 取得页面分类名称

        public DownloadCategoryInfo DownloadCategoryInfos(int id)
        {
            DownloadCategoryInfo pageinfo = new DownloadCategoryInfo();
            pageinfo = SiteBLL.GetDownloadCategoryInfo(id);
            return pageinfo;
        }
        #endregion

    }
}
