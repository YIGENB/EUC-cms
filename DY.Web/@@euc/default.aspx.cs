/**
 * 功能描述：首页
 * 创建时间：2010-1-29 12:43:46
 * 最后修改时间：2010-1-29 12:43:46
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
using System.Text;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class _default : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (base.act)
            { 
                case "logout":
                    SiteUtils.ClearUserCookie("DYAdmin");

                    Response.Redirect("login.aspx");
                    break;
                case "get_quick_menu":
                    GetQuickMenu();
                    break;
                case"add_quick_menu":
                    AddQuickMenu();
                    break;
                case "skin":
                    Skin();
                    break;
                default:
                    IDictionary context = new Hashtable();
                    //context.Add("menuString", CreatMenu());
                    context.Add("username", base.username);
                    context.Add("userid", base.userid);
                    //context.Add("menuTop", CreatMenuTop());
                    context.Add("message_wck", SiteBLL.GetFeedbackAllList("", "is_show=0").Count);
                    context.Add("email_wck", SiteBLL.GetEmailListAllList("", "stat=0").Count);
                    base.DisplayTemplate(context, "default");
                    break;
            }
        }

        /// <summary>
        /// 保存后台皮肤
        /// </summary>
        /// <param name="mp_id"></param>
        /// <returns></returns>
        protected void Skin()
        {
            string skinname = DYRequest.getFormString("name");
            skinname = string.IsNullOrEmpty(skinname) ? "默认" : skinname;
            Utils.SaveConfig("AdminSkin", DYRequest.getFormString("skin"));
            base.DisplayMemoryTemplate(base.MakeJson("", 1, "已保存为：" + skinname + "主题,重启主机生效！"));
        }



        /// <summary>
        /// 取得微信菜单
        /// </summary>
        /// <param name="mp_id"></param>
        /// <returns></returns>
        protected string GetWeCatMenu(string mp_id)
        {
            StringBuilder wecatmenu = new StringBuilder();
            #region 微信菜单
            foreach (DataRow dr in MenuManage.GetWeCatMenu().Rows)
            {
                wecatmenu.Append(string.Format("<li><a href='{0}&pid={3}' target='mainFrame' title='{1}'>{2}</a></li>", dr["link"].ToString(), dr["menutitle"].ToString(), dr["menutitle"].ToString(), mp_id));
            }
            #endregion
            return wecatmenu.ToString();
        }

        /// <summary>
        /// 取得快捷菜单
        /// </summary>
        protected void GetQuickMenu()
        {
            StringBuilder sb = new StringBuilder("");
            int i = 0;
            sb.Append("<tr>");
            foreach (QuickMenuInfo dr in SiteBLL.GetQuickMenuAllList("quickmenuorderid desc,id asc", "user_id=" + base.userid))
            {
                if (i % 2 == 0&&i!=0)
                    sb.Append("</tr><tr>	");
                sb.Append("<td class=\"able \"><a href=\"" + dr.quickmenulink + "\" class=\"name\"><span class=\"icon\"></span>" + dr.quickmenuname + "</a></td>");
                i++;
            }
            sb.Append("</tr>");
            base.DisplayMemoryTemplate(base.MakeJson(sb.ToString(),0,""));
        }
        protected void AddQuickMenu()
        {
            string name = DYRequest.getRequest("name"),
                url = DYRequest.getRequest("url");

            //判断是否存在
            if (SiteBLL.GetQuickMenuAllList("", "quickmenuname='" + name + "'").Count <= 0)
            {
                QuickMenuInfo menuinfo = new QuickMenuInfo();
                menuinfo.creattime = DateTime.Now;
                menuinfo.creatuser = base.username;
                menuinfo.isenabled = true;
                menuinfo.lastupdateip = "";
                menuinfo.lastupdatetime = DateTime.Now;
                menuinfo.lastupdateuser = "";
                menuinfo.quickmenulink = url;
                menuinfo.quickmenuname = name;
                menuinfo.quickmenuorderid = 0;
                menuinfo.siteid = 0;
                menuinfo.sitelanguage = "";
                menuinfo.user_id = base.userid;
                SiteBLL.InsertQuickMenuInfo(menuinfo);
            }

            base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
        }

        
    }
}
