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
    /// CmsPage实体
    /// </summary>
    [Serializable]
    public class CmsPageInfo
    {

        // Internal member variables

        private System.Int32? _page_id;
        private System.Int32? _parent_id;
        private System.String _title;
        private System.String _content;
        private System.Boolean? _is_show;
        private System.Boolean? _show_in_nav;
        private System.String _info_tlp;
        private System.String _tag;
        private System.String _urlrewriter;
        private System.DateTime? _add_time;
        private System.Int32? _click_count;
        private System.String _pagetitle;
        private System.String _pagekeywords;
        private System.String _pagedesc;
        private System.Int32? _order_id;
        private System.String _des;
        private System.String _entitle;
        private System.String _mobile_content;
        private System.String _photo;
        private System.Int32? _ad_id;
        private List<CmsPageInfo> _children;  
        /// <summary>
        /// Default constructor
        /// </summary>
        public CmsPageInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="page_id">CmsPage page_id</param>
        /// <param name="parent_id">CmsPage parent_id</param>
        /// <param name="title">CmsPage title</param>
        /// <param name="content">CmsPage content</param>
        /// <param name="is_show">CmsPage is_show</param>
        /// <param name="show_in_nav">CmsPage show_in_nav</param>
        /// <param name="info_tlp">CmsPage info_tlp</param>
        /// <param name="tag">CmsPage tag</param>
        /// <param name="urlrewriter">CmsPage urlrewriter</param>
        /// <param name="add_time">CmsPage add_time</param>
        /// <param name="click_count">CmsPage click_count</param>
        /// <param name="pagetitle">CmsPage pagetitle</param>
        /// <param name="pagekeywords">CmsPage pagekeywords</param>
        /// <param name="pagedesc">CmsPage pagedesc</param>
        /// <param name="order_id">CmsPage order_id</param>
        /// <param name="des">CmsPage des</param>
        /// <param name="entitle">CmsPage entitle</param>
        /// <param name="mobile_content">CmsPage mobile_content</param>
        /// <param name="photo">CmsPage photo</param>
        /// <param name="ad_id">CmsPage ad_id</param>
        public CmsPageInfo(System.Int32 page_id, System.Int32 parent_id, System.String title, System.String content, System.Boolean is_show, System.Boolean show_in_nav, System.String info_tlp, System.String tag, System.String urlrewriter, System.DateTime add_time, System.Int32 click_count, System.String pagetitle, System.String pagekeywords, System.String pagedesc, System.Int32 order_id, System.String des, System.String entitle, System.String mobile_content, System.String photo, System.Int32 ad_id)
        {
            this._page_id = page_id;
            this._parent_id = parent_id;
            this._title = title;
            this._content = content;
            this._is_show = is_show;
            this._show_in_nav = show_in_nav;
            this._info_tlp = info_tlp;
            this._tag = tag;
            this._urlrewriter = urlrewriter;
            this._add_time = add_time;
            this._click_count = click_count;
            this._pagetitle = pagetitle;
            this._pagekeywords = pagekeywords;
            this._pagedesc = pagedesc;
            this._order_id = order_id;
            this._des = des;
            this._entitle = entitle;
            this._mobile_content = mobile_content;
            this._photo = photo;
            this._ad_id = ad_id;
           
        }
        /// <summary>
        /// 
        /// </summary>
        public List<CmsPageInfo> children
        {
            get { return _children; }
            set { _children = value; }
        }  

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? page_id
        {
            get { return _page_id; }
            set { _page_id = value; }
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
        public System.String title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? is_show
        {
            get { return _is_show; }
            set { _is_show = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? show_in_nav
        {
            get { return _show_in_nav; }
            set { _show_in_nav = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String info_tlp
        {
            get { return _info_tlp; }
            set { _info_tlp = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String urlrewriter
        {
            get { return _urlrewriter; }
            set { _urlrewriter = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? add_time
        {
            get { return _add_time; }
            set { _add_time = value; }
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
        public System.String pagetitle
        {
            get { return _pagetitle; }
            set { _pagetitle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String pagekeywords
        {
            get { return _pagekeywords; }
            set { _pagekeywords = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String pagedesc
        {
            get { return _pagedesc; }
            set { _pagedesc = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? order_id
        {
            get { return _order_id; }
            set { _order_id = value; }
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
        public System.String entitle
        {
            get { return _entitle; }
            set { _entitle = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String mobile_content
        {
            get { return _mobile_content; }
            set { _mobile_content = value; }
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
        public System.Int32? ad_id
        {
            get { return _ad_id; }
            set { _ad_id = value; }
        }

    }
}
