/**
 * 功能描述：解决方案页
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
using System.Text;

using DY.Common;
using DY.Site;
using DY.Entity;


namespace DY.Web
{
    public partial class cart : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            context.Add("discount_list", Store.GetDiscountList());

            #region 最近浏览
            string cookieName = Cookies.SetShopViewCookie();
            ShowViewCollection SVC = Cookies.CookieToShopView(cookieName);
            string revalue="";
            if (SVC != null)
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                sb.Append("<ul>");

                foreach (ShopViewInfo svi in SVC.ShowView)
                {

                    string gname = svi.Goodsname;
                    string shoppri = svi.Shopprice;
                    string imgpath = svi.Imgpath;
                    int gid = svi.Goodsid;
                    string urlrewriter = svi.Urlpath;

                    string urlpath = "";

                    string ids = urlrewriter;
                    if (ids == "") { ids = gid.ToString(); }

                    if (config.EnableHtml)
                    {
                        urlpath = "/html/goods/" + ids + ".html";
                    }
                    else
                    {
                        urlpath = "/goods/detail/" + ids + ".aspx";
                    }

                    sb.AppendFormat(" <li><a class=\"pic\" href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a> <a class=\"desc\" href=\"{0}\" target=\"_blank\">{2}</a><span class=\"price\">{3}</span> </li>", urlpath, imgpath, gname, shoppri);

                    i++;

                    if (i>4)
                    break;
                }
               
                sb.Append("</ul>");

                revalue=sb.ToString();
            }

            
            if (revalue == "<ul></ul>")
            {
                revalue = "";
            }

            context.Add("showview", revalue);

            #endregion

            #region 添加商品到购物车
            if (base.act == "add_to_cart")
            {
                AddToCart(context);
            }
            #endregion

            #region 改变购物车中商品的数量
            else if (base.act == "change_number")
            {
                this.ChangeNumber(context);
            }
            #endregion

            #region 删除购物车中的商品
            else if (base.act == "remove_cart_goods")
            {
                this.RemoveCartGoods(context);
            }
            #endregion

            #region 显示迷你购物车
            else if (base.act == "mini_cart")
            {
                this.ShowMiniCart(context);
            }
            #endregion

            #region 显示购物车
            else
            {
                ShowCart(context);
            }
            #endregion

        }
        /// <summary>
        /// 迷你购物车
        /// </summary>
        /// <param name="context"></param>
        private void ShowMiniCart(IDictionary context)
        {
            CashFacade cf = new CashFacade();
            decimal goods_amount = Store.SumCartGoodsPrice();

            context.Add("cart_count_goods", Store.GetCartSumGoods());
            context.Add("cart_count_price",cf.GetFactTotal(Store.SumCartGoodsPrice()));  
            base.DisplayTemplate(context, "store/mini_cart", base.isajax);
        }
        /// <summary>
        /// 显示购物车
        /// </summary>
        /// <param name="context"></param>
        protected void ShowCart(IDictionary context)
        {
            CashFacade cf = new CashFacade();
            decimal goods_amount = Store.SumCartGoodsPrice();

            context.Add("goods_amount",goods_amount);
            context.Add("discount", goods_amount - cf.GetFactTotal(Store.SumCartGoodsPrice()));
            context.Add("cartlist", Store.GetCartList());
            context.Add("fav_list", Goods.GetGoodsFavorites(base.userid).Rows);
            base.DisplayTemplate(context, "store/cart",base.isajax);
        }
        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        protected void AddToCart(IDictionary context)
        {
            int ddd = DYRequest.getRequestInt("goods_number");
            int rec_id = Store.AddToCart(
                            DYRequest.getRequestInt("goods_id"),
                            DYRequest.getRequestInt("parent_id"),
                            DYRequest.getRequest("goods_attr"),
                            DYRequest.getRequest("goods_attr_id"),
                            DYRequest.getRequestInt("goods_number"),
                            base.userid);

            if (base.isajax)
            {
                int err = 0;
                string message = "";
                if (rec_id == 0)
                {
                    err = 1;
                    message = "添加至购物车失败，请联系客服以协助解决，谢谢。";
                }

                base.DisplayMemoryTemplate(base.MakeJson("", err, message));
            }
            else
            {
                Response.Redirect("/cart.aspx");
            }
        }
        /// <summary>
        /// 改变购物车中商品的数量
        /// </summary>
        /// <param name="context"></param>
        protected void ChangeNumber(IDictionary context)
        {
            int rec_id = DYRequest.getRequestInt("rec_id");
            int number = DYRequest.getRequestInt("number",1);
            SiteBLL.UpdateCartFieldValue("goods_number", number, rec_id);
            CartInfo CI=SiteBLL.GetCartInfo("rec_id="+rec_id+"");
            int integral = CI.give_integral.Value.ToString() == "" ? 0 : CI.give_integral.Value;
            int integralnum = integral * number;
            SiteBLL.UpdateCartFieldValue("give_integral", integralnum, rec_id);
            this.ShowCart(context);
        }
        /// <summary>
        /// 删除购物车中的商品
        /// </summary>
        /// <param name="context"></param>
        protected void RemoveCartGoods(IDictionary context)
        {
            int rec_id = DYRequest.getRequestInt("rec_id");

            SiteBLL.DeleteCartInfo(rec_id);

            this.ShowCart(context);
        }
    }
}
