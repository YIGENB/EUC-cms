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
    /// UserFeed实体
    /// </summary>
    [Serializable]
    public class UserFeedInfo {

        // Internal member variables
        
        private System.Int32? _feed_id;
        private System.Int32? _user_id;
        private System.Int32? _value_id;
        private System.Int32? _goods_id;
        private System.Int32? _feed_type;
        private System.Int32? _is_feed;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserFeedInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="feed_id">UserFeed feed_id</param>
        /// <param name="user_id">UserFeed user_id</param>
        /// <param name="value_id">UserFeed value_id</param>
        /// <param name="goods_id">UserFeed goods_id</param>
        /// <param name="feed_type">UserFeed feed_type</param>
        /// <param name="is_feed">UserFeed is_feed</param>
        public UserFeedInfo(System.Int32 feed_id,System.Int32 user_id,System.Int32 value_id,System.Int32 goods_id,System.Int32 feed_type,System.Int32 is_feed) {
            this._feed_id = feed_id;
            this._user_id = user_id;
            this._value_id = value_id;
            this._goods_id = goods_id;
            this._feed_type = feed_type;
            this._is_feed = is_feed;
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? feed_id {
            get { return _feed_id; }
            set { _feed_id = value; }
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
        public System.Int32? value_id {
            get { return _value_id; }
            set { _value_id = value; }
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
        public System.Int32? feed_type {
            get { return _feed_type; }
            set { _feed_type = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? is_feed {
            get { return _is_feed; }
            set { _is_feed = value; }
        }
        
    }
}