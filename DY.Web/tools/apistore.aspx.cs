using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Collections.Generic;
using DY.Cache;

namespace DY.Web
{
    public partial class apistore : WebPage
    {
        IDictionary context = new Hashtable();
        private ExpressControl control = new ExpressControl();
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (base.act)
            {
                //case "kuaidi100": Express(control.Get(ExpressControl.ExpressType.Express100)); break;
                case "express": this.Express(); break;
                case "submitzhizhuchi": this.SubmitZhiZhuChi(); break;
                case "selectzhizhuchi": this.SelectZhiZhuChi(); break;
                case "rollnews": this.RollNewsFromQQ(); break;
            }
        }

        #region 百度快递接口
        /// <summary>
        /// 百度快递接口(http://apistore.baidu.com/apiworks/servicedetail/1727.html)
        /// </summary>
        public void Express()
        {
            string nu = DYRequest.getFormString("nu");
            DYCache cache = DYCache.GetCacheService();
            string data = cache.RetrieveObject("/DY/BaiduApiStore/express/t" + nu) as string;
            if (data == null)
            {
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("type", DYRequest.getFormString("com").ToUpper());
                parameters.Add("number", nu);
                data = control.Get(ExpressControl.ExpressType.Expressjishu).CreatePostHttpResponse(parameters);

                cache.AddObject("/DY/BaiduApiStore/express/t" + nu, data);
            }
            base.DisplayMemoryTemplate(data);
            
        }

        #endregion

        #region 超级站长蜘蛛池
        /// <summary>
        /// 超级站长蜘蛛池提交接口(http://zzuser.chaojirj.com/)
        /// </summary>
        public void SubmitZhiZhuChi()
        {
            DYCache cache = DYCache.GetCacheService();
            string data = "";
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("u", config.Zzuser);
            parameters.Add("p", config.Zzpwd);
            parameters.Add("softid", config.Zzsoftid);
            parameters.Add("softkey", config.Zzsoftkey);
            parameters.Add("urls", urlrewriteinfo.http + new SiteUtils().GetDomain() + "/sitemap.xml");
            data=control.Get(ExpressControl.ExpressType.SubmitZhiZhuChi).CreatePostHttpResponse(parameters);
            base.DisplayMemoryTemplate(data);
        }

        /// <summary>
        /// 超级站长蜘蛛池积分查询(http://zzuser.chaojirj.com/)
        /// </summary>
        public void SelectZhiZhuChi()
        {
            DYCache cache = DYCache.GetCacheService();
            string data = "";
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("username", config.Zzuser);
            parameters.Add("pwd", config.Zzpwd);
            data = control.Get(ExpressControl.ExpressType.SelectZhiZhuChi).CreatePostHttpResponse(parameters);
            base.DisplayMemoryTemplate(data);
        }
        #endregion

        #region 腾讯滚动新闻接口（HTTP接口）
        /// <summary>
        /// 腾讯滚动新闻接口(http://roll.news.qq.com/interface/roll.php?site=news&date=&page=1&mode=1&of=xml)
        /// </summary>
        public void RollNewsFromQQ()
        {
            string data = "";
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("site", new SiteUtils().DefaultValue(DYRequest.getRequest("site"), "auto"));
            parameters.Add("date", DYRequest.getRequest("date"));
            parameters.Add("page", new SiteUtils().DefaultValue(DYRequest.getRequest("page"), "1"));
            parameters.Add("mode", new SiteUtils().DefaultValue(DYRequest.getRequest("mode"), "1"));
            parameters.Add("of", "xml");
            data = control.Get(ExpressControl.ExpressType.RollNewsFromQQ).CreatePostHttpResponse(parameters);
            Rollroot roll = RollNewsXMLHelper.FromXml<Rollroot>(data);
            base.DisplayMemoryTemplate(roll.data.article_info);
        }
        #endregion


    }
}