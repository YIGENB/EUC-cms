/**
 * 功能描述：GoodsSpecValues管理类
 * 创建时间：2010-3-18 下午 16:07:40
 * 最后修改时间：2010-3-18 下午 16:07:40
 * 作者：gudufy
 * 文件标识：61b5b50e-9b89-4e86-b1b9-fd1da5f0c67d
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
    public partial class goods_spec_values : AdminPage
    {
        private int spec_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.spec_id = DYRequest.getRequestInt("spec_id");

            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_spec_values_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_spec_values_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertGoodsSpecValuesInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加商品规格值：" + DYRequest.getForm("title"));

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&spec_id=" + this.spec_id);

                    //显示提示信息
                    this.DisplayMessage("商品规格值添加成功", 2, "?act=list&spec_id="+this.spec_id, links);
                }

                IDictionary context = new Hashtable();
                context.Add("spec_id", this.spec_id);
                context.Add("spec_info", SiteBLL.GetGoodsSpecInfo(this.spec_id));

                base.DisplayTemplate(context, "goods/goods_spec_values_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_spec_values_edit");

                if (ispost)
                {
                    SiteBLL.UpdateGoodsSpecValuesInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改商品规格值：" + DYRequest.getForm("title"));

                    base.DisplayMessage("商品规格值修改成功", 2, "?act=list&spec_id=" + this.spec_id);
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetGoodsSpecValuesInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("spec_id", this.spec_id);
                context.Add("spec_info", SiteBLL.GetGoodsSpecInfo(this.spec_id));

                base.DisplayTemplate(context, "goods/goods_spec_values_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_spec_values_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateGoodsSpecValuesFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改商品规格值：" + val);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("goods_spec_values_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateGoodsSpecValuesFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("goods_spec_values_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteGoodsSpecValuesInfo("spec_value_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除商品规格值");
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
                this.IsChecked("goods_spec_values_del", true);

                //执行删除
                SiteBLL.DeleteGoodsSpecValuesInfo(base.id);

                //日志记录
                base.AddLog("删除商品规格值");

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
            string filter = " and spec_id="+this.spec_id;

            this.GetList("goods/goods_spec_values_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetGoodsSpecValuesList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("spec_value_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("spec_id", this.spec_id);
            context.Add("spec_info", SiteBLL.GetGoodsSpecInfo(this.spec_id));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected GoodsSpecValuesInfo SetEntity()
        {
            GoodsSpecValuesInfo entity = new GoodsSpecValuesInfo();

            entity.spec_id = this.spec_id;
            entity.spec_value = DYRequest.getForm("spec_value");
            entity.alias = DYRequest.getForm("alias");
            entity.spec_image = DYRequest.getForm("spec_image");
            entity.p_order = DYRequest.getFormInt("p_order");
            entity.supplier_id = DYRequest.getFormInt("supplier_id");
            entity.supplier_spec_value_id = DYRequest.getFormInt("supplier_spec_value_id");
            entity.spec_value_id = base.id;

            return entity;
        }
    }
}


