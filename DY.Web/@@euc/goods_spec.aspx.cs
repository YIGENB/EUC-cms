/**
 * 功能描述：GoodsSpec管理类
 * 创建时间：2010-3-1 下午 14:58:27
 * 最后修改时间：2010-3-1 下午 14:58:27
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
    public partial class goods_spec : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_spec_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_spec_add");

                if (ispost)
                {
                    SiteBLL.InsertGoodsSpecInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("商品规格添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("goods/goods_spec_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_spec_edit");

                if (ispost)
                {
                    SiteBLL.UpdateGoodsSpecInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("商品规格修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetGoodsSpecInfo(base.id));

                base.DisplayTemplate(context, "goods/goods_spec_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_spec_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateGoodsSpecFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("goods_spec_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateGoodsSpecFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("goods_spec_del", true);

                //执行删除
                SiteBLL.DeleteGoodsSpecInfo(base.id);

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
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetGoodsSpecList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("spec_id desc,p_order desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, "goods/goods_spec_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected GoodsSpecInfo SetEntity()
        {
            GoodsSpecInfo entity = new GoodsSpecInfo();

            entity.spec_name = DYRequest.getForm("spec_name");
            entity.alias = DYRequest.getForm("alias");
            entity.spec_show_type = DYRequest.getFormInt("spec_show_type");
            entity.spec_type = DYRequest.getFormInt("spec_type");
            entity.spec_memo = DYRequest.getForm("spec_memo");
            entity.p_order = DYRequest.getFormInt("p_order");
            entity.disabled = DYRequest.getFormBoolean("disabled");
            entity.supplier_spec_id = DYRequest.getFormInt("supplier_spec_id");
            entity.supplier_id = DYRequest.getFormInt("supplier_id");
            entity.lastmodify = DYRequest.getFormInt("lastmodify");
            entity.spec_id = base.id;

            return entity;
        }
    }
}
