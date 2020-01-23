/**
 * 功能描述：DeliveryArea管理类
 * 创建时间：2010-1-29 12:53:55
 * 最后修改时间：2010-1-29 12:53:55
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
    public partial class delivery_area : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //显示列表数据
                this.GetList();
            } 
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                if (ispost)
                {
                    SiteBLL.InsertDeliveryAreaInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("配送区域添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("deliverys/delivery_area_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                if (ispost)
                {
                    SiteBLL.UpdateDeliveryAreaInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("配送区域修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDeliveryAreaInfo(base.id));

                base.DisplayTemplate(context, "deliverys/delivery_area_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateDeliveryAreaFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //执行删除
                SiteBLL.DeleteDeliveryAreaInfo(base.id);

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
            context.Add("list", SiteBLL.GetDeliveryAreaList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("shipping_area_id desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "deliverys/delivery_area_list", base.isajax);
        }
        /// <summary>
        /// 保存
        /// </summary>
        private DeliveryAreaInfo SetEntity()
        {
            DeliveryAreaInfo entity = new DeliveryAreaInfo();
            entity.shipping_area_name = DYRequest.getForm("shipping_area_name");
            entity.shipping_id = DYRequest.getRequestInt("delivery");
            entity.basic_fee = DYRequest.getFormDecimal("basic_fee");
            entity.free_money = DYRequest.getFormDecimal("free_money");
            entity.configure = DYRequest.getForm("regions[]");
            entity.shipping_area_id = base.id;

            return entity;
        }
    }
}
