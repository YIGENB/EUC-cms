/**
 * 功能描述：Ad管理类
 * 创建时间：2010-1-29 12:51:21
 * 最后修改时间：2010-1-29 12:51:21
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
    public partial class ads : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("ad_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("ad_add");

                if (ispost)
                {
                    //日志记录
                    this.AddLog("添加广告：" + DYRequest.getForm("ad_name"));

                    SiteBLL.InsertAdInfo(this.SetEntity());

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("广告添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();
                context.Add("adposlist", SiteBLL.GetAdPositionAllList("position_id desc", ""));
                context.Add("sdate", DateTime.Now.Date.ToShortDateString());
                context.Add("edate", DateTime.Now.AddYears(1).Date.ToShortDateString());
                base.DisplayTemplate(context, "ads/ad_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("ad_edit");

                if (ispost)
                {
                    SiteBLL.UpdateAdInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改广告");

                    int pid = DYRequest.getRequestInt("pid", 0);
                    //显示提示信息
                    base.DisplayMessage("广告修改成功", 2, "?act=list&pid=" + pid);
                }
                Entity.AdInfo model = SiteBLL.GetAdInfo(base.id);
                IDictionary context = new Hashtable();
                context.Add("entity", model);
                context.Add("adposlist", SiteBLL.GetAdPositionAllList("position_id desc", ""));
                context.Add("sdate", model.start_time.Value.ToShortDateString());
                context.Add("edate", model.end_time.Value.ToShortDateString());

                base.DisplayTemplate(context, "ads/ad_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("ad_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateAdFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改广告");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("ad_del", true);

                //日志记录
                base.AddLog("删除广告");

                //执行删除
                SiteBLL.DeleteAdInfo(base.id);

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
            string filter = " ";

            int pid = DYRequest.getRequestInt("cat_id", 0);
            if (pid == 0)
            {
                pid = DYRequest.getRequestInt("pid", 0);
            }

            if (pid != 0)
            {
                filter += " and position_id=" + pid;
            }

            context.Add("list", SiteBLL.GetAdList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("orderid desc,ad_id asc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));

            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, "ads/ad_list", base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected AdInfo SetEntity()
        {
            int media_type = DYRequest.getFormInt("media_type_value");

            AdInfo entity = new AdInfo();
            if (media_type == 1) //图片
                entity.ad_code = DYRequest.getForm("img_url");
            if (media_type == 2) //flash
                entity.ad_code = DYRequest.getForm("flash_url");
            if (media_type == 3) //轮播
                entity.ad_code = DYRequest.getForm("img_url2");
            if (media_type == 4) //文字
                entity.ad_code = DYRequest.getForm("ad_code");
            if (media_type == 5) //视频
                entity.ad_code = DYRequest.getForm("video_url");
            entity.ad_id = base.id;
            if (media_type == 1)
                entity.ad_link = DYRequest.getForm("ad_link");
            else
                entity.ad_link = DYRequest.getForm("ad_link2");
            entity.ad_name = DYRequest.getForm("ad_name");
            entity.enabled = Convert.ToBoolean(DYRequest.getFormInt("enabled"));
            if (!string.IsNullOrEmpty(DYRequest.getForm("end_time")))
                entity.end_time = Utils.StrToDataTime(DYRequest.getForm("end_time"), DateTime.Now.AddYears(1));
            entity.link_email = DYRequest.getForm("link_email");
            entity.link_man = DYRequest.getForm("link_man");
            entity.link_phone = DYRequest.getForm("link_phone");
            entity.media_type = media_type;
            entity.position_id = DYRequest.getFormInt("position_id");
            if (this.id < 1)
                entity.click_count = 0;
            if (!string.IsNullOrEmpty(DYRequest.getForm("start_time")))
                entity.start_time = Utils.StrToDataTime(DYRequest.getForm("start_time"), DateTime.Now);

            return entity;
        }
    }
}
