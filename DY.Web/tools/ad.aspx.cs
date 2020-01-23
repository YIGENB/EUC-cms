using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using System.Data;

namespace DY.Web.tools
{
    public partial class ad : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();


            DataTable adtb = Caches.GetAds(DYRequest.getRequestInt("pid"));
            if (adtb.Rows.Count < 1)
            {
                return;
            }
            context.Add("ads", adtb.Rows);
            context.Add("position_type", adtb.Rows[0]["position_type"]);
            context.Add("context", context);
            base.DisplayTemplate(context, "Ad", "/static/template", false);
        }
    }
}
