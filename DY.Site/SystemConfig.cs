using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Common;
using DY.Cache;
using DY.Data;
using DY.Entity;
using System.Collections;
using System.Text.RegularExpressions;

namespace DY.Site
{
    public class SystemConfig
    {
        /// <summary>
        /// 取得所有网站设置数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetConfigAllData()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("config", "*", "", "");
        }
        /// <summary>
        /// 取得所有权限数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAdminActionAllData()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("admin_action", "*", "", "");
        }
        /// <summary>
        /// 获取网站设置数据
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DataTable GetConfigData(int parent_id)
        {
            return DatabaseProvider.GetInstance().GetConfigList("isshow=1 and parent_id=" + parent_id);
        }
        /// <summary>
        /// 获取网站地图数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetWebAddressData()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("web_address","*","","");
            //return DatabaseProvider.GetInstance().GetAllDataToDataTable("admin_action", "*", "", "");
        }
        /// <summary>
        /// 获取导航数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNavigatorData()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("navigate", "*", "type desc,vieworder desc,id asc", "");
        }
        /// <summary>
        /// 获取微信菜单数据
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DataTable GetWeixinMenuData(int parent_id)
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("weixin_menu", "*", "", "parent_id=" + parent_id);
        }
        /// <summary>
        /// 获取网站设置数据
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public IList<ConfigInfo> GetConfigList(int parent_id)
        {
            List<ConfigInfo> list = new List<ConfigInfo>();

            using (DbDataReader rdr = DatabaseProvider.GetInstance().GetConfigData(parent_id))
            {
                while (rdr.Read())
                {
                    ConfigInfo configinfo = new ConfigInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4),
                        rdr.GetString(5), rdr.GetInt32(6), rdr.GetString(7), rdr.GetString(8), rdr.GetString(9), rdr.GetInt32(10), rdr.GetBoolean(11));

                    list.Add(configinfo);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="region_type"></param>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public ArrayList GetRegion(int region_type, int parent_id)
        {
            return SiteBLL.GetRegionAllList("region_id asc", "is_show=1 and region_type=" + region_type + " and parent_id=" + parent_id);
        }
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="region_ids"></param>
        /// <returns></returns>
        public ArrayList GetRegion(string region_ids)
        {
            return SiteBLL.GetRegionAllList("region_id asc", "region_id in (" + region_ids + ")");
        }
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public ArrayList GetAdminActionList(int parent_id)
        {
            return SiteBLL.GetAdminActionAllList("action_id asc", "parent_id=" + parent_id+" and isenable=1");
        }
        /// <summary>
        /// 根据地区ID取得地区名称
        /// </summary>
        /// <param name="region_id"></param>
        /// <returns></returns>
        public string GetRegionNameById(int region_id)
        {
            object obj = SiteBLL.GetRegionValue("region_name", "region_id=" + region_id);

            if (obj != null)
                return obj.ToString();

            return "";
        }
        /// <summary>
        /// 更新网站配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        public static void UpdateConfigInfo(int id, string val)
        {
            DatabaseProvider.GetInstance().UpdateConfigInfo(id, val);
        }

        /// Sql处理
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int SqlProcess(string sql)
        {
            return DatabaseProvider.GetInstance().SqlProcess(sql);
        }

        /// <summary>
        /// 创建内链关键词
        /// </summary>
        /// <param name="strs">字符串</param>
        /// <returns></returns>
        public string CreateLiskText(string strs)
        {
            if (string.IsNullOrEmpty(strs))
            {
                return "";
            }
            ArrayList data = Caches.InternalLink();
            if (data.Count < 1)
            {
                return strs;
            }

            foreach (Entity.InternalLinksInfo item in data)
            {
                strs = strs.Replace(item.title, "<a href='" + item.link + "'>" + item.title + "</a>");
            }
            return strs;
        }
        /// <summary>
        /// 创建内链关键词(有次数限制)
        /// </summary>
        /// <param name="strs">字符串</param>
        /// <returns></returns>
        public string CreateLiskTextFr(string strs)
        {
            if (string.IsNullOrEmpty(strs))
            {
                return "";
            }
            StringBuilder s = new StringBuilder();
            int k = 0;
            ArrayList data = Caches.InternalLink();
            if (data.Count < 1)
            {
                return strs;
            }

            foreach (Entity.InternalLinksInfo item in data)
            {
                string strsk = "";
                if (item.frequency > 0)
                {
                    for (int i = 0; i < item.frequency; i++)
                    {
                        if (i == 0)
                        {
                            k = strs.IndexOf(item.title);
                            if (k < 1)
                            {
                                strsk = strs;
                                break;
                            }
                            s.Append(strs.Substring(0, k));
                            s.Append("<a href='" + item.link + "' class='text_a'>" + item.title + "</a>");
                            strsk = strs.Substring(k + item.title.Length);
                        }
                        else
                        {
                            k = strsk.IndexOf(item.title);
                            if (k < 1)
                            {
                                break;
                            }
                            s.Append(strsk.Substring(0, k));
                            s.Append("<a href='" + item.link + "' class='text_a'>" + item.title + "</a>");
                            strsk = strsk.Substring(k + item.title.Length);
                        }
                    }
                }
                else
                {
                    strsk = strs.Replace(item.title, "<a href='" + item.link + "' class='text_a'>" + item.title + "</a>");
                }
                s.Append(strsk);
                strs = s.ToString();
                s = new StringBuilder();
            }
            return strs;
        }

        /// <summary>
        /// 创建单页分页
        /// </summary>
        /// <param name="strs">内容</param>
        /// <returns></returns>
        public string CreateDescPage(string strs)
        {
            string[] str = Regex.Split(strs, "<div style=\"page-break-after: always\"><span style=\"display: none\">&nbsp;</span></div>", RegexOptions.IgnoreCase);
            string gotopage = "";
            int k = str.Length;
            if (k > 1)
            {
                strs = "";
                for (int i = 0; i < k; i++)
                {
                    if (i == 0)
                    {
                        strs += "<!--begion" + (i + 1).ToString() + "--><div id='desc_page" + (i + 1).ToString() + "'>" + str[i] + "</div><!--end" + (i + 1).ToString() + "-->";
                        gotopage += "<a id=\"desc_page_link" + (i + 1).ToString() + "\" class=\"desc_page_link_cur\" href=\"javascript:go_desc_page('" + (i + 1).ToString() + "','" + k + "')\">" + (i + 1).ToString() + "</a>";
                    }
                    else
                    {
                        strs += "<!--begion" + (i + 1).ToString() + "--><div id='desc_page" + (i + 1).ToString() + "' style='display:none;'>" + str[i] + "</div><!--end" + (i + 1).ToString() + "-->";
                        gotopage += "<a id=\"desc_page_link" + (i + 1).ToString() + "\" class=\"desc_page_link\" href=\"javascript:go_desc_page('" + (i + 1).ToString() + "','" + k + "')\">" + (i + 1).ToString() + "</a>";
                    }
                }
                return strs + "<br/><div style='text-align:center;'>" + gotopage + "</div>";
            }
            else
            {
                return strs;
            }
        }

        /// <summary>
        /// URL自动生成设置
        /// </summary>
        /// <param name="url">自定义URL</param>
        /// <param name="title">标题</param>
        /// <param name="table">数据库表(1新闻分类表，2新闻内容表，3单页面表，4产品分类表，5产品内容表，6下载分类表，7,下载内容表...)</param>
        /// <returns></returns>
        public string UrlConfig(string url, string title, int table)
        {
            string strs = "";
            BaseConfigInfo config = DY.Config.BaseConfig.Get();
            Random r = new Random();
            if (string.IsNullOrEmpty(url) && config.Url_auto)
            {
                switch (config.Url_pinyin_type)
                {
                    case 0: strs = FunctionUtils.Text.ConvertSpellFull(title.Length > config.Url_pinyin_sublength ? title.Substring(0, config.Url_pinyin_sublength) : title).ToLower(); break;
                    case 1: strs = FunctionUtils.Text.ConvertSpellFirst(title).ToLower(); break;
                }
                //重名处理
                switch (table)
                {
                    case 1: strs = SiteBLL.ExistsCmsCat("urlrewriter='" + strs + "'") ? SiteBLL.ExistsCmsCat("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                    case 2: strs = SiteBLL.ExistsCms("urlrewriter='" + strs + "'") ? SiteBLL.ExistsCms("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                    case 3: strs = SiteBLL.ExistsCmsPage("urlrewriter='" + strs + "'") ? SiteBLL.ExistsCmsPage("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                    case 4: strs = SiteBLL.ExistsGoodsCategory("urlrewriter='" + strs + "'") ? SiteBLL.ExistsGoodsCategory("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                    case 5: strs = SiteBLL.ExistsGoods("urlrewriter='" + strs + "'") ? SiteBLL.ExistsGoods("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                    case 6: strs = SiteBLL.ExistsDownloadCategory("urlrewriter='" + strs + "'") ? SiteBLL.ExistsDownloadCategory("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                    case 7: strs = SiteBLL.ExistsDownload("urlrewriter='" + strs + "'") ? SiteBLL.ExistsDownload("urlrewriter='" + strs + r.Next(10) + "'") ? strs + r.Next(10) + r.Next(10) : strs + r.Next(10) : strs; break;
                }
            }
            else
            {
                strs = url;
            }
            return strs;
        }
    }
}
