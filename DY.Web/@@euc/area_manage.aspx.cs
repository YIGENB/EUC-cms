/**
 * 功能描述：Region管理类
 * 创建时间：2010-1-29 12:51:41
 * 最后修改时间：2010-1-29 12:51:41
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
    public partial class area_manage : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("area_list");

                //显示列表数据
                this.GetList(DYRequest.getRequestInt("parent_id"));
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("area_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加地区：" + DYRequest.getForm("region_name"));

                    RegionInfo entity = new RegionInfo();
                    entity.agency_id = 0;
                    entity.parent_id = DYRequest.getRequestInt("parent_id");
                    entity.region_name = DYRequest.getForm("region_name");
                    entity.region_type = DYRequest.getFormInt("region_type") - 1;
                    entity.is_show = true;
                    SiteBLL.InsertRegionInfo(entity);

                    base.DisplayMessage("地区添加成功", 2);
                }
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("area_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateRegionFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("area_del", true);

                int parent_id = SiteBLL.GetRegionInfo(base.id).parent_id.Value;

                //执行删除
                SiteBLL.DeleteRegionInfo(base.id);

                //显示列表数据
                this.GetList(parent_id);
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(int region_id)
        {
            RegionInfo regioninfo = SiteBLL.GetRegionInfo(region_id);

            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetRegionAllList(SiteUtils.GetSortOrder("region_id asc"), "parent_id=" + region_id));
            context.Add("entity", regioninfo);
            
            //to json
            context.Add("parent_id", region_id);
            context.Add("region_type", region_id == 0 ? 0 : regioninfo.region_type + 1);

            base.DisplayTemplate(context, "systems/area_list",base.isajax);
        }
    }
}
