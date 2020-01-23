using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Entity;
using DY.Data;

namespace DY.Site
{
    /// <summary>
    /// 菜单管理类
    /// </summary>
    public class MenuManage
    {
        /// <summary>
        /// 一级菜单
        /// </summary>
        private static readonly string adminMenuMainPath = Utils.GetMapPath("/config/adminmain.config");

        /// <summary>
        /// 二级菜单
        /// </summary>
        private static readonly string adminMenuSubPath = Utils.GetMapPath("/config/adminsub.config");

        /// <summary>
        /// 三级菜单
        /// </summary>
        private static readonly string adminMenuThreePath = Utils.GetMapPath("/config/adminthree.config");

        /// <summary>
        /// 微信菜单
        /// </summary>
        private static readonly string wecatPath = Utils.GetMapPath("/config/wecatmenu.config");

        /// <summary>
        /// 获取后台一级菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminMainMenu()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(adminMenuMainPath);

            return ds.Tables[0];
        }
        /// <summary>
        /// 获取后台二级菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminSubMenu()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(adminMenuSubPath);

            return ds.Tables[0];
        }

        /// <summary>
        /// 获取后台三级菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminThreeMenu()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(adminMenuThreePath);

            return ds.Tables[0];
        }

        /// <summary>
        /// 获取后台微信菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetWeCatMenu()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(wecatPath);

            return ds.Tables[0];
        }

        /// <summary>
        /// 取得所有菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminMenuAllList()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("admin_menu", "*", "", "");
        }
        /// <summary>
        /// 取得菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminMenuList(int parent_id)
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("admin_menu", "*", "", parent_id >= 0 ? "parent_id=" + parent_id + " and isshow=1" : "isshow=1");
        }
        /// <summary>
        /// 添加到导航栏
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="add"></param>
        public static void AddToNav(string url,string name,bool add)
        {
            if (add) //在导航栏显示
            {
                NavigateInfo navinfo = new NavigateInfo(0, name, true, 0, false, url, "主导航", true,false,"",0,"");

                SiteBLL.InsertNavigateInfo(navinfo);
            }
            else  //从导航栏删除
            {
                SiteBLL.DeleteNavigateInfo("url='" + url + "' and type='主导航'");
            }
        }
    }
}
