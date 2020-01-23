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
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Entity;

namespace DY.Data
{
    /// <summary>
    /// 系统接口类
    /// </summary>
    public partial interface IDataProvider
    {
        /// <summary>
        /// 插入DepartureDate
        /// </summary>
        /// <param name="adinfo"></param>
        int InsertDepartureDateInfo(DepartureDateInfo entity);
        /// <summary>
        /// 更新DepartureDate
        /// </summary>
        /// <param name="adinfo"></param>
        void UpdateDepartureDateInfo(DepartureDateInfo entity);
        /// <summary>
        /// 获取单个DepartureDate
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        DbDataReader GetDepartureDateInfo(int departure_date_id);
        /// <summary>
        /// 获取单个DepartureDate
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        DbDataReader GetDepartureDateInfo(string filter);
        /// <summary>
        /// 删除DepartureDate（按主键删除）
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        void DeleteDepartureDateInfo(int departure_date_id);
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        bool ExistsDepartureDate(string filter);
        /// <summary>
        /// 获取DepartureDate单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        object GetDepartureDateValue(string Fields, string filter);
    }
}
