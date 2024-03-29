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
    /// Products实体
    /// </summary>
    [Serializable]
    public class ProductsInfo {

        // Internal member variables
        
        private System.Int32? _product_id;
        private System.Int32? _goods_id;
        private System.String _barcode;
        private System.String _title;
        private System.String _bn;
        private System.Decimal? _price;
        private System.Decimal? _cost;
        private System.Decimal? _mktprice;
        private System.String _name;
        private System.Decimal? _weight;
        private System.String _unit;
        private System.Int32? _store;
        private System.String _store_place;
        private System.Int32? _freez;
        private System.String _pdt_desc;
        private System.String _props;
        private System.DateTime? _uptime;
        private System.DateTime? _last_modify;
        private System.Boolean? _disabled;
        private System.Boolean? _marketable;
        private System.Boolean? _is_local_stock;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="product_id">Products product_id</param>
        /// <param name="goods_id">Products goods_id</param>
        /// <param name="barcode">Products barcode</param>
        /// <param name="title">Products title</param>
        /// <param name="bn">Products bn</param>
        /// <param name="price">Products price</param>
        /// <param name="cost">Products cost</param>
        /// <param name="mktprice">Products mktprice</param>
        /// <param name="name">Products name</param>
        /// <param name="weight">Products weight</param>
        /// <param name="unit">Products unit</param>
        /// <param name="store">Products store</param>
        /// <param name="store_place">Products store_place</param>
        /// <param name="freez">Products freez</param>
        /// <param name="pdt_desc">Products pdt_desc</param>
        /// <param name="props">Products props</param>
        /// <param name="uptime">Products uptime</param>
        /// <param name="last_modify">Products last_modify</param>
        /// <param name="disabled">Products disabled</param>
        /// <param name="marketable">Products marketable</param>
        /// <param name="is_local_stock">Products is_local_stock</param>
        public ProductsInfo(System.Int32 product_id,System.Int32 goods_id,System.String barcode,System.String title,System.String bn,System.Decimal price,System.Decimal cost,System.Decimal mktprice,System.String name,System.Decimal weight,System.String unit,System.Int32 store,System.String store_place,System.Int32 freez,System.String pdt_desc,System.String props,System.DateTime uptime,System.DateTime last_modify,System.Boolean disabled,System.Boolean marketable,System.Boolean is_local_stock) {
            this._product_id = product_id;
            this._goods_id = goods_id;
            this._barcode = barcode;
            this._title = title;
            this._bn = bn;
            this._price = price;
            this._cost = cost;
            this._mktprice = mktprice;
            this._name = name;
            this._weight = weight;
            this._unit = unit;
            this._store = store;
            this._store_place = store_place;
            this._freez = freez;
            this._pdt_desc = pdt_desc;
            this._props = props;
            this._uptime = uptime;
            this._last_modify = last_modify;
            this._disabled = disabled;
            this._marketable = marketable;
            this._is_local_stock = is_local_stock;
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? product_id {
            get { return _product_id; }
            set { _product_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? goods_id {
            get { return _goods_id; }
            set { _goods_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String barcode {
            get { return _barcode; }
            set { _barcode = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String title {
            get { return _title; }
            set { _title = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String bn {
            get { return _bn; }
            set { _bn = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? price {
            get { return _price; }
            set { _price = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? cost {
            get { return _cost; }
            set { _cost = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? mktprice {
            get { return _mktprice; }
            set { _mktprice = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String name {
            get { return _name; }
            set { _name = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? weight {
            get { return _weight; }
            set { _weight = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String unit {
            get { return _unit; }
            set { _unit = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? store {
            get { return _store; }
            set { _store = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String store_place {
            get { return _store_place; }
            set { _store_place = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? freez {
            get { return _freez; }
            set { _freez = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String pdt_desc {
            get { return _pdt_desc; }
            set { _pdt_desc = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String props {
            get { return _props; }
            set { _props = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? uptime {
            get { return _uptime; }
            set { _uptime = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? last_modify {
            get { return _last_modify; }
            set { _last_modify = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? disabled {
            get { return _disabled; }
            set { _disabled = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? marketable {
            get { return _marketable; }
            set { _marketable = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? is_local_stock {
            get { return _is_local_stock; }
            set { _is_local_stock = value; }
        }
        
    }
}