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
    public partial class form : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();


            DataTable formtb = Caches.GetForm(DYRequest.getRequestInt("pid"));
            if (formtb.Rows.Count < 1)
            {
                return;
            }
            context.Add("form", formtb.Rows);
            context.Add("pid", DYRequest.getRequestInt("pid"));
            base.DisplayTemplate(context, "public/Form");
        }
    }
}
