using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Collections.Generic;


namespace DY.Web.admin
{
    public partial class goods_inquiry : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("goods_inquiry_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_inquiry_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertGoodsInquiryInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加goods_inquiry");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("goods_inquiry添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "goods_inquiry/goods_inquiry_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("goods_inquiry_edit");

                if (ispost)
                {
                    SiteBLL.UpdateGoodsInquiryInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改goods_inquiry");

                    base.DisplayMessage("goods_inquiry修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                ArrayList list_id = new ArrayList();
                DataTable goodst = new DataTable();
                goodst.Columns.Add("goods_id", typeof(int));
                goodst.Columns.Add("goods_number");
                int i = 0;
                foreach (string r in new SiteUtils().Split(SiteBLL.GetGoodsInquiryInfo(base.id).goods_id, ","))
                {
                    list_id.Add(Convert.ToInt32(r));
                    DataRow row = goodst.NewRow();
                    goodst.Rows.Add(row);
                    goodst.Rows[i]["goods_id"] = Convert.ToInt32(r);
                     i++;
                }
                i = 0;
                if (SiteBLL.GetGoodsInquiryInfo(base.id).userid > 0)
                {
                    foreach (string r in new SiteUtils().Split(SiteBLL.GetGoodsInquiryInfo(base.id).goods_number, ","))
                    {
                        goodst.Rows[i]["goods_number"] = r;
                        i++;
                    }
                }
                context.Add("goodst", goodst);
                context.Add("list_id", list_id);
                
                context.Add("entity", SiteBLL.GetGoodsInquiryInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "goods_inquiry/goods_inquiry_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("goods_inquiry_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateGoodsInquiryFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改goods_inquiry");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("goods_inquiry_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateGoodsInquiryFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("goods_inquiry_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteGoodsInquiryInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除goods_inquiry");
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
                this.IsChecked("goods_inquiry_del", true);

                //执行删除
                SiteBLL.DeleteGoodsInquiryInfo(base.id);

                //日志记录
                base.AddLog("删除goods_inquiry");

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

            this.GetList("goods_inquiry/goods_inquiry_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetGoodsInquiryList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected GoodsInquiryInfo SetEntity()
        {
            GoodsInquiryInfo entity = new GoodsInquiryInfo();

            entity.name = DYRequest.getForm("name");
            entity.company = DYRequest.getForm("company");
            entity.tel = DYRequest.getForm("tel");
            entity.address = DYRequest.getForm("address");
            entity.email = DYRequest.getForm("email");
            entity.advice = DYRequest.getForm("advice");
            entity.goods_id = DYRequest.getForm("goods_id");
            entity.id = base.id;

            return entity;
        }
    }
}