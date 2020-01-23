/**
 * 功能描述：WeixinUser管理类
 * 创建时间：2014/3/18 10:02:39
 * 最后修改时间：2014/3/18 10:02:39
 * 作者：gudufy
 * 文件标识：3801cd87-6871-4d11-82fe-d5ed738c3863
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
using Senparc.Weixin.Exceptions;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.User;

namespace DY.Web.admin
{
    public partial class weixin_user : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("weixin_user_list");

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("weixin_user_add");

                if (ispost)
                {
                    base.id = SiteBLL.InsertWeixinUserInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加weixin_user");

                    Hashtable links = new Hashtable();
                    links.Add("继续添加", "?act=add");

                    //显示提示信息
                    this.DisplayMessage("weixin_user添加成功", 2, "?act=list", links);
                }

                IDictionary context = new Hashtable();

                base.DisplayTemplate(context, "weixin_user/weixin_user_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("weixin_user_edit");

                if (ispost)
                {
                    SiteBLL.UpdateWeixinUserInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("修改weixin_user");

                    base.DisplayMessage("weixin_user修改成功", 2, "?act=list");
                }

                IDictionary context = new Hashtable();
                context.Add("entity", SiteBLL.GetWeixinUserInfo(base.id));
                context.Add("update", DYRequest.getRequest("update"));

                base.DisplayTemplate(context, "weixin_user/weixin_user_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("weixin_user_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateWeixinUserFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改weixin_user");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("weixin_user_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateWeixinUserFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("weixin_user_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteWeixinUserInfo("user_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除weixin_user");
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
                this.IsChecked("weixin_user_del", true);

                //执行删除
                SiteBLL.DeleteWeixinUserInfo(base.id);

                //日志记录
                base.AddLog("删除weixin_user");

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

            this.GetList("weixin/weixin_user_list", filter);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            WeixinInfo entity = SiteBLL.GetWeixinInfo(base.pid);
            if (CookieHelper.GetCookieValue("get_weixin_user_info") != "yes")
            {
                try
                {
                    string accessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.TryGetToken(entity.appid, entity.appsecret);
                    OpenIdResultJson openJson = UserApi.Get(accessToken, "");
                    context.Add("openJson", openJson);
                    context.Add("accessToken", accessToken);
                    CookieHelper.SetCookie("get_weixin_user_info", "yes", DateTime.Now.AddMinutes(4.0));
                    SiteBLL.TruncateWeixinUser();
                    foreach (UserInfoJson userjson in SiteUtils.GetWeixinUserList(accessToken, openJson.data.openid))
                    { 
                        WeixinUserInfo userinfo = new WeixinUserInfo();
                        userinfo.subscribe = userjson.subscribe;
                        userinfo.openid = userjson.openid;
                        userinfo.nickname = userjson.nickname;
                        userinfo.sex = userjson.sex;
                        userinfo.city = userjson.city;
                        userinfo.country = userjson.country;
                        userinfo.province = userjson.province;
                        userinfo.language = userjson.language;
                        userinfo.headimgurl = userjson.headimgurl;
                        userinfo.subscribe_time = Utils.StrToDataTime(SiteUtils.GetDateTimeFromXml(userjson.subscribe_time),DateTime.Now);
                        userinfo.user_id = base.id;
                        userinfo.pid = base.pid;
                        base.id = SiteBLL.InsertWeixinUserInfo(userinfo);
                    }
                }
                catch (WeixinException w)
                {
                    base.WriteFile(DateTime.Now + "\t" + w.Message);
                    base.DisplayMessage(ReturnCode.接口调用超过限制.ToString(), 2, "weixin_set.aspx?act=list");
                }
            }

            context.Add("list", SiteBLL.GetWeixinUserList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("user_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
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
        protected WeixinUserInfo SetEntity()
        {
            WeixinUserInfo entity = new WeixinUserInfo();

            entity.subscribe = DYRequest.getFormInt("subscribe");
            entity.openid = DYRequest.getForm("openid");
            entity.nickname = DYRequest.getForm("nickname");
            entity.sex = DYRequest.getFormInt("sex");
            entity.city = DYRequest.getForm("city");
            entity.country = DYRequest.getForm("country");
            entity.province = DYRequest.getForm("province");
            entity.language = DYRequest.getForm("language");
            entity.headimgurl = DYRequest.getForm("headimgurl");
            entity.subscribe_time = DYRequest.getFormDateTime("subscribe_time");
            entity.user_id = base.id;

            return entity;
        }
    }
}


