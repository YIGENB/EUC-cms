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
        public static ArrayList GetFormAllAllList(string FieldOrder, string Where)
        {
            return GetFormAllAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetFormAllAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("form_all", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    FormAllInfo entity = new FormAllInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取FormAll分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetFormAllList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetFormAllList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取FormAll分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetFormAllList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("form_all", "allform_id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    FormAllInfo entity = new FormAllInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetFormAllValue("Count(allform_id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取FormAll信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static FormAllInfo GetFormAllInfo(int allform_id)
        {
            return GetFormAllInfo("allform_id="+allform_id);
        }
        /// <summary>
        /// 获取FormAll信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FormAllInfo GetFormAllInfo(string filter)
        {
            FormAllInfo entity = new FormAllInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetFormAllInfo(filter))
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
        /// 更新FormAll信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdateFormAllInfo(FormAllInfo entity)
        {
            DatabaseProvider.GetInstance().UpdateFormAllInfo(entity);
        }
        /// <summary>
        /// 添加FormAll信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertFormAllInfo(FormAllInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertFormAllInfo(entity);
        }
        /// <summary>
        /// 更新FormAll指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdateFormAllFieldValue(string fieldName, object fieldValue, int allform_id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("form_all", fieldName, fieldValue, "allform_id", allform_id);
        }
        /// <summary>
        /// 更新FormAll指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdateFormAllFieldValue(string fieldName, object fieldValue, string allform_ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("form_all", fieldName, fieldValue, "allform_id", allform_ids);
        }
        /// <summary>
        /// 删除指定FormAll数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteFormAllInfo(int allform_id)
        {
            DatabaseProvider.GetInstance().DeleteFormAllInfo(allform_id);
        }
        /// <summary>
        /// 按自定义条件删除FormAll中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeleteFormAllInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("form_all", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsFormAll(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsFormAll(filter);
        }
        /// <summary>
        /// 获取FormAll单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetFormAllValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetFormAllValue(Fields, filter);
        }
        /// <summary>
        /// 获取FormAll单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetFormAllValue(string Fields)
        {
            return GetFormAllValue(Fields, "");
        }
    }
}