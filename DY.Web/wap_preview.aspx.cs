/**
 * 功能描述：首页
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
using DY.Config;

namespace DY.Web
{
    public partial class _wap_preview : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_UnLoad(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.Session.Remove("theme");
        }
    }
}
