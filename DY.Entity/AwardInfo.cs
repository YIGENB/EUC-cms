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
    /// Award实体
    /// </summary>
    [Serializable]
    public class AwardInfo
    {

        // Internal member variables

        private System.Int32? _award_id;
        private System.String _name;
        private System.String _type;
        private System.String _count;
        private System.String _des;
        private System.Int32? _parent_id;
        private System.Int32? _atype;
        private System.Int32? _winning_rate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AwardInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="award_id">Award award_id</param>
        /// <param name="name">Award name</param>
        /// <param name="type">Award type</param>
        /// <param name="count">Award count</param>
        /// <param name="des">Award des</param>
        /// <param name="parent_id">Award parent_id</param>
        /// <param name="atype">Award atype</param>
        /// <param name="winning_rate">Award winning_rate</param>
        public AwardInfo(System.Int32 award_id, System.String name, System.String type, System.String count, System.String des, System.Int32 parent_id, System.Int32 atype, System.Int32 winning_rate)
        {
            this._award_id = award_id;
            this._name = name;
            this._type = type;
            this._count = count;
            this._des = des;
            this._parent_id = parent_id;
            this._atype = atype;
            this._winning_rate = winning_rate;

        }


        /// <summary>
        /// 
        /// </summary>
        public System.Int32? award_id
        {
            get { return _award_id; }
            set { _award_id = value; }
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
        public System.String type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String des
        {
            get { return _des; }
            set { _des = value; }
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
        public System.Int32? atype
        {
            get { return _atype; }
            set { _atype = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? winning_rate
        {
            get { return _winning_rate; }
            set { _winning_rate = value; }
        }

    }
}
