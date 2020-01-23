/**
 * 功能描述：资讯首页
 * 创建时间：2010-3-2 上午 10:19:30
 * 最后修改时间：2010-3-2 上午 10:19:30
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using CShop.Common;
using CShop.Site;
using CShop.Entity;

namespace CShop.Web.help
{
    public partial class detail : WebPage
    {
        /// <summary>
        /// 定义本页hashtable以供模板引擎使用
        /// </summary>
        protected IDictionary context = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = CShopRequest.getRequest("code");
            string tlp = "help";
            string catltp = "";
            #region 获取CMS信息
            CmsInfo cmsinfo = SiteBLL.GetCmsInfo(string.Format("urlrewriter='{0}'", code));
            if (cmsinfo == null)
            {
                cmsinfo = SiteBLL.GetCmsInfo(Utils.StrToInt(code, 0));
            }
            #endregion
            
            if (cmsinfo != null)
            {
                CmsCatInfo catmodel = SiteBLL.GetCmsCatInfo(cmsinfo.cat_id.Value);
                if (!string.IsNullOrEmpty(catmodel.info_tlp))
                {
                    tlp = catmodel.info_tlp;
                }
                if (!string.IsNullOrEmpty(cmsinfo.info_tlp))
                {
                    tlp = cmsinfo.info_tlp;
                }
                #region seo
                if (string.IsNullOrEmpty(cmsinfo.pagetitle))
                {
                    pagetitle = cmsinfo.title + "-" + config.Title;
                }
                else
                {
                    pagetitle = cmsinfo.pagetitle;
                }

                if (string.IsNullOrEmpty(cmsinfo.pagekeywords))
                {
                    pagekeywords = config.Keywords;
                }
                else
                {
                    pagekeywords = cmsinfo.pagekeywords;
                }

                if (string.IsNullOrEmpty(cmsinfo.pagedesc))
                {
                    pagedesc = config.Desc;
                }
                else
                {
                    pagedesc = cmsinfo.pagedesc;
                }
                #endregion

                catltp += "  &raquo; <a href='/cms/" + catmodel.cat_id.ToString() + ".html'>" + catmodel.cat_name + "</a>";
                context.Add("cmsinfo", cmsinfo);
                context.Add("titles", cmsinfo.title);
                context.Add("comment_type", 2);
                context.Add("id_value", cmsinfo.article_id);
                context.Add("cat_name", catmodel.cat_name);
                context.Add("catltp", catltp);
                //更新访问统计
                SiteBLL.UpdateCmsFieldValue("click_count", Convert.ToInt32(cmsinfo.click_count)+1, cmsinfo.article_id.Value);

                base.DisplayTemplate(context, tlp);
            }
            else  //页面不存在
            { 
            
            }
        }
    }
}
