/**
 * 功能描述：VoteOption管理类
 * 创建时间：2010/1/30 21:49:00
 * 最后修改时间：2010/1/30 21:49:00
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
    public partial class vote_option : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("vote_option_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("vote_option_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加投票选项");

                    SiteBLL.InsertVoteOptionInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("投票选项添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("votelist", SiteBLL.GetVoteAllList("vote_id desc", ""));
                context.Add("entity", SiteBLL.GetVoteOptionInfo(base.id));
                base.DisplayTemplate(context,"votes/vote_option_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("vote_option_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改投票选项");

                    SiteBLL.UpdateVoteOptionInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("投票选项修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("votelist", SiteBLL.GetVoteAllList("vote_id desc", ""));
                context.Add("entity", SiteBLL.GetVoteOptionInfo(base.id));

                base.DisplayTemplate(context, "votes/vote_option_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("vote_option_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改投票选项");

                    //执行修改
                    SiteBLL.UpdateVoteOptionFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("vote_option_del", true);

                //日志记录
                base.AddLog("删除投票选项");

                //执行删除
                SiteBLL.DeleteVoteOptionInfo(base.id);

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

            string sqlwhere = "";
            string vote_id = Request.QueryString["vote_id"];
            if (!string.IsNullOrEmpty(vote_id))
            {
                sqlwhere = "vote_id=" + vote_id;
            }

            context.Add("list", SiteBLL.GetVoteOptionList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("option_id desc"), sqlwhere, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "votes/vote_option_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected VoteOptionInfo SetEntity()
        {
            VoteOptionInfo entity = new VoteOptionInfo();

            entity.option_id = DYRequest.getFormInt("option_id");
            entity.vote_id = DYRequest.getFormInt("vote_id");
            entity.option_name = DYRequest.getFormString("option_name");
            entity.option_count = DYRequest.getFormInt("option_count");
            entity.option_id = base.id;

            return entity;
        }
    }
}
