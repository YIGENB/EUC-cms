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
    /// GoodsCat实体
    /// </summary>
    [Serializable]
    public class GoodsCatInfo {

        // Internal member variables
        
        private System.Int32? _other_cat_id;
        private System.Int32? _goods_id;
        private System.Int32? _cat_id;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GoodsCatInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="other_cat_id">GoodsCat other_cat_id</param>
        /// <param name="goods_id">GoodsCat goods_id</param>
        /// <param name="cat_id">GoodsCat cat_id</param>
        public GoodsCatInfo(System.Int32 other_cat_id,System.Int32 goods_id,System.Int32 cat_id) {
            this._other_cat_id = other_cat_id;
            this._goods_id = goods_id;
            this._cat_id = cat_id;
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? other_cat_id {
            get { return _other_cat_id; }
            set { _other_cat_id = value; }
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
        public System.Int32? cat_id {
            get { return _cat_id; }
            set { _cat_id = value; }
        }
        
    }
}