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
    /// UserAccount实体
    /// </summary>
    [Serializable]
    public class UserAccountInfo {

        // Internal member variables
        
        private System.Int32? _id;
        private System.Int32? _user_id;
        private System.String _admin_user;
        private System.Decimal? _amount;
        private System.DateTime? _add_time;
        private System.DateTime? _paid_time;
        private System.String _admin_note;
        private System.String _user_note;
        private System.Int32? _process_type;
        private System.String _payment;
        private System.Int32? _is_paid;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserAccountInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">UserAccount id</param>
        /// <param name="user_id">UserAccount user_id</param>
        /// <param name="admin_user">UserAccount admin_user</param>
        /// <param name="amount">UserAccount amount</param>
        /// <param name="add_time">UserAccount add_time</param>
        /// <param name="paid_time">UserAccount paid_time</param>
        /// <param name="admin_note">UserAccount admin_note</param>
        /// <param name="user_note">UserAccount user_note</param>
        /// <param name="process_type">UserAccount process_type</param>
        /// <param name="payment">UserAccount payment</param>
        /// <param name="is_paid">UserAccount is_paid</param>
        public UserAccountInfo(System.Int32 id,System.Int32 user_id,System.String admin_user,System.Decimal amount,System.DateTime add_time,System.DateTime paid_time,System.String admin_note,System.String user_note,System.Int32 process_type,System.String payment,System.Int32 is_paid) {
            this._id = id;
            this._user_id = user_id;
            this._admin_user = admin_user;
            this._amount = amount;
            this._add_time = add_time;
            this._paid_time = paid_time;
            this._admin_note = admin_note;
            this._user_note = user_note;
            this._process_type = process_type;
            this._payment = payment;
            this._is_paid = is_paid;
            
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
        public System.Int32? user_id {
            get { return _user_id; }
            set { _user_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String admin_user {
            get { return _admin_user; }
            set { _admin_user = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Decimal? amount {
            get { return _amount; }
            set { _amount = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? add_time {
            get { return _add_time; }
            set { _add_time = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? paid_time {
            get { return _paid_time; }
            set { _paid_time = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String admin_note {
            get { return _admin_note; }
            set { _admin_note = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String user_note {
            get { return _user_note; }
            set { _user_note = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? process_type {
            get { return _process_type; }
            set { _process_type = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String payment {
            get { return _payment; }
            set { _payment = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? is_paid {
            get { return _is_paid; }
            set { _is_paid = value; }
        }
        
    }
}