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
    public partial class brand_list : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            IDictionary context = new Hashtable();
            string code = CShopRequest.getRequest("code");
            string filter = "brand_id > 0";
            int ipagesize = 7;
            context.Add("list", SiteBLL.GetGoodsBrandList(base.pageindex, ipagesize, SiteUtils.GetSortOrder("sort_order desc,brand_id desc"), filter, out base.ResultCount));


            string url = string.IsNullOrEmpty(code) ? "/brand/p" : "/brand/" + code + "/p";

            context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, ipagesize, base.pageindex, url, ".aspx", 6));
       
            base.DisplayTemplate(context, "brand");

        }
    }
}
