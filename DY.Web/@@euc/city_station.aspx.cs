/**
 * 功能描述：CityStation管理类
 * 创建时间：2016/3/2 10:40:27
 * 最后修改时间：2016/3/2 10:40:27
 * 作者：gudufy
 * 文件标识：f6a0fdda-312b-4e42-8a82-a00989fa0b63
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com
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
    public partial class city_station : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("city_station_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("city_station_add");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("添加城市分站：" + DYRequest.getForm("name"));

                    CityStationInfo entity = new CityStationInfo();
                    entity.name = DYRequest.getForm("name");
                    entity.pinyin = FunctionUtils.Text.ConvertSpellFull(DYRequest.getForm("name")).ToLower();
                    entity.other = DYRequest.getForm("other");
                    entity.pagetitle = string.IsNullOrEmpty(DYRequest.getForm("pagetitle"))?config.Title:DYRequest.getForm("pagetitle");
                    entity.pagekeywords = string.IsNullOrEmpty(DYRequest.getForm("pagekeywords")) ? config.Keywords : FunctionUtils.Text.ToDBC(DYRequest.getForm("pagekeywords"));
                    entity.pagedesc = string.IsNullOrEmpty(DYRequest.getForm("pagedesc")) ? config.Desc : DYRequest.getForm("pagedesc");
                    entity.is_enable = true;
                    entity.date = DateTime.Now;
                    SiteBLL.InsertCityStationInfo(entity);

                    base.DisplayMessage("添加成功", 2);
                }
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("city_station_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateCityStationFieldValue(fieldName, val, base.id);

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("city_station_del", true);

                //执行删除
                SiteBLL.DeleteCityStationInfo(base.id);

                //日志记录
                base.AddLog("删除城市分站");

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
            //CityStationInfo cityinfo = SiteBLL.GetCityStationInfo(region_id);

            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetCityStationAllList(SiteUtils.GetSortOrder("city_id asc"), ""));
            //context.Add("entity", regioninfo);
            
            ////to json
            //context.Add("parent_id", region_id);
            //context.Add("region_type", region_id == 0 ? 0 : regioninfo.region_type + 1);

            base.DisplayTemplate(context, "systems/city_station_list", base.isajax);
        }
    }
}
