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
    /// GoodsAttribute实体
    /// </summary>
    [Serializable]
    public class GoodsAttributeInfo
    {

        // Internal member variables

        private System.Int32? _attr_id;
        private System.Int32? _type_id;
        private System.String _attr_name;
        private System.Int32? _attr_input_type;
        private System.String _attr_values;
        private System.Int32? _sort_order;
        private System.Int32? _attr_type;
        /// <summary>
        /// Default constructor
        /// </summary>
        public GoodsAttributeInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="attr_id">GoodsAttribute attr_id</param>
        /// <param name="type_id">GoodsAttribute type_id</param>
        /// <param name="attr_name">GoodsAttribute attr_name</param>
        /// <param name="attr_input_type">GoodsAttribute attr_input_type</param>
        /// <param name="attr_values">GoodsAttribute attr_values</param>
        /// <param name="sort_order">GoodsAttribute sort_order</param>
        public GoodsAttributeInfo(System.Int32 attr_id, System.Int32 type_id, System.String attr_name, System.Int32 attr_input_type, System.String attr_values, System.Int32 sort_order, System.Int32 attr_type)
        {
            this._attr_id = attr_id;
            this._type_id = type_id;
            this._attr_name = attr_name;
            this._attr_input_type = attr_input_type;
            this._attr_values = attr_values;
            this._sort_order = sort_order;
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
        public System.Int32? attr_id
        {
            get { return _attr_id; }
            set { _attr_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? type_id
        {
            get { return _type_id; }
            set { _type_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String attr_name
        {
            get { return _attr_name; }
            set { _attr_name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? attr_input_type
        {
            get { return _attr_input_type; }
            set { _attr_input_type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String attr_values
        {
            get { return _attr_values; }
            set { _attr_values = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? sort_order
        {
            get { return _sort_order; }
            set { _sort_order = value; }
        }

    }
}