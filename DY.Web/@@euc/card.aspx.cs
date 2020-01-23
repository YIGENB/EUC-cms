/**
 * 功能描述：活动管理类
 * 创建时间：2011-11-5 10:29:37
 * 最后修改时间：2011-11-5 10:29:37
 * 作者：gudufy
 * 文件标识：d359ce6a-2eb9-4b04-bfa5-559958e14af7
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 目前使用本表为活动库，不单单只用于活动
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
    public partial class card : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("card_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("card_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertCardInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加活动");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&atype=" + base.atype);

                    //显示提示信息
                    this.DisplayMessage("活动添加成功", 2, "?act=list&atype="+base.atype, links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "card/card_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("card_edit");

                if (ispost)
                {
                    SiteBLL.UpdateCardInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改活动");

                    base.DisplayMessage("活动修改成功", 2, "?act=list&atype=" + base.atype);
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetCardInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "card/card_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("card_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateCardFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改活动");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("card_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateCardFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("card_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteCardInfo("card_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除活动");
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
                this.IsChecked("card_del", true);

                //执行删除
                SiteBLL.DeleteCardInfo(base.id);

                //日志记录
                base.AddLog("删除活动");

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

            filter += " and type=" + base.atype;

            this.GetList("card/card_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetCardList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("card_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected CardInfo SetEntity()
        {
            CardInfo entity = new CardInfo();

            entity.user_id = DYRequest.getFormInt("user_id");
            entity.card_num = DYRequest.getForm("card_num");
            entity.is_enabled = DYRequest.getFormBoolean("is_enabled");
            entity.is_validated = DYRequest.getFormBoolean("is_validated");
            entity.use_time = DYRequest.getFormDateTime("use_time");
            entity.weixin_word = DYRequest.getForm("weixin_word");
            entity.name = DYRequest.getForm("name");
            entity.start_time = DYRequest.getFormDateTime("start_time");
            entity.end_time = DYRequest.getFormDateTime("end_time");
            entity.pic = DYRequest.getForm("photo");
            entity.des = DYRequest.getForm("des");
            entity.user_day_count = DYRequest.getFormInt("user_day_count");
            entity.day_users = DYRequest.getFormInt("day_users");
            entity.winning_rate = DYRequest.getForm("winning_rate");
            entity.type = base.atype;
            entity.card_id = base.id;

            return entity;

        }
    }
}

