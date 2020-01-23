/**
 * 功能描述：Cms管理类
 * 创建时间：2010-1-29 12:51:56
 * 最后修改时间：2010-1-29 12:51:56
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
using CookComputing.XmlRpc;
using DY.LanguagePack;

namespace DY.Web.admin
{
    public partial class cms : AdminPage
    {
        /// <summary>
        /// 定义本页hashtable以供模板引擎使用
        /// </summary>
        protected IDictionary context = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("cms_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("cms_add");


                if (ispost)
                {
                    CmsInfo cmsinfo = this.SetEntity();
                    string urlr = DYRequest.getFormString("urlrewriter"); 

                    if (!string.IsNullOrEmpty(urlr))
                    {
                        if (SiteBLL.ExistsCms(" article_id='" + Utils.StrToInt(urlr, 0) + "' or urlrewriter='" + urlr + "'"))
                        {

                            //显示提示信息
                            this.DisplayMessage("文章添加失败,自定义Url重复存在！", 2, "?act=add");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(cmsinfo.title))
                    {
                        if (SiteBLL.ExistsCms(" title='" + cmsinfo.title + "'"))
                        {

                            //显示提示信息
                            this.DisplayMessage("文章添加失败,标题重复存在！", 2, "?act=add");
                            return;
                        }
                    }

                    int cms_id= SiteBLL.InsertCmsInfo(cmsinfo);


                    //加入搜索库
                    Search.ChangeSearch(cmsinfo.title, SiteUtils.NoHTML(cmsinfo.des), SiteUtils.NoHTML(cmsinfo.content), cmsinfo.tag, cmsinfo.photo, 2, cmsinfo.click_count.Value, cms_id);

                    //关联商品
                    this.handle_link_cms(cms_id);

                    string sql = "DELETE FROM " + DY.Config.BaseConfig.TablePrefix + "Cms_Link WHERE (cms_id = 0 OR link_goods_id = 0) and admin_id = " + id + "";

                    SystemConfig.SqlProcess(sql);

                    base.id = cms_id;
                    //保存属性信息
                    this.SaveGoodsAttrValue(base.id);

                    #region 发送微博
                    string title = SiteUtils.GetKeyToWeibo(FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag")).Split(','), FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords")).Split(',')) + "【" + cmsinfo.title+"】";
                    string des = title+" "+SiteUtils.GetDes(cmsinfo.des, cmsinfo.content);
                    string photo = string.IsNullOrEmpty(cmsinfo.photo) ? SiteUtils.GetContentImgUrl(cmsinfo.content) : cmsinfo.photo;
                    if (config.Is_oauth)
                    {
                        base.AddLog("发送微博：" + SiteUtils.SendWeibo(new SiteUtils().GetTopic(des, 160) + urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + base.id + config.UrlRewriterKzm, Server.MapPath(photo)));

                        base.AddLog("发送文章（头条）：" + SiteUtils.SendArticle(cmsinfo.title, SiteUtils.GetDes(cmsinfo.des, cmsinfo.content), cmsinfo.content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                    }
                    #endregion

                    #region 同步到博客
                    base.AddLog("同步博客：" + SiteUtils.SendWeblog(cmsinfo.title, cmsinfo.content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                    #endregion

                    //添加标签到标签库
                    CMS.SaveTag(FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag")).Split(','));

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (cmsinfo != null)
                        {
                            string cid = cmsinfo.urlrewriter;
                            if (cid == "") { cid = base.id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms-detail.aspx?code=" + cid,
                                    Server.MapPath(urlrewrite.html + urlrewrite.article_detail + id + urlrewrite.html_suffix));
                        }
                    }


                    //日志记录
                    base.AddLog("添加文章：" + DYRequest.getForm("title"));

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&cat_id=" + DYRequest.getRequestInt("cat_id",0));

                    string message = "文章添加成功";
                    if (config.Is_BaiduPing)
                        message += siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.article_detail + base.id + config.UrlRewriterKzm) == 0 ? "，百度ping成功" : "，百度ping失败";

                    //显示提示信息
                    this.DisplayMessage(message, 2, "?act=list&cat_id=" + DYRequest.getRequestInt("cat_id", 0), links);
                }

                context.Add("cat_id", DYRequest.getRequestInt("cat_id"));
                context.Add("username", base.username);

                base.DisplayTemplate(context, "cms/cms_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
               
                string urlr = DYRequest.getFormString("urlrewriter"); ;
                if (!string.IsNullOrEmpty(urlr))
                {
                    if (SiteBLL.ExistsCms("(article_id='" + Utils.StrToInt(urlr, 0) + "' or urlrewriter='" + urlr + "') and article_id<>" + base.id))
                    {

                        //显示提示信息
                        this.DisplayMessage("文章修改失败,自定义Url重复存在！", 2, "?act=edit&t=iframe&id=" + base.id);
                        return;
                    }
                }


                //检测权限
                this.IsChecked("cms_edit");
               

                if (ispost)
                {
                    CmsInfo cmsinfo = this.SetEntity();
                    //if (!string.IsNullOrEmpty(cmsinfo.title))
                    //{
                    //    if (SiteBLL.ExistsCms(" title='" + cmsinfo.title + "'"))
                    //    {

                    //        //显示提示信息
                    //        this.DisplayMessage("文章添加失败,标题重复存在！", 2, "?act=add");
                    //        return;
                    //    }
                    //}
                    SiteBLL.UpdateCmsInfo(cmsinfo);

                    //加入搜索库
                    Search.ChangeSearch(cmsinfo.title, SiteUtils.NoHTML(cmsinfo.des), SiteUtils.NoHTML(cmsinfo.content), cmsinfo.tag, cmsinfo.photo, 2, cmsinfo.click_count.Value, base.id);

                    //保存属性信息
                    this.SaveGoodsAttrValue(base.id);

                    if (DYRequest.getFormBoolean("is_send"))
                    {
                        #region 发送微博
                        string title = SiteUtils.GetKeyToWeibo(FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag")).Split(','), FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords")).Split(',')) + "【" + cmsinfo.title + "】";
                        string des = title + " " + SiteUtils.GetDes(cmsinfo.des, cmsinfo.content);
                        string photo = string.IsNullOrEmpty(cmsinfo.photo) ? SiteUtils.GetContentImgUrl(cmsinfo.content) : cmsinfo.photo;
                        if (config.Is_oauth)
                        {
                            base.AddLog("发送微博：" + SiteUtils.SendWeibo(new SiteUtils().GetTopic(des, 160) + urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + base.id + config.UrlRewriterKzm, Server.MapPath(photo)));

                            base.AddLog("发送文章（头条）：" + SiteUtils.SendArticle(cmsinfo.title, SiteUtils.GetDes(cmsinfo.des, cmsinfo.content), cmsinfo.content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                        }
                        #endregion

                        #region 同步到博客
                        base.AddLog("同步博客：" + SiteUtils.SendWeblog(cmsinfo.title, cmsinfo.content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                        #endregion
                    }

                    //添加标签到标签库
                    CMS.SaveTag(DYRequest.getFormString("tag").Split(','));

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (cmsinfo != null)
                        {
                            string id = cmsinfo.urlrewriter;
                            if (id == "") { id = cmsinfo.article_id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/cms-detail.aspx?code=" + id,
                                    Server.MapPath(urlrewrite.html+urlrewrite.article_detail+ id + ".html"));
                        }
                    }

                    //日志记录
                    base.AddLog("修改文章：" + DYRequest.getForm("title"));

                    string message = "文章修改成功";
                    //if (config.Is_BaiduPing)
                    //    message += siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.article_detail + base.id + config.UrlRewriterKzm) == 0 ? "，百度ping成功" : "，百度ping失败";

                    //显示提示信息
                    base.DisplayMessage(message, 2, "?act=list&cat_id=" + DYRequest.getFormString("cat_id"));
                }
                if (SiteBLL.GetCmsInfo(base.id).down_id != null && SiteBLL.GetCmsInfo(base.id).down_id != "")
                    context.Add("downlist", SiteBLL.GetDownloadAllList("", "down_id in(" + SiteBLL.GetCmsInfo(base.id).down_id + ")"));

                context.Add("goodsLink", SiteBLL.GetCmsLinkAllList("link_goods_id", "cms_id=" + base.id + ""));
                context.Add("entity", SiteBLL.GetCmsInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "cms/cms_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("cms_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateCmsFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改文章：" + val);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("cms_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateCmsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("cms_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteCmsInfo("article_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //执行索引库删除
                        Search.DeleteSearch(2, ids.Remove(ids.Length - 1, 1));

                        //日志记录
                        base.AddLog("删除文章");
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
                this.IsChecked("cms_del", true);

                //执行删除
                SiteBLL.DeleteCmsInfo(base.id);
                //执行索引库删除
                Search.DeleteSearch(2, base.id);

                //日志记录
                base.AddLog("删除文章");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 移动位置
            else if (this.act == "order")
            {
                //检测权限
                this.IsChecked("cms_edit", true);

                //日志记录
                base.AddLog("移动资讯位置");

                //移动
                int state = CMS.MoveCmsPos(DYRequest.getRequest("move_act"), base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region JSON取得关联商品列表
            else if (this.act == "get_goods_list")
            {
                string sql = "";

                string[] qur = DYRequest.getRequest("filter").Split(',');

                if (qur[0] != "0")
                    sql += " and cat_id in(" + goods.GetGoodsAllCatIds(int.Parse(qur[0].ToString())) + ")";

                if (qur[1] != "0")
                    sql += " and brand_id=" + qur[1] + "";

                if (qur[2] != "")
                    sql += " and goods_name like '%" + qur[2] + "%'";

                string content = goods.Get_json_goods_list(sql);

                //输出json数据
                base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + content.ToString() + "]}");

            }
            #endregion

            #region JSON取得关联资讯列表
            else if (this.act == "get_cms_list")
            {
                string sql = "";

                string[] qur = DYRequest.getRequest("filter").Split(',');

                if (qur[0] != "0")
                    sql += " and cat_id in(" + cms.GetCMSCatAllIds(int.Parse(qur[0].ToString())) + ")";
                if (qur[1] != "")
                    sql += " and title like '%" + qur[1] + "%'";

                string content = cms.Get_json_cms_list(sql);

                //输出json数据
                base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + content.ToString() + "]}");

            }
            #endregion

            #region JSON取得关联下载列表
            else if (this.act == "get_down_list")
            {
                string sql = "";

                string[] qur = DYRequest.getRequest("filter").Split(',');

                if (qur[0] != "0")
                    sql += " and cat_id in(" + down.GetDownloadCatAllIds(int.Parse(qur[0].ToString())) + ")";
                if (qur[1] != "")
                    sql += " and title like '%" + qur[1] + "%'";

                string content = down.Get_json_down_list(sql);

                //输出json数据
                base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + content.ToString() + "]}");

            }
            #endregion

            #region JSON添加关联
            else if (this.act == "add_link_goods")
            {
                this.add_link_goods();
            }
            #endregion

            #region JSON移除关联
            else if (this.act == "drop_link_goods")
            {
                this.drop_link_goods();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";
            if (DYRequest.getRequestInt("cat_id") > 0)
                filter += " and cat_id in (" + cms.GetCMSCatIds(DYRequest.getRequestInt("cat_id"))+")";

            this.GetList("goods/goods_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            context.Add("list", SiteBLL.GetCmsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("sort_order desc,is_top desc,article_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //context.Add("isajax", base.isajax);
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("cat_id", DYRequest.getRequestInt("cat_id"));
            context.Add("type", DYRequest.getRequest("type"));

            context.Add("userid", base.userid);

            base.DisplayTemplate(context, "cms/cms_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected CmsInfo SetEntity()
        {
            CmsInfo entity = new CmsInfo();

            entity.cat_id = DYRequest.getFormInt("cat_id");
            entity.title = DYRequest.getForm("title");
            entity.fu_title = DYRequest.getForm("fu_title");
            entity.title_style ="";
            entity.content = DYRequest.getForm("content");
            entity.author = DYRequest.getForm("author");
            entity.source = DYRequest.getForm("source");
            entity.editor = DYRequest.getForm("editor");
            entity.is_newopen = DYRequest.getFormBoolean("is_newopen");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.is_top = DYRequest.getFormBoolean("is_top");
            entity.is_best = DYRequest.getFormBoolean("is_best");
            entity.info_tlp = DYRequest.getForm("info_tlp");
            entity.tag = FunctionUtils.Text.ToDBC(DYRequest.getForm("tag"));
            entity.link = DYRequest.getForm("link");
            entity.photo = string.IsNullOrEmpty(DYRequest.getForm("photo")) ? SiteUtils.GetContentImgUrl(DYRequest.getForm("content")) : DYRequest.getForm("photo");
            entity.des = string.IsNullOrEmpty(DYRequest.getForm("des")) ? new SiteUtils().GetTopic(Utils.RemoveHtml(DYRequest.getForm("content")), 250).Replace("...","") : DYRequest.getForm("des");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.title, 2);
            entity.add_time = string.IsNullOrEmpty(DYRequest.getFormString("add_time")) ? DateTime.Now : DYRequest.getFormDateTime("add_time");
            entity.showtime = string.IsNullOrEmpty(DYRequest.getFormString("showtime")) ? DateTime.Now : DYRequest.getFormDateTime("showtime");
            entity.click_count = DYRequest.getFormInt("click_count");
            entity.pagetitle = DYRequest.getForm("pagetitle");
            entity.pagekeywords = FunctionUtils.Text.ToDBC(DYRequest.getForm("pagekeywords"));
            entity.pagedesc = DYRequest.getForm("pagedesc");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.is_hot = DYRequest.getFormBoolean("is_hot");
            entity.is_mobile = DYRequest.getFormBoolean("is_mobile");
            entity.is_oauth = DYRequest.getFormBoolean("is_oauth");
            entity.cms_type = DYRequest.getFormInt("cms_type");
            entity.down_id = DYRequest.getFormString("down_id");
            entity.article_id = base.id;

            return entity;
        }

        /// <summary>
        /// 保存商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        protected void SaveGoodsAttrValue(int article_id)
        {
            if (Request.Form["attr_id_list"] != null)
            {
                string[] attr_id_list = Request.Form.GetValues("attr_id_list");
                string[] attr_value_list = Request.Form.GetValues("attr_value_list");
                string[] goods_attr_id_list = Request.Form.GetValues("goods_attr_id_list");

                //删除原来存在的资讯属性值
                Goods.DeleteGoodsAttrValueByNewsId(article_id);

                //循环处理每个属性
                for (int i = 0; i < attr_id_list.Length; i++)
                {
                    //属性ID
                    int attr_id = Utils.StrToInt(attr_id_list[i], -1);

                    //属性值
                    string attr_value = attr_value_list[i];

                    //属性值ID
                    int goods_attr_id = Utils.StrToInt(goods_attr_id_list[i], -1);

                    if (!string.IsNullOrEmpty(attr_value))
                    {
                        //赋值 
                        GoodsAttrInfo attrinfo = new GoodsAttrInfo(0, 0, attr_id, attr_value, 0, article_id);

                        SiteBLL.InsertGoodsAttrInfo(attrinfo);
                    }
                }
            }
        }



        #region 关联商品
        /// <summary>
        /// 添加关联商品
        /// </summary>
        protected void add_link_goods()
        {
            object[] linked_array = DYRequest.getRequest("add_ids").Split(',');
            object[] linked_goods = DYRequest.getRequest("test").Split(',');
            int is_double = Utils.StrToBool(linked_goods[1], false) == true ? 0 : 1;
            int cms_id = Convert.ToInt32(linked_goods[0]);
            int type = Convert.ToInt32(linked_goods[2]);
            CmsLinkInfo cmslink=new CmsLinkInfo();
            cmslink.cms_id=Convert.ToInt32(linked_goods[0]);
            cmslink.is_double = Utils.StrToBool(linked_goods[1], false) == true ? false : true; ;
            cmslink.link_dowsload_id=0;
            cmslink.admin_id=base.id;
            cmslink.type = type;

            for (int i = 0; i < linked_array.Length; i++)
            {
                int ids = Convert.ToInt32(linked_array[i]);
                cmslink.link_goods_id = ids;
                SiteBLL.InsertCmsLinkInfo(cmslink);
            }

            string content = "";
            switch (type)
            {
                case 0: content = goods.Get_json_goods_list("and goods_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "cms_Link  where cms_id=" + cms_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                case 1: content = cms.Get_json_cms_list("and article_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "cms_Link  where cms_id=" + cms_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                case 2: content = down.Get_json_down_list("and down_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "cms_Link  where cms_id=" + cms_id + " and admin_id=" + id + " and type=" + type + ")"); break;
            }
            //输出json数据
            base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + content.ToString() + "]}");


        }

        /// <summary>
        /// 移除关联商品
        /// </summary>
        protected void drop_link_goods()
        {
            object[] drop_goods = DYRequest.getRequest("drop_ids").Split(',');
            object[] linked_goods = DYRequest.getRequest("test").Split(',');
            int cms_id = Convert.ToInt32(linked_goods[0]);
            int type = Convert.ToInt32(linked_goods[2]);
            bool is_signle = Utils.StrToBool(linked_goods[1], false);
            int is_double = Utils.StrToBool(linked_goods[1], false) == true ? 0 : 1;
            CmsLinkInfo cmslink = new CmsLinkInfo();
            cmslink.cms_id = Convert.ToInt32(linked_goods[0]);
            cmslink.is_double = Utils.StrToBool(linked_goods[1], false) == true ? false : true; ;
            cmslink.link_dowsload_id = 0;
            cmslink.admin_id = base.id;
            cmslink.type = type;

                if (is_signle)
                    SiteBLL.DeleteCmsLinkInfo("link_goods_id in (" + DYRequest.getRequest("drop_ids") + ") and type=" + type + " and cms_id=" + cms_id);
                else
                {
                    //sql = "UPDATE " + DY.Config.BaseConfig.TablePrefix + "Cms_Link  SET is_double = 0 WHERE link_goods_id = " + cms_id + " or cms_id in (" + DYRequest.getRequest("drop_ids") + ")";
                    for (int i = 0; i < drop_goods.Length; i++)
                    {
                        SiteBLL.UpdateCmsLinkFieldValue("is_double", "0", Convert.ToInt32(drop_goods[i]));
                    }
                }

                string content = "";
                switch (type)
                {
                    case 0: content = goods.Get_json_goods_list("and goods_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "cms_Link  where cms_id=" + cms_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                    case 1: content = cms.Get_json_cms_list("and article_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "cms_Link  where cms_id=" + cms_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                    case 2: content = down.Get_json_down_list("and down_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "cms_Link  where cms_id=" + cms_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                }
            //输出json数据
            base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + content.ToString() + "]}");
        }



        /// <summary>
        /// 保存某资讯的关联商品
        /// </summary>
        /// <param name="goods_id"></param>
        public void handle_link_cms(int article_id)
        {
            string sql = "UPDATE " + DY.Config.BaseConfig.TablePrefix + "Cms_Link SET cms_id = " + article_id + " WHERE cms_id = 0 AND admin_id = " + id + "";

            SystemConfig.SqlProcess(sql);

            sql = "UPDATE " + DY.Config.BaseConfig.TablePrefix + "Cms_Link SET link_goods_id = " + article_id + " WHERE link_goods_id = 0 AND admin_id = " + id + "";

            SystemConfig.SqlProcess(sql);
        }

        #endregion
    }
}

