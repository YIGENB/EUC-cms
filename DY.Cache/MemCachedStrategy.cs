using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Cache
{
    /// <summary>
    /// MemCache缓存策略类
    /// </summary>
    public class MemCachedStrategy : ICacheStrategy
    {

        /// <summary>
        /// 添加指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        public void AddObject(string objId, object o)
        {
            RemoveObject(objId);
            if (TimeOut > 0)
                MemCachedManager.CacheClient.Set(objId, o, System.DateTime.Now.AddMinutes(TimeOut));
            else
                MemCachedManager.CacheClient.Set(objId, o);
        }

        /// <summary>
        /// 添加指定ID的对象(关联指定文件组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="files"></param>
        public void AddObjectWithFileChange(string objId, object o, string[] files)
        {
            ;
        }

        /// <summary>
        /// 添加指定ID的对象(关联指定键值组)
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="dependKey"></param>
        public void AddObjectWithDepend(string objId, object o, string[] dependKey)
        {
            ;
        }

        /// <summary>
        /// 移除指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        public void RemoveObject(string objId)
        {
            if (MemCachedManager.CacheClient.KeyExists(objId))
                MemCachedManager.CacheClient.Delete(objId);
        }

        /// <summary>
        /// 返回指定ID的对象
        /// </summary>
        /// <param name="objId"></param>
        /// <returns></returns>
        public object RetrieveObject(string objId)
        {
            return MemCachedManager.CacheClient.Get(objId);
        }

        /// <summary>
        /// 到期时间
        /// </summary>
        public int TimeOut { set; get; }
    }
}
