using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.IO;

namespace DY.Web.admin.updatemoreimg
{
    public partial class ajax : WebPage
    {
        private string seoUrl = "http://www.aizhan.com/siteall/" + new SiteUtils().GetDomain();
        private string seoUrl1 = "http://seo.chinaz.com/seohis/?host=" + new SiteUtils().GetDomain();
        private string words_url = "http://ci.aizhan.com/";
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (base.act)
            {

                case "V_proname":
                    if (ispost)
                        V_proname();
                    break;
                case "V_proorder":
                    if (ispost)
                        V_proorder();
                    break;
                case "V_goods_sn":
                    if (ispost)
                        V_goods_sn();
                    break;

                case "delpic":
                    if (ispost)
                        Del_pic();
                    break;

                //seo
                case "baidu_seo":
                    if (ispost)
                        Baidu_SEO();
                    break;

                //seo
                case "alexaUsageStatistic":
                    if (ispost)
                        Alexa();
                    break;

                //长尾
                case "words":
                    if (ispost)
                        Words();
                    break;

            }
        }

        /// <summary>
        /// 抓取站长工具长尾信息
        /// </summary>
        protected void Words()
        {
            string content = "", messages = config.Name;
            if (config.Site_word)
            {
                string pattern = string.Format("(?is){0}(.*?){1}", DYRequest.getForm("s"), DYRequest.getForm("e"));
                string keyword = SiteUtils.GetRelateKeyword(DYRequest.getForm("title"), DYRequest.getForm("content"));
                if (config.Participle_word)
                {
                    if (keyword.Contains(","))
                    {
                        for (int i = 0; i < keyword.Split(',').Length; i++)
                        {
                            int count = keyword.Split(',').Length > 3 ? 1 : config.Word_count;
                            content += SiteUtils.NoHTML(SiteUtils.GetWordMatch(words_url + keyword.Split(',')[i] + "/", pattern, count));
                        }
                    }
                    else
                        content = SiteUtils.GetWordMatch(words_url + keyword + "/", pattern, config.Word_count);
                    content = string.IsNullOrEmpty(content) ? "" : content;

                    #region 以下为收费工具，词库
                    //string content = "",message="";
                    //string kw = DYRequest.getForm("kw");
                    //if (string.IsNullOrEmpty(kw))
                    //{
                    //    message = string.Format(KWUtils.ErrorInfoFormat, "输入的关键词不合法");
                    //}
                    //else
                    //{
                    //    WordQuery<WordModelList> wq = new WordQuery<WordModelList>();
                    //    wq.ModuleType = EnumUtils.Module.NagaoWord;
                    //    wq.PageIndex = 1;
                    //    wq.Wd = kw;
                    //    WordModelList list = wq.GetResult();
                    //    List<WordModel> modellist = list.data;
                    //    foreach (WordModel model in modellist)
                    //    {
                    //        content += model.keyword + ",";
                    //    }
                    //    content = content.Substring(0, content.LastIndexOf(','));
                    //}
                    #endregion

                    content = SiteUtils.NoHTML(content.Substring(0, content.LastIndexOf(',')));
                }
                else
                    content = keyword;
            }

            base.DisplayMemoryTemplate(base.MakeJson(content, 0, messages));
        }


        /// <summary>
        /// 抓取页面信息
        /// </summary>
        protected void Baidu_SEO()
        {
            string pattern = string.Format("(?is){0}(.*?){1}", DYRequest.getForm("s"), DYRequest.getForm("e"));
            string type = DYRequest.getForm("type");
            string content = SiteUtils.GetMatch(type == "0" ? seoUrl : seoUrl1, pattern, type == "1" ? "chinaz" : "aizhan");
            content = string.IsNullOrEmpty(content) ? "-" : content;
            base.DisplayMemoryTemplate(base.MakeJson(content, 0, ""));
        }

        /// <summary>
        /// 抓取爱站alexa排名json
        /// </summary>
        protected void Alexa()
        {
            string pattern = string.Format("(?is){0}(.*?){1}", DYRequest.getForm("s"), DYRequest.getForm("e"));
            string content = SiteUtils.GetMatch(seoUrl, pattern, "aizhan");
            //string sss = content.Replace("(", "").Replace(")", "");
            content = string.IsNullOrEmpty(content) ? "-" : content;
            base.DisplayMemoryTemplate(content.Replace("(", "").Replace(")", ""));
        }


        /// <summary>
        /// 检测产品名
        /// </summary>
        protected void V_proname()
        {
            string goodsname = DYRequest.getForm("goods_name");
            string original_img = SiteBLL.GetGoodsValue("goods_name", "goods_name='" + goodsname + "'").ToString();
            base.DisplayMemoryTemplate(base.MakeJson(original_img, 0, ""));
        }
        /// <summary>
        /// 检测序号
        /// </summary>
        protected void V_proorder()
        {
            string sort_order = DYRequest.getForm("sort_order");
            string original_img = SiteBLL.GetGoodsValue("sort_order", "sort_order='" + sort_order + "'").ToString();
            base.DisplayMemoryTemplate(base.MakeJson(original_img, 0, ""));
        }
        /// <summary>
        /// 检测货号
        /// </summary>
        protected void V_goods_sn()
        {
            string goods_sn = DYRequest.getForm("goods_sn");
            string original_img = SiteBLL.GetGoodsValue("goods_sn", "goods_sn='" + goods_sn + "'").ToString();
            base.DisplayMemoryTemplate(base.MakeJson(original_img, 0, ""));
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        protected void Del_pic()
        {

            try
            {
                string sort_order = DYRequest.getForm("file_path");
                string[] arr = sort_order.Split(',');
                foreach (string ImageUrl in arr)
                {
                    string FilePath = Server.MapPath(ImageUrl);//转换物理路径
                    File.Delete(FilePath);//执行IO文件删除,需引入命名空间System.IO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}