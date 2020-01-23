/**
 * 功能描述：Comment管理类
 * 创建时间：2010/2/7 11:02:48
 * 最后修改时间：2010/2/7 11:02:48
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

namespace DY.Web.admin
{
    public partial class feedback : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("feedback_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 查看/回复
            else if (this.act == "reply")
            {
                if (ispost)
                {
                    //检测权限
                    this.IsChecked("feedback_replay");

                    //日志记录
                    base.AddLog("回复留言反馈");

                    if (DYRequest.getFormInt("parent_id") > 0)
                    {
                        SiteBLL.UpdateFeedbackInfo(this.SetEntity());
                        FeedbackInfo feedobj = SiteBLL.GetFeedbackInfo(DYRequest.getFormInt("msg_id"));
                        string s = feedobj.user_email.ToString();
                        if (feedobj.user_email != "")
                        {
                            SiteUtils.SendMailUseGmail(feedobj.user_email, feedobj.user_name+"您好，您提交的投诉建议内容我们已经查看", "您提交的投诉建议内容'" + feedobj.msg_content + "'我们已经查看，我们的回答是：" + DYRequest.getFormString("content"));
                        }
                    }
                    else
                    {
                        SiteBLL.InsertFeedbackInfo(this.SetEntity());
                        FeedbackInfo feedobj = SiteBLL.GetFeedbackInfo(DYRequest.getFormInt("msg_id"));
                        string s = feedobj.user_email.ToString();
                        if (feedobj.user_email != "")
                        {
                            SiteUtils.SendMailUseGmail(feedobj.user_email, feedobj.user_name + "您好，您提交的投诉建议内容我们已经查看", "您提交的投诉建议内容'" + feedobj.msg_content + "'我们已经查看，我们的回答是：" + DYRequest.getFormString("content"));
                        }
                    }

                    //显示提示信息
                    base.DisplayMessage("回复成功。", 2, "?act=list");
                }

                //标记为已查看
                SiteBLL.UpdateFeedbackFieldValue("is_show", 1, base.id);

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetFeedbackInfo(base.id));
                context.Add("re_entity", SiteBLL.GetFeedbackInfo("parent_id="+base.id));

                base.DisplayTemplate(context, "feedback/feedback_reply");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("feedback_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("更新留言");

                    //执行修改
                    SiteBLL.UpdateFeedbackFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("feedback_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("更新留言");

                        //执行修改
                        SiteBLL.UpdateFeedbackFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("feedback_del", true);

                //日志记录
                base.AddLog("删除留言");

                //执行删除
                SiteBLL.DeleteFeedbackInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("feedback_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除留言");

                        //执行删除
                        SiteBLL.DeleteFeedbackInfo("msg_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
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
            string filter = "parent_id=0";
            int type = DYRequest.getRequestInt("type", 0);
            if (type != 0)
                filter += " and msg_type=" + type;
            if (DYRequest.getRequestInt("is_show", -1) >= 0)
                filter += " and is_show="+DYRequest.getRequestInt("is_show");
            if (DYRequest.getRequestInt("is_reply", -1) >= 0)
                filter += " and (msg_id NOT IN (SELECT parent_id FROM " + BaseConfig.TablePrefix + "feedback WHERE parent_id != 0))";

            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetFeedbackList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("msg_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("type", type);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, "feedback/feedback_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected FeedbackInfo SetEntity()
        {
            FeedbackInfo entity = new FeedbackInfo();
            entity.user_email = "";
            entity.user_name = base.username;
            entity.msg_content = DYRequest.getFormString("content");
            entity.msg_time = DateTime.Now;
            entity.msg_title = "";
            entity.parent_id = DYRequest.getFormInt("msg_id");
            entity.user_id = base.userid;
            entity.is_show = true;
            entity.msg_file = "";
            entity.msg_id = DYRequest.getFormInt("parent_id") > 0 ? DYRequest.getFormInt("parent_id") :base.id;
            entity.url = DYRequest.getFormString("url");
            return entity;
        }
    }
}
