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
using System.Net;
using DY.LanguagePack;

namespace DY.Site
{
    /// <summary>
    /// 缓存网站前台的一些界面HTML数据
    /// </summary>
    public partial class Caches
    {
        string mobile = SiteUtils.IsMobileDevice() ? "" : ""; //手机访问调取and is_mobile=0

        #region 获取域名内容
        public static string HttpGet(string url, Encoding encoding = null)
        {
            WebClient wc = new WebClient();
            wc.Encoding = encoding ?? Encoding.UTF8;
            return wc.DownloadString(url);
        }
        #endregion

        #region url参数增加
        public static String BuildQueryString(Dictionary<String, String> kvp)
        {
            List<String> list = new List<String>();
            foreach (var item in kvp)
            {
                String value = String.Format("{0}", item.Value);
                if (!String.IsNullOrEmpty(value))
                {
                    list.Add(String.Format("{0}={1}", Uri.EscapeDataString(item.Key),
                        Uri.EscapeDataString(value)));
                }
            }
            return String.Join("&", list.ToArray());
        }
        #endregion

        #region Vote (投票)
        /// <summary>
        /// 取得单个投票主题信息
        /// </summary>
        /// <param name="vote_id">ID</param>
        /// <returns></returns>
        public static VoteInfo GetVote(int vote_id)
        {
            return SiteBLL.GetVoteInfo(vote_id);
        }
        public static ArrayList GetVoteTop(int top)
        {
            return SiteBLL.GetVoteAllList("vote_id desc", "top " + top + " *", "");
        }
        public static ArrayList GetVoteOption(int vote_id)
        {
            ArrayList data = SiteBLL.GetVoteOptionAllList("", "vote_id=" + vote_id);
            return data;
        }
        #endregion



        private DataTable cat_dt = new DataTable();
        private DataTable down_cat_dt = new DataTable();
        private DataTable cms_page_dt = new DataTable();
        private DataTable weixin_menu_dt = new DataTable();
        private DataTable config_dt = new DataTable();
        private DataTable admin_menu_dt = new DataTable();
        private DataTable admin_action_dt = new DataTable();
        /// <summary>
        /// 产品分类
        /// </summary>
        /// <returns>返回产品分类列表信息</returns>
        public DataTable GoodsCat()
        {
            return GoodsCat(0, true);
        }

        /// <summary>
        /// 产品分类
        /// </summary>
        /// <returns>返回产品分类列表信息</returns>
        public DataTable GoodsCat(int parent_id)
        {
            return GoodsCat(parent_id, true);
        }

        /// <summary>
        /// 产品分类
        /// </summary>
        /// <returns>返回产品分类列表信息</returns>
        public DataTable GoodsCat(int parent_id, int counts)
        {
            return GoodsCat(parent_id, counts, true);
        }
        /// <summary>
        /// 产品分类
        /// </summary>
        /// <returns></returns>
        public DataTable GoodsCat(int parent_id, bool read_next)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject(CacheKeys.前台商品分类 + "1") as DataTable;
            if (data == null)
            {
                data = Goods.GetGoodsCatAllList();

                cache.AddObject(CacheKeys.前台商品分类 + "1", data);
            }

            //DataTable data = Goods.GetGoodsCatAllList();

            cat_dt = data.Clone();

            AddTree(data.Select("parent_id=" + parent_id, "sort_order desc,cat_id asc"), data, read_next);

            return cat_dt;
        }

        /// <summary>
        /// 产品分类
        /// </summary>
        /// <returns></returns>
        public DataTable GoodsCat(int parent_id, int counts, bool read_next)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject(CacheKeys.前台商品分类 + "n" + counts) as DataTable;
            if (data == null)
            {
                data = Goods.GetGoodsCatAllList(counts, parent_id);

                cache.AddObject(CacheKeys.前台商品分类 + "n" + counts, data);
            }

            //DataTable data = Goods.GetGoodsCatAllList();

