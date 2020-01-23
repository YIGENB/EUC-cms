using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Adapters;

namespace DY.UrlRewriter
{
    /// <summary>
    /// ControlAdapter for rewriting form actions
    /// </summary>
    public class FormRewriterControlAdapter : ControlAdapter
    {
        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(new RewriteFormHtmlTextWriter(writer));
        }
    }
}
