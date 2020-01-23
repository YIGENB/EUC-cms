/**
 * 功能描述：Tag管理类
 * 创建时间：2010-1-29 12:55:49
 * 最后修改时间：2010-1-29 12:55:49
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
    public partial class tag : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("tag_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("tag_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加标签");

                    SiteBLL.InsertTagInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("Tag添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("tags/tag_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("tag_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改标签");

                    SiteBLL.UpdateTagInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("Tag修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetTagInfo(base.id));

                base.DisplayTemplate(context, "tags/tag_info");
            }
            #endregion

            #region 写入txt文件
            else if (this.act == "write")
            {
                //检测权限
                this.IsChecked("tag_write");
                    //日志记录
                    base.AddLog("标签写入txt文件");
                    string content="";
                    int i=1;
                    foreach(TagInfo tag in SiteBLL.GetTagAllList("",""))
                    {
                        if(i!=SiteBLL.GetTagAllList("","").Count)
                            content+="'"+tag.tag_name+"',";
                        else
                            content+="'"+tag.tag_name+"'";
                        i++;
                    }
                    FileOperate.WriteFile(Server.MapPath("/include/js/tags.js"), "var keylis = new Array(" + content + ");");

                    //显示提示信息
                    this.DisplayMessage("标签写入txt文件成功", 2, "?act=list");
                GetList();
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("tag_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改标签");

                    //执行修改
                    SiteBLL.UpdateTagFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("tag_del", true);

                //日志记录
                base.AddLog("删除标签");

                //执行删除
                SiteBLL.DeleteTagInfo(base.id);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("tag_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteTagInfo("tag_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除标签");
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
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetTagList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("tag_id desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "tags/tag_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected TagInfo SetEntity()
        {
            TagInfo entity = new TagInfo();

            entity.tag_id = DYRequest.getFormInt("tag_id");
            entity.tag_type = DYRequest.getRequestInt("tag_type");
            entity.tag_name = DYRequest.getFormString("tag_name");
            entity.tag_desc = DYRequest.getFormString("tag_desc");
            entity.urlrewriter = DYRequest.getForm("urlrewriter") == "" ? FunctionUtils.Text.ConvertSpellFirst(DYRequest.getForm("tag_name")) : DYRequest.getForm("urlrewriter");
            entity.tag_id = base.id;

            return entity;
        }
    }
}



