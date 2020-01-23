/**
 * 功能描述：Navigate管理类
 * 创建时间：2010-3-2 上午 11:49:28
 * 最后修改时间：2010-3-2 上午 11:49:28
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
    public partial class navigate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("navigate_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("navigate_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加导航栏");

                    SiteBLL.InsertNavigateInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("导航栏添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("navigators/navigator_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("navigate_edit");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("修改导航栏");

                    SiteBLL.UpdateNavigateInfo(this.SetEntity());

                    //显示提示信息
                    base.DisplayMessage("导航栏修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetNavigateInfo(base.id));

                base.DisplayTemplate(context, "navigators/navigator_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("navigate_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //日志记录
                    base.AddLog("修改导航栏");

                    //执行修改
                    SiteBLL.UpdateNavigateFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("navigate_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //日志记录
                        base.AddLog("修改导航栏");

                        //执行修改
                        SiteBLL.UpdateNavigateFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("navigate_del", true);

                //日志记录
                base.AddLog("删除导航栏");

                //执行删除
                SiteBLL.DeleteNavigateInfo(base.id);

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
            context.Add("list", SiteBLL.GetNavigateAllList(SiteUtils.GetSortOrder("type desc,vieworder desc,id asc"), ""));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));

            base.DisplayTemplate(context, "navigators/navigator_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected NavigateInfo SetEntity()
        {
            NavigateInfo entity = new NavigateInfo();

            entity.id = DYRequest.getFormInt("id");
            entity.name = DYRequest.getFormString("name");
            entity.en_name = DYRequest.getFormString("en_name");
            entity.isshow = DYRequest.getFormBoolean("isshow");
            entity.vieworder = DYRequest.getFormInt("vieworder");
            entity.opennew = DYRequest.getFormBoolean("opennew");
            entity.url = DYRequest.getFormString("url");
            entity.issystem = DYRequest.getFormString("issystem") == "True" ? true : false;
            entity.is_mobile = DYRequest.getFormBoolean("is_mobile");
            entity.type = DYRequest.getFormString("type");
            entity.mobile_ico = DYRequest.getFormString("photo");
            entity.parent_id = DYRequest.getFormInt("parent_id");
            entity.id = base.id;

            return entity;
        }
    }
}
