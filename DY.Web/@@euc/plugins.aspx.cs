/**
 * 功能描述：Weixin管理类
 * 创建时间：2014/2/20 14:10:42
 * 最后修改时间：2014/2/20 14:10:42
 * 作者：gudufy
 * 文件标识：3b9bc044-67ad-4934-9cb5-fec294bbdf7b
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
using System.Xml;

namespace DY.Web.admin
{
    public partial class plugins : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("plugin_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 安装
            if (this.act == "install")
            {
                //检测权限
                this.IsChecked("plugin_install");
                try
                {
                    //安装插件
                    Plugin.InstallPlugin(DYRequest.getRequest("name"), this);
                }
                catch (PluginInstallException p)
                {
                    this.DisplayMessage(p.ToString(), 2, "?act=list");
                }
                this.DisplayMessage("安装成功", 2, "?act=list");
            }
            #endregion


            #region 更新
            if (this.act == "update")
            {
                //检测权限
                this.IsChecked("plugin_update");
                try
                {
                    //安装插件
                    Plugin.UpdatePlugin(DYRequest.getRequest("name"), this);
                }
                catch (PluginInstallException p)
                {
                    this.DisplayMessage(p.ToString(), 2, "?act=list");
                }
            }
            #endregion


            #region 卸载
            if (this.act == "uninstall")
            {
                //检测权限
                this.IsChecked("plugin_uninstall");
                try
                {
                    //安装插件
                    Plugin.UnInstallPlugin(DYRequest.getRequest("name"), this);
                }
                catch (PluginInstallException p)
                {
                    this.DisplayMessage(p.ToString(), 2, "?act=list");
                }
                this.DisplayMessage("卸载成功", 2, "?act=list");
            }
            #endregion

            #region 读取
            if (this.act == "pluginoutput")
            {
                //检测权限
                this.IsChecked("plugin_pluginoutput");
                try
                {
                    //安装插件
                    Plugin.GetPluginOutPut(DYRequest.getRequest("name"), this);
                }
                catch (PluginInstallException p)
                {
                    this.DisplayMessage(p.ToString(), 2, "?act=list");
                }
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";

            this.GetList("plugins/plugins_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", Plugin.PluginList());

            base.DisplayTemplate(context, tpl, base.isajax);
        }
    }
}



