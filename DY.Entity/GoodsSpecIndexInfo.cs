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
    /// GoodsSpecIndex实体
    /// </summary>
    [Serializable]
    public class GoodsSpecIndexInfo {

        // Internal member variables
        
        private System.Int32? _id;
        private System.Int32? _type_id;
        private System.Int32? _spec_id;
        private System.Int32? _spec_value_id;
        private System.String _spec_value;
        private System.Int32? _goods_id;
        private System.Int32? _product_id;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GoodsSpecIndexInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">GoodsSpecIndex id</param>
        /// <param name="type_id">GoodsSpecIndex type_id</param>
        /// <param name="spec_id">GoodsSpecIndex spec_id</param>
        /// <param name="spec_value_id">GoodsSpecIndex spec_value_id</param>
        /// <param name="spec_value">GoodsSpecIndex spec_value</param>
        /// <param name="goods_id">GoodsSpecIndex goods_id</param>
        /// <param name="product_id">GoodsSpecIndex product_id</param>
        public GoodsSpecIndexInfo(System.Int32 id,System.Int32 type_id,System.Int32 spec_id,System.Int32 spec_value_id,System.String spec_value,System.Int32 goods_id,System.Int32 product_id) {
            this._id = id;
            this._type_id = type_id;
            this._spec_id = spec_id;
            this._spec_value_id = spec_value_id;
            this._spec_value = spec_value;
            this._goods_id = goods_id;
            this._product_id = product_id;
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? id {
            get { return _id; }
            set { _id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? type_id {
            get { return _type_id; }
            set { _type_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? spec_id {
            get { return _spec_id; }
            set { _spec_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? spec_value_id {
            get { return _spec_value_id; }
            set { _spec_value_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String spec_value {
            get { return _spec_value; }
            set { _spec_value = value; }
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
        public System.Int32? product_id {
            get { return _product_id; }
            set { _product_id = value; }
        }
        
    }
}