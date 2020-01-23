/**
 * 功能描述：商品相关类
 * 创建时间：2010-1-29 15:17:25
 * 最后修改时间：2010-1-29 15:17:25
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 文件名：Goods.cs
 * ID：fa304b76-21b9-405d-a574-2b877543e079
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Common;
using DY.Data;
using DY.Entity;
using System.Collections;

namespace DY.Site
{
    public class Download
    {
       
        /// <summary>
        /// 取得下载分类信息
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ArrayList GetDownloadCatList(int parent_id)
        {
            return SiteBLL.GetDownloadCategoryAllList("sort_order asc", parent_id >= 0 ? "parent_id=" + parent_id : "");
        }

        /// <summary>
        /// 取得所有下载分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDownloadCatAllList()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("download_category", "*", "cat_id asc,sort_order asc", "");
        }    
        /// <summary>
        /// 获取下载目录表的所有字段
        /// </summary>
        /// <returns></returns>
        public static DataColumnCollection GetGoodsColumns()
        {
            return DatabaseProvider.GetInstance().GetTableColumns("download_category").Columns;
        }
        /// <summary>
        /// 获取下载信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public static DbDataReader GetDownloadInfo(string urlrewriter)
        {
            return DatabaseProvider.GetInstance().GetDownloadInfo(urlrewriter);
        }
        /// <summary>
        /// 获取下载信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public static DbDataReader GetDownloadInfo(int download_id)
        {
            return DatabaseProvider.GetInstance().GetDownloadInfo(download_id);
        }
        /// <summary>
        /// 获取当前分类下的所有子类Id，包括当前分类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public string GetDownloadCatIds(int cat_id)
        {
            string ids = "";
            foreach (DownloadCategoryInfo catinfo in GetDownloadCatList(cat_id))
            {
                ids += catinfo.cat_id + ",";
            }

            return ids + cat_id;
        }


        /// <summary>
        /// 获取当前分类下的所有子类Id以及下面所有的子类，包括当前分类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        string idsall = "";
        public string GetDownloadCatAllIds(int cat_id)
        {
            foreach (DownloadCategoryInfo catinfo in GetDownloadCatList(cat_id))
            {
                if (catinfo.parent_id.Value != 0)
                {
                    idsall += catinfo.cat_id + ",";
                    GetDownloadCatAllIds(catinfo.cat_id.Value);
                }
                else
                {
                    idsall += catinfo.cat_id + ",";
                }
            }

            return idsall + cat_id;
        }


        /// <summary>
        /// 获取当前分类的所有父类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public static ArrayList GetPrevDownloadCat(int cat_id)
        {
            ArrayList list = new ArrayList();

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetDownloadCategoryInfo(cat_id))
            {
                while (sdr.Read())
                {
                    DownloadCategoryInfo catinfo = new DownloadCategoryInfo();
                    catinfo.cat_id = sdr.GetInt32(0);
                    catinfo.cat_name = sdr.GetString(1);

                    list.Add(catinfo);
                }
            }

            return list;
        }
        /// <summary>
        /// 插入商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        public static int InsertDownloadCategory(DownloadCategoryInfo catinfo)
        {
            return DatabaseProvider.GetInstance().InsertDownloadCategoryInfo(catinfo);
        }
     
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="cat_id"></param>
        public static void DeleteDownloadCategory(int cat_id)
        {
             DatabaseProvider.GetInstance().DeleteDownloadCategoryInfo(cat_id);
        }

        #region 关联下载操作类
        /// <summary>
        /// JSON生成商品关联数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string Get_json_down_list(string filter)
        {
            string opt = "{\"error\":0,\"message\":\"\",\"content\":[";

            foreach (DownloadInfo downinfo in SiteBLL.GetDownloadAllList("down_id desc", "down_id > 0 and is_enable=1 " + filter))
            {
                opt += "{\"value\":\"" + downinfo.down_id + "\",\"text\":\"" + downinfo.title + "\",\"data\":\"" + downinfo.down_id + "\"},";
            }

            opt = opt.TrimEnd(',') + "]}";

            return opt;
        }

        #endregion

        public DownloadInfo getDownInfo(int down_id)
        {
            return SiteBLL.GetDownloadInfo(down_id);
        }
    }
}
