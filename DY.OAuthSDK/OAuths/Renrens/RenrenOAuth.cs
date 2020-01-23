using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.OAuths;
using DY.OAuthV2SDK.Helpers;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.OAuths.Renrens.Models;

namespace DY.OAuthV2SDK.OAuths.Renrens
{
    /// <summary>
    /// 人人网协议
    /// </summary>
    public class RenrenOAuth : OAuthBase
    {
        /// <summary>
        /// 协议节点名称(区分大小写)
        /// </summary>
        public override string OAuthName { get { return "renren"; } }
        /// <summary>
        /// 协议节点描述
        /// </summary>
        public override string OAuthDesc { get { return "人人网"; } }

        public RenrenOAuth() { }

        public RenrenOAuth(string cfgAppName) { }

        /// <summary>
        /// 获取参数签名
        /// </summary>
        /// <param name="paras">参数列表</param>
        /// <returns></returns>
        public string GetParaSig(NameValueCollection paras)
        {
            var list = new List<string>();
            foreach (var key in paras.AllKeys)
            {
                list.Add(string.Format("{0}={1}", key, paras[key]));
            }
            list.Sort();
            list.Add(this.ClientSecret);
            return SecurityHelper.MD5Encrypt(string.Join("", list.ToArray()));
        }


        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        public override ApiToken GetAccessToken()
        {
            this.ClientId = this.App.AppKey;
            this.RedirectUri = this.App.RedirectUri;
            string accessTokenUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, "access_token");
            RenrenMToken token = UtilHelper.ParseJson<RenrenMToken>(GetAccessToken(accessTokenUrl));
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
                api.msg = token.error_description;
            }
            return api;
        }

        /// <summary>
        /// 根据api名称获取资源(GET)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <returns></returns>
        public override string ApiByHttpGet(string apiName, NameValueCollection queryParas)
        {
            queryParas.Add("sig", this.GetParaSig(queryParas));
            return base.ApiByHttpGet(apiName, queryParas);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <returns></returns>
        public override string ApiByHttpPost(string apiName, NameValueCollection queryParas)
        {
            queryParas.Add("sig", this.GetParaSig(queryParas));
            return base.ApiByHttpPost(apiName, queryParas);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <param name="files">文件路径集合</param>
        /// <returns></returns>
        public override string ApiByHttpPostWithPic(string apiName, NameValueCollection queryParas, NameValueCollection files)
        {
            queryParas.Add("sig", this.GetParaSig(queryParas));
            return base.ApiByHttpPostWithPic(apiName, queryParas, files);
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <returns></returns>
        public override ApiResult GetUid()
        {
            ApiResult api = new ApiResult();
            NameValueCollection paras = this.GetTokenParas();
            string response = ApiByHttpGet("user_login_get", paras);
            RenrenMRsUser user = UtilHelper.ParseJson<RenrenMRsUser>(response);
            if (user.error == null)
            {
                api.data = user.response.id;
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
                api.msg = Convert.ToString(dic["error_description"]);
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
            paras.Add("content", strText);
            string response = ApiByHttpPost("status_put", paras);
            RenrenMRsStatus result = UtilHelper.ParseJson<RenrenMRsStatus>(response);
            ApiResult api = new ApiResult();
            api.response = response;
            api.request = "statuses_update";
            if (result.error == null)
            {
                api.data = Convert.ToString(result.response.id);
            }
            else
            {
                api.ret = 1;
                api.errcode = result.error.code;
                api.msg = result.error.message;
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
            //官方暂无接口
            ApiResult api = new ApiResult();
            api.ret = 1;
            api.errcode = "1";
            api.msg = "官方暂无接口";
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