using System;

namespace DY.OAuthSDK.Login.Model
{
    public class UserInfo
    {
        public String id { get; set; }

        public String screen_name { get; set; }

        public String profile_image_url { get; set; }
        /// <summary>
        /// 所在地区
        /// </summary>
        public String location { get; set; }

        public String gender { get; set; }

        public Byte uType { get; set; }
    }

    public class TentenctInfo
    {
        public Int32 ret { get; set; }

        public String nickname { get; set; }

        public String gender { get; set; }

        public String figureurl_qq_1 { get; set; }
    }
}
