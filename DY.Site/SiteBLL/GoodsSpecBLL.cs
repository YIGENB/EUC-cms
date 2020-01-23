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
        public static ArrayList GetGoodsSpecAllList(string FieldOrder, string Where)
        {
            return GetGoodsSpecAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetGoodsSpecAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("goods_spec", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    GoodsSpecInfo entity = new GoodsSpecInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取GoodsSpec分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetGoodsSpecList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetGoodsSpecList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取GoodsSpec分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetGoodsSpecList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("goods_spec", "spec_id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    GoodsSpecInfo entity = new GoodsSpecInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetGoodsSpecValue("Count(spec_id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取GoodsSpec信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static GoodsSpecInfo GetGoodsSpecInfo(int spec_id)
        {
            return GetGoodsSpecInfo("spec_id="+spec_id);
        }
        /// <summary>
        /// 获取GoodsSpec信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static GoodsSpecInfo GetGoodsSpecInfo(string filter)
        {
            GoodsSpecInfo entity = new GoodsSpecInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetGoodsSpecInfo(filter))
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
        /// 更新GoodsSpec信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdateGoodsSpecInfo(GoodsSpecInfo entity)
        {
            DatabaseProvider.GetInstance().UpdateGoodsSpecInfo(entity);
        }
        /// <summary>
        /// 添加GoodsSpec信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertGoodsSpecInfo(GoodsSpecInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertGoodsSpecInfo(entity);
        }
        /// <summary>
        /// 更新GoodsSpec指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdateGoodsSpecFieldValue(string fieldName, object fieldValue, int spec_id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("goods_spec", fieldName, fieldValue, "spec_id", spec_id);
        }
        /// <summary>
        /// 更新GoodsSpec指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdateGoodsSpecFieldValue(string fieldName, object fieldValue, string spec_ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("goods_spec", fieldName, fieldValue, "spec_id", spec_ids);
        }
        /// <summary>
        /// 删除指定GoodsSpec数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteGoodsSpecInfo(int spec_id)
        {
            DatabaseProvider.GetInstance().DeleteGoodsSpecInfo(spec_id);
        }
        /// <summary>
        /// 按自定义条件删除GoodsSpec中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeleteGoodsSpecInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("goods_spec", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsGoodsSpec(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsGoodsSpec(filter);
        }
        /// <summary>
        /// 获取GoodsSpec单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetGoodsSpecValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetGoodsSpecValue(Fields, filter);
        }
        /// <summary>
        /// 获取GoodsSpec单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetGoodsSpecValue(string Fields)
        {
            return GetGoodsSpecValue(Fields, "");
        }
    }
}