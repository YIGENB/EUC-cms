/**
 * 功能描述：Discount管理类
 * 创建时间：2010-3-23 上午 11:06:02
 * 最后修改时间：2010-3-23 上午 11:06:02
 * 作者：gudufy
 * 文件标识：d43d8bef-34fe-4231-b90b-fc2332d6b645
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
    public partial class discount : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("discount_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("discount_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertDiscountInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加满立减规则");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("满立减规则添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "discount/discount_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("discount_edit");

                if (ispost)
                {
                    SiteBLL.UpdateDiscountInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改满立减规则：" + DYRequest.getForm("title"));

                    base.DisplayMessage("满立减规则修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetDiscountInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "discount/discount_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("discount_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (fieldName == "is_enabled")
                    {
                        if (SiteBLL.GetDiscountInfo(base.id).discount_class == "CashReturn")
                        {
                            foreach (DiscountInfo disinfo in SiteBLL.GetDiscountAllList("", "discount_class='CashReturn' and is_enabled=1"))
                            {
                                SiteBLL.UpdateDiscountFieldValue(fieldName, 0, disinfo.discount_id.Value);
                            }
                        }
                    }

                    //执行修改
                    SiteBLL.UpdateDiscountFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改满立减规则");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("discount_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateDiscountFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("discount_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteDiscountInfo("discount_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除满立减规则");
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
                this.IsChecked("discount_del", true);

                //执行删除
                SiteBLL.DeleteDiscountInfo(base.id);

                //日志记录
                base.AddLog("删除满立减规则");

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

            this.GetList("discount/discount_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetDiscountList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("discount_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected DiscountInfo SetEntity()
        {
            DiscountInfo entity = new DiscountInfo();

            entity.discount_name = DYRequest.getForm("discount_name");
            entity.discount_class = DYRequest.getForm("discount_class");
            if (entity.discount_class == "CashReturn")
                entity.discount_para = DYRequest.getForm("discount_para");
            else
                entity.discount_para = DYRequest.getForm("para_zk");
            entity.star_date = DYRequest.getFormDateTime("star_date");
            entity.end_date = DYRequest.getFormDateTime("end_date");
            entity.discount_id = base.id;

            return entity;
        }
    }
}


