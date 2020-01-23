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
using System.Collections.Generic;

namespace DY.Entity {

    /// <summary>
    /// Config实体
    /// </summary>
    [Serializable]
    public class ConfigInfo {

        // Internal member variables
        
        private System.Int32? _id;
        private System.Int32? _parent_id;
        private System.String _name;
        private System.String _code;
        private System.String _type;
        private System.String _tip;
        private System.Int32? _size;
        private System.String _store_range;
        private System.String _store_dir;
        private System.String _value;
        private System.Int32? _sort_order;
        private System.Boolean? _isshow;
        private List<ConfigInfo> _children;  
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">Config id</param>
        /// <param name="parent_id">Config parent_id</param>
        /// <param name="name">Config name</param>
        /// <param name="code">Config code</param>
        /// <param name="type">Config type</param>
        /// <param name="tip">Config tip</param>
        /// <param name="size">Config size</param>
        /// <param name="store_range">Config store_range</param>
        /// <param name="store_dir">Config store_dir</param>
        /// <param name="value">Config value</param>
        /// <param name="sort_order">Config sort_order</param>
        /// <param name="isshow">Config isshow</param>
        public ConfigInfo(System.Int32 id,System.Int32 parent_id,System.String name,System.String code,System.String type,System.String tip,System.Int32 size,System.String store_range,System.String store_dir,System.String value,System.Int32 sort_order,System.Boolean isshow) {
            this._id = id;
            this._parent_id = parent_id;
            this._name = name;
            this._code = code;
            this._type = type;
            this._tip = tip;
            this._size = size;
            this._store_range = store_range;
            this._store_dir = store_dir;
            this._value = value;
            this._sort_order = sort_order;
            this._isshow = isshow;
            
        }
        /// <summary>
        /// 
        /// </summary>
        public List<ConfigInfo> children
        {
            get { return _children; }
            set { _children = value; }
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
        public System.Int32? parent_id {
            get { return _parent_id; }
            set { _parent_id = value; }
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
        public System.String code {
            get { return _code; }
            set { _code = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String type {
            get { return _type; }
            set { _type = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String tip {
            get { return _tip; }
            set { _tip = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? size {
            get { return _size; }
            set { _size = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String store_range {
            get { return _store_range; }
            set { _store_range = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String store_dir {
            get { return _store_dir; }
            set { _store_dir = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String value {
            get { return _value; }
            set { _value = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? sort_order {
            get { return _sort_order; }
            set { _sort_order = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? isshow {
            get { return _isshow; }
            set { _isshow = value; }
        }
        
    }
}