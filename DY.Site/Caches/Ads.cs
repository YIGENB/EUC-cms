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
    /// 广告缓存类
    /// </summary>
    public partial class Caches
    {
        #region 广告相关

        /// <summary>
        /// 取得广告列表
        /// </summary>
        /// <param name="pos_id">广告位置ID</param>
        /// <returns></returns>
        public static DataTable GetAds(int pos_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Ads/t" + pos_id) as DataTable;
            if (data == null)
            {
                data = DatabaseProvider.GetInstance().GetAds(pos_id);

                cache.AddObject("/DY/Web/Ads/t" + pos_id, data);
            }
            return data;
        }

        /// <summary>
        /// 取得广告列表
        /// </summary>
        /// <param name="pos_id">广告位置ID</param>
        /// <returns></returns>
        public static ArrayList AllAds(int pos_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Ads/t" + pos_id) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetAdAllList("", "position_id=" + pos_id);

                cache.AddObject("/DY/Web/Ads/t" + pos_id, data);
            }
            return data;
        }

        /// <summary>
        /// 取得广告位
        /// </summary>
        /// <returns></returns>
        public ArrayList AdPosition()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Ads/Position") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetAdPositionAllList("", "");

                cache.AddObject("/DY/Web/Ads/Position", data);
            }
            return data;
        }

        #endregion
    }
}
