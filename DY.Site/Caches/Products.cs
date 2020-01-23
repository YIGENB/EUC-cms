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
    /// 商品操作(降序：先按排序)
    /// </summary>
    public partial class Caches
    {
        #region 取得所有推荐商品
        /// <summary>
        /// 取得所有推荐商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns>返回推荐商品列表信息</returns>
        public ArrayList AllProduct()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/all" ) as ArrayList;
            if (data == null)
            {
  
                data = SiteBLL.GetGoodsAllList("","");

                cache.AddObject("/DY/Web/Goods/all", data);
            }
            return data;
        }
        #endregion

        #region 取得所有热卖商品
        /// <summary>
        /// 取得所有热卖商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns>返回推荐商品列表信息</returns>
        public ArrayList HotProduct(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Hot/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_hot=1 and is_delete=0" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/Goods/Hot/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得所有促销商品
        /// <summary>
        /// 取得所有促销商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns>返回推荐商品列表信息</returns>
        public ArrayList PromoteProduct(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Promote/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_promote=1 and is_delete=0" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/Goods/Promote/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得所有特价商品
        /// <summary>
        /// 取得所有特价商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns>返回推荐商品列表信息</returns>
        public ArrayList SpecialsProduct(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Specials/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_specials=1 and is_delete=0" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/Goods/Specials/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得所有新品商品
        /// <summary>
        /// 取得所有新品商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns>返回推荐商品列表信息</returns>
        public ArrayList NewProduct(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/New/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_new=1 and is_delete=0" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/Goods/New/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得所有推荐商品
        /// <summary>
        /// 取得所有推荐商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns>返回推荐商品列表信息</returns>
        public ArrayList BestProduct(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Best/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_best=1 and is_delete=0" + mobile, out ResultCount);

                cache.AddObject("/DY/Web/Goods/Best/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得指定分类下所有热卖商品
        /// <summary>
        /// 取得指定分类下所有新品商品
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList HotProduct(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Hot" + cat_id + "/Top" + topN) as ArrayList;
            Goods goods = new Goods();
            bool is_cat = !string.IsNullOrEmpty(GetGoodsCatId(cat_id).ToString());//扩展分类
            bool is_child = true;//子分类

            if (data == null)
            {
                int ResultCount = 0;

                string filter = is_cat ? " or goods_id in (" + GetGoodsCatId(cat_id).ToString() + ")" : "";
                string cat_f = is_child ? " in (" + goods.GetGoodsAllCatIds(cat_id) + ")" : "=" + cat_id;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_hot=1 and is_delete=0 and cat_id" + cat_f + filter + mobile, out ResultCount);

                DY.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                cache.AddObject("/DY/Web/Goods/Hot" + cat_id + "/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得指定分类下所有促销商品
        /// <summary>
        /// 取得指定分类下所有促销商品
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList PromoteProduct(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Promote" + cat_id + "/Top" + topN) as ArrayList;
            Goods goods = new Goods();
            bool is_cat = !string.IsNullOrEmpty(GetGoodsCatId(cat_id).ToString());//扩展分类
            bool is_child = true;//子分类

            if (data == null)
            {
                int ResultCount = 0;

                string filter = is_cat ? " or goods_id in (" + GetGoodsCatId(cat_id).ToString() + ")" : "";
                string cat_f = is_child ? " in (" + goods.GetGoodsAllCatIds(cat_id) + ")" : "=" + cat_id;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_promote=1 and is_delete=0 and cat_id" + cat_f + filter + mobile, out ResultCount);

                DY.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                cache.AddObject("/DY/Web/Goods/Promote" + cat_id + "/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得指定分类下所有特价商品
        /// <summary>
        /// 取得指定分类下所有特价商品
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList SpecialsProduct(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Specials" + cat_id + "/Top" + topN) as ArrayList;
            Goods goods = new Goods();
            bool is_cat = !string.IsNullOrEmpty(GetGoodsCatId(cat_id).ToString());//扩展分类
            bool is_child = true;//子分类

            if (data == null)
            {
                int ResultCount = 0;

                string filter = is_cat ? " or goods_id in (" + GetGoodsCatId(cat_id).ToString() + ")" : "";
                string cat_f = is_child ? " in (" + goods.GetGoodsAllCatIds(cat_id) + ")" : "=" + cat_id;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_specials=1 and is_delete=0 and cat_id" + cat_f + filter + mobile, out ResultCount);

                DY.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                cache.AddObject("/DY/Web/Goods/Specials" + cat_id + "/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得指定分类下所有新品商品
        /// <summary>
        /// 取得指定分类下所有新品商品
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList NewProduct(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/New" + cat_id + "/Top" + topN) as ArrayList;
            Goods goods = new Goods();
            bool is_cat = !string.IsNullOrEmpty(GetGoodsCatId(cat_id).ToString());//扩展分类
            bool is_child = true;//子分类

            if (data == null)
            {
                int ResultCount = 0;

                string filter = is_cat ? " or goods_id in (" + GetGoodsCatId(cat_id).ToString() + ")" : "";
                string cat_f = is_child ? " in (" + goods.GetGoodsAllCatIds(cat_id) + ")" : "=" + cat_id;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_new=1 and is_delete=0 and cat_id" + cat_f + filter + mobile, out ResultCount);

                DY.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                cache.AddObject("/DY/Web/Goods/New" + cat_id + "/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得指定分类下所有推荐商品
        /// <summary>
        /// 取得指定分类下所有推荐商品
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList BestProduct(int topN, int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Best" + cat_id + "/Top" + topN) as ArrayList;
            Goods goods = new Goods();
            bool is_cat = !string.IsNullOrEmpty(GetGoodsCatId(cat_id).ToString());//扩展分类
            bool is_child = true;//子分类

            if (data == null)
            {
                int ResultCount = 0;

                string filter = is_cat ? " or goods_id in (" + GetGoodsCatId(cat_id).ToString() + ")" : "";
                string cat_f = is_child ? " in (" + goods.GetGoodsAllCatIds(cat_id) + ")" : "=" + cat_id;

                data = SiteBLL.GetGoodsList(1, topN, "sort_order desc,goods_id desc", "is_on_sale=1 and is_best=1 and is_delete=0 and cat_id" + cat_f + filter + mobile, out ResultCount);

                DY.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                cache.AddObject("/DY/Web/Goods/Best" + cat_id + "/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得商品关联资讯
        /// <summary>
        /// 取得商品关联资讯
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ArrayList ProductRelationNews(int topN, int goods_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/RelationNews/Top" + topN + goods_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                //data = SiteBLL.GetGoodsLinkList(1, topN, "goods_id desc ", "goods_id=" + goods_id + " and type=1" + mobile, out ResultCount);
                data = new ArrayList();
                foreach (GoodsLinkInfo link in SiteBLL.GetGoodsLinkList(1, topN, "goods_id desc ", "goods_id=" + goods_id + " and type=1" + mobile, out ResultCount))
                    data.Add(SiteBLL.GetCmsInfo(link.link_goods_id.Value));

                cache.AddObject("/DY/Web/Goods/RelationNews/Top" + topN + goods_id, data);
            }
            return data;
        }
        #endregion

        #region 取得商品关联商品
        /// <summary>
        /// 取得商品关联资讯
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ArrayList ProductRelationProduct(int topN, int goods_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/RelationProduct/Top" + topN + goods_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                //data = SiteBLL.GetGoodsLinkList(1, topN, "goods_id desc ", "goods_id=" + goods_id + " and type=0" + mobile, out ResultCount);

                data = new ArrayList();

                foreach (GoodsLinkInfo link in SiteBLL.GetGoodsLinkList(1, topN, "goods_id desc ", "goods_id=" + goods_id + " and type=0" + mobile, out ResultCount))
                    data.Add(SiteBLL.GetGoodsInfo(link.link_goods_id.Value));

                cache.AddObject("/DY/Web/Goods/RelationProduct/Top" + topN + goods_id, data);
            }
            return data;
        }
        #endregion

        #region 取得商品关联下载
        /// <summary>
        /// 取得商品关联下载
        /// </summary>
        /// <param name="topN"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ArrayList ProductRelationDown(int topN, int goods_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/RelationDown/Top" + topN + goods_id) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                //data = SiteBLL.GetGoodsLinkList(1, topN, "goods_id desc ", "goods_id=" + goods_id + " and type=0" + mobile, out ResultCount);

                data = new ArrayList();

                foreach (GoodsLinkInfo link in SiteBLL.GetGoodsLinkList(1, topN, "goods_id desc ", "goods_id=" + goods_id + " and type=2" + mobile, out ResultCount))
                    data.Add(SiteBLL.GetDownloadInfo(link.link_goods_id.Value));

                cache.AddObject("/DY/Web/Goods/RelationDown/Top" + topN + goods_id, data);
            }
            return data;
        }
        #endregion

        #region 拓展分类
        /// <summary>
        /// 获取商品扩展分类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public static object GetGoodsCatId(int cat_id)
        {
            Goods goods = new Goods();
            string sql = "SELECT goods_id FROM " + DY.Config.BaseConfig.TablePrefix + "goods_cat WHERE cat_id in(" + goods.GetGoodsAllCatIds(cat_id) + ")";
            //string ids = "";
            //Goods goods = new Goods();
            //string catids = !string.IsNullOrEmpty(goods.GetGoodsAllCatIds(cat_id)) ? goods.GetGoodsAllCatIds(cat_id) + "," + cat_id : cat_id.ToString() + ",";
            //for (int i = 0; i < catids.Split(',').Length; i++)
            //{
            //    foreach (GoodsCatInfo catinfo in SiteBLL.GetGoodsCatAllList("", "cat_id=" + catids.Split(',')[i]))
            //    {
            //        ids += catinfo.goods_id + ",";
            //    }
            //}
            //if (ids != "")
            //    ids = ids.Remove(ids.LastIndexOf(","), 1);
            //return ids;
            return sql;
        }
        #endregion

        #region 取得指定商品分类下的子类
        /// <summary>
        /// 取得指定商品分类下的子类
        /// </summary>
        /// <returns></returns>
        public ArrayList ProductCat(int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台商品分类 + cat_id) as ArrayList;
            if (data == null)
            {
                data = new Goods().GetGoodsCatList(cat_id);

                cache.AddObject(CacheKeys.前台商品分类 + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 取得手机端指定商品分类下的子类
        /// <summary>
        /// 取得手机端指定商品分类下的子类
        /// </summary>
        /// <returns></returns>
        public ArrayList MobileProductCat(int cat_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台手机商品分类 + cat_id) as ArrayList;
            if (data == null)
            {
                data = new Goods().GetGoodsCatList(cat_id);

                cache.AddObject(CacheKeys.前台手机商品分类 + cat_id, data);
            }
            return data;
        }
        #endregion

        #region 获取显示商品分类
        /// <summary>
        /// 获取显示商品分类
        /// </summary>
        /// <returns>返回产品分类列表信息</returns>
        public ArrayList ShowProductCat(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台商品分类 + "/Show") as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;
                data = SiteBLL.GetGoodsCategoryList(1, topN, "sort_order asc", "is_show=1", out ResultCount);

                cache.AddObject(CacheKeys.前台商品分类 + "/Show", data);
            }
            return data;
        }
        #endregion

        #region 获取商品热门分类
        /// <summary>
        /// 获取商品热门分类
        /// </summary>
        /// <returns>返回产品分类列表信息</returns>
        public ArrayList HotProductCat()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台商品分类 + "/Hot") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetGoodsCategoryAllList("sort_order asc", "is_hot=1 and is_show=1");

                cache.AddObject(CacheKeys.前台商品分类 + "/Hot", data);
            }
            return data;
        }
        #endregion

        #region 取得热门城市分站
        /// <summary>
        /// 取得热门城市分站
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList CityHot(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/City/Hot/Top" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetRegionList(1, topN, "region_id desc ", "is_pay=1 and region_type=1", out ResultCount);

                cache.AddObject("/DY/Web/City/Hot/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region 单个属性

        #endregion


        #region 取得资讯分类名称

        public GoodsCategoryInfo GoodsCatInfos(int id)
        {
            GoodsCategoryInfo pageinfo = new GoodsCategoryInfo();
            pageinfo = SiteBLL.GetGoodsCategoryInfo(id);
            return pageinfo;
        }
        #endregion

        #region 取得产品详情

        public GoodsInfo GoodsInfos(int id)
        {
            GoodsInfo pageinfo = new GoodsInfo();
            pageinfo = SiteBLL.GetGoodsInfo(id);
            return pageinfo;
        }
        #endregion

    }
}
