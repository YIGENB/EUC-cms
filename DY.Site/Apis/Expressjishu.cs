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
    /// 百度快递急速接口
    /// </summary>
   public class Expressjishu: Expressabstract
    {
        /// <summary>
        /// 快递接口地址
        /// </summary>
        public string Url { set; get; }

       /// <summary>
       /// 构造函数
       /// </summary>
        public Expressjishu()
		{
            Url = "http://apis.baidu.com/netpopo/express/express1";
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">参数表</param>
        /// <returns></returns>
        public override string CreatePostHttpResponse(IDictionary<string, string> parameters)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            return ExpressMethod.GetPostHttpResponse(Url, parameters, encoding);

        }
    }
}
