using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

using DY.Common;
using DY.Config;
using DY.Entity;
using System.Net.Mail;
using DY.Cache;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.Helpers;
using CookComputing.XmlRpc;
using System.Data;
using DY.LanguagePack;

namespace DY.Site
{
    /// <summary>
    /// 网站工具类
    /// </summary>
    public class SiteUtils
    {
        /// <summary>
        /// 网站基本信息
        /// </summary>
        protected internal BaseConfigInfo config;
        /// <summary>
        /// SiteUtils构造函数
        /// </summary>
        public SiteUtils()
        {
            config = BaseConfig.Get();
        }

        #region 静态函数
        /// <summary>
        /// 写后台登录用户的cookie
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="passwordkey">用户密码Key</param>
        public static void WriteAdminLoginCookie(AdminUserInfo userinfo, int expires, string passwordkey)
        {
            if (userinfo == null)
                return;

            HttpCookie cookie = new HttpCookie("DYAdmin");
            cookie.Values["userid"] = userinfo.user_id.ToString();
            cookie.Values["password"] = Utils.UrlEncode(DES.Encode(userinfo.password, passwordkey));
            cookie.Values["expires"] = expires.ToString();
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddDays(expires);
            }
            string cookieDomain = BaseConfig.CookieDomain;
            if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
            {
                cookie.Domain = cookieDomain;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 写前台登录用户的cookie
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <param name="expires">cookie有效期</param>
        /// <param name="passwordkey">用户密码Key</param>
        public static void WriteUserLoginCookie(UsersInfo userinfo, int expires, string passwordkey)
        {
            if (userinfo == null)
                return;

            HttpCookie cookie = new HttpCookie("DYUser");
            cookie.Values["userid"] = userinfo.user_id.ToString();
            cookie.Values["password"] = Utils.UrlEncode(DES.Encode(userinfo.password, passwordkey));
            cookie.Values["expires"] = expires.ToString();
            if (expires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            string cookieDomain = BaseConfig.CookieDomain;
            if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
            {
                cookie.Domain = cookieDomain;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 获得网站cookie值
        /// </summary>
        /// <param name="strName">项</param>
        /// <returns>值</returns>
        public static string GetCookie(string strName)
        {
            return GetCookie(strName, "DYAdmin");
        }
        /// <summary>
        /// 获得网站cookie值
        /// </summary>
        /// <param name="strName">项</param>
        /// <returns>值</returns>
        public static string GetCookie(string strName, string domain)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[domain] != null && HttpContext.Current.Request.Cookies[domain][strName] != null)
                return Utils.UrlDecode(HttpContext.Current.Request.Cookies[domain][strName].ToString());

            return "";
        }
        /// <summary>
        /// 清除论坛登录用户的cookie
        /// </summary>
        public static void ClearUserCookie()
        {
            ClearUserCookie("DYAdmin");
        }

        public static void ClearUserCookie(string cookieName)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddYears(-1);
            string cookieDomain = BaseConfig.CookieDomain;
            if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
                cookie.Domain = cookieDomain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 是否为有效域
        /// </summary>
        /// <param name="host">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string host)
        {
            if (host.IndexOf(".") == -1)
                return false;

            return new Regex(@"^\d+$").IsMatch(host.Replace(".", string.Empty)) ? false : true;
        }
        /// <summary>
        /// 强效加密（先用密钥加密再用MD5加密）
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string Encryption(string str)
        {
            return Utils.MD5(DES.Encode(str, BaseConfig.WebEncrypt));
        }
        /// <summary>
        /// 返回网站用户密码cookie明文
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string GetCookiePassword(string key)
        {
            return DES.Decode(GetCookie("password"), key).Trim();
        }
        /// <summary>
        /// 返回网站用户密码cookie明文
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string GetCookieUserPassword(string key)
        {
            return DES.Decode(GetCookie("password", "DYUser"), key).Trim();
        }
        /// <summary>
        /// 返回网站用户密码cookie明文
        /// </summary>
        /// <param name="password">密码密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string GetCookiePassword(string password, string key)
        {
            return DES.Decode(password, key);
        }
        /// <summary>
        /// 获取排序
        /// </summary>
        /// <param name="defaultOrder"></param>
        /// <returns></returns>
        public static string GetSortOrder(string defaultOrder)
        {
            string order = defaultOrder;
            if (!string.IsNullOrEmpty(DYRequest.getRequest("sort_by")) && !string.IsNullOrEmpty(DYRequest.getRequest("sort_order")))
            {
                order = DYRequest.getRequest("sort_by") + " " + DYRequest.getRequest("sort_order");
            }

            return order;
        }
        /// <summary>
        /// 获取排序
        /// </summary>
        /// <param name="defaultOrder"></param>
        /// <returns></returns>
        public static string GetGoodsSortOrder(IDictionary context, string defaultOrder)
        {
            string order = defaultOrder, sort_by = DYRequest.getRequest("sort_by", "time"), sort_order = DYRequest.getRequest("sort_order", "DESC")
                , s_by = "", s_order = "";

            if (sort_by == "price")
                s_by = "shop_price";
            else if (sort_by == "time")
                s_by = "add_time";

            if (sort_order != "DESC" || sort_order != "ASC")
                s_order = "DESC";

            if (!string.IsNullOrEmpty(s_by) && !string.IsNullOrEmpty(s_order))
                order = s_by + " " + s_order;

            context.Add("sort_by", sort_by);
            context.Add("sort_order", sort_order);

            return order;
        }
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        public static string GetFilter(IDictionary context)
        {
            context.Clear();

            string
                filter = "1=1",
                field = DYRequest.getRequest("field"),
                target = DYRequest.getRequest("target"),
                val = DYRequest.getRequest("val"),
                checkboxfield = DYRequest.getRequest("checkboxfield"),
                checkboxfieldvals = DYRequest.getRequest("checkboxfieldvals");

            if (!string.IsNullOrEmpty(val))
            {
                if (target == "istext")
                    filter += " and " + field + "='" + val + "'";
                if (target == "isnum" && FunctionUtils.Text.IsDouble(val) == true)
                    filter += " and " + field + "=" + val + "";
                if (target == "like")
                    filter += " and " + field + " like '%" + val + "%'";
                if (target == "gt" && FunctionUtils.Text.IsDouble(val) == true)
                    filter += " and " + field + ">=" + val + "";
                if (target == "lt" && FunctionUtils.Text.IsDouble(val) == true)
                    filter += " and " + field + "<=" + val + "";
            }

            if (!string.IsNullOrEmpty(checkboxfield))
            {
                for (int i = 0; i < checkboxfield.Split(',').Length - 1; i++)
                {
                    filter += " and " + checkboxfield.Split(',')[i] + "=" + checkboxfieldvals.Split(',')[i];
                }
            }

            context.Add("field", field);
            context.Add("target", target);
            context.Add("val", val);
            context.Add("checkboxfield", checkboxfield);
            context.Add("checkboxfieldvals", checkboxfieldvals);

            return filter;
        }
        /// <summary>
        /// 创建一个guid
        /// </summary>
        /// <returns></returns>
        public static string CreatGUID()
        {
            return Guid.NewGuid().ToString("D");
        }
        /// <summary>
        /// 创建一个商品货号
        /// </summary>
        /// <returns></returns>
        public static string CreatGoodsSn()
        {
            Random rd = new Random();

            return rd.Next(99999999).ToString();
        }
        /// <summary>
        /// 获取后台分页大小
        /// </summary>
        /// <returns></returns>
        public static int GetPageSize()
        {
            if (DYRequest.getRequestInt("page_size", 0) > 0)
                Utils.WriteCookie("adminPageSize", DYRequest.getRequest("page_size"), 30);

            return Utils.StrToInt(Utils.GetCookie("adminPageSize"), 0) > 0 ? Convert.ToInt16(Utils.GetCookie("adminPageSize")) : 14;
        }
        /// <summary>
        /// 发送邮件 yj(2009-12-28)
        /// </summary>
        /// <param name="to">要发送的邮件列表</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        public static bool SendMailUseGmail(string to, string subject, string body)
        {
            BaseConfigInfo config = DY.Config.BaseConfig.Get();

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(to);

            msg.From = new MailAddress(config.SmtpUser, config.Name, System.Text.Encoding.UTF8);
            /**/
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = subject;//邮件标题 
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码 
            msg.Body = body;//邮件内容 
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码 
            msg.IsBodyHtml = true;//是否是HTML邮件 
            msg.Priority = MailPriority.Normal;//邮件优先级 

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(config.SmtpUser, config.SmtpPass);
            //上述写你的GMail邮箱和密码 
            client.Port = Utils.StrToInt(config.SmtpPort, 25);//Gmail使用的端口 
            client.Host = config.SmtpHost;
            client.EnableSsl = true;//经过ssl加密 
            object userState = msg;
            try
            {
                client.Send(msg);
                //client.SendAsync(msg, userState);
                //简单一点儿可以client.Send(msg); 
                //MessageBox.Show("发送成功");
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 自访式生成静态文件
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="targetPath"></param>
        public static void MakeHtml(string sourceUrl, string targetPath)
        {
            string html = CommonUtils.LoadURLString(sourceUrl);

            if (!string.IsNullOrEmpty(html))
                //写入html文件
                FileOperate.WriteFile(targetPath, html);
        }


        #region 腾讯滚动新闻接口
        /// <summary>
        /// 腾讯滚动新闻接口(http://roll.news.qq.com/interface/roll.php?site=news&date=&page=1&mode=1&of=xml)
        /// </summary>
        /// <param name="site">站点名声</param>
        /// <param name="date">新闻时间</param>
        /// <param name="page">页面</param>
        /// <returns></returns>
        public static Rollroot RollNewsFromQQ(string site,string date,string page)
        {
            ExpressControl control = new ExpressControl();
            string data = "";
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("site", site);
            parameters.Add("date", date);
            parameters.Add("page", page);
            parameters.Add("mode", "1");
            parameters.Add("of", "xml");
            data = control.Get(ExpressControl.ExpressType.RollNewsFromQQ).CreatePostHttpResponse(parameters);
            return RollNewsXMLHelper.FromXml<Rollroot>(data);
        }

        /// <summary>
        /// 替换成统一网址，如：http://xw.qq.com/news/20161011034332
        /// </summary>
        /// <param name="str">腾讯滚动新闻PC网址</param>
        /// <returns></returns>
        public static string IgetNumber(string str, string url = "http://xw.qq.com/news/")
        {
            return url + System.Text.RegularExpressions.Regex.Replace(str, @"[^\d{2}-]*", "");
        }
        #endregion


        #region 采集工具类
        /// <summary>
        /// 根据规则获取内容
        /// </summary>
        /// <param name="sourceUrl">url</param>
        /// <param name="match">正则</param>
        /// <returns></returns>
        public static string GetMatch(string sourceUrl, string match,string caches_type)
        {
            StringBuilder str = new StringBuilder();

            #region 缓存页面
            DYCache cache = DYCache.GetCacheService();
            string html = cache.RetrieveObject("/DY/Web/page/" + caches_type) as string;
            if (string.IsNullOrEmpty(html))
            {
                html = CommonUtils.LoadURLString(sourceUrl);
                cache.AddObject("/DY/Web/page/" + caches_type, html);
            }
            #endregion

            Match TitleMatch = Regex.Match(html.ToLower(), match, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //while (TitleMatch.Success)
            //{
            //    Group g = TitleMatch.Groups[1];
            //    if (g != null)
            //        str.Append(g).Append(",");
            //    TitleMatch = TitleMatch.NextMatch();
            //}
            //取出匹配项的值
            return TitleMatch.Groups[1].Value;
            //return str.ToString();
        }

        /// <summary>
        /// 根据规则获取内容组合
        /// </summary>
        /// <param name="sourceUrl">url</param>
        /// <param name="match">正则</param>
        /// <returns></returns>
        public static string GetWordMatch(string sourceUrl, string match,int count)
        {
            StringBuilder str = new StringBuilder();

            string html = CommonUtils.LoadURLString(sourceUrl);

            Match TitleMatch = Regex.Match(html.ToLower(), match, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            int i = 0;
            while (TitleMatch.Success)
            {
                if (i >= count)
                {
                    break;
                }
                else
                {
                    Group g = TitleMatch.Groups[1];
                    if (g != null)
                        str.Append(g).Append(",");
                    TitleMatch = TitleMatch.NextMatch();
                }
                i++;
            }
            return str.ToString();
        }


        /// <summary>
        /// 获取标签之间数据
        /// </summary>
        /// <param name="sourceUrl">url</param>
        /// <param name="startHtml">开始html标签</param>
        /// <param name="endHtml">结束html标签</param>
        /// <returns></returns>
        public static string SubHtmlContent(string sourceUrl, string startHtml, string endHtml)
        {
            try
            {
                string html = CommonUtils.LoadURLString(sourceUrl);
                //string s = html.Substring((html.IndexOf(startHtml) + startHtml.Length), (html.IndexOf(endHtml) - (html.IndexOf(startHtml) + startHtml.Length)));
                return html.Substring((html.IndexOf(startHtml) + startHtml.Length), (html.IndexOf(endHtml) - (html.IndexOf(startHtml) + startHtml.Length)));
            }
            catch
            {
                return "";
            }
        }

        #endregion

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public static string MakeOrderSn()
        {
            Random rnd = new Random();

            return DateTime.Now.ToString("yyyyMMdd") + rnd.Next(10000, 99999);
        }

        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <returns></returns>
        public static OrderInfoInfo GetOrderInfo(int order_id)
        {
            return SiteBLL.GetOrderInfoInfo(order_id);
        }

        #endregion

        #region 非静态函数
        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public string[] Split(object strContent)
        {
            return Utils.SplitString(strContent.ToString(), "\r\n");
        }
        /// <summary>
        /// 拆分字符串
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public string[] Split(object strContent, object strSplit)
        {
            return Utils.SplitString(strContent.ToString(), strSplit.ToString());
        }
        /// <summary>
        /// 返回拆分后的字符串
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string SplitReturn(object strContent, int index)
        {
            if (strContent == null)
                return "";

            if (string.IsNullOrEmpty(strContent.ToString()))
                return "";

            return strContent.ToString().Split(',')[index];
        }
        /// <summary>
        /// 指定值是否存在
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool IsContains(string strContent, object targetString)
        {
            return strContent.Contains(targetString.ToString());
        }
        /// <summary>
        /// 赋默认值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public string DefaultValue(object obj, object val)
        {
            if (obj == null)
                return val.ToString();
            if (string.IsNullOrEmpty(obj.ToString()))
                return val.ToString();
            return obj.ToString();
        }
        /// <summary>
        /// 赋默认值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int DefaultValue(object obj, object val, int flag)
        {
            if (obj == null)
                return int.Parse(val.ToString());

            return int.Parse(obj.ToString());
        }
        /// <summary>
        /// 生成指定数量的空格
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string Spaces(int count)
        {
            return Utils.Spaces(count);
        }
        /// <summary>
        /// 获取排序图标
        /// </summary>
        /// <param name="sort_by"></param>
        /// <param name="sort_order"></param>
        /// <returns></returns>
        public string GetSortImg(string sort_by, string name)
        {
            string str = "<span onclick=\"javascript:listTable.sort('" + sort_by + "'); \" style='cursor: pointer;'>" + name + "</span>";

            if (sort_by == DYRequest.getRequest("sort_by") && sort_by != "")
            {
                str += " <img src=images/sort_" + DYRequest.getRequest("sort_order") + ".gif />";
            }

            return str;
        }
        /// <summary>
        /// 获取指定字段值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        public object GetValue(System.Data.DataRow dr, System.Data.DataColumn dc)
        {
            return dr[dc];
        }
        /// <summary>
        /// 获取指定字段int值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        public int GetIntValue(System.Data.DataRow dr, System.Data.DataColumn dc)
        {
            return Utils.StrToInt(GetValue(dr, dc), 0);
        }
        /// <summary>
        /// 删除某两个字符之外的字符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="le"></param>
        /// <param name="ri"></param>
        /// <returns></returns>
        public string Trim(string content, string le, string ri)
        {
            if (string.IsNullOrEmpty(content))
                return "";

            string c = content.Remove(0, content.IndexOf(le) + 1);

            return c.Remove(c.IndexOf(ri));
        }
        /// <summary>
        /// 删除括号外的字符，包括括号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string TrimRowValue(string str)
        {
            return Trim(str, "(", ")");
        }
        /// <summary>
        /// 删除某两个字符之内的字符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="le"></param>
        /// <param name="ri"></param>
        /// <returns></returns>
        public string Remove(string content, string le, string ri)
        {
            if (string.IsNullOrEmpty(content))
                return "";

            string c = content.Remove(content.IndexOf(le));

            return c;
        }
        /// <summary>
        /// 删除某两个字符之内的字符，包括括号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Remove(string str)
        {
            return Remove(str, "(", ")");
        }
        /// <summary>
        /// 格式化textarea字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string StrFormat(string str)
        {
            return Utils.StrFormat(str);
        }

        /// <summary>
        /// 获取当前网站域名
        /// </summary>
        /// <returns></returns>
        public string GetDomain()
        {
            string host = HttpContext.Current.Request.Url.Host;

            if (host.ToLower().IndexOf(".") >= 0)
                return host;

            return HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
        }
        /// <summary>
        /// 获取当前域名的二级域名主机头
        /// </summary>
        /// <returns></returns>
        public string GetDomainHost()
        {
            string url = GetDomain();
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            var uri = new Uri(url);
            string rootDomain;
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    {
                        if (uri.IsLoopback)
                        {
                            rootDomain = uri.Host;
                        }
                        else
                        {
                            string host = uri.Host;
                            var hosts = host.Split('.');
                            rootDomain = hosts[0];
                        }
                    }
                    break;
                default:
                    rootDomain = uri.Host;
                    break;
            }
            return rootDomain;
        }
        /// <summary>
        /// 获取当前网站整体url
        /// </summary>
        /// <returns></returns>
        public string GetURL()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            return url;
        }
        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string UrlEncode(object val)
        {
            return HttpContext.Current.Server.UrlEncode(val.ToString());
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string GetTopic(string str, int len)
        {
            return Utils.get_topic(str, len);
        }


        #region 返回SEO设置
        /// <summary>
        /// 返回页面标题
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetPageTitle(string title)
        {
            return string.IsNullOrEmpty(title) ? config.Title : title;
        }
        /// <summary>
        /// 返回页面关键字
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetPageKeyowrd(string keyword)
        {
            return string.IsNullOrEmpty(keyword) ? config.Keywords : keyword;
        }
        /// <summary>
        /// 返回页面描述
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetPageDesc(string desc)
        {
            return string.IsNullOrEmpty(desc) ? config.Desc : desc;
        }
        /// <summary>
        /// 返回产品页面标题
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetProPageTitle(string title)
        {
            return string.IsNullOrEmpty(title) ? string.IsNullOrEmpty(config.ProTitle) ? config.Title : config.ProTitle : title + " - " + config.Title;
        }
        /// <summary>
        /// 返回产品页面标题
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetProPageTitle(string title, string defaultValue)
        {
            return string.IsNullOrEmpty(title) ? defaultValue + " - " + config.Title : title;
        }
        /// <summary>
        /// 返回产品页面关键字
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetProPageKeyowrd(string keyword)
        {
            return string.IsNullOrEmpty(keyword) ? string.IsNullOrEmpty(config.ProKeywords) ? config.Keywords : config.ProKeywords : keyword;
        }
        /// <summary>
        /// 返回产品页面描述
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetProPageDesc(string desc)
        {
            return string.IsNullOrEmpty(desc) ? string.IsNullOrEmpty(config.ProDesc) ? config.Desc : config.ProDesc : desc;
        }
        #endregion
        /// <summary>
        /// 支付方式是否安装
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsInstallPayment(string code)
        {
            return SiteBLL.ExistsPayment("pay_code='" + code + "'");
        }
        /// <summary>
        /// 生成指定数量的空格
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string MakeSpace(int num)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < num; i++)
            {
                sb.Append("&nbsp; &nbsp; ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// ip的后段隐藏
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string IpHidden(string ip)
        {

            if (ip.IndexOf('.') >= 0)
            {
                string[] strIp = ip.Split('.');
                string revalue = strIp[0].ToString() + "." + strIp[1].ToString() + "." + "*.*";
                return revalue;
            }
            else
            {
                return "";
            }

        }


        /// <summary>
        /// 求余
        /// </summary>
        /// <param name="num"></param>
        /// <param name="numa"></param>
        /// <returns></returns>
        public int GetMod(int num, int numa)
        {
            if (num == 0)
            {
                return 1;
            }
            else
            {
                int revalue = num % numa;
                return revalue;
            }
        }

        /// <summary>
        /// 比较两个时间
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public int CompareTime(DateTime StartTime, DateTime EndTime)
        {
            return DateTime.Compare(StartTime, EndTime);
        }

        /// <summary>
        /// 比较两个时间
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public int CompareTime(DateTime StartTime)
        {
            DateTime EndTime = DateTime.Now;
            return DateTime.Compare(StartTime, EndTime);
        }

        ///   <summary>   
        ///   返回两个日期之间的时间间隔（y：年份间隔、M：月份间隔、d：天数间隔、h：小时间隔、m：分钟间隔、s：秒钟间隔、ms：微秒间隔）   
        ///   </summary>   
        ///   <param   name="Date1">开始日期</param>   
        ///   <param   name="Interval">间隔标志</param>   
        ///   <returns>返回间隔标志指定的时间间隔</returns>   
        public int GetDateDiff(DateTime Date1, string Interval)
        {
            double dblYearLen = 365;//年的长度，365天   
            double dblMonthLen = (365 / 12);//每个月平均的天数   
            System.TimeSpan objT;
            objT = DateTime.Now.Subtract(Date1);
            switch (Interval)
            {
                case "y"://返回日期的年份间隔   
                    return System.Convert.ToInt32(objT.Days / dblYearLen);
                case "M"://返回日期的月份间隔   
                    return System.Convert.ToInt32(objT.Days / dblMonthLen);
                case "d"://返回日期的天数间隔   
                    return objT.Days;
                case "h"://返回日期的小时间隔   
                    return objT.Hours;
                case "m"://返回日期的分钟间隔   
                    return objT.Minutes;
                case "s"://返回日期的秒钟间隔   
                    return objT.Seconds;
                case "ms"://返回时间的微秒间隔   
                    return objT.Milliseconds;
                default:
                    break;
            }
            return 0;
        }

        #endregion

        /// <summary>
        /// 从数据库里取得的数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SqlHtml(String str)
        {
            while (str.IndexOf("\n") != -1)
            {
                str = str.Substring(0, str.IndexOf("\n")) + "<br>" + str.Substring(str.IndexOf("\n") + 1);
            }
            while (str.IndexOf(" ") != -1)
            {
                str = str.Substring(0, str.IndexOf(" ")) + "&nbsp;" + str.Substring(str.IndexOf(" ") + 1);
            }
            return str;
        }

        public static int DeciToInt(decimal deci)
        {
            return Convert.ToInt32(deci);
        }

        /// <summary>
        /// 搜索结果标红
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string KeyReplace(string key,string str)
        {
            return !string.IsNullOrEmpty(key) ? str.Replace(key, "<font style=\"color:red;\">" + key + "</font>") : str;
        }
        //清除HTML函数  
        public static string NoHTML(string Htmlstring)
        {

            //删除脚本  

            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML  

            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");

            Htmlstring.Replace(">", "");

            Htmlstring.Replace("\r\n", "");

            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;

        }

        /// <summary>
        /// 获取产品实体类
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        public GoodsInfo GetGoodsInfo(int goods_id)
        {
            return SiteBLL.GetGoodsInfo(goods_id);
        }

        #region 自动获取描述
        /// <summary>
        /// 自动获取描述
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="count">截取长度</param>
        /// <returns></returns>
        public static string GetDes(string content, int count)
        {
            content = NoHTML(content);
            if (content.Length > count)
                content = content.Substring(0, count);
            return content;
        }
        #endregion

        #region 是否手机访问
        /// <summary>
        /// 是否手机访问
        /// </summary>
        /// <returns></returns>
        public static bool IsMobileDevice()
        {
            string u = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino|ucweb|mqqbrowser", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!string.IsNullOrEmpty(u))
            {
                if (b.IsMatch(u))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 会员关怀发送邮件
        /// <summary>
        /// 会员关怀发送邮件
        /// </summary>
        /// <param name="emailtitle"></param>
        /// <param name="mailbody"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public static bool SendEmail(string emailtitle,string mailbody,string users)
        {
            bool isok = false;
            IDictionary context = new Hashtable();
            if (!string.IsNullOrEmpty(users))
            {
                for (int i = 0; i < users.Split(',').Length; i++)
                {
                    string email = SiteBLL.GetUsersInfo(Convert.ToInt32(users.Split(',')[i])).email;
                    System.Threading.Thread t = new System.Threading.Thread(() =>
                    {
                        SiteUtils.SendMailUseGmail(email, emailtitle, mailbody);
                    });
                    t.Start();
                    t.IsBackground = true;
                    t.Join();
                }
            }
            return isok;
        }
        #endregion

        #region 获取文章中图片地址的方法
        /// <summary>  
        /// 获取文章中图片地址的方法  
        /// </summary>  
        /// <param name="html">文章内容</param>  
        /// <returns></returns>  
        public static ArrayList GetImgUrl(string html)
        {
            ArrayList resultStr = new ArrayList();
            Regex r = new Regex(@"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(html);

            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups["src"].Value.ToLower());
            }
            if (resultStr.Count > 0)
            {
                return resultStr;
            }
            else
            {
                //没有地址的时候返回空字符  
                resultStr.Add("");
                return resultStr;
            }
        }
        #endregion

        #region 发送微博
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="des">简介</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static string GetDes(string des, string content)
        {
            return string.IsNullOrEmpty(des) ? new SiteUtils().GetTopic(SiteUtils.NoHTML(content), 140) : des;
        }

        /// <summary>
        /// 读取关键词或tag替换成微博话题
        /// </summary>
        /// <param name="tags">标签列表</param>
        /// <param name="keyword">keyword</param>
        public static string GetKeyToWeibo(string[] tags, string[] keyword)
        {
            string key = "";
            string[] content = tags.Length > 0 ? tags : keyword;
            foreach (string str in content)
            {
                key += "#" + str + "# ";
            }
            return key;
        }

        /// <summary>
        /// 获取内容中随机一张图
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static string GetContentImgUrl(string content)
        {
            string imgUrl = "";
            if (GetImgUrl(content).Count > 0)
            {
                Random r = new Random();
                imgUrl = GetImgUrl(content)[r.Next(0, GetImgUrl(content).Count - 1)].ToString();
            }

            return imgUrl;
        }

        /// <summary>
        /// 发送微博
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="file">图片文件</param>
        /// <returns>返回消息</returns>
        public static string SendWeibo(string text, string file)
        {
            string message = "";
            bool isFile = !string.IsNullOrEmpty(file) ? true : false;
            if (isFile && System.IO.File.Exists(file))
            {
                //发图片微博
                var result = DY.OAuthV2Demo.Controllers.MoreOAuth.SendStatusWithPic(text, file);
                message = "发送图片微博";
                if (!string.IsNullOrEmpty(result))
                    message = result;
                else
                    message = result + "，发送成功";
            }
            else
            {
                var result = DY.OAuthV2Demo.Controllers.MoreOAuth.SendStatus(text);
                message = "发送微博";
                if (!string.IsNullOrEmpty(result))
                    message = result;
                else
                    message = result + "，发送成功";
            }
            return message;
        }

        /// <summary>
        /// 发送文章（如头条）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="des">简介</param>
        /// <param name="content">内容</param>
        /// <returns>返回消息</returns>
        public static string SendArticle(string title, string des,string content)
        {
            string message = "";
            if (!string.IsNullOrEmpty(title))
            {
                //发文章
                var result = DY.OAuthV2Demo.Controllers.MoreOAuth.SendStatusToArticle(title, des,content);
                message = "发送文章";
                if (!string.IsNullOrEmpty(result))
                    message = result;
                else
                    message = result + "，发送成功";
            }
            return message;
        }

        /// <summary>
        /// 发送博客
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>返回消息</returns>
        public static string SendWeblog(string title, string content)
        {
            string message = "";
            if (!string.IsNullOrEmpty(title))
            {
                foreach (WeblogInfo webblog in SiteBLL.GetWeblogAllList("sort desc", ""))
                {
                    DY.Site.MetaWeblog.M_MetaWeblog m_blog = new DY.Site.MetaWeblog.M_MetaWeblog();
                    m_blog.Url = webblog.blog_api_url;

                    DY.Site.MetaWeblog.Post newPost = new DY.Site.MetaWeblog.Post();
                    newPost.dateCreated = DateTime.Now;

                    newPost.title = title;
                    newPost.description = content;

                    var result = m_blog.newPost("_blogid", webblog.name, webblog.password, newPost, true);
                    message = webblog.blog_name+"博客";
                    if (!string.IsNullOrEmpty(result))
                        message += result;
                    else
                        message += result + "，发送成功";
                }
            }
            return message;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Entity(string token, string name)
        {
            object user = null;
            var oauth = DY.OAuthV2SDK.OAuths.OAuthBase.CreateInstance();
            string userto = "";
            switch (name)
            {
                case "sina": userto = "users_show"; break;
                case "tenc": userto = "user_info"; break;
                case "neasy": userto = "users_show"; break;
                case "qzone": userto = "user_get_user_info"; break;
                case "renren": userto = "user_get"; break;
                case "kaixin": userto = "users_show"; break;
            }
            System.Collections.Specialized.NameValueCollection paras = new System.Collections.Specialized.NameValueCollection();
            paras.Add("access_token", token);
            var response = oauth.ApiByHttpGet(userto, paras);
            if (response != null)
            {
                switch (name)
                {
                    case "sina": user = DY.OAuthV2SDK.Helpers.UtilHelper.ParseJson<DY.OAuthV2SDK.OAuths.Sinas.Models.SinaMUser>(response); break;
                    case "tenc": user = DY.OAuthV2SDK.Helpers.UtilHelper.ParseJson<DY.OAuthV2SDK.OAuths.Tencs.Models.TencMUserData>(response); break;
                    case "neasy": user = DY.OAuthV2SDK.Helpers.UtilHelper.ParseJson<DY.OAuthV2SDK.OAuths.Neasys.Models.NeasyMUser>(response); break;
                    case "qzone": user = DY.OAuthV2SDK.Helpers.UtilHelper.ParseJson<DY.OAuthV2SDK.OAuths.Qzones.Models.QzoneMUser>(response); break;
                    case "renren": user = DY.OAuthV2SDK.Helpers.UtilHelper.ParseJson<DY.OAuthV2SDK.OAuths.Renrens.Models.RenrenMRsUser>(response); break;
                    case "kaixin": user = DY.OAuthV2SDK.Helpers.UtilHelper.ParseJson<DY.OAuthV2SDK.OAuths.Kaixins.Models.KaixinMUserList>(response); break;
                }

            }
            return user;
        }
        #endregion

        #region 微信
        #region 微信用户
        /// <summary>
        /// 获取关注者列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ArrayList GetWeixinUserList(string accessToken, System.Collections.Generic.List<string> list)
        {
            ArrayList data = new ArrayList(); ;
            for (int i = 0; i < list.Count; i++)
            {
                data.Add(UserApi.Info(accessToken, list[i]));
            }
            return data;
        }
        #endregion

        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(long dateTimeFromXml)
        {
            return DateTimeHelper.GetDateTimeFromXml(dateTimeFromXml);
        }
    #endregion

        #region 时间操作
        /// <summary>
        /// 获取文件修改时间差
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>返回分钟数</returns>
        public static TimeSpan GetDateDiff(string filePath)
        {
            TimeSpan ts = DateDiff2(DateTime.Now, (FileOperate.GetFileAttibe(filePath)));
            return ts;
        }

        #region 获得两个日期的间隔
        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff2(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        #endregion
        /// <summary>
        ///文件保存
        /// </summary>
        /// <returns></returns>
        public static void WriteFile(string str, string path)
        {
            if (!FileOperate.IsExist(path, DY.Common.FileOperate.FsoMethod.File))
                FileOperate.WriteFile(path, str);
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            string oemAll = "";
            if (FileOperate.IsExist(path, DY.Common.FileOperate.FsoMethod.File))
                oemAll = FileOperate.ReadFile(path);
            return oemAll;
        }
        #endregion

        #region 插件
        /// <summary>
        /// 检测插件是否安装或者更新
        /// </summary>
        /// <param name="strPluginName">插件名称</param>
        /// <returns>bool</returns>
        public bool CheckPluginIntall(string strPluginName)
        {
            return Plugin.PluginIsInstalled(strPluginName);
        }
        #endregion

        #region 活动
        /// <summary>
        /// 获取活动名称
        /// </summary>
        /// <param name="atype"></param>
        /// <param name="atype_id"></param>
        /// <returns></returns>
        public static object GetAwardName(int atype, int atype_id)
        {
            object obj = null;
            if (atype_id > 0)
            {
                obj=SiteBLL.GetCardInfo(atype_id);  //刮刮卡
            }
            return obj;
        }

        /// <summary>
        /// 获取随机字符
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            string str = "acbdefghijklmnopqrstuvwxyzACBDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] ch = new char[length];
            Random r = new Random(System.Guid.NewGuid().GetHashCode());
            for (int i = 0; i < ch.Length; i++)
            {
                ch[i] = str[r.Next(0, str.Length)];
            }
            return new string(ch);
        }
        /// <summary>
        /// 获取奖品实体类
        /// </summary>
        /// <param name="award_id"></param>
        /// <returns></returns>
        public static AwardInfo GetAward(int award_id)
        {
            return SiteBLL.GetAwardInfo(award_id);
        }

        #region 返回时间差
        /// <summary>
        /// 时间比较,返回精确的几秒
        /// </summary>
        /// <param name="DateTime1">较早的日期和时间</param>
        /// <param name="DateTime2">较迟的日期和时间</param>
        /// <returns></returns>
        public static int DateDiff_Sec(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            int dateDiff = ts.Days * 86400 + ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
            return dateDiff;
        }
        #endregion
        #endregion


        #region 读取网站风格数据
        public static MStyleInfo GetStyle()
        {
            return SiteBLL.GetMStyleInfo("is_checked=1");
        }
        #endregion

        public static int Conv(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }

        #region
        /// <summary>
        /// 创建新的datatable
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        //public System.Data.DataTable NewFormValue(int pos_id)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    //创建字段标题
        //    foreach (FormAllInfo list in Caches.GetForm(pos_id).Rows)
        //    {
        //        dt.Columns.Add(FunctionUtils.Text.ConvertSpellFirst(list.name));
        //    }
        //    foreach (FormAllInfo list in Caches.GetForm(pos_id).Rows)
        //    {
        //        foreach (FromvalueInfo valueinfo in Caches.GetFormValueSessionId().Rows)
        //        {
        //            foreach (FromvalueInfo valueinfoto in SiteBLL.GetFromvalueAllList(pos_id))
        //            {

        //            }
        //        }
        //        dt.Rows.Add("1", FunctionUtils.Text.ConvertSpellFirst(list.name));
        //    }
        //    return dt;
        //}
        #endregion


        #region 安装目录文件信息
        /// <summary>
        /// 检查安装用录下是否有安装文件,但有就删除,如删除出问题就返回true
        /// </summary>
        /// <returns></returns>
        public static bool IsExistsSetupFile()
        {
            #region 检查安装目录
            string[] installFiles = {
#if !DEBUG
                                        "../install/index.aspx","../install/step2.aspx","../install/step3.aspx","../install/step4.aspx","../install/succeed.aspx",
                                     "../install/systemfile.aspx","../install/pluginsetup.aspx","../install/album.xml","../install/space.xml","../install/mall.xml",
                                        "../install/ajax.aspx","../install/install.aspx",
#endif
                                        "/bin/DY.Install.dll"
                                    };
            foreach (string file in installFiles)
            {
                if (CheckAndDeleteFile(Utils.GetMapPath(file)))
                {
                    return true;
                }
            }
            #endregion

            #region 检查升级目录
            string[] upgradeFiles = {
#if !DEBUG

                                        "../upgrade/index.aspx", "../upgrade/step2.aspx", "../upgrade/succeed.aspx",  "../upgrade/changeavatars.aspx" 
#endif
                                    };

            foreach (string file in upgradeFiles)
            {
                if (CheckAndDeleteFile(Utils.GetMapPath(file)))
                {
                    return true;
                }
            }
            #endregion

            return false;//表示无安装文件
        }


        //检查并删除指定路径的文件
        private static bool CheckAndDeleteFile(string path)
        {
            if (Utils.FileExists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                    return false;
                }
                catch
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 分词工具
        /// 利用SCWS进行中文分词(暂时弃用)
        /// 1638988@gmail.com
        /// </summary>
        /// <param name="str">需要分词的字符串</param>
        /// <returns>用半角,分开的分词结果</returns>
        public static string Segment(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                string s = string.Empty;
                System.Net.CookieContainer cookieContainer = new System.Net.CookieContainer();
                // 将提交的字符串数据转换成字节数组           
                byte[] postData = System.Text.Encoding.ASCII.GetBytes("data=" + System.Web.HttpUtility.UrlEncode(str) + "&respond=json&charset=utf8&ignore=yes&duality=no&traditional=no&multi=0");

                // 设置提交的相关参数
                System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://www.ftphp.com/scws/api.php") as System.Net.HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookieContainer;
                request.ContentLength = postData.Length;

                // 提交请求数据
                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                // 接收返回的页面
                System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                string val = reader.ReadToEnd();

                Newtonsoft.Json.Linq.JObject results = Newtonsoft.Json.Linq.JObject.Parse(val);
                foreach (var item in results["words"].Children())
                {
                    Newtonsoft.Json.Linq.JObject word = Newtonsoft.Json.Linq.JObject.Parse(item.ToString());
                    sb.Append(word["word"].ToString() + ",");
                }
            }
            catch
            {
            }

            return sb.ToString().Substring(0, sb.ToString().LastIndexOf(','));
        }
        #region Discuz分词工具
        /// <summary>
        /// 获取关键字分词
        /// </summary>
        /// <param name="titleenc">标题</param>
        /// <param name="contentenc">内容</param>
        /// <returns></returns>
        public static string GetRelateKeyword(string titleenc,string contentenc)
        {
            string title = Utils.UrlEncode(Utils.RemoveHtml(Utils.ClearUBB(titleenc.Trim())));
            string content = Utils.RemoveHtml(Utils.ClearUBB(contentenc.Trim()));
            content = content.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("　", "");
            content = new SiteUtils().GetTopic(content, 500);
            content = Utils.UrlEncode(content);

            string xmlContent = Utils.GetSourceTextByUrl(string.Format("http://keyword.discuz.com/related_kw.html?title={0}&content={1}&ics=utf-8&ocs=utf-8", title, content));

            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.LoadXml(xmlContent);

            System.Xml.XmlNodeList xnl = xmldoc.GetElementsByTagName("kw");
            StringBuilder builder = new StringBuilder();
            foreach (System.Xml.XmlNode node in xnl)
            {
                builder.AppendFormat("{0},", node.InnerText);
            }

//            StringBuilder xmlBuilder = new StringBuilder(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
//                                            <root><![CDATA[
//                                            <script type=""text/javascript"">
//                                            var tagsplit = $('tags').value.split(' ');
//                                            var inssplit = '{0}';
//                                            var returnsplit = inssplit.split(' ');
//                                            var result = '';
//                                            for(i in tagsplit) {{
//                                                for(j in returnsplit) {{
//                                                    if(tagsplit[i] == returnsplit[j]) {{
//                                                        tagsplit[i] = '';break;
//                                                    }}
//                                                }}
//                                            }}
//
//                                            for(i in tagsplit) {{
//                                                if(tagsplit[i] != '') {{
//                                                    result += tagsplit[i] + ' ';
//                                                }}
//                                            }}
//                                            $('tags').value = result + '{0}';
//                                            </script>
//                                            ]]></root>", builder.ToString()));

//            ResponseXML(xmlBuilder);
            return !string.IsNullOrEmpty(builder.ToString())?builder.ToString().Substring(0,builder.ToString().LastIndexOf(',')):"";
        }
        #endregion

        #region 盘古分词工具
        public static string[] SplitWords(string content)
        {
            List<string> strList = new List<string>();
            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.PanGu.PanGuAnalyzer();//指定使用盘古 PanGuAnalyzer 分词算法
            Lucene.Net.Analysis.TokenStream tokenStream = analyzer.TokenStream("", new System.IO.StringReader(content));
            Lucene.Net.Analysis.Token token = null;
            while ((token = tokenStream.Next()) != null)
            { //Next继续分词 直至返回null
                strList.Add(token.TermText()); //得到分词后结果
            }
            return strList.ToArray();
        }

        //需要添加PanGu.HighLight.dll的引用
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        public static string HightLight(string keyword, string content)
        {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                new PanGu.HighLight.SimpleHTMLFormatter("<font style=\"font-style:normal;color:#cc0000;\"><b>", "</b></font>");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
                            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                            new PanGu.Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 1000;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
        #endregion

        #endregion

        #region 检测是否存在此模板
        /// <summary>
        /// 检测是否存在此模板
        /// </summary>
        /// <param name="tlp">默认模板</param>
        /// <param name="new_tlp">自定义模板</param>
        /// <returns></returns>
        public static string CheckTlp(string tlp,string new_tlp)
        {
            if (FileOperate.IsExist(System.Web.HttpContext.Current.Server.MapPath(SkinPath() + new_tlp + BaseConfig.WebSkinSuffix), FileOperate.FsoMethod.File))
                return new_tlp;
            else
                return tlp;
        }
        #endregion

        #region 返回模板路径
        /// <summary>
        /// 返回模板路径
        /// </summary>
        /// <returns></returns>
        public static string SkinPath()
        {
            string skinPath = IsMobileDevice() ? BaseConfig.WapSkinPath : BaseConfig.WebSkinPath;

            #region 针对演示手机使用
            //string theme = DYRequest.getRequest("theme");
            //if (!string.IsNullOrEmpty(theme))
            //{
            //    skinPath = string.IsNullOrEmpty(theme) ? skinPath : "/mobile/" + theme + "/";
            //    System.Web.HttpContext.Current.Session["theme"] = theme;
            //}
            //if (System.Web.HttpContext.Current.Session["theme"] != null)
            //{
            //    skinPath = "/mobile/" + System.Web.HttpContext.Current.Session["theme"] + "/";
            //}
            #endregion

            return skinPath;
        }
        /// <summary>
        /// 返回模板路径
        /// </summary>
        /// <returns></returns>
        public static string SkinPath(IDictionary context)
        {
            string skinPath = IsMobileDevice() ? BaseConfig.WapSkinPath : BaseConfig.WebSkinPath;

            #region 针对演示手机使用
            //string theme = DYRequest.getRequest("theme");
            //if (!string.IsNullOrEmpty(theme))
            //{
            //    skinPath = string.IsNullOrEmpty(theme) ? skinPath : "/mobile/" + theme + "/";
            //    System.Web.HttpContext.Current.Session["theme"] = theme;
            //    context.Remove("mstyle");
            //    context.Add("mstyle", SiteBLL.GetMStyleInfo("skin_path='" + theme + "'"));
            //}
            //if (System.Web.HttpContext.Current.Session["theme"] != null)
            //{
            //    skinPath = "/mobile/" + System.Web.HttpContext.Current.Session["theme"] + "/";
            //    context.Remove("mstyle");
            //    context.Add("mstyle", SiteBLL.GetMStyleInfo("skin_path='" + System.Web.HttpContext.Current.Session["theme"] + "'"));
            //}
            #endregion

            return skinPath;
        }
        #endregion

        #region ping 百度接口
        /// <summary>
        /// ping 百度接口
        /// </summary>
        /// <param name="type">baidu,google,360(暂未开通)</param>
        /// <param name="thisUrl">当前文章地址</param>
        /// <returns></returns>
        public int SendPing(string type,string thisUrl)
        {
            string url = "";
            switch (type)
            {
                case "baidu": url = "http://ping.baidu.com/ping/RPC2"; break;
                case "360": url = ""; break;
                case "google": url = "http://blogsearch.google.com/ping/RPC2"; break;
            }
            return Execute(url, thisUrl, "http://" + new SiteUtils().GetDomain() + "/sitemap.xml");
        }
        /// <summary>
        /// 创建一个web请求和XML代码流中的RPC。
        /// </summary>
        /// <param name="url">ping服务器地址，如：http://ping.baidu.com/ping/RPC2 </param>
        /// <param name="thisUrl">当前文章地址</param>
        /// <param name="rss">rss或者站点地图</param>
        /// <returns></returns>
        private int Execute(string url, string thisUrl,string rss)
        {
            //try
            //{
            //    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            //    request.Method = "POST";
            //    request.ContentType = "text/xml";
            //    request.Timeout = 3000;

            //    AddXmlToRequest(request, config.Title, "http://" + new SiteUtils().GetDomain(), "http://" + new SiteUtils().GetDomain() + "n_detail/" + base.id + ".htm", "http://" + new SiteUtils().GetDomain() + "/sitemap.xml");
            //    //System.Net.HttpWebResponse respone = (System.Net.HttpWebResponse)request.GetResponse();
            //    //System.IO.StreamReader stream = new System.IO.StreamReader(respone.GetResponseStream(), System.Text.Encoding.Default);
            //    //string resultStr = stream.ReadToEnd();
            //}
            //catch (Exception)
            //{
            //    // Log the error.
            //}


            return Ping(url, config.Title, "http://" + new SiteUtils().GetDomain(), thisUrl, rss);

        }

        /// <summary>
        /// 生成XML请求
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="title">网站title</param>
        /// <param name="domain">网站域名</param>
        /// <param name="thisUrl">当前url</param>
        /// <param name="siteMap">rss或者站点地图</param>
        private static void AddXmlToRequest(System.Net.HttpWebRequest request, string title, string domain, string thisUrl, string siteMap)
        {
            ////读取最近一周添加的新闻
            //List<News> listNews = new List<News>();
            //List<News> listAdd = new List<News>();
            //var whereNews = QueryBuilder.Create<News>().Equals(c => c.IsPass, true).Equals(c => c.Lang, "CN");
            //listNews = tNewsDao.Search_NewsByconditions(whereNews);
            //for (int m = 0; m < listNews.Count; m++)
            //{
            //    DateTime timeOld = Convert.ToDateTime(listNews[m].AddTime);
            //    TimeSpan spanTime = DateTime.Now.Subtract(timeOld);

            //    if (spanTime.TotalDays < Convert.ToDouble(7))
            //    {
            //        listAdd.Add(listNews[m]); //判断添加新闻的时间跟当前时间比较，小于7天，添加到listAdd
            //    }
            //}


            System.IO.Stream stream = (System.IO.Stream)request.GetRequestStream();
            using (System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("methodCall");
                writer.WriteElementString("methodName", "weblogUpdates.extendedPing");
                writer.WriteStartElement("params");

                writer.WriteStartElement("param");
                writer.WriteStartElement("value");
                writer.WriteElementString("string", title);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("param");
                writer.WriteStartElement("value");
                writer.WriteElementString("string", domain);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("param");
                writer.WriteStartElement("value");
                writer.WriteElementString("string", thisUrl);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("param");
                writer.WriteStartElement("value");
                writer.WriteElementString("string", siteMap);
                writer.WriteEndElement();
                writer.WriteEndElement();


                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }



        public interface IMath : IXmlRpcProxy
        {

            [XmlRpcMethod("weblogUpdates.extendedPing")]
            [return: XmlRpcReturnValue(Description = "返回结果1,0")]
            //CookComputing.XmlRpc.XmlRpcStruct GooglePing(string a, string b, string c, string d); //google
            int Ping(string a, string b, string c, string d);//百度

        }

        protected IMath Proxy = XmlRpcProxyGen.Create<IMath>();
        protected int Ping(string url, string title, string domain, string thisUrl, string siteMap)
        {
            Proxy.Url = url;
            Proxy.XmlEncoding = System.Text.Encoding.UTF8;
            Proxy.Ping(title, domain, thisUrl, siteMap);
            return Proxy.Ping(title, domain, thisUrl, siteMap);
        }
        #endregion

        #region cnzz-DEM合作接口
        /// <summary>
        /// cnzz-DEM文件保存
        /// </summary>
        /// <returns></returns>
        public static bool WriteFileToCnzz()
        {
            try
            {
                string domain = Utils.GetDomain();
                string key = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(domain + BaseConfig.Oem, "MD5").ToLower();
                string url = "http://wss.cnzz.com/user/companion/ctmon.php?domain=" + domain + "&key=" + key + "&cms=" + BaseConfig.OemCms;
                string filePath = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.OemPath);
                FileOperate.WriteFile(filePath, Utils.NoHTML(CommonUtils.LoadURLString(url)));
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        public static string ReadFileToCnzz()
        {
            string oemAll = "";
            string path = System.Web.HttpContext.Current.Server.MapPath(BaseConfig.OemPath);
            if (FileOperate.IsExist(path, DY.Common.FileOperate.FsoMethod.File))
                oemAll = FileOperate.ReadFile(path);
            else
                WriteFileToCnzz();
            return oemAll;
        }
        #endregion

        #region 将两个列不同的DataTable合并成一个新的DataTable
        ///<summary>   
        /// 将两个列不同的DataTable合并成一个新的DataTable   
        ///</summary>   
        ///<param name="dt1">源表</param>   
        ///<param name="dt2">需要合并的表</param>   
        ///<param name="primaryKey">需要排重列表（为空不排重）</param>   
        ///<param name="maxRows">合并后Table的最大行数</param>   
        ///<returns>合并后的datatable</returns>
        public static DataTable MergeDataTable(DataTable dt1, DataTable dt2, string primaryKey, int maxRows)
        {
            //判断是否需要合并
            if (dt1 == null && dt2 == null)
            {
                return null;
            }
            if (dt1 == null && dt2 != null)
            {
                return dt2.Copy();
            }
            else if (dt1 != null && dt2 == null)
            {
                return dt1.Copy();
            }
            //复制dt1的数据
            DataTable dt = dt1.Copy();
            //补充dt2的结构（dt1中没有的列）到dt中
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                string cName = dt2.Columns[i].ColumnName;
                if (!dt.Columns.Contains(cName))
                {
                    dt.Columns.Add(new DataColumn(cName));
                }
            }
            //复制dt2的数据
            if (dt2.Rows.Count > 0)
            {
                Type t = dt2.Rows[0][primaryKey].GetType();
                bool isNeedFilter = string.IsNullOrEmpty(primaryKey) ? false : true;
                bool isNeedQuotes = t.Name == "String" ? true : false;
                int mergeTableNum = dt.Rows.Count;
                for (int i = 0; i < dt2.Rows.Count && mergeTableNum < maxRows; i++)
                {
                    bool isNeedAdd = true;
                    //如果需要排重时，判断是否需要添加当前行
                    if (isNeedFilter)
                    {
                        string primaryValue = dt2.Rows[i][primaryKey].ToString();
                        string fileter = primaryKey + "=" + primaryValue;
                        if (isNeedQuotes)
                        {
                            fileter = primaryKey + "='" + primaryValue + "'";
                        }
                        DataRow[] drs = dt.Select(fileter);
                        if (drs != null && drs.Length > 0)
                        {
                            isNeedAdd = false;
                        }
                    }
                    //添加数据
                    if (isNeedAdd)
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string cName = dt.Columns[j].ColumnName;
                            if (dt2.Columns.Contains(cName))
                            {
                                //防止因同一字段不同类型赋值出错
                                if (dt2.Rows[i][cName] != null && dt2.Rows[i][cName] != DBNull.Value && dt2.Rows[i][cName].ToString() != "")
                                {
                                    dr[cName] = dt2.Rows[i][cName];
                                }
                            }
                        }
                        dt.Rows.Add(dr);
                        mergeTableNum++;
                    }
                }
            }
            return dt;
        }
        #endregion

        #region 模板文件路径操作类
        /// <summary>
        /// 模板文件路径操作类，主要针对前台模板目录路径
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content">html内容</param>
        /// <param name="tlpPath">当前模板路径</param>
        /// <returns></returns>
        public static string ReplacePath(IDictionary context, string content, string tlpPath)
        {
            BaseConfigInfo config = BaseConfig.Get();
            string url_type = config.UrlType > 0 ? urlrewrite.http + new SiteUtils().GetDomain() : "";

            //如果为后台模板，则替换相应路径
            if (tlpPath == BaseConfig.AdminSkinPath)
            {
                content = content.Replace("href=\"css", string.Format("href=\"{0}css", BaseConfig.AdminUIPath));
                content = content.Replace("href=\"font-awesome", string.Format("href=\"{0}font-awesome", BaseConfig.AdminUIPath));
                content = content.Replace("src=\"js", string.Format("src=\"{0}js", BaseConfig.AdminUIPath));
                content = content.Replace("src=\"img", string.Format("src=\"{0}img", BaseConfig.AdminUIPath));
                content = content.Replace("href=\"js", string.Format("href=\"{0}js", BaseConfig.AdminUIPath));
                //context.Clear();

            }
            else
            {
                content = content.Replace("href=\"css", string.Format("href=\"{0}css", tlpPath));
                content = content.Replace("src=\"images", string.Format("src=\"{0}images", tlpPath));
                content = content.Replace("src=\"js", string.Format("src=\"{0}js", tlpPath));

                //绝对路径暂时只针对前台页面
                content = content.Replace("href=\"/", string.Format("href=\"{0}/", url_type));
                content = content.Replace("src=\"/", string.Format("src=\"{0}/", url_type));

                //替换分站地区词，当前默认指定主站为重庆
                if (context["cityname"] != null && !string.IsNullOrEmpty(context["cityname"].ToString()))
                    content=ReplaceCityName(content, context["cityname"].ToString());
                //content = Regex.Replace(content, @"(?<=<img[\s\S]*?)alt=((['""])[^'""]*\2|\S+)(?=[^>]*>)", "alt=\"测试\"", RegexOptions.IgnoreCase);

                context.Clear();
            }
            return content;
        }
        #endregion

        /// <summary>
        /// 替换指定城市
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="cityname">当前城市名</param>
        /// <returns></returns>
        public static string ReplaceCityName(string str, string cityname)
        {
            //Match mc = Regex.Match(str, @"(?is)<title>(?<title>.*?)</title>");
            //string title = mc.Groups["title"].Value;
            //str = System.Text.RegularExpressions.Regex.Replace(str, @"<title>(.*)<\/title>", "替换临时文件", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            str = str.Replace("重庆", cityname);
            //str = str.Replace("替换临时文件", "<title>" + title + "</title>");
            return str;
        }



       /// <summary>
       /// 加密压缩文件
       /// </summary>
       /// <param name="tlpPath"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        //public static string SiteBundles(string tlpPath,string type)
        //{
        //    System.Web.Optimization.BundleTable.Bundles.IgnoreList.Clear();
        //    switch (type)
        //    {
        //        case "js": System.Web.Optimization.BundleTable.Bundles.Add(new System.Web.Optimization.ScriptBundle("~"+tlpPath + type).Include("~" + tlpPath + "" + type + "/*." + type)); break;
        //        case "css": System.Web.Optimization.BundleTable.Bundles.Add(new System.Web.Optimization.StyleBundle("~" + tlpPath + type).Include("~" + tlpPath + "" + type + "/*." + type)); break;
        //    }
        //    System.Web.Optimization.BundleTable.EnableOptimizations = true;
        //    return System.Web.Optimization.BundleTable.Bundles.ResolveBundleUrl("~" + tlpPath + type);
           
        //}

        #region 记录蜘蛛爬行记录
        /// <summary>
        /// 记录蜘蛛爬行记录
        /// </summary>
        public static string GetSpiderBot()
        {
            try
            {
                string agent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToLower();
                string bot = "";
                if (agent.Contains("googlebot"))
                    bot = "谷歌";
                else if (agent.Contains("mediapartners-google"))
                    bot = "Google_Adsense";
                else if (agent.Contains("baiduspider"))
                    bot = "百度";
                else if (agent.Contains("sogou"))
                    bot = "搜狗";
                else if (agent.Contains("yahoo"))
                    bot = "雅虎";
                else if (agent.Contains("msn"))
                    bot = "MSN";
                else if (agent.Contains("bingbot"))
                    bot = "必应";
                else if (agent.Contains("360spider"))
                    bot = "360";
                else if (agent.Contains("ia_archiver"))
                    bot = "Alexa";
                else if (agent.Contains("iaarchiver"))
                    bot = "Alexa";
                else if (agent.Contains("compatible"))
                    bot = "Alexa";
                else if (agent.Contains("iaarchiver"))
                    bot = "Alexa";
                else if (agent.Contains("sohu"))
                    bot = "搜狐";
                else if (agent.Contains("yodaobot"))
                    bot = "有道";
                else if (agent.Contains("iaskspider"))
                    bot = "Iask";
                else if (agent.Contains("iaskspider"))
                    bot = "Iask";
                else if (agent.Contains("jikespider"))
                    bot = "Jike";
                else if (agent.Contains("sosospider"))
                    bot = "腾讯SOSO";
                else if (agent.Contains("yisouspider"))
                    bot = "易搜";
                else if (agent.Contains("twiceler"))
                    bot = "Twiceler";
                else if (agent.Contains("xunlei"))
                    bot = "迅雷";
                //else if (agent.Contains("mozilla"))
                //    bot = "正常访问/模拟蜘蛛";
                else if (agent.Contains("bot"))
                    bot = "其他";

                return bot;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 给实体赋值
        /// </summary>
        public static RobotsInfo SetRobotsEntity()
        {
            RobotsInfo entity = new RobotsInfo();

            entity.url = urlrewrite.http + new SiteUtils().GetDomain() + System.Web.HttpContext.Current.Request.RawUrl;
            entity.robots = GetSpiderBot();
            entity.date = DateTime.Now;
            entity.ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            return entity;
        }

        public static void SpiderBot()
        {
            RobotsInfo robots = SetRobotsEntity();
            if (GetSpiderBot().Length > 0)
            {
                int id = SiteBLL.InsertRobotsInfo(robots);
            }
        }

        /// <summary>
        /// 查询统计蜘蛛
        /// </summary>
        /// <param name="type">蜘蛛类型</param>
        /// <param name="date">0为当天，1为昨天,以此类推</param>
        /// <param name="where">是否查询全部</param>
        public static int StatisticsSpiderBot(string type,int date,string where)
        {
            if (string.IsNullOrEmpty(where))
                return Convert.ToInt32(SiteBLL.GetRobotsValue("count(id)", "robots='" + type + "' and DateDiff(dd,date,getdate())=" + date));
            else
                return Convert.ToInt32(SiteBLL.GetRobotsValue("count(id)", "robots='" + type + "'"));
        }

        /// <summary>
        /// 处理升降变化
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        public static int GetSpiderBotNum(int a,int b)
        {
            if (a > b)
                return a - b;
            else if (a == b)
                return 0;
            else
                return b - a;
        }
        #endregion

        #region 留言处理
        /// <summary>
        /// Feedback留言处理
        /// </summary>
        /// <param name="feedbackinfo">留言实体类</param>
        /// <param name="mailbody">解析后的邮件模板</param>
        /// <returns></returns>
        public static string CheckFeedback(FeedbackInfo feedbackinfo, string mailbody)
        {
            BaseConfigInfo config = new SiteUtils().config;
            string[] keys = Utils.SplitString(config.Keywords_shielding, "\r\n");
            string filter_message = "";

            #region 屏蔽关键词
            if (!string.IsNullOrEmpty(config.Keywords_shielding))
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (feedbackinfo.user_name.Contains(keys[i]) || feedbackinfo.user_email.Contains(keys[i]) || feedbackinfo.msg_content.Contains(keys[i]))
                    {
                        filter_message = "存在不良或敏感词，提交失败！";
                    }
                }
            }
            #endregion

            #region IP记录
            feedbackinfo.ip = Utils.GetIP();
            if (Convert.ToInt32(SiteBLL.GetFeedbackValue("count(*)", "ip='" + Utils.GetIP() + "' and DateDiff(dd,msg_time,getdate())<1")) >= config.Ip_count)
            {
                filter_message = "提交失败，超过了系统限制次数！";
            }
            if (!Utils.IsSafeSqlString(feedbackinfo.user_name) || !Utils.IsValidEmail(feedbackinfo.user_email) || !Utils.IsSafeUserInfoString(feedbackinfo.msg_content))
            {
                filter_message = "系统检测到危险字符，提交失败！";
            }
            #endregion

            #region 发送邮件
            if (new SiteUtils().config.Mailalert)
            {
                bool isok = SiteUtils.SendMailUseGmail(config.SmtpMail, config.Name + "收到用户信息", mailbody.Replace("{username}", feedbackinfo.user_name).Replace("{useremail}", feedbackinfo.user_email).Replace("{content}", Utils.RemoveHtml(feedbackinfo.msg_content)));

                if (!isok)
                {
                    filter_message = "内部错误";
                }
            }
            #endregion

            return filter_message;
        }


        /// <summary>
        /// email留言处理
        /// </summary>
        /// <param name="feedbackinfo">留言实体类</param>
        /// <param name="mailbody">解析后的邮件模板</param>
        /// <returns></returns>
        public static string CheckEmailInfo(EmailListInfo emailinfo, string mailbody)
        {
            BaseConfigInfo config = new SiteUtils().config;
            string[] keys = Utils.SplitString(config.Keywords_shielding, "\r\n");
            string filter_message = "";

            #region 屏蔽关键词
            if (!string.IsNullOrEmpty(config.Keywords_shielding))
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (emailinfo.hash.Contains(keys[i]) || emailinfo.email.Contains(keys[i]) || emailinfo.remark.Contains(keys[i]))
                    {
                        filter_message = "存在不良或敏感词，提交失败！";
                    }
                }
            }
            #endregion

            #region 发送邮件
            if (new SiteUtils().config.Mailalert)
            {
                bool isok = SiteUtils.SendMailUseGmail(config.SmtpMail, config.Name + "收到用户信息", mailbody.Replace("{username}", emailinfo.nickname).Replace("{useremail}", emailinfo.email).Replace("{content}", Utils.RemoveHtml(emailinfo.remark)));

                if (!isok)
                {
                    filter_message = "内部错误";
                }
            }
            #endregion

            return filter_message;
        }
        #endregion

        /// <summary>
        /// string类模板,再次解析字段内的模板
        /// </summary>
        /// <param name="context">Hashtable</param>
        /// <param name="templateString"></param>
        public static string GetTemplateString(IDictionary context, string templateString)
        {
            NVelocityTemplateEngine.Interfaces.INVelocityEngine memoryEngine =
                NVelocityTemplateEngine.NVelocityEngineFactory.CreateNVelocityMemoryEngine(true);

            return memoryEngine.Process(context, templateString);
        }

    }
}
