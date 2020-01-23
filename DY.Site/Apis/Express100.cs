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
    /// 快递100
    /// </summary>
    public class Express100 : Expressabstract
    {
        /// <summary>
        /// 快递接口地址
        /// </summary>
        public string Url { set; get; }

        public Express100()
		{
            Url = "http://api.kuaidi100.com/api";
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
