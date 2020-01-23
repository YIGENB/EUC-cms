/**
 * 功能描述：城市分站相关类
 * 创建时间：2016-3-2 15:17:25
 * 最后修改时间：2016-3-2 15:17:25
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 文件名：CityStation.cs
 * ID：fa304b76-21b9-405d-a574-2b877543e079
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Text;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 城市分站数据处理类
    /// </summary>
    public class CityStation
    {
        /// <summary>
        /// 关键词增加分站城市名
        /// </summary>
        /// <param name="str">原始关键词或标题</param>
        /// <returns></returns>
        public static string ReplaceCityStationName(string str)
        {
            string newstr = "";
            string cityname = GetCityStationName();

            string sign = @",-|_、，/\";//定义关键词常用分隔符
            if (!string.IsNullOrEmpty(cityname) && !string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < sign.Length; i++)
                {
                    if (str.Contains(sign[i]))
                    {
                        for (int j = 0; j < str.Split(sign[i]).Length; j++)
                        {
                                if (j == str.Split(sign[i]).Length - 1)
                                    newstr += cityname + str.Split(sign[i])[j];
                                else
                                    newstr += cityname + str.Split(sign[i])[j] + sign[i];
                            
                        }
                        break;
                    }
                }
            }
            else
                newstr = str;
            return newstr;
        }

        /// <summary>
        /// 返回城市实体类
        /// </summary>
        /// <returns></returns>
        public static CityStationInfo GetCityStation()
        {
            BaseConfigInfo config = DY.Config.BaseConfig.Get();
            CityStationInfo citystation = SiteBLL.GetCityStationInfo("is_enable=1 and pinyin='" + new SiteUtils().GetDomainHost() + "'");
            if (citystation != null && config.Is_hotcity)
                return citystation;
            else
                return null;
        }
        /// <summary>
        /// 返回城市名
        /// </summary>
        /// <returns></returns>
        public static string GetCityStationName()
        {
            BaseConfigInfo config = DY.Config.BaseConfig.Get();
            string name = "";
            CityStationInfo citystation = SiteBLL.GetCityStationInfo("is_enable=1 and pinyin='" + new SiteUtils().GetDomainHost() + "'");
            if (citystation != null && config.Is_hotcity)
                name = citystation.name;
            return name;
        }
        /// <summary>
        /// 程序处理分站数据
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="type">CityStationDataType数据类型</param>
        public static ArrayList ManageData(ArrayList data, CityStationDataType type)
        {
            BaseConfigInfo config = DY.Config.BaseConfig.Get();
            ArrayList newList = new ArrayList();
            string cityname = "";
            CityStationInfo citystation = SiteBLL.GetCityStationInfo("is_enable=1 and pinyin='fengjie'");
            if (citystation != null && config.Is_hotcity)
           {
               cityname = citystation.name;

               switch (type)
               {
                   case CityStationDataType.GoodsCat:
                       newList = ManageGoodsCategory(data, cityname);
                       break;
                   case CityStationDataType.Goods:
                       newList = ManageGoods(data, cityname);
                       break;
                   case CityStationDataType.CMSCat:
                       newList = ManageCmsCat(data, cityname);
                       break;
                   case CityStationDataType.CMS:
                       newList = ManageCms(data, cityname);
                       break;
               }
           }
           else
               newList = data;
            return newList;
            
        }

        #region 处理默认数据展示
        /// <summary>
        /// 处理产品分类
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="cityname">分站城市名</param>
        /// <returns></returns>
        private static ArrayList ManageGoodsCategory(ArrayList data, string cityname)
        {
            ArrayList newList = new ArrayList();
            foreach (GoodsCategoryInfo entity in data)
            {
                entity.cat_name = cityname + entity.cat_name;
                newList.Add(entity);
            }
            return newList;
        }

        /// <summary>
        /// 处理产品
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="cityname">分站城市名</param>
        /// <returns></returns>
        private static ArrayList ManageGoods(ArrayList data, string cityname)
        {
            ArrayList newList = new ArrayList();
            foreach (GoodsInfo entity in data)
            {
                entity.goods_name = cityname + entity.goods_name;
                newList.Add(entity);
            }
            return newList;
        }

        /// <summary>
        /// 处理新闻分类
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="cityname">分站城市名</param>
        /// <returns></returns>
        private static ArrayList ManageCmsCat(ArrayList data, string cityname)
        {
            ArrayList newList = new ArrayList();
            foreach (CmsCatInfo entity in data)
            {
                entity.cat_name = cityname + entity.cat_name;
                newList.Add(entity);
            }
            return newList;
        }

        /// <summary>
        /// 处理新闻
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="cityname">分站城市名</param>
        /// <returns></returns>
        private static ArrayList ManageCms(ArrayList data, string cityname)
        {
            ArrayList newList = new ArrayList();
            foreach (CmsInfo entity in data)
            {
                entity.title = cityname + entity.title;
                newList.Add(entity);
            }
            return newList;
        }
        #endregion

    }


    /// <summary>
    /// 数据可选范围
    /// </summary>
    public enum CityStationDataType
    {
        /// <summary>
        /// 产品分类
        /// </summary>
        GoodsCat,
        /// <summary>
        /// 产品
        /// </summary>
        Goods,
        /// <summary>
        /// 新闻分类
        /// </summary>
        CMSCat,
        /// <summary>
        /// 新闻
        /// </summary>
        CMS,
        /// <summary>
        /// 下载分类	
        /// </summary>
        DownloadCat,
        /// <summary>
        /// 下载	
        /// </summary>
        Download,
        /// <summary>
        /// 招聘
        /// </summary>
        Job

    }
}
