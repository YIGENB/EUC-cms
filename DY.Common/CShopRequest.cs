using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DY.Common
{
    public class DYRequest
    {
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objid"></param>
        /// <returns></returns>
        public static string getForm(string objName)
        {
            if (HttpContext.Current.Request.Form[objName] == null)
                return HttpContext.Current.Request.Form[objName];

            return Utils.ChkSQL(HttpContext.Current.Request.Form[objName].Trim());
        }
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objid"></param>
        /// <returns></returns>
        public static string getFormString(string objName)
        {
            if (HttpContext.Current.Request.Form[objName] == null)
                return "";

            return Utils.ChkSQL(HttpContext.Current.Request.Form[objName].Trim());
        }
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static int getFormInt(string objName)
        {
            return Utils.StrToInt(HttpContext.Current.Request.Form[objName], 0);
        }
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static decimal getFormDecimal(string objName)
        {
            return Utils.StrToDecimal(getForm(objName), 0);
        }
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="DefaultValue">如果获取失败，则取该值</param>
        /// <returns></returns>
        public static int getFormInt(string objName, int DefaultValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.Form[objName], DefaultValue);
        }
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static bool getFormBoolean(string objName)
        {
            return Convert.ToBoolean(Utils.StrToInt(getForm(objName), 0));
        }
        /// <summary>
        /// 取得post提交表单值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static DateTime? getFormDateTime(string objName)
        {
            if (string.IsNullOrEmpty(getForm(objName)))
                return null;

            return Utils.StrToDataTime(getForm(objName), DateTime.Now);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static string getRequest(string objName)
        {
            return Utils.ChkSQL(HttpContext.Current.Request.QueryString[objName]);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static string getRequest(string objName,string defaultValue)
        {
            if (HttpContext.Current.Request.QueryString[objName] == null)
                return defaultValue;

            return Utils.ChkSQL(HttpContext.Current.Request.QueryString[objName]);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static int getRequestInt(string objName)
        {
            return Utils.StrToInt(HttpContext.Current.Request.QueryString[objName], 0);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static int getRequestInt(string objName, int defaultValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.QueryString[objName], defaultValue);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static decimal getRequestDecimal(string objName)
        {
            return Utils.StrToDecimal(HttpContext.Current.Request.QueryString[objName], 0);
        }
        /// <summary>
        /// 取得页面执行动作
        /// </summary>
        /// <returns></returns>
        public static string getAction()
        {
            if (!string.IsNullOrEmpty(getRequest("act")))
                return getRequest("act");

            return getForm("act");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        public static bool IsPost {
            get {
                return HttpContext.Current.Request.HttpMethod.Equals("POST");
            }
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        public static bool IsGet
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("GET");
            }
        }
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !Utils.IsIP(result))
                return "127.0.0.1";

            return result;
        }
        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }
    }
}
