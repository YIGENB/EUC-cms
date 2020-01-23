/**
 * 功能描述：Oauth管理类
 * 创建时间：2014/3/6 11:48:36
 * 最后修改时间：2014/3/6 11:48:36
 * 作者：gudufy
 * 文件标识：111c6f2b-297e-4ed6-be22-db9982f0b42d
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
using System.Xml;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK;

namespace DY.Web.admin
{
    public partial class oauth_post : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string msg = DYRequest.getRequest("msg");//Request["msg"];
            string text = DYRequest.getRequest("text");
        }
    }
}


