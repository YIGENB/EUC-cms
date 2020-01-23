/**
 * 功能描述：产品详细页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using CShop.Common;
using CShop.Site;
using CShop.Entity;

namespace CShop.Web
{
    public partial class goods : WebPage
    {
        /// <summary>
        /// 定义本页hashtable以供模板引擎使用
        /// </summary>
        protected IDictionary context = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = CShopRequest.getRequest("code");

            #region 获取商品信息
            GoodsInfo goodsinfo = SiteBLL.GetGoodsInfo(string.Format("urlrewriter='{0}'", code));
            if (goodsinfo == null)
                goodsinfo = SiteBLL.GetGoodsInfo(Utils.StrToInt(code, 0)); 
            #endregion

            if (goodsinfo != null)
            {
                context.Add("goodsinfo", goodsinfo);
                context.Add("shipping_list", SiteBLL.GetDeliveryAllList("", ""));
                context.Add("comment_type", 1);
                context.Add("id_value", goodsinfo.goods_id);
                context.Add("products", SiteBLL.GetProductsAllList("", "goods_id=" + goodsinfo.goods_id));
                context.Add("cat_name", SiteBLL.GetGoodsCategoryInfo(goodsinfo.cat_id.Value).cat_name);
                context.Add("specList", SiteBLL.GetGoodsSpecAllList("", ""));
                context.Add("goodsLinkcount", SiteBLL.GetGoodsLinkAllList("goods_id", "goods_id=" + goodsinfo.goods_id + "").Count);

                int cat_id = goodsinfo.cat_id.Value;

                //是否是全部产品
                if (cat_id != 0)
                {
                    GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(cat_id);
                    string pagelinktop = "/category-list-" + cat_id + "-";
                    if (catinfo != null)
                    {
                        #region 生成价格区间（对顶级才生成价格区间）

                        int cat_level = int.Parse(catinfo.cat_level.ToString());
                        if (cat_level == 1)
                        {
                            string pricearea = catinfo.pricearea;
                            if (pricearea != "")
                            {
                                string pricelistincat = CreatePriceAreaInCat("", "grip", "sort_order", "desc", 0, "0", pagelinktop, pricearea, "0", "0");
                                context.Add("pricelistincat", pricelistincat);
                            }
                        }
                        else
                        {
                            string cat_path = catinfo.cat_path;
                            int cat_ida = int.Parse(cat_path.Split('|')[0].ToString());
                            GoodsCategoryInfo catinfoa = SiteBLL.GetGoodsCategoryInfo(cat_ida);
                            string pricearea = catinfoa.pricearea;
                            if (pricearea != "")
                            {
                                string pricelistincat = CreatePriceAreaInCat("", "grip", "sort_order", "desc", 0, "0", pagelinktop, pricearea, "0", "0");
                                context.Add("pricelistincat", pricelistincat);
                            }
                        }

                        #endregion
                    }
                }

                //添加访问记录
                SiteBLL.InsertGoodsVisitStatInfo(new GoodsVisitStatInfo(0, goodsinfo.goods_id.Value, Request.UrlReferrer == null ? "直接访问" : Request.UrlReferrer.PathAndQuery, Utils.GetIP(), DateTime.Now));

                //添加Cookie中
                string cookieName = Cookies.SetShopViewCookie();
                ShowViewCollection SVC = Cookies.CookieToShopView(cookieName);
                if (SVC != null)
                {
                    SVC.AddViewItem(goodsinfo.goods_id.Value, goodsinfo.goods_id.Value, goodsinfo.goods_img, goodsinfo.shop_price.ToString(), goodsinfo.goods_name, goodsinfo.urlrewriter);
                    Cookies.ShopViewToCookie(SVC, cookieName);
                }
                else
                {
                    SVC = new ShowViewCollection();
                    SVC.AddViewItem(goodsinfo.goods_id.Value, goodsinfo.goods_id.Value, goodsinfo.goods_img, goodsinfo.shop_price.ToString(), goodsinfo.goods_name, goodsinfo.urlrewriter);
                    Cookies.ShopViewToCookie(SVC, cookieName);
                }

                base.DisplayTemplate(context, "goods_show");
            }
            else  //页面不存在
            {

            }
        }

        /// <summary>
        /// 放在类别上的价格区间
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="list_type"></param>
        /// <param name="sort_by"></param>
        /// <param name="sort_order"></param>
        /// <param name="brand_id"></param>
        /// <param name="price"></param>
        /// <param name="pagelinktop"></param>
        /// <param name="pricearea"></param>
        /// <returns></returns>
        private static string CreatePriceAreaInCat(string tag, string list_type, string sort_by, string sort_order, int brand_id, string price, string pagelinktop, string pricearea, string type, string face)
        {

            string attrlist = "";

            string[] attrArr = pricearea.Split(',');

            for (int j = 0; j < attrArr.Length; j++)
            {
                string urlstr = pagelinktop + "1-" + sort_by + "-" + sort_order + "";

                if (tag != "")
                {
                    urlstr += "-" + tag + "";
                }

                urlstr += "-" + attrArr[j] + "-" + brand_id + "-" + type + "-" + face + "-" + list_type + ".aspx";

                attrlist = attrlist + "<span><a href=\"" + urlstr + "\">" + attrArr[j] + "</a>/</span>";

            }
            return attrlist;
        }
    }
}
