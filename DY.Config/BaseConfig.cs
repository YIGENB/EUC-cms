using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Data;

using DY.Common;
using DY.Cache;
using DY.Data;
using DY.Entity;

namespace DY.Config
{
    /// <summary>
    /// 网站基本设置类
    /// </summary>
    public class BaseConfig
    {
        /// <summary>
        /// 前台模板路径
        /// </summary>
        public static readonly string WebSkinPath = ConfigurationManager.AppSettings["WebSkinPath"];
        /// <summary>
        /// 手机前台模板路径
        /// </summary>
        public static readonly string WapSkinPath = ConfigurationManager.AppSettings["WapSkinPath"];
        /// <summary>
        /// 后台模板路径
        /// </summary>
        public static readonly string AdminSkinPath = ConfigurationManager.AppSettings["AdminSkinPath"];
        /// <summary>
        /// 后台模板皮肤
        /// </summary>
        public static readonly string AdminSkin = ConfigurationManager.AppSettings["AdminSkin"];
        /// <summary>
        /// 后台UI路径
        /// </summary>
        public static readonly string AdminUIPath = ConfigurationManager.AppSettings["AdminUIPath"];
        /// <summary>
        /// 前台模板后缀
        /// </summary>
        public static readonly string WebSkinSuffix = ConfigurationManager.AppSettings["WebSkinSuffix"];
        /// <summary>
        /// 数据表前辍
        /// </summary>
        public static readonly string TablePrefix = ConfigurationManager.AppSettings["TablePrefix"];
        /// <summary>
        /// 身份验证cookie域 
        /// </summary>
        public static readonly string CookieDomain = ConfigurationManager.AppSettings["CookieDomain"];
        /// <summary>
        /// 本地升级XML文件名
        /// </summary>
        public static readonly string Update_client = ConfigurationManager.AppSettings["update_client"];
        /// <summary>
        /// 本地升级临时文件夹
        /// </summary>
        public static readonly string Upd_forder = ConfigurationManager.AppSettings["upd_forder"];
        /// <summary>
        /// 站点认证key
        /// </summary>
        public static readonly string SoftReg = ConfigurationManager.AppSettings["SoftReg"];
        /// <summary>
        /// 网站加密密钥
        /// </summary>
        public static readonly string WebEncrypt = "#$%^^@!0";
        /// <summary>
        /// cznn-OEM固定值（bdWEQdfU）
        /// </summary>
        public static readonly string Oem= "bdWEQdfU";
        /// <summary>
        /// cznn-OEM文件保存地址（）
        /// </summary>
        public static readonly string OemPath = "/config/cnzz/cnzz.inc";
        /// <summary>
        /// cznn-OEM服务商
        /// </summary>
        public static readonly string OemCms = ConfigurationManager.AppSettings["OemCms"];
        /// <summary>
        /// 临时文件夹
        /// </summary>
        public static readonly string TemporaryFolder = "/include/upload/~temp/";
        /// <summary>
        ///微信保存地址
        /// </summary>
        public static readonly string WxUserPath = "/config/weixin/user/";
        /// <summary>
        ///插件地址
        /// </summary>
        public static readonly string PlunginPath = ConfigurationManager.AppSettings["pluginPath"];
        /// <summary>
        ///日志文件保存地址
        /// </summary>
        public static readonly string LogPath = "/log/";
        /// <summary>
        /// 维护账户
        /// </summary>
        public static readonly string username = "易优创";
        /// <summary>
        /// 维护账户密码
        /// </summary>
        public static readonly string password = "@@@euc";

        private static readonly DYCache cache = DYCache.GetCacheService();

