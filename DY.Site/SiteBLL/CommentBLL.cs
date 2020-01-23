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
        public static ArrayList GetCommentAllList(string FieldOrder, string Where)
        {
            return GetCommentAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetCommentAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("comment", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    CommentInfo entity = new CommentInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取Comment分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetCommentList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetCommentList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取Comment分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetCommentList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("comment", "comment_id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    CommentInfo entity = new CommentInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetCommentValue("Count(comment_id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取Comment信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static CommentInfo GetCommentInfo(int comment_id)
        {
            return GetCommentInfo("comment_id="+comment_id);
        }
        /// <summary>
        /// 获取Comment信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static CommentInfo GetCommentInfo(string filter)
        {
            CommentInfo entity = new CommentInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetCommentInfo(filter))
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
        /// 更新Comment信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdateCommentInfo(CommentInfo entity)
        {
            DatabaseProvider.GetInstance().UpdateCommentInfo(entity);
        }
        /// <summary>
        /// 添加Comment信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertCommentInfo(CommentInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertCommentInfo(entity);
        }
        /// <summary>
        /// 更新Comment指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdateCommentFieldValue(string fieldName, object fieldValue, int comment_id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("comment", fieldName, fieldValue, "comment_id", comment_id);
        }
        /// <summary>
        /// 更新Comment指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdateCommentFieldValue(string fieldName, object fieldValue, string comment_ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("comment", fieldName, fieldValue, "comment_id", comment_ids);
        }
        /// <summary>
        /// 删除指定Comment数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteCommentInfo(int comment_id)
        {
            DatabaseProvider.GetInstance().DeleteCommentInfo(comment_id);
        }
        /// <summary>
        /// 按自定义条件删除Comment中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeleteCommentInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("comment", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsComment(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsComment(filter);
        }
        /// <summary>
        /// 获取Comment单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetCommentValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetCommentValue(Fields, filter);
        }
        /// <summary>
        /// 获取Comment单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetCommentValue(string Fields)
        {
            return GetCommentValue(Fields, "");
        }
    }
}