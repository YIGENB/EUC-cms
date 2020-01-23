/**
 * 功能描述：Goods管理类
 * 创建时间：2010-1-29 14:20:15
 * 最后修改时间：2010-1-29 14:20:15
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
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.IO;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Text;
using DY.LanguagePack;

namespace DY.Web.admin
{
    public partial class goods : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 回收站
            else if (this.act == "trash")
            {
                //检测权限
                this.IsChecked("goods_trash");

                //显示列表数据
                this.GetList("goods/goods_trash", " and is_delete=1");
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_add");

                if (ispost)
                {
                    GoodsInfo goodsinfo = this.SetEntity();
                    int goods_id = SiteBLL.InsertGoodsInfo(goodsinfo);

                    //加入搜索库
                    Search.ChangeSearch(goodsinfo.goods_name, SiteUtils.NoHTML(goodsinfo.goods_desc), SiteUtils.NoHTML(goodsinfo.goods_content),goodsinfo.tag, goodsinfo.original_img, 1, goodsinfo.click_count.Value, goods_id);

                    //关联商品
                    this.handle_link_goods(goods_id);

                    string sql = "DELETE FROM " + DY.Config.BaseConfig.TablePrefix + "Goods_Link WHERE (goods_id = 0 OR link_goods_id = 0) and admin_id = " + id + "";

                    SystemConfig.SqlProcess(sql);

                    string strsql = "update " + DY.Config.BaseConfig.TablePrefix + "goods set urlrewriter='" + goods_id + "' where goods_id=" + goods_id + "";

                    SystemConfig.SqlProcess(strsql);

                    #region 发送微博
                    string title = SiteUtils.GetKeyToWeibo(FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag")).Split(','), FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords")).Split(',')) + "【" + goodsinfo.goods_name + "】";
                    string des = title + " " + SiteUtils.GetDes(goodsinfo.goods_desc, goodsinfo.goods_content);
                    string photo = string.IsNullOrEmpty(goodsinfo.original_img) ? SiteUtils.GetContentImgUrl(goodsinfo.goods_content) : goodsinfo.original_img;
                    if (config.Is_oauth)
                    {
                        base.AddLog("发送微博：" + SiteUtils.SendWeibo(new SiteUtils().GetTopic(des, 160) + urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + base.id + config.UrlRewriterKzm, Server.MapPath(photo)));

                        base.AddLog("发送文章（头条）：" + SiteUtils.SendArticle(goodsinfo.goods_name, SiteUtils.GetDes(goodsinfo.goods_desc, goodsinfo.goods_content), goodsinfo.goods_content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                    }
                    #endregion

                    #region 同步到博客
                    base.AddLog("同步博客：" + SiteUtils.SendWeblog(goodsinfo.goods_name, goodsinfo.goods_content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                    #endregion

                    //更新商品相册信息
                    this.UpdateGoodsGallery(goods_id);
                    //保存属性信息
                    this.SaveGoodsAttrValue(goods_id);

                    #region 启用百度ping服务
                    if (config.Is_BaiduPing)
                        siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.product_detail + base.id + config.UrlRewriterKzm);
                    #endregion


                    //保存扩展分类
                    this.SaveGoodsCat(goods_id);

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (goods_id > 0)
                        {
                            string ids = goodsinfo.urlrewriter;
                            if (ids == "") { ids = goods_id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/goods-detail.aspx?code=" + ids,
                                Server.MapPath(urlrewrite.html + urlrewrite.product_detail + ids + urlrewrite.html_suffix));
                        }
                    }
                    //添加标签到标签库
                    Goods.SaveTag(FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag")).Split(','));

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //日志记录
                    base.AddLog("添加商品");

                    string message = "商品添加成功";
                    if (config.Is_BaiduPing)
                        message += siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.product_detail + base.id + config.UrlRewriterKzm) == 0 ? "，百度ping成功" : "，百度ping失败";

                    //显示提示信息
                    this.DisplayMessage(message, 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("userRankPrice", Goods.GetGoodsUserRankPrice(base.id).Rows);
                context.Add("specList", SiteBLL.GetGoodsSpecAllList("", ""));

                context.Add("userid", base.userid);

                base.DisplayTemplate(context, "goods/goods_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_edit");

                if (ispost)
                {
                    int goods_id = base.id;
                    string message = "商品修改成功";

                    GoodsInfo goodsinfo = this.SetEntity();

                    if (DYRequest.getRequest("type") == "copy")
                    {
                        //日志记录
                        base.AddLog("添加商品");

                        goods_id = SiteBLL.InsertGoodsInfo(goodsinfo);
                        message = "商品添加成功";
                    }
                    else
                    {
                        //日志记录
                        base.AddLog("修改商品");

                        SiteBLL.UpdateGoodsInfo(goodsinfo);
                    }

                    //加入搜索库
                    Search.ChangeSearch(goodsinfo.goods_name, SiteUtils.NoHTML(goodsinfo.goods_desc), SiteUtils.NoHTML(goodsinfo.goods_content), goodsinfo.tag, goodsinfo.original_img, 1, goodsinfo.click_count.Value, goods_id);

                    //添加标签到标签库
                    Goods.SaveTag(DYRequest.getFormString("tag").Split(','));
                    //更新商品相册信息
                    this.UpdateGoodsGallery(goods_id);
                    //保存属性信息
                    this.SaveGoodsAttrValue(goods_id);

                    #region 发送微博
                    string title = SiteUtils.GetKeyToWeibo(FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag")).Split(','), FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords")).Split(',')) + "【" + goodsinfo.goods_name + "】";
                    string des = title + " " + SiteUtils.GetDes(goodsinfo.goods_desc, goodsinfo.goods_content);
                    string photo = string.IsNullOrEmpty(goodsinfo.original_img) ? SiteUtils.GetContentImgUrl(goodsinfo.goods_content) : goodsinfo.original_img;
                    if (config.Is_oauth)
                    {
                        base.AddLog("发送微博：" + SiteUtils.SendWeibo(new SiteUtils().GetTopic(des, 160) + urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + base.id + config.UrlRewriterKzm, Server.MapPath(photo)));

                        base.AddLog("发送文章（头条）：" + SiteUtils.SendArticle(goodsinfo.goods_name, SiteUtils.GetDes(goodsinfo.goods_desc, goodsinfo.goods_content), goodsinfo.goods_content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));

                        #region 同步到博客
                        base.AddLog("同步博客：" + SiteUtils.SendWeblog(goodsinfo.goods_name, goodsinfo.goods_content.Replace("src=\"/", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain())).Replace("src=\" /", string.Format("src=\"{0}/", urlrewrite.http + new SiteUtils().GetDomain()))));
                        #endregion
                    }
                    #endregion

                    #region 启用百度ping服务
                    if (config.Is_BaiduPing)
                        message += siteUtils.SendPing("baidu", urlrewrite.http + siteUtils.GetDomain() + urlrewrite.product_detail + base.id + config.UrlRewriterKzm) == 0 ? "，百度ping成功" : "，百度ping失败";
                    #endregion

                    //保存扩展分类
                    this.SaveGoodsCat(goods_id);
                    //保存规格信息
                    this.SaveSpec(goods_id, goodsinfo);

                    //生成静态页
                    if (config.EnableHtml)
                    {
                        if (goodsinfo != null)
                        {
                            string ids = goodsinfo.urlrewriter;
                            if (ids == "") { ids = goods_id.ToString(); }

                            SiteUtils.MakeHtml(urlrewrite.http + siteUtils.GetDomain() + "/goods-detail.aspx?code=" + ids,
                                Server.MapPath(urlrewrite.html + urlrewrite.product_detail + ids + urlrewrite.html_suffix));
                        }
                    }

                    //显示提示信息
                    if (base.target == "iframe")
                        Response.Redirect("goods.aspx?act=edit&t=iframe&update=success&id=" + base.id);
                    else
                        base.DisplayMessage(message, 2, "?act=list");
                }

                IDictionary context = new Hashtable();

                GoodsCategoryInfo GoodsCategory = SiteBLL.GetGoodsCategoryInfo(SiteBLL.GetGoodsInfo(base.id).cat_id.Value);


                if (GoodsCategory != null)
                {
                    context.Add("cat_name", GoodsCategory.cat_name);
                    //context.Add("cat_names", Caches.GoodsNav(GoodsCategory.cat_id.Value, ""));
                }

                if (SiteBLL.GetGoodsInfo(base.id).down_id != null && SiteBLL.GetGoodsInfo(base.id).down_id != "")
                    context.Add("downlist", SiteBLL.GetDownloadAllList("", "down_id in(" + SiteBLL.GetGoodsInfo(base.id).down_id + ")"));

                context.Add("entity", SiteBLL.GetGoodsInfo(base.id));
                context.Add("othercat", Goods.GetGoodsOtherCat(base.id));
                context.Add("userRankPrice", Goods.GetGoodsUserRankPrice(base.id).Rows);
                context.Add("goodsLink", SiteBLL.GetGoodsLinkAllList("goods_id", "goods_id=" + base.id + " and type=0"));
                context.Add("cmsLink", SiteBLL.GetGoodsLinkAllList("goods_id", "goods_id=" + base.id + " and type=1"));
                context.Add("downLink", SiteBLL.GetGoodsLinkAllList("goods_id", "goods_id=" + base.id + " and type=2"));
                context.Add("specList", SiteBLL.GetGoodsSpecAllList("", ""));

                context.Add("userid", base.userid);

                base.DisplayTemplate(context, "goods/goods_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改商品");

                    //执行修改
                    SiteBLL.UpdateGoodsFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("goods_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改商品");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateGoodsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                        //执行索引库修改
                        Search.ChangeSearch(1, ids.Remove(ids.Length - 1, 1), val);
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
                this.IsChecked("goods_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除商品");

                        //执行删除
                        SiteBLL.DeleteGoodsInfo("goods_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //执行索引库删除
                        Search.DeleteSearch(1, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("goods_del", true);

                //日志记录
                base.AddLog("删除商品");

                //执行删除
                SiteBLL.DeleteGoodsInfo(base.id);

                //执行索引库删除
                Search.DeleteSearch(1, base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 上传商品图片
            else if (this.act == "upload")
            {
                this.Upload();
            }
            #endregion

            #region 动态生成商品相册缩略图
            else if (this.act == "thumbnail")
            {
                this.CreatThumbnail();
            }
            #endregion

            #region 删除商品相册图片
            else if (this.act == "DropGallery")
            {
                //检测权限
                this.IsChecked("goods_gallery_del", true);

                int img_id = DYRequest.getRequestInt("img_id");

                //取得图片信息
                GoodsGalleryInfo galleryinfo = SiteBLL.GetGoodsGalleryInfo(img_id);
                if (galleryinfo != null)
                {
                    //日志记录
                    base.AddLog("删除商品图片");

                    string original_img = galleryinfo.original_img;

                    //先删除服务器上对应的图片文件
                    FileOperate.Delete(Server.MapPath(galleryinfo.goods_img), FileOperate.FsoMethod.File);
                    FileOperate.Delete(Server.MapPath(galleryinfo.goods_thumb), FileOperate.FsoMethod.File);
                    FileOperate.Delete(Server.MapPath(galleryinfo.info_img), FileOperate.FsoMethod.File);
                    FileOperate.Delete(Server.MapPath(original_img), FileOperate.FsoMethod.File);

                    //执行删除
                    SiteBLL.DeleteGoodsGalleryInfo(img_id);

                    //显示提示
                    base.DisplayMemoryTemplate(base.MakeJson(original_img, 0, img_id.ToString()));
                }
                else
                {
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, "您要删除的图片不存在。"));
                }
            }
            #endregion

            #region 搜索商品
            else if (base.act == "search_goods")
            {
                SearchGood();
            }
            #endregion

            #region 获取商品详情
            else if (base.act == "get_goods_info")
            {
                GetGoodsInfo();
            }
            #endregion

            #region 获取商品规格信息
            else if (base.act == "get_spec")
            {
                this.GetSpec();
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

            #region 重新生成水印
            else if (this.act == "reset")
            {
                if (config.WatermarkGoods)
                {
                    if (config.WatermarkPic != "" && Utils.FileExists(Server.MapPath(config.WatermarkPic)))
                    {
                        int ico_w = Utils.StrToInt(config.GoodsImgIco.Split('*')[0], 0);
                        int ico_h = Utils.StrToInt(config.GoodsImgIco.Split('*')[1], 0);
                        int list_w = Utils.StrToInt(config.GoodsImgList.Split('*')[0], 0);
                        int list_h = Utils.StrToInt(config.GoodsImgList.Split('*')[1], 0);
                        int info_w = Utils.StrToInt(config.GoodsImgInfo.Split('*')[0], 0);
                        int info_h = Utils.StrToInt(config.GoodsImgInfo.Split('*')[1], 0);

                        foreach (GoodsGalleryInfo item in SiteBLL.GetGoodsGalleryAllList("", ""))
                        {
                            string original_img = item.original_img;

                            if (Utils.FileExists(Server.MapPath(original_img)))
                            {
                                WebGDI.GetWaterMarkPicImage(Server.MapPath(original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(original_img.Replace("_", "info_")), info_w, info_h);
                                WebGDI.GetWaterMarkPicImage(Server.MapPath(original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(original_img.Replace("_", "list_")), list_w, list_h);
                                //WebGDI.GetWaterMarkPicImage(Server.MapPath(original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(original_img.Replace("_", "ico_")), ico_w, ico_h);
                                WebGDI.SendSmallImage(Server.MapPath(original_img), Server.MapPath(original_img.Replace("_", "ico_")), ico_w, ico_h);
                            }
                        }

                        //日志记录
                        base.AddLog("重新生成水印");

                        //显示提示信息
                        this.DisplayMessage("重新生成水印成功", 2, "?act=list");
                    }
                    else
                    {
                        //显示提示信息
                        this.DisplayMessage("水印图片不存在", 1, "?act=list");
                    }
                }
                else
                {
                    //显示提示信息
                    this.DisplayMessage("没有启用水印", 1, "?act=list");
                }

                //检测权限
                //this.IsChecked("goods_list");
                //显示列表数据
                //this.GetList();
            }
            #endregion

            #region 移动位置
            else if (this.act == "order")
            {
                //检测权限
                this.IsChecked("goods_edit", true);

                //日志记录
                base.AddLog("移动商品位置");

                //移动
                int state = Goods.MoveGoodsPos(DYRequest.getRequest("move_act"), base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = " and is_delete=0";
            if (DYRequest.getRequestInt("cat_id") > 0)
                filter += " and cat_id in (" + goods.GetGoodsAllCatIds(DYRequest.getRequestInt("cat_id")) + ")";

            this.GetList("goods/goods_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();

            context.Add("list", SiteBLL.GetGoodsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("sort_order desc,goods_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));

            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("cat_id", DYRequest.getRequestInt("cat_id"));
            context.Add("page_size", base.pagesize);
            context.Add("type", DYRequest.getRequest("type"));
            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值_goods_gg
        /// </summary>
        protected GoodsInfo SetEntity()
        {
            int ico_w = Utils.StrToInt(config.GoodsImgIco.Split('*')[0], 0);
            int ico_h = Utils.StrToInt(config.GoodsImgIco.Split('*')[1], 0);
            int list_w = Utils.StrToInt(config.GoodsImgList.Split('*')[0], 0);
            int list_h = Utils.StrToInt(config.GoodsImgList.Split('*')[1], 0);
            int info_w = Utils.StrToInt(config.GoodsImgInfo.Split('*')[0], 0);
            int info_h = Utils.StrToInt(config.GoodsImgInfo.Split('*')[1], 0);

            GoodsInfo entity = new GoodsInfo();
            entity.cat_id = DYRequest.getFormInt("cat_id");
            entity.brand_id = DYRequest.getFormInt("brand_id");
            entity.goods_type = DYRequest.getFormInt("goods_type");
            entity.goods_sn = string.IsNullOrEmpty(DYRequest.getFormString("goods_sn")) ? config.SnPrefix + SiteUtils.CreatGoodsSn() : DYRequest.getFormString("goods_sn");
            entity.goods_name = DYRequest.getFormString("goods_name");
            entity.goods_subname = DYRequest.getFormString("goods_subname");
            entity.click_count = DYRequest.getFormInt("click_count");
            entity.goods_number = DYRequest.getFormInt("goods_number");
            entity.warn_number = DYRequest.getFormInt("warn_number");
            entity.goods_weight = DYRequest.getFormDecimal("goods_weight");
            entity.weight_unit = DYRequest.getFormString("weight_unit");
            entity.market_price = DYRequest.getFormDecimal("market_price");
            entity.shop_price = DYRequest.getFormDecimal("shop_price");
            entity.promote_price = DYRequest.getFormDecimal("promote_price");
            entity.promote_start_date = DYRequest.getFormDateTime("promote_start_date");
            entity.promote_end_date = DYRequest.getFormDateTime("promote_end_date");
            entity.tag = FunctionUtils.Text.ToDBC(DYRequest.getFormString("tag"));
            entity.goods_desc = DYRequest.getFormString("goods_desc");
            entity.goods_content = DYRequest.getFormString("goods_content").Replace("''''", "");
            entity.goods_gg = DYRequest.getFormString("goods_gg").Replace("''''", "");
            entity.original_img = DYRequest.getFormString("original_img");
            if (!string.IsNullOrEmpty(entity.original_img) && Utils.FileExists(Server.MapPath(entity.original_img)))
            {
                if (config.WatermarkGoods && Utils.FileExists(Server.MapPath(config.WatermarkPic)))
                {
                    WebGDI.GetWaterMarkPicImage(Server.MapPath(entity.original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(entity.original_img.Replace("_", "info_")), info_w, info_h);
                    WebGDI.GetWaterMarkPicImage(Server.MapPath(entity.original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(entity.original_img.Replace("_", "list_")), list_w, list_h);
                    //WebGDI.GetWaterMarkPicImage(Server.MapPath(entity.original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(entity.original_img.Replace("_", "ico_")), ico_w, ico_h);
                    WebGDI.SendSmallImage(Server.MapPath(entity.original_img), Server.MapPath(entity.original_img.Replace("_", "ico_")), ico_w, ico_h);
                }
                else
                {
                    //生成缩略图(详细页)
                    WebGDI.SendSmallImage(Server.MapPath(entity.original_img), Server.MapPath(entity.original_img.Replace("_", "info_")), info_w, info_h);
                    //生成缩略图(列表页)
                    WebGDI.SendSmallImage(Server.MapPath(entity.original_img), Server.MapPath(entity.original_img.Replace("_", "list_")), list_w, list_h);
                    //生成缩略图(ico)
                    WebGDI.SendSmallImage(Server.MapPath(entity.original_img), Server.MapPath(entity.original_img.Replace("_", "ico_")), ico_w, ico_h);
                }
            }

            entity.goods_thumb = entity.original_img.Replace("_", "ico_");
            entity.goods_img = entity.original_img.Replace("_", "list_");
            if (!string.IsNullOrEmpty(entity.goods_img) && entity.goods_img.IndexOf("/include/upload/images/") >= 0)
            {
                if (config.WatermarkPic != "")
                {
                    if (DYRequest.getFormString("goods_img") != "")
                    {
                        WebGDI.GetWaterMarkPicImage(Server.MapPath(entity.goods_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(entity.goods_img.Replace("_", "sy_")), 230, 281);

                        entity.goods_img = entity.goods_img.Replace("_", "sy_");
                    }
                }

            }
            entity.info_img = entity.original_img.Replace("_", "info_");
            entity.goods_video = DYRequest.getFormString("goods_video");
            entity.is_on_sale = DYRequest.getFormBoolean("is_on_sale");
            entity.is_alone_sale = DYRequest.getFormBoolean("is_alone_sale");
            entity.integral = DYRequest.getFormInt("integral");
            entity.add_time = string.IsNullOrEmpty(DYRequest.getFormString("add_time")) ? DateTime.Now : DYRequest.getFormDateTime("add_time");
            entity.last_update = DateTime.Now;
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.is_delete = DYRequest.getFormBoolean("is_delete");
            entity.is_import = DYRequest.getFormBoolean("is_import");
            entity.is_best = DYRequest.getFormBoolean("is_best");
            entity.is_new = DYRequest.getFormBoolean("is_new");
            entity.is_hot = DYRequest.getFormBoolean("is_hot");
            entity.is_promote = DYRequest.getFormBoolean("is_promote");
            entity.give_integral = DYRequest.getFormInt("give_integral");
            entity.lowest_quantity = DYRequest.getFormInt("lowest_quantity");
            entity.max_quantity = DYRequest.getFormInt("max_quantity");
            entity.urlrewriter = systemConfig.UrlConfig(DYRequest.getForm("urlrewriter"), entity.goods_name, 5);
            entity.pagetitle = DYRequest.getFormString("pagetitle");
            entity.pagekeywords = FunctionUtils.Text.ToDBC(DYRequest.getFormString("pagekeywords"));
            entity.pagedesc = DYRequest.getFormString("pagedesc");
            entity.info_tlp = DYRequest.getForm("info_tlp");
            entity.is_specials = DYRequest.getFormBoolean("is_specials");
            entity.is_mobile = true;
            entity.down_id = DYRequest.getFormString("down_id");
            //entity.str1 = DYRequest.getForm("str1");
            //entity.str2 = DYRequest.getForm("str2");
            //entity.str3 = DYRequest.getForm("str3");
            //entity.str4 = DYRequest.getForm("str4");
            //entity.goods_cpte = DYRequest.getForm("goods_cpte");
            entity.goods_id = base.id;



            return entity;
        }
        /// <summary>
        /// 动态生成商品相册缩略图
        /// </summary>
        protected void CreatThumbnail()
        {
            string id = DYRequest.getRequest("id");
            if (id == null)
            {
                Response.StatusCode = 404;
                Response.Write("Not Found");
                Response.End();
                return;
            }

            List<ThumbnailInfo> thumbnails = Session["file_info"] as List<ThumbnailInfo>;

            if (thumbnails == null)
            {
                Response.StatusCode = 404;
                Response.Write("Not Found");
                Response.End();
                return;
            }

            foreach (ThumbnailInfo thumb in thumbnails)
            {
                if (thumb.ID == id)
                {
                    Response.ContentType = "image/jpeg";
                    Response.BinaryWrite(thumb.Data);
                    Response.End();
                    return;
                }
            }

            // If we reach here then we didn't find the file id so return 404
            Response.StatusCode = 404;
            Response.Write("Not Found");
            Response.End();
        }
        /// <summary>
        /// 上传商品图片
        /// </summary>
        protected void Upload()
        {
            int ico_w = Utils.StrToInt(config.GoodsImgIco.Split('*')[0], 0);
            int ico_h = Utils.StrToInt(config.GoodsImgIco.Split('*')[1], 0);
            int list_w = Utils.StrToInt(config.GoodsImgList.Split('*')[0], 0);
            int list_h = Utils.StrToInt(config.GoodsImgList.Split('*')[1], 0);
            int info_w = Utils.StrToInt(config.GoodsImgInfo.Split('*')[0], 0);
            int info_h = Utils.StrToInt(config.GoodsImgInfo.Split('*')[1], 0);

            System.Drawing.Image thumbnail_image = null;
            System.Drawing.Image original_image = null;
            System.Drawing.Bitmap final_image = null;
            System.Drawing.Graphics graphic = null;
            MemoryStream ms = null;

            try
            {
                // Get the data
                HttpPostedFile jpeg_image_upload = Request.Files["Filedata"];

                // Retrieve the uploaded image
                original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);

                // Calculate the new width and height
                int width = original_image.Width;
                int height = original_image.Height;
                int target_width = 120;
                int target_height = 120;
                int new_width, new_height;

                float target_ratio = (float)target_width / (float)target_height;
                float image_ratio = (float)width / (float)height;

                if (target_ratio > image_ratio)
                {
                    new_height = target_height;
                    new_width = (int)Math.Floor(image_ratio * (float)target_height);
                }
                else
                {
                    new_height = (int)Math.Floor((float)target_width / image_ratio);
                    new_width = target_width;
                }

                new_width = new_width > target_width ? target_width : new_width;
                new_height = new_height > target_height ? target_height : new_height;

                int gallery_id = 0;

                #region 上传图片到服务器并添加到数据库
                //图片保存路径
                string imgSavePath = "/include/upload/goods/" + DateTime.Now.ToString("yyyyMMdd") + "/";

                string[] results = CommonUtils.UploadFile(jpeg_image_upload, "jpg,gif,png", 800 * 1024, true, Server.MapPath(imgSavePath));

                string OriginaImage = imgSavePath + results[1];
                string InfoImage = imgSavePath + "info" + results[1];
                string ListImage = imgSavePath + "list" + results[1];
                string ICOImage = imgSavePath + "ico" + results[1];


                if (config.WatermarkGoods && Utils.FileExists(Server.MapPath(config.WatermarkPic)))
                {
                    WebGDI.GetWaterMarkPicImage(Server.MapPath(OriginaImage), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(InfoImage), info_w, info_h);
                    WebGDI.GetWaterMarkPicImage(Server.MapPath(OriginaImage), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(ListImage), list_w, list_h);
                    //WebGDI.GetWaterMarkPicImage(Server.MapPath(OriginaImage), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(ICOImage), ico_w, ico_h);
                    WebGDI.SendSmallImage(Server.MapPath(OriginaImage), Server.MapPath(ICOImage), ico_w, ico_h);
                }
                else
                {
                    //生成缩略图(详细页)
                    WebGDI.SendSmallImage(Server.MapPath(OriginaImage), Server.MapPath(InfoImage), info_w, info_h);
                    //生成缩略图(列表页)
                    WebGDI.SendSmallImage(Server.MapPath(OriginaImage), Server.MapPath(ListImage), list_w, list_h);
                    //生成缩略图(ico)
                    WebGDI.SendSmallImage(Server.MapPath(OriginaImage), Server.MapPath(ICOImage), ico_w, ico_h);
                }


                //入库
                GoodsGalleryInfo galleryinfo = new GoodsGalleryInfo(0, base.id, "", ICOImage, ListImage, InfoImage, imgSavePath + results[1], userid, 0);

                gallery_id = SiteBLL.InsertGoodsGalleryInfo(galleryinfo);
                #endregion

                #region 生成临时缩略图
                // Create the thumbnail
                final_image = new System.Drawing.Bitmap(target_width, target_height);
                graphic = System.Drawing.Graphics.FromImage(final_image);
                graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, target_width, target_height));
                int paste_x = (target_width - new_width) / 2;
                int paste_y = (target_height - new_height) / 2;
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
                //graphic.DrawImage(thumbnail_image, paste_x, paste_y, new_width, new_height);
                graphic.DrawImage(original_image, paste_x, paste_y, new_width, new_height);

                // Store the thumbnail in the session (Note: this is bad, it will take a lot of memory, but this is just a demo)
                ms = new MemoryStream();
                final_image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Store the data in my custom Thumbnail object
                string thumbnail_id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ThumbnailInfo thumb = new ThumbnailInfo(thumbnail_id, ms.GetBuffer(), gallery_id);

                // Put it all in the Session (initialize the session if necessary)			
                List<ThumbnailInfo> thumbnails = Session["file_info"] as List<ThumbnailInfo>;
                if (thumbnails == null)
                {
                    thumbnails = new List<ThumbnailInfo>();
                    Session["file_info"] = thumbnails;
                }
                thumbnails.Add(thumb);
                #endregion

                IDictionary context = new Hashtable();
                context.Add("thumbnail_id", thumbnail_id);
                context.Add("gallery_id", gallery_id);
                context.Add("OriginaImage", OriginaImage);

                Response.StatusCode = 200;
                Response.Write(base.MakeJson(context));
            }
            catch
            {
                // If any kind of error occurs return a 500 Internal Server error
                Response.StatusCode = 500;
                Response.Write("An error occured");
                Response.End();
            }
            finally
            {
                // Clean up
                if (final_image != null) final_image.Dispose();
                if (graphic != null) graphic.Dispose();
                if (original_image != null) original_image.Dispose();
                if (thumbnail_image != null) thumbnail_image.Dispose();
                if (ms != null) ms.Close();
                Response.End();
            }
        }
        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        protected void UpdateGoodsGallery(int goods_id)
        {
            if (Request.Form["img_id"] != null)
            {
                string[] img_desc = Request.Form.GetValues("img_desc");
                string[] img_id = Request.Form.GetValues("img_id");
                string[] order_id = Request.Form.GetValues("order_id");

                string strleng = order_id.Length.ToString();

                for (int i = 0; i < img_id.Length; i++)
                {
                    //更新
                    Goods.UpdateGoodsGallery(goods_id, img_desc[i], Convert.ToInt32(img_id[i]), Convert.ToInt32(order_id[i]));
                }

                //删除临时上传且未更新所属商品的图片信息
                foreach (GoodsGalleryInfo dr in SiteBLL.GetGoodsGalleryAllList("", "admin_user_id=" + userid + " and goods_id=0"))
                {
                    //先删除服务器上对应的图片文件
                    //FileOperate.Delete(Server.MapPath(dr.goods_img), FileOperate.FsoMethod.File);
                    //FileOperate.Delete(Server.MapPath(dr.goods_thumb), FileOperate.FsoMethod.File);
                    //FileOperate.Delete(Server.MapPath(dr.info_img), FileOperate.FsoMethod.File);
                    FileOperate.Delete(Server.MapPath(dr.original_img), FileOperate.FsoMethod.File);
                }

                //重数据库中删除
                Goods.DeleteGoodsGalleryByTemp(userid);
            }
        }
        /// <summary>
        /// 保存商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        protected void SaveGoodsAttrValue(int goods_id)
        {
            if (Request.Form["attr_id_list"] != null)
            {
                string[] attr_id_list = Request.Form.GetValues("attr_id_list");
                string[] attr_value_list = Request.Form.GetValues("attr_value_list");
                string[] goods_attr_id_list = Request.Form.GetValues("goods_attr_id_list");

                //删除原来存在的商品属性值
                Goods.DeleteGoodsAttrValueByGoodsId(goods_id);

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
                        GoodsAttrInfo attrinfo = new GoodsAttrInfo(0, goods_id, attr_id, attr_value, 0, 0);

                        SiteBLL.InsertGoodsAttrInfo(attrinfo);
                    }
                }
            }
        }
        /// <summary>
        /// 保存商品扩展分类
        /// </summary>
        /// <param name="goods_id"></param>
        protected void SaveGoodsCat(int goods_id)
        {
            if (Request.Form["other_cat[]"] != null)
            {
                //删除原来存在的商品分类
                SiteBLL.DeleteGoodsCatInfo("goods_id=" + goods_id);

                foreach (string other_id in Request.Form.GetValues("other_cat[]"))
                {
                    if (Utils.StrToInt(other_id, 0) > 0)
                    {
                        GoodsCatInfo catinfo = new GoodsCatInfo(0, goods_id, Utils.StrToInt(other_id, 0));

                        SiteBLL.InsertGoodsCatInfo(catinfo);
                    }
                }
            }
        }
        /// <summary>
        /// 商品搜索
        /// </summary>
        protected void SearchGood()
        {
            string filter = "", q = DYRequest.getRequest("q");
            int cat_id = DYRequest.getRequestInt("cat_id");

            if (!string.IsNullOrEmpty(q))
                filter += "goods_name like '%" + q + "%'";

            if (cat_id > 0)
                filter += "cat_id=" + cat_id;

            StringBuilder sb = new StringBuilder();
            foreach (GoodsInfo dr in SiteBLL.GetGoodsAllList("goods_id desc", filter))
            {
                sb.Append("{\"goods_id\":\"" + dr.goods_id + "\",\"name\":\"" + dr.goods_name + "\"},");
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + sb.ToString() + "]}");
        }
        /// <summary>
        /// 取得商品详细情况
        /// </summary>
        protected void GetGoodsInfo()
        {
            int goods_id = DYRequest.getRequestInt("goods_id");
            string attr = "";

            GoodsInfo goods = SiteBLL.GetGoodsInfo(goods_id);
            Goods goodscode=new Goods();

            if (goods != null)
            {
                string html = "{";

                html += "\"goods_id\":\"" + goods.goods_id + "\",";
                html += "\"cat_name\":\"" + SiteBLL.GetGoodsCategoryInfo(goods.cat_id.Value).cat_name + "\",";
                html += "\"goods_sn\":\"" + goods.goods_sn + "\",";
                html += "\"goods_name\":\"" + goods.goods_name + "\",";
                html += "\"brand_name\":\"\",";
                html += "\"market_price\":\"" + goods.market_price + "\",";
                html += "\"shop_price\":\"" + ((goods.is_promote.Value == true && goods.promote_start_date.Value <= DateTime.Now && goods.promote_end_date.Value >= DateTime.Now) ? goods.promote_price.Value : goods.shop_price.Value) + "\",";
                html += "\"weight_unit\":\"" + goods.weight_unit + "\",";
                html += "\"user_price\":[";

                foreach (DataRow dr in Goods.GetGoodsUserRankPrice(goods_id).Select("user_price>0"))
                {
                    html += "{\"user_price\":\"" + dr["user_price"] + "\",\"rank_name\":\"" + dr["rank_name"] + "\"},";
                }

                html = html.TrimEnd(',') + "],";
                foreach (DataRow dr in goodscode.GetGoodsAttr(goods_id, goods.goods_type.Value).Rows)
                {
                    attr += "{\"attr_name\":\"" + dr["attr_name"] + "\",\"attr_value\":\"" + dr["attr_value"] + "\"},";
                }
                attr = !string.IsNullOrEmpty(attr) ? attr.Substring(0, attr.LastIndexOf(",")) : "";
                html += "\"attr_list\":[" + attr + "]";

                base.DisplayMemoryTemplate(html + "}");
            }
        }
        /// <summary>
        /// 获取用户指定的规格
        /// </summary>
        protected void GetSpec()
        {
            string spec_ids = DYRequest.getRequest("spec_ids");
            ArrayList rows = new ArrayList();
            for (int i = 1; i <= DYRequest.getRequestInt("rows"); i++)
            {
                rows.Add(i);
            }

            IDictionary context = new Hashtable();
            context.Add("specs", SiteBLL.GetGoodsSpecAllList("", "spec_id in (" + spec_ids.Remove(spec_ids.Length - 1, 1) + ")"));
            context.Add("rows", rows);

            base.DisplayTemplate(context, "goods/spec_temp", base.isajax);
        }
        /// <summary>
        /// 保存规格
        /// </summary>
        protected void SaveSpec(int goods_id, GoodsInfo goodsinfo)
        {
            //删除以前的数据
            SiteBLL.DeleteGoodsSpecIndexInfo("goods_id=" + goodsinfo.goods_id);
            SiteBLL.DeleteProductsInfo("goods_id=" + goodsinfo.goods_id);

            if (Request.Form["spec_value_id"] != null)
            {
                int i = 1;
                foreach (string str in Request.Form.GetValues("spec_value_id"))
                {
                    int spec_value_id = Utils.StrToInt(str, 0);
                    if (spec_value_id > 0)
                    {
                        ProductsInfo proinfo = new ProductsInfo();
                        proinfo.bn = goodsinfo.goods_sn + '-' + i;
                        proinfo.cost = 0;
                        proinfo.disabled = false;
                        proinfo.goods_id = goods_id;
                        proinfo.is_local_stock = true;
                        proinfo.last_modify = DateTime.Now;
                        proinfo.marketable = true;
                        proinfo.mktprice = goodsinfo.market_price.Value;
                        proinfo.name = goodsinfo.goods_name;
                        proinfo.pdt_desc = "";
                        proinfo.price = goodsinfo.shop_price;
                        proinfo.store = 0;
                        proinfo.store_place = "";
                        proinfo.unit = goodsinfo.weight_unit;
                        proinfo.uptime = DateTime.Now;
                        proinfo.weight = 0;
                        int pro_id = SiteBLL.InsertProductsInfo(proinfo);

                        //插入spec_index
                        GoodsSpecIndexInfo indexinfo = new GoodsSpecIndexInfo(0, 0, SiteBLL.GetGoodsSpecValuesInfo(spec_value_id).spec_id.Value, spec_value_id, "", goods_id, pro_id);
                        SiteBLL.InsertGoodsSpecIndexInfo(indexinfo);

                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 保存某商品的关联商品
        /// </summary>
        /// <param name="goods_id"></param>
        public void handle_link_goods(int goods_id)
        {
            string sql = "UPDATE " + DY.Config.BaseConfig.TablePrefix + "Goods_Link SET goods_id = " + goods_id + " WHERE goods_id = 0 AND admin_id = " + id + "";

            SystemConfig.SqlProcess(sql);

            sql = "UPDATE " + DY.Config.BaseConfig.TablePrefix + "Goods_Link SET link_goods_id = " + goods_id + " WHERE link_goods_id = 0 AND admin_id = " + id + "";

            SystemConfig.SqlProcess(sql);
        }

        /// <summary>
        /// 添加关联商品
        /// </summary>
        protected void add_link_goods()
        {
            object[] linked_array = DYRequest.getRequest("add_ids").Split(',');
            object[] linked_goods = DYRequest.getRequest("test").Split(',');
            int goods_id = Convert.ToInt32(linked_goods[0]);
            int type = Convert.ToInt32(linked_goods[2]);
            int is_double = Utils.StrToBool(linked_goods[1], false) == true ? 0 : 1;
            string content = "";
            string sql = "";

            for (int i = 0; i < linked_array.Length; i++)
            {
                int ids = Convert.ToInt32(linked_array[i]);
                try
                {
                    //双向关联
                    //if (is_double == 1)
                    //{
                    //    sql = "INSERT INTO " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  (goods_id, link_goods_id, is_double, admin_id,type) VALUES (" + ids + ", " + goods_id + ", " + is_double + "," + id + ",type="+type+")";

                    //    SystemConfig.SqlProcess(sql);
                    //}

                    sql = "INSERT INTO " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  (goods_id, link_goods_id, is_double, admin_id,type) VALUES (" + goods_id + ", " + ids + ", " + is_double + "," + id + ","+type+")";
                    SystemConfig.SqlProcess(sql);
                }
                catch
                {

                }
            }

            switch (type)
            {
                case 0: content = goods.Get_json_goods_list("and goods_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  where goods_id=" + goods_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                case 1: content = cms.Get_json_cms_list("and article_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  where goods_id=" + goods_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                case 2: content=down.Get_json_down_list("and down_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  where goods_id=" + goods_id + " and admin_id=" + id + " and type=" + type + ")"); break;
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
            int goods_id = Convert.ToInt32(linked_goods[0]);
            int type = Convert.ToInt32(linked_goods[2]);
            bool is_signle = Utils.StrToBool(linked_goods[1], false);

            try
            {
                string sql = "";

                if (!is_signle)
                {
                    sql = "DELETE FROM " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  WHERE link_goods_id in (" + DYRequest.getRequest("drop_ids") + ") or goods_id in (" + DYRequest.getRequest("drop_ids") + ") and type="+type;
                }
                else
                {
                    sql = "UPDATE " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  SET is_double = 0 WHERE link_goods_id = " + goods_id + " or goods_id in (" + DYRequest.getRequest("drop_ids") + ") and type=" + type;
                }

                if (goods_id == 0)
                {
                    sql += " and admin_id=" + id + "";
                }

                SystemConfig.SqlProcess(sql);

                sql = "DELETE FROM " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  WHERE goods_id = " + goods_id + " and link_goods_id in (" + DYRequest.getRequest("drop_ids") + ") and type=" + type;

                if (goods_id == 0)
                {
                    sql += " and admin_id=" + id + "";
                }

                SystemConfig.SqlProcess(sql);
            }
            catch
            {

            }

            string content = "";

            switch (type)
            {
                case 0: content = goods.Get_json_goods_list("and goods_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  where goods_id=" + goods_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                case 1: content = cms.Get_json_cms_list("and article_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  where goods_id=" + goods_id + " and admin_id=" + id + " and type=" + type + ")"); break;
                case 2: content = down.Get_json_down_list("and down_id in (select link_goods_id from " + DY.Config.BaseConfig.TablePrefix + "Goods_Link  where goods_id=" + goods_id + " and admin_id=" + id + " and type=" + type + ")"); break;
            }
            //输出json数据
            base.DisplayMemoryTemplate("{\"error\":0,\"message\":\"\",\"content\":[" + content.ToString() + "]}");
        }


    }
}
