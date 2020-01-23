/**
 * 功能描述：资讯首页
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
    public partial class help : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            navid = "0";
            int id = CShopRequest.getRequestInt("id");
            if (id == 0) { id = 184; }
            CmsInfo cmsinfo = null;
            if (id > 0)
                cmsinfo = SiteBLL.GetCmsInfo(id);

            IDictionary context = new Hashtable();
            context.Add("id", id);
            context.Add("cmsinfo", cmsinfo);
            if (cmsinfo != null)
                context.Add("catinfo", SiteBLL.GetCmsCatInfo(cmsinfo.cat_id.Value));

            base.DisplayTemplate(context, "help");
        }
    }
}
