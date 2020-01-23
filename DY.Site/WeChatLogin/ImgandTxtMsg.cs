using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DY.Site
{
	class ImgandTxtMsg
	{
        public static JArray getImgandTxt()
        {

            CookieContainer cookie = null;
            string token = null;
            cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
            token = WeiXinLogin.LoginInfo.Token;//取得token

            /* 1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数

            3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id*/
            string Url = "https://mp.weixin.qq.com/cgi-bin/appmsg?begin=0&count=1000&t=media/appmsg_list&type=10&action=list&token="+token+"&lang=zh_CN";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);//Url为获取用户信息的链接
            webRequest.CookieContainer = cookie;
            webRequest.ContentType = "text/html; charset=UTF-8";
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string text = sr.ReadToEnd();
            MatchCollection mc;
            Regex Rex = new Regex(@"(?<=\{""item"":).+(?=,""file_cnt"":)");
            mc = Rex.Matches(text);
           JArray ImgandTxt = new JArray();
            if (mc.Count != 0)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    ImgandTxt=(JArray)JsonConvert.DeserializeObject(mc[i].Value);
                }
            }

            return ImgandTxt;
        
        }


        public static class dataofImgandTxt
        {
            public static JArray ImgandTxtData;
           
        
        }
	}
}