        /// <summary>
        /// 设置网站配置信息
        /// </summary>
        /// <returns></returns>
        public static BaseConfigInfo Get()
        {

            DbHelper.ConnectionString = BaseConfigs.GetBaseConfig().Dbconnectstring;

            BaseConfigInfo baseconfiginfo = new BaseConfigInfo();
            baseconfiginfo.SearchKeywords = GetBaseConfigInfo("search_keywords");
            baseconfiginfo.ShoppingTel = GetBaseConfigInfo("shopping_tel");
            baseconfiginfo.Sitedomain = GetBaseConfigInfo("sitedomain");
            baseconfiginfo.ShowGoodssn = Utils.StrToInt(GetBaseConfigInfo("show_goodssn"), 0) == 0?false :true;
            baseconfiginfo.ShowBrand = Utils.StrToInt(GetBaseConfigInfo("show_brand"), 0) == 0 ? false : true;
            baseconfiginfo.ShowGoodsweight = Utils.StrToInt(GetBaseConfigInfo("show_goodsweight"), 0) == 0 ? false : true;
            baseconfiginfo.ShowGoodsnumber = Utils.StrToInt(GetBaseConfigInfo("show_goodsnumber"), 0) == 0 ? false : true;
            baseconfiginfo.ShowAddtime = Utils.StrToInt(GetBaseConfigInfo("show_addtime"), 0) == 0 ? false : true;
            baseconfiginfo.ShowMarketprice = Utils.StrToInt(GetBaseConfigInfo("show_marketprice"), 0) == 0 ? false : true;
            baseconfiginfo.WatermarkGoods = Utils.StrToInt(GetBaseConfigInfo("watermark_goods"), 0) == 0 ? false : true;
            baseconfiginfo.WatermarkAlpha = GetBaseConfigInfo("watermark_alpha");
            baseconfiginfo.WatermarkPlace = Utils.StrToInt(GetBaseConfigInfo("watermark_place"), 0);
            baseconfiginfo.WatermarkPic = GetBaseConfigInfo("watermark_pic");
            baseconfiginfo.RegisterPoints = GetBaseConfigInfo("register_points");
            baseconfiginfo.BestRegInteger = GetBaseConfigInfo("best_reg_integer");
            baseconfiginfo.GoodsInteger = GetBaseConfigInfo("goods_integer");
            baseconfiginfo.ReviewInteger = GetBaseConfigInfo("review_integer");
            baseconfiginfo.Name = GetBaseConfigInfo("name");
            baseconfiginfo.Title = GetBaseConfigInfo("title");
            baseconfiginfo.Desc = GetBaseConfigInfo("desc");
            baseconfiginfo.Keywords = FunctionUtils.Text.ToDBC(GetBaseConfigInfo("keywords"));
            baseconfiginfo.ProTitle = GetBaseConfigInfo("pro_title");
            baseconfiginfo.ProDesc = GetBaseConfigInfo("pro_desc");
            baseconfiginfo.ProKeywords = FunctionUtils.Text.ToDBC(GetBaseConfigInfo("pro_keywords"));
            baseconfiginfo.ArticleTitle = GetBaseConfigInfo("article_title");
            baseconfiginfo.ArticleDesc = GetBaseConfigInfo("article_desc");
            baseconfiginfo.ArticleKeywords = FunctionUtils.Text.ToDBC(GetBaseConfigInfo("article_keywords"));
            baseconfiginfo.Copyright = GetBaseConfigInfo("copyright");
            baseconfiginfo.ShopLogo = GetBaseConfigInfo("shop_logo");
            baseconfiginfo.IcpNumber = GetBaseConfigInfo("icp_number");
            baseconfiginfo.IcpFile = GetBaseConfigInfo("icp_file");
            baseconfiginfo.MarketPriceRate = GetBaseConfigInfo("market_price_rate");
            baseconfiginfo.IntegralName = GetBaseConfigInfo("integral_name");
            baseconfiginfo.SnPrefix = GetBaseConfigInfo("sn_prefix");
            baseconfiginfo.CommentCheck = Utils.StrToInt(GetBaseConfigInfo("comment_check"), 0) == 0 ? false : true;

            baseconfiginfo.Top10Time = Utils.StrToInt(GetBaseConfigInfo("top10_time"), 0);
            baseconfiginfo.CommentValidator = Utils.StrToInt(GetBaseConfigInfo("comment_validator"), 0) == 0 ? false : true;
            baseconfiginfo.DefaultStorage = GetBaseConfigInfo("default_storage");
            baseconfiginfo.CanInvoice = Utils.StrToInt(GetBaseConfigInfo("can_invoice"), 0) == 0 ? false : true;
            baseconfiginfo.UseIntegral = Utils.StrToInt(GetBaseConfigInfo("use_integral"), 0) == 0 ? false : true;
            baseconfiginfo.UseSurplus = Utils.StrToInt(GetBaseConfigInfo("use_surplus"), 0) == 0 ? false : true;
            baseconfiginfo.UseHowOos = Utils.StrToInt(GetBaseConfigInfo("use_how_oos"), 0) == 0 ? false : true;
            baseconfiginfo.InvoiceContent = GetBaseConfigInfo("invoice_content");
            baseconfiginfo.AnonymousBuy = Utils.StrToInt(GetBaseConfigInfo("anonymous_buy"), 0) == 0 ? false : true;
            baseconfiginfo.MinGoodsAmount = GetBaseConfigInfo("min_goods_amount");
            baseconfiginfo.StockDelTime = Utils.StrToInt(GetBaseConfigInfo("stock_del_time"), 0);
            baseconfiginfo.TopNumber = Utils.StrToInt(GetBaseConfigInfo("top_number"), 0); 
            baseconfiginfo.HistoryNumber = GetBaseConfigInfo("history_number");
            baseconfiginfo.CommentsNumber = Utils.StrToInt(GetBaseConfigInfo("comments_number"),10);
            baseconfiginfo.SortOrderType = Utils.StrToInt(GetBaseConfigInfo("sort_order_type"), 0);
            baseconfiginfo.PageSize = Utils.StrToInt(GetBaseConfigInfo("page_size"),20);
            baseconfiginfo.ShowOrderType = Utils.StrToInt(GetBaseConfigInfo("show_order_type"), 0);
            baseconfiginfo.SmtpHost = GetBaseConfigInfo("smtp_host");
            baseconfiginfo.SmtpPort = GetBaseConfigInfo("smtp_port");
            baseconfiginfo.SmtpUser = GetBaseConfigInfo("smtp_user");
            baseconfiginfo.SmtpPass = GetBaseConfigInfo("smtp_pass");
            baseconfiginfo.SmtpMail = GetBaseConfigInfo("smtp_mail");
            baseconfiginfo.CaptchaEnble = GetBaseConfigInfo("captcha_enble");
            baseconfiginfo.CaptchaErrShow = Utils.StrToInt(GetBaseConfigInfo("captcha_err_show"), 0) == 0 ? false : true;
            baseconfiginfo.CaptchaWidth = Utils.StrToInt(GetBaseConfigInfo("captcha_width"),160);
            baseconfiginfo.CaptchaHeight = Utils.StrToInt(GetBaseConfigInfo("captcha_height"),60);
            baseconfiginfo.CaptchaBorder = GetBaseConfigInfo("captcha_border");
            baseconfiginfo.CaptchaBadPiont = GetBaseConfigInfo("captcha_BadPiont");
            baseconfiginfo.CaptchaStrukLineCount = GetBaseConfigInfo("captcha_StrukLineCount");
            baseconfiginfo.CaptchaPatternCount = GetBaseConfigInfo("captcha_PatternCount");
            baseconfiginfo.CaptchaLength = Utils.StrToInt(GetBaseConfigInfo("captcha_length"),4);
            baseconfiginfo.CaptchaUseNum = Utils.StrToInt(GetBaseConfigInfo("captcha_useNum"), 0) == 0 ? false : true;
            baseconfiginfo.CaptchaUseLow = Utils.StrToInt(GetBaseConfigInfo("captcha_useLow"), 0) == 0 ? false : true;
            baseconfiginfo.CaptchaUseUpp = Utils.StrToInt(GetBaseConfigInfo("captcha_useUpp"), 0) == 0 ? false : true;
            baseconfiginfo.CaptchaUseSpe = Utils.StrToInt(GetBaseConfigInfo("captcha_useSpe"), 0) == 0 ? false : true;
            baseconfiginfo.CaptchaCustom = GetBaseConfigInfo("captcha_custom");
            baseconfiginfo.PhotoNone = GetBaseConfigInfo("photo_none");
            baseconfiginfo.RegTiaokuan = GetBaseConfigInfo("reg_tiaokuan");
            baseconfiginfo.MinGoodsNum = GetBaseConfigInfo("min_goods_num");
            baseconfiginfo.SmsApi = GetBaseConfigInfo("sms_api");
            baseconfiginfo.UrlType = Utils.StrToInt(GetBaseConfigInfo("url_type"), 0);
            baseconfiginfo.GoodsPhotoSaveType = Utils.StrToInt(GetBaseConfigInfo("goods_photo_save_type"), 0);
            baseconfiginfo.UrlRewriter = Utils.StrToInt(GetBaseConfigInfo("UrlRewriter"), 0);
            baseconfiginfo.UrlRewriterKzm = GetBaseConfigInfo("UrlRewriter_kzm");
            baseconfiginfo.PriceName1 = GetBaseConfigInfo("price_name1");
            baseconfiginfo.PriceName2 = GetBaseConfigInfo("price_name2");
            baseconfiginfo.Admin_allowIp = GetBaseConfigInfo("admin_allowIp");
            baseconfiginfo.Web_allowIp = GetBaseConfigInfo("web_allowIp");
            baseconfiginfo.BanIp = GetBaseConfigInfo("banIp");


            baseconfiginfo.FootInfo = Utils.StrFormat(GetBaseConfigInfo("foot_info"));
            baseconfiginfo.FootKeywords = Utils.StrFormat(GetBaseConfigInfo("foot_keywords"));
            baseconfiginfo.CaptchaReg = baseconfiginfo.CaptchaEnble.IndexOf("0") >= 0 ? true : false;
            baseconfiginfo.CaptchaLogin = baseconfiginfo.CaptchaEnble.IndexOf("1") >= 0 ? true : false;
            baseconfiginfo.CaptchaReview = baseconfiginfo.CaptchaEnble.IndexOf("2") >= 0 ? true : false;
            baseconfiginfo.EnableHtml = Utils.StrToInt(GetBaseConfigInfo("enable_html"), 0) == 0 ? false : true;
            baseconfiginfo.ServiceCode = GetBaseConfigInfo("service_code");
            baseconfiginfo.GoodsImgIco = GetBaseConfigInfo("goods_img_ico");
            baseconfiginfo.GoodsImgInfo = GetBaseConfigInfo("goods_img_info");
            baseconfiginfo.GoodsImgList = GetBaseConfigInfo("goods_img_list");
            baseconfiginfo.Email = GetBaseConfigInfo("email");
            baseconfiginfo.Qqzx = GetBaseConfigInfo("qqzx");
            baseconfiginfo.Is_BaiduPing = Utils.StrToInt(GetBaseConfigInfo("is_BaiduPing"), 0) == 0 ? false : true;
            baseconfiginfo.Visualization = Utils.StrToInt(GetBaseConfigInfo("visualization"), 0) == 0 ? false : true;
            baseconfiginfo.MobileLogo = GetBaseConfigInfo("mobile_logo");
            baseconfiginfo.Is_cnzz = Utils.StrToInt(GetBaseConfigInfo("is_cnzz"), 0) == 0 ? false : true;
            baseconfiginfo.Is_hotcity = Utils.StrToInt(GetBaseConfigInfo("is_hotcity"), 0) == 0 ? false : true;

            baseconfiginfo.Feedback_customer = Utils.StrToInt(GetBaseConfigInfo("feedback_customer"), 0);
            baseconfiginfo.Email_customer = Utils.StrToInt(GetBaseConfigInfo("email_customer"), 0);
            baseconfiginfo.Vote_customer = Utils.StrToInt(GetBaseConfigInfo("vote_customer"), 0);
            baseconfiginfo.Reg_shenhe = Utils.StrToInt(GetBaseConfigInfo("Reg_shenhe"), 0) == 0 ? false : true;

            baseconfiginfo.Customer_seeting_one = GetBaseConfigInfo("customer_seeting_one");
            baseconfiginfo.Customer_count = GetBaseConfigInfo("customer_count");
            baseconfiginfo.Phone = GetBaseConfigInfo("phone");
            baseconfiginfo.Site_word = Utils.StrToInt(GetBaseConfigInfo("site_word"), 0) == 0 ? false : true;
            baseconfiginfo.Participle_word = Utils.StrToInt(GetBaseConfigInfo("participle_word"), 0) == 0 ? false : true;
            baseconfiginfo.Word_count = Utils.StrToInt(GetBaseConfigInfo("word_count"), 0);
            baseconfiginfo.Short_name = GetBaseConfigInfo("short_name");
            baseconfiginfo.Secret = GetBaseConfigInfo("secret");
            baseconfiginfo.Search_logo = GetBaseConfigInfo("search_logo");
            baseconfiginfo.Is_oauth = Utils.StrToInt(GetBaseConfigInfo("is_oauth"), 0) == 0 ? false : true;

            baseconfiginfo.Users_sign = Utils.StrToInt(GetBaseConfigInfo("users_sign"), 0);
            baseconfiginfo.Withdrawals = Utils.StrToInt(GetBaseConfigInfo("withdrawals"), 0);

            //自动生成统计代码
            baseconfiginfo.StatsCode = GetBaseConfigInfo("stats_code");

            //URL重写自动生成模块
            baseconfiginfo.Url_pinyin_type = Utils.StrToInt(GetBaseConfigInfo("url_pinyin_type"), 0);
            baseconfiginfo.Url_pinyin_sublength = Utils.StrToInt(GetBaseConfigInfo("url_pinyin_sublength"), 0);
            baseconfiginfo.Url_auto = Utils.StrToInt(GetBaseConfigInfo("url_auto"), 0) == 0 ? false : true;

            baseconfiginfo.Integral_into_total = GetBaseConfigInfo("integral_into_total");
            baseconfiginfo.Into_cash = GetBaseConfigInfo("into_cash");

            //留言模块
            baseconfiginfo.Keywords_shielding = GetBaseConfigInfo("keywords_shielding");
            baseconfiginfo.Mailalert = Utils.StrToInt(GetBaseConfigInfo("mailalert"), 0) == 0 ? false : true;
            baseconfiginfo.Ip_count = Utils.StrToInt(GetBaseConfigInfo("ip_count"), 0);

            baseconfiginfo.Zzuser = GetBaseConfigInfo("zzuser");
            baseconfiginfo.Zzpwd = GetBaseConfigInfo("zzpwd");
            baseconfiginfo.Zzsoftid = GetBaseConfigInfo("zzsoftid");
            baseconfiginfo.Zzsoftkey = GetBaseConfigInfo("zzsoftkey");

            baseconfiginfo.Autocms_num = Utils.StrToInt(GetBaseConfigInfo("autocms_num"), 0);

            return baseconfiginfo;
        }

        public static string GetBaseConfigInfo(string code)
        {
            System.Data.DataTable data = cache.RetrieveObject(CacheKeys.网站设置) as DataTable;
            string val = "";

            if (data == null)
            {
                data = DatabaseProvider.GetInstance().GetConfigList("parent_id>0");

                cache.AddObject(CacheKeys.网站设置, data);
            }

            foreach (DataRow dr in data.Select("code='" + code + "'"))
            {
                val = dr["value"].ToString();
                break;
            }

            return val;
        }

    }
}
