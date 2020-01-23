using System;

namespace DY.OAuthSDK.Connect.Model
{
    public class AccessToken
    {
        public String access_token { get; set; }
        public Int32 expires_in { get; set; }
        public String uid { get; set; }
        public Int32 remind_in { get; set; }
        public String openid { get; set; }
        public Int32 client_id { get; set; }
    }

    public class AceessOpenId
    {
        public String openid { get; set; }
        public Int32 client_id { get; set; }
    }
}
