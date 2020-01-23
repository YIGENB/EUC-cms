/**
 * 功能描述：GoodsType管理类
 * 创建时间：2010-1-29 15:54:06
 * 最后修改时间：2010-1-29 15:54:06
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
    public partial class goods_type : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_type_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_type_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加商品类型");

                    SiteBLL.InsertGoodsTypeInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&attr_type=" + DYRequest.getRequestInt("attr_type"));
                    
                    //显示提示信息
                    this.DisplayMessage("商品类型添加成功", 2, "?act=list&attr_type=" + DYRequest.getRequestInt("attr_type"), links);
                }
                IDictionary context = new Hashtable();
                context.Add("attr_type", DYRequest.getRequestInt("attr_type"));
                base.DisplayTemplate(context,"goods/goods_type_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_type_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改商品类型");

                    SiteBLL.UpdateGoodsTypeInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("商品类型修改成功", 2, "?act=list&attr_type=" + DYRequest.getRequestInt("attr_type"));
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetGoodsTypeInfo(base.id));
                context.Add("attr_type", DYRequest.getRequestInt("attr_type"));
                base.DisplayTemplate(context, "goods/goods_type_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_type_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改商品类型");

                    //执行修改
                    SiteBLL.UpdateGoodsTypeFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("goods_type_del", true);

                //日志记录
                base.AddLog("删除商品类型");

                //执行删除
                SiteBLL.DeleteGoodsTypeInfo(base.id);

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
            context.Add("list", SiteBLL.GetGoodsTypeList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("cat_id desc"), "attr_type=" + DYRequest.getRequestInt("attr_type"), out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("attr_type", DYRequest.getRequestInt("attr_type"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, "goods/goods_type_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected GoodsTypeInfo SetEntity()
        {
            GoodsTypeInfo entity = new GoodsTypeInfo();

            entity.cat_id = DYRequest.getFormInt("cat_id");
            entity.cat_name = DYRequest.getFormString("cat_name");
            entity.enabled = true;//DYRequest.getFormBoolean("enabled");
            entity.attr_type = DYRequest.getFormInt("attr_type");
            entity.cat_id = base.id;

            return entity;
        }
    }
}
