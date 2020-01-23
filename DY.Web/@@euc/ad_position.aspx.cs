/**
 * 功能描述：AdPosition管理类
 * 创建时间：2010-1-29 12:50:46
 * 最后修改时间：2010-1-29 12:50:46
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
    public partial class ad_position : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("ad_position_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("ad_position_add");

                if (ispost)
                {
                    SiteBLL.InsertAdPositionInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加广告位：" + DYRequest.getForm("position_name"));

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("广告位添加成功", 2, "?act=list", links);
                }

                base.DisplayTemplate("ads/ad_position_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("ad_position_edit");

                if (ispost)
                {
                    SiteBLL.UpdateAdPositionInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改广告位：" + DYRequest.getForm("position_name"));

                    //显示提示信息
                    base.DisplayMessage("广告位修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetAdPositionInfo(base.id));

                base.DisplayTemplate(context, "ads/ad_position_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("ad_position_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateAdPositionFieldValue(fieldName, val, base.id);
                    
                    //日志记录
                    base.AddLog("修改广告位置");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("ad_position_del", true);

                //日志记录
                base.AddLog("删除广告位：" + SiteBLL.GetAdPositionInfo(base.id).position_name);

                //执行删除
                SiteBLL.DeleteAdPositionInfo(base.id);

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
            context.Add("list", SiteBLL.GetAdPositionList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("position_id desc"), "", out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "ads/ad_position_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected AdPositionInfo SetEntity()
        {
            AdPositionInfo entity = new AdPositionInfo();
            entity.ad_height = DYRequest.getFormInt("ad_height");
            entity.ad_width = DYRequest.getFormInt("ad_width");
            entity.position_desc = DYRequest.getForm("position_desc");
            entity.position_name = DYRequest.getForm("position_name");
            entity.position_style = DYRequest.getForm("position_style");
            entity.position_url = DYRequest.getForm("position_url");
            entity.position_type = DYRequest.getFormInt("position_type");
            entity.position_id = base.id;
            entity.is_show_text = DYRequest.getFormBoolean("is_show_text");
            return entity;
        }
    }
}
