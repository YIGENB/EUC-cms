using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

using DY.Common;
using DY.Entity;
using DY.Config;
using DY.Data;

namespace DY.Site
{
    public class Search
    {
        /// <summary>
        /// 清空搜索库数据
        /// </summary>
        public static void Truncate()
        {
            string sql = "truncate table " + DY.Config.BaseConfig.TablePrefix + "search";

            SystemConfig.SqlProcess(sql);
        }

        /// <summary>
        /// 处理搜索库
        /// </summary>
        /// <param name="title"></param>
        /// <param name="des"></param>
        /// <param name="contents"></param>
        /// <param name="tags"></param>
        /// <param name="photo"></param>
        /// <param name="type"></param>
        /// <param name="type_id"></param>
        ///<param name="click_count"></param>
        public static void ChangeSearch(string title, string des, string contents,string tags, string photo, int type, int click_count,int type_id)
        {
            SearchInfo searchinfo = new SearchInfo();
            searchinfo.title = title;
            searchinfo.des = des;
            searchinfo.photo = photo;
            searchinfo.contents = contents;
            searchinfo.type = type;
            searchinfo.type_id = type_id;
            searchinfo.date = DateTime.Now;
            searchinfo.click_count = click_count;
            searchinfo.is_delete = false;
            searchinfo.tag = tags;
            int count = Convert.ToInt32(DatabaseProvider.GetInstance().GetSearchValue("count(search_id)", "type_id=" + searchinfo.type_id + " and type=" + searchinfo.type));
            if (count > 0)
            {
                
                searchinfo.search_id = SiteBLL.GetSearchInfo("type_id=" + searchinfo.type_id + " and type=" + searchinfo.type).search_id;
                SiteBLL.UpdateSearchInfo(searchinfo);
            }
            else
                SiteBLL.InsertSearchInfo(searchinfo);
        }

        /// <summary>
        /// 处理搜索库回收站，目前只针对商品
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type_id"></param>
        /// <param name="val"></param>
        public static void ChangeSearch(int type, string type_id, object val)
        {
            string where = type_id.Split(',').Length>1?"type_id in(" + type_id + ")":"type_id=" + type_id;
            foreach (SearchInfo search in SiteBLL.GetSearchAllList("", where))
            {
                SiteBLL.UpdateSearchFieldValue("is_delete", val, search.search_id.Value);
            }
        }

        /// <summary>
        /// 删除搜索库
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type_id"></param>
        public static void DeleteSearch(int type, int type_id)
        {
            SiteBLL.DeleteSearchInfo("type_id=" + type_id + " and type=" + type);
        }

        /// <summary>
        /// 批量删除搜索库
        /// </summary>
        /// <param name="type"></param>
        /// <param name="type_id"></param>
        public static void DeleteSearch(int type, string type_id)
        {
            SiteBLL.DeleteSearchInfo("type_id in(" + type_id + ") and type=" + type);
        }
    }
}
