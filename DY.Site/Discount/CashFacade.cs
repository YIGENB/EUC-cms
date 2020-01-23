using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

using DY.Entity;

namespace DY.Site
{
    public class CashFacade
    {
        const string ASSEMBLY_NAME = "DY.Site";
        
        //得到现金收取类型列表，返回字符串数组
        public string[] GetCashAcceptTypeList()
        {
            ArrayList list = SiteBLL.GetDiscountAllList("", "");
            int rowCount = list.Count;
            string[] arrarResult = new string[rowCount];

            int i = 0;
            foreach (DiscountInfo dis in list)
            {
                arrarResult[i] = dis.discount_name;

                i++;
            }
            return arrarResult;
        }

        /// <summary>
        /// 用于根据商品活动的不同和原价格，计算此商品的实际收费
        /// </summary>
        /// <param name="startTotal">原价</param>
        /// <returns>实际价格</returns>
        public decimal GetFactTotal(decimal startTotal)
        {
            CashContext cc = new CashContext();
            DiscountInfo dr = SiteBLL.GetDiscountInfo("is_enabled=1 and discount_class='CashReturn'");
            if (dr == null)
                return startTotal;

            object[] args = null;
            if (!string.IsNullOrEmpty(dr.discount_para))
                args = dr.discount_para.Split(',');

            cc.setBehavior((CashSuper)Assembly.Load(ASSEMBLY_NAME).CreateInstance(ASSEMBLY_NAME + "." + dr.discount_class, false, BindingFlags.Default, null, args, null, null));
            return cc.GetResult(startTotal);

        }
    }
}
