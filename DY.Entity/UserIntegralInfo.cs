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
    /// UserIntegral实体
    /// </summary>
    [Serializable]
    public class UserIntegralInfo
    {

        // Internal member variables

        private System.Int32? _id;
        private System.Int32? _user_id;
        private System.Int32? _integral;
        private System.DateTime? _change_time;
        private System.String _remark;
        private System.Int32? _order_id;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserIntegralInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">UserIntegral id</param>
        /// <param name="user_id">UserIntegral user_id</param>
        /// <param name="integral">UserIntegral integral</param>
        /// <param name="change_time">UserIntegral change_time</param>
        /// <param name="remark">UserIntegral remark</param>
        /// <param name="order_id">UserIntegral order_id</param>
        public UserIntegralInfo(System.Int32 id, System.Int32 user_id, System.Int32 integral, System.DateTime change_time, System.String remark, System.Int32 order_id)
        {
            this._id = id;
            this._user_id = user_id;
            this._integral = integral;
            this._change_time = change_time;
            this._remark = remark;
            this._order_id = order_id;

        }


        /// <summary>
        /// 
        /// </summary>
        public System.Int32? id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? user_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? integral
        {
            get { return _integral; }
            set { _integral = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? change_time
        {
            get { return _change_time; }
            set { _change_time = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? order_id
        {
            get { return _order_id; }
            set { _order_id = value; }
        }

    }
}