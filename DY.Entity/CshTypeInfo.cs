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
    /// CshType实体
    /// </summary>
    [Serializable]
    public class CshTypeInfo {

        // Internal member variables
        
        private System.Int32? _type_id;
        private System.String _type_name;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CshTypeInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="type_id">CshType type_id</param>
        /// <param name="type_name">CshType type_name</param>
        public CshTypeInfo(System.Int32 type_id,System.String type_name) {
            this._type_id = type_id;
            this._type_name = type_name;
            
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
        public System.String type_name {
            get { return _type_name; }
            set { _type_name = value; }
        }
        
    }
}