/**
 * 功能描述：产品首页
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
    public partial class category : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            string code = CShopRequest.getRequest("code","list");
            string tag = CShopRequest.getRequest("tag");
            string k = CShopRequest.getRequest("k");
            int cat_id = CShopRequest.getRequestInt("cat_id",7);
            string list_type = CShopRequest.getRequest("list_type","grid");  //列表显示方式
            string sort_by = CShopRequest.getRequest("sort_by", "sort_order");
            int sort_order_name=config.ShowOrderType;
            string sort_order_value="";
            if(sort_order_name==0){sort_order_value="asc";}else{sort_order_value="asc";}
            string sort_order = CShopRequest.getRequest("sort_order", sort_order_value);
            int brand_id = CShopRequest.getRequestInt("brand_id");
            string price = CShopRequest.getRequest("price","0");
            string type = CShopRequest.getRequest("type", "0");
            string face = CShopRequest.getRequest("face", "0");
            string filter = "goods_id > 0 and is_delete=0";
            string tlp = "category";
            string cat_name = "";
            string pageString = "";
            string pagelinktop = "/category-" + code + "-" + cat_id + "-";
            string pagelinkend = "-" + sort_by + "-" + sort_order+"";
            if (tag != "")
            {
                pagelinkend += "-" + tag + "";
            }
            pagelinkend += "-" + price + "-" + brand_id + "-"+type+"-"+face+"-" + list_type + ".aspx";


            //是否推荐热销新品
            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case "new":
                        filter += " and is_new=1";
                        cat_name = "新品上市";
                        break;
                    case "hot":
                        filter += " and is_hot=1";
                        cat_name = "热销产品";
                        break;
                    case "best":
                        filter += " and is_best=1";
                        cat_name = "推荐产品";
                        break;
                    case "list":
                        cat_name = "产品中心";
                        break;
                }
            }

            //是否是全部产品
            if (cat_id!=0)
            {
                GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(cat_id);
                if (catinfo != null)
                {
                    filter += " and cat_id in (" + goods.GetGoodsAllCatIds(catinfo.cat_id.Value) + ")";
                    cat_name = catinfo.cat_name;
                    context.Add("catinfo", catinfo);
                    context.Add("goods_cat_sitemap", Goods.GetPrevGoodsCat(cat_id));
                
                    #region 生成价格区间（对顶级才生成价格区间）

                        int cat_level = int.Parse(catinfo.cat_level.ToString());
                        if (cat_level == 1)
                        {
                            string pricearea = catinfo.pricearea;
                            if (pricearea != "")
                            {
                                string pricelist = CreatePriceArea(Server.UrlEncode(tag), list_type, sort_by, sort_order, brand_id, price, pagelinktop, pricearea,type,face);
                                string pricelistincat = CreatePriceAreaInCat(Server.UrlEncode(tag), list_type, sort_by, sort_order, brand_id, price, pagelinktop, pricearea,type,face);
                                context.Add("pricelist", pricelist);
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
                                string pricelist = CreatePriceArea(Server.UrlEncode(tag), list_type, sort_by, sort_order, brand_id, price, pagelinktop, pricearea,type,face);
                                string pricelistincat = CreatePriceAreaInCat(tag, list_type, sort_by, sort_order, brand_id, price, pagelinktop, pricearea,type,face);
                                context.Add("pricelist", pricelist);
                                context.Add("pricelistincat", pricelistincat);
                            }
                        }

                  #endregion

                    #region 分页的页码控制
                        if (catinfo.page_size != null && catinfo.page_size.Value != 0)
                            pagesize = catinfo.page_size.Value;
                        else
                            pagesize = config.PageSize;
                    #endregion
                }  
            }

            //是否有关键字
            if (!string.IsNullOrEmpty(tag))
            {
                filter += " and goods_name like '%" + tag + "%'";
            }

            //是否有品牌
            if (brand_id != 0)
            {
                filter+=" and brand_id='"+brand_id+"'";
            }

            //是否有价格区间
            if (!string.IsNullOrEmpty(price))
            {
                int nums = price.IndexOf('~');
                if (nums > 0)
                {
                    string[] numArr = price.Split('~');
                    filter += " and shop_price>=" + numArr[0] + " and shop_price<=" + numArr[1] + "";

                }
                else
                {
                    int numsa = price.IndexOf("以上");
                    if(numsa>0)
                    {
                        string numsValue = price.Replace("以上", "");
                        filter += " and shop_price>=" + numsValue + "";
                    }  
                }

            }

            //是否有手机单双模,手机外形
            if (type != "0")
            {
                if (face != "0")
                {
                    if (goods.GetGoodsIds(int.Parse(type), int.Parse(face)) != "")
                    {
                        filter += " and goods_id in (" + goods.GetGoodsIds(int.Parse(type), int.Parse(face)) + ")";
                    }
                    else
                    {
                        filter += " and goods_id<0";
                    }
                }
                else
                {
                    if (goods.GetGoodsIds(int.Parse(type)) != "")
                    {
                        filter += " and goods_id in (" + goods.GetGoodsIds(int.Parse(type)) + ")";
                    }
                    else
                    {
                        filter += " and goods_id<0";
                    }
                }
            }
            else
            {
                if (face != "0")
                {
                    if (goods.GetGoodsIds(int.Parse(face)) != "")
                    {
                        filter += " and goods_id in (" + goods.GetGoodsIds(int.Parse(face)) + ")";
                    }
                    else
                    {
                        filter += " and goods_id<0";
                    }
                }
            }

            #region 生成品牌

                ArrayList gbrand = goods.GetGoodsBrand();
                string allbrandlink = pagelinktop + "1-" + sort_by + "-" + sort_order + "";
                if (tag != "")
                {
                    allbrandlink += "-" + tag + "";
                }
                
                allbrandlink += "-"+price+"-0-" + list_type + ".aspx";

                string branklist="<div class=\"cpp\"><strong>品牌：</strong>";

                if (brand_id == 0)
                {
                    branklist+="<a href=\"" + allbrandlink + "\" class=\"cur\">全部</a>";
                }
                else
                {
                    branklist+="<a href=\"" + allbrandlink + "\">全部</a>";
                }

                foreach (GoodsBrandInfo gbi in gbrand)
                {
                   string  brankurlstr=pagelinktop + "1-" + sort_by + "-" + sort_order + "";
                   if (tag != "")
                   {
                      brankurlstr += "-" + tag + "";
                   }

                   brankurlstr += "-"+price+"-"+gbi.brand_id+"-" + list_type + ".aspx";

                   if (brand_id == gbi.brand_id)
                   {
                       branklist += "<a href=\"" + brankurlstr + "\" class=\"cur\">" + gbi.brand_name + "</a>";
                   }
                   else
                   {
                       branklist += "<a href=\"" + brankurlstr + "\">" + gbi.brand_name + "</a>";
                   }
                }

                branklist += "</div>";

                context.Add("branklist", branklist);

            #endregion

            context.Add("list", SiteBLL.GetGoodsList(base.pageindex, pagesize, "goods_id,goods_name,goods_sn,shop_price,market_price,urlrewriter,goods_thumb,goods_img,is_best,is_new,is_hot", SiteUtils.GetGoodsSortOrder(context, "" + sort_by + " " + sort_order + ""), filter, out base.ResultCount));
            context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, pagesize, pagelinktop, pagelinkend, 6, out pageString));
            context.Add("cat_name", cat_name);
            context.Add("cat_id", cat_id);
            context.Add("top_cat_id", goods.GetTopCatId(cat_id));
            context.Add("list_type", list_type);
            context.Add("pageString", pageString);
            context.Add("tag", Server.UrlEncode(tag));
            context.Add("specList", SiteBLL.GetGoodsSpecAllList("", ""));

            #region 排序,显示方式

                string orderlist = "";

                #region 价格
                    string ordershopprice = pagelinktop + "1-shop_price-" + sort_order + "";
                    if (tag != "")
                    {
                        ordershopprice += "-" + tag + "";
                    }

                    ordershopprice += "-" + price + "-" + brand_id + "-" + list_type + ".aspx";

                    string shopprice = "";

                    if (sort_by == "shop_price")
                    {
                        shopprice = shopprice + "<a href=\"" + ordershopprice + "\" class=\"cur\">价格</a>";
                    }
                    else
                    {
                        shopprice = shopprice + "<a href=\"" + ordershopprice + "\">价格</a>";
                    }
                #endregion

                #region 添加时间
                    string orderaddtime = pagelinktop + "1-add_time-" + sort_order + "";
                    if (tag != "")
                    {
                        orderaddtime += "-" + tag + "";
                    }
                    orderaddtime += "-" + price + "-" + brand_id + "-" + list_type + ".aspx";

                    string addtime = "";

                    if (sort_by == "add_time")
                    {
                        addtime = addtime + "<a href=\"" + orderaddtime + "\" class=\"cur\">添加时间</a>";
                    }
                    else
                    {
                        addtime = addtime + "<a href=\"" + orderaddtime + "\">添加时间</a>";
                    }

                #endregion

                #region 自定排序

                    string ordersortorder = pagelinktop + "1-sort_order-" + sort_order + "";
                    if (tag != "")
                    {
                        ordersortorder += "-" + tag + "";
                    }
                    ordersortorder += "-" + price + "-" + brand_id + "-" + list_type + ".aspx";

                    string sortorder = "";

                    if (sort_by == "sort_order")
                    {
                        sortorder = sortorder + "<a href=\"" + ordersortorder + "\" class=\"cur\">默认排序</a>";
                    }
                    else
                    {
                        sortorder = sortorder + "<a href=\"" + ordersortorder + "\">默认排序</a>";
                    }

                #endregion

                #region 显示方式

                if (list_type == "list")
                {
                    tlp = "category-v";
                }
                else
                {
                    tlp = "category";
                }

                string waylist = pagelinktop + "1-"+sort_by+"-" + sort_order + "";
                if (tag != "")
                {
                    waylist += "-" + tag + "";
                }
                waylist += "-" + price + "-" + brand_id + "-list.aspx";

                string waygrid = pagelinktop + "1-" + sort_by + "-" + sort_order + "";
                if (tag != "")
                {
                    waygrid += "-" + tag + "";
                }
                waygrid += "-" + price + "-" + brand_id + "-grid.aspx";

                #endregion

                orderlist += "<div class=\"cppx\"><span class=\"s1\">共<span class=\"s2\">" + base.ResultCount + "</span>件商品</span>排序方式：" + shopprice + "" + addtime + "" + sortorder + "<span class=\"s3\">显示方式：<a href=\"" + waygrid + "\"><img src=\"img/1.gif\" /></a><a href=\"" + waylist + "\"><img src=\"img/2.gif\" /></a></span></div>";

                context.Add("orderlist", orderlist);

            #endregion

            base.DisplayTemplate(context, tlp);
        }

        /// <summary>
        /// 放在列表上面的价格区间
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
        private static string CreatePriceArea(string tag, string list_type, string sort_by, string sort_order, int brand_id, string price, string pagelinktop, string pricearea,string type,string face)
        {
            string alllink = pagelinktop + "1-" + sort_by + "-" + sort_order + "";
            if (tag != "")
            {
                alllink += "-" + tag + "";
            }
            alllink += "-0-" + brand_id + "-"+type+"-"+face+"-" + list_type + ".aspx";

            string attrlist = "<div class=\"cpjg\"><strong>价格：</strong>";

            if (price == "0")
            {
                attrlist += "<a href=\"" + alllink + "\" class=\"cur\">全部</a>";
            }
            else
            {
                attrlist += "<a href=\"" + alllink + "\">全部</a>";
            }


            string[] attrArr = pricearea.Split(',');

            for (int j = 0; j < attrArr.Length; j++)
            {
                string urlstr = pagelinktop + "1-" + sort_by + "-" + sort_order + "";

                if (tag != "")
                {
                    urlstr += "-" +tag + "";
                }

                urlstr += "-" + attrArr[j] + "-" + brand_id + "-" + type + "-" + face + "-" + list_type + ".aspx";

                if (price == attrArr[j])
                {
                    attrlist = attrlist + "<a href=\"" + urlstr + "\" class=\"cur\">" + attrArr[j] + "</a>";
                }
                else
                {
                    attrlist = attrlist + "<a href=\"" + urlstr + "\">" + attrArr[j] + "</a>";
                }

            }

            attrlist += "</div>";
            return attrlist;
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
        private static string CreatePriceAreaInCat(string tag, string list_type, string sort_by, string sort_order, int brand_id, string price, string pagelinktop, string pricearea,string type,string face)
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
