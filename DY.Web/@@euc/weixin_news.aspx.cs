/**
 * 功能描述：WeixinNews管理类
 * 创建时间：2014/2/22 17:13:24
 * 最后修改时间：2014/2/22 17:13:24
 * 作者：gudufy
 * 文件标识：8727e082-08dd-4741-a070-104aaec207d9
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
    public partial class weixin_news : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("weixin_news_list");

                //检测公众号id
                this.IsCheckedWeCat(base.pid);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("weixin_news_add");

                //检测公众号id
                this.IsCheckedWeCat(base.pid);

                if (ispost)
                {
                    base.id = SiteBLL.InsertWeixinNewsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加自定义图文回复");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add&pid=" + base.pid);

                    //显示提示信息
                    this.DisplayMessage("自定义图文回复添加成功", 2, "?act=list&pid=" + base.pid, links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "weixin/weixin_news_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("weixin_news_edit");

                //检测公众号id
                this.IsCheckedWeCat(base.pid);

                if (ispost)
                {
                    SiteBLL.UpdateWeixinNewsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改自定义图文回复");

                    base.DisplayMessage("自定义图文回复修改成功", 2, "?act=list&pid=" + base.pid);
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetWeixinNewsInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                if (!string.IsNullOrEmpty(SiteBLL.GetWeixinNewsInfo(base.id).double_news))
                {
                    if (SiteBLL.GetWeixinNewsInfo(base.id).radio_type==1)
                        context.Add("newslist", SiteBLL.GetCmsAllList("", "article_id in(" + SiteBLL.GetWeixinNewsInfo(base.id).double_news + ")"));
                    else if(SiteBLL.GetWeixinNewsInfo(base.id).radio_type==2)
                        context.Add("goodslist", SiteBLL.GetGoodsAllList("", "goods_id in(" + SiteBLL.GetWeixinNewsInfo(base.id).double_news + ")"));
                }
                base.DisplayTemplate(context, "weixin/weixin_news_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("weixin_news_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateWeixinNewsFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改自定义图文回复");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("weixin_news_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateWeixinNewsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("weixin_news_del", true);

                //检测公众号id
                this.IsCheckedWeCat(base.pid);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteWeixinNewsInfo("replay_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除自定义图文回复");
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
                this.IsChecked("weixin_news_del", true);

                //执行删除
                SiteBLL.DeleteWeixinNewsInfo(base.id);

                //日志记录
                base.AddLog("删除自定义图文回复");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 获取自定义回复内容列表
            else if (this.act == "GetWeixinNews")
            {
                string key = DYRequest.getRequest("key");
                int type = DYRequest.getRequestInt("type");
                IDictionary context = new Hashtable();
                context.Add("list", SiteBLL.GetWeixinNewsAllList("", "keyword like '%" + key + "%' or title like '%" + key + "%'"));
                context.Add("type", type);

                base.DisplayTemplate(context, "weixin/weixin_news_temp", true);
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = " and pid=" + base.pid;

            this.GetList("weixin/weixin_news_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetWeixinNewsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("replay_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);
            context.Add("type", DYRequest.getRequest("type"));

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected WeixinNewsInfo SetEntity()
        {
            WeixinNewsInfo entity = new WeixinNewsInfo();

            entity.keyword = DYRequest.getForm("keyword");
            entity.type = DYRequest.getFormInt("type");
            entity.title = DYRequest.getForm("title");
            entity.des = DYRequest.getForm("des");
            entity.pic = DYRequest.getForm("pic");
            entity.content = DYRequest.getForm("content");
            entity.url = DYRequest.getForm("url");
            entity.date = DateTime.Now;//DYRequest.getFormDateTime("date");
            entity.enabled = DYRequest.getFormBoolean("enabled");
            entity.radio_type = DYRequest.getFormInt("radio_type");
            entity.double_news = DYRequest.getForm("double_news");
            entity.replay_id = base.id;
            entity.pid = base.pid;

            return entity;
        }
    }
}


