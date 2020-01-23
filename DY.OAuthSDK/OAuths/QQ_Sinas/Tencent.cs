using DY.OAuthSDK.Connect.Model;
using DY.OAuthSDK.Connect.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DY.OAuthSDK.Login
{
    public class Tencent : Connect
    {
        private const String AUTHORIZE_URL = "https://graph.qq.com/oauth2.0/authorize";
        private const String ACCESS_TOKEN_URL = "https://graph.qq.com/oauth2.0/token";
        private const String ACCESS_OPENID = "https://graph.qq.com/oauth2.0/me";

        public Tencent(QQSite site) : base(site) { }

        public override string GetAuthorizeURL(ResponseType response = ResponseType.Code, string state = null, DisplayType display = DisplayType.Default)
        {
            Dictionary<String, String> config = new Dictionary<string, string>(){
               { "response_type", "code"},
               {"client_id",this.appKey},
               {"redirect_uri",this.callbackUrl},
               {"state",state}
            };
            UriBuilder builder = new UriBuilder(AUTHORIZE_URL);
            builder.Query = SkyUtility.BuildQueryString(config);
            return builder.ToString();
        }

        public override AccessToken GetAccessTokenByAuthorizationCode(string code)
        {
            AccessToken token = null;
            if (!String.IsNullOrEmpty(code))
            {
                WeiboParaeter[] parameter = new WeiboParaeter[]{
                    new WeiboParaeter{ Name="grant_type", Value="authorization_code"},
                    new WeiboParaeter{ Name="client_id", Value= this.appKey},
                    new WeiboParaeter{ Name="client_secret", Value = this.appSecret},
                    new WeiboParaeter{ Name="code", Value= code},
                    new WeiboParaeter{ Name = "redirect_uri", Value= callbackUrl},
                };
                String json = Request(ACCESS_TOKEN_URL, RequestMethod.Get, parameter);
                if (!String.IsNullOrEmpty(json))
                {
                    token = GetValByKey(json);
                    parameter = new WeiboParaeter[]{
                        new WeiboParaeter{ Name="access_token",Value=token.access_token}
                    };
                    json = Request(ACCESS_OPENID, RequestMethod.Get, parameter);
                    if (!String.IsNullOrEmpty(json))
                    {
                        json = json.Replace("callback( ", String.Empty);
                        json = json.Replace(" );", String.Empty);
                        AceessOpenId open = DY.OAuthSDK.Helper.JsonHelper.GetJosnModel<AceessOpenId>(json);
                        token.client_id = open.client_id;
                        token.openid = open.openid;
                    }
                }
            }
            return token;
        }

        /// <summary>
        /// 此方法个人重新写一下
        /// 我这里处理的比较难看
        /// 只是为了满足结果而已
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static AccessToken GetValByKey(String input)
        {
            AccessToken token = new AccessToken();
            String[] strArray = input.Split('&');
            for (int i = 0; i < strArray.Length; i++)
            {
                String[] tempArray = strArray[i].Split('=');
                if (i == 0)
                    token.access_token = tempArray[1];
                else
                    token.expires_in = int.Parse(tempArray[1]);
            }
            return token;
        }
    }
}
