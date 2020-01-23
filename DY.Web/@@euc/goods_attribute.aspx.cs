/**
 * 功能描述：GoodsAttribute管理类
 * 创建时间：2010-1-29 16:01:17
 * 最后修改时间：2010-1-29 16:01:17
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
    public partial class goods_attribute : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_attribute_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_attribute_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加商品属性");

                    SiteBLL.InsertGoodsAttributeInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&type_id=" + DYRequest.getRequestInt("type_id") + "&attr_type=" + DYRequest.getRequestInt("attr_type"));

                    //显示提示信息
                    this.DisplayMessage("商品属性添加成功", 2, "?act=list&type_id=" + DYRequest.getRequestInt("type_id") + "&attr_type=" + DYRequest.getRequestInt("attr_type"), links);
                }
                IDictionary context = new Hashtable();
                context.Add("attr_type", DYRequest.getRequestInt("attr_type"));
                context.Add("type_id", DYRequest.getRequestInt("type_id"));
                base.DisplayTemplate(context, "goods/goods_attribute_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_attribute_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改商品属性");

                    SiteBLL.UpdateGoodsAttributeInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("商品属性修改成功", 2, "?act=list&type_id=" + DYRequest.getFormInt("type_id") + "&attr_type=" + DYRequest.getRequestInt("attr_type"));
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetGoodsAttributeInfo(base.id));
                context.Add("type_id", DYRequest.getRequestInt("type_id"));
                context.Add("attr_type", DYRequest.getRequestInt("attr_type"));
                base.DisplayTemplate(context, "goods/goods_attribute_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_attribute_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改商品属性");

                    //执行修改
                    SiteBLL.UpdateGoodsAttributeFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("goods_attribute_del", true);

                //日志记录
                base.AddLog("删除商品属性");

                //执行删除
                SiteBLL.DeleteGoodsAttributeInfo(base.id);

                //显示列表数据
                //this.GetList();
            }
            #endregion

            #region 获取商品属性值
            else if (this.act == "GetAttr")
            {
                int goods_id = DYRequest.getRequestInt("goods_id");
                int goods_type = DYRequest.getRequestInt("goods_type");
                int article_id = DYRequest.getRequestInt("article_id");
                int goods_ids = DYRequest.getRequestInt("goods_ids");
                IDictionary context = new Hashtable();
                if (goods_ids == 1)
                    context.Add("list", goods.GetNewsAttr(article_id, goods_type).Rows);
                else
                    context.Add("list", goods.GetGoodsAttr(goods_id, goods_type).Rows);

                base.DisplayTemplate(context, "goods/attr_temp", true);
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据    (type_id)
        /// </summary>
        protected void GetList()
        {
            IDictionary context = new Hashtable();
            int attrtype = DYRequest.getRequestInt("attr_type");
            int type_id = DYRequest.getRequestInt("type_id");

            string file = "type_id=" + DYRequest.getRequest("type_id") + " and attr_type=" + DYRequest.getRequest("attr_type");
            context.Add("list", SiteBLL.GetGoodsAttributeList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("sort_order desc,attr_id asc"), file, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));

            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("type_id", DYRequest.getRequestInt("type_id"));
            context.Add("attr_type", DYRequest.getRequestInt("attr_type"));
            base.DisplayTemplate(context, "goods/goods_attribute_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected GoodsAttributeInfo SetEntity()
        {
            GoodsAttributeInfo entity = new GoodsAttributeInfo();

            entity.attr_id = DYRequest.getFormInt("attr_id");
            entity.type_id = DYRequest.getFormInt("type_id");
            entity.attr_name = DYRequest.getFormString("attr_name");
            entity.attr_input_type = DYRequest.getFormInt("attr_input_type");
            entity.attr_values = DYRequest.getFormString("attr_values");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.attr_id = base.id;
            entity.attr_type = DYRequest.getFormInt("attr_type");
            return entity;
        }
    }
}
