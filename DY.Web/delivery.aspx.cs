/**
 * 功能描述：解决方案页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
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
using DY.Config;

namespace DY.Web
{
    public partial class delivery : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.userid <= 0)
            {
                Response.Redirect("/user-login.htm");
                return;
            }

            #region 列表
            if (base.act == "list")
            {
                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (base.act == "add")
            {
                if (ispost)
                {
                    base.id = SiteBLL.InsertDeliveryAddressInfo(this.SetEntity());

                    //显示列表数据
                    Response.Redirect("/delivery-list.htm");
                    //显示提示信息
                    //base.DisplayMemoryTemplate(base.MakeJson("", 0, "添加成功"));
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "user/delivery_address_info");
            }
            #endregion

            #region 修改
            else if (base.act == "edit")
            {
                if (ispost)
                {
                    SiteBLL.UpdateDeliveryAddressInfo(this.SetEntity());

                    //显示列表数据
                    Response.Redirect("/delivery-list.htm");
                    //base.DisplayMemoryTemplate(base.MakeJson("", 0, "修改成功"));
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDeliveryAddressInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "user/delivery_address_info");
            }
            #endregion

            #region 更新单个字段值
            else if (base.act == "edit_field_value")
            {
                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateDeliveryAddressFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (base.act == "edit_field_values")
            {
                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateDeliveryAddressFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 批量彻底删除
            else if (base.act == "CompletelyDelete")
            {
                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteDeliveryAddressInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 删除记录
            else if (base.act == "remove")
            {

                //执行删除
                SiteBLL.DeleteDeliveryAddressInfo(base.id);

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
            string filter = " and userid="+base.userid;

            this.GetList("user/delivery_address_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetDeliveryAddressList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected DeliveryAddressInfo SetEntity()
        {
            DeliveryAddressInfo entity = new DeliveryAddressInfo();

            entity.consignee = DYRequest.getForm("consignee");
            entity.country = DYRequest.getFormInt("country");
            entity.province = DYRequest.getFormInt("province");
            entity.city = DYRequest.getFormInt("city");
            entity.district = DYRequest.getFormInt("district");
            entity.address = DYRequest.getForm("address");
            entity.zipcode = DYRequest.getForm("zipcode");
            entity.tel = DYRequest.getForm("tel");
            entity.mobile = DYRequest.getForm("mobile");
            entity.email = DYRequest.getForm("email");
            entity.is_checked = DYRequest.getFormBoolean("is_checked");
            entity.userid = base.userid;
            entity.id = base.id;

            return entity;
        }
    }
}
