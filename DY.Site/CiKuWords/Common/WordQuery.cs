using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Site
{
    public class WordQuery<T> where T:class
    {
        /// <summary>
        /// 从接口获取的结果
        /// </summary>
        private string Json { get; set; }

        /// <summary>
        /// 调用接口的URL
        /// </summary>
        private string Url { get; set; }

        /// <summary>
        /// 所属哪个模块调用
        /// </summary>
        public EnumUtils.Module ModuleType { get; set; }

        /// <summary>
        /// 关键词或者网址
        /// </summary>
        public string Wd { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get;private set; }

        /// <summary>
        /// 获取结果 返回结果必须判断是否为null再进行其它操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetResult()
        {
            FromatUrl();
            GetJson();
            if (string.IsNullOrEmpty(Json))
            {
                ErrorMessage = "获取不到数据";
                return default(T);
            }
            try
            {
                Json = KWUtils.UnicodeToString(Json);
                if (Json.IndexOf(@"state"":1,") == -1)
                {
                    ErrorMessage = KWUtils.GetSubString(Json, @"error"":""", @"""}");
                    return default(T);
                }
                T result = LitJson.JsonMapper.ToObject<T>(Json);
                return result;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return default(T);
            }
        
        }

        /// <summary>
        /// 格式化调用接口地址
        /// </summary>
        private void FromatUrl (){
            switch (ModuleType)
            {
                case EnumUtils.Module.NagaoWord:
                case EnumUtils.Module.SpecifiedWord:
                case EnumUtils.Module.SiteWord:
                    Url = string.Format("{0}?action={1}&appid={2}&apptoken={3}&wd={4}&pageindex={5}&ver={6}", KWConfig.ApiBaseUrl, (byte)ModuleType, KWConfig.AppId, KWUtils.MD5Encrypt(KWConfig.AppKey), KWUtils.UrlEncode(Wd), PageIndex, KWConfig.Ver); 
                     break;
                case EnumUtils.Module.NewWord:
                case EnumUtils.Module.HotTreadWord:
                case EnumUtils.Module.HotWord:
                     Url = string.Format("{0}?action={1}&appid={2}&apptoken={3}&pageindex={4}&ver={5}", KWConfig.ApiBaseUrl, (byte)ModuleType, KWConfig.AppId, KWUtils.MD5Encrypt(KWConfig.AppKey), PageIndex, KWConfig.Ver); 
                     break;
            }
        }

        protected  void GetJson()
        {
            HttpHelper http = new HttpHelper();
            Json = http.GetHtml(Url);
        }
    }
}
