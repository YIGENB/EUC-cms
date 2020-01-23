/**
 * 功能描述：Tpcomments管理类
 * 创建时间：2015/5/14 17:32:42
 * 最后修改时间：2015/5/14 17:32:42
 * 作者：gudufy
 * 文件标识：8ee7a5d4-9004-4db6-bbef-eccf686b424a
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
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.IO;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Text;
using Senparc.Weixin.HttpUtility;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;

namespace DY.Web.admin
{
    public partial class tpcomments : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("tpcomments_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 查看/回复
            else if (this.act == "reply")
            {

                this.IsChecked("tpcomments_replay");
                //标记为已查看
               

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetTpcommentsInfo(base.id));
                context.Add("re_entitylist", SiteBLL.GetTpcommentsAllList("id desc", "parent_id=" + SiteBLL.GetTpcommentsInfo(base.id).post_id));

                base.DisplayTemplate(context, "tpcomments/tpcomments_reply");
            }
            #endregion


            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("tpcomments_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertTpcommentsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加tpcomments");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("tpcomments添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "tpcomments/tpcomments_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("tpcomments_edit");

                if (ispost)
                {
                    SiteBLL.UpdateTpcommentsInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改tpcomments");

                    base.DisplayMessage("tpcomments修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetTpcommentsInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "tpcomments/tpcomments_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("tpcomments_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateTpcommentsFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改tpcomments");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("tpcomments_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateTpcommentsFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("tpcomments_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteTpcommentsInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除tpcomments");
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
                this.IsChecked("tpcomments_del", true);

                //执行删除
                SiteBLL.DeleteTpcommentsInfo(base.id);

                //日志记录
                base.AddLog("删除tpcomments");

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
        
            this.GetList("tpcomments/tpcomments_list", filter);
        }
        public  string HttpGet(string url, Encoding encoding = null)
        {
            WebClient wc = new WebClient();
            wc.Encoding = encoding ?? Encoding.UTF8;
            return wc.DownloadString(url);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            CommentsSimple cs = new CommentsSimple();
            CommentsFactory cf = cs.Insert("duoshuo");
            cf.Insert();
            context.Add("list", SiteBLL.GetTpcommentsList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("post_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected TpcommentsInfo SetEntity()
        {
            TpcommentsInfo entity = new TpcommentsInfo();

            entity.log_id = DYRequest.getForm("log_id");
            entity.user_id = DYRequest.getForm("user_id");
            entity.post_id = DYRequest.getForm("post_id");
            entity.thread_id = DYRequest.getForm("thread_id");
            entity.thread_key = DYRequest.getForm("thread_key");
            entity.author_id = DYRequest.getForm("author_id");
            entity.author_name = DYRequest.getForm("author_name");
            entity.ip = DYRequest.getForm("ip");
            entity.created_at = DYRequest.getFormDateTime("created_at");
            entity.message = DYRequest.getForm("message");
            entity.parent_id = DYRequest.getForm("parent_id");
            entity.action = DYRequest.getForm("action");
            entity.id = base.id;

            return entity;
        }
    }
}

