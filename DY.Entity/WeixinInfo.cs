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
    /// Weixin实体
    /// </summary>
    [Serializable]
    public class WeixinInfo
    {

        // Internal member variables

        private System.Int32? _mp_id;
        private System.String _name;
        private System.String _ghid;
        private System.String _code;
        private System.String _head;
        private System.String _iurl;
        private System.String _token;
        private System.DateTime? _create_date;
        private System.Boolean? _enabled;
        private System.String _sbuscribe;
        private System.String _appid;
        private System.String _appsecret;
        private System.String _nomatch_replay;
        private System.String _username;
        private System.String _password;
        private System.String _send_content;
        private System.DateTime? _send_date;
        private System.String _encodingAESKey;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WeixinInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="mp_id">Weixin mp_id</param>
        /// <param name="name">Weixin name</param>
        /// <param name="ghid">Weixin ghid</param>
        /// <param name="code">Weixin code</param>
        /// <param name="head">Weixin head</param>
        /// <param name="iurl">Weixin iurl</param>
        /// <param name="token">Weixin token</param>
        /// <param name="create_date">Weixin create_date</param>
        /// <param name="enabled">Weixin enabled</param>
        /// <param name="sbuscribe">Weixin sbuscribe</param>
        /// <param name="appid">Weixin appid</param>
        /// <param name="appsecret">Weixin appsecret</param>
        /// <param name="nomatch_replay">Weixin nomatch_replay</param>
        /// <param name="username">Weixin username</param>
        /// <param name="password">Weixin password</param>
        /// <param name="send_content">Weixin send_content</param>
        /// <param name="send_date">Weixin send_date</param>
        /// <param name="encodingAESKey">Weixin encodingAESKey</param>
        public WeixinInfo(System.Int32 mp_id, System.String name, System.String ghid, System.String code, System.String head, System.String iurl, System.String token, System.DateTime create_date, System.Boolean enabled, System.String sbuscribe, System.String appid, System.String appsecret, System.String nomatch_replay, System.String username, System.String password, System.String send_content, System.DateTime send_date, System.String encodingAESKey)
        {
            this._mp_id = mp_id;
            this._name = name;
            this._ghid = ghid;
            this._code = code;
            this._head = head;
            this._iurl = iurl;
            this._token = token;
            this._create_date = create_date;
            this._enabled = enabled;
            this._sbuscribe = sbuscribe;
            this._appid = appid;
            this._appsecret = appsecret;
            this._nomatch_replay = nomatch_replay;
            this._username = username;
            this._password = password;
            this._send_content = send_content;
            this._send_date = send_date;
            this._encodingAESKey = encodingAESKey;

        }


        /// <summary>
        /// 
        /// </summary>
        public System.Int32? mp_id
        {
            get { return _mp_id; }
            set { _mp_id = value; }
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
        public System.String ghid
        {
            get { return _ghid; }
            set { _ghid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String head
        {
            get { return _head; }
            set { _head = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String iurl
        {
            get { return _iurl; }
            set { _iurl = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String token
        {
            get { return _token; }
            set { _token = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? create_date
        {
            get { return _create_date; }
            set { _create_date = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean? enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String sbuscribe
        {
            get { return _sbuscribe; }
            set { _sbuscribe = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String appid
        {
            get { return _appid; }
            set { _appid = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String appsecret
        {
            get { return _appsecret; }
            set { _appsecret = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String nomatch_replay
        {
            get { return _nomatch_replay; }
            set { _nomatch_replay = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String username
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String send_content
        {
            get { return _send_content; }
            set { _send_content = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? send_date
        {
            get { return _send_date; }
            set { _send_date = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String encodingAESKey
        {
            get { return _encodingAESKey; }
            set { _encodingAESKey = value; }
        }

    }
}