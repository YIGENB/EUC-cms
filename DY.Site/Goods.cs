/**
 * 功能描述：商品相关类
 * 创建时间：2010-1-29 15:17:25
 * 最后修改时间：2010-1-29 15:17:25
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 文件名：Goods.cs
 * ID：fa304b76-21b9-405d-a574-2b877543e079
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 商品相关类
    /// </summary>
    public class Goods
    {
        #region 静态函数
        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="tags">标签列表</param>
        public static void SaveTag(string[] tags)
        {
            foreach (string str in tags)
            {
                if (SiteBLL.GetTagAllList("", "tag_name='" + str + "'").Count <= 0)
                {
                    TagInfo taginfo = new TagInfo(0, 1, str, "", FunctionUtils.Text.ConvertSpellFirst(str));

                    SiteBLL.InsertTagInfo(taginfo);
                }
            }
        }
        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        public static void UpdateGoodsGallery(int goods_id, string img_desc, int img_id)
        {
            DatabaseProvider.GetInstance().UpdateGoodsGallery(goods_id, img_desc, img_id);
        }
        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        public static void UpdateGoodsGallery(int goods_id, string img_desc, int img_id, int order_id)
        {
            DatabaseProvider.GetInstance().UpdateGoodsGallery(goods_id, img_desc, img_id, order_id);
        }
        /// <summary>
        /// 插入商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        public static void InsertGoodsGallery(GoodsGalleryInfo entity)
        {
            DatabaseProvider.GetInstance().InsertGoodsGalleryInfo(entity);
        }
        /// <summary>
        /// 删除商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        public static void DeletedGoodsGallery(string where)
        {
            SiteBLL.DeleteGoodsGalleryInfo(where);
        }
        /// <summary>
        /// 删除临时上传且未更新所属商品的图片信息
        /// </summary>
        /// <param name="user_id"></param>
        public static void DeleteGoodsGalleryByTemp(int user_id)
        {
            DatabaseProvider.GetInstance().DeleteGoodsGalleryByTemp(user_id);
        }
        /// <summary>
        /// 删除商品相关属性值
        /// </summary>
        /// <param name="goods_id"></param>
        public static void DeleteGoodsAttrValueByGoodsId(int goods_id)
        {
            DatabaseProvider.GetInstance().DeleteGoodsAttrValueByGoodsId(goods_id);
        }
        /// <summary>
        /// 删除资讯相关属性值
        /// </summary>
        /// <param name="article_id"></param>
        public static void DeleteGoodsAttrValueByNewsId(int article_id)
        {
            DatabaseProvider.GetInstance().DeleteGoodsAttrValueByNewsId(article_id);
        }
        /// <summary>
        /// 获取商品会员等级价格信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public static DataTable GetGoodsUserRankPrice(int goods_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsUserRankPrice(goods_id);
        }
        /// <summary>
        /// 删除商品相关会员等级价格
        /// </summary>
        /// <param name="goods_id"></param>
        public static void DeleteGoodsUserRankPriceByGoodsId(int goods_id)
        {
            DatabaseProvider.GetInstance().DeleteGoodsUserRankPriceByGoodsId(goods_id);
        }
        /// <summary>
        /// 获取商品扩展分类
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public static ArrayList GetGoodsOtherCat(int goods_id)
        {
            return SiteBLL.GetGoodsCatAllList("", "goods_id=" + goods_id);
        }
        /// <summary>
        /// 获取商品表的所有字段
        /// </summary>
        /// <returns></returns>
        public static DataColumnCollection GetGoodsColumns()
        {
            return DatabaseProvider.GetInstance().GetTableColumns("goods").Columns;
        }
        /// <summary>
        /// 添加到收藏夹
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id_value"></param>
        public static int SaveFavorites(int user_id, int id_value)
        {
            return DatabaseProvider.GetInstance().SaveFavorites(user_id, 1, id_value);
        }
        /// <summary>
        /// 获取商品收藏夹
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static DataTable GetGoodsFavorites(int user_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsFavorites(user_id);
        }
        /// <summary>
        /// 删除收藏夹信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        public static void DeleteFavorites(int user_id, int id)
        {
            DatabaseProvider.GetInstance().DeleteFavorites(user_id, id);
        }
        /// <summary>
        /// 更新订单商品数量（在原来的数量上加一）
        /// </summary>
        /// <param name="order_id"></param>
        /// <param name="goods_id"></param>
        public static void UpdateOrderGoodsNumber(int order_id, int goods_id)
        {
            DatabaseProvider.GetInstance().UpdateOrderGoodsNumber(order_id, goods_id);
        }
        /// <summary>
        /// 获取当前分类的所有父类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public static ArrayList GetPrevGoodsCat(int cat_id)
        {
            ArrayList list = new ArrayList();

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPrevGoodsCat(cat_id))
            {
                while (sdr.Read())
                {
                    GoodsCategoryInfo catinfo = new GoodsCategoryInfo();
                    catinfo.cat_id = sdr.GetInt32(0);
                    catinfo.cat_name = sdr.GetString(1);

                    list.Add(catinfo);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取goods信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGoodsFullInfo(string urlrewriter)
        {
            return DatabaseProvider.GetInstance().GetGoodsFullInfo(urlrewriter);
        }
        /// <summary>
        /// 获取goods信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGoodsFullInfo(int goods_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsFullInfo(goods_id);
        }
        /// <summary>
        /// 插入商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        public static int InsertGoodsCategory(GoodsCategoryInfo catinfo)
        {
            return DatabaseProvider.GetInstance().InsertGoodsCategory(catinfo);
        }
        /// <summary>
        /// 更新商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        public static int UpdateGoodsCategory(GoodsCategoryInfo catinfo)
        {
            return DatabaseProvider.GetInstance().UpdateGoodsCategory(catinfo);
        }
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="cat_id"></param>
        public static int DeleteGoodsCategory(int cat_id)
        {
            return DatabaseProvider.GetInstance().DeleteGoodsCategory(cat_id);
        }
        /// <summary>
        /// 移动商品分类位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public static int MoveGoodsCategoryPos(string act, int cat_id)
        {
            return DatabaseProvider.GetInstance().MoveGoodsCategoryPos(act, cat_id);
        }
        /// <summary>
        /// 移动商品位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public static int MoveGoodsPos(string act, int goods_id)
        {
            return DatabaseProvider.GetInstance().MoveGoodsPos(act, goods_id);
        }
        #endregion

        #region 非静态函数
        /// <summary>
        /// 取得所有商品分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGoodsCatAllList()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("goods_category", "*", "", "");
        }

        /// <summary>
        /// 取得所有商品分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGoodsCatAllList(int counts, int parent_id)
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("goods_category", "top " + counts + " *", "", "parent_id=" + parent_id + "");
        }

        /// <summary>
        /// 取得商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ArrayList GetGoodsGalleryList(object goods_id)
        {
            return SiteBLL.GetGoodsGalleryAllList("order_id desc", "goods_id=" + goods_id);
        }

        /// <summary>
        /// 取得商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public ArrayList GetGoodsGalleryList(object goods_id, object user_id)
        {
            return SiteBLL.GetGoodsGalleryAllList("order_id desc", "goods_id=" + goods_id + " and admin_user_id=" + user_id);
        }

        /// <summary>
        /// 取得商品类型信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetGoodsTypeList()
        {
            return SiteBLL.GetGoodsTypeAllList("cat_id asc", "");
        }
        /// <summary>
        /// 取得商品类型信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetGoodsTypeLists()
        {
            return SiteBLL.GetGoodsTypeAllList("cat_id asc", "attr_type=0");
        }
        /// <summary>
        /// 取得商品分类信息
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public ArrayList GetGoodsCatList(int parent_id)
        {
            return SiteBLL.GetGoodsCategoryAllList("sort_order asc", parent_id >= 0 ? "parent_id=" + parent_id + " and is_show=1" : "");
        }
        /// <summary>
        /// 获取商品热门分类
        /// </summary>
        /// <returns></returns>
        public ArrayList GetHotGoodsCatList()
        {
            return SiteBLL.GetGoodsCategoryAllList("sort_order asc", "is_hot=1 and is_show=1");
        }
        /// <summary>
        /// 获取规格属性列表
        /// </summary>
        /// <param name="spec_type"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public DataRowCollection GetSpecValues(int spec_type, object goods_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsSpecs(spec_type, Utils.StrToInt(goods_id, 0)).Rows;
        }

        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsAttr(int goods_id, int type_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsAttr(goods_id, type_id);
        }
        /// <summary>
        /// 获取资讯属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public DataTable GetNewsAttr(int goods_id, int type_id)
        {
            return DatabaseProvider.GetInstance().GetNewsAttr(goods_id, type_id);
        }
        public ArrayList GetGoodsSpec(int spec_value_id)
        {
            return SiteBLL.GetGoodsSpecIndexAllList("id desc", "spec_value_id=" + spec_value_id + "");
        }

        public ArrayList GetGoodsSpec(int spec_value_id, int spec_value_ida, bool flag)
        {
            return SiteBLL.GetGoodsSpecIndexAllList("id desc", "spec_value_id in (" + spec_value_id + "," + spec_value_ida + ")");
        }

        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public DataTable GetGoodsAttribute(int attr_id)
        {
            return DatabaseProvider.GetInstance().GetGoodsAttribute(attr_id);
        }


        /// <summary>
        /// 获取商品品牌信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetGoodsBrand()
        {
            return SiteBLL.GetGoodsBrandAllList("", "");
        }

        /// <summary>
        /// 获取商品品牌信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetGoodsBrand(int top)
        {
            int ResultCount = 0;
            return SiteBLL.GetGoodsBrandList(1, top, "*", "sort_order desc,brand_id desc", "", out ResultCount);
        }

        /// <summary>
        /// 获取当前分类下的所有子类Id，包括当前分类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public string GetGoodsCatIds(int cat_id)
        {
            string ids = "";
            foreach (GoodsCategoryInfo catinfo in GetGoodsCatList(cat_id))
            {
                ids += catinfo.cat_id + ",";
            }

            return ids + cat_id;
        }

        /// <summary>
        /// 获取当前分类下的所有子类Id以及子类下的子类，包括当前分类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        string idsall = "";
        public string GetGoodsAllCatIds(int cat_id)
        {
            string sql = "SELECT a.cat_id FROM " + DY.Config.BaseConfig.TablePrefix + "goods_category a,Goods_Cid(" + cat_id + ") b WHERE a.cat_id=b.ID";
            //foreach (GoodsCategoryInfo catinfo in GetGoodsCatList(cat_id))
            //{
            //    if (catinfo.parent_id.Value != 0)
            //    {
            //        idsall += catinfo.cat_id + ",";
            //        GetGoodsAllCatIds(catinfo.cat_id.Value);
            //    }
            //    else
            //    {
            //        idsall += catinfo.cat_id + ",";
            //    }
            //}

            //return idsall + cat_id;
            return sql;
        }


        public string GetGoodsIds(int spec_value_id)
        {
            string ids = "";
            foreach (GoodsSpecIndexInfo ginfo in GetGoodsSpec(spec_value_id))
            {
                ids += ginfo.goods_id + ",";
            }
            if (ids != "")
            {
                ids = ids.Substring(0, ids.Length - 1);
            }
            else
            {
                ids = "";
            }
            return ids;
        }

        public string GetGoodsIds(int spec_value_id, int spec_value_ida)
        {
            string ids = "";
            foreach (GoodsSpecIndexInfo ginfo in GetGoodsSpec(spec_value_id, spec_value_ida, true))
            {
                ids += ginfo.goods_id + ",";
            }
            if (ids != "")
            {
                ids = ids.Substring(0, ids.Length - 1);
            }
            else
            {
                ids = "";
            }
            return ids;
        }

        /// <summary>
        /// 得到父类的CatID
        /// </summary>
        int returncatid = 0;
        public int GetTopCatId(int cat_id)
        {
            GoodsCategoryInfo CCI = SiteBLL.GetGoodsCategoryInfo(cat_id);
            if (CCI.parent_id == 0)
            {
                returncatid = cat_id;
            }
            else
            {
                GetTopCatId(CCI.parent_id.Value);
            }
            return returncatid;
        }

        public GoodsInfo getGoodsInfo(int goods_id)
        {
            return SiteBLL.GetGoodsInfo(goods_id);
        }

        #region 关联商品操作类
        /// <summary>
        /// JSON生成商品关联数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string Get_json_goods_list(string filter)
        {
            string opt = "{\"error\":0,\"message\":\"\",\"content\":[";

            foreach (GoodsInfo goodsinfo in SiteBLL.GetGoodsAllList("goods_id desc", "goods_id > 0 and is_delete=0 and is_on_sale=1 " + filter))
            {
                opt += "{\"value\":\"" + goodsinfo.goods_id + "\",\"text\":\"" + goodsinfo.goods_name + "\",\"data\":\"" + goodsinfo.shop_price + "\"},";
            }

            opt = opt.TrimEnd(',') + "]}";

            return opt;
        }

        #endregion
        /// <summary>
        /// 获取上一篇下一篇文章
        /// </summary>
        /// <param name="article_id">当前文章ID</param>
        /// <param name="cat_id">当前文章分类ID</param>
        /// <param name="next">是否为下一条，否则为上一条</param>
        /// <returns></returns>
        public ArrayList GetPreNext(int article_id, int cat_id, bool next)
        {
            StringBuilder sb = new StringBuilder("cat_id=" + cat_id + " and ");
            int ResultCount = 0;
            string sort = "";
            if (next)
            {
                sb.Append("goods_id>" + article_id);
                sort = "goods_id asc";
            }
            else
            {
                sb.Append("goods_id<" + article_id);
                sort = "goods_id desc";
            }

            return SiteBLL.GetGoodsList(1, 1, sort, sb.ToString(), out ResultCount);
        }
        /// <summary>
        /// 取得最近浏览
        /// </summary>
        /// <returns></returns>
        public ArrayList GoodsHistory(int top)
        {
            #region 最近浏览

            ArrayList list = new ArrayList();

            string cookieName = Cookies.SetShopViewCookie();
            ShowViewCollection SVC = Cookies.CookieToShopView(cookieName);
            //BaseConfigInfo config = new BaseConfigInfo();
            //string revalue = "";
            if (SVC != null)
            {
                StringBuilder sb = new StringBuilder();
                //int i = 0;
                //sb.Append("<dl class=\"njl\">");


                foreach (ShopViewInfo svi in SVC.ShowView)
                {
                    GoodsInfo GoodsInfo = new GoodsInfo();
                    GoodsInfo.goods_name = svi.Goodsname;
                    GoodsInfo.market_price = Convert.ToDecimal(svi.Marketprice);
                    GoodsInfo.shop_price = Convert.ToDecimal(svi.Shopprice);
                    GoodsInfo.urlrewriter = svi.Urlpath;
                    GoodsInfo.goods_img = svi.Imgpath;
                    GoodsInfo.goods_id = svi.Goodsid;

                    list.Add(GoodsInfo);



                    //string gname = svi.Goodsname;
                    //string shoppri = svi.Shopprice;
                    //string imgpath = svi.Imgpath;
                    //int gid = svi.Goodsid;
                    //string urlrewriter = svi.Urlpath;

                    //string urlpath = "";

                    //if (config.EnableHtml)
                    //{
                    //    urlpath = "/goods/" + urlrewriter + ".html";
                    //}
                    //else
                    //{
                    //    urlpath = "/goods/detail/" + gid + ".aspx";
                    //}

                    //sb.AppendFormat(" <dt><a  href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"85\" height=\"85\"/></a></dt><dd><b>{2}</b></dd><dd><span>参考价：<a>{3}</a> 元/幅</span></dd>", urlpath, imgpath, gname, shoppri);

                    //i++;

                    // if (i >= top)
                    //break;
                }

                //sb.Append("</dl><br class=\"clear\" />");

                //revalue = sb.ToString();
            }
            //return revalue;
            if (list == null || list.Count == 0)
            {
                return null;
            }

            ArrayList list2 = new ArrayList();
            int count = list.Count;
            int j = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (j < top)
                {
                    list2.Add(list[i]);
                }
                j++;
            }

            return list2;

            #endregion
        }
    }
        #endregion


}
