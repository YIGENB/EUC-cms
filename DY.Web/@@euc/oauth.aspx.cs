using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Xml;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK;

namespace DY.Web.admin
{
    public partial class oauth : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("oauth_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("oauth_add");

                if (ispost)
                {
                   // base.id = SiteBLL.InsertOauthInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加授权配置");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("授权配置添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "oauth/oauth_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("oauth_edit");
                string name=DYRequest.getRequest("name");
                if (ispost)
                {
                    UpdateConfig(name);
                    //更新微信配置(单微信号)
                    WeixinInfo entity = new WeixinInfo();
                    entity.appid = DYRequest.getForm("clientId");
                    entity.appsecret = DYRequest.getForm("clientSecret");
                    entity.mp_id = 1;
                    SiteBLL.UpdateWeixinInfo(entity);

                    //日志记录
                    base.AddLog("修改授权配置");

                    base.DisplayMessage("授权配置修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", OAuthConfig.GetConfigApp(name, ""));
                context.Add("oauthEntity", SetOAuthEntity(name));

                base.DisplayTemplate(context, "oauth/oauth_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("oauth_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateOauthFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改授权配置");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("oauth_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateOauthFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 批量彻底删除
            else if (this.act == "CompletelyDelete")
            {
                //检测权限
                this.IsChecked("oauth_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteOauthInfo("oauth_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除授权配置");
                    }

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
                }
            }
            #endregion

            #region 删除记录
            else if (this.act == "remove")
            {
                //检测权限
                this.IsChecked("oauth_del", true);

                //执行删除
                SiteBLL.DeleteOauthInfo(base.id);

                //日志记录
                base.AddLog("删除授权配置");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 单个取消授权
            else if (this.act == "out")
            {
                //检测权限
                this.IsChecked("oauth_loginout", true);

                string name = DYRequest.getRequest("name");

                //执行删除
                var oauth = DY.OAuthV2SDK.OAuths.OAuthBase.CreateInstance();
                if (!string.IsNullOrEmpty(name))
                {
                    UpdateConfig(name, "", "");
                    oauth.ClearCache();
                }

                //日志记录
                base.AddLog(name+"取消授权配置");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 批量取消授权
            else if (this.act == "loginout")
            {
                //检测权限
                this.IsChecked("oauth_loginout", true);

                foreach(OAuthEntity entity in OAuthConfig.GetConfigOAuths())
                {
                    if (!string.IsNullOrEmpty(OAuthConfig.GetConfigApp(entity.name, "").Access_token))
                    {
                        UpdateConfig(entity.name, "", "");
                    }
                }

                //执行删除
                var oauth = DY.OAuthV2SDK.OAuths.OAuthBase.CreateInstance();
                if (oauth != null)
                {
                    var oauth_name = oauth.OAuthName;
                    UpdateConfig(oauth_name, "", "");
                    oauth.ClearCache();
                }

                //日志记录
                base.AddLog("取消授权配置");

                //显示列表数据
                this.GetList();
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";

            this.GetList("oauth/oauth_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", OAuthConfig.GetConfigOAuths());//SiteBLL.GetOauthList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("oauth_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            //context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            //context.Add("sort_by", DYRequest.getRequest("sort_by"));
            //context.Add("sort_order", DYRequest.getRequest("sort_order"));
            //context.Add("page", base.pageindex);
            context.Add("OAuthConfig", new OAuthConfig());

            base.DisplayTemplate(context, tpl, base.isajax);
        }

        /// <summary>
        /// 修改节点值
        /// </summary>
        /// <param name="name">上级节点名</param>
        protected void UpdateConfig(string name)
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
                        if (item.Name == "clientId")
                            item.InnerText = DYRequest.getForm("clientId");
                        else if (item.Name == "clientSecret")
                            item.InnerText = DYRequest.getForm("clientSecret");
                        else if (item.Name == "randomAT")
                            item.InnerText = DYRequest.getForm("randomAT");
                        else if (item.Name == "redirectUri")
                            item.InnerText = "http://" + new SiteUtils().GetDomain() + "/api/oauth_return.aspx";//DYRequest.getForm("redirectUri");
                    }
                }
                _xml.Save(Server.MapPath(OAuthConfig.configPath));
            }
        }

                /// <summary>
        /// 更新XML文件中的指定节点内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">更新内容</param>
        /// <returns>更新是否成功</returns>
        public  bool UpdateNode(string filePath, string nodeName, string nodeValue)
        {
            bool flag = false;

            XmlDocument xd = new XmlDocument();
            xd.Load(System.Web.HttpContext.Current.Server.MapPath(filePath));
            XmlElement xe = xd.DocumentElement;
            XmlNode xn = xe.SelectSingleNode("//" + nodeName);
            if (xn != null)
            {
                xn.InnerText = nodeValue;
                flag = true;
            }
            else
            {
                flag = false;
            }

            return flag;
        }

        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected OAuthEntity SetOAuthEntity(string name)
        {
            OAuthEntity entity = new OAuthEntity();
            string xpath = OAuthConfig.CONFIG_ROOT + OAuthConfig.CONFIG_OAUTH;
            XmlNode oauth_node = OAuthConfig.XmlConfig.SelectSingleNode(xpath);
            if (oauth_node != null)
            {
                foreach (XmlNode item in oauth_node.ChildNodes)
                {
                    if (item.Name == name)
                    {
                        entity.name = item.Name;
                        if (item.Attributes["desc"] != null)
                        {
                            entity.desc = item.Attributes["desc"].Value;
                        }
                        if (item.Attributes["cnname"] != null)
                        {
                            entity.cnname = item.Attributes["cnname"].Value;
                        }
                        if (item.Attributes["isat"] != null)
                        {
                            entity.isAt = item.Attributes["isat"].Value;
                        }
                    }
                }
            }
            return entity;
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
    }
}


