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
    /// GoodsInquiry实体
    /// </summary>
    [Serializable]
    public class GoodsInquiryInfo {

        // Internal member variables
        
        private System.Int32? _id;
        private System.String _name;
        private System.String _company;
        private System.String _tel;
        private System.String _address;
        private System.String _email;
        private System.String _advice;
        private System.String _goods_id;
        private System.String _goods_number;
        private System.String _username;
        private System.Int32? _userid;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GoodsInquiryInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">GoodsInquiry id</param>
        /// <param name="name">GoodsInquiry name</param>
        /// <param name="company">GoodsInquiry company</param>
        /// <param name="tel">GoodsInquiry tel</param>
        /// <param name="address">GoodsInquiry address</param>
        /// <param name="email">GoodsInquiry email</param>
        /// <param name="advice">GoodsInquiry advice</param>
        /// <param name="goods_id">GoodsInquiry goods_id</param>
        /// <param name="goods_number">GoodsInquiry goods_number</param>
        /// <param name="username">GoodsInquiry username</param>
        /// <param name="userid">GoodsInquiry userid</param>
        public GoodsInquiryInfo(System.Int32 id,System.String name,System.String company,System.String tel,System.String address,System.String email,System.String advice,System.String goods_id,System.String goods_number,System.String username,System.Int32 userid) {
            this._id = id;
            this._name = name;
            this._company = company;
            this._tel = tel;
            this._address = address;
            this._email = email;
            this._advice = advice;
            this._goods_id = goods_id;
            this._goods_number = goods_number;
            this._username = username;
            this._userid = userid;
            
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
        public System.String name {
            get { return _name; }
            set { _name = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String company {
            get { return _company; }
            set { _company = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String tel {
            get { return _tel; }
            set { _tel = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String address {
            get { return _address; }
            set { _address = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String email {
            get { return _email; }
            set { _email = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String advice {
            get { return _advice; }
            set { _advice = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String goods_id {
            get { return _goods_id; }
            set { _goods_id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String goods_number {
            get { return _goods_number; }
            set { _goods_number = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String username {
            get { return _username; }
            set { _username = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? userid {
            get { return _userid; }
            set { _userid = value; }
        }
        
    }
}