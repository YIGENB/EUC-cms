/**
 * 功能描述：WeixinMenu管理类
 * 创建时间：2014/2/26 11:32:40
 * 最后修改时间：2014/2/26 11:32:40
 * 作者：gudufy
 * 文件标识：2033739d-11d3-40e4-88bd-063053b28d5d
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
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.Weixin.MP;

namespace DY.Web.admin
{
    public partial class weixin_menu : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 列表
            if (this.act == "list")
            {
                //检测权限
                this.IsChecked("weixin_menu_list");

                //检测公众号id
                this.IsCheckedWeCat(base.pid);

                //显示列表数据
                this.GetList();
            }
            #endregion

            #region 添加
            else if (this.act == "add")
            {
                //检测权限
                this.IsChecked("weixin_menu_add");

                //检测公众号id
                this.IsCheckedWeCat(DYRequest.getFormInt("pid"));

                if (ispost)
                {
                    base.id = SiteBLL.InsertWeixinMenuInfo(this.SetEntity());

                    //日志记录
                    base.AddLog("添加自定义菜单");

                    //Hashtable links = new Hashtable();
                    //links.Add("继续添加", "?act=add&pid=" + base.pid);

                    ////显示提示信息
                    //this.DisplayMessage("自定义菜单添加成功", 2, "?act=list&pid=" + base.pid, links);
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, null));
                }

                //IDictionary context = new Hashtable();

                //base.DisplayTemplate(context, "weixin/weixin_menu_info");
            }
            #endregion

            #region 修改
            else if (this.act == "edit")
            {
                //检测权限
                this.IsChecked("weixin_menu_edit");
                //检测公众号id
                this.IsCheckedWeCat(DYRequest.getFormInt("pid"));

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    SiteBLL.UpdateWeixinMenuInfo(this.SetEntity());

                    ////日志记录
                    base.AddLog("修改自定义菜单");

                    //base.DisplayMessage("自定义菜单修改成功", 2, "?act=list&pid=" + base.pid);
                    base.DisplayMemoryTemplate(base.MakeJson("", 0, null));
                }

                //IDictionary context = new Hashtable();
                //context.Add("entity", SiteBLL.GetWeixinMenuInfo(base.id));
                //context.Add("update", DYRequest.getRequest("update"));

                //base.DisplayTemplate(context, "weixin/weixin_menu_info");
            }
            #endregion

            #region 更新单个字段值
            else if (this.act == "edit_field_value")
            {
                //检测权限
                this.IsChecked("weixin_menu_edit", true);

                if (ispost)
                {
                    base.id = DYRequest.getFormInt("id");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    //执行修改
                    SiteBLL.UpdateWeixinMenuFieldValue(fieldName, val, base.id);

                    //日志记录
                    base.AddLog("修改自定义菜单");

                    //输出json数据
                    base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
                }
            }
            #endregion

            #region 更新多条记录的单个字段值
            else if (this.act == "edit_field_values")
            {
                //检测权限
                this.IsChecked("weixin_menu_edit", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");
                    object val = DYRequest.getForm("val");
                    string fieldName = DYRequest.getForm("fieldName");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行修改
                        SiteBLL.UpdateWeixinMenuFieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
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
                this.IsChecked("weixin_menu_del", true);

                if (ispost)
                {
                    string ids = DYRequest.getForm("ids");

                    if (!string.IsNullOrEmpty(ids))
                    {
                        //执行删除
                        SiteBLL.DeleteWeixinMenuInfo("menu_id in (" + ids.Remove(ids.Length - 1, 1) + ")");

                        //日志记录
                        base.AddLog("删除自定义菜单");
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
                this.IsChecked("weixin_menu_del", true);

                //执行删除
                base.id = DYRequest.getFormInt("id");
                SiteBLL.DeleteWeixinMenuInfo(base.id);

                //日志记录
                base.AddLog("删除自定义菜单");

                //显示列表数据
                //this.GetList();
            }
            #endregion

            #region 生成自定义菜单
            else if (this.act == "create")
            {
                //检测权限
                this.IsChecked("weixin_menu_create");
                StringBuilder sb = new StringBuilder();
                string type = "";

                ButtonGroup bg = new ButtonGroup();
                foreach (WeixinMenuInfo menu in SiteBLL.GetWeixinMenuAllList("", "parent_id=0 and enabled=1 and pid=" + base.pid))
                {
                    type = menu.trigger_word.Contains("http") ? ButtonType.view.ToString() : ButtonType.click.ToString();
                    if (SiteBLL.GetWeixinMenuAllList("", "parent_id=" + menu.menu_id + " and enabled=1 and pid=" + base.pid).Count > 0)
                    {
                        //二级菜单
                        var subButton = new SubButton()
                        {
                            name = menu.name
                        };

                        foreach (WeixinMenuInfo entity in SiteBLL.GetWeixinMenuAllList("", "parent_id=" + menu.menu_id + " and enabled=1 and pid=" + base.pid))
                        {
                            if (entity.trigger_word.Contains("http"))
                            {
                                subButton.sub_button.Add(new SingleViewButton()
                                {
                                    url = entity.trigger_word,
                                    name = entity.name
                                });
                            }
                            else
                            {
                                subButton.sub_button.Add(new SingleClickButton()
                                {
                                    key = entity.menu_key,
                                    name = entity.name
                                });
                            }
                        }
                        bg.button.Add(subButton);
                    }
                    else
                    {
                        //单击
                        bg.button.Add(new SingleClickButton()
                        {
                            name = menu.name,
                            key = menu.menu_key,
                            type = type,//类型
                        });
                    }
                }
                WeixinInfo weixi = SiteBLL.GetWeixinInfo(base.pid);
                string token = AccessTokenContainer.TryGetToken(weixi.appid, weixi.appsecret);
                //日志记录
                base.AddLog("生成自定义菜单");

                base.DisplayMessage("生成自定义菜单：" + CreateMenu(token, bg), 2, "?act=list&pid=" + base.pid);

            }
            #endregion

            #region 撤销自定义菜单
            else if (this.act == "del")
            {
                //检测权限
                this.IsChecked("weixin_menu_del", true);

                //执行删除
                WeixinInfo weixi = SiteBLL.GetWeixinInfo(base.pid);
                string token = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.TryGetToken(weixi.appid, weixi.appsecret);
                Senparc.Weixin.MP.Entities.WxJsonResult json = Senparc.Weixin.MP.CommonAPIs.CommonApi.DeleteMenu(token);

                //日志记录
                base.AddLog("撤销自定义菜单" + json.errmsg);
                base.DisplayMessage("撤销自定义菜单" + json.errmsg, 2, "?act=list&pid=" + base.pid);
            }
            #endregion
        }

        public string CreateMenu(string token, ButtonGroup buttonData)
        {
            try
            {
                //重新整理按钮信息
                //var bg = DY.Weixin.MP.CommonAPIs.CommonApi.GetMenuFromJsonResult(resultFull).menu;
                var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(token, buttonData);
                string message = "";
                //var json = new
                //{
                //    Success = result.errmsg == "ok",
                message = result.errmsg;
                //};
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList()
        {
            IDictionary context = new Hashtable();
            WeixinInfo weixi = SiteBLL.GetWeixinInfo(base.pid);
            context.Add("list", caches.WxMenuFormat(0).Rows);
            //context.Add("appid", weixi.appid);
            //context.Add("appsecret", weixi.appsecret);


            base.DisplayTemplate(context, "weixin/weixin_menu_list", base.isajax);
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        protected void GetList(string tpl, string filter)
        {
            IDictionary context = new Hashtable();
            //context.Add("list", SiteBLL.GetWeixinMenuList(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("menu_id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
            //context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
            //to json
            //context.Add("sort_by", DYRequest.getRequest("sort_by"));
            //context.Add("sort_order", DYRequest.getRequest("sort_order"));
            //context.Add("page", base.pageindex);

            base.DisplayTemplate(context, tpl, base.isajax);
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        protected WeixinMenuInfo SetEntity()
        {
            WeixinMenuInfo entity = new WeixinMenuInfo();
            Random r = new Random();

            entity.parent_id = DYRequest.getFormInt("parent_id");
            entity.name = DYRequest.getForm("name");
            entity.trigger_word = DYRequest.getForm("trigger_word");
            entity.sort_order = DYRequest.getFormInt("sort_order");
            entity.enabled = true;
            entity.data = DateTime.Now;//DYRequest.getFormDateTime("data");
            entity.menu_key = FunctionUtils.Text.ConvertSpellFirst(entity.name) +"_"+ SiteUtils.Encryption(r.Next(15).ToString());
            entity.menu_id = base.id;
            entity.pid = DYRequest.getFormInt("pid");

            return entity;
        }
    }
}


