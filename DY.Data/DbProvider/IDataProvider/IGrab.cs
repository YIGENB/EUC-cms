﻿//------------------------------------------------------------------------------
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
        /// 插入Grab2
        /// </summary>
        /// <param name="adinfo"></param>
        int InsertGrabInfo(Grab2Info entity);
        /// <summary>
        /// 更新Grab2
        /// </summary>
        /// <param name="adinfo"></param>
        void UpdateGrab2Info(Grab2Info entity);
        /// <summary>
        /// 获取单个Grab2
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        DbDataReader GetGrab2Info(int id);
        /// <summary>
        /// 获取单个Grab2
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        DbDataReader GetGrab2Info(string filter);
        /// <summary>
        /// 删除Grab2（按主键删除）
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        void DeleteGrab2Info(int id);
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        bool ExistsGrab2(string filter);
        /// <summary>
        /// 获取Grab2单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        object GetGrab2Value(string Fields, string filter);
    }
}

