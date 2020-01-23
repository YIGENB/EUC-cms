/**
 * 功能描述：Promotion管理类
 * 创建时间：2010-5-13 12:04:07
 * 最后修改时间：2010-5-13 12:04:07
 * 作者：gudufy
 * 文件标识：574e85c5-92b2-4fea-9c5d-898a92a0a7a8
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
    public partial class promotion : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("promotion_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("promotion_add");

                if (ispost)
                {
                    if (!IsEnbale(DYRequest.getFormInt("pid"), 0))
                    {
                        base.id = SiteBLL.InsertPromotionInfo(this.SetEntity());

                        //日志记录
                        base.AddLog("添加promotion");

                        Hashtable links = new Hashtable();
                        links.Add("继续添加", "?act=add");

                        //显示提示信息
                        this.DisplayMessage("promotion添加成功", 2, "?act=list", links);
                    }
                    else
                    {
                        base.DisplayMessage("推广计划修改失败，此推广ID已经存在！", 2, "?act=list");
                    }
                }
               

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "promotion/promotion_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("promotion_edit");

                if (ispost)
                {
                    if (!IsEnbale(DYRequest.getFormInt("pid"), base.id))
                    {
                        SiteBLL.UpdatePromotionInfo(this.SetEntity());

                        //日志记录
                        base.AddLog("推广计划修改");

                        base.DisplayMessage("推广计划修改成功", 2, "?act=list");
                    }
                    else
                    {
                        base.DisplayMessage("推广计划修改失败，此推广ID已经存在！", 2, "?act=list");
                    }
                }
               
                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetPromotionInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "promotion/promotion_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("promotion_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdatePromotionFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("推广计划修改");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("promotion_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdatePromotionFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("promotion_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeletePromotionInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除promotion");
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
                this.IsChecked("promotion_del", true);

                //执行删除
                SiteBLL.DeletePromotionInfo(base.id);
                
                //日志记录
                base.AddLog("删除promotion");

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

            this.GetList("promotion/promotion_list", filter);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetPromotionList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("website", "http://" + new SiteUtils().GetDomain());
            base.DisplayTemplate(context, tpl, base.isajax);
        }

        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected PromotionInfo SetEntity()
        {
            PromotionInfo entity = new PromotionInfo();

            entity.pid = DYRequest.getFormInt("pid");
            entity.website = DYRequest.getForm("website").ToLower();
            entity.remark = DYRequest.getForm("remark");

            entity.input_time = DateTime.Now;
            entity.cost =Utils.StrToDecimal(DYRequest.getForm("cost").ToString(),0);
            entity.id = base.id;
            entity.is_default = Utils.StrToBool(DYRequest.getForm("is_default"), false);
            return entity;
        }

        /// <summary>
        /// 判断推广ID是否存在
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        protected bool IsEnbale(int pid,int id)
        {
            Entity.PromotionInfo model;
            if (id == 0)
            {
                model = SiteBLL.GetPromotionInfo("pid=" + pid);
            }
            else
            {
                model = SiteBLL.GetPromotionInfo("pid=" + pid + " and id!=" + id);
            }

            if (model == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}


