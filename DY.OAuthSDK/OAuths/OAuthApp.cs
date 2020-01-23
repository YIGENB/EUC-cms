using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.Entitys;
using DY.OAuthV2SDK.Helpers;

namespace DY.OAuthV2SDK.OAuths
{
    public abstract partial class OAuthBase
    {
        /// <summary>
        /// 协议节点名称(区分大小写)
        /// </summary>
        public abstract string OAuthName { get; }
        /// <summary>
        /// 协议节点描述
        /// </summary>
        public abstract string OAuthDesc { get; }
        /// <summary>
        /// 当前应用
        /// </summary>
        public virtual AppEntity App { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public virtual string Uid { get; set; }

        /// <summary>
        /// oauth协议app应用
        /// <para>获取app节点第一子节点</para>
        /// </summary>
        public OAuthBase()
        {
            this.App = OAuthConfig.GetConfigApp(OAuthName, string.Empty);
            this.ClientId = this.App.AppKey;
            this.ClientSecret = this.App.AppSecret;
            this.Scope = this.App.Scope;
            this.Appid = this.App.Appid;
            this.UserName = this.App.UserName;
            this.Password = this.App.Password;
            this.RedirectUri = this.App.RedirectUri;
        }

        /// <summary>
        /// oauth协议app应用
        /// </summary>
        /// <param name="cfgAppName">app节点名称(区分大小写)</param>
        /// <returns></returns>
        public OAuthBase(string cfgAppName)
        {
            this.App = OAuthConfig.GetConfigApp(OAuthName, cfgAppName);
        }
        #region 获取资源方法
        /// <summary>
        /// 根据api名称获取Url
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <returns></returns>
        public virtual string ApiUrl(string apiName)
        {
            return OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, apiName);
        }
        /// <summary>
        /// 根据api名称获取资源(GET)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <returns></returns>
        public virtual string ApiUrlByHttpGet(string apiUrl, NameValueCollection queryParas)
        {
            return HttpHelper.HttpGet(apiUrl, queryParas);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <returns></returns>
        public virtual string ApiUrlByHttpPost(string apiUrl, NameValueCollection queryParas)
        {
            return HttpHelper.HttpPost(apiUrl, queryParas);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <param name="files">文件路径集合</param>
        /// <returns></returns>
        public virtual string ApiUrlByHttpPostWithPic(string apiUrl, NameValueCollection queryParas, NameValueCollection files)
        {
            return HttpHelper.HttpPostWithFile(apiUrl, queryParas, files);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <param name="files">文件路径集合</param>
        /// <param name="parasEncode">参数是否编码(解决中文乱码)</param>
        /// <returns></returns>
        public virtual string ApiUrlByHttpPostWithPic(string apiUrl, NameValueCollection queryParas, NameValueCollection files, bool parasEncode)
        {
            return HttpHelper.HttpPostWithFile(apiUrl, queryParas, files, parasEncode);
        }
        /// <summary>
        /// 根据api名称获取资源(GET)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <returns></returns>
        public virtual string ApiByHttpGet(string apiName, NameValueCollection queryParas)
        {
            string apiUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, apiName);
            return HttpHelper.HttpGet(apiUrl, queryParas);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <returns></returns>
        public virtual string ApiByHttpPost(string apiName, NameValueCollection queryParas)
        {
            string apiUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, apiName);
            return HttpHelper.HttpPost(apiUrl, queryParas);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <param name="files">文件路径集合</param>
        /// <returns></returns>
        public virtual string ApiByHttpPostWithPic(string apiName, NameValueCollection queryParas, NameValueCollection files)
        {
            string apiUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, apiName);
            return HttpHelper.HttpPostWithFile(apiUrl, queryParas, files);
        }

        /// <summary>
        /// 根据api名称获取资源(POST)
        /// </summary>
        /// <param name="apiName">api名称</param>
        /// <param name="queryParas">查询参数集合</param>
        /// <param name="files">文件路径集合</param>
        /// <param name="parasEncode">参数是否编码(解决中文乱码)</param>
        /// <returns></returns>
        public virtual string ApiByHttpPostWithPic(string apiName, NameValueCollection queryParas, NameValueCollection files, bool parasEncode)
        {
            string apiUrl = OAuthConfig.GetConfigAPI(OAuthName, this.App.AppName, apiName);
            return HttpHelper.HttpPostWithFile(apiUrl, queryParas, files, parasEncode);
        }
        #endregion


        #region 参数转化操作

        /// <summary>
        /// 获取token参数
        /// </summary>
        /// <returns></returns>
        public virtual NameValueCollection GetTokenParas()
        {
            NameValueCollection paras = new NameValueCollection();
            paras.Add(OAuthAccessTokenKey, this.AccessToken);
            return paras;
        }

        /// <summary>
        /// 获取空参数
        /// </summary>
        /// <returns></returns>
        public virtual NameValueCollection GetEmptyParas()
        {
            return new NameValueCollection();
        }
        #endregion

    }
}