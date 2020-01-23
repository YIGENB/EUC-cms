/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
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
    public partial class brand : WebPage
    {
        protected string body = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            IDictionary context = new Hashtable();
            int code = CShopRequest.getRequestInt("code");
            if (code <= 0)
            {
                return;
            }

            string filter = "brand_id=" + code;
            string tlp = "brand-detail";
            string cat_name = "";
            string catltp = "";
            pagetitle = config.ProTitle;
            pagekeywords = config.ProKeywords;
            pagedesc = config.ProDesc;
            int ipagesize = config.PageSize;

            GoodsBrandInfo brandinfo = new GoodsBrandInfo();
            
            brandinfo = SiteBLL.GetGoodsBrandInfo(code);

            if (brandinfo != null)
            {
                context.Add("info", brandinfo);
                context.Add("list", SiteBLL.GetGoodsList(base.pageindex, ipagesize, SiteUtils.GetSortOrder("sort_order desc,goods_id desc"), filter, out base.ResultCount));
                context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, ipagesize, base.pageindex, "/product/" + code + "/", ".aspx", 6));

                base.DisplayTemplate(context, tlp);
            }
            else  //页面不存在
            {

            }
        }
    }
}
