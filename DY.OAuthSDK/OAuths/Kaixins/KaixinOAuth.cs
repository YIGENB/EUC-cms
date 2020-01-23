using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.OAuths;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.Helpers;
using DY.OAuthV2SDK.OAuths.Kaixins.Models;

namespace DY.OAuthV2SDK.OAuths.Kaixins
{
    /// <summary>
    /// 开心网协议
    /// </summary>
    public class KaixinOAuth : OAuthBase
    {
        /// <summary>
        /// 协议节点名称(区分大小写)
        /// </summary>
        public override string OAuthName { get { return "kaixin"; } }
        /// <summary>
        /// 协议节点描述
        /// </summary>
        public override string OAuthDesc { get { return "开心网"; } }


        public KaixinOAuth() { }

        public KaixinOAuth(string cfgAppName) { }

        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        public override ApiToken GetAccessToken()
        {
            this.ClientId = this.App.AppKey;
            this.RedirectUri = this.App.RedirectUri;
            string accessTokenUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "access_token");
            KaixinMToken token = UtilHelper.ParseJson<KaixinMToken>(GetAccessToken(accessTokenUrl));
            ApiToken api = new ApiToken();
            api.request = "access_token";
            if (token.error_code == 0)
            {
                api.access_token = token.access_token;
                api.refresh_token = token.refresh_token;
                api.expires_in = token.expires_in;
            }
            else
            {
                api.ret = 1;
                api.errcode = Convert.ToString(token.error_code);
                api.msg = token.error;
            }
            return api;
        }

        /// <summary>
        /// 获取用户id
        /// <para>data: 当前的返回Uid</para>
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <returns></returns>
        public override ApiResult GetUid(string accessToken)
        {
            this.AccessToken = accessToken;
            NameValueCollection paras = this.GetTokenParas();
            string response = ApiByHttpGet("users_me", paras);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "users_me";
            KaixinMUser user = UtilHelper.ParseJson<KaixinMUser>(response);
            if (user.error_code != 0)
            {
                api.ret = 1;
                api.errcode = Convert.ToString(user.error_code);
                api.msg = user.error;
            }
            else
            {
                api.data = Convert.ToString(user.uid);
            }

            return api;
        }


        /// <summary>
        /// 发送微博
        ///  <para>data: 当前的返回status_id</para>
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="strText">微博内容</param>
        /// <returns></returns>
        public override ApiResult SendStatus(string accessToken, string strText)
        {
            this.AccessToken = accessToken;
            NameValueCollection paras = this.GetTokenParas();
            paras.Add("content", strText);
            string response = ApiByHttpPost("records_add", paras);
            KaixinMRecord status = UtilHelper.ParseJson<KaixinMRecord>(response);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "records_add";
            if (status.error_code == 0)
            {
                api.data = Convert.ToString(status.rid);
            }
            else
            {
                api.ret = 1;
                api.errcode = Convert.ToString(status.error_code);
                api.msg = status.message_code;
            }
            return api;
        }

        /// <summary>
        /// 发送图片微博
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="strText">微博内容</param>
        /// <param name="strFile">图片绝对路径</param>
        /// <returns></returns>
        public override ApiResult SendStatusWithPic(string accessToken, string strText, string strFile)
        {
            this.AccessToken = accessToken;
            NameValueCollection paras = this.GetTokenParas();
            NameValueCollection files = this.GetEmptyParas();
            paras.Add("content", strText);
            files.Add("pic", strFile);
            string response = ApiByHttpPostWithPic("records_add", paras, files);
            KaixinMRecord status = UtilHelper.ParseJson<KaixinMRecord>(response);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "records_add";
            if (status.error_code == 0)
            {

                api.data = Convert.ToString(status.rid);
            }
            else
            {
                api.ret = 1;
                api.errcode = Convert.ToString(status.error_code);
                api.msg = status.message_code;
            }
            return api;
        }

        /// <summary>
        /// 发送文章类型，如头条
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="title">标题</param>
        /// <param name="des">简介</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public override ApiResult SendStatusToArticle(string accessToken, string title, string des, string content)
        {
            //官方暂无接口
            ApiResult api = new ApiResult();
            api.ret = 1;
            api.errcode = "1";
            api.msg = "官方暂无接口";
            return api;
        }
    }
}