/**
 * 功能描述：产品详细页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class product_detail : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string catnav = "";
            string tlp = "goods-detail";
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");
            DataTable goodsinfo = Goods.GetGoodsFullInfo(code);
            if (goodsinfo.Rows.Count == 0)
                goodsinfo = Goods.GetGoodsFullInfo(Utils.StrToInt(code, 0));

            if (goodsinfo.Rows.Count > 0)
            {
                DataRow dr = goodsinfo.Rows[0];

                if (string.IsNullOrEmpty(dr["pagetitle"].ToString()))
                {
                    pagetitle = dr["goods_name"] + "-" + config.ProTitle;
                }
                else
                {
                    pagetitle = dr["pagetitle"].ToString();
                }

                if (string.IsNullOrEmpty(dr["pagekeywords"].ToString()))
                {
                    pagekeywords = dr["goods_name"].ToString() + "," + config.ProKeywords;
                }
                else
                {
                    pagekeywords = dr["pagekeywords"].ToString();
                }

                if (string.IsNullOrEmpty(dr["pagedesc"].ToString()))
                {
                    pagedesc = string.IsNullOrEmpty(dr["goods_desc"].ToString()) ? config.ProDesc : dr["goods_desc"].ToString();
                }
                else
                {
                    pagedesc = dr["pagedesc"].ToString();
                }

                if (!string.IsNullOrEmpty(dr["info_tlp"].ToString()))
                {
                    tlp = SiteUtils.CheckTlp(tlp, dr["info_tlp"].ToString());
                }

                catnav = Caches.GoodsNav(Convert.ToInt32(dr["cat_id"]), "");
                context.Add("catnav", catnav);

                context.Add("goodsinfo", dr);
                context.Add("comment_type", 1);
                context.Add("id_value", dr[0]);
                //context.Add("catinfo", SiteBLL.GetGoodsCategoryInfo(Convert.ToInt32(dr["cat_id"])));


                int parent_id = 0;
                int cat_id = Convert.ToInt32(dr["cat_id"]);
                if (cat_id > 0)
                {
                    //通过3级分类查询2级分类
                    GoodsCategoryInfo goodscategoryinfo = SiteBLL.GetGoodsCategoryInfo(cat_id);
                    if (goodscategoryinfo != null)
                    {
                        parent_id = goodscategoryinfo.parent_id.Value;
                        if (goodscategoryinfo.cat_level.Value == 3)
                        {
                            GoodsCategoryInfo gcInfo = SiteBLL.GetGoodsCategoryInfo(parent_id);
                            parent_id = gcInfo.parent_id.Value;
                        }
                    }
                }

                if (parent_id == 0)
                {
                    parent_id = cat_id;
                }

                GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(string.Format("cat_id={0}", cat_id));
                int catid = Caches.GoodsCatID(catinfo.parent_id.Value, catinfo.cat_id.Value);//catinfo.parent_id > 0 ? catinfo.parent_id.Value : catinfo.cat_id.Value;
                //导航id
                switch (catid)
                {
                    case 1: navid = "36"; break;
                    case 8: navid = "2"; break;
                }


                int id = 0;
                id = Utils.StrToInt(code, 0);
                GoodsInfo goodsinf = new GoodsInfo();
                if (id > 0)
                {
                    goodsinf = SiteBLL.GetGoodsInfo(id);
                }

                context.Add("cat_name", SiteBLL.GetGoodsCategoryValue("cat_name", "cat_id=" + catid));
                context.Add("en_cat_name", catinfo.cat_name_en);
                context.Add("catinfo", catinfo);
                context.Add("cat_id", catid);
                context.Add("this_id", catinfo.cat_id.Value);
                //string ip = DYRequest.GetIP();
                //ArrayList visitlist = SiteBLL.GetGoodsVisitStatAllList("visit_time desc", "*", "visit_ip='" + ip + "'");
                //string goodsids = "";
                //if (visitlist.Count > 0)
                //{
                //    goodsids += "goods_id in (";
                //    for (int i = 0; i < visitlist.Count; i++)
                //    {
                //        GoodsVisitStatInfo visitinfo = (GoodsVisitStatInfo)visitlist[i];
                //        if (i == 0) goodsids += visitinfo.goods_id.ToString();
                //        else goodsids += "," + visitinfo.goods_id.ToString();
                //    }
                //    goodsids += "2)";
                //}
                //else
                //{
                //    goodsids += "2=1";
                //}
                //ResultCount = 0;
                //ArrayList goodsvisitlist = SiteBLL.GetGoodsList(1, 4, "sort_order desc,goods_id desc", goodsids, out ResultCount);
                //context.Add("goodsvisitlist", goodsvisitlist);
                //更新访问统计
                //SiteBLL.UpdateGoodsFieldValue("click_count", Convert.ToInt32(dr["click_count"]) + 1, Convert.ToInt16(dr[0]));
                ////添加访问记录
                //SiteBLL.InsertGoodsVisitStatInfo(new GoodsVisitStatInfo(0, Convert.ToInt16(dr[0]), Request.UrlReferrer == null ? "直接访问" : Request.UrlReferrer.PathAndQuery, Utils.GetIP(), DateTime.Now));

                ////添加访问记录到Cookie\

                //string cookieName = Cookies.SetShopViewCookie();
                //ShowViewCollection SVC = Cookies.CookieToShopView(cookieName);

                //if (SVC != null)
                //{
                //    SVC.AddViewItem(Utils.StrToInt(dr["goods_id"], 0), Utils.StrToInt(dr["goods_id"], 0), dr["goods_img"].ToString(), dr["shop_price"].ToString(), dr["market_price"].ToString(), dr["goods_name"].ToString(), dr["urlrewriter"].ToString());
                //    Cookies.ShopViewToCookie(SVC, cookieName);
                //}
                //else
                //{
                //    SVC = new ShowViewCollection();
                //    SVC.AddViewItem(Utils.StrToInt(dr["goods_id"], 0), Utils.StrToInt(dr["goods_id"], 0), dr["goods_img"].ToString(), dr["shop_price"].ToString(), dr["market_price"].ToString(), dr["goods_name"].ToString(), dr["urlrewriter"].ToString());
                //    Cookies.ShopViewToCookie(SVC, cookieName);
                //}

                #region 显示城市推广信息
                pagetitle = CityStation.ReplaceCityStationName(pagetitle);
                pagekeywords = CityStation.ReplaceCityStationName(pagekeywords);
                #endregion
                base.DisplayTemplate(context, tlp);
            }
            else  //页面不存在
            {

            }
        }
    }
}
