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
    /// QuickMenu实体
    /// </summary>
    [Serializable]
    public class QuickMenuInfo {

        // Internal member variables
        
        private System.Int32? _id;
        private System.Int32? _siteid;
        private System.String _sitelanguage;
        private System.String _quickmenuname;
        private System.String _quickmenulink;
        private System.Int32? _quickmenuorderid;
        private System.Boolean? _isenabled;
        private System.String _creatuser;
        private System.DateTime? _creattime;
        private System.String _lastupdateuser;
        private System.DateTime? _lastupdatetime;
        private System.String _lastupdateip;
        private System.Int32? _user_id;

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuickMenuInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="id">QuickMenu id</param>
        /// <param name="siteid">QuickMenu siteid</param>
        /// <param name="sitelanguage">QuickMenu sitelanguage</param>
        /// <param name="quickmenuname">QuickMenu quickmenuname</param>
        /// <param name="quickmenulink">QuickMenu quickmenulink</param>
        /// <param name="quickmenuorderid">QuickMenu quickmenuorderid</param>
        /// <param name="isenabled">QuickMenu isenabled</param>
        /// <param name="creatuser">QuickMenu creatuser</param>
        /// <param name="creattime">QuickMenu creattime</param>
        /// <param name="lastupdateuser">QuickMenu lastupdateuser</param>
        /// <param name="lastupdatetime">QuickMenu lastupdatetime</param>
        /// <param name="lastupdateip">QuickMenu lastupdateip</param>
        /// <param name="user_id">QuickMenu user_id</param>
        public QuickMenuInfo(System.Int32 id,System.Int32 siteid,System.String sitelanguage,System.String quickmenuname,System.String quickmenulink,System.Int32 quickmenuorderid,System.Boolean isenabled,System.String creatuser,System.DateTime creattime,System.String lastupdateuser,System.DateTime lastupdatetime,System.String lastupdateip,System.Int32 user_id) {
            this._id = id;
            this._siteid = siteid;
            this._sitelanguage = sitelanguage;
            this._quickmenuname = quickmenuname;
            this._quickmenulink = quickmenulink;
            this._quickmenuorderid = quickmenuorderid;
            this._isenabled = isenabled;
            this._creatuser = creatuser;
            this._creattime = creattime;
            this._lastupdateuser = lastupdateuser;
            this._lastupdatetime = lastupdatetime;
            this._lastupdateip = lastupdateip;
            this._user_id = user_id;
            
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
        public System.Int32? siteid {
            get { return _siteid; }
            set { _siteid = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String sitelanguage {
            get { return _sitelanguage; }
            set { _sitelanguage = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String quickmenuname {
            get { return _quickmenuname; }
            set { _quickmenuname = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String quickmenulink {
            get { return _quickmenulink; }
            set { _quickmenulink = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? quickmenuorderid {
            get { return _quickmenuorderid; }
            set { _quickmenuorderid = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? isenabled {
            get { return _isenabled; }
            set { _isenabled = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String creatuser {
            get { return _creatuser; }
            set { _creatuser = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? creattime {
            get { return _creattime; }
            set { _creattime = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String lastupdateuser {
            get { return _lastupdateuser; }
            set { _lastupdateuser = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? lastupdatetime {
            get { return _lastupdatetime; }
            set { _lastupdatetime = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.String lastupdateip {
            get { return _lastupdateip; }
            set { _lastupdateip = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? user_id {
            get { return _user_id; }
            set { _user_id = value; }
        }
        
    }
}