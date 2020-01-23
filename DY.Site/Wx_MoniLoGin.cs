using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

/// <summary>
///LoGin 的摘要说明
/// </summary>
public class Wx_MoniLoGin
{
    public Wx_MoniLoGin()
    {
    }
    static string GetMd5Str32(string str)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        // Convert the input string to a byte array and compute the hash. 
        char[] temp = str.ToCharArray();
        byte[] buf = new byte[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            buf[i] = (byte)temp[i];
        }
        byte[] data = md5Hasher.ComputeHash(buf);
        // Create a new Stringbuilder to collect the bytes 
        // and create a string. 
        StringBuilder sBuilder = new StringBuilder();
        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string. 
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        // Return the hexadecimal string. 
        return sBuilder.ToString();

    }
    //执行登陆操作
    public static bool ExecLogin(string name, string pass)
    {
        bool result = false;
        string password = GetMd5Str32(pass).ToUpper();
        string padata = "username=" + name + "&pwd=" + password + "&imgcode=&f=json";
        string url = "http://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN ";//请求登录的URL
        try
        {
            CookieContainer cc = new CookieContainer();//接收缓存
            byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
            webRequest2.CookieContainer = cc;                                      //保存cookie 
            webRequest2.Method = "POST";                                          //请求方式是POST
            webRequest2.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
            webRequest2.ContentLength = byteArray.Length;
            webRequest2.Referer = "https://mp.weixin.qq.com/";
            Stream newStream = webRequest2.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
            // Send the data.
            newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
            newStream.Close();
            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();
            StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);
            string text2 = sr2.ReadToEnd();

            //此处用到了newtonsoft来序列化
            JavaScriptSerializer js = new JavaScriptSerializer();
            WeiXinRetInfo retinfo = js.Deserialize<WeiXinRetInfo>(text2);
            string token = string.Empty;
            if (retinfo.ErrMsg.Length > 0)
            {
                if (retinfo.ErrMsg.Contains("ok"))
                {
                    token = retinfo.ErrMsg.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令牌
                    LoginInfo.LoginCookie = cc;
                    LoginInfo.CreateDate = DateTime.Now;
                    LoginInfo.Token = token;
                    result = true;
                }
                else
                    result = false;
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.StackTrace);
        }
        return result;
    }

    public static class LoginInfo
    {
        /// <summary>
        /// 登录后得到的令牌
        /// </summary>       
        public static string Token;
        /// <summary>
        /// 登录后得到的cookie
        /// </summary>
        public static CookieContainer LoginCookie;
        /// <summary>
        /// 创建时间
        /// </summary>
        public static DateTime CreateDate;

    }


    //2.在WeiXin.cs类中实现发送数据
    public static bool SendMessage(string Message, string fakeid)
    {
        bool result = false;
        CookieContainer cookie = null;
        string token = null;
        cookie = LoginInfo.LoginCookie;//取得cookie
        token = LoginInfo.Token;//取得token

        string strMsg = System.Web.HttpUtility.UrlEncode(Message);  //对传递过来的信息进行url编码
        string padate = "type=3&content=" + strMsg + "&error=false&tofakeid=" + fakeid + "&token=" + token + "&ajax=1";
        string url = "https://mp.weixin.qq.com/cgi-bin/singlesend?t=ajax-response&lang=zh_CN";

        byte[] byteArray = Encoding.UTF8.GetBytes(padate); // 转化

        HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(url);

        webRequest2.CookieContainer = cookie; //登录时得到的缓存
                              
        webRequest2.Referer = "https://mp.weixin.qq.com/cgi-bin/singlemsgpage?token=" + token + "&fromfakeid=" + fakeid + "&msgid=&source=&count=20&t=wxm-singlechat&lang=zh_CN";

        webRequest2.Method = "POST";

        webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";

        webRequest2.ContentType = "application/x-www-form-urlencoded";

        webRequest2.ContentLength = byteArray.Length;

        Stream newStream = webRequest2.GetRequestStream();

        // Send the data.           
        newStream.Write(byteArray, 0, byteArray.Length);    //写入参数   

        newStream.Close();

        HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();

        StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.Default);

        string text2 = sr2.ReadToEnd();
        if (text2.Contains("ok"))
        {
            result = true;
        }
        return result;
    }








    //3.SendMessage.aspx.cs中主要实现获取fakeid
    public static ArrayList SubscribeMP()
    {

        try
        {
            CookieContainer cookie = null;
            string token = null;


            cookie = LoginInfo.LoginCookie;//取得cookie
            token = LoginInfo.Token;//取得token

            /*获取用户信息的url，这里有几个参数给大家讲一下，
             * 1.token此参数为上面的token 
             * 2.pagesize此参数为每一页显示的记录条数
             * 3.pageid为当前的页数，
             * 4.groupid为微信公众平台的用户分组的组id，当然这也是我的猜想不一定正确*/
            string Url = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&token=" + token + "&lang=zh_CN&pagesize=10&pageidx=0&type=0";
            HttpWebRequest webRequest2 = (HttpWebRequest)WebRequest.Create(Url);
            webRequest2.CookieContainer = cookie;
            webRequest2.ContentType = "text/html; charset=UTF-8";
            webRequest2.Method = "GET";
            webRequest2.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:18.0) Gecko/20100101 Firefox/18.0";
            webRequest2.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse response2 = (HttpWebResponse)webRequest2.GetResponse();


            StreamReader sr2 = new StreamReader(response2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string text2 = sr2.ReadToEnd();
            MatchCollection mc;
            mc = Regex.Matches(text2, "\"id\":\\d{10}");
            Int32 friendSum = mc.Count;  
            string fackID = "";

            ArrayList fackID1 = new ArrayList();

            for (int i = 0; i < friendSum; i++)
            {
                fackID = mc[i].Value.Split(new char[] { ':' })[1];
                fackID = fackID.Replace("\"", "").Trim();
                fackID1.Add(fackID);
            }

            return fackID1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.StackTrace);
        }
    }

}


