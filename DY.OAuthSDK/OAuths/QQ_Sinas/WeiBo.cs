using DY.OAuthSDK.Connect.Model;
using DY.OAuthSDK.Connect.Util;
using System;
using System.Collections.Generic;
using System.Text;
using DY.OAuthSDK.Helper;

namespace DY.OAuthSDK.Login
{
    public class WeiBo : Connect
    {
        private const string AUTHORIZE_URL = "https://api.weibo.com/oauth2/authorize";
        private const string ACCESS_TOKEN_URL = "https://api.weibo.com/oauth2/access_token";

        public WeiBo(QQSite site) : base(site) { }

        public override string GetAuthorizeURL(ResponseType response = ResponseType.Code, string state = null, DisplayType display = DisplayType.Default)
        {
            Dictionary<string, string> config = new Dictionary<string, string>()
			{
				{"client_id",appKey},
				{"redirect_uri",callbackUrl},
				{"response_type",response.ToString().ToLower()},
				{"state",state??string.Empty},
				{"display",display.ToString().ToLower()},
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
                WeiboParaeter[] weiboPar = new WeiboParaeter[]{
                    new WeiboParaeter{ Name="client_id", Value=appKey },
                    new WeiboParaeter{ Name = "client_secret", Value = appSecret},
                    new WeiboParaeter{ Name = "grant_type", Value= "authorization_code"},
                    new WeiboParaeter{ Name="code", Value = code},
                    new WeiboParaeter{ Name="redirect_uri",Value = callbackUrl},
                };
                String accessToken = Request(ACCESS_TOKEN_URL, RequestMethod.Post, weiboPar);
                if (!String.IsNullOrEmpty(accessToken))
                {
                    token = JsonHelper.GetJosnModel<AccessToken>(accessToken);
                }
            }
            return token;
        }
    }
}
