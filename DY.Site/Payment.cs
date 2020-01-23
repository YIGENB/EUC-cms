using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

using DY.Common;
using DY.Entity;

namespace DY.Site
{
    /// <summary>
    /// 支付信息类
    /// </summary>
    public class Payment
    {
        private static readonly string paymentPluginPath = Utils.GetMapPath("/config/payment.config");

        /// <summary>
        /// 支付插件列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPayments()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(paymentPluginPath);

            return ds.Tables[0];
        }
    }
}
