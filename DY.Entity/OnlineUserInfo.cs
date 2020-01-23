using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Entity
{
    /// <summary>
    /// 在线用户信息描述类
    /// </summary>
    public class OnlineUserInfo
    {

        private int m_olid;	//唯一ID
        private int m_userid;	//用户ID
        private string m_username;	//用户登录名
        private string m_nickname;	//用户昵称
        private string m_password;	//登录密码
        private string m_ip;        //IP地址
        private short m_groupid;	//用户组ID

        private short m_newpms;  //新短消息数
        private short m_newnotices;  //新通知数
        private string m_action;  //权限列表


        ///<summary>
        ///唯一ID
        ///</summary>
        public int Olid
        {
            get { return m_olid; }
            set { m_olid = value; }
        }
        ///<summary>
        ///用户ID
        ///</summary>
        public int Userid
        {
            get { return m_userid; }
            set { m_userid = value; }
        }
        ///<summary>
        ///用户登录名
        ///</summary>
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }
        ///<summary>
        ///用户昵称
        ///</summary>
        public string Nickname
        {
            get { return m_nickname; }
            set { m_nickname = value; }
        }
        ///<summary>
        ///登录密码
        ///</summary>
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }
        ///<summary>
        ///IP地址
        ///</summary>
        public string Ip
        {
            get { return m_ip; }
            set { m_ip = value; }
        }
        ///<summary>
        ///用户组ID
        ///</summary>
        public short Groupid
        {
            get { return m_groupid; }
            set { m_groupid = value; }
        }
        ///<summary>
        ///新短消息数
        ///</summary>
        public short Newpms
        {
            get { return m_newpms; }
            set { m_newpms = value; }
        }
        ///<summary>
        ///新通知数
        ///</summary>
        public short Newnotices
        {
            get { return m_newnotices; }
            set { m_newnotices = value; }
        }
        ///<summary>
        ///权限列表
        ///</summary>
        public string Actions
        {
            get { return m_action; }
            set { m_action = value; }
        }
    }
}
