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
    /// Search实体
    /// </summary>
    [Serializable]
    public class SearchInfo
    {

        // Internal member variables

        private System.Int32? _search_id;
        private System.String _title;
        private System.String _des;
        private System.String _contents;
        private System.String _photo;
        private System.Int32? _type;
        private System.DateTime? _date;
        private System.Int32? _click_count;
        private System.Int32? _type_id;
        private System.Boolean? _is_delete;
        private System.String _tag;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="search_id">Search search_id</param>
        /// <param name="title">Search title</param>
        /// <param name="des">Search des</param>
        /// <param name="contents">Search contents</param>
        /// <param name="photo">Search photo</param>
        /// <param name="type">Search type</param>
        /// <param name="date">Search date</param>
        /// <param name="click_count">Search click_count</param>
        /// <param name="type_id">Search type_id</param>
        /// <param name="is_delete">Search is_delete</param>
        /// <param name="is_delete">Search tag</param>
        public SearchInfo(System.Int32 search_id, System.String title, System.String des, System.String contents, System.String photo, System.Int32 type, System.DateTime date, System.Int32 click_count, System.Int32 type_id, System.Boolean is_delete, System.String tag)
        {
            this._search_id = search_id;
            this._title = title;
            this._des = des;
            this._contents = contents;
            this._photo = photo;
            this._type = type;
            this._date = date;
            this._click_count = click_count;
            this._type_id = type_id;
            this._is_delete = is_delete;
            this._tag = tag;

        }


        /// <summary>
        /// 
        /// </summary>
        public System.Int32? search_id
        {
            get { return _search_id; }
            set { _search_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String title
        {
            get { return _title; }
            set { _title = value; }
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
        public System.String contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String photo
        {
            get { return _photo; }
            set { _photo = value; }
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
        public System.DateTime? date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? click_count
        {
            get { return _click_count; }
            set { _click_count = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? type_id
        {
            get { return _type_id; }
            set { _type_id = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? is_delete
        {
            get { return _is_delete; }
            set { _is_delete = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

    }
}
