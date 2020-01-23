/**
 * 功能描述：产品首页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
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
using DY.LanguagePack;

namespace DY.Web
{
    public partial class product : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Collections.ArrayList l = new ArrayList();
            
            IDictionary context = new Hashtable();
            string code = DYRequest.getRequest("code");
            string flag = DYRequest.getRequest("f");
            string k = Server.UrlDecode(DYRequest.getRequest("k"));
            int cat_id = DYRequest.getRequestInt("cat_id");
            string filter = "is_delete=0 and is_on_sale=1";
            //filter += SiteUtils.IsMobileDevice() ? " and is_mobile=1" : "";
            string tlp = "goods";
            string cat_name = "";
            string catnav = "&raquo; 产品中心";
            pagetitle = config.ProTitle;
            pagekeywords = config.ProKeywords;
            pagedesc = config.ProDesc;
            int ipagesize = config.PageSize;

            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case "new":
                        filter += " and is_new=1";
                        cat_name = " &raquo; 新品上市";
                        break;
                    case "hot":
                        filter += " and is_hot=1";
                        cat_name = " &raquo; 现货热卖";
                        break;
                    case "best":
                        filter += " and is_best=1";
                        cat_name = " &raquo; 推荐产品";
                        break;
                    case "specials":
                        filter += " and is_specials=1";
                        cat_name = " &raquo; 产品清仓";
                        break;


                    default:
                        navid = "37";
                        GoodsCategoryInfo catinfo = SiteBLL.GetGoodsCategoryInfo(string.Format("urlrewriter='{0}'", code));
                        if (catinfo == null)
                        {
                            catinfo = SiteBLL.GetGoodsCategoryInfo(string.Format("cat_id={0}", Utils.StrToInt(code, 0)));

                        }

                        if (catinfo != null)
                        {

                            if (!string.IsNullOrEmpty(catinfo.pagetitle))
                            {
                                pagetitle = catinfo.pagetitle;
                            }
                            else
                            {
                                pagetitle = catinfo.cat_name + "-" + config.ProTitle;
                            }

                            if (!string.IsNullOrEmpty(catinfo.pagekeywords))
                            {
                                pagekeywords = catinfo.pagekeywords;
                            }
                            else
                            {
                                pagekeywords = catinfo.cat_name + "," + config.ProKeywords;
                            }

                            if (!string.IsNullOrEmpty(catinfo.pagedesc))
                            {
                                pagedesc = catinfo.pagedesc;
                            }
                            else
                            {
                                pagekeywords = string.IsNullOrEmpty(catinfo.cat_desc) ? config.ProDesc : catinfo.cat_desc;
                            }

                            if (!string.IsNullOrEmpty(catinfo.list_tlp))
                            {
                                tlp = catinfo.list_tlp;
                            }
                            if (!string.IsNullOrEmpty(catinfo.list_tlp))
                            {
                                tlp = catinfo.list_tlp;
                            }
                            if (catinfo.page_size > 0)
                            {
                                ipagesize = (int)catinfo.page_size;
                            }


                            int id = catinfo.parent_id.Value;
                            if (id == 0)
                            {
                                id = catinfo.cat_id.Value;
                            }


                            int catid = Caches.GoodsCatID(catinfo.parent_id.Value, catinfo.cat_id.Value);//catinfo.parent_id > 0 ? catinfo.parent_id.Value : catinfo.cat_id.Value;
                            //通过3级分类查询2级分类 临时代码
                            //if (catinfo.cat_level == 3)
                            //{
                            //    GoodsCategoryInfo gcInfo = SiteBLL.GetGoodsCategoryInfo(catid);
                            //    catid = gcInfo.parent_id.Value;
                            //    id = catid;
                            //}
                            context.Add("cat_id", catid);
                            context.Add("this_id", catinfo.cat_id.Value);
                            //context.Add("catnavid", catinfo.cat_id.Value);
                            //导航id
                            switch (catid)
                            {
                                case 1: navid = "36"; break;
                                case 8: navid = "2"; break;
                            }

                            filter += " and cat_id in (" + goods.GetGoodsAllCatIds(catinfo.cat_id.Value) + ")";

                            //扩展分类
                            filter += !string.IsNullOrEmpty(Caches.GetGoodsCatId(catinfo.cat_id.Value).ToString()) ? " or is_delete=0 and goods_id in (" + Caches.GetGoodsCatId(catinfo.cat_id.Value).ToString() + ")" : "";

                            cat_name = SiteBLL.GetGoodsCategoryValue("cat_name", "cat_id=" + catid).ToString();

                            catnav = Caches.GoodsNav(catinfo.cat_id.Value, "");

                            context.Add("catinfo", catinfo);

                        }
                        break;
                }

            }

            if (flag == "s")
            {

                catnav = " &raquo; 产品搜索";

                if (k!="")
                {
                    //商品属性值搜索
                    //filter += " and goods_id in (select goods_id from " + DY.Config.BaseConfig.TablePrefix + "goods_attr where ";
                    //string str = "";
                    //string[] s = k.Split(',');
                    //for (int i = 0; i < s.Length; i++)
                    //{
                    //    if (i == (s.Length - 1))
                    //        str += "attr_value like '%" + s[i] + "%'";
                    //    else
                    //        str += "attr_value like '%" + s[i] + "%' or ";
                    //}
                    //    filter += str + ")";
                    filter += " and (goods_name like '%" + k.Trim() + "%' or goods_subname like '%" + k.Trim() + "%')";
                }

                if (cat_id > 0)
                    filter += " and cat_id in (" + goods.GetGoodsCatIds(cat_id) + ")";
                context.Add("key", k);
                context.Add("cat_id", cat_id);
            }

            #region 是否手机访问，显示相应列表
            //if (SiteUtils.IsMobileDevice())
            //    filter += " and is_mobile=1";
            #endregion

            context.Add("list", SiteBLL.GetGoodsList(base.pageindex, ipagesize, SiteUtils.GetSortOrder("sort_order desc,is_new desc,goods_id desc"), filter, out base.ResultCount));
            if (flag == "s")
            {
                context.Add("pager", Utils.GetStaticPageNumbers(base.pageindex, base.ResultCount, ipagesize, Request.Url.PathAndQuery, "", 5, false));
            }
            else
            {
                //string suffix = config.EnableHtml ? ".html" : ".aspx";
                string url = string.IsNullOrEmpty(code) ? urlrewrite.product + "p" : urlrewrite.product + code + "/p";

                context.Add("pager", Utils.GetWebPageNumbers(base.ResultCount, ipagesize, base.pageindex, url, config.UrlRewriterKzm, 6));
            }

            #region 显示城市推广信息
            pagetitle = CityStation.ReplaceCityStationName(pagetitle);
            pagekeywords = CityStation.ReplaceCityStationName(pagekeywords);
            #endregion

            context.Add("countPage", (base.ResultCount - 1) / ipagesize + 1);
            context.Add("pagesize", ipagesize);
            context.Add("catnav", catnav);
            context.Add("cat_name", cat_name);

            base.DisplayTemplate(context, tlp);
        }
    }
}
