using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Data;
using DY.Cache;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;
namespace DY.Web.admin.tree
{
    public partial class treelist : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (base.act)
            {

                case "cmspagetree":
                        CmsPageTree();//页面树表格列表
                    break;
                case "cmscattree":
                    CmsCatTree();//资讯分类树表格列表
                    break;
                case "downloadcattree":
                    DownloadCatTree();//下载分类树表格列表
                    break;
                case "cmspagecattree":
                    CmsPageCatTree();//页面树列表
                    break;
                case "cmscatlisttree":
                    CmsCatListTree();//资讯树分类列表
                    break;
                case "downloadcatlisttree":
                    DownloadCatListTree();//下载分类树列表
                    break;
                case "Goods_CategoryTree":
                    Goods_CategoryTree();//产品分类树表格列表
                    break;
                case "goods_categorylisttree":
                    Goods_CategoryListTree();//产品分类树表格列表
                    break;
                case "configtree":
                    ConfigTree();//功能设置表格列表
                    break;
                case "configlisttree":
                    ConfigListTree();//产品分类树表格列表AdminActionListTree
                    break;
                case "admin_menutree":
                    Admin_MenuTree();//菜单表格列表
                    break;
                case "admin_menulisttree":
                    Admin_MenuListTree();//菜单设置列表
                    break;
                case "adminactiontree":
                    AdminActionTree();//权限表格树列表
                    break;
                case "adminactionlisttree":
                    AdminActionListTree();//权限设置列表
                    break;
                case "webaddresstree":
                    AdminWebAddressTree();//网站地图列表
                    break;
                case "navigatorlisttree":
                    NavigatorListTree();//导航列表
                    break;
                case "navigatortree":
                    NavigatorTree();//导航列表
                    break;
            }
        }

        /// <summary>
        /// 页面表格树json
        /// </summary>
        public void CmsPageTree()
        {
            DataTable data = caches.CMSPageTable(0);
            Func<DataRow, CmsPageInfo> row2node = null;
            row2node = row => new CmsPageInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("page_id") + "").Select(r => row2node(r)).ToList(),
                page_id = row.Field<Int32>("page_id"),
                title = row.Field<string>("title"),
                is_show = row.Field<bool>("is_show"),
                order_id = row.Field<Int32>("order_id"),
                urlrewriter = row.Field<string>("urlrewriter")
                
            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 页面分类json
        /// </summary>
        public void CmsPageCatTree()
        {
            DataTable data = null;
            data = caches.CMSPageTable(0);
            DataRow newRow = data.NewRow();
            newRow["page_id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["title"] = "请选择分类...";
            data.Rows.Add(newRow);//新增列
            DataView dv = data.DefaultView;
            dv.Sort = "page_id";//排序行
            data = dv.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 资讯分类表格树json
        /// </summary>
        public void CmsCatTree()
        {
            DataTable data = caches.CMSCatTable(0);
            Func<DataRow, CmsCatInfo> row2node = null;
            row2node = row => new CmsCatInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("cat_id") + "").Select(r => row2node(r)).ToList(),
                cat_id = row.Field<Int32>("cat_id"),
                cat_name = row.Field<string>("cat_name"),
                cat_type = row.Field<Int32>("cat_type"),
                show_in_nav = row.Field<bool>("show_in_nav"),
                urlrewriter = row.Field<string>("urlrewriter"),
                sort_order = row.Field<Int32>("sort_order")
                
            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 资讯分类json
        /// </summary>
        public void CmsCatListTree()
        {
            DataTable data = null;
            data = caches.CMSCatTable(0);
            DataRow newRow = data.NewRow();
            newRow["cat_id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["cat_name"] = "请选择分类...";
            data.Rows.Add(newRow);//新增列
            DataView dv = data.DefaultView;
            dv.Sort = "cat_id";//排序行
            data = dv.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 下载分类表格树json
        /// </summary>
        public void DownloadCatTree()
        {
            DataTable data = caches.DownloadcatTable();
            Func<DataRow, DownloadCategoryInfo> row2node = null;
            row2node = row => new DownloadCategoryInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("cat_id") + "").Select(r => row2node(r)).ToList(),
                cat_id = row.Field<Int32>("cat_id"),
                cat_name = row.Field<string>("cat_name"),
                show_in_nav = row.Field<bool>("show_in_nav"),
                urlrewriter = row.Field<string>("urlrewriter"),
                sort_order = row.Field<Int32>("sort_order")

            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 下载分类json
        /// </summary>
        public void DownloadCatListTree()
        {
            DataTable data = null;
            data = caches.DownloadcatTable();
            DataRow newRow = data.NewRow();
            newRow["cat_id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["cat_name"] = "请选择分类...";
            data.Rows.Add(newRow);//新增列
            DataView dv = data.DefaultView;
            dv.Sort = "cat_id";//排序行
            data = dv.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 产品分类表格树json
        /// </summary>
        public void Goods_CategoryTree()
        {
            DataTable data = caches.Goods_CategoryTable();
            Func<DataRow, GoodsCategoryInfo> row2node = null;
            row2node = row => new GoodsCategoryInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("cat_id") + "").Select(r => row2node(r)).ToList(),
                cat_id = row.Field<Int32>("cat_id"),
                cat_name = row.Field<string>("cat_name"),
                cat_name_en = row.Field<string>("cat_name_en"),
                measure_unit = row.Field<string>("measure_unit"),
                is_show = row.Field<bool>("is_show"),
                show_in_nav = row.Field<bool>("show_in_nav"),
                is_hot = row.Field<bool>("is_hot"),
                is_mobile = row.Field<bool>("is_mobile"),
                urlrewriter = row.Field<string>("urlrewriter"),
                sort_order = row.Field<Int32>("sort_order")

            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 产品分类json
        /// </summary>
        public void Goods_CategoryListTree()
        {
            DataTable data = null;
            data = caches.Goods_CategoryTable();
            DataRow newRow = data.NewRow();
            newRow["cat_id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["cat_name"] = "请选择分类...";
            data.Rows.Add(newRow);//新增列
            DataView dv = data.DefaultView;
            dv.Sort = "cat_id";//排序行
            data = dv.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 功能设置表格树json
        /// </summary>
        public void ConfigTree()
        {
            DataTable data = caches.ConfigTable();
            Func<DataRow, ConfigInfo> row2node = null;
            row2node = row => new ConfigInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("id") + "").Select(r => row2node(r)).ToList(),
                id = row.Field<Int32>("id"),
                name = row.Field<string>("name"),
                code = row.Field<string>("code"),
                type = row.Field<string>("type"),
                tip = row.Field<string>("tip"),
                size = row.Field<Int32>("size"),
                store_range = row.Field<string>("store_range"),
                value = row.Field<string>("value"),
                isshow = row.Field<bool>("isshow"),
                sort_order = row.Field<Int32>("sort_order")

            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 功能设置树json
        /// </summary>
        public void ConfigListTree()
        {
            DataTable datalist = null;
            datalist = caches.ConfigTable();
            DataRow newRow = datalist.NewRow();
            newRow["id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["name"] = "请选择分类...";
            datalist.Rows.Add(newRow);//新增列
            DataView datev = datalist.DefaultView;
            datev.Sort = "id";//排序行
            datalist = datev.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(datalist, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 菜单表格树json
        /// </summary>
        public void Admin_MenuTree()
        {
            DataTable data = caches.AdminMenuTable();
            Func<DataRow, AdminMenuInfo> row2node = null;
            row2node = row => new AdminMenuInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("menu_id") + "").Select(r => row2node(r)).ToList(),
                menu_id = row.Field<Int32>("menu_id"),
                name = row.Field<string>("name"),
                link = row.Field<string>("link"),
                isshow = row.Field<bool>("isshow")
            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 菜单设置树json
        /// </summary>
        public void Admin_MenuListTree()
        {
            DataTable datalist = null;
            datalist = caches.AdminMenuTable();
            DataRow newRow = datalist.NewRow();
            newRow["menu_id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["name"] = "请选择分类...";
            datalist.Rows.Add(newRow);//新增列
            DataView datev = datalist.DefaultView;
            datev.Sort = "menu_id";//排序行
            datalist = datev.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(datalist, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 权限表格树json
        /// </summary>
        public void AdminActionTree()
        {
            DataTable data = caches.AdminActionTable();
            Func<DataRow, AdminActionInfo> row2node = null;
            row2node = row => new AdminActionInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("action_id") + "").Select(r => row2node(r)).ToList(),
                action_id = row.Field<Int32>("action_id"),
                action_text = row.Field<string>("action_text"),
                action_code = row.Field<string>("action_code"),
                isenable = row.Field<bool>("isenable")
            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 权限树json
        /// </summary>
        public void AdminActionListTree()
        {
            DataTable datalist = null;
            datalist = caches.AdminActionTable();
            DataRow newRow = datalist.NewRow();
            newRow["action_id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["action_text"] = "请选择分类...";
            datalist.Rows.Add(newRow);//新增列
            DataView datev = datalist.DefaultView;
            datev.Sort = "action_id";//排序行
            datalist = datev.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(datalist, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }

        /// <summary>
        /// 页面表格树json
        /// </summary>
        public void AdminWebAddressTree()
        {
            DataTable data = caches.WebAddressTable();

            Func<DataRow, WebAddressInfo> row2node = null;
            row2node = row => new WebAddressInfo
            {
                children = data.Select("[parentid]=" + row.Field<Int32>("id") + "").Select(r => row2node(r)).ToList(),
                id = row.Field<Int32>("id"),
                name = row.Field<string>("name"),
                lastmod = row.Field<DateTime>("lastmod"),
                priority = row.Field<string>("priority"),
                orderid = row.Field<Int32>("orderid"),
                isenable = row.Field<bool>("isenable")
            };
            var nodes = data.Select("[parentid]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }

        /// <summary>
        /// 导航表格树json
        /// </summary>
        public void NavigatorTree()
        {
            DataTable data = caches.NavigatorTable();

            Func<DataRow, NavigateInfo> row2node = null;
            row2node = row => new NavigateInfo
            {
                children = data.Select("[parent_id]=" + row.Field<Int32>("id") + "").Select(r => row2node(r)).ToList(),
                id = row.Field<Int32>("id"),
                name = row.Field<string>("name"),
                isshow = row.Field<bool>("isshow"),
                url = row.Field<string>("url"),
                type = row.Field<string>("type"),
                issystem = row.Field<bool>("issystem"),
                is_mobile = row.Field<bool>("is_mobile"),
                parent_id = row.Field<Int32>("parent_id"),
                opennew = row.Field<bool>("opennew"),
                vieworder = row.Field<Int32>("vieworder"),
                en_name = row.Field<string>("en_name")
            };
            var nodes = data.Select("[parent_id]='0'").Select(r => row2node(r));//linq编辑childeren里的json
            var jsonrow = new { Rows = nodes };//,Total=10
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(jsonrow, Formatting.Indented, jSetting).Replace("\"children\": [],", "");//去除空children，然后json序列化
            base.DisplayMemoryTemplate(json);
        }
        /// <summary>
        /// 导航树json
        /// </summary>
        public void NavigatorListTree()
        {
            DataTable datalist = null;
            datalist = caches.NavigatorTable();
            DataRow newRow = datalist.NewRow();
            newRow["id"] = "-1";
            newRow["parent_id"] = "0";
            newRow["name"] = "请选择分类...";
            datalist.Rows.Add(newRow);//新增列
            DataView datev = datalist.DefaultView;
            datev.Sort = "type desc";//排序行
            datalist = datev.ToTable();
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };//去除多余字段
            var json = JsonConvert.SerializeObject(datalist, Formatting.Indented, jSetting);
            base.DisplayMemoryTemplate(json);
        }
    }
}