//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

using DY.Common;
using DY.Cache;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    public partial class SiteBLL
    {
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetPromotionLogAllList(string FieldOrder, string Where)
        {
            return GetPromotionLogAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetPromotionLogAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("promotion_log", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    PromotionLogInfo entity = new PromotionLogInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取PromotionLog分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetPromotionLogList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetPromotionLogList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取PromotionLog分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetPromotionLogList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("promotion_log", "id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    PromotionLogInfo entity = new PromotionLogInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetPromotionLogValue("Count(id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取PromotionLog信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static PromotionLogInfo GetPromotionLogInfo(int id)
        {
            return GetPromotionLogInfo("id="+id);
        }
        /// <summary>
        /// 获取PromotionLog信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static PromotionLogInfo GetPromotionLogInfo(string filter)
        {
            PromotionLogInfo entity = new PromotionLogInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPromotionLogInfo(filter))
            {
                if (sdr.Read())
                {
                    ReaderToEntity(sdr, entity);
                }
                else
                {
                    entity = null;
                }
                sdr.Close();
            }

            return entity;
        }
        /// <summary>
        /// 更新PromotionLog信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdatePromotionLogInfo(PromotionLogInfo entity)
        {
            DatabaseProvider.GetInstance().UpdatePromotionLogInfo(entity);
        }
        /// <summary>
        /// 添加PromotionLog信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertPromotionLogInfo(PromotionLogInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertPromotionLogInfo(entity);
        }
        /// <summary>
        /// 更新PromotionLog指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdatePromotionLogFieldValue(string fieldName, object fieldValue, int id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("promotion_log", fieldName, fieldValue, "id", id);
        }
        /// <summary>
        /// 更新PromotionLog指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdatePromotionLogFieldValue(string fieldName, object fieldValue, string ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("promotion_log", fieldName, fieldValue, "id", ids);
        }
        /// <summary>
        /// 删除指定PromotionLog数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeletePromotionLogInfo(int id)
        {
            DatabaseProvider.GetInstance().DeletePromotionLogInfo(id);
        }
        /// <summary>
        /// 按自定义条件删除PromotionLog中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeletePromotionLogInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("promotion_log", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsPromotionLog(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsPromotionLog(filter);
        }
        /// <summary>
        /// 获取PromotionLog单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetPromotionLogValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetPromotionLogValue(Fields, filter);
        }
        /// <summary>
        /// 获取PromotionLog单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetPromotionLogValue(string Fields)
        {
            return GetPromotionLogValue(Fields, "");
        }
    }
}