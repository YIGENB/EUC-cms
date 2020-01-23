using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.OAuths;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.Helpers;
using DY.OAuthV2SDK.OAuths.Neasys.Models;

namespace DY.OAuthV2SDK.OAuths.Neasys
{
    /// <summary>
    /// 网易微博协议
    /// </summary>
    public class NeasyOAuth : OAuthBase
    {
        /// <summary>
        /// 协议节点名称(区分大小写)
        /// </summary>
        public override string OAuthName { get { return "neasy"; } }
        /// <summary>
        /// 协议节点描述
        /// </summary>
        public override string OAuthDesc { get { return "网易微博"; } }

        public NeasyOAuth() { }

        public NeasyOAuth(string cfgAppName) { }

        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        public override ApiToken GetAccessToken()
        {
            this.ClientId = this.App.AppKey;
            this.RedirectUri = this.App.RedirectUri;
            string accessTokenUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "access_token");
            NeasyMToken token = UtilHelper.ParseJson<NeasyMToken>(GetAccessToken(accessTokenUrl));
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
            string response = ApiByHttpGet("account_verify_credentials", paras);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "account_verify_credentials";
            NeasyMUser user = UtilHelper.ParseJson<NeasyMUser>(response);
            if (user.error_code != 0)
            {
                api.ret = 1;
                api.errcode = Convert.ToString(user.error_code);
                api.msg = user.description;
            }
            else
            {
                api.data = Convert.ToString(user.id);
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
            paras.Add("status", strText);
            string response = ApiByHttpPost("statuses_update", paras);
            NeasyMStatus status = UtilHelper.ParseJson<NeasyMStatus>(response);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "statuses_update";
            if (status.error_code == 0)
            {
                api.data = Convert.ToString(status.id);
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
            files.Add("pic", strFile);
            string upload_image_url = string.Empty;
            //1步，发图片
            string response = ApiByHttpPostWithPic("statuses_upload", paras, files);
            ADictionary<string, string> image = UtilHelper.ParseJson<ADictionary<string, string>>(response);
            if (image.ContainsKey("upload_image_url"))
            {
                upload_image_url = image["upload_image_url"].ToString();
            }
            paras.Add("status", strText + upload_image_url);
            //2步，发微博
            response = ApiByHttpPost("statuses_update", paras);
            NeasyMStatus status = UtilHelper.ParseJson<NeasyMStatus>(response);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "statuses_update";
            if (status.error_code == 0)
            {

                api.data = Convert.ToString(status.id);
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