            cat_dt = data.Clone();

            AddTree(data.Select("", "sort_order asc,cat_id asc"), data, read_next);

            return cat_dt;
        }

        /// <summary>
        /// 移除商品分类缓存
        /// </summary>
        public void RemoveGoodsCat()
        {
            DYCache cache = DYCache.GetCacheService();
            cache.RemoveObject(CacheKeys.前台商品分类 + "1");
        }



        /// <summary>
        /// 资讯分类
        /// </summary>
        /// <returns></returns>
        public DataTable CMSCat(int parent_id, bool read_next)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject(CacheKeys.前台资讯分类) as DataTable;
            if (data == null)
            {
                data = CMS.GetCMSCatAllList();

                cache.AddObject(CacheKeys.前台资讯分类, data);
            }

            // DataTable data = CMS.GetCMSCatAllList();

            cat_dt = data.Clone();

            AddTree(data.Select("is_show=1 and parent_id=" + parent_id, "sort_order desc,cat_id asc"), data, read_next);

            return cat_dt;
        }

        /// <summary>
        /// 移除CMS分类缓存
        /// </summary>
        public void RemoveCMSCat()
        {
            DYCache cache = DYCache.GetCacheService();
            cache.RemoveObject(CacheKeys.前台资讯分类);
        }

        /// <summary>
        /// 添加到树
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddTree(DataRow[] drs, DataTable dt, bool read_next)
        {
            for (int i = 0; i < drs.Length; i++)
            {
                cat_dt.Rows.Add(drs[i].ItemArray);

                if (read_next)
                {
                    DataRow[] rows = dt.Select("parent_id=" + drs[i]["cat_id"], "sort_order asc,cat_id asc");

                    if (rows.Length > 0)
                    {
                        AddTree(rows, dt, read_next);
                    }
                }
            }
        }
        /// <summary>
        /// 创建订单人名称
        /// </summary>
        /// <param name="consignee"></param>
        /// <returns></returns>
        public string CreateName(string consignee)
        {
            if (consignee.Length > 0)
            {
                return consignee.Substring(0, 1) + " *";
            }
            else
            { return ""; }
        }

        /// <summary>
        /// 手机号码加星号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string MobileStar(string str)
        {
            if (str.Length > 0)
            {
                return str.Substring(0, 4) + "****" + str.Substring(str.Length - 3, 3);
            }
            return str;
        }

        /// <summary>
        /// 移除内部链接缓存
        /// </summary>
        public void InternalLinkRemove()
        {
            DYCache cache = DYCache.GetCacheService();
            cache.RemoveObject("/DY/Web/InternalLink/List");
        }


        /// <summary>
        /// 取得站点地图所有分类
        /// </summary>
        /// <returns></returns>
        public DataTable get_address_list(int parent_id)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("parentid", typeof(int));
            dt.Columns.Add("path", typeof(string));
            dt.Columns.Add("priority", typeof(string));
            dt.Columns.Add("changefreq", typeof(string));
            dt.Columns.Add("isenable", typeof(bool));
            dt.Columns.Add("orderid", typeof(int));
            dt.Columns.Add("lastmod", typeof(DateTime));

            return this.get_address_list(parent_id, dt);
        }

        /// <summary>
        /// 递归取出所有子类，并放在DataTable中保存
        /// </summary>
        /// <param name="parentid">父类ID</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected DataTable get_address_list(int parentid, DataTable dt)
        {
            ArrayList bll_address = SiteBLL.GetWebAddressAllList("orderid asc,id asc", "parentid=" + parentid);
            foreach (WebAddressInfo entity in bll_address)
            {
                string tempLevel = "";
                for (int i = 1; i <= entity.levels; i++)
                {
                    tempLevel += "&nbsp;&nbsp;";
                }

                dt.Rows.Add(new object[] { entity.id, tempLevel + entity.name, entity.parentid, entity.path, entity.priority, entity.changefreq, entity.isenable, entity.orderid, entity.lastmod });

                this.get_address_list(entity.id.Value, dt);
            }

            return dt;
        }
        /// <summary>
        ///资讯分类Table
        /// </summary>
        /// <returns></returns>
        public DataTable CMSCatTable(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Cms/Catf/") as DataTable;
            if (data == null)
            {
                data = CMS.GetCMSCatAllList();

                cache.AddObject("/DY/Web/Cms/Catf", data);
            }
            return data;
        }
        /// <summary>
        /// 格式化单页分类Table
        /// </summary>
        /// <returns></returns>
        public DataTable CMSPageTable(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Cms/Pagef/") as DataTable;
            if (data == null)
            {
                data = CMS.GetCMSPageAllList();

                cache.AddObject("/DY/Web/Cms/Pagef", data);
            }
            return data;
        }
        /// <summary>
        /// 格式化下载分类Table
        /// </summary>
        /// <returns></returns>
        public DataTable DownloadcatTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Cms/downloadcatpid/") as DataTable;
            if (data == null)
            {
                data = DatabaseProvider.GetInstance().GetAllDataToDataTable("download_category", "*", "", "");// CMS.GetCMSPageAllList();

                cache.AddObject("/DY/Web/Cms/downloadcatpid", data);
            }
            return data;
        }
        /// <summary>
        /// 格式化产品分类Table
        /// </summary>
        /// <returns></returns>
        public DataTable Goods_CategoryTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/goods_cat/Category/") as DataTable;
            if (data == null)
            {
                
                data = Goods.GetGoodsCatAllList();
                cache.AddObject("/DY/Web/goods_cat/Category", data);
            }
            return data;
        }

        #region jxjy

        /// <summary>
        /// 产品分类
        /// </summary>
        /// <returns></returns>
        public ArrayList GoodsCaseCat()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject(CacheKeys.前台商品分类 + "CaseCat") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetGoodsCategoryAllList("sort_order desc,cat_id asc", "is_case=1");

                cache.AddObject(CacheKeys.前台商品分类 + "CaseCat", data);
            }
            return data;
        }



        /// <summary>
        /// 取得内部链接
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetGoodsLink(int goods_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/GoodsLink/List") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetGoodsLinkAllList("", "goods_id=" + goods_id);
                cache.AddObject("/DY/Web/GoodsLink/List", data);
            }
            return data;
        }


        public GoodsInfo GetGoodsInfo(int goods_id)
        {
            return SiteBLL.GetGoodsInfo(goods_id);
        }

        #endregion

        #region Nav (主导航 底部导航)

        /// <summary>
        /// 移除主导航缓存
        /// </summary>
        public void RemoveNav()
        {
            DYCache cache = DYCache.GetCacheService();
            cache.RemoveObject(CacheKeys.前台主导航);
        }

        /// <summary>
        /// 移除底部航缓存
        /// </summary>
        public void RemoveFootNav()
        {
            DYCache cache = DYCache.GetCacheService();
            cache.RemoveObject(CacheKeys.前台底部导航);
        }
        #endregion


        #region GetGoods (推荐 新品 热销 特价)

        /// <summary>
        /// 统计商品
        /// </summary>
        /// <param name="topN">显示多少条记录</param>
        /// <returns></returns>
        public ArrayList GetGoodsClick(int topN)
        {

            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Goods/Click/Top" + topN) as ArrayList;
            Goods goods = new Goods();
            if (data == null)
            {
                int ResultCount = 0;

                data = SiteBLL.GetGoodsList(1, topN, "click_count desc,goods_id desc", "is_on_sale=1 and is_delete=0 ", out ResultCount);

                cache.AddObject("/DY/Web/Goods/Click/Top" + topN, data);
            }
            return data;
        }
        #endregion

        #region GetNav (当前位置)

        /// <summary>
        /// 后缀
        /// </summary>
        public static BaseConfigInfo config = BaseConfig.Get();
        public static string suffix = BaseConfig.Get().EnableHtml ? urlrewrite.html_suffix : config.UrlRewriterKzm;
        public static string html = BaseConfig.Get().EnableHtml ? urlrewrite.html : "";

        public static string GoodsNav(int cat_id, string str)
        {
            if (cat_id > 0)
            {
                GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(cat_id);

                str = " &raquo;  <a href='" + html + "/product/" + catinfo.cat_id + "" + suffix + "'>" + catinfo.cat_name + "</a>" + str;
                return GoodsNav(catinfo.parent_id.Value, str);
            }
            return str;


        }

        public static string CmsNav(int cat_id, string str)
        {
            if (cat_id > 0)
            {
                CmsCatInfo catinfo = SiteBLL.GetCmsCatInfo(cat_id);
                string nav_id = string.IsNullOrEmpty(catinfo.urlrewriter) ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter;
                str = "&raquo; <a href='" + html + urlrewrite.article + nav_id + "" + suffix + "'>" + catinfo.cat_name + "</a>" + str;//&raquo;
                return CmsNav(catinfo.parent_id.Value, str);
            }
            return str;
        }
        public static string DownLoadNav(int cat_id, string str)
        {
            if (cat_id > 0)
            {
                DownloadCategoryInfo catinfo = SiteBLL.GetDownloadCategoryInfo(cat_id);
                string nav_id = string.IsNullOrEmpty(catinfo.urlrewriter) ? catinfo.cat_id.Value.ToString() : catinfo.urlrewriter;
                str = " &raquo; <a href='" + html + urlrewrite.download + nav_id + "" + suffix + "'>" + catinfo.cat_name + "</a>" + str;
                return DownLoadNav(catinfo.parent_id.Value, str);
            }
            return str;
        }
        public static string PageNav(int cat_id, string str)
        {
            if (cat_id > 0)
            {
                CmsPageInfo catinfo = SiteBLL.GetCmsPageInfo(cat_id);
                string nav_id = string.IsNullOrEmpty(catinfo.urlrewriter) ? catinfo.page_id.Value.ToString() : catinfo.urlrewriter;
                str = " &raquo; <a href='" + html + urlrewrite.page + nav_id + "" + suffix + "'>" + catinfo.title + "</a>" + str;
                return PageNav(catinfo.parent_id.Value, str);
            }
            return str;
        }
        #endregion

        #region 获取顶层id
        /// <summary>
        /// 获取页面最顶层分类id
        /// </summary>
        /// <param name="parent_id">上级分类id</param>
        /// <param name="cat_id">当前分类id</param>
        /// <returns></returns>
        public static int PageCatID(int parent_id, int cat_id)
        {
            if (parent_id > 0)
            {
                CmsPageInfo catinfo = SiteBLL.GetCmsPageInfo(parent_id);
                return PageCatID(catinfo.parent_id.Value, catinfo.page_id.Value);
            }
            return cat_id;
        }

        /// <summary>
        /// 获取资讯最顶层分类id
        /// </summary>
        /// <param name="parent_id">上级分类id</param>
        /// <param name="cat_id">当前分类id</param>
        /// <returns></returns>
        public static int CMSCatID(int parent_id, int cat_id)
        {
            if (parent_id > 0)
            {
                CmsCatInfo catinfo = SiteBLL.GetCmsCatInfo(parent_id);
                return CMSCatID(catinfo.parent_id.Value, catinfo.cat_id.Value);
            }
            return cat_id;
        }

        /// <summary>
        /// 获取产品最顶层分类id
        /// </summary>
        /// <param name="parent_id">上级分类id</param>
        /// <param name="cat_id">当前分类id</param>
        /// <returns></returns>
        public static int GoodsCatID(int parent_id, int cat_id)
        {
            if (parent_id > 0)
            {
                GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(parent_id);
                return GoodsCatID(catinfo.parent_id.Value, catinfo.cat_id.Value);
            }
            return cat_id;
        }

        /// <summary>
        /// 获取产品最顶层分类id
        /// </summary>
        /// <param name="parent_id">上级分类id</param>
        /// <param name="cat_id">当前分类id</param>
        /// <returns></returns>
        public static int DownloadCatID(int parent_id, int cat_id)
        {
            if (parent_id > 0)
            {
                DownloadCategoryInfo catinfo = SiteBLL.GetDownloadCategoryInfo(parent_id);
                return DownloadCatID(catinfo.parent_id.Value, catinfo.cat_id.Value);
            }
            return cat_id;
        }
        #endregion

        /// <summary>
        /// 获取上一篇下一篇文章
        /// </summary>
        /// <param name="article_id">当前文章ID</param>
        /// <param name="cat_id">当前产品ID</param>
        /// <param name="next">是否为下一条，否则为上一条</param>
        /// <returns></returns>
        public ArrayList GetPreNext(int goods_id, int cat_id, bool next)
        {
            StringBuilder sb = new StringBuilder("cat_id=" + cat_id + " and ");
            int ResultCount = 0;
            string sort = "";
            if (next)
            {
                sb.Append("goods_id>" + goods_id);
                sort = "goods_id asc";
            }
            else
            {
                sb.Append("goods_id<" + goods_id);
                sort = "goods_id desc";
            }

            return SiteBLL.GetGoodsList(1, 1, sort, sb.ToString(), out ResultCount);
        }


        #region 格式化分类
        /// <summary>
        /// 资讯分类
        /// </summary>
        /// <returns>返回产品分类列表信息</returns>
        public DataTable CMSCat()
        {
            return CMSCat(0);
        }
        /// <summary>
        /// 资讯分类
        /// </summary>
        /// <returns></returns>
        public DataTable CMSCat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject(CacheKeys.前台资讯分类) as DataTable;
            if (data == null)
            {
                data = CMS.GetCMSCatAllList();

                cache.AddObject(CacheKeys.前台资讯分类, data);
            }

            //DataTable data = CMS.GetCMSCatAllList();

            cat_dt = data.Clone();

            AddTree(data.Select("parent_id=" + parent_id, "sort_order desc,cat_id asc"), data);

            return cat_dt;
        }


        /// <summary>
        /// 格式化网站功能设置
        /// </summary>
        /// <returns></returns>
        public DataTable ConfigFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Config") as DataTable;
            if (data == null)
            {
                data = SystemConfig.GetConfigAllData();

                cache.AddObject("/DY/Web/Config", data);
            }

            //DataTable data = CMS.GetCMSCatAllList();

            config_dt = data.Clone();

            AddConfigTreeFormat(data.Select("parent_id=" + parent_id, "sort_order desc,id asc"), data, "");

            return config_dt;
        }
        /// <summary>
        /// 功能设置表格
        /// </summary>
        /// <returns></returns>
        public DataTable ConfigTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/ConfigTable/Category/") as DataTable;
            if (data == null)
            {

                data = SystemConfig.GetConfigAllData();
                cache.AddObject("/DY/Web/ConfigTable/Category", data);
            }
            return data;
        }
        /// <summary>
        /// 菜单表格
        /// </summary>
        /// <returns></returns>
        public DataTable AdminMenuTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/AdminMenu/Category/") as DataTable;
            if (data == null)
            {

                data = MenuManage.GetAdminMenuAllList();
                cache.AddObject("/DY/Web/AdminMenu/Category", data);
            }
            return data;
        }
        /// <summary>
        /// 权限表格
        /// </summary>
        /// <returns></returns>
        public DataTable AdminActionTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/AdminAction/Category/") as DataTable;
            if (data == null)
            {

                data = SystemConfig.GetAdminActionAllData();
                cache.AddObject("/DY/Web/AdminAction/Category", data);
            }
            return data;
        }

        /// <summary>
        /// 格式化网站地图
        /// </summary>
        /// <returns></returns>
        public DataTable WebAddressTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Admin/WebAddressTable/") as DataTable;
            if (data == null)
            {
                data = SystemConfig.GetWebAddressData();
                //cache.AddObject("/DY/Web/Admin/WebAddressTable/", data);
            }
            return data;
        }

        /// <summary>
        /// 格式化导航
        /// </summary>
        /// <returns></returns>
        public DataTable NavigatorTable()
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Admin/NavigatorTable/") as DataTable;
            if (data == null)
            {
                data = SystemConfig.GetNavigatorData();
                //cache.AddObject("/DY/Web/Admin/NavigatorTable/", data);
            }
            return data;
        }

        /// <summary>
        /// 格式化资讯分类
        /// </summary>
        /// <returns></returns>
        public DataTable CMSCatFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Cms/Catf/") as DataTable;
            if (data == null)
            {
                data = CMS.GetCMSCatAllList();

                cache.AddObject("/DY/Web/Cms/Catf", data);
            }

            //DataTable data = CMS.GetCMSCatAllList();

            cat_dt = data.Clone();

            AddCMSTreeFormat(data.Select("parent_id=" + parent_id, "sort_order desc,cat_id asc"), data, "");

            return cat_dt;
        }

        /// <summary>
        /// 格式化下载分类
        /// </summary>
        /// <returns></returns>
        public DataTable DownloadCatFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Download/Catf/") as DataTable;
            if (data == null)
            {
                data = Download.GetDownloadCatAllList();

                cache.AddObject("/DY/Web/Download/Catf", data);

            }


            down_cat_dt = data.Clone();

            AddDownloadTreeFormat(data.Select("parent_id=" + parent_id, "sort_order desc,cat_id asc"), data, "");



            return down_cat_dt;
        }

        /// <summary>
        /// 格式化单页分类
        /// </summary>
        /// <returns></returns>
        public DataTable CMSPageFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Cms/Pagef/") as DataTable;
            if (data == null)
            {
                data = CMS.GetCMSPageAllList();

                cache.AddObject("/DY/Web/Cms/Pagef", data);
            }

            cms_page_dt = data.Clone();

            AddCMSPageTreeFormat(data.Select("parent_id=" + parent_id, "order_id desc,page_id asc"), data, "");

            return cms_page_dt;
        }

        /// <summary>
        /// 格式化自定义菜单
        /// </summary>
        /// <returns></returns>
        public DataTable WxMenuFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = CMS.GetWxMenuAllList();

            //DataTable data = CMS.GetCMSCatAllList();

            weixin_menu_dt = data.Clone();

            AddWxMenuTreeFormat(data.Select("parent_id=" + parent_id, "sort_order desc,menu_id asc"), data, "");

            return weixin_menu_dt;
        }

        /// <summary>
        /// 格式化权限列表
        /// </summary>
        /// <returns></returns>
        public DataTable AdminActionFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Admin/Action") as DataTable;

            if (data == null)
            {
                data = SystemConfig.GetAdminActionAllData();

                cache.AddObject("/DY/Web/Admin/Action", data);
            }
            //DataTable data = CMS.GetCMSCatAllList();

            admin_action_dt = data.Clone();

            AddAdminActionTreeFormat(data.Select("parent_id=" + parent_id, "action_id asc"), data, "");

            return admin_action_dt;
        }

        /// <summary>
        /// 格式化网站菜单
        /// </summary>
        /// <returns></returns>
        public DataTable AdminMenuFormat(int parent_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = MenuManage.GetAdminMenuAllList();//cache.RetrieveObject("/DY/Web/Menu/") as DataTable;
            if (data == null)
            {
                data = MenuManage.GetAdminMenuAllList();

                cache.AddObject("/DY/Web/Menu/", data);
            }

            //DataTable data = CMS.GetCMSCatAllList();

            admin_menu_dt = data.Clone();

            AddAdminMenuTreeFormat(data.Select("parent_id=" + parent_id, "menu_id asc"), data, "");

            return admin_menu_dt;
        }

        /// <summary>
        /// 移除资讯分类缓存
        /// </summary>
        public void CMSCatRemove()
        {
            DYCache cache = DYCache.GetCacheService();
            cache.RemoveObject(CacheKeys.前台资讯分类);
        }
        /// <summary>
        /// 添加到树
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddConfigTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = config_dt.NewRow();
                rows["id"] = drs[i]["id"];
                rows["name"] = ats + drs[i]["name"];
                rows["code"] = drs[i]["code"];
                rows["type"] = drs[i]["type"];
                rows["tip"] = drs[i]["tip"];
                rows["size"] = drs[i]["size"];
                rows["store_range"] = drs[i]["store_range"];
                rows["store_dir"] = drs[i]["store_dir"];
                rows["value"] = drs[i]["value"];
                rows["sort_order"] = drs[i]["sort_order"];
                rows["isshow"] = drs[i]["isshow"];
                config_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["id"], "sort_order desc,id asc");

                if (rowsd.Length > 0)
                {
                    AddConfigTreeFormat(rowsd, dt, ats);
                }
            }
        }
        /// <summary>
        /// 添加到树(自定义菜单)
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddWxMenuTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = weixin_menu_dt.NewRow();
                rows["menu_id"] = drs[i]["menu_id"];
                rows["parent_id"] = drs[i]["parent_id"];
                rows["name"] = ats + drs[i]["name"];
                rows["trigger_word"] = drs[i]["trigger_word"];
                rows["sort_order"] = drs[i]["sort_order"];
                rows["enabled"] = drs[i]["enabled"];
                rows["data"] = drs[i]["data"];
                weixin_menu_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["menu_id"], "sort_order desc,menu_id asc");

                if (rowsd.Length > 0)
                {
                    AddWxMenuTreeFormat(rowsd, dt, ats);
                }
            }
        }
        /// <summary>
        /// 添加到树(权限列表)
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddAdminActionTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = admin_action_dt.NewRow();
                rows["action_id"] = drs[i]["action_id"];
                rows["parent_id"] = drs[i]["parent_id"];
                rows["action_code"] = drs[i]["action_code"];
                rows["action_text"] = ats + drs[i]["action_text"];
                rows["isenable"] = drs[i]["isenable"];
                admin_action_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["action_id"], "action_id asc");

                if (rowsd.Length > 0)
                {
                    AddAdminActionTreeFormat(rowsd, dt, ats);
                }
            }
        }
        /// <summary>
        /// 添加到树
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddCMSPageTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = cms_page_dt.NewRow();
                rows["page_id"] = drs[i]["page_id"];
                rows["parent_id"] = drs[i]["parent_id"];
                rows["title"] = ats + drs[i]["title"];
                rows["is_show"] = drs[i]["is_show"];
                rows["order_id"] = drs[i]["order_id"];
                rows["urlrewriter"] = drs[i]["urlrewriter"];
                rows["pagetitle"] = drs[i]["pagetitle"];
                rows["pagekeywords"] = drs[i]["pagekeywords"];
                rows["pagedesc"] = drs[i]["pagedesc"];
                cms_page_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["page_id"], "order_id desc,page_id asc");

                if (rowsd.Length > 0)
                {
                    AddCMSPageTreeFormat(rowsd, dt, ats);
                }
            }
        }

        /// <summary>
        /// 添加到树
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddAdminMenuTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = admin_menu_dt.NewRow();
                rows["menu_id"] = drs[i]["menu_id"];
                rows["parent_id"] = drs[i]["parent_id"];
                rows["name"] = ats + drs[i]["name"];
                rows["link"] = drs[i]["link"];
                rows["type"] = drs[i]["type"];
                rows["isshow"] = drs[i]["isshow"];
                admin_menu_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["menu_id"], "menu_id asc");

                if (rowsd.Length > 0)
                {
                    AddAdminMenuTreeFormat(rowsd, dt, ats);
                }
            }
        }
        /// <summary>
        /// 添加到树
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddCMSTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = cat_dt.NewRow();
                rows["cat_id"] = drs[i]["cat_id"];
                rows["parent_id"] = drs[i]["parent_id"];
                rows["cat_name"] = ats + drs[i]["cat_name"];
                rows["cat_type"] = drs[i]["cat_type"];
                rows["show_in_nav"] = drs[i]["show_in_nav"];
                rows["is_review"] = drs[i]["is_review"];
                rows["cat_path"] = drs[i]["cat_path"];
                rows["cat_level"] = drs[i]["cat_level"];
                rows["sort_order"] = drs[i]["sort_order"];
                rows["page_size"] = drs[i]["page_size"];
                rows["list_tlp"] = drs[i]["list_tlp"];
                rows["info_tlp"] = drs[i]["info_tlp"];
                rows["urlrewriter"] = drs[i]["urlrewriter"];
                rows["is_mobile"] = drs[i]["is_mobile"];
                rows["pagetitle"] = drs[i]["pagetitle"];
                rows["pagekeywords"] = drs[i]["pagekeywords"];
                rows["pagedesc"] = drs[i]["pagedesc"];
                cat_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["cat_id"], "sort_order desc,cat_id asc");

                if (rowsd.Length > 0)
                {
                    AddCMSTreeFormat(rowsd, dt, ats);
                }
            }
        }

        /// <summary>
        /// 添加到树（下载）
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddDownloadTreeFormat(DataRow[] drs, DataTable dt, string ats)
        {
            ats += "&nbsp;&nbsp;&nbsp;&nbsp;";
            for (int i = 0; i < drs.Length; i++)
            {
                //cat_dt.Rows.Add(drs[i].ItemArray);
                DataRow rows = down_cat_dt.NewRow();
                rows["cat_id"] = drs[i]["cat_id"];
                rows["parent_id"] = drs[i]["parent_id"];
                rows["cat_name"] = ats + drs[i]["cat_name"];
                rows["cat_type"] = drs[i]["cat_type"];
                rows["keywords"] = drs[i]["keywords"];
                rows["cat_desc"] = drs[i]["cat_desc"];
                rows["sort_order"] = drs[i]["sort_order"];
                rows["cat_level"] = drs[i]["cat_level"];
                rows["template_file"] = drs[i]["template_file"];
                rows["pagesize"] = drs[i]["pagesize"];
                rows["template_detail_path"] = drs[i]["template_detail_path"];
                rows["urlrewriter"] = drs[i]["urlrewriter"];
                rows["is_review"] = drs[i]["is_review"];
                //rows["pagetitle"] = drs[i]["pagetitle"];
                //rows["pagekeywords"] = drs[i]["pagekeywords"];
                //rows["pagedesc"] = drs[i]["pagedesc"];
                down_cat_dt.Rows.Add(rows);
                DataRow[] rowsd = dt.Select("parent_id=" + drs[i]["cat_id"], "sort_order desc,cat_id asc");

                if (rowsd.Length > 0)
                {
                    AddDownloadTreeFormat(rowsd, dt, ats);
                }
            }
        }
        /// <summary>
        /// 添加到树
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="dt"></param>
        private void AddTree(DataRow[] drs, DataTable dt)
        {
            for (int i = 0; i < drs.Length; i++)
            {
                cat_dt.Rows.Add(drs[i].ItemArray);

                DataRow[] rows = dt.Select("parent_id=" + drs[i]["cat_id"], "sort_order desc,cat_id asc");

                if (rows.Length > 0)
                {
                    AddTree(rows, dt);
                }
            }
        }
        #endregion

    }
}
