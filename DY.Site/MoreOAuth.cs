using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using DY.OAuthV2SDK.OAuths;
using DY.OAuthV2SDK;
using DY.OAuthV2SDK.Entitys;

namespace DY.OAuthV2Demo.Controllers
{
    /// <summary>
    /// 多个平台OAuth协议
    /// </summary>
    public class MoreOAuth
    {
        /// <summary>
        /// 多个平台发送微博
        /// </summary>
        /// <param name="text">微博内容</param>
        /// <returns></returns>
        public static string SendStatus(string text)
        {
            var msg = new StringBuilder();
            foreach (var item in DY.OAuthV2SDK.OAuthConfig.GetConfigOAuths())
            {
                try
                {
                    var oauth = OAuthBase.CreateInstance(item.name);
                    AppEntity entity = oauth.App;
                    #region 随机读取@
                    int k = 0;
                    if (item.isAt == "1")
                    {
                        Random r = new Random();
                        System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
                        string[] at = DY.Common.Utils.SplitString(entity.RandomAT, "\n");
                        for (int i = 0; i < at.Length; i++)
                        {
                            int rnum = r.Next(0, at.Length);
                            if (!hashtable.ContainsValue(at[rnum]))
                            {
                                hashtable.Add(at[rnum], at[rnum]);
                            }
                        }
                        foreach (System.Collections.DictionaryEntry de in hashtable)//h为Hashtable 
                        {
                            if (k < 5)
                                text += " @" + de.Value + " ";
                            k++;
                        }
                    }
                    #endregion
                    //模拟数据库获取accessToken
                    var token = entity.Access_token;
                    var uid = entity.Uid;//HttpContext.Current.Session[item.name + "_uid"];
                    if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(uid))
                    {
                        string accessToken = token.ToString(); //从数据库读取
                        oauth.Uid = uid.ToString();//腾讯微博必需参数
                        var result = oauth.SendStatus(accessToken, text);
                        if (result.ret != 0)
                        {
                            msg.AppendFormat("{0}发生错误：{1} <br>", item.name, result.msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg.AppendFormat("{0}发生异常：{1} <br>", item.name, ex.Message);
                }
            }
            return msg.ToString();
        }
        /// <summary>
        /// 多个平台发送图片微博
        /// </summary>
        /// <param name="text">微博内容</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static string SendStatusWithPic(string text, string filename)
        {
            var msg = new StringBuilder();
            foreach (var item in DY.OAuthV2SDK.OAuthConfig.GetConfigOAuths())
            {
                try
                {
                    var oauth = OAuthBase.CreateInstance(item.name);
                    AppEntity entity = OAuthConfig.GetConfigApp(item.name, "");
                    #region 随机读取@
                    int k = 0;
                    if (item.isAt == "1")
                    {
                        Random r = new Random();
                        System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
                        string[] at = DY.Common.Utils.SplitString(entity.RandomAT, "\n");
                        for (int i = 0; i < at.Length; i++)
                        {
                            int rnum= r.Next(0,at.Length);
                            if (!hashtable.ContainsValue(at[rnum]))
                            {
                                hashtable.Add(at[rnum], at[rnum]);
                            }
                        }
                        foreach (System.Collections.DictionaryEntry de in hashtable)//h为Hashtable 
                        {
                            if (k < 5)
                                text += " @" + de.Value + " ";
                            k++;
                        }
                    }
                    #endregion
                    //模拟数据库获取accessToken
                    var token = entity.Access_token;
                    var uid = entity.Uid;//HttpContext.Current.Session[item.name + "_uid"];
                    if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(uid))
                    {
                        string accessToken = token.ToString(); //从数据库读取
                        oauth.Uid = uid.ToString();//腾讯微博必需参数
                        var result = oauth.SendStatusWithPic(accessToken, text, filename);
                        if (result.ret != 0)
                        {
                            msg.AppendFormat("{0}发生错误：{1} <br>", item.name, result.msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg.AppendFormat("{0}发生异常：{1} <br>", item.name, ex.Message);
                }
            }
            return msg.ToString();
        }


        /// <summary>
        /// 多个平台发送图片微博(针对文章类型)
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <param name="des">简介描述</param>
        /// <param name="content">内容详情</param>
        /// <returns></returns>
        public static string SendStatusToArticle(string title, string des, string content)
        {
            var msg = new StringBuilder();
            foreach (var item in DY.OAuthV2SDK.OAuthConfig.GetConfigOAuths())
            {
                try
                {
                    if (item.isAt != "1")
                    {
                        var oauth = OAuthBase.CreateInstance(item.name);
                        AppEntity entity = OAuthConfig.GetConfigApp(item.name, "");
                        //模拟数据库获取accessToken
                        var token = entity.Access_token;
                        var uid = entity.Uid;//HttpContext.Current.Session[item.name + "_uid"];
                        if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(uid))
                        {
                            string accessToken = token.ToString(); //从数据库读取
                            oauth.Uid = uid.ToString();
                            var result = oauth.SendStatusToArticle(accessToken, title, des,content);
                            if (result.ret != 0)
                            {
                                msg.AppendFormat("{0}发生错误：{1} <br>", item.name, result.msg);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg.AppendFormat("{0}发生异常：{1} <br>", item.name, ex.Message);
                }

            }
            return msg.ToString();
        }
    }
}
