/**
 * 功能描述：GoodsBrand管理类
 * 创建时间：2010-1-29 15:46:52
 * 最后修改时间：2010-1-29 15:46:52
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

namespace DY.Web.admin
{
    public partial class goods_brand : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_brand_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_brand_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加品牌");

                    SiteBLL.InsertGoodsBrandInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("品牌添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("goods/goods_brand_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_brand_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改品牌");

                    SiteBLL.UpdateGoodsBrandInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("品牌修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetGoodsBrandInfo(base.id));

                base.DisplayTemplate(context, "goods/goods_brand_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_brand_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改品牌");

                    //执行修改
                    SiteBLL.UpdateGoodsBrandFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("goods_brand_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("修改品牌");

                        //执行修改
                        SiteBLL.UpdateGoodsBrandFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("goods_brand_del", true);

                //日志记录
                base.AddLog("删除品牌");

                //执行删除
                SiteBLL.DeleteGoodsBrandInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("goods_brand_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除品牌");

                        //执行删除
                        SiteBLL.DeleteGoodsBrandInfo("brand_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetGoodsBrandList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("brand_id desc,sort_order desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, "goods/goods_brand_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected GoodsBrandInfo SetEntity()
        {
            GoodsBrandInfo entity = new GoodsBrandInfo();

            entity.brand_id = DYRequest.getFormInt("brand_id");
            entity.brand_name = DYRequest.getFormString("brand_name");
            entity.brand_logo = DYRequest.getFormString("brand_logo");
            entity.brand_desc = DYRequest.getFormString("brand_desc");
            entity.site_url = DYRequest.getFormString("site_url");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.is_show = DYRequest.getFormBoolean("is_show");
            entity.brand_id = base.id;

            return entity;
        }
    }
}
