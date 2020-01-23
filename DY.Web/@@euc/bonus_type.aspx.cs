/**
 * 功能描述：BonusType管理类
 * 创建时间：2010/1/30 16:21:49
 * 最后修改时间：2010/1/30 16:21:49
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
    public partial class bonus_type : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("bonus_type_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("bonus_type_add");

                if (ispost)
                {
                    SiteBLL.InsertBonusTypeInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("红包类型添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("bonus/bonus_type_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("bonus_type_edit");

                if (ispost)
                {
                    SiteBLL.UpdateBonusTypeInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("红包类型修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetBonusTypeInfo(base.id));

                base.DisplayTemplate(context, "bonus/bonus_type_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("bonus_type_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateBonusTypeFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("bonus_type_del", true);

                //执行删除
                SiteBLL.DeleteBonusTypeInfo(base.id);

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
            context.Add("list", SiteBLL.GetBonusTypeList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("type_id desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("this", this);

            base.DisplayTemplate(context, "bonus/bonus_type_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected BonusTypeInfo SetEntity()
        {
            BonusTypeInfo entity = new BonusTypeInfo();
            
            entity.type_id = DYRequest.getFormInt("type_id");
            entity.type_name = DYRequest.getFormString("type_name");
            entity.type_money = DYRequest.getFormDecimal("type_money");
            entity.send_type = DYRequest.getFormInt("send_type");
            entity.min_amount = DYRequest.getFormDecimal("min_amount");
            entity.max_amount = DYRequest.getFormDecimal("max_amount");
            entity.send_start_date = DYRequest.getFormDateTime("send_start_date");
            entity.send_end_date = DYRequest.getFormDateTime("send_end_date");
            entity.use_start_date = DYRequest.getFormDateTime("use_start_date");
            entity.use_end_date = DYRequest.getFormDateTime("use_end_date");
            entity.min_goods_amount = DYRequest.getFormDecimal("min_goods_amount");
            entity.type_id = base.id;
            
            return entity;
        }
        /// <summary>
        /// 返回制卡数量
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public int GetCreatCount(int type_id)
        {
            object obj = SiteBLL.GetBonusValue("COUNT(bonus_id)", "bonus_type_id="+type_id);

            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 返回制卡数量
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public int GetEnbledCount(int type_id)
        {
            object obj = SiteBLL.GetBonusValue("COUNT(bonus_id)", "is_enbled=1 and bonus_type_id=" + type_id);

            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 返回使用数量
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public int GetUseredCount(int type_id)
        {
            object obj = SiteBLL.GetBonusValue("COUNT(bonus_id)", "used_time is not null and bonus_type_id=" + type_id);

            return Convert.ToInt32(obj);
        }
    }
}
