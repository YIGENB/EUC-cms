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

namespace DY.Entity {

    /// <summary>
    /// OrderGoods实体
    /// </summary>
    [Serializable]
    public class OrderGoodsInfo {

        // Internal member variables
        
        private System.Int32? _rec_id;
        private System.Int64 _order_id;
        private System.Int64 _goods_id;
        private System.String _goods_name;
        private System.String _goods_sn;
        private System.Int32? _goods_number;
        private System.Decimal? _market_price;
        private System.Decimal? _goods_price;
        private System.String _goods_attr;
        private System.Int32? _send_number;
        private System.Boolean? _is_real;
        private System.String _extension_code;
        private System.Int64 _parent_id;
        private System.Boolean? _is_gift;
        private System.DateTime? _add_time;

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrderGoodsInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="rec_id">OrderGoods rec_id</param>
        /// <param name="order_id">OrderGoods order_id</param>
        /// <param name="goods_id">OrderGoods goods_id</param>
        /// <param name="goods_name">OrderGoods goods_name</param>
        /// <param name="goods_sn">OrderGoods goods_sn</param>
        /// <param name="goods_number">OrderGoods goods_number</param>
        /// <param name="market_price">OrderGoods market_price</param>
        /// <param name="goods_price">OrderGoods goods_price</param>
        /// <param name="goods_attr">OrderGoods goods_attr</param>
        /// <param name="send_number">OrderGoods send_number</param>
        /// <param name="is_real">OrderGoods is_real</param>
        /// <param name="extension_code">OrderGoods extension_code</param>
        /// <param name="parent_id">OrderGoods parent_id</param>
        /// <param name="is_gift">OrderGoods is_gift</param>
        /// <param name="add_time">OrderGoods add_time</param>
        public OrderGoodsInfo(System.Int32 rec_id,System.Int64 order_id,System.Int64 goods_id,System.String goods_name,System.String goods_sn,System.Int32 goods_number,System.Decimal market_price,System.Decimal goods_price,System.String goods_attr,System.Int32 send_number,System.Boolean is_real,System.String extension_code,System.Int64 parent_id,System.Boolean is_gift,System.DateTime add_time) {
            this._rec_id = rec_id;
            this._order_id = order_id;
            this._goods_id = goods_id;
            this._goods_name = goods_name;
            this._goods_sn = goods_sn;
            this._goods_number = goods_number;
            this._market_price = market_price;
            this._goods_price = goods_price;
            this._goods_attr = goods_attr;
            this._send_number = send_number;
            this._is_real = is_real;
            this._extension_code = extension_code;
            this._parent_id = parent_id;
            this._is_gift = is_gift;
            this._add_time = add_time;
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? rec_id {
            get { return _rec_id; }
            set { _rec_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int64 order_id {
            get { return _order_id; }
            set { _order_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int64 goods_id {
            get { return _goods_id; }
            set { _goods_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String goods_name {
            get { return _goods_name; }
            set { _goods_name = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String goods_sn {
            get { return _goods_sn; }
            set { _goods_sn = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? goods_number {
            get { return _goods_number; }
            set { _goods_number = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? market_price {
            get { return _market_price; }
            set { _market_price = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? goods_price {
            get { return _goods_price; }
            set { _goods_price = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String goods_attr {
            get { return _goods_attr; }
            set { _goods_attr = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? send_number {
            get { return _send_number; }
            set { _send_number = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? is_real {
            get { return _is_real; }
            set { _is_real = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String extension_code {
            get { return _extension_code; }
            set { _extension_code = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int64 parent_id {
            get { return _parent_id; }
            set { _parent_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? is_gift {
            get { return _is_gift; }
            set { _is_gift = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? add_time {
            get { return _add_time; }
            set { _add_time = value; }
        }
        
    }
}