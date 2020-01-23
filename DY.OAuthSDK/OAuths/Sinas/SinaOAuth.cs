using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.OAuths;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.Helpers;
using DY.OAuthV2SDK.OAuths.Sinas.Models;

namespace DY.OAuthV2SDK.OAuths.Sinas
{
    /// <summary>
    /// 新浪微博协议
    /// </summary>
    public class SinaOAuth : OAuthBase
    {
        /// <summary>
        /// 协议节点名称(区分大小写)
        /// </summary>
        public override string OAuthName { get { return "sina"; } }
        /// <summary>
        /// 协议节点描述
        /// </summary>
        public override string OAuthDesc { get { return "新浪微博"; } }

        public SinaOAuth() { }

        public SinaOAuth(string cfgAppName) { }

        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        public override ApiToken GetAccessToken()
        {
            string accessTokenUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "access_token");
            string response = GetAccessToken(accessTokenUrl);
            SinaMToken token = UtilHelper.ParseJson<SinaMToken>(response);
            ApiToken api = new ApiToken();
            api.request = "access_token";
            api.response = response;
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
                api.msg = SinaApiError.GetChinese(api.errcode);
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
            string response = ApiByHttpGet("account_get_uid", paras);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "account_get_uid";
            ADictionary<string, object> dic = UtilHelper.ParseJson<ADictionary<string, object>>(response);
            if (dic.ContainsKey("error_code"))
            {
                api.ret = 1;
                api.errcode = Convert.ToString(dic["error_code"]);
                api.msg = SinaApiError.GetChinese(api.errcode);
            }
            else if (dic.ContainsKey("uid"))
            {
                api.data = Convert.ToString(dic["uid"]);
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
            SinaMStatus status = UtilHelper.ParseJson<SinaMStatus>(response);
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
                api.msg = SinaApiError.GetChinese(api.errcode);
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
            paras.Add("status", strText);
            string response = ApiByHttpPostWithPic("statuses_upload", paras, files);
            SinaMStatus status = UtilHelper.ParseJson<SinaMStatus>(response);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "statuses_upload";
            if (status.error_code == 0)
            {

                api.data = Convert.ToString(status.id);
            }
            else
            {
                api.ret = 1;
                api.errcode = Convert.ToString(status.error_code);
                api.msg = SinaApiError.GetChinese(api.errcode);
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