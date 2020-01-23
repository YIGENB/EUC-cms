using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using DY.Config;
using DY.Entity;

namespace DY.Web.tools
{
    public partial class password : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string info = DYRequest.getRequest("u");
            
            if (!string.IsNullOrEmpty(info))
            {
                info = DES.Decode(info, BaseConfig.WebEncrypt);

                if (info.IndexOf(",") >= 0)
                {
                    string[] str = info.Split(',');

                    //验证时间是否在30分钟之内
                    if (Convert.ToDateTime(str[1]) < DateTime.Now && Convert.ToDateTime(str[1]) >= DateTime.Now.AddMinutes(-30))
                    {
                        //查询用户是否存在
                        UsersInfo userinfo = SiteBLL.GetUsersInfo("user_name='"+ str[0] +"'");
                        if (userinfo == null)
                            Response.Write("用户不存在");
                        else
                        {
                            string pass = new Random().Next(111111, 999999).ToString();
                            //重置密码
                            SiteBLL.UpdateUsersFieldValue("password", SiteUtils.Encryption(pass), userinfo.user_id.Value);

                            Response.Write("您的密码已经重置为"+ pass +"，请<a href=\"/\">马上登录后</a>修改您的密码。");
                        }
                    }
                    else
                    {
                        Response.Write("该信息已过期！！！");
                    }
                }
            }
        }
    }
}
