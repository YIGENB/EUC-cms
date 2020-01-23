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

namespace DY.Entity
{

    /// <summary>
    /// AdminMenu实体
    /// </summary>
    [Serializable]
    public class AdminMenuInfo
    {

        // Internal member variables

        private System.Int32? _menu_id;
        private System.Int32? _parent_id;
        private System.String _name;
        private System.String _link;
        private System.Int32? _type;
        private System.Boolean? _isshow;
        private List<AdminMenuInfo> _children;  
        /// <summary>
        /// Default constructor
        /// </summary>
        public AdminMenuInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="menu_id">AdminMenu menu_id</param>
        /// <param name="parent_id">AdminMenu parent_id</param>
        /// <param name="name">AdminMenu name</param>
        /// <param name="link">AdminMenu link</param>
        /// <param name="type">AdminMenu type</param>
        /// <param name="isshow">AdminMenu isshow</param>
        public AdminMenuInfo(System.Int32 menu_id, System.Int32 parent_id, System.String name, System.String link, System.Int32 type, System.Boolean isshow)
        {
            this._menu_id = menu_id;
            this._parent_id = parent_id;
            this._name = name;
            this._link = link;
            this._type = type;
            this._isshow = isshow;

        }
        /// <summary>
        /// 
        /// </summary>
        public List<AdminMenuInfo> children
        {
            get { return _children; }
            set { _children = value; }
        }  

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? menu_id
        {
            get { return _menu_id; }
            set { _menu_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? parent_id
        {
            get { return _parent_id; }
            set { _parent_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String link
        {
            get { return _link; }
            set { _link = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? isshow
        {
            get { return _isshow; }
            set { _isshow = value; }
        }

    }
}
