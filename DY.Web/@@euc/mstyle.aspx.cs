/**
 * 功能描述：MStyle管理类
 * 创建时间：2014/4/3 17:20:20
 * 最后修改时间：2014/4/3 17:20:20
 * 作者：gudufy
 * 文件标识：c904adb3-6ea2-4475-810a-33ab610914da
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
    public partial class mstyle : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("m_style_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("m_style_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertMStyleInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加3g风格");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("3g风格添加成功", 2, "tlp.aspx?act=color", links);
                }

                IDictionary context = new Hashtable();
                context.Add("pathlist", FileOperate.getDirectoryInfos(System.Web.HttpContext.Current.Server.MapPath("/mobile/"), FileOperate.FsoMethod.Folder).Rows);
                base.DisplayTemplate(context, "mstyle/mstyle_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                //this.IsChecked("m_style_edit");

                if (ispost)
                {
                    SiteBLL.UpdateMStyleInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改3g风格");

                    base.DisplayMessage("3g风格修改成功", 2, "tlp.aspx?act=color");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetMStyleInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("pathlist", FileOperate.getDirectoryInfos(System.Web.HttpContext.Current.Server.MapPath("/mobile/"), FileOperate.FsoMethod.Folder).Rows);

                base.DisplayTemplate(context, "mstyle/mstyle_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("m_style_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateMStyleFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改3g风格");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("m_style_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateMStyleFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("m_style_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteMStyleInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除3g风格");
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
                this.IsChecked("m_style_del", true);

                //执行删除
                SiteBLL.DeleteMStyleInfo(base.id);

                //日志记录
                base.AddLog("删除3g风格");

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

            this.GetList("mstyle/mstyle_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetMStyleList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected MStyleInfo SetEntity()
        {
            MStyleInfo entity = new MStyleInfo();

            entity.top_bg_color = DYRequest.getForm("top_bg_color");
            entity.top_search_bg_color = DYRequest.getForm("top_search_bg_color");
            entity.menu_bg_color = DYRequest.getForm("menu_bg_color");
            entity.in_menu_bg_color = DYRequest.getForm("in_menu_bg_color");
            entity.menu_font_color = DYRequest.getForm("menu_font_color");
            entity.in_menu_font_color = DYRequest.getForm("in_menu_font_color");
            entity.b_menu_bg_color = DYRequest.getForm("b_menu_bg_color");
            entity.b_in_menu_bg_color = DYRequest.getForm("b_in_menu_bg_color");
            entity.b_menu_font_color = DYRequest.getForm("b_menu_font_color");
            entity.b_in_menu_font_color = DYRequest.getForm("b_in_menu_font_color");
            entity.bg_color = DYRequest.getForm("bg_color");
            entity.content_color = DYRequest.getForm("content_color");
            entity.content_link_color = DYRequest.getForm("content_link_color");
            entity.list_bg_color = DYRequest.getForm("list_bg_color");
            entity.title_font_color = DYRequest.getForm("title_font_color");
            entity.title_content_font_color = DYRequest.getForm("title_content_font_color");
            entity.foot_bg_color = DYRequest.getForm("foot_bg_color");
            entity.foot_font_color = DYRequest.getForm("foot_font_color");
            entity.in_foot_font_color = DYRequest.getForm("in_foot_font_color");
            //entity.is_checked =DYRequest.getFormBoolean("is_checked");
            entity.copyright_bg_color = DYRequest.getForm("copyright_bg_color");
            entity.copyright_font_color = DYRequest.getForm("copyright_font_color");
            entity.list_class_border_color = DYRequest.getForm("list_class_border_color");
            entity.list_class_font_color = DYRequest.getForm("list_class_font_color");
            entity.list_class_all_border_color = DYRequest.getForm("list_class_all_border_color");
            entity.list_class_bg_color = DYRequest.getForm("list_class_bg_color");
            entity.list_class_ification_bg_color = DYRequest.getForm("list_class_ification_bg_color");
            entity.content_updown_bg_color = DYRequest.getForm("content_updown_bg_color");
            if (base.userid == 0)
            {
                entity.date = DateTime.Now;
                entity.style_name = DYRequest.getForm("style_name");
                entity.is_custom = DYRequest.getFormBoolean("is_custom");
                entity.skin_path = DYRequest.getForm("skin_path");
                entity.style_img = string.IsNullOrEmpty(DYRequest.getForm("style_img")) ? "/mobile/" + entity.skin_path + "/preview.jpg" : DYRequest.getForm("style_img");
                entity.des = DYRequest.getForm("des");
                entity.is_sign = DYRequest.getFormBoolean("is_sign");
                entity.sort_order = DYRequest.getFormInt("sort_order");
            }

            entity.index_logo = DYRequest.getForm("index_logo");
            entity.index_bg = DYRequest.getForm("index_bg");

            entity.id = base.id;

            foreach (MStyleInfo mstyleinfo in SiteBLL.GetMStyleAllList("", ""))
            {
                if (mstyleinfo.id == base.id)
                {
                    mstyleinfo.is_checked = true;
                    mstyleinfo.id = base.id;
                    Utils.SaveConfig("WapSkinPath", "/mobile/" + mstyleinfo.skin_path + "/");
                }
                else
                    mstyleinfo.is_checked = false;
                SiteBLL.UpdateMStyleInfo(mstyleinfo);
            }

            return entity;
        }
    }
}


