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
    /// RegFields实体
    /// </summary>
    [Serializable]
    public class RegFieldsInfo {

        // Internal member variables
        
        private System.Int32? _id;
        private System.String _reg_field_name;
        private System.Int32? _sort_order;
        private System.Boolean? _is_show;
        private System.Int32? _type;
        private System.Boolean? _is_need;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegFieldsInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">RegFields id</param>
        /// <param name="reg_field_name">RegFields reg_field_name</param>
        /// <param name="sort_order">RegFields sort_order</param>
        /// <param name="is_show">RegFields is_show</param>
        /// <param name="type">RegFields type</param>
        /// <param name="is_need">RegFields is_need</param>
        public RegFieldsInfo(System.Int32 id,System.String reg_field_name,System.Int32 sort_order,System.Boolean is_show,System.Int32 type,System.Boolean is_need) {
            this._id = id;
            this._reg_field_name = reg_field_name;
            this._sort_order = sort_order;
            this._is_show = is_show;
            this._type = type;
            this._is_need = is_need;
            
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
        public System.String reg_field_name {
            get { return _reg_field_name; }
            set { _reg_field_name = value; }
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
        public System.Boolean? is_show {
            get { return _is_show; }
            set { _is_show = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? type {
            get { return _type; }
            set { _type = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? is_need {
            get { return _is_need; }
            set { _is_need = value; }
        }
        
    }
}