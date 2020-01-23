/**
 * 功能描述：Payment管理类
 * 创建时间：2010-3-15 下午 15:27:04
 * 最后修改时间：2010-3-15 下午 15:27:04
 * 作者：gudufy
 * 文件标识：f7ef4fa3-01b0-4250-b8d5-26e3407d8435
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
    public partial class payment : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("payment_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("payment_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertPaymentInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加支付方式");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("支付方式添加成功", 2, "?act=list", links);
                }
                
                IDictionary context = new Hashtable();
                context.Add("payment_list", Payment.GetPayments().Rows);
                
                base.DisplayTemplate(context, "payments/payment_install");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("payment_edit");

                if (ispost)
                {
                    SiteBLL.UpdatePaymentInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改支付方式" );

                    base.DisplayMessage("支付方式修改成功", 2, "?act=list");
                }

                PaymentInfo paymentinfo = SiteBLL.GetPaymentInfo(base.id);

                IDictionary context = new Hashtable();
                context.Add("entity", paymentinfo);
                context.Add("paymentinfo", Payment.GetPayments().Select("code='" + paymentinfo.pay_code + "'")[0]);

                base.DisplayTemplate(context, "payments/payment_edit");
            }
            #endregion

            #region 安装支付方式
            else if (base.act == "install_payment")
            {
                //检测权限
                this.IsChecked("payment_add");

                InstallPayment();
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("payment_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdatePaymentFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改支付方式");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("payment_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdatePaymentFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("payment_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeletePaymentInfo("article_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除支付方式");
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
                this.IsChecked("payment_del", true);

                //执行删除
                SiteBLL.DeletePaymentInfo(base.id);

                //日志记录
                base.AddLog("删除支付方式");

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
            string filter = "";

            this.GetList("payments/payment_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetPaymentList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("pay_order desc,pay_id asc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("cat_id", DYRequest.getRequestInt("cat_id"));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected PaymentInfo SetEntity()
        {
            PaymentInfo entity = new PaymentInfo();

            entity.pay_code = DYRequest.getForm("pay_code");
            entity.pay_name = DYRequest.getForm("pay_name");
            entity.pay_fee = DYRequest.getForm("pay_fee");
            entity.pay_desc = DYRequest.getForm("pay_desc");
            entity.pay_order = DYRequest.getFormInt("pay_order");
            entity.pay_config = DYRequest.getForm("pay_config");
            entity.is_cod = DYRequest.getFormBoolean("is_cod");
            entity.is_online = DYRequest.getFormBoolean("is_online");
            entity.pay_id = base.id;

            return entity;
        }
        /// <summary>
        /// 安装支付方式
        /// </summary>
        protected void InstallPayment()
        {
            IDictionary context = new Hashtable();
            context.Add("paymentinfo", Payment.GetPayments().Select("code='"+ DYRequest.getRequest("code") +"'")[0]);

            base.DisplayTemplate(context, "payments/payment_install", base.isajax);
        }
    }
}


