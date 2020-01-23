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
    public partial class card_detail : WebPage
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
                context.Add("cat_name",SiteBLL.GetGoodsCategoryInfo(goodsinfo.cat_id.Value).cat_name);

                //添加访问记录
                SiteBLL.InsertGoodsVisitStatInfo(new GoodsVisitStatInfo(0, goodsinfo.goods_id.Value, Request.UrlReferrer == null ? "直接访问" : Request.UrlReferrer.PathAndQuery, Utils.GetIP(), DateTime.Now));

                base.DisplayTemplate(context, "card-detail");
            }
            else  //页面不存在
            { 
            
            }
        }
    }
}
