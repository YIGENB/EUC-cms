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
        public static ArrayList GetTpcommentsAllList(string FieldOrder, string Where)
        {
            return GetTpcommentsAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetTpcommentsAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("tpcomments", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    TpcommentsInfo entity = new TpcommentsInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取Tpcomments分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetTpcommentsList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetTpcommentsList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取Tpcomments分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetTpcommentsList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("tpcomments", "id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    TpcommentsInfo entity = new TpcommentsInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetTpcommentsValue("Count(id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取Tpcomments信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static TpcommentsInfo GetTpcommentsInfo(int id)
        {
            return GetTpcommentsInfo("id="+id);
        }
        /// <summary>
        /// 获取Tpcomments信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static TpcommentsInfo GetTpcommentsInfo(string filter)
        {
            TpcommentsInfo entity = new TpcommentsInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetTpcommentsInfo(filter))
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
        /// 更新Tpcomments信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdateTpcommentsInfo(TpcommentsInfo entity)
        {
            DatabaseProvider.GetInstance().UpdateTpcommentsInfo(entity);
        }
        /// <summary>
        /// 添加Tpcomments信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertTpcommentsInfo(TpcommentsInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertTpcommentsInfo(entity);
        }
        /// <summary>
        /// 更新Tpcomments指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdateTpcommentsFieldValue(string fieldName, object fieldValue, int id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("tpcomments", fieldName, fieldValue, "id", id);
        }
        /// <summary>
        /// 更新Tpcomments指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdateTpcommentsFieldValue(string fieldName, object fieldValue, string ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("tpcomments", fieldName, fieldValue, "id", ids);
        }
        /// <summary>
        /// 删除指定Tpcomments数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteTpcommentsInfo(int id)
        {
            DatabaseProvider.GetInstance().DeleteTpcommentsInfo(id);
        }
        /// <summary>
        /// 按自定义条件删除Tpcomments中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeleteTpcommentsInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("tpcomments", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsTpcomments(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsTpcomments(filter);
        }
        /// <summary>
        /// 获取Tpcomments单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetTpcommentsValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetTpcommentsValue(Fields, filter);
        }
        /// <summary>
        /// 获取Tpcomments单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetTpcommentsValue(string Fields)
        {
            return GetTpcommentsValue(Fields, "");
        }
    }
}