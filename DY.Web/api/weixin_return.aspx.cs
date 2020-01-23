using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DY.OAuthV2SDK;
using System.Xml;
using DY.OAuthV2SDK.OAuths;
using DY.Entity;
using DY.Common;
using DY.Site;
using DY.Config;

namespace DY.Web.api
{
    public partial class weixin_return : DY.Site.WebPage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (OAuthBase.HasCacheOAuth)
            {
                var oauth = OAuthBase.CreateInstance();
                if (oauth != null)
                {
                    try
                    {
                        var token = oauth.GetAccessToken(); //获取认证信息
                        if (token.ret == 0)
                        {
                            oauth.AccessToken = token.access_token;
                            oauth.ExpiresIn = token.expires_in;
                            oauth.UpdateCache(); //缓存认证信息

                            //获取用户id
                            var result = oauth.GetUid();
                            if (result.ret == 0)
                            {
                                
                                    //LoginBing(oauth);
                                Response.Write("<script language='javascript'>alert('" + oauth.Appid + "====" + oauth.Password + "')</script>");
     
                            }
                            else
                            {
                                Response.Write(result.msg + "(" + result.errcode + ")");
                                Response.Write("<br />" + result.response);
                            }
                        }
                        else
                        {
                            Response.Write(token.msg + "(" + token.errcode + ")");
                            Response.Write("<br />" + token.response);
                        }

                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                }
                else
                {
                    Response.Write("登录失败，找不到相对应的接口");
                }
            }
            else
            {
                Response.Write("登录失败，找不到相对应的缓存");
            }
        }
        /// <summary>
        /// 修改节点值
        /// </summary>
        /// <param name="name">上级节点名</param>
        protected void UpdateConfig(string name, string access_token, string uid)
        {
            string xpath = OAuthConfig.CONFIG_ROOT + OAuthConfig.CONFIG_OAUTH + "/" + name + OAuthConfig.CONFIG_APP + "/";
            XmlDocument _xml = new XmlDocument();
            _xml.Load(Server.MapPath(OAuthConfig.configPath));
            XmlNode noList = _xml.SelectSingleNode(xpath + "my_app");
            if (noList != null)
            {
                foreach (XmlNode item in noList.ChildNodes)
                {
                    if (item.NodeType == XmlNodeType.Element)
                    {
                        if (item.Name == "access_token")
                            item.InnerText = access_token;
                        else if (item.Name == "uid")
                            item.InnerText = uid;
                    }
                }
                _xml.Save(Server.MapPath(OAuthConfig.configPath));
            }
        }
        /// <summary>
        /// 登录绑定
        /// </summary>
        /// <param name="name">上级节点名</param>
        protected void LoginBing(OAuthBase oauth)
        {
            UsersInfo stact_id = SiteBLL.GetUsersInfo(oauth.Uid);
            if (stact_id != null)
            {
                UsersInfo userinfo = GetUserInfo(username, password);

                if (userinfo != null)
                {
                    SiteUtils.WriteUserLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                    DY.Site.SiteUser.UpdateUserLoginInfo(DateTime.Now, DYRequest.GetIP(), userinfo.user_id.Value);

                }
            }
            else
            {
                UsersInfo userinfo = new UsersInfo();
                userinfo.user_name = oauth.Uid;
                userinfo.password = oauth.Password;
                userinfo.email = DYRequest.getFormString("email");
                userinfo.question = "";
                userinfo.answer = "";
                userinfo.sex = 0;
                userinfo.birthday = DYRequest.getFormDateTime("birthday");
                userinfo.user_money = 0;
                userinfo.frozen_money = 0;
                userinfo.pay_points = 0;
                userinfo.rank_points = 0;
                userinfo.address_id = 0;
                userinfo.reg_time = DateTime.Now;
                userinfo.last_login = DYRequest.getFormDateTime("last_login");
                userinfo.last_ip = "";
                userinfo.login_count = 0;
                userinfo.user_rank = 0;
                userinfo.parent_id = DYRequest.getFormInt("puid", 0);
                userinfo.is_validated = config.Reg_shenhe;
                userinfo.is_enabled = config.Reg_shenhe;
                userinfo.user_photo = DYRequest.getFormString("headimgurl");
                userinfo.remarks = DYRequest.getFormString("qq");
                //userinfo.address = DYRequest.getFormString("address");
                userinfo.openid = DYRequest.getFormString("openid");
                userinfo.distribution_level = DYRequest.getFormInt("dlevel", 0);
                int user_id = SiteBLL.InsertUsersInfo(userinfo);
                userinfo.user_id = user_id;
                SiteUtils.WriteUserLoginCookie(userinfo, DYRequest.getFormInt("expires", -1), BaseConfig.WebEncrypt);
                DY.Site.SiteUser.UpdateUserLoginInfo(DateTime.Now, DYRequest.GetIP(), user_id);
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected UsersInfo GetUserInfo(string username, string password)
        {
            int uid = DY.Site.SiteUser.CheckUserPassword(username, password, true);

            return uid > 0 ? SiteBLL.GetUsersInfo(uid) : null;
        }
    }
}