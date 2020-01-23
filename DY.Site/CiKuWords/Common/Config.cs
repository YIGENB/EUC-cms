using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace DY.Site
{
    public  class KWConfig
    {
        /// <summary>
        /// 接口调用地址
        /// </summary>
        public const string ApiBaseUrl = "http://api.ciku5.com";

        /// <summary>
        /// 接口appid
        /// </summary>
        public static string AppId = "100909";

        /// <summary>
        /// 接口apptoken
        /// </summary>
        public static string AppKey = "06072a66401b489ba4655c1c9153f415";

        /// <summary>
        /// 接口版本号 0为旧版 1为新版本
        /// </summary>
        public const int Ver = 1;


    }
}
