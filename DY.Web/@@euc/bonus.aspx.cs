/**
 * 功能描述：Bonus管理类
 * 创建时间：2010-3-1 下午 13:54:03
 * 最后修改时间：2010-3-1 下午 13:54:03
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
using System.Text;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class bonus : AdminPage
    {
        protected int send_type = 0;
        protected int bonus_type = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            send_type = DYRequest.getRequestInt("send_by");
            bonus_type = DYRequest.getRequestInt("bonus_type");

            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("bonus_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 生成优惠券
            else if (this.act == "send")
            {
                //检测权限
                this.IsChecked("bonus_add");

                IDictionary context = new Hashtable();

                if (send_type == 3)
                {
                    if (ispost)
                    {
                        this.SendByPrint();

                        Hashtable links = new Hashtable();
                        links.Add("继续生成", "?act=send&id="+ base.id +"&send_by=" + send_type);

                        //显示提示信息
                        this.DisplayMessage("优惠券成功", 2, "?act=list&bonus_type=" + base.id, links);
                    }

                    context.Add("type_list", SiteBLL.GetBonusTypeAllList("", "send_type=" + send_type));

                    base.DisplayTemplate(context, "bonus/bonus_send");
                }
                else if (send_type == 0)
                {
                    if (ispost)
                    {
                        this.SendByUser();

                        Hashtable links = new Hashtable();
                        links.Add("继续生成", "?act=send&id=" + base.id + "&send_by=" + send_type);

                        //显示提示信息
                        this.DisplayMessage("优惠券成功", 2, "?act=list&bonus_type=" + base.id, links);
                    }

                    base.DisplayTemplate(context, "bonus/bonus_by_user");
                }
            }
            #endregion

            #region 导出线下优惠券
            else if (this.act == "get_excel")
            {
                this.GetExcel();
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("bonus_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateBonusFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("bonus_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateBonusFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("bonus_del", true);

                //执行删除
                SiteBLL.DeleteBonusInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("bonus_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("批量删除优惠券");

                        //执行删除
                        SiteBLL.DeleteBonusInfo("bonus_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "bonus_type_id=" + bonus_type;

            this.GetList(filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetBonusList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("used_time desc,order_id desc,bonus_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("order_id", DYRequest.getRequest("order_id"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);
            context.Add("bonus_type", bonus_type);

            base.DisplayTemplate(context, "bonus/bonus_list", base.isajax);
        }
        /// <summary>
        /// 生成线下优惠券
        /// </summary>
        protected void SendByPrint()
        {
            int bonus_type_id = DYRequest.getFormInt("bonus_type_id");
            int bonus_sum = DYRequest.getFormInt("bonus_sum");

            //生成优惠券序列号
            object val = SiteBLL.GetBonusValue("MAX(bonus_sn)", "bonus_type_id=" + bonus_type_id);

            double num = !string.IsNullOrEmpty(val.ToString()) ? Math.Floor(Convert.ToDouble(val) / 10000) : 100000;

            Random rnd = new Random();
            for (int i = 0; i < bonus_sum; i++)
            {
                string bonus_sn = (num + i) + rnd.Next(0,9999).ToString().PadLeft(4,'0');

                BonusInfo bonusinfo = new BonusInfo();
                bonusinfo.bonus_sn = Convert.ToInt32(bonus_sn);
                bonusinfo.bonus_type_id = bonus_type_id;
                bonusinfo.user_id = 0;

                SiteBLL.InsertBonusInfo(bonusinfo);
            }
        }
        /// <summary>
        /// 按用户生成优惠券
        /// </summary>
        protected void SendByUser()
        {
            if (Request.Form["user[]"] == null)
            {
                base.DisplayMessage("请选择要发放优惠券的用户", 1);
            }

            int bonus_type_id = DYRequest.getFormInt("bonus_type_id");

            //生成优惠券序列号
            object val = SiteBLL.GetBonusValue("MAX(bonus_sn)", "bonus_type_id=" + bonus_type_id);

            double num = !string.IsNullOrEmpty(val.ToString()) ? Math.Floor(Convert.ToDouble(val) / 10000) : 100000;

            Random rnd = new Random();
            int i = 0;
            foreach (UsersInfo userinfo in SiteBLL.GetUsersAllList("", "user_id in (" + DYRequest.getForm("user[]") + ")"))
            { 
                int is_emailed = 0;

                //向会员优惠券表加入数据
                string bonus_sn = (num + i) + rnd.Next(0, 9999).ToString().PadLeft(4, '0');

                BonusInfo bonusinfo = new BonusInfo();
                bonusinfo.bonus_sn = Convert.ToInt32(bonus_sn);
                bonusinfo.user_id = userinfo.user_id.Value;
                bonusinfo.user_name = userinfo.user_name;
                bonusinfo.emailed = 0;
                bonusinfo.bonus_type_id = bonus_type_id;

                SiteBLL.InsertBonusInfo(bonusinfo);

                i++;
            }
        }
        /// <summary>
        /// 生成线下优惠券的excel文档
        /// </summary>
        protected void GetExcel()
        {
            BonusTypeInfo typeinfo = SiteBLL.GetBonusTypeInfo(base.id);
            StringBuilder sb = new StringBuilder();

            Response.ContentType = "application/vnd.ms-excel; charset=utf-8";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(typeinfo.type_name) + "_bonus_list.xls");
            Response.ContentEncoding = System.Text.Encoding.Default;

            sb.Append("线下优惠券信息列表\t\n");
            sb.Append("优惠券序列号\t优惠券金额\t类型名称\t使用结束日期\t\n");
            foreach (BonusInfo bonusinfo in SiteBLL.GetBonusAllList("used_time desc,bonus_id desc,order_id desc", "bonus_type_id=" + base.id))
            {
                sb.Append(bonusinfo.bonus_sn + "\t" + typeinfo.type_money + "\t" + typeinfo.type_name + "\t"+ typeinfo.use_end_date.Value.ToString("yyyy-MM-dd") +"\t\n");
            }

            base.DisplayMemoryTemplate(sb.ToString());
        }
    }
}
