﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using DY.Entity;
using DY.Site;
using System.IO;

namespace DY.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        private IResponseMessageBase GetWelcomeInfo()
        {
            IResponseMessageBase reponseMessage = null;
            WeixinInfo weixin = SiteBLL.GetWeixinInfo(Convert.ToInt32(System.Web.HttpContext.Current.Session["pid"]));
            if (!string.IsNullOrEmpty(weixin.sbuscribe))
            {
                string[] sbuscribe = weixin.sbuscribe.Split(',');
                for (int i = 0; i < sbuscribe.Length; i++)
                {
                    foreach (WeixinNewsInfo news in SiteBLL.GetWeixinNewsAllList("", "enabled=1 and replay_id=" + sbuscribe[i]))
                    {
                        #region 完全匹配
                        reponseMessage = GetKeyWordNews(news);
                        return reponseMessage;
                        #endregion
                    }
                }
            }
            return reponseMessage;
        }

        /// <summary>
        /// 重写
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //using (TextWriter tw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/bin/Error_" + DateTime.Now.Ticks + ".txt")))
            //{
            //    tw.WriteLine(requestMessage.EventKey);
            //    tw.Flush();
            //    tw.Close();
            //}
            WeixinMenuInfo menu = SiteBLL.GetWeixinMenuInfo("enabled=1 and pid=" + System.Web.HttpContext.Current.Session["pid"] + " and menu_key='" + requestMessage.EventKey.Trim() + "'");
            if (menu!=null)
            {
                #region 匹配信息
                foreach (WeixinNewsInfo news in SiteBLL.GetWeixinNewsAllList("", "enabled=1 and pid=" + System.Web.HttpContext.Current.Session["pid"]))
                {
                    #region 完全匹配
                    if (news.type == 0)
                    {
                        if (menu.trigger_word.Trim() == news.keyword)
                        {
                            reponseMessage = GetKeyWordNews(news);
                            return reponseMessage;
                        }
                    }
                    #endregion
                    #region 包含匹配
                    else if (news.type == 1)
                    {
                        if (menu.trigger_word.Trim().Contains(news.keyword))
                        {
                            reponseMessage = GetKeyWordNews(news);
                            return reponseMessage;
                        }
                    }
                    #endregion
                }
                #endregion
            }
            //菜单点击，需要跟创建菜单时的Key匹配
            //switch (requestMessage.EventKey)
            //{
                //case "wz":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
                //    }
                //    break;
                //case "SubClickRoot_Text":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Content = "您点击了子菜单按钮。";
                //    }
                //    break;
                //case "SubClickRoot_News":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Articles.Add(new Article()
                //        {
                //            Title = "您点击了子菜单图文按钮",
                //            Description = "您点击了子菜单图文按钮，这是一条图文信息。",
                //            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg",
                //            Url = "http://weixin.senparc.com"
                //        });
                //    }
                //    break;
                //case "SubClickRoot_Music":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageMusic>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
                //    }
                //    break;
                //case "SubClickRoot_Image":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                //        reponseMessage = strongResponseMessage;
                //        strongResponseMessage.Image.MediaId = "Mj0WUTZeeG9yuBKhGP7iR5n1xUJO9IpTjGNC4buMuswfEOmk6QSIRb_i98do5nwo";
                //    }
                //    break;
                //case "SubClickRoot_Agent"://代理消息
                //    {
                //        //获取返回的XML
                //        DateTime dt1 = DateTime.Now;
                //        reponseMessage = MessageAgent.RequestResponseMessage(this, agentUrl, agentToken, RequestDocument.ToString());
                //        //上面的方法也可以使用扩展方法：this.RequestResponseMessage(this,agentUrl, agentToken, RequestDocument.ToString());

                //        DateTime dt2 = DateTime.Now;

                //        if (reponseMessage is ResponseMessageNews)
                //        {
                //            (reponseMessage as ResponseMessageNews)
                //                .Articles[0]
                //                .Description += string.Format("\r\n\r\n代理过程总耗时：{0}毫秒", (dt2 - dt1).Milliseconds);
                //        }
                //    }
                //    break;
                //case "Member"://托管代理会员信息
                //    {
                //        //原始方法为：MessageAgent.RequestXml(this,agentUrl, agentToken, RequestDocument.ToString());//获取返回的XML
                //        reponseMessage = this.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
                //    }
                //    break;
                //case "OAuth"://OAuth授权测试
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                //        strongResponseMessage.Articles.Add(new Article()
                //        {
                //            Title = "OAuth2.0测试",
                //            Description = "点击【查看全文】进入授权页面。\r\n注意：此页面仅供测试（是专门的一个临时测试账号的授权，并非Senparc.Weixin.MP SDK官方账号，所以如果授权后出现错误页面数正常情况），测试号随时可能过期。请将此DEMO部署到您自己的服务器上，并使用自己的appid和secret。",
                //            Url = "http://weixin.senparc.com/oauth2",
                //            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg"
                //        });
                //        reponseMessage = strongResponseMessage;
                //    }
                //    break;
                //case "Description":
                //    {
                //        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                //        strongResponseMessage.Content = GetWelcomeInfo();
                //        reponseMessage = strongResponseMessage;
                //    }
                //    break;
            //}

            return reponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "欢迎使用创同盟营销系统！";
            return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            //responseMessage.Content = GetWelcomeInfo();
            return GetWelcomeInfo();
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }
    }
}