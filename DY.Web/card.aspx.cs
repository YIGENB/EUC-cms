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
    public partial class card : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            string code = CShopRequest.getRequest("code","grid");
            int cat_id = CShopRequest.getRequestInt("cat_id",8);
            string filter = "goods_id > 0";
            string cat_name = "";
            string tag = "";
            string tlp = "card";
            string pageString = "";

            //是否推荐热销新品
            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case "new":
                        filter += " and is_new=1";
                        cat_name = "特价卡号";
                        tlp = "card_list";
                        break;
                    case "hot":
                        filter += " and is_hot=1";
                        cat_name = "精选卡号";
                        tlp = "card_list";
                        break;
                    case "best":
                        filter += " and is_best=1";
                        cat_name = "精品卡号";
                        tlp = "card_list";
                        break;
                    case "list":
                        cat_name = "卡号中心";
                        tlp = "card_list";
                        break;
                }
            }

            //是否是全部产品
            if (cat_id!=0)
            {
                GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(cat_id);
                if (catinfo != null)
                {
                    filter += " and cat_id in (" + goods.GetGoodsCatIds(catinfo.cat_id.Value) + ")";
                    if (code != "new" || code != "hot" || code != "best")
                    {
                        cat_name = catinfo.cat_name;
                    }
                    context.Add("catinfo", catinfo);
                    context.Add("goods_cat_sitemap", Goods.GetPrevGoodsCat(cat_id));
                }  
            }

            //是否有关键字
            if (!string.IsNullOrEmpty(tag))
            {
                filter += " and goods_name like '%" + tag + "%'";
            }

            context.Add("list", SiteBLL.GetGoodsList(base.pageindex, 600, "goods_id,goods_name,goods_sn,shop_price,market_price,urlrewriter,goods_thumb,goods_img,is_best,is_new,is_hot", SiteUtils.GetGoodsSortOrder(context, "goods_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetAjaxPageNumbers(base.pageindex, base.ResultCount, 600, "", "", 6, out pageString));
            context.Add("cat_name", cat_name);
            context.Add("cat_id", cat_id);
            context.Add("tag", Server.UrlEncode(tag));

            base.DisplayTemplate(context, tlp);
        }

    }
}
