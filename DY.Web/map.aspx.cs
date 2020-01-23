/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web
{
    public partial class map : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IDictionary context = new Hashtable();
            int id = DYRequest.getRequestInt("id");
            LbsInfo lbs = new LbsInfo();
            lbs = SiteBLL.GetLbsInfo(id);
            #region SEO
            if (!string.IsNullOrEmpty(lbs.title))
            {
                pagetitle = lbs.title + "-" +lbs.tel+ "-" + config.Title;
            }
            else
            {
                pagetitle = config.Title;
            }

            //if (string.IsNullOrEmpty(catinfo.pagekeywords))
            //{
                pagekeywords = config.Keywords;
            //}
            //else
            //{
            //    pagekeywords = catinfo.pagekeywords;
            //}

            if (string.IsNullOrEmpty(lbs.des))
            {
                pagedesc = config.Desc;
            }
            else
            {
                pagedesc = lbs.des;
            }
            #endregion
            if (lbs != null)
            {
                string mtit = lbs.title;
                string mcon = lbs.des;
                string center = lbs.xposition + "," + lbs.yposition;
                string zoom = DYRequest.getRequest("zoom");
                string tel = lbs.tel;
                string addr = lbs.address;
                string pic = lbs.pic;
                bool devicestat = SiteUtils.IsMobileDevice();
                context.Add("stat", devicestat);
                context.Add("mtit", mtit);
                context.Add("pic", pic);
                context.Add("tel", tel);
                context.Add("addr", addr);
                context.Add("mcon", mcon);
                context.Add("center", center);
                context.Add("zoom", zoom);
                context.Add("lbs", lbs);
                context.Add("pagetitle", pagetitle);
                context.Add("pagekeywords", pagekeywords);
                context.Add("pagedesc", pagedesc);
            }
            ///map.aspx?mtit=魔法屋【所有设置为演示版，请修改】&mcon=人民广场&center=118.343683,35.068229&zoom=15
            base.DisplayTemplate(context, "map", "static/template", false);
        }
    }
}