using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.LanguagePack;

namespace DY.Web.admin
{
    public partial class goods_category : AdminPage
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
                this.IsChecked("goods_category_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_category_add");

                if (ispost)
                {
                    GoodsCategoryInfo goodscat = this.SetEntity();
                    Goods.InsertGoodsCategory(goodscat);

                    //清除分类缓存
                    caches.RemoveGoodsCat();

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        base.id = SiteBLL.GetGoodsCategoryInfo("cat_id=(select IDENT_CURRENT('" + DY.Config.BaseConfig.TablePrefix + "goods_category'))").cat_id.Value;

                        if (base.id > 0)
                        {
                            string id = goodscat.urlrewriter;
                            if (id == "") { id = base.id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/goods.aspx?code=" + id,
                                Server.MapPath(urlrewrite.html + urlrewrite.product + id + urlrewrite.html_suffix));
                        }
                    }

                    //日志记录
                    base.AddLog("添加商品分类");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("商品分类添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate(context, "goods/goods_category_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_category_edit");

                if (ispost)
                {
                    GoodsCategoryInfo goodscat = this.SetEntity();
                    Goods.UpdateGoodsCategory(goodscat);

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (base.id > 0)
                        {
                            string id = goodscat.urlrewriter;
                            if (id == "") { id = base.id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/goods.aspx?code=" + id,
                                Server.MapPath(urlrewrite.html + urlrewrite.product + id + urlrewrite.html_suffix));
                        }
                    }

                    //日志记录
                    base.AddLog("修改商品分类");

                    //清除分类缓存
                    caches.RemoveGoodsCat();

                    //显示提示信息
                    base.DisplayMessage("商品分类修改成功", 2, "?act=list");
                }

                context.Add("entity", SiteBLL.GetGoodsCategoryInfo(base.id));

                base.DisplayTemplate(context, "goods/goods_category_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_category_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改商品分类");

                    #region 添加到导航栏\从导航栏删除
                    if (fieldName == "show_in_nav")
                    {
                        AddNav(base.id.ToString(), val.ToString());
                    }
                    #endregion

                    //执行修改
                    SiteBLL.UpdateGoodsCategoryFieldValue(fieldName, val, base.id);

                    //清除分类缓存
                    caches.RemoveGoodsCat();

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("goods_category_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改商品分类");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        #region 添加到导航栏\从导航栏删除
                        if (fieldName == "show_in_nav")
                        {
                            AddNav(ids, Convert.ToInt16(val) == 0 ? false : true);
                        }
                        #endregion

                        //执行修改
                        SiteBLL.UpdateGoodsCategoryFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 移动分类位置
            else if (this.act == "order")
            {
                //检测权限
                this.IsChecked("goods_category_edit", true);

                //日志记录
                base.AddLog("移动商品分类位置");

                //移动
                int state = Goods.MoveGoodsCategoryPos(DYRequest.getRequest("move_act"), base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("goods_category_del", true);

                //日志记录
                base.AddLog("删除商品分类");

                //执行删除
                int state = Goods.DeleteGoodsCategory(base.id);

                //清除分类缓存
                caches.RemoveGoodsCat();

                if (state == 0)
                    //显示列表数据
                    this.GetList();
                else if (state == 1)
                    base.DisplayJsonMessage(1, "请先删除该分类下的子分类。");
            }
            #endregion
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            context.Add("list", new Caches().GoodsCat().Rows);
            base.DisplayTemplate(context, "goods/goods_category_list", base.isajax);
        }

        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected GoodsCategoryInfo SetEntity()
        {
            GoodsCategoryInfo entity = new GoodsCategoryInfo();
            entity.parent_id = DYRequest.getFormInt("parent_id") < 0 ? 0 : DYRequest.getFormInt("parent_id");
            entity.cat_name = DYRequest.getFormString("cat_name");
            entity.cat_ico = DYRequest.getFormString("cat_ico");
            entity.cat_desc = DYRequest.getFormString("cat_desc");
            entity.cat_level = DYRequest.getFormInt("cat_level");
            entity.measure_unit = DYRequest.getFormString("measure_unit");
            entity.show_in_nav = DYRequest.getFormBoolean("show_in_nav");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.cat_name, 4);
            entity.pagetitle = DYRequest.getFormString("pagetitle");
            entity.pagekeywords = FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords"));
            entity.pagedesc = DYRequest.getFormString("pagedesc");
            entity.list_tlp = DYRequest.getForm("list_tlp");
            entity.info_tlp = DYRequest.getForm("info_tlp");
            entity.page_size = DYRequest.getFormInt("page_size");
            //entity.pricearea = DYRequest.getForm("price_area");
            entity.cat_name_en = DYRequest.getForm("cat_name_en");
            entity.is_mobile = DYRequest.getFormBoolean("is_mobile");
            entity.cat_h_ico = DYRequest.getForm("cat_h_ico");
            entity.cat_id = base.id;
            return entity;
        }

        protected void AddNav(string ids, object val)
        {
            try
            {
                foreach (string str in ids.Split(','))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(Utils.StrToInt(str, 0));
                        
                        if (catinfo != null)
                        {
                            string url = string.IsNullOrEmpty(catinfo.urlrewriter) ? catinfo.cat_id.ToString() : catinfo.urlrewriter;
                            url = urlrewrite.product + url + config.UrlRewriterKzm;
                            if (config.EnableHtml)
                                url = urlrewrite.html + "/" + url + urlrewrite.html_suffix;

                            MenuManage.AddToNav(url, catinfo.cat_name, Convert.ToInt16(val) == 0 ? false : true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //输出json数据
                base.DisplayMemoryTemplate(base.MakeJson("", 1, ex.Message));
            }
        }
    }
}
