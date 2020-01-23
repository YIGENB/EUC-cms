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
    public partial class brand_goods : WebPage
    {
        protected string body = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //string tlp = "brand_info";
            //IDictionary context = new Hashtable();
            //string code = CShopRequest.getRequest("code");
            //if (string.IsNullOrEmpty(code))
            //{
            //    code = "brand";
            //}
            //int id = 0;
            //id = Utils.StrToInt(code, 0);
            //GoodsBrandInfo brandinfo = new GoodsBrandInfo();
            //if (id > 0)
            //{
            //    brandinfo = SiteBLL.GetGoodsBrandInfo(id);
            //}

            //if (brandinfo != null)
            //{
            //    context.Add("info", brandinfo);
            //    base.DisplayTemplate(context, tlp);
            //}
            //else  //页面不存在
            //{

            //}

            IDictionary context = new Hashtable();
            int code = CShopRequest.getRequestInt("code");
            if (code <= 0)
            {
                return;
            }

            string filter = "brand_id=" + code;
            string tlp = "brand-goods";
            string cat_name = "";
            string catltp = "";
            pagetitle = config.ProTitle;
            pagekeywords = config.ProKeywords;
            pagedesc = config.ProDesc;
            int ipagesize = config.PageSize;

            int catid = 0;

            //if (!string.IsNullOrEmpty(code))
            //{

            //    //if (!string.IsNullOrEmpty(catinfo.pagetitle))
            //    //{
            //    //    pagetitle = catinfo.pagetitle;
            //    //}

            //    //if (!string.IsNullOrEmpty(catinfo.pagekeywords))
            //    //{
            //    //    pagekeywords = catinfo.pagekeywords;
            //    //}

            //    //if (!string.IsNullOrEmpty(catinfo.pagedesc))
            //    //{
            //    //    pagedesc = catinfo.pagedesc;
            //    //}
            //    //if (!string.IsNullOrEmpty(catinfo.list_tlp))
            //    //    tlp = catinfo.list_tlp;

            //}
            context.Add("brand_name", SiteBLL.GetGoodsBrandInfo(code).brand_name);
            
            context.Add("list", SiteBLL.GetGoodsList(base.pageindex, ipagesize, SiteUtils.GetSortOrder("goods_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetWebPageNumbers3(base.ResultCount, ipagesize, base.pageindex, "/product/" + code + "/", ".aspx", 6));
          
            context.Add("cat_name", cat_name);
            context.Add("catid", catid);
            context.Add("catltp", catltp);
            base.DisplayTemplate(context, tlp);
        }
    }
}
