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
    /// Csh实体
    /// </summary>
    [Serializable]
    public class CshInfo
    {

        // Internal member variables

        private System.Int32? _csh_id;
        private System.String _csh_title;
        private System.String _csh_con;
        private System.String _csh_pic;
        private System.Int32? _csh_type;
        private System.Int32? _csh_order;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CshInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="csh_id">Csh csh_id</param>
        /// <param name="csh_title">Csh csh_title</param>
        /// <param name="csh_con">Csh csh_con</param>
        /// <param name="csh_pic">Csh csh_pic</param>
        /// <param name="csh_type">Csh csh_type</param>
        /// <param name="csh_order">Csh csh_order</param>
        public CshInfo(System.Int32 csh_id, System.String csh_title, System.String csh_con, System.String csh_pic, System.Int32 csh_type, System.Int32 csh_order)
        {
            this._csh_id = csh_id;
            this._csh_title = csh_title;
            this._csh_con = csh_con;
            this._csh_pic = csh_pic;
            this._csh_type = csh_type;
            this._csh_order = csh_order;

        }


        /// <summary>
        /// 
        /// </summary>
        public System.Int32? csh_id
        {
            get { return _csh_id; }
            set { _csh_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String csh_title
        {
            get { return _csh_title; }
            set { _csh_title = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String csh_con
        {
            get { return _csh_con; }
            set { _csh_con = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String csh_pic
        {
            get { return _csh_pic; }
            set { _csh_pic = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? csh_type
        {
            get { return _csh_type; }
            set { _csh_type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? csh_order
        {
            get { return _csh_order; }
            set { _csh_order = value; }
        }

    }
}