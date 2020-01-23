using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Configuration;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using DY.Site;
using DY.Entity;
using Senparc.Weixin.MP.Entities.Request;
using DY.LanguagePack;

namespace DY.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */


//#if DEBUG
//        string agentUrl = "http://www.weiweihi.com/App/Weixin/18718";
//        string agentToken = "8B78F3D8EF124F90";
//        string souideaKey = "Gfj4FyYVleH9At5SGxt5/NRNMptf5iZw";
//#else
        //下面的Url和Token可以用其他平台的消息，或者到www.souidea.com注册微信用用，并申请“微信营销工具”得到
        private string agentUrl = WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.souidea.com微信自动托管平台
        private string agentToken = WebConfigurationManager.AppSettings["WeixinAgentToken"];//Token
        private string souideaKey = WebConfigurationManager.AppSettings["WeixinAgentSouideaKey"];//SouideaKey专门用于对接www.souidea.com平台，获取方式见：http://www.souidea.com/ApiDocuments/Item/25#51
//#endif

        private string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
        }

        public override void OnExecuting()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            IResponseMessageBase responseMessage = null;
            //临时存储
            System.Web.HttpContext.Current.Session["content"] = requestMessage.Content.Trim();
            #region 匹配信息
            foreach (WeixinNewsInfo news in SiteBLL.GetWeixinNewsAllList("", "enabled=1 and pid=" + System.Web.HttpContext.Current.Session["pid"]))
            {
                #region 完全匹配
                if (news.type == 0)
                {
                    if (requestMessage.Content.Trim() == news.keyword)
                    {
                        responseMessage = GetKeyWordNews(news);
                        return responseMessage;
                    }
                }
                #endregion
                #region 包含匹配
                else if (news.type == 1)
                {
                    if (requestMessage.Content.Trim().Contains(news.keyword))
                    {
                        responseMessage = GetKeyWordNews(news);
                        return responseMessage;
                    }
                }
                #endregion
            }
                #endregion

            #region 匹配刮刮卡
            foreach (CardInfo card in SiteBLL.GetCardAllList("", "is_enabled=1"))
            {
                #region 完全匹配
                if (requestMessage.Content.Trim() == card.weixin_word)
                {
                    responseMessage = GetActivitiesKeyWordNews(card);
                    return responseMessage;
                }
                #endregion
            }
            #endregion

            #region 匹配会员信息
            if (requestMessage.Content.Trim() == "登录")
            {
                responseMessage = GetUsersResponseMessage();
                return responseMessage;
            }
            #endregion

            responseMessage = DefaultResponseMessage(requestMessage);
            return responseMessage;
        }

        public bool ResponseType(int type) 
        {
            bool flag = false;
            switch (type)
            {
                case 0: flag=true; break;
            }
            return flag;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = requestMessage.PicUrl,
                Url = "http://www.ctmon.com"
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "第二条",
                Description = "第二条带连接的内容",
                PicUrl = requestMessage.PicUrl,
                Url = "http://www.ctmon.com"
            });
            return responseMessage;
        }


        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = "http://" + new SiteUtils().GetDomain() +"/mp3/wmzdyd.mp3";
            responseMessage.Music.Title = "这里是一条音乐消息";
            responseMessage.Music.Description = "来自回音哥";
            return responseMessage;
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条视频信息，ID：" + requestMessage.MediaId;
            return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
Title：{0}
Description:{1}
Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */
                //return MessageAgent.RequestResponseMessage(this, agentUrl, agentToken, RequestDocument.ToString());//this.CreateResponseMessage<ResponseMessageText>();
            return GetDefaultInfo(requestMessage);
        }

        private IResponseMessageBase GetDefaultInfo(IRequestMessageBase requestMessage)
        {
            RequestMessageText request = requestMessage as RequestMessageText;
            var respMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            IResponseMessageBase reponseMessage = null;
            WeixinInfo weixin = SiteBLL.GetWeixinInfo(Convert.ToInt32(System.Web.HttpContext.Current.Session["pid"]));
            if (!string.IsNullOrEmpty(weixin.nomatch_replay))
            {
                if (Convert.ToInt32(weixin.nomatch_replay) ==0)
                {
                    #region 搜索数据
                    foreach (SearchInfo search in new Caches().Search(8, request.Content.Trim()))
                    {
                        string detail = search.type.Value == 1 ? urlrewrite.product_detail : urlrewrite.article_detail;
                        respMessage.Articles.Add(new Article()
                        {
                            Title = search.title,
                            Description = search.des,
                            PicUrl = !search.photo.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + search.photo.Trim() : search.photo.Trim(),
                            Url = urlrewrite.http + new SiteUtils().GetDomain() + detail + search.type_id + config.UrlRewriterKzm
                        });
                    }
                    return respMessage;
                    #endregion
                }
                else
                {
                    string[] sbuscribe = weixin.nomatch_replay.Split(',');
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
            }
            return reponseMessage;
        }
    }
}
