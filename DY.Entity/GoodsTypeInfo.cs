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

namespace DY.Entity
{

    /// <summary>
    /// GoodsType实体
    /// </summary>
    [Serializable]
    public class GoodsTypeInfo
    {

        // Internal member variables

        private System.Int32? _cat_id;
        private System.String _cat_name;
        private System.Boolean? _enabled;
        private System.Int32? _attr_type;
        /// <summary>
        /// Default constructor
        /// </summary>
        public GoodsTypeInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="cat_id">GoodsType cat_id</param>
        /// <param name="cat_name">GoodsType cat_name</param>
        /// <param name="enabled">GoodsType enabled</param>
        public GoodsTypeInfo(System.Int32 cat_id, System.String cat_name, System.Boolean enabled, System.Int32 attr_type)
        {
            this._cat_id = cat_id;
            this._cat_name = cat_name;
            this._enabled = enabled;
            this._attr_type = attr_type;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? attr_type
        {
            get { return _attr_type; }
            set { _attr_type = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? cat_id
        {
            get { return _cat_id; }
            set { _cat_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String cat_name
        {
            get { return _cat_name; }
            set { _cat_name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

    }
}