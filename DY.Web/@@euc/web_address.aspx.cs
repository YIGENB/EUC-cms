/**
 * 功能描述：WebAddress管理类
 * 创建时间：2010/7/14 23:25:55
 * 最后修改时间：2010/7/14 23:25:55
 * 作者：gudufy
 * 文件标识：2e9fb1da-dd83-4828-a2b7-17448fa19a50
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
using System.IO;
using DY.LanguagePack;

namespace DY.Web.admin
{
    public partial class web_address : AdminPage
    {
        XmlDocument xDoc;

        string stlist = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("sitemap");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("sitemap");

                if (ispost)
                {
                    base.id = SiteBLL.InsertWebAddressInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加站点地图");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("站点地图添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("list", SiteBLL.GetWebAddressAllList("", ""));
                base.DisplayTemplate(context, "web_address/web_address_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("sitemap");

                if (ispost)
                {
                    SiteBLL.UpdateWebAddressInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改web_address");

                    base.DisplayMessage("站点地图修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetWebAddressInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "web_address/web_address_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("sitemap", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateWebAddressFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改站点地图");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("sitemap", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateWebAddressFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("sitemap", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteWebAddressInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除站点地图");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("sitemap", true);

                //执行删除
                SiteBLL.DeleteWebAddressInfo(base.id);

                //日志记录
                base.AddLog("删除站点地图");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 自动生成站点地图
            else if (this.act == "createzd")
            {
                //检测权限
                this.IsChecked("sitemap");
                createSietmap();
            }
            #endregion

            #region 手动生成xml文件和网站地图html
            else if (this.act == "tohtml")
            {
                //检测权限
                this.IsChecked("sitemap");

                #region 生成XML文件
                xDoc = new XmlDocument();

                string filename = Server.MapPath("/sitemap.xml");

                delXml(filename);

                createXml(filename);

                xDoc.Load(filename);



                ArrayList arr = SiteBLL.GetWebAddressAllList("orderid desc", "isenable=1");

                foreach (Entity.WebAddressInfo entity in arr)
                {
                    insertNode(filename, entity.path.ToString(), entity.lastmod.ToString(), entity.changefreq.ToString(), entity.priority.ToString());
                }
                #endregion

                #region 生成html文件
                DataTable dt = caches.get_address_list(0);
                string modelfilename = Server.MapPath("/sitemapmodel.htm");

                //调用配置信息
                string nettitle = "网站地图 -- " + config.Name;
                string netlogo = config.ShopLogo;
                string netzl = config.FootInfo;

                string strlist = "";
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["parentid"].ToString() == "0")
                    {
                        strlist = strlist + "<tr><td align='left'><strong><a href=\"" + dt.Rows[j]["path"].ToString() + "\">" + dt.Rows[j]["name"].ToString().Replace("&nbsp;", "") + "</a></strong></td></tr>";
                        stlist = "";
                        strlist = strlist + tolist(dt, dt.Rows[j]["id"].ToString(), 20, 0);
                    }
                }

                string[] tagArr = { "$contents", "$title" };
                string[] val = { strlist, nettitle };

                //  proHtml(modelfilename, tagArr, val);
                IDictionary context = new Hashtable();

                context.Add("contents", strlist);

                string sitemaphtml = base.GetTemplate(context, "systems/sitemapmodel", DY.Config.BaseConfig.AdminSkinPath, false);

                FileOperate.WriteFile(Server.MapPath("/sitemap.html"), sitemaphtml);
                #endregion

                //生成网站地图
                DY.Site.SiteMap sitemap = new DY.Site.SiteMap();
                #region 生成txt地图
                context.Clear();
                context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
                context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
                context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
                context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
                context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

                string txthtml = base.GetTemplate(context, "systems/txt", DY.Config.BaseConfig.AdminSkinPath, false);

                #endregion
                #region 生成sitemapindex地图
                context.Clear();
                context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
                context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
                context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
                context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
                context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

                string sitemapIndexhtml = base.GetTemplate(context, "systems/sitemapIndex", DY.Config.BaseConfig.AdminSkinPath, false);

                #endregion

                sitemap.createSietmap(sitemaphtml, txthtml, sitemapIndexhtml, config);

                //日志记录
                base.AddLog("生成网站地图");

                //显示提示信息
                base.DisplayMessage("已经成功更新网站地图内容", 2);

                base.DisplayTemplate(context, "systems/sitemap_info");

            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";

            this.GetList("web_address/web_address_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetWebAddressList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected WebAddressInfo SetEntity()
        {
            WebAddressInfo entity = new WebAddressInfo();

            entity.name = DYRequest.getForm("name");
            entity.parentid = DYRequest.getFormInt("parentid");
            entity.path = DYRequest.getForm("path");
            entity.priority = DYRequest.getForm("priority");
            entity.changefreq = DYRequest.getForm("changefreq");
            entity.isenable = DYRequest.getFormBoolean("isenable");
            entity.orderid = DYRequest.getFormInt("orderid");
            entity.lastmod = DYRequest.getFormDateTime("lastmod");
            entity.levels = DYRequest.getFormInt("levels");
            entity.id = base.id;

            return entity;
        }

        /// <summary>
        /// 修改sitemap文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="insertTxt"></param>
        /// <param name="insertPalce"></param>
        public void insertNode(string filepath, string insertTxt1, string insertTxt2, string insertTxt3, string insertTxt4)
        {
            XmlElement node = xDoc.DocumentElement;

            XmlElement newNode = xDoc.CreateElement("url");
            XmlElement subnewNode1 = xDoc.CreateElement("loc");
            XmlElement subnewNode2 = xDoc.CreateElement("lastmod");
            XmlElement subnewNode3 = xDoc.CreateElement("changefreq");
            XmlElement subnewNode4 = xDoc.CreateElement("priority");

            XmlText subnewNodeTxt1 = xDoc.CreateTextNode(insertTxt1);
            XmlText subnewNodeTxt2 = xDoc.CreateTextNode(insertTxt2);
            XmlText subnewNodeTxt3 = xDoc.CreateTextNode(insertTxt3);
            XmlText subnewNodeTxt4 = xDoc.CreateTextNode(insertTxt4);

            subnewNode1.AppendChild(subnewNodeTxt1);
            subnewNode2.AppendChild(subnewNodeTxt2);
            subnewNode3.AppendChild(subnewNodeTxt3);
            subnewNode4.AppendChild(subnewNodeTxt4);

            newNode.AppendChild(subnewNode1);
            newNode.AppendChild(subnewNode2);
            newNode.AppendChild(subnewNode3);
            newNode.AppendChild(subnewNode4);

            node.AppendChild(newNode);

            xDoc.Save(filepath);
        }


        /// <summary>
        /// 创建一个xml文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        public void createXml(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            if (!file.Exists)
            {
                xDoc = new XmlDocument();
                XmlNode xdoctext = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xDoc.AppendChild(xdoctext);
                XmlElement root = xDoc.CreateElement("urlset");
                XmlAttribute rootAtt = xDoc.CreateAttribute("xmlns");
                rootAtt.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
                root.SetAttributeNode(rootAtt);
                XmlText roottext = xDoc.CreateTextNode("");
                root.AppendChild(roottext);
                xDoc.AppendChild(root);
                xDoc.Save(filepath);
            }
        }

        /// <summary>
        /// 删除一个xml文件
        /// </summary>
        /// <param name="filepath">文件路径</param>
        public void delXml(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        /// <summary>
        /// 生成html文件
        /// </summary>
        /// <param name="modelfilename">模板的文件路径</param>
        /// <param name="tag">要换的标记</param>
        /// <param name="value">标记换成的内容</param>
        public void proHtml(string modelfilename, string[] tag, string[] value)
        {
            string strcontent;

            using (StreamReader reader = new StreamReader(modelfilename, System.Text.Encoding.UTF8, false))
            {
                strcontent = reader.ReadToEnd();
                for (int i = 0; i < tag.Length; i++)
                {
                    strcontent = strcontent.Replace(tag[i], value[i]);
                }
            };

            using (StreamWriter writer = new StreamWriter(Server.MapPath("/sitemap.html"), false, System.Text.Encoding.UTF8))
            {
                writer.Write(strcontent);
            };

        }

        /// <summary>
        /// 递归出网站地图的栏目
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strvalue"></param>
        /// <param name="beValue">设置margin-left</param>
        /// <param name="flag">判断是不是第一次调用</param>
        /// <returns></returns>
        public string tolist(DataTable dt, string strvalue, int beValue, int flag)
        {
            DataRow[] rows = dt.Select("parentid=" + strvalue + "", "orderid asc");
            if (rows.Length > 0)
            {
                stlist = stlist + "<tr><td style=\"text-align:left;background-color:#EEEEEE\"><table style=\"margin:0px;padding:0px;margin-left:" + beValue + "px\"><tr>";
                for (int z = 0; z < rows.Length; z++)
                {
                    int t = z + 1;
                    //if (t % 8 == 0)
                    //{
                    //    stlist = stlist + "<td style=\"text-align:left;padding-left:10px;\"><a href=\"" + rows[z][3].ToString() + "\">" + rows[z][1].ToString().Replace("&nbsp;", "") + "</a></td></tr><tr>";
                    //}
                    //else
                    //{
                    stlist = stlist + "<td style=\"text-align:left;padding-left:10px;\"><a href=\"" + rows[z]["path"].ToString() + "\">" + rows[z]["name"].ToString().Replace("&nbsp;", "") + "</a></td>";
                    // }

                    tolist(dt, rows[z]["id"].ToString(), beValue, 1);
                }
                stlist = stlist + "</tr></table></td></tr>";
            }
                stlist = stlist + "</tr>";

            return stlist;
        }

        /// <summary>
        /// 自动生成站点地图
        /// </summary>
        public void createSietmap()
        {
            string filename = Server.MapPath("/sitemap.xml");

            //检测权限
            this.IsChecked("sitemap");

            IDictionary context = new Hashtable();

            //生成网站地图
            DY.Site.SiteMap sitemap = new DY.Site.SiteMap();

            #region 生成html地图
            context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
            context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
            context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
            context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
            context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

            string sitemaphtml = base.GetTemplate(context, "systems/sitemap", DY.Config.BaseConfig.AdminSkinPath, false);

            #endregion
            #region 生成txt地图
            context.Clear();
            context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
            context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
            context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
            context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
            context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

            string txthtml = base.GetTemplate(context, "systems/txt", DY.Config.BaseConfig.AdminSkinPath, false);

            #endregion
            #region 生成sitemapindex地图
            context.Clear();
            context.Add("pages", SiteBLL.GetCmsPageAllList("", "is_show=1"));
            context.Add("productCats", SiteBLL.GetGoodsCategoryAllList("", ""));
            context.Add("newsCats", SiteBLL.GetCmsCatAllList("", ""));
            context.Add("datetime", DateTime.Now.ToString("yyyy-MM-dd"));
            context.Add("newslist", SiteBLL.GetCmsAllList("", ""));

            string sitemapIndexhtml = base.GetTemplate(context, "systems/sitemapIndex", DY.Config.BaseConfig.AdminSkinPath, false);

            #endregion

            sitemap.createSietmap(sitemaphtml, txthtml, sitemapIndexhtml, config);
            //日志记录
            base.AddLog("生成网站地图");

            //显示提示信息
            base.DisplayMessage("已经成功更新网站地图内容", 2);

            context.Add("sitemap", FileOperate.IsExist(filename, FileOperate.FsoMethod.File) ? 1 : 0);

            base.DisplayTemplate(context, "web_address/web_address_list");
        }
    }
}


