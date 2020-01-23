using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
namespace DY.Site
{
    /// <summary>
    /// 腾讯滚动新闻接口
    /// </summary>
   public class RollNewsFromQQ: Expressabstract
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { set; get; }

       /// <summary>
       /// 构造函数
       /// </summary>
        public RollNewsFromQQ()
		{
            Url = "http://roll.news.qq.com/interface/roll.php";
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">参数表</param>
        /// <returns></returns>
        public override string CreatePostHttpResponse(IDictionary<string, string> parameters)
        {
            Encoding encoding = Encoding.GetEncoding("gb2312");
            return ExpressMethod.GetPostHttpResponse(Url, parameters, encoding, "http://roll.news.qq.com/");

        }
    }
}
