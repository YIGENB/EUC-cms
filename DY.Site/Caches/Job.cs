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
    /// 招聘缓存类
    /// </summary>
    public partial class Caches
    {
        #region 部门相关
        /// <summary>
        /// 取得部门
        /// </summary>
        /// <returns></returns>
        public ArrayList JobDepartment()
        {

            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Job/Department") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetJobDepartmentAllList("sort_order desc", "");

                cache.AddObject("/DY/Web/Job/Department", data);
            }
            return data;
        }

        /// <summary>
        /// 取得部门下所有职位
        /// </summary>
        /// <param name="cat_id">部门id</param>
        /// <returns></returns>
        public ArrayList JobPosition(int cat_id)
        {

            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Job/Position" + cat_id) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetJobPositionAllList("sort_order desc,en_time desc", "getdate()<en_time and department_id=" + cat_id);

                cache.AddObject("/DY/Web/Job/Position" + cat_id, data);
            }
            return data;
        }
        #endregion
    }
}
