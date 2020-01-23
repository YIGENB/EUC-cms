/**
 * 功能描述：main
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

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.Config;

namespace DY.Web.admin
{
    public partial class tlp : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 网站模板
            if (this.act == "site")
            {
                TlpSite();
            } 
            #endregion

            #region 修改网站模板
            else if (this.act == "edit")
            {
                TlpSiteEdit();
            }
            #endregion

            #region 邮件模板
            else if (this.act == "email")
            {
                TlpEmail();
            }
            #endregion

            #region 风格style
            else if (this.act == "color")
            {
                TlpColor();
            }
            #endregion
        }
        /// <summary>
        /// 网站模板
        /// </summary>
        protected void TlpSite()
        {
            base.IsChecked("edit_site_tlp");

            IDictionary context = new Hashtable();
            context.Add("list", FileOperate.getDirectoryAllInfos(Server.MapPath(BaseConfig.WebSkinPath), FileOperate.FsoMethod.File, "*" + BaseConfig.WebSkinSuffix).Rows);

            base.DisplayTemplate(context, "template/site_list");
        }
        /// <summary>
        /// 修改网站模板
        /// </summary>
        protected void TlpSiteEdit()
        {
            base.IsChecked("edit_site_tlp");

            if (ispost)
            {
                FileOperate.WriteFile(Server.MapPath(DYRequest.getRequest("path")), DYRequest.getForm("filecontent"));

                Response.Redirect(Request.Url.PathAndQuery);
            }

            IDictionary context = new Hashtable();
            context.Add("fileinfo", FileOperate.ReadFile(Server.MapPath(DYRequest.getRequest("path"))));
            context.Add("list", FileOperate.getDirectoryAllInfos(Server.MapPath(BaseConfig.WebSkinPath), FileOperate.FsoMethod.File, "*" + BaseConfig.WebSkinSuffix).Rows);
            context.Add("path", DYRequest.getRequest("path"));

            base.DisplayTemplate(context, "template/edit");
        }
        /// <summary>
        /// 邮件模板
        /// </summary>
        protected void TlpEmail()
        {
            base.IsChecked("edit_email_tlp");

            if (ispost)
            {
                MailTemplatesInfo mailtlpinfo = new MailTemplatesInfo();
                mailtlpinfo.is_html = DYRequest.getFormBoolean("is_html");
                mailtlpinfo.last_modify = DateTime.Now;
                mailtlpinfo.tlp_content = DYRequest.getForm("tlp_content");
                mailtlpinfo.tlp_id = base.id;
                mailtlpinfo.tlp_subject = DYRequest.getForm("tlp_subject");

                SiteBLL.UpdateMailTemplatesInfo(mailtlpinfo);

                Response.Redirect("tlp.aspx?act=email&id="+base.id);
            }

            IDictionary context = new Hashtable();
            context.Add("fileinfo",SiteBLL.GetMailTemplatesInfo(base.id));
            context.Add("list", SiteBLL.GetMailTemplatesAllList("",""));

            base.DisplayTemplate(context, "template/edit_mail_tlp");
        }
        /// <summary>
        /// 风格style
        /// </summary>
        protected void TlpColor()
        {
            base.IsChecked("edit_color_tlp");
            string message = "";

            if (ispost)
            {
                base.id = DYRequest.getFormInt("checked_id");
                //SiteBLL.UpdateMStyleFieldValue("is_checked", "1", base.id);
                foreach (MStyleInfo mstyleinfo in SiteBLL.GetMStyleAllList("", ""))
                {
                    if (mstyleinfo.id == base.id)
                    {
                        mstyleinfo.is_checked = true;
                        mstyleinfo.id = base.id;
                        Utils.SaveConfig("WapSkinPath", "/mobile/" + mstyleinfo.skin_path + "/");
                        message += "已切换为：" + mstyleinfo.style_name + "风格";
                    }
                    else
                        mstyleinfo.is_checked = false;
                    SiteBLL.UpdateMStyleInfo(mstyleinfo);
                }
                base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
                //base.DisplayMemoryTemplate(base.MakeJson("", 0, "切换风格成功"));
                //mstyleinfo.top_bg_color = DYRequest.getFormString("top_bg_color");
                //mstyleinfo.top_search_bg_color = DYRequest.getFormString("top_search_bg_color");
                //mstyleinfo.menu_bg_color = DYRequest.getFormString("menu_bg_color");
                //mstyleinfo.in_menu_bg_color = DYRequest.getFormString("in_menu_bg_color");
                //mstyleinfo.menu_font_color = DYRequest.getFormString("menu_font_color");
                //mstyleinfo.in_menu_font_color = DYRequest.getFormString("in_menu_font_color");
                //mstyleinfo.b_menu_bg_color = DYRequest.getFormString("b_menu_bg_color");
                //mstyleinfo.b_in_menu_bg_color = DYRequest.getFormString("b_in_menu_bg_color");
                //mstyleinfo.b_menu_font_color = DYRequest.getFormString("b_menu_font_color");
                //mstyleinfo.b_in_menu_font_color = DYRequest.getFormString("b_in_menu_font_color");
                //mstyleinfo.bg_color = DYRequest.getFormString("bg_color");
                //mstyleinfo.content_color = DYRequest.getFormString("content_color");
                //mstyleinfo.content_link_color = DYRequest.getFormString("content_link_color");
                //mstyleinfo.list_bg_color = DYRequest.getFormString("list_bg_color");
                //mstyleinfo.title_font_color = DYRequest.getFormString("title_font_color");
                //mstyleinfo.title_content_font_color = DYRequest.getFormString("title_content_font_color");
                //mstyleinfo.foot_bg_color = DYRequest.getFormString("foot_bg_color");
                //mstyleinfo.foot_font_color = DYRequest.getFormString("foot_font_color");
                //mstyleinfo.in_foot_font_color = DYRequest.getFormString("in_foot_font_color");
                //mstyleinfo.is_checked = DYRequest.getFormBoolean("is_checked");
                //mstyleinfo.date = DateTime.Now;

                //SiteBLL.UpdateMStyleInfo(mstyleinfo);

                //Response.Redirect("tlp.aspx?act=email&id=" + base.id);
            }

            IDictionary context = new Hashtable();
            context.Add("fileinfo", SiteBLL.GetMStyleInfo(base.id));
            context.Add("list", SiteBLL.GetMStyleAllList("sort_order asc", ""));

            base.DisplayTemplate(context, "template/style_tlp");
        }
    }
}
