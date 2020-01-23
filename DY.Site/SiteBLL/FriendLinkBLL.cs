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
        public static ArrayList GetFriendLinkAllList(string FieldOrder, string Where)
        {
            return GetFriendLinkAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetFriendLinkAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("friend_link", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    FriendLinkInfo entity = new FriendLinkInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取FriendLink分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetFriendLinkList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetFriendLinkList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取FriendLink分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetFriendLinkList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("friend_link", "link_id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    FriendLinkInfo entity = new FriendLinkInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetFriendLinkValue("Count(link_id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取FriendLink信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static FriendLinkInfo GetFriendLinkInfo(int link_id)
        {
            return GetFriendLinkInfo("link_id="+link_id);
        }
        /// <summary>
        /// 获取FriendLink信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FriendLinkInfo GetFriendLinkInfo(string filter)
        {
            FriendLinkInfo entity = new FriendLinkInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetFriendLinkInfo(filter))
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
        /// 更新FriendLink信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdateFriendLinkInfo(FriendLinkInfo entity)
        {
            DatabaseProvider.GetInstance().UpdateFriendLinkInfo(entity);
        }
        /// <summary>
        /// 添加FriendLink信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertFriendLinkInfo(FriendLinkInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertFriendLinkInfo(entity);
        }
        /// <summary>
        /// 更新FriendLink指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdateFriendLinkFieldValue(string fieldName, object fieldValue, int link_id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("friend_link", fieldName, fieldValue, "link_id", link_id);
        }
        /// <summary>
        /// 更新FriendLink指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdateFriendLinkFieldValue(string fieldName, object fieldValue, string link_ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("friend_link", fieldName, fieldValue, "link_id", link_ids);
        }
        /// <summary>
        /// 删除指定FriendLink数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteFriendLinkInfo(int link_id)
        {
            DatabaseProvider.GetInstance().DeleteFriendLinkInfo(link_id);
        }
        /// <summary>
        /// 按自定义条件删除FriendLink中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeleteFriendLinkInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("friend_link", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsFriendLink(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsFriendLink(filter);
        }
        /// <summary>
        /// 获取FriendLink单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetFriendLinkValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetFriendLinkValue(Fields, filter);
        }
        /// <summary>
        /// 获取FriendLink单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetFriendLinkValue(string Fields)
        {
            return GetFriendLinkValue(Fields, "");
        }
    }
}