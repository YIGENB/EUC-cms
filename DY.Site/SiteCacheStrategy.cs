using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace DY.Site
{
    /// <summary>
    /// ForumCacheStrategy 缓存
    /// 定制的论坛缓存策略类, 以实现到期时间设置和其它设置
    /// </summary>
    public class ForumCacheStrategy : DY.Cache.ICacheStrategy
    {
        protected static volatile System.Web.Caching.Cache webCacheforfocus = null;

        private int _timeOut = 1200; // 默认缓存存活期为20分钟

        /// <summary>
        /// 构造函数
        /// </summary>
        static ForumCacheStrategy()
        {
            webCacheforfocus = DY.Cache.DefaultCacheStrategy.GetWebCacheObj;
        }

        /// <summary>
        /// 设置到期相对时间[单位:秒]
        /// </summary>
        virtual public int TimeOut
        {
            set { _timeOut = (value < 1200) ? value : 1200; }
            get { return (_timeOut < 1200) ? _timeOut : 1200; }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        public void AddObject(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
                return;

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);
            webCacheforfocus.Insert(objId, o, null, DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
        }


        public void AddObjectWith(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null)
                return;

            webCacheforfocus.Insert(objId, o, null, System.DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration);
        }



        /// <summary>
        /// 加入当前对象到缓存中,并对相关文件建立依赖
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        /// <param name="files">监视的路径文件</param>
        public void AddObjectWithFileChange(string objId, object o, string[] files)
        {
            if (objId == null || objId.Length == 0 || o == null)
                return;

            CacheDependency dep = new CacheDependency(files, DateTime.Now);
            webCacheforfocus.Insert(objId, o, dep, System.DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration);
        }



        /// <summary>
        /// 加入当前对象到缓存中,并使用依赖键
        /// </summary>
        /// <param name="objId">key for the object</param>
        /// <param name="o">object</param>
        /// <param name="dependKey">监视的路径文件</param>
        public void AddObjectWithDepend(string objId, object o, string[] dependKey)
        {
            if (objId == null || objId.Length == 0 || o == null)
                return;

            CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);
            webCacheforfocus.Insert(objId, o, dep, System.DateTime.Now.AddSeconds(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        public void RemoveObject(string objId)
        {
            if (objId == null || objId.Length == 0)
                return;

            webCacheforfocus.Remove(objId);
        }


        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public object RetrieveObject(string objId)
        {
            if (objId == null || objId.Length == 0)
                return null;

            return webCacheforfocus.Get(objId);
        }


        //建立回调委托的一个实例
        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
            return;
        }
    }


    /// <summary>
    /// RssCacheStrategy 缓存
    /// Rss缓存策略类, 以实现到期时间设置设置
    /// </summary>
    /// </summary>
    public class RssCacheStrategy : ForumCacheStrategy
    {
        private int _timeOut = 600; // 默认缓存存活期为10分钟

        //设置到期相对时间[单位:分钟] 
        override public int TimeOut
        {
            set { _timeOut = (value > 0 && value < 9999) ? value : 600; }
            get { return (_timeOut > 0 && _timeOut < 9999) ? _timeOut : 600; }
        }
    }

    /// <summary>
    /// SitemapCacheStrategy 缓存
    /// Sitemap缓存策略类, 以实现到期时间设置设置
    /// </summary>
    /// </summary>
    public class SitemapCacheStrategy : ForumCacheStrategy
    {
        private int _timeOut = 720; // 默认缓存存活期为12分钟

        //设置到期相对时间[单位:秒] 
        override public int TimeOut
        {
            set { _timeOut = (value > 0 && value < 9999) ? value : 720; }
            get { return (_timeOut > 0 && _timeOut < 9999) ? _timeOut : 720; }
        }
    }
}
