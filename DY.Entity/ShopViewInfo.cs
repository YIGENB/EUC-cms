using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Entity
{
    /// <summary>
    /// 保存到Cookies对象
    /// </summary>
    [Serializable]
    public class ShopViewInfo
    {
        private int _goodsid;
        private string _goodsname;
        private string _shopprice;
        private string _marketprice;
        private string _imgpath;
        private string _urlpath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ShopViewInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodsid"></param>
        public ShopViewInfo(int goodsid, string imgpath, string shop_price, string market_price, string goods_name, string urlpath)
        {
            _goodsid = goodsid;
            _goodsname = goods_name;
            _imgpath = imgpath;
            _shopprice = shop_price;
            _marketprice = market_price;
            _urlpath = urlpath;
        }

        /// <summary>
        /// 商品id
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Goodsname
        {
            get { return _goodsname; }
            set { _goodsname = value; }
        }

        /// <summary>
        /// 商品价格2
        /// </summary>
        public string Shopprice
        {
            get { return _shopprice; }
            set { _shopprice = value; }
        }


        /// <summary>
        /// 商品价格1
        /// </summary>
        public string Marketprice
        {
            get { return _marketprice; }
            set { _marketprice = value; }
        }

        /// <summary>
        /// 商品图片路径
        /// </summary>
        public string Imgpath
        {
            get { return _imgpath; }
            set { _imgpath = value; }
        }

        /// <summary>
        /// 商品链接
        /// </summary>
        public string Urlpath
        {
            get { return _urlpath; }
            set { _urlpath = value; }
        }
    }
}
