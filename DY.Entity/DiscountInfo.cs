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
    /// Discount实体
    /// </summary>
    [Serializable]
    public class DiscountInfo {

        // Internal member variables
        
        private System.Int32? _discount_id;
        private System.String _discount_name;
        private System.String _discount_class;
        private System.String _discount_para;
        private System.DateTime? _star_date;
        private System.DateTime? _end_date;
        private System.DateTime? _add_time;
        private System.Boolean? _is_enabled;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="discount_id">Discount discount_id</param>
        /// <param name="discount_name">Discount discount_name</param>
        /// <param name="discount_class">Discount discount_class</param>
        /// <param name="discount_para">Discount discount_para</param>
        /// <param name="star_date">Discount star_date</param>
        /// <param name="end_date">Discount end_date</param>
        /// <param name="add_time">Discount add_time</param>
        /// <param name="is_enabled">Discount is_enabled</param>
        public DiscountInfo(System.Int32 discount_id,System.String discount_name,System.String discount_class,System.String discount_para,System.DateTime star_date,System.DateTime end_date,System.DateTime add_time,System.Boolean is_enabled) {
            this._discount_id = discount_id;
            this._discount_name = discount_name;
            this._discount_class = discount_class;
            this._discount_para = discount_para;
            this._star_date = star_date;
            this._end_date = end_date;
            this._add_time = add_time;
            this._is_enabled = is_enabled;
            
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? discount_id {
            get { return _discount_id; }
            set { _discount_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String discount_name {
            get { return _discount_name; }
            set { _discount_name = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String discount_class {
            get { return _discount_class; }
            set { _discount_class = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String discount_para {
            get { return _discount_para; }
            set { _discount_para = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? star_date {
            get { return _star_date; }
            set { _star_date = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? end_date {
            get { return _end_date; }
            set { _end_date = value; }
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
        public System.Boolean? is_enabled {
            get { return _is_enabled; }
            set { _is_enabled = value; }
        }
        
    }
}