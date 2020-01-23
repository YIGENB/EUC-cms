/**
 * 功能描述：FriendLink管理类
 * 创建时间：2010-1-29 12:54:08
 * 最后修改时间：2010-1-29 12:54:08
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
    public partial class friend_link : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("friend_link_list");

                //显示列表数据
                this.GetList();
            } 
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("friend_link_add");

                if (ispost)
                {
                    SiteBLL.InsertFriendLinkInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加友情链接");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("友情链接添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("friend-links/friend_link_info");
            } 
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("friend_link_edit");

                if (ispost)
                {
                    SiteBLL.UpdateFriendLinkInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改友情链接");

                    //显示提示信息
                    base.DisplayMessage("友情链接修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetFriendLinkInfo(base.id));

                base.DisplayTemplate(context, "friend-links/friend_link_info");
            } 
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("friend_link_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改友情链接");

                    //执行修改
                    SiteBLL.UpdateFriendLinkFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("friend_link_del", true);

                //日志记录
                base.AddLog("删除友情链接");

                //执行删除
                SiteBLL.DeleteFriendLinkInfo(base.id);

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
            context.Add("list", SiteBLL.GetFriendLinkList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("show_order desc,link_id desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "friend-links/friend_link_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected FriendLinkInfo SetEntity()
        {
            FriendLinkInfo entity = new FriendLinkInfo();
            entity.is_tuijian = Convert.ToBoolean(DYRequest.getFormInt("is_tuijian"));
            entity.link_logo = DYRequest.getForm("link_logo");
            entity.link_name = DYRequest.getForm("link_name");
            entity.link_url = DYRequest.getForm("link_url");
            entity.show_order = DYRequest.getFormInt("show_order");
            entity.link_id = base.id;

            return entity;
        }
    }
}
