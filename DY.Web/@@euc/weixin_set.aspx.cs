/**
 * 功能描述：Weixin管理类
 * 创建时间：2014/2/20 14:10:42
 * 最后修改时间：2014/2/20 14:10:42
 * 作者：gudufy
 * 文件标识：3b9bc044-67ad-4934-9cb5-fec294bbdf7b
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Text;
using System.Xml;
using DY.OAuthV2SDK;

namespace DY.Web.admin
{
    public partial class weixin_set : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("weixin_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("weixin_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertWeixinInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加微信公众号");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("微信公众号添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "weixin/weixin_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("weixin_edit");

                if (ispost)
                {
                    SiteBLL.UpdateWeixinInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改微信公众号");

                    base.DisplayMessage("微信公众号修改成功", 2, "");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetWeixinInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("sbuscribe", SiteBLL.GetWeixinNewsInfo("replay_id=" + SiteBLL.GetWeixinInfo(base.id).sbuscribe).keyword);
                #region 处理搜索数据使用
                int nomatch_replay = Convert.ToInt32(SiteBLL.GetWeixinInfo(base.id).nomatch_replay);
                if (nomatch_replay > 0)
                    context.Add("nomatch_replay", SiteBLL.GetWeixinNewsInfo("replay_id=" + SiteBLL.GetWeixinInfo(base.id).nomatch_replay).keyword);
                else
                    context.Add("nomatch_replay", "搜索数据");
                #endregion

                if (!string.IsNullOrEmpty(SiteBLL.GetWeixinInfo(base.id).sbuscribe))
                    context.Add("sbuscribelist", SiteBLL.GetWeixinNewsAllList("", "replay_id in(" + SiteBLL.GetWeixinInfo(base.id).sbuscribe + ")"));
                if (!string.IsNullOrEmpty(SiteBLL.GetWeixinInfo(base.id).nomatch_replay))
                    context.Add("nomatch_replaylist", SiteBLL.GetWeixinNewsAllList("", "replay_id in(" + SiteBLL.GetWeixinInfo(base.id).nomatch_replay + ")"));
                base.DisplayTemplate(context, "weixin/weixin_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("weixin_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateWeixinFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改微信公众号");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("weixin_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateWeixinFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("weixin_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteWeixinInfo("mp_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除微信公众号");
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
                this.IsChecked("weixin_del", true);

                //执行删除
                SiteBLL.DeleteWeixinInfo(base.id);

                //日志记录
                base.AddLog("删除微信公众号");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 单独设置appid
            else if (this.act == "setAppid")
            {
                //检测权限
                this.IsChecked("weixin_edit", true);

                string appid = DYRequest.getForm("appid");
                string appsecret = DYRequest.getForm("appsecret");
                base.pid = DYRequest.getFormInt("pid");

                if (!string.IsNullOrEmpty(appid))
                {
                    //执行修改
                    SiteBLL.UpdateWeixinFieldValue("appid", appid, base.pid);
                }
                if (!string.IsNullOrEmpty(appsecret))
                {
                    //执行修改
                    SiteBLL.UpdateWeixinFieldValue("appsecret", appsecret, base.pid);
                }


                //输出json数据
                base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
            }
            #endregion

            #region 统计
            else if (this.act == "stats")
            {
                //检测权限
                this.IsChecked("weixin_stats", true);

                Session["mp_id"] = base.pid;
                IDictionary context = new Hashtable();
                StringBuilder sb = new StringBuilder();
                StringBuilder weixin_user_data = new StringBuilder();
                int d = DYRequest.getRequestInt("d", 15);
                for (int i = d - 1; i >= 0; i--)
                {
                    if (DateTime.Now.AddDays(-i).Day == DateTime.Now.Day)
                    {
                        sb.Append("'今天',");
                        //order_data.Append("" + this.GetOrderCount(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ",");
                        //user_data.Append("" + this.GetUserCount(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ",");
                        weixin_user_data.Append("" + this.GetWeixinUserCount(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ",");
                    }
                    else
                    {
                        sb.Append("'" + DateTime.Now.AddDays(-i).ToString("M月d") + "',");
                        //order_data.Append("" + this.GetOrderCount(DateTime.Now.AddDays(-i).Year, DateTime.Now.AddDays(-i).Month, DateTime.Now.AddDays(-i).Day) + ",");
                        //user_data.Append("" + this.GetUserCount(DateTime.Now.AddDays(-i).Year, DateTime.Now.AddDays(-i).Month, DateTime.Now.AddDays(-i).Day) + ",");
                        weixin_user_data.Append("" + this.GetWeixinUserCount(DateTime.Now.AddDays(-i).Year, DateTime.Now.AddDays(-i).Month, DateTime.Now.AddDays(-i).Day) + ",");
                    }
                }
                context.Add("days", sb.Remove(sb.Length - 1, 1));
                context.Add("weixin_user_data", weixin_user_data.Remove(weixin_user_data.Length - 1, 1));
                context.Add("d", d);
                context.Add("entity", SiteBLL.GetWeixinInfo(base.pid));
                context.Add("update", DYRequest.getRequest("update"));
                context.Add("appstoreList", SiteBLL.GetAppstoreAllList("", "is_buy=1"));

                base.DisplayTemplate(context, "weixin/stats");
            }
            #endregion
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            string filter = "";

            this.GetList("weixin/weixin_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            context.Add("list", SiteBLL.GetWeixinList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("mp_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            context.Add("sort_by", DYRequest.getRequest("sort_by"));
            context.Add("sort_order", DYRequest.getRequest("sort_order"));
            context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected WeixinInfo SetEntity()
        {
            WeixinInfo entity = new WeixinInfo();

            entity.name = DYRequest.getForm("name");
            entity.ghid = DYRequest.getForm("ghid");
            entity.code = DYRequest.getForm("code");
            entity.head = DYRequest.getForm("photo");
            entity.token = string.IsNullOrEmpty(DYRequest.getForm("token")) ? System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(DYRequest.getForm("ghid"), "md5").ToUpper() : DYRequest.getForm("token"); //MD5生成token
            entity.create_date = DateTime.Now;//DYRequest.getFormDateTime("caeate_date");
            entity.enabled = true;
            entity.mp_id = base.id;
            entity.iurl = "http://" + new SiteUtils().GetDomain() + "/api/weixinApi.aspx?pid=" + base.id;//string.IsNullOrEmpty(DYRequest.getForm("iurl"))
            entity.sbuscribe = DYRequest.getForm("sbuscribe");
            entity.appid = DYRequest.getForm("appid");
            entity.appsecret = DYRequest.getForm("appsecret");
            if (!string.IsNullOrEmpty(entity.appid) && !string.IsNullOrEmpty(entity.appsecret))
            {
                Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.Register(entity.appid, entity.appsecret);
                //修改多平台微信节点
                UpdateConfig("weixin");
            }
            entity.nomatch_replay = DYRequest.getForm("nomatch_replay");
            entity.username = DYRequest.getForm("username");
            entity.password = DYRequest.getForm("password");
            entity.send_content = DYRequest.getForm("send_content");
            entity.encodingAESKey = DYRequest.getForm("encodingAESKey");
            //if (Wx_MoniLoGin.ExecLogin(entity.username, entity.password))
            //{
            //    ArrayList faid = Wx_MoniLoGin.SubscribeMP();
            //    if (faid.Count > 0)
            //    {
            //        foreach (ArrayList row in faid)
            //        {
            //           Wx_MoniLoGin.SendMessage("测试",row.ToString());
            //        }
            //    }
            //}

            return entity;
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
                            item.InnerText = DYRequest.getForm("appid");
                        else if (item.Name == "clientSecret")
                            item.InnerText = DYRequest.getForm("appsecret");
                        else if (item.Name == "redirectUri")
                            item.InnerText = "http://" + new SiteUtils().GetDomain() + "/api/oauth_return.aspx";//DYRequest.getForm("redirectUri");
                    }
                }
                _xml.Save(Server.MapPath(OAuthConfig.configPath));
            }
        }

        /// <summary>
        /// 取得指定日期制定公众号的关注人数
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        protected int GetWeixinUserCount(int year, int month, int day)
        {
            object obj = SiteBLL.GetWeixinUserValue("COUNT(user_id)", "YEAR(subscribe_time)=" + year + " and MONTH(subscribe_time)=" + month + " and DAY(subscribe_time)=" + day + "");

            return Convert.ToInt32(obj);
        }


    }
}



