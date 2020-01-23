using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CShop.Entity;

namespace CShop.Common
{
    /// <summary>
    /// 浏览商品集合
    /// </summary>
    [Serializable]
    public class ShowViewCollection
    {
        //public Hashtable _ShowView = new Hashtable();
        Dictionary<object, object> _ShowView = new Dictionary<object, object>();

        #region 返回已浏览过的所有商品(接口类型)
        /// <summary>
        /// 返回已浏览过的所有商品(接口类型)
        /// </summary>
        public ICollection ShowView
        {
            get { return _ShowView.Values; }
        }
        #endregion

        #region 向已浏览过实体中添加某商品
        /// <summary>
        /// 向已浏览过实体中添加某商品
        /// </summary>
        /// <param name="productID">产品编号</param>
        public void AddViewItem(int saledID, int goodsId, string imgPath, string shopPrice, string marketPrice, string goodsName, string urlPath)
        {
            if (_ShowView.Count > 3)
            {
                _ShowView.Clear();
            }
            try
            {
                _ShowView.Remove(saledID);
            }
            catch
            {
            }
            _ShowView.Add(saledID, new ShopViewInfo(goodsId, imgPath, shopPrice, marketPrice, goodsName, urlPath));

        }
        #endregion

        #region 从已浏览过实体中移除某商品
        /// <summary>
        /// 从已浏览过实体中移除某商品
        /// </summary>
        /// <param name="productID"></param>
        public void RemoveViewItem(string saledID)
        {
            ShopViewInfo item = (ShopViewInfo)_ShowView[saledID];
            if (item == null)
                return;
            else
                _ShowView.Remove(saledID);
        }
        #endregion

        #region 清空已浏览过实体
        /// <summary>
        /// 清空已浏览过实体
        /// </summary>
        public void ClearView()
        {
            _ShowView.Clear();
        }
        #endregion
    }
}
