using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
///WeiXin 的摘要说明
/// </summary>
/// 
namespace DY.Site
{
    public class WeiXin
    {
         
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
        public static bool SendMessage(string Message, string fakeid, int flag)//发送消息
        {
            bool result = false;
            CookieContainer cookie = null;
            string token = null;
            cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
            token = WeiXinLogin.LoginInfo.Token;//取得token
            string strMsg = "";
            string padate = "";
            if (flag == 0)//发送文字
            {
                strMsg = Message;
                padate = "type=1&content=" + strMsg + "&tofakeid=" + fakeid + "&imgcode=&token=" + token + "&lang=zh_CN&random=0.4486911059357226&t=ajax-response";

            }
            if (flag == 1)//发送图文
            {

                padate = "type=10&app_id=" + Message + "&tofakeid=" + fakeid + "&appmsgid=" + Message + "&imgcode=&token=" + token + "&lang=zh_CN&random=0.22518408996984363&f=json&ajax=1&t=ajax-response";

            }
           

            string url = "https://mp.weixin.qq.com/cgi-bin/singlesend";

            byte[] byteArray = Encoding.UTF8.GetBytes(padate); // 转化

            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);

            webRequest2.CookieContainer = cookie; //登录时得到的缓存

            //webRequest2.Referer = "https://mp.weixin.qq.com/cgi-bin/singlemsgpage?fromfakeid=" + fakeid + "&count=20&t=wxm-singlechat&token=" + token + "&token=" + token + "&lang=zh_CN";
            webRequest2.Referer = "https://mp.weixin.qq.com/cgi-bin/singlesendpage?t=message/send&action=index&tofakeid=" + fakeid + "&token=" + token + "&lang=zh_CN";
            webRequest2.Method = "POST";

            webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";

            webRequest2.ContentType = "application/x-www-form-urlencoded";

            webRequest2.ContentLength = byteArray.Length;

            Stream newStream = webRequest2.GetRequestStream();

            // Send the data.            
            newStream.Write(byteArray, 0, byteArray.Length);    //写入参数    

            newStream.Close();

            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

            StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

            string text2 = sr2.ReadToEnd();
            if (text2.Contains("ok"))
            {
                result = true;
            }
            return result;
        }
        public static List<SingleGroup> getAllGroupInfo()//获取所有分组数据存储在List里
        {

            try
            {
                CookieContainer cookie = null;
                string token = null;
                cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
                token = WeiXinLogin.LoginInfo.Token;//取得token

                /* 1.token此参数为上面的token 2.pagesize此参数为每一页显示的记录条数

                3.pageid为当前的页数，4.groupid为微信公众平台的用户分组的组id*/
                string Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=10&pageidx=0&type=0&groupid=0&token=" + token + "&lang=zh_CN";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);//Url为获取用户信息的链接
                webRequest.CookieContainer = cookie;
                webRequest.ContentType = "text/html; charset=UTF-8";
                webRequest.Method = "GET";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string text = sr.ReadToEnd();
                MatchCollection mcGroup;
                Regex GroupRex = new Regex(@"(?<=""groups"":).*(?=\}\).groups)");
                mcGroup = GroupRex.Matches(text);
                List<SingleGroup> allgroupinfo = new List<SingleGroup>();
                if (mcGroup.Count != 0)
                {
                    JArray groupjarray = (JArray)JsonConvert.DeserializeObject(mcGroup[0].Value);

                    for (int i = 0; i < groupjarray.Count; i++)
                    {

                        getEachGroupInfo(groupjarray[i]["id"].ToString(), groupjarray[i]["cnt"].ToString(), groupjarray[i]["name"].ToString(), ref allgroupinfo);

                    }
                }
                return allgroupinfo;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }

        }
        public static void getEachGroupInfo(string groupid, string count, string group_name, ref List<SingleGroup> groupdata)//获取单个分组数据
        {


            CookieContainer cookie = null;
            string token = null;
            cookie = WeiXinLogin.LoginInfo.LoginCookie;//取得cookie
            token = WeiXinLogin.LoginInfo.Token;//取得token    
            SingleGroup obj_single = new SingleGroup();
            obj_single.group_name = group_name;
            string TotalUser;
            if (count != "0")
            {
                TotalUser = count;
            }
            else
            {

                return;

            }

            string Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=" + TotalUser + "&pageidx=0&type=0&groupid=" + groupid.Trim() + "&token=" + token + "&lang=zh_CN";
            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(Url);
            webRequest2.CookieContainer = cookie;
            webRequest2.ContentType = "text/html; charset=UTF-8";
            webRequest2.Method = "GET";
            webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
            webRequest2.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
            StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string text2 = sr2.ReadToEnd();
            MatchCollection mcJsonData;
            Regex rexJsonData = new Regex(@"(?<=friendsList : \({""contacts"":).*(?=}\).contacts)");
            mcJsonData = rexJsonData.Matches(text2);
            if (mcJsonData.Count != 0)
            {
                JArray JsonArray = (JArray)JsonConvert.DeserializeObject(mcJsonData[0].Value);

                obj_single.groupdata = JsonArray;
                groupdata.Add(obj_single);

            }


        }
    }
    public class SingleGroup//存储一个分组的信息的类
    {
        public string group_name;

        public JArray groupdata;

    }
    public class LoginUser
    {

        private string uid;

        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private string pwd;

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        public LoginUser()
        {
          Uid = null;
          Pwd = null;
        
        }
    
    }
}