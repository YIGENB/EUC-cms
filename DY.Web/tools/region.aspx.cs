using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.tools
{
    public partial class region : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int type = DYRequest.getRequestInt("type");
            int parent = DYRequest.getRequestInt("parent");

            string target = DYRequest.getRequest("target") == null ? "" : DYRequest.getRequest("target");

            string html = "";

            foreach (RegionInfo regioninfo in systemConfig.GetRegion(type,parent))
            {
                html += "{\"region_id\":\"" + regioninfo.region_id + "\",\"region_name\":\"" + regioninfo.region_name + "\"},";
            }

            string template = "{\"regions\":["+ html.TrimEnd(',') +"],\"type\":"+ type +",\"target\":\""+ target +"\"}";

            base.DisplayMemoryTemplate(template);
        }
    }
}
