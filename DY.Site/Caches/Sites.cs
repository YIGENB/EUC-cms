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
using DY.OAuthV2SDK;
using DY.OAuthV2SDK.Entitys;

namespace DY.Site
{
    /// <summary>
    /// 网站缓存类（包括友情链接、导航等）
    /// </summary>
    public partial class Caches
    {
        #region 取得lbs位置
        /// <summary>
        /// 取得lbs位置
        /// </summary>
        /// <returns></returns>
        public ArrayList AllLBS()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/lbsall") as ArrayList;
            if (data == null)
            {

                data = SiteBLL.GetLbsAllList("", "");
                cache.AddObject("/DY/Web/lbsall", data);
            }
            return data;
        }
        #endregion

        #region LBS
        /// <summary>
        /// LBS
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns>返回LBS列表信息</returns>
        public ArrayList AllLBS(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/LBS/TOP" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetLbsList(1, topN, "order_id desc,id desc", "", out ResultCount);

                cache.AddObject("/DY/Web/LBS/TOP" + topN, data);
            }
            return data;
        }
        #endregion

        #region 取得登录接口
        /// <summary>
        /// 取得登录接口
        /// </summary>
        /// <returns></returns>
        public List<OAuthEntity> GetConfigOAuths()
        {
            DYCache cache = DYCache.GetCacheService();
            List<OAuthEntity> data = cache.RetrieveObject("/DY/Web/ConfigOAuths") as List<OAuthEntity>;
            if (data == null)
            {

                data = OAuthConfig.GetConfigOAuths();
                cache.AddObject("/DY/Web/ConfigOAuths", data);
            }
            return data;
        }
        #endregion

        #region 配送方式
        public DeliveryInfo GetDelivery_Info(int id)
        {
            DeliveryInfo del_info = new DeliveryInfo();
            del_info = SiteBLL.GetDeliveryInfo(id);
            return del_info;

        }
        #endregion

        #region 取得友情链接列表
        /// <summary>
        /// 取得友情链接列表
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns></returns>
        public ArrayList FriendLink(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Friend/Link/t" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                if (topN > 0)
                    data = SiteBLL.GetFriendLinkList(1, topN, "show_order desc,link_id desc", "", out ResultCount);
                else
                    data = SiteBLL.GetFriendLinkAllList("show_order desc,link_id desc", "");

                cache.AddObject("/DY/Web/Friend/Link/t" + topN, data);
            }
            return data;
        }

        /// <summary>
        /// 取得所有友情链接列表
        /// </summary>
        /// <returns></returns>
        public ArrayList FriendLink()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Friend/Link") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetFriendLinkAllList("show_order desc,link_id desc", "");

                cache.AddObject("/DY/Web/Friend/Link", data);
            }
            return data;
        }
        #endregion

        #region 获取推荐订单列表
        /// <summary>
        /// 获取推荐订单列表
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns></returns>
        public ArrayList BestOrder(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Order/Lint/t" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                //data = SiteBLL.GetOrderInfoList(1, topN, "order_sn,consignee,add_time", "add_time desc", "order_status=4 and id_delete=0", out ResultCount);
                data = SiteBLL.GetOrderInfoList(1, topN, "*", "add_time desc", "order_status=4 and id_delete=0", out ResultCount);

                cache.AddObject("/DY/Web/Order/Lint/t" + topN, data);
            }
            return data;
        }
        #endregion

        #region 获取推荐评论
        /// <summary>
        /// 获取推荐评论
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList Comm(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/comment/list/t" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCommentList(1, topN, "comment_type,id_value,user_name,content,url,add_time", "add_time desc", "enabled=1 and is_recomm=1", out ResultCount);

                cache.AddObject("/DY/Web/comment/list/t" + topN, data);
            }
            return data;
        }
        #endregion

        #region 获取首页评论信息
        /// <summary>
        /// 获取首页评论信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public static ArrayList IndexReviews(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Index/Review/t" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetCommentList(1, topN, "comment_id desc", "enabled=1 and is_read=1", out ResultCount);

                cache.AddObject("/DY/Web/Index/Review/t" + topN, data);
            }
            return data;
        }

        #endregion

        #region 取得所有内部链接
        /// <summary>
        /// 取得所有内部链接
        /// </summary>
        /// <returns></returns>
        public static ArrayList InternalLink()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/InternalLink/List") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetInternalLinksAllList("ID asc", "is_enable=1");

                cache.AddObject("/DY/Web/InternalLink/List", data);
            }
            return data;
        }
        #endregion

        #region 获取首页搜索关键字
        /// <summary>
        /// 获取首页搜索关键字
        /// </summary>
        /// <returns></returns>
        public string SearchKeywords()
        {
            string key = Config.BaseConfig.Get().SearchKeywords;
            string newkey = "";
            if (string.IsNullOrEmpty(key)) { return ""; }
            string[] keys = key.Split(',');
            if (keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    newkey += "<a href='/goods/s" + keys[i] + ".aspx'>" + keys[i] + "</a>";
                }
            }
            return newkey;
        }
        #endregion

        #region 主导航
        /// <summary>
        /// 主导航
        /// </summary>
        /// <returns>返回主导航列表信息</returns>
        public ArrayList Nav()
        {
            DYCache cache = DYCache.GetCacheService();
            string mobile = SiteUtils.IsMobileDevice() ? "and is_mobile=1" : "";
            ArrayList data = SiteUtils.IsMobileDevice() ? cache.RetrieveObject(CacheKeys.前台手机导航) as ArrayList : cache.RetrieveObject(CacheKeys.前台主导航) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetNavigateAllList("vieworder desc,id asc", "isshow=1 and type='主导航' and parent_id=0" + mobile);
                if (SiteUtils.IsMobileDevice())
                    cache.AddObject(CacheKeys.前台手机导航, data);
                else
                    cache.AddObject(CacheKeys.前台主导航, data);
            }
            return data;
        }
        #endregion

        #region 主导航下的导航
        /// <summary>
        /// 主导航
        /// </summary>
        /// <returns>返回主导航列表信息</returns>
        public ArrayList Nav(int id)
        {
            DYCache cache = DYCache.GetCacheService();
            mobile = SiteUtils.IsMobileDevice() ? "and is_mobile=1" : "";
            ArrayList data = SiteUtils.IsMobileDevice() ? cache.RetrieveObject(CacheKeys.前台手机导航 + id) as ArrayList : cache.RetrieveObject(CacheKeys.前台主导航 + id) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetNavigateAllList("vieworder desc,id asc", "isshow=1 and type='主导航' and parent_id=" + id + mobile);
                if (SiteUtils.IsMobileDevice())
                    cache.AddObject(CacheKeys.前台手机导航 + id, data);
                else
                    cache.AddObject(CacheKeys.前台主导航 + id, data);
            }
            return data;
        }
        #endregion

        #region 快捷导航
        /// <summary>
        /// 快捷导航
        /// </summary>
        /// <returns></returns>
        public ArrayList FastNav()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = SiteUtils.IsMobileDevice() ? cache.RetrieveObject(CacheKeys.前台手机快捷导航) as ArrayList : cache.RetrieveObject(CacheKeys.前台快捷导航) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetNavigateAllList("vieworder desc,id asc", "isshow=1 and type='快捷菜单'" + mobile);
                if (SiteUtils.IsMobileDevice())
                    cache.AddObject(CacheKeys.前台手机快捷导航, data);
                else
                    cache.AddObject(CacheKeys.前台快捷导航, data);
            }
            return data;
        }
        #endregion

        #region 底部导航
        /// <summary>
        /// 底部导航
        /// </summary>
        /// <returns>返回主底部导航列表信息</returns>
        public ArrayList FootNav()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = SiteUtils.IsMobileDevice() ? cache.RetrieveObject(CacheKeys.前台手机底部导航) as ArrayList : cache.RetrieveObject(CacheKeys.前台底部导航) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetNavigateAllList("vieworder desc,id asc", "isshow=1 and type='底部导航'" + mobile);

                if (SiteUtils.IsMobileDevice())
                    cache.AddObject(CacheKeys.前台手机底部导航, data);
                else
                    cache.AddObject(CacheKeys.前台底部导航, data);
            }
            return data;
        }
        #endregion

        #region 底部导航下的导航
        /// <summary>
        /// 底部导航
        /// </summary>
        /// <returns>返回主底部导航列表信息</returns>
        public ArrayList FootNav(int id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = SiteUtils.IsMobileDevice() ? cache.RetrieveObject(CacheKeys.前台手机底部导航 + id) as ArrayList : cache.RetrieveObject(CacheKeys.前台底部导航 + id) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetNavigateAllList("vieworder desc,id asc", "isshow=1 and type='底部导航' and parent_id=" + id);

                if (SiteUtils.IsMobileDevice())
                    cache.AddObject(CacheKeys.前台手机底部导航 + id, data);
                else
                    cache.AddObject(CacheKeys.前台底部导航 + id, data);
            }
            return data;
        }
        #endregion

        #region 后台菜单
        /// <summary>
        /// 后台菜单
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns>返回后台菜单列表信息</returns>
        public DataTable AdminMenu(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Admin/Menu" + parent_id) as DataTable;
            if (data == null)
            {
                data = MenuManage.GetAdminMenuList(parent_id);

                //cache.AddObject("/DY/Web/Admin/Menu" + parent_id, data);
            }
            return data;
        }
        #endregion


        #region 公众号信息列表
        /// <summary>
        /// 公众号信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns>返回后台公众号列表信息</returns>
        public ArrayList WeiXin(int topN)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/WeixinMp" + topN) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetWeixinList(1, topN, "mp_id asc", "enabled=1", out ResultCount);

                cache.AddObject("/DY/Web/WeixinMp" + topN, data);
            }
            return data;
        }
        #endregion

        /// <summary>
        /// 取得网站前台所有模板
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSiteTlp()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/All/Tlp") as DataTable;
            if (data == null)
            {
                DataTable dataweb = FileOperate.searchDirectoryAllInfo(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.WebSkinPath), "*" + BaseConfig.WebSkinSuffix);
                DataTable datawap = FileOperate.searchDirectoryAllInfo(System.Web.HttpContext.Current.Server.MapPath(BaseConfig.WapSkinPath), "*" + BaseConfig.WebSkinSuffix);
                data = SiteUtils.MergeDataTable(dataweb, datawap, "name", 100);

                cache.AddObject("/DY/Web/All/Tlp", data);
            }
            return data;
        }
        #region 取得网站设置详细

        public ConfigInfo ConfigInfos(int id)
        {
            ConfigInfo pageinfo = new ConfigInfo();
            pageinfo = SiteBLL.GetConfigInfo(id);
            return pageinfo;
        }
        #endregion
        #region 取得菜单设置详细

        public AdminMenuInfo AdminMenuInfos(int id)
        {
            AdminMenuInfo pageinfo = new AdminMenuInfo();
            pageinfo = SiteBLL.GetAdminMenuInfo(id);
            return pageinfo;
        }
        #endregion
        #region 取得权限设置详细

        public AdminActionInfo AdminActionInfos(int id)
        {
            AdminActionInfo pageinfo = new AdminActionInfo();
            pageinfo = SiteBLL.GetAdminActionInfo(id);
            return pageinfo;
        }
        #endregion

        #region 取得导航名称

        public NavigateInfo NavigatorInfos(int id)
        {
            NavigateInfo navigatorinfo = new NavigateInfo();
            navigatorinfo = SiteBLL.GetNavigateInfo(id);
            return navigatorinfo;
        }
        #endregion

        #region 搜索的信息
        /// <summary>
        /// 搜索的信息
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public ArrayList Search(int topN, string keyword)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Search/Top" + topN + FunctionUtils.Text.ConvertSpellFirst(keyword)) as ArrayList;
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetSearchList(1, topN, "date desc", "is_delete=0 and title like '%" + keyword + "%'", out ResultCount);
                cache.AddObject("/DY/Web/Search/Top" + topN + FunctionUtils.Text.ConvertSpellFirst(keyword), data);
            }
            return data;
        }
        #endregion

        #region 加密压缩文件
        /// <summary>
        /// 加密压缩文件
        /// </summary>
        /// <param name="tlpPath">当前模板路径</param>
        /// <param name="type">压缩类型js、css</param>
        /// <returns></returns>
        public static string SiteBundles(string tlpPath, string type)
        {
            DYCache cache = DYCache.GetCacheService();
            string data = cache.RetrieveObject("/DY/Web/" + type + "/min") as string;
            if (data == null)
            {
                System.Web.Optimization.BundleTable.Bundles.IgnoreList.Clear();
                string minPath = "~" + tlpPath + type + "/min";
                string outFilePath = "~" + tlpPath + type + "/bundleszip" + type;
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(minPath)))
                    return "目录不存在";

                //类型压缩加密
                switch (type)
                {
                    case "js": System.Web.Optimization.BundleTable.Bundles.Add(new System.Web.Optimization.ScriptBundle(outFilePath).IncludeDirectory(minPath, "*." + type, false)); break;
                    case "css": System.Web.Optimization.BundleTable.Bundles.Add(new System.Web.Optimization.StyleBundle(outFilePath).IncludeDirectory(minPath, "*." + type, false)); break;
                }
                System.Web.Optimization.BundleTable.EnableOptimizations = true;
                data = System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl(outFilePath);

                cache.AddObject("/DY/Web/" + type + "/min", data);
            }
            return data;

        }
        #endregion
    }
}
