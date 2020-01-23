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
        public static ArrayList GetWeixinUserAllList(string FieldOrder, string Where)
        {
            return GetWeixinUserAllList(FieldOrder,"*", Where);
        }
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="strFields">要查询的字段列表</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        public static ArrayList GetWeixinUserAllList(string FieldOrder,string strFields, string Where)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetAllData("weixin_user", strFields, FieldOrder, Where))
            {
                while (sdr.Read())
                {
                    WeixinUserInfo entity = new WeixinUserInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }

            return entityList;
        }
        /// <summary>
        /// 获取WeixinUser分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetWeixinUserList(int PageCurrent, int PageSize, string FieldOrder, string Where, out int ResultCount)
        {
            return GetWeixinUserList(PageCurrent, PageSize, "*", FieldOrder, Where, out ResultCount);
        }
        /// <summary>
        /// 获取WeixinUser分页列表数据
        /// </summary>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        public static ArrayList GetWeixinUserList(int PageCurrent, int PageSize, string strFields, string FieldOrder, string Where, out int ResultCount)
        {
            ArrayList entityList = new ArrayList();
            using (IDataReader sdr = DatabaseProvider.GetInstance().GetPagerData("weixin_user", "user_id", PageCurrent, PageSize, strFields, FieldOrder, Where, out ResultCount))
            {
                while (sdr.Read())
                {
                    WeixinUserInfo entity = new WeixinUserInfo();
                    ReaderToEntity(sdr, entity);
                    entityList.Add(entity);
                }
            }
            
            ResultCount = Convert.ToInt32(SiteBLL.GetWeixinUserValue("Count(user_id)", Where));

            return entityList;
        }
        /// <summary>
        /// 获取WeixinUser信息
        /// </summary>
        /// <param name="ad_id"></param>
        /// <returns></returns>
        public static WeixinUserInfo GetWeixinUserInfo(int user_id)
        {
            return GetWeixinUserInfo("user_id="+user_id);
        }
        /// <summary>
        /// 获取WeixinUser信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static WeixinUserInfo GetWeixinUserInfo(string filter)
        {
            WeixinUserInfo entity = new WeixinUserInfo();;

            using (IDataReader sdr = DatabaseProvider.GetInstance().GetWeixinUserInfo(filter))
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
        /// 更新WeixinUser信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static void UpdateWeixinUserInfo(WeixinUserInfo entity)
        {
            DatabaseProvider.GetInstance().UpdateWeixinUserInfo(entity);
        }
        /// <summary>
        /// 添加WeixinUser信息
        /// </summary>
        /// <param name="deliveryinfo"></param>
        public static int InsertWeixinUserInfo(WeixinUserInfo entity)
        {
            return DatabaseProvider.GetInstance().InsertWeixinUserInfo(entity);
        }
        /// <summary>
        /// 更新WeixinUser指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_id"></param>
        public static void UpdateWeixinUserFieldValue(string fieldName, object fieldValue, int user_id)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("weixin_user", fieldName, fieldValue, "user_id", user_id);
        }
        /// <summary>
        /// 更新WeixinUser指定字段的值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="ad_ids"></param>
        public static void UpdateWeixinUserFieldValue(string fieldName, object fieldValue, string user_ids)
        {
            DatabaseProvider.GetInstance().UpdateFieldValue("weixin_user", fieldName, fieldValue, "user_id", user_ids);
        }
        /// <summary>
        /// 删除指定WeixinUser数据
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteWeixinUserInfo(int user_id)
        {
            DatabaseProvider.GetInstance().DeleteWeixinUserInfo(user_id);
        }
        /// <summary>
        /// 按自定义条件删除WeixinUser中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        public static void DeleteWeixinUserInfo(string filter)
        {
            DatabaseProvider.GetInstance().DeleteData("weixin_user", filter);
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static bool ExistsWeixinUser(string filter)
        {
            return DatabaseProvider.GetInstance().ExistsWeixinUser(filter);
        }
        /// <summary>
        /// 获取WeixinUser单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetWeixinUserValue(string Fields, string filter)
        {
            return DatabaseProvider.GetInstance().GetWeixinUserValue(Fields, filter);
        }
        /// <summary>
        /// 获取WeixinUser单个值
        /// </summary>
        /// <param name="Fields">要获取的字段或函数，如Count(字段名)</param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static object GetWeixinUserValue(string Fields)
        {
            return GetWeixinUserValue(Fields, "");
        }

        /// <summary>
        /// 清空表
        /// </summary>
        /// <returns></returns>
        public static object TruncateWeixinUser()
        {
            return DatabaseProvider.GetInstance().TruncateWeixinUser();
        }
    }
}