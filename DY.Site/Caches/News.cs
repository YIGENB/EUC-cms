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
    /// 资讯文章操作(降序：先按排序，在按发表时间)
    /// </summary>
    public partial class Caches
    {

        #region 资讯关联商品
        /// <summary>
        /// 取得资讯关联商品
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ArrayList NewsRelationProducts(int topN, int article_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Cms/Goods/Top" + topN + article_id) as ArrayList;
            if (data == null)
            {
                data = new ArrayList();
                int ResultCount = 0;
                foreach (CmsLinkInfo link in SiteBLL.GetCmsLinkList(1, topN, "cms_id desc ", "cms_id=" + article_id + " and type=0 " + mobile, out ResultCount))
                    data.Add(SiteBLL.GetGoodsInfo(link.link_goods_id.Value));
                //data = SiteBLL.GetCmsLinkList(1, topN, "cms_id desc ", "cms_id=" + article_id + " " + mobile, out ResultCount);

                cache.AddObject("/DY/Web/Cms/Goods/Top" + topN + article_id, data);
            }
            return data;
        }
        #endregion

        #region 资讯关联资讯
        /// <summary>
        /// 取得资讯关联资讯
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ArrayList NewsRelationNews(int topN, int article_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/News/Cms/Top" + topN + article_id) as ArrayList;
            if (data == null)
            {
                data = new ArrayList();
                int ResultCount = 0;
                foreach (CmsLinkInfo link in SiteBLL.GetCmsLinkList(1, topN, "cms_id desc ", "cms_id=" + article_id + " and type=1 " + mobile, out ResultCount))
                    data.Add(SiteBLL.GetCmsInfo(link.link_goods_id.Value));
                //data = SiteBLL.GetCmsLinkList(1, topN, "cms_id desc ", "cms_id=" + article_id + " " + mobile, out ResultCount);

                cache.AddObject("/DY/Web/News/Cms/Top" + topN + article_id, data);
            }
            return data;
        }
        #endregion

        #region 资讯关联下载
        /// <summary>
        /// 取得资讯关联下载
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public ArrayList NewsRelationDown(int topN, int article_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/News/RelationDown/Top" + topN + article_id) as ArrayList;
            if (data == null)
            {

                data = new ArrayList();
                int ResultCount = 0;
                foreach (CmsLinkInfo link in SiteBLL.GetCmsLinkList(1, topN, "cms_id desc ", "cms_id=" + article_id + " and type=2 " + mobile, out ResultCount))
                    data.Add(SiteBLL.GetDownloadInfo(link.link_goods_id.Value));
                //data = SiteBLL.GetCmsLinkList(1, topN, "cms_id desc ", "cms_id=" + article_id + " " + mobile, out ResultCount);

                cache.AddObject("/DY/Web/News/RelationDown/Top" + topN + article_id, data);
            }
            return data;
        }
        #endregion

        #region 所有热门的信息
        /// <summary>
        /// 取得指定分类下热门的信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList HotNews(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/Hot" + topN) as ArrayList;
            if (data == null)
            {

                int ResultCount = 0;

                data = SiteBLL.GetCmsList(1, topN, "sort_order desc,showtime desc,article_id desc", "is_show=1 and is_hot=1" + mobile, out ResultCount);

                ////DY.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                ////ics.TimeOut = 10;
                ////cache.LoadCacheStrategy(ics);
                cache.AddObject("/DY/Web/CMS/Hot" + topN, data);
                ////cache.LoadDefaultCacheStrategy();
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
        public ArrayList BestNews(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/Best/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;
                data = SiteBLL.GetCmsList(1, topN, "sort_order desc,showtime desc,article_id desc", "is_show=1 and is_best=1" + mobile, out ResultCount);
                cache.AddObject("/DY/Web/CMS/Best/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 所有置顶的信息
        /// <summary>
        /// 取得所有置顶的信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList TopNews(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/Best/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;
                data = SiteBLL.GetCmsList(1, topN, "sort_order desc,showtime desc,article_id desc", "is_show=1 and is_top=1" + mobile, out ResultCount);
                cache.AddObject("/DY/Web/CMS/Best/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 所有最新的信息
        /// <summary>
        /// 所有最新的信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList NewNews(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/New" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCmsList(1, topN, "sort_order desc,showtime desc,article_id desc", "showtime<getdate() and is_show=1", out ResultCount);
                cache.AddObject("/DY/Web/CMS/New" + topN, data);
            }
            return data;
        }
        #endregion

        #region 指定分类下热门的信息
        /// <summary>
        /// 取得指定分类下热门的信息
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public ArrayList HotNews(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            CMS cms = new CMS();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/Hot" + topN + "/Cat" + cat_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCmsList(1, topN, "article_id desc", "showtime<getdate() and is_show=1 and is_hot=1 and cat_id in (" + cms.GetCMSCatAllIds(cat_id) + ")" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/CMS/Hot" + topN + "/Cat" + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 指定分类下的置顶新闻
        /// <summary>
        /// 取得指定分类下的置顶新闻
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList TopNews(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            CMS cms = new CMS();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/Top" + topN + "/Cat" + cat_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCmsList(1, topN, "sort_order desc,showtime desc,article_id desc", "showtime<getdate() and is_show=1 and is_top=1 and cat_id in (" + cms.GetCMSCatAllIds(cat_id) + ")" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/CMS/Top" + topN + "/Cat" + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 指定分类下的推荐新闻
        /// <summary>
        /// 取得指定分类下的推荐新闻
        /// </summary>
        /// <param name="cat_id"></param>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList BestNews(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            CMS cms = new CMS();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/Best" + cat_id + "/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCmsList(1, topN, "sort_order desc,showtime desc,article_id desc", "showtime<getdate() and is_show=1 and is_best=1 and cat_id in (" + cms.GetCMSCatAllIds(cat_id) + ")" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/CMS/Best" + cat_id + "/Top" + topN, data);
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
        public ArrayList NewNews(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            CMS cms = new CMS();
            ArrayList data = cache.RetrieveObject("/DY/Web/CMS/New" + topN + "/Cat" + cat_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCmsList(1, topN, "showtime desc", "showtime<getdate() and is_show=1 and cat_id in (" + cms.GetCMSCatAllIds(cat_id) + ")" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/CMS/New" + topN + "/Cat" + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 取得指定资讯分类下的子类
        /// <summary>
        /// 取得指定资讯分类下的子类
        /// </summary>
        /// <returns></returns>
        public ArrayList NewsCat(int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台资讯分类 + cat_id) as ArrayList;
            if (data == null)
            {
                data = new CMS().GetCMSCatList(cat_id);

                cache.AddObject(CacheKeys.前台资讯分类 + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 取得资讯分类名称

        public CmsCatInfo CatInfos(int id)
        {
            CmsCatInfo pageinfo = new CmsCatInfo();
            pageinfo = SiteBLL.GetCmsCatInfo(id);
            return pageinfo;
        }
        #endregion


    }
}
