/**
 * 功能描述：WeixinWord管理类
 * 创建时间：2014/2/21 15:50:12
 * 最后修改时间：2014/2/21 15:50:12
 * 作者：gudufy
 * 文件标识：2b7dca18-1f83-4905-a0f4-4004c41dd7ba
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
    public partial class weixin_word : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("weixin_word_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("weixin_word_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertWeixinWordInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加自定义文本回复");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&pid="+base.pid);

                    //显示提示信息
                    this.DisplayMessage("自定义文本回复添加成功", 2, "?act=list&pid=" + base.pid, links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "weixin/weixin_word_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("weixin_word_edit");

                if (ispost)
                {
                    SiteBLL.UpdateWeixinWordInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改自定义文本回复");

                    base.DisplayMessage("自定义文本回复修改成功", 2, "?act=list&pid=" + base.pid);
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetWeixinWordInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "weixin/weixin_word_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("weixin_word_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateWeixinWordFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改自定义文本回复");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("weixin_word_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateWeixinWordFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("weixin_word_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteWeixinWordInfo("mpword_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除自定义文本回复");
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
                this.IsChecked("weixin_word_del", true);

                //执行删除
                SiteBLL.DeleteWeixinWordInfo(base.id);

                //日志记录
                base.AddLog("删除自定义文本回复");

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
            string filter = " and pid="+base.pid;

            this.GetList("weixin/weixin_word_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetWeixinWordList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("mpword_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected WeixinWordInfo SetEntity()
        {
            WeixinWordInfo entity = new WeixinWordInfo();

            entity.keyword = DYRequest.getForm("keyword");
            entity.type = DYRequest.getFormInt("type");
            entity.content = DYRequest.getForm("content");
            entity.date = DateTime.Now;//DYRequest.getFormDateTime("date");
            entity.enabled = DYRequest.getFormBoolean("enabled");
            entity.mpword_id = base.id;
            entity.pid = base.pid;

            return entity;
        }
    }
}


