﻿/**
 * 功能描述：Delivery管理类
 * 创建时间：2010-1-29 12:53:42
 * 最后修改时间：2010-1-29 12:53:42
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
    public partial class delivery : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("delivery_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("delivery_add");

                if (ispost)
                {
                    SiteBLL.InsertDeliveryInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("配送方式添加成功",2,"?act=list",links);
                }

                base.DisplayTemplate("deliverys/delivery_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("delivery_edit");

                if (ispost)
                {
                    SiteBLL.UpdateDeliveryInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("配送方式修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDeliveryInfo(base.id));

                base.DisplayTemplate(context, "deliverys/delivery_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("delivery_edit",true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateDeliveryFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("delivery_del", true);

                //执行删除
                SiteBLL.DeleteDeliveryInfo(base.id);

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
            context.Add("list", SiteBLL.GetDeliveryList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("orderid desc,delivery_id asc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "deliverys/delivery_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected DeliveryInfo SetEntity()
        {
            DeliveryInfo entity = new DeliveryInfo();
            entity.delivery_desc = DYRequest.getForm("delivery_desc");
            entity.delivery_name = DYRequest.getForm("delivery_name");
            entity.enabled = Convert.ToBoolean(DYRequest.getFormInt("is_show"));
            entity.support_cod = Convert.ToBoolean(DYRequest.getFormInt("support_cod"));
            entity.delivery_price = DYRequest.getFormDecimal("delivery_price");
            entity.orderid = DYRequest.getFormInt("orderid");
            entity.com = DYRequest.getForm("com");
            entity.delivery_id = base.id;
           
            return entity;
        }
    }
}
