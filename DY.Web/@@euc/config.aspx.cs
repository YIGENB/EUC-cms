/**
 * 功能描述：Config管理类
 * 创建时间：2010-1-29 12:52:49
 * 最后修改时间：2010-1-29 12:52:49
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
using DY.Config;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class config : AdminPage
    {
        /// <summary>
        /// 定义本页hashtable以供模板引擎使用
        /// </summary>
        protected IDictionary context = new Hashtable();
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 用户设置list
            if (this.act == "all")
            {
                //检测权限
                this.IsChecked("site_config");

                int cid = Utils.StrToInt(DYRequest.getRequestInt("cid"), 0);

                if (ispost)
                {
                    string[] id = Request.Form.GetValues("code");
                    for (int i = 0; i < id.Length; i++)
                    {
                        SystemConfig.UpdateConfigInfo(Convert.ToInt32(id[i]), Request.Form["value[" + id[i] + "]"] == null ? "" : Request.Form["value[" + id[i] + "]"]);
                    }

                    string EnableHtml = SiteBLL.GetConfigInfo("code='enable_html'").value;
                    //导航静态页
                    foreach (NavigateInfo item in SiteBLL.GetNavigateAllList("", ""))
                    {
                        string url = item.url;
                        if (url != "/sitemap.htm" && url != "/")
                        {
                            if (EnableHtml == "1")
                            {
                                if (url.IndexOf(".aspx") > 0)
                                {
                                    url = "/html" + url.Replace(".aspx", ".html");
                                }
                            }
                            else
                            {
                                //if (url.IndexOf(".html") > 0)
                                //{
                                //    url = url.Replace("/html", "").Replace(".html", ".aspx");
                                //}
                                //删除首页文件
                                FileOperate.Delete(Server.MapPath("/index.html"), FileOperate.FsoMethod.File);
                            }
                            SiteBLL.UpdateNavigateFieldValue("url", url, item.id.Value);
                        }
                    }


                    //日志记录
                    base.AddLog("更新网站配置信息");

                    //更新缓存
                    RemoveCache.Config();
                    RemoveCache.MainNav();
                    RemoveCache.FootNav();

                    base.DisplayMessage("网站已经成功设置", 2);
                }
              IDictionary context = new Hashtable();
              context.Add("cid", cid);
              context.Add("configinfo", SiteBLL.GetConfigInfo(cid));
              context.Add("user", oluserinfo.Userid);
                //输出
              base.DisplayTemplate(context, cid == 0 ? "systems/config" : "systems/configtwo");
            }  
            #endregion

            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("config_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("config_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertConfigInfo(this.SetEntity());
                    //移除缓存
                    RemoveCache.All();
                    //日志记录
                    //base.AddLog("添加config");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("config添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "config/config_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("config_edit");

                if (ispost)
                {
                    SiteBLL.UpdateConfigInfo(this.SetEntity());
                    //移除缓存
                    RemoveCache.All();
                    //日志记录
                    //base.AddLog("修改config");

                    base.DisplayMessage("config修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetConfigInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "config/config_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("config_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateConfigFieldValue(fieldName, val, base.id);
                    //移除缓存
                    RemoveCache.All();
                    //日志记录
                    //base.AddLog("修改config");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("config_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateConfigFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("config_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteConfigInfo("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除config");
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
                this.IsChecked("config_del", true);

                //执行删除
                SiteBLL.DeleteConfigInfo(base.id);
                //移除缓存
                RemoveCache.All();
                //日志记录
                //base.AddLog("删除config");

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
            context.Add("list", caches.ConfigFormat(0).Rows);
            //context.Add("isajax", base.isajax);

            base.DisplayTemplate(context, "config/config_list", base.isajax);
        }

        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected ConfigInfo SetEntity()
        {
            ConfigInfo entity = new ConfigInfo();

            entity.parent_id = DYRequest.getFormInt("parent_id") < 0 ? 0 : DYRequest.getFormInt("parent_id");
            entity.name = DYRequest.getForm("name");
            entity.code = DYRequest.getForm("code");
            entity.type = DYRequest.getForm("type");
            entity.tip = DYRequest.getForm("tip");
            entity.size = DYRequest.getFormInt("size");
            entity.store_range = DYRequest.getForm("store_range");
            entity.store_dir = DYRequest.getForm("store_dir");
            entity.value = DYRequest.getForm("value");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.isshow = DYRequest.getFormBoolean("isshow");
            entity.id = base.id;

            return entity;
        }
    }
}
