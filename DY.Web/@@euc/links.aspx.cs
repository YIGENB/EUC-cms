using System;
using System.Collections;
using System.Data;
using System.Web;

using CShop.Common;
using CShop.Site;
using CShop.Entity;

namespace CShop.Web.admin
{
    public partial class links : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.act == "list")
            {
                tm = getTemplate("systems/links_list.htm");

                tm.SetValue("list", _config.get_links_list(""));
            }

            if (this.act == "add")
            {
                try
                {
                    if (!_config.bll_links.Exists("link_keyword='" + getForm("link_keyword").Trim() + "'"))
                    {
                        CShop_Links entity = new CShop_Links();
                        entity.link_keyword = getForm("link_keyword").Trim();
                        entity.link_url = getForm("link_url").Trim();
                        entity.link_des = getForm("link_des").Trim();
                        _config.bll_links.Add(entity);
                    }

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    Response.End();
                }

                Response.Redirect("links.aspx?act=list");
            }

            if (this.act == "remove")
            {
                //执行删除
                _config.bll_links.Delete(getRequestInt("id"));

                //显示提示信息
                tm = getTemplateString(this.make_json_result("", "", null));
            }

            else if (this.act == "edit_link_keyword")
            {
                tm = getTemplateString(this.edit_link_keyword());
            }

            else if (this.act == "edit_link_des")
            {
                tm = getTemplateString(this.edit_link_des());
            }

            else if (this.act == "edit_link_url")
            {
                tm = getTemplateString(this.edit_link_url());
            }

            Response.Write(tm.Process());
            Response.End();
        }

        private string edit_link_url()
        {
            CShop_Links entity = new CShop_Links();
            entity.link_id = getFormInt("id");
            entity.link_url = getForm("val");
            _ad.bll_ad.Update(entity);

            return this.make_json_result(getForm("val"), "", null);
        }

        private string edit_link_des()
        {
            CShop_Links entity = new CShop_Links();
            entity.link_id = getFormInt("id");
            entity.link_des = getForm("val");
            _ad.bll_ad.Update(entity);

            return this.make_json_result(getForm("val"), "", null);
        }

        private string edit_link_keyword()
        {
            CShop_Links entity = new CShop_Links();
            entity.link_id = getFormInt("id");
            entity.link_keyword = getForm("val");
            _ad.bll_ad.Update(entity);

            return this.make_json_result(getForm("val"), "", null);
        }
    }
}
