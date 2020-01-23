/**
 * 功能描述：用户相关类
 * 创建时间：2010-1-29 15:01:01
 * 最后修改时间：2010-1-29 15:01:01
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 文件名：UserGroup.cs
 * ID：bd53d549-7bf2-4d5a-b017-3aa0668f882e
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class SiteUser
    {
        #region 静态函数
        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="loginIp"></param>
        /// <param name="loginTime"></param>
        /// <param name="uid"></param>
        public static void UpdateUserLoginInfo(DateTime loginTime, string loginIp, int uid)
        {
            UsersInfo userinfo = new UsersInfo();
            userinfo.last_ip = loginIp;
            userinfo.last_login = loginTime;
            userinfo.user_id = uid;

            DatabaseProvider.GetInstance().UpdateUserLoginInfo(userinfo);
        }

        /// <summary>
        /// 绑定用户微信openid
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="uid"></param>
        public static void UpdateUserOpenidInfo(string openid, int uid)
        {
            UsersInfo userinfo = new UsersInfo();
            userinfo.openid = openid;
            userinfo.user_id = uid;

            DatabaseProvider.GetInstance().UpdateUserLoginInfo(userinfo);
        }
        /// <summary>
        /// 判断指定用户名是否已存在
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>如果已存在该用户id则返回true, 否则返回false</returns>
        public static bool UserExists(int uid)
        {
            return DatabaseProvider.GetInstance().UserExists(uid);
        }

        /// <summary>
        /// 判断指定用户名是否已存在.
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
        public static bool UserExists(string username)
        {
            return DatabaseProvider.GetInstance().UserExists(username);
        }
        /// <summary>
        /// 判断指定openid是否已存在.
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
        public static bool UserOpenidExists(string openid)
        {
            return DatabaseProvider.GetInstance().UserOpenidExists(openid);
        }
        /// <summary>
        /// 判断用户密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否为未MD5密码</param>
        /// <returns>如果正确则返回uid</returns>
        public static int CheckUserPassword(string username, string password, bool originalpassword)
        {
            return DatabaseProvider.GetInstance().CheckUserPassword(username, originalpassword ? SiteUtils.Encryption(password) : password);
        }
        /// <summary>
        /// 获取会员注册项信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static DataTable GetUserRegFields(int user_id)
        {
            return DatabaseProvider.GetInstance().GetUserRegFields(user_id);
        }
        /// <summary>
        /// 保存注册项信息
        /// </summary>
        /// <param name="user_id"></param>
        public static void SaveRegFields(int user_id, string[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                string val = DYRequest.getForm("userFields[" + ids[i] + "]");

                if (!string.IsNullOrEmpty(val))
                {
                    RegFieldsValueInfo valinfo = new RegFieldsValueInfo(0, user_id, Convert.ToInt16(ids[i]), val);

                    if (!SiteBLL.ExistsRegFieldsValue("user_id=" + user_id + " and reg_field_id=" + ids[i] + ""))
                        SiteBLL.InsertRegFieldsValueInfo(valinfo);
                }
            }
        }

        #endregion

        #region 非静态函数
        /// <summary>
        /// 获取用户组列表信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetUserGroupList()
        {
            return SiteBLL.GetUserRankAllList("rank_id asc", "");
        }
        /// <summary>
        /// 根据用户ID取得用户名
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public string GetUserNameById(int user_id)
        {
            return SiteBLL.GetUsersValue("user_name", "user_id=" + user_id).ToString();
        }

        /// <summary>
        /// 支付完成更新会员积分
        /// </summary>
        /// <param name="ordersn">订单号</param>
        public void UpdateUserIntegral(string ordersn)
        {
            OrderInfoInfo orderinfo = SiteBLL.GetOrderInfoInfo("order_sn='" + ordersn + "'");
            UserIntegralInfo userintegral = SiteBLL.GetUserIntegralInfo("order_id=" + orderinfo.order_id);
            //删除记录
            SiteBLL.DeleteUserIntegralInfo(userintegral.id.Value);
            //获取会员原始积分记录
            UsersInfo user = SiteBLL.GetUsersInfo(userintegral.user_id.Value);
            user.pay_points += userintegral.integral.Value;
            //更改会员积分记录
            SiteBLL.UpdateUsersInfo(user);
        }
        #endregion

         /// <summary>
        /// 支付完成更新分销信息
         /// </summary>
         /// <param name="order_id">订单号</param>
         /// <param name="goods_id">商品id</param>
        //public void UpdateUserDistribution(string ordersn)
        //{
        //    OrderInfoInfo orderinfo = SiteBLL.GetOrderInfoInfo("order_sn='" + ordersn + "'");

        //    //删除记录
        //    SiteBLL.DeleteUserIntegralInfo(userintegral.id.Value);
        //    //获取会员原始积分记录
        //    UsersInfo user = SiteBLL.GetUsersInfo(userintegral.user_id.Value);
        //    user.pay_points += userintegral.integral.Value;
        //    //更改会员积分记录
        //    SiteBLL.UpdateUsersInfo(user);
        //}


    }
}
