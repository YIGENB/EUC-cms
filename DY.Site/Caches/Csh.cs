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
    /// 客服操作类
    /// </summary>
    public partial class Caches
    {
        /// <summary>
        /// 取得客服数据
        /// </summary>
        /// <returns></returns>
        public ArrayList AllCsh(int type)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Csh/Csh" + type) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetCshAllList("", "csh_type=" + type);

                cache.AddObject("/DY/Web/Csh/Csh" + type, data);
            }
            return data;
        }

        /// <summary>
        /// 取得客服数据
        /// </summary>
        /// <returns></returns>
        public ArrayList AllCsh(int topN, int type)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Csh/Csh/top" + topN + type) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCshList(1, topN, "csh_order desc", "csh_type=" + type, out ResultCount);

                cache.AddObject("/DY/Web/Csh/Csh" + topN + type, data);
            }
            return data;
        }

    }
}
