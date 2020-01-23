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

namespace DY.Web.admin
{
    public partial class comment : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("comment_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 查看/回复
            else if (this.act == "reply")
            {
                //检测权限
                this.IsChecked("comment_reply");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("回复留言");

                    if (DYRequest.getFormInt("parent_id") > 0)
                        SiteBLL.UpdateCommentInfo(this.SetEntity());
                    else
                        SiteBLL.InsertCommentInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("回复成功。", 2, "?act=list&type=" + DYRequest.getFormInt("comment_type"));
                }

                //更新查看状态
                SiteBLL.UpdateCommentFieldValue("is_read", 1, base.id);

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetCommentInfo(base.id));
                context.Add("re_entity", SiteBLL.GetCommentInfo("parent_id=" + base.id));

                base.DisplayTemplate(context, "comment/comment_reply");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("comment_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("更新留言");

                    //执行修改
                    SiteBLL.UpdateCommentFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("comment_edit", true);

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
                        SiteBLL.UpdateCommentFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("comment_del", true);

                //日志记录
                base.AddLog("删除留言");

                //删除该评论下的回复
                SiteBLL.DeleteCommentInfo("parent_id="+base.id);

                //执行删除
                SiteBLL.DeleteCommentInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("comment_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("删除留言");

                        //删除该评论下的回复
                        SiteBLL.DeleteCommentInfo("parent_id in(" + ids.Remove(ids.Length - 1, 1) + ")");

                        //执行删除
                        SiteBLL.DeleteCommentInfo("comment_id in (" + ids.Remove(ids.Length - 1, 1) + ")");
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
            if (Request.QueryString["type"] != null)
                filter += " and comment_type=" + DYRequest.getRequestInt("type");
            if (Request.QueryString["is_read"] != null)
                filter += " and is_read=" + DYRequest.getRequestInt("is_read");

            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetCommentList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("comment_id desc"), filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("page_size", base.pagesize);

            base.DisplayTemplate(context, "comment/comment_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected CommentInfo SetEntity()
        {
            CommentInfo entity = new CommentInfo();
            entity.comment_type = DYRequest.getFormInt("comment_type");
            entity.id_value = DYRequest.getFormInt("id_value");
            entity.email = "";
            entity.user_name = base.username;
            entity.content = DYRequest.getFormString("contents");
            entity.comment_rank = 0;
            entity.add_time = DateTime.Now;
            entity.ip_address = Utils.GetIP();
            entity.enabled = true;
            entity.parent_id = DYRequest.getFormInt("comment_id");
            entity.user_id = base.userid;
            entity.is_read = true;
            entity.comment_id = DYRequest.getFormInt("parent_id") > 0 ? DYRequest.getFormInt("parent_id") :base.id;
            entity.url = DYRequest.getFormString("url");
            entity.is_recomm = Utils.StrToBool(DYRequest.getFormString("is_recomm"), false);
            return entity;
        }
    }
}
