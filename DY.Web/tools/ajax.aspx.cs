using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.tools
{
    public partial class ajax : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (base.act)
            {
                case "InsertEmail":
                    if (ispost)
                        InsertEmail();
                    break;
                case "GetReviewList":
                    GetReviewList();
                    break;
                case "GetNewsList":
                    GetNewsList();
                    break;
                case "GetGoodsList":
                    GetGoodsList();
                    break;
                case "GetSearchList":
                    GetSearchList();
                    break;
                case "AddReview":
                    if (ispost)
                        AddReview();
                    break;
                case "AddFavorite":
                    AddFavorite();
                    break;
                case "deleteFavorite":
                    DeleteFavorite();
                    break;
                case "feedbook":
                    feedbook();
                    break;
                case "feedbooknew":
                    feedbooknew();
                    break;
                case "feedbook2":
                    feedbook2();
                    break;
                case "get_cart":
                    Get_Cart();
                    break;
                case "getgoodscat":
                    GetGoodsCat();
                    break;
                case "islogin":
                    IsLogin();
                    break;
                case "InsertGrab":
                    InsertGrab();
                    break;

                case "GetReviewList2":
                    GetReviewList2();
                    break;

                case "CrackImg":
                    CrackImg();
                    break;

                case "InsertEmail2":
                    if (ispost)
                        InsertEmail2();
                    break;
                case "InsertEmail3":
                    if (ispost)
                        InsertEmail3();
                    break;
                case "InsertEmail4":
                    if (ispost)
                        InsertEmail4();
                    break;
                case "InsertEmailSlasher":
                    if (ispost)
                        InsertEmailSlasher();
                    break;
                case "ggcard": GgCard(); break;
                case "exchange": Exchange(); break;
                case "InsertPromotion": InsertPromotion(); break;
                case "checkin": Checkin(); break;
                case "withdrawals": Withdrawals(); break;
                case "sendsms": PhoneSendSMS(); break;
                //case "createMenu": CreateMenu(); break;
            }
        }

        /// <summary>
        /// 破解防盗链图片
        /// </summary>
        protected void CrackImg()
        {
            System.Net.HttpWebRequest myrequest = System.Net.HttpWebRequest.Create(DYRequest.getRequest("imgurl")) as System.Net.HttpWebRequest;

            myrequest.Referer = "http://xw.qq.com/news/";
            System.Net.WebResponse myresponse = myrequest.GetResponse();
            System.IO.Stream imgstream = myresponse.GetResponseStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(imgstream);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            Response.ContentType = "image/Jpeg";
            Response.BinaryWrite(ms.ToArray());
            ms.Dispose();

            //base.DisplayMemoryTemplate(base.MakeJson("", 1, SendSMS.HttpPostSMS(smsinfo)));

        }

        /// <summary>
        /// 手机验证码
        /// </summary>
        protected void PhoneSendSMS()
        {

            string rnd = VerifyImage.GetRnd(6, true, false, false, false, config.CaptchaCustom);

            string cont = DYRequest.getForm("des")+"检验码： " + rnd + " ,【请勿向任何人提供您收到的短信校验码】";
            string phone = DYRequest.getForm("phone");
            SMSInfo smsinfo = new SMSInfo();
            smsinfo.Mobiles = phone;
            smsinfo.Content = cont;
            Session["DYSMS"] = rnd;
            base.DisplayMemoryTemplate(base.MakeJson("", 1, SendSMS.HttpPostSMS(smsinfo)));
            
        }

        /// <summary>
        /// 提现记录
        /// </summary>
        protected void Withdrawals()
        {
            if (base.userid <= 0)
                return;
            int error = 0;
            UserWithdrawalsInfo entity = new UserWithdrawalsInfo();

            entity.user_id =base.userid;
            entity.date = DateTime.Now;
            entity.name = DYRequest.getForm("name");
            entity.des = DYRequest.getForm("des");
            entity.blank = DYRequest.getForm("blank");
            entity.money = DYRequest.getFormInt("money");
            entity.admin_id = 0;
            entity.admin_user = "";
            entity.des = "";
            entity.state = 0;
            entity.account = DYRequest.getForm("alipay_account");

            int id = SiteBLL.InsertUserWithdrawalsInfo(entity);
            if (id > 0)
            {
                if (entity.state == 0)
                {
                    UsersInfo user = SiteBLL.GetUsersInfo(entity.user_id.Value);
                    user.user_money -= entity.money;
                    user.frozen_money += entity.money;
                    SiteBLL.UpdateUsersInfo(user);
                    error = 1;
                }
            }

            base.DisplayMemoryTemplate(base.MakeJson("", error, "提交成功！"));
        }


        /// <summary>
        /// 签到
        /// </summary>
        protected void Checkin()
        {
            int error =0;
            UserSignInfo sign = new UserSignInfo();
            

            sign.userid = DYRequest.getFormInt("userid");
            sign.date = DateTime.Now;
            sign.ip = Utils.GetIP();
            sign.des = DYRequest.getForm("des");
            sign.points_type = DYRequest.getFormInt("points_type");
            sign.change = DYRequest.getFormInt("change");

            int id=SiteBLL.InsertUserSignInfo(sign);
            if (id > 0)
            {
                if (sign.points_type == 0)
                {
                    UsersInfo user = SiteBLL.GetUsersInfo(sign.userid.Value);
                    user.pay_points += sign.change;
                    SiteBLL.UpdateUsersInfo(user);
                    error = 1;
                }
                else if (sign.points_type == 1)
                {
                    UsersInfo user = SiteBLL.GetUsersInfo(sign.userid.Value);
                    user.rank_points += sign.change;
                    SiteBLL.UpdateUsersInfo(user);
                    error = 1;
                }
            }

            base.DisplayMemoryTemplate(base.MakeJson("", error, "提交成功！"));
        }


        /// <summary>
        /// 更改奖品库
        /// </summary>
        protected void Exchange()
        {
            ExchangeInfo moble = new ExchangeInfo();
            moble.phone = DYRequest.getForm("phone");
            moble.user_id = DYRequest.getForm("name");
            moble.state = 1;
            moble.date = DateTime.Now;
            moble.exchange_id = DYRequest.getFormInt("exchangeid");

            SiteBLL.UpdateExchangeInfo(moble);
        }
        //public void CreateMenu(string token, DY.Weixin.MP.GetMenuResultFull resultFull)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    try
        //    {
        //        //重新整理按钮信息
        //        var bg = DY.Weixin.MP.CommonAPIs.CommonApi.GetMenuFromJsonResult(resultFull).menu;
        //        var result = DY.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(token, bg);
        //        var json = new
        //        {
        //            Success = result.errmsg == "ok",
        //            Message = result.errmsg
        //        };
        //        Response.Write(jss.Serialize(json));
        //    }
        //    catch (Exception ex)
        //    {
        //        var json = new { Success = false, Message = ex.Message };
        //        Response.Write(jss.Serialize(json));
        //    }
        //}

        protected void InsertGrab()
        {
            Grab2Info moble = new Grab2Info();
            moble.companyname = DYRequest.getForm("companyname");
            moble.companyurl = DYRequest.getForm("companyurl");
            moble.companyaddress = DYRequest.getForm("companyaddress");
            moble.companyscale = DYRequest.getFormInt("companyscale");
            moble.predictcount = DYRequest.getFormInt("predictcount");
            moble.monthcount = DYRequest.getFormInt("monthcount");
            moble.yearcount = DYRequest.getFormInt("yearcount");
            moble.askfor = DYRequest.getForm("askfor");
            moble.photo = DYRequest.getForm("photo");
            moble.monthlyConsumption = DYRequest.getForm("monthlyConsumption");
            SiteBLL.InsertGrabInfo(moble);
            base.DisplayMemoryTemplate(base.MakeJson("", 0, "提交成功！"));
        }
        /// <summary>
        /// 
        /// </summary>
        protected void GgCard()
        {
            string card_num = DYRequest.getRequest("card_num");
            if (string.IsNullOrEmpty(card_num))
            {
                base.DisplayJsonMessage("隐藏码不能为空！");
            }
            else
            {
                CardInfo cardinfo = SiteBLL.GetCardInfo("is_validated=0 and card_num='" + card_num + "'");

                if (cardinfo != null)
                {
                    CardInfo card = DY.Site.SiteBLL.GetCardInfo("user_id='" + userid + "'");
                    if (card != null)
                    {
                        base.DisplayJsonMessage(0, "抱歉，你在" + card.use_time + "已经使用过隐藏码" + card.card_num);
                    }
                    else
                    {
                        try
                        {
                            BonusTypeInfo bonustype = SiteBLL.GetBonusTypeInfo("type_name='注册送礼' and getdate() between send_start_date and send_end_date");
                            if (bonustype != null)
                            {
                                for (int i = 0; i < 12; i++)
                                {
                                    //生成优惠券序列号
                                    object val = SiteBLL.GetBonusValue("MAX(bonus_sn)", "bonus_type_id=" + bonustype.type_id);

                                    double num = !string.IsNullOrEmpty(val.ToString()) ? Math.Floor(Convert.ToDouble(val) / 10000) : 100000;

                                    Random rnd = new Random();

                                    //向会员优惠券表加入数据
                                    string bonus_sn = (num + i) + rnd.Next(0, 9999).ToString().PadLeft(4, '0');

                                    BonusInfo bonusinfo = new BonusInfo();
                                    bonusinfo.bonus_sn = Convert.ToInt32(bonus_sn);
                                    bonusinfo.user_id = userid;
                                    bonusinfo.user_name = username;
                                    bonusinfo.emailed = 0;
                                    bonusinfo.bonus_type_id = bonustype.type_id;
                                    bonusinfo.is_enbled = true;
                                    bonusinfo.use_start_date = DateTime.Now.AddMonths(i);
                                    bonusinfo.use_end_date = DateTime.Now.AddMonths(i + 1);

                                    SiteBLL.InsertBonusInfo(bonusinfo);
                                }
                            }
                            cardinfo.user_id = userid;
                            cardinfo.is_validated = true;
                            cardinfo.use_time = DateTime.Now;
                            SiteBLL.UpdateCardInfo(cardinfo);
                        }
                        catch 
                        {
                            base.DisplayJsonMessage(0, "发生未知错误，请于客服联系");
                        }
                        base.DisplayJsonMessage(0, "该隐藏码兑换成功，请在我的优惠券中查看");
                    }
                }
                else
                {
                    base.DisplayJsonMessage("该隐藏码" + card_num + "无效！");


                }
            }

        }

        protected void GetGoodsCat()
        {
            int cat_id = DYRequest.getRequestInt("cat_id");
            Caches c = new Caches();
            System.Data.DataTable goods_cat = c.GoodsCat(cat_id, false);

            IDictionary context = new Hashtable();
            context.Add("list", goods_cat);
            base.DisplayTemplate(context, "jxjy/goods_cat");
        }

        /// <summary>
        /// 登录信息
        /// </summary>
        protected void IsLogin()
        {
            base.DisplayMemoryTemplate(base.MakeJson(base.userid.ToString(), 0, base.username));
        }

        protected void Get_Cart()
        {
            //购物车产品数
            int cart_count_goods = 0;
            //购物车总金额
            decimal cart_count_price = 0.00m;
            ArrayList cart_list = Store.GetCartList();
            if (cart_list.Count > 0)
            {
                foreach (CartInfo model in cart_list)
                {
                    cart_count_goods += (int)model.goods_number;
                    cart_count_price += (decimal)model.goods_number * (decimal)model.goods_price;
                }
            }
            base.DisplayMemoryTemplate(base.MakeJson(cart_count_goods.ToString(), 0, cart_count_price.ToString()));
        }

        /// <summary>
        /// 添加邮件订阅
        /// </summary>
        protected void InsertEmail()
        {
            EmailListInfo emailinfo = new EmailListInfo();
            emailinfo.email = DYRequest.getForm("email");
            emailinfo.city = DYRequest.getForm("city");
            emailinfo.hash = DYRequest.getForm("hash"); //临时更改为邮件标题
            emailinfo.mobile = DYRequest.getForm("mobile");
            emailinfo.nickname = DYRequest.getForm("name");
            emailinfo.url = DYRequest.getForm("url");
            emailinfo.stat = false;
            emailinfo.remark = Utils.RemoveHtml(DYRequest.getForm("remark"));
            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckEmailInfo(emailinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion
            //emailinfo.type = 1;
            SiteBLL.InsertEmailListInfo(emailinfo);

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }
        /// <summary>
        /// 添加邮件订阅
        /// </summary>
        protected void InsertEmail2()
        {
            EmailListInfo emailinfo = new EmailListInfo();
            emailinfo.email = DYRequest.getForm("email1");
            emailinfo.hash = DYRequest.getForm("city1");
            emailinfo.mobile = DYRequest.getForm("tel1");
            emailinfo.nickname = DYRequest.getForm("name1");
            emailinfo.stat = false;
            emailinfo.remark = Utils.RemoveHtml(DYRequest.getForm("remark1"));
            int type = DYRequest.getFormInt("type1", 0);
            emailinfo.type = type == 0 ? 1 : type;

            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckEmailInfo(emailinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion

            SiteBLL.InsertEmailListInfo(emailinfo);

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }
        /// <summary>
        /// 添加邮件订阅
        /// </summary>
        protected void InsertEmail3()
        {
            EmailListInfo emailinfo = new EmailListInfo();
            emailinfo.email = DYRequest.getForm("email3");
            emailinfo.hash = DYRequest.getForm("hash3");
            emailinfo.mobile = DYRequest.getForm("tel3");
            emailinfo.nickname = DYRequest.getForm("name3");
            emailinfo.stat = false;
            emailinfo.remark = Utils.RemoveHtml(DYRequest.getForm("remark3"));
            emailinfo.type = 1;

            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckEmailInfo(emailinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion
            SiteBLL.InsertEmailListInfo(emailinfo);

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }
        /// <summary>
        /// 添加邮件订阅
        /// </summary>
        protected void InsertEmail4()
        {
            EmailListInfo emailinfo = new EmailListInfo();
            emailinfo.email = DYRequest.getForm("email4");
            emailinfo.hash = DYRequest.getForm("hash4");
            emailinfo.mobile = DYRequest.getForm("tel4");
            emailinfo.nickname = DYRequest.getForm("name4");
            emailinfo.stat = false;
            emailinfo.remark = Utils.RemoveHtml(DYRequest.getForm("remark4"));
            emailinfo.type = 1;

            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckEmailInfo(emailinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion
            SiteBLL.InsertEmailListInfo(emailinfo);

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }

        /// <summary>
        /// 用户发送邮件
        /// </summary>
        protected void InsertEmailSlasher()
        {
            EmailListInfo emailinfo = new EmailListInfo();
            emailinfo.email = DYRequest.getForm("contactEmailField");
            emailinfo.hash = DYRequest.getForm("contactEmailTitleField");
            emailinfo.mobile = DYRequest.getForm("tel4");
            emailinfo.nickname = DYRequest.getForm("name4");
            emailinfo.stat = false;
            emailinfo.remark = Utils.RemoveHtml(DYRequest.getForm("contactMessageTextarea"));
            emailinfo.type = 1;
            SiteBLL.InsertEmailListInfo(emailinfo);

            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckEmailInfo(emailinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }

        /// <summary>
        /// 添加邮件订阅
        /// </summary>
        protected void feedbooknew()
        {
            FeedbackInfo feedbackinfo = new FeedbackInfo();
            feedbackinfo.is_show = false;
            feedbackinfo.msg_content = Utils.RemoveHtml(DYRequest.getForm("contactMessageTextarea"));
            feedbackinfo.msg_file = DYRequest.getForm("tel");
            feedbackinfo.msg_time = DateTime.Now;
            feedbackinfo.msg_title = DYRequest.getForm("hash");
            feedbackinfo.msg_type = DYRequest.getFormInt("type");
            feedbackinfo.order_id = 0;
            feedbackinfo.parent_id = 0;
            feedbackinfo.user_email = DYRequest.getForm("contactEmailField");
            feedbackinfo.user_id = base.userid;
            feedbackinfo.user_name = DYRequest.getForm("contactNameField");

            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckFeedback(feedbackinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion

            SiteBLL.InsertFeedbackInfo(feedbackinfo);

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }


        /// <summary>
        /// 添加邮件订阅
        /// </summary>
        protected void feedbook()
        {
            FeedbackInfo feedbackinfo = new FeedbackInfo();
            feedbackinfo.is_show = false;
            feedbackinfo.msg_content = Utils.RemoveHtml(DYRequest.getForm("remark"));
            feedbackinfo.msg_file = DYRequest.getForm("tel");
            feedbackinfo.msg_time = DateTime.Now;
            feedbackinfo.msg_title = DYRequest.getForm("hash");
            feedbackinfo.msg_type = DYRequest.getFormInt("type");
            feedbackinfo.order_id = 0;
            feedbackinfo.parent_id = 0;
            feedbackinfo.user_email = DYRequest.getForm("email");
            feedbackinfo.user_id = base.userid;
            feedbackinfo.user_name = DYRequest.getForm("name");


            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckFeedback(feedbackinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的信息已经递交成功，请等待我们的回复。"));
        }

        /// <summary>
        /// 商品预订
        /// </summary>
        protected void feedbook2()
        {
            FeedbackInfo feedbackinfo = new FeedbackInfo();
            feedbackinfo.is_show = false;
            feedbackinfo.msg_content = Utils.RemoveHtml(DYRequest.getForm("msg_content"));
            feedbackinfo.msg_file = DYRequest.getForm("user_tel");
            feedbackinfo.msg_time = DateTime.Now;
            feedbackinfo.msg_title = DYRequest.getForm("dg_num");//订购数量
            feedbackinfo.msg_type = 2;//商品预订
            feedbackinfo.order_id = 0;
            feedbackinfo.parent_id = 0;
            feedbackinfo.user_email = DYRequest.getForm("user_email");
            feedbackinfo.user_id = 0;
            feedbackinfo.user_name = DYRequest.getForm("user_name");

            #region 处理信息
            IDictionary context = new Hashtable();
            string mailbody = base.GetTemplate(context, "tlp/email/sendtoadmin", DY.Config.BaseConfig.WebSkinPath, false);
            string msg = SiteUtils.CheckFeedback(feedbackinfo, mailbody);

            if (!string.IsNullOrEmpty(msg))
            {
                base.DisplayMemoryTemplate(base.MakeJson("", 0, msg));
                return;
            }
            #endregion

            SiteBLL.InsertFeedbackInfo(feedbackinfo);

            base.DisplayMemoryTemplate(base.MakeJson("", 0, "恭喜，您的预订信息已经递交成功，请等待我们的回复。"));
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        protected void GetReviewList()
        {
            int type = DYRequest.getRequestInt("comment_type");
            IDictionary context = new Hashtable();
            context.Add("reviewlist", SiteBLL.GetCommentList(DYRequest.getRequestInt("pageindex"),
                config.CommentsNumber, "comment_id desc",
                "parent_id=0 and comment_type=" + type + " and enabled=1 and id_value=" + DYRequest.getRequestInt("id_value") + "",
                out base.ResultCount));

            context.Add("pager", Utils.GetAjaxPageNumbers(DYRequest.getRequestInt("pageindex"), base.ResultCount, config.CommentsNumber,
                "javascript:LoadReviewList(", ");", 5));
            context.Add("reviewcount", base.ResultCount);
            context.Add("type", "list");
            base.DisplayTemplate(context, "public/review");
        }

        /// <summary>
        /// 获取资讯列表
        /// </summary>
        protected void GetNewsList()
        {
            IDictionary context = new Hashtable();
            base.pageindex = DYRequest.getRequestInt("pageindex");
            int cat_id = DYRequest.getRequestInt("cat_id");
            int pagesize = DYRequest.getRequestInt("pagesize");
            int is_mobile = DYRequest.getRequestInt("is_mobile");
            string tlp = DYRequest.getRequest("tlp");
            string filter = "article_id > 0";
            filter = "cat_id in (" + cms.GetCMSCatIds(cat_id) + ")";
            filter += " and is_show=1 and showtime<=getdate()";
            if (is_mobile > 0)
                filter += " and is_mobile=" + is_mobile;
            ArrayList list = SiteBLL.GetCmsList(base.pageindex, pagesize, SiteUtils.GetSortOrder("is_top desc,sort_order desc,article_id desc,showtime desc"), filter, out base.ResultCount);
            //string getstr = "";
            foreach (CmsInfo row in list)
            {
                //string cmsLink = config.EnableHtml ? "/html/n_detail/" + row.article_id.Value + ".html" : "/n_detail/" + row.article_id.Value + ".htm";
                //getstr += "<li class='active'><a href='" + cmsLink + "' title='" + row.title + "'><div class='js_gotoNewsDetail' data-id='143'><h3 class='newsTitle'><em class='icon'></em><span class='name'>" + row.title + "</span></h3><p class='time'>" + row.showtime.Value.ToString("yyyy-MM-dd hh:mm") + "</p></div></a></li>";
            }
            context.Add("list",list);
            base.DisplayTemplate(context, "more/" + tlp + "_list");
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        protected void GetGoodsList()
        {
            IDictionary context = new Hashtable();
            base.pageindex = DYRequest.getRequestInt("pageindex");
            int cat_id = DYRequest.getRequestInt("cat_id");
            int pagesize = DYRequest.getRequestInt("pagesize");
            int is_mobile = DYRequest.getRequestInt("is_mobile");
            string filter = "goods_id > 0";
            filter = "cat_id in (" + goods.GetGoodsCatIds(cat_id) + ")";
            filter += " and is_specials=1 and add_time<=getdate()";
            if (is_mobile > 0)
                filter += " and is_mobile=" + is_mobile;
            ArrayList list = SiteBLL.GetGoodsList(base.pageindex, pagesize, SiteUtils.GetSortOrder("sort_order desc,is_new desc,goods_id desc"), filter, out base.ResultCount);
            //string getstr = "";
            foreach (GoodsInfo row in list)
            {
                //string goodsLink = config.EnableHtml ? "/html/goods/detail/" + row.goods_id.Value + ".html" : "/goods/detail/" + row.goods_id.Value + ".htm";
                //getstr += "<li class='active'><div class='js_gotoNewsDetail' data-id='143'><h3 class='newsTitle'><em class='icon'></em><span class='name'><a href='" + goodsLink + "' title='" + row.goods_name + "'>" + row.goods_name + "</a></span></h3><p class='time'>" + row.goods_sn + "</p></div></li>";
                //getstr += "<dl class=\"rows\"><dd class=\"active\"><div alt=\"\" data-otherlink=\"0\" data-phref=\"" + goodsLink + "\" class=\"productatag\"><div class=\"imgWrap\"><a href=\"" + goodsLink + "\"><img src=\"" + row.goods_img + "\"></a></div><div class=\"infoWrap\"><h3 class=\"productTitle\"><span class=\"name\"><a href=\"" + goodsLink + "\">" + row.goods_name + "</a></span></h3><p class=\"code\">" + row.goods_sn + "</p><p class=\"mark\"></p></div></div></dd></dl>";
            }
            context.Add("list", list);
            base.DisplayTemplate(context, "more/goods_list");
        }
        /// <summary>
        /// 获取搜索列表
        /// </summary>
        protected void GetSearchList()
        {
            IDictionary context = new Hashtable();
            base.pageindex = DYRequest.getRequestInt("pageindex");
            string keyword = DYRequest.getRequest("keyword");
            int pagesize = DYRequest.getRequestInt("pagesize");
            string url = "";
            string filter = "is_delete=0";
            if (!string.IsNullOrEmpty(keyword))
            {
                filter += " and (title like '%" + keyword + "%')";
                url = "/search/" + keyword + "/";
                //标签库
                TagInfo taginfo = SiteBLL.GetTagInfo(string.Format("urlrewriter='{0}'", keyword));
                if (taginfo != null)
                {
                    filter += " or (tag like '%" + taginfo.tag_name + "%')";
                }
            }
            ArrayList list = SiteBLL.GetSearchList(base.pageindex, pagesize, SiteUtils.GetSortOrder("date desc"), filter, out base.ResultCount);
            context.Add("list", list);
            context.Add("keyword", keyword);
            base.DisplayTemplate(context, "search_list", "/static/template", false);
        }
        /// <summary>
        /// 获取评论列表
        /// </summary>
        protected void GetReviewList2()
        {
            int type = DYRequest.getRequestInt("comment_type");
            IDictionary context = new Hashtable();
            context.Add("reviewlist2", SiteBLL.GetCommentList(DYRequest.getRequestInt("pageindex"),
                20, "comment_id desc",
                "parent_id=0 and comment_type=" + type + " and enabled=1 and id_value=" + DYRequest.getRequestInt("id_value") + "",
                out base.ResultCount));
            context.Add("type", "list");
            base.DisplayTemplate(context, "public/review2");
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        protected void AddReview()
        {
            if (ispost)
            {
                //UsersInfo userinfo = new UsersInfo();


                string message = "", captcha = DYRequest.getForm("captcha");
                //if (base.userid <= 0)
                //{
                //    message = "请先登录，才能提交评论！";
                //}
                //else
                //{
                //    userinfo = SiteBLL.GetUsersInfo(base.userid);
                //}

                if (config.CaptchaReview)
                {
                    if (Session["DYCaptcha"] == null)
                        message = "验证码不存在，请刷新验证码";

                    if (string.IsNullOrEmpty(captcha) && config.CaptchaLogin == true)
                        message = "请输入验证码";

                    if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
                        message = "你输入的验证码与系统产生的不一致";
                }

                if (!string.IsNullOrEmpty(message))
                    base.DisplayMemoryTemplate(base.MakeJson("login", 1, message));
                else
                {
                    CommentInfo commentinfo = new CommentInfo();
                    commentinfo.add_time = DateTime.Now;
                    commentinfo.comment_rank = 0;
                    commentinfo.comment_type = DYRequest.getFormInt("comment_type");
                    commentinfo.content = Utils.RemoveHtml(DYRequest.getFormString("review_content"));
                    commentinfo.email = Utils.RemoveHtml(DYRequest.getFormString("review_email"));
                    commentinfo.enabled = false;
                    commentinfo.id_value = DYRequest.getFormInt("id_value");
                    commentinfo.ip_address = Utils.GetIP();
                    commentinfo.is_read = false;
                    commentinfo.parent_id = 0;
                    //commentinfo.user_id = userinfo.user_id;
                    commentinfo.user_name = DYRequest.getForm("review_name");//userinfo.user_name;
                    commentinfo.url = HttpContext.Current.Request.UrlReferrer.ToString();
                    commentinfo.is_recomm = false;
                    SiteBLL.InsertCommentInfo(commentinfo);

                    if (config.CaptchaReview)
                    {
                        Session["DYCaptcha"] = null; //请空验证码
                    }

                    base.DisplayMemoryTemplate(base.MakeJson("", 0, "您的评论已经成功添加,请等待我们的审核！"));
                }
            }
        }

        ///// <summary>
        ///// 添加评论
        ///// </summary>
        //protected void AddVote()
        //{
        //    if (ispost)
        //    {
        //        UsersInfo userinfo = new UsersInfo();


        //        string message = "", captcha = DYRequest.getForm("captcha");
        //        if (base.userid <= 0)
        //        {
        //            message = "请先登录，才能提交评论！";
        //        }
        //        else
        //        {
        //            userinfo = SiteBLL.GetUsersInfo(base.userid);
        //        }

        //        //if (config.CaptchaReview)
        //        //{
        //        //    if (Session["DYCaptcha"] == null)
        //        //        message = "验证码不存在，请刷新验证码";

        //        //    if (string.IsNullOrEmpty(captcha) && config.CaptchaLogin == true)
        //        //        message = "请输入验证码";

        //        //    if (captcha.ToLower() != Session["DYCaptcha"].ToString().ToLower())
        //        //        message = "你输入的验证码与系统产生的不一致";
        //        //}

        //        if (!string.IsNullOrEmpty(message))
        //            base.DisplayMemoryTemplate(base.MakeJson("login", 1, message));
        //        else
        //        {
        //            string ids = Request.Form["option_id"];

        //            int option_id = DYRequest.getFormInt("option_id");
        //            if (option_id > 0)
        //            {
        //                ids = option_id.ToString();
        //            }


        //            VoteLogInfo voteloginfo = new VoteLogInfo();
        //            voteloginfo.vote_id = DYRequest.getFormInt("vote_id");
        //            voteloginfo.ip_address = Utils.GetIP() + "|" + base.userid;
        //            voteloginfo.vote_time = DateTime.Now;

        //            if (voteloginfo.vote_id != 0)
        //            {

        //                SiteBLL.UpdateOption_count(ids, voteloginfo.vote_id.Value);
        //                SiteBLL.InsertVoteLogInfo(voteloginfo);
        //                base.DisplayMemoryTemplate(base.MakeJson("", 0, "投票成功！感谢你的支持"));
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 添加到收藏夹
        /// </summary>
        protected void AddFavorite()
        {
            if (ispost)
            {
                if (base.userid <= 0)
                    base.DisplayMemoryTemplate(base.MakeJson("", 1, "请先登录"));

                int id = Goods.SaveFavorites(base.userid, DYRequest.getFormInt("goods_id"));

                string message = "";
                if (id > 0)
                    message = "产品已成功添加到收藏夹，您可以在用户中心查看";
                else
                    message = "您要收藏的产品已经存在，不需重复添加";

                base.DisplayMemoryTemplate(base.MakeJson("", 1, message));
            }
        }
        /// <summary>
        /// 删除收藏夹
        /// </summary>
        protected void DeleteFavorite()
        {
            Goods.DeleteFavorites(base.userid, DYRequest.getFormInt("id"));

            base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
        }

        /// <summary>
        ///添加推广统计
        /// </summary>
        protected void InsertPromotion()
        {
            int pid = DYRequest.getFormInt("pid");
            string source = DYRequest.getFormString("source");
            string url = DYRequest.getFormString("url");
            if (pid > 0)
            {
                Entity.PromotionLogInfo model = new PromotionLogInfo();
                model.input_time = DateTime.Now;
                model.ip = Utils.GetIP();
                model.pid = pid;
                model.source = source;
                model.website = url;
                SiteBLL.InsertPromotionLogInfo(model);

                //写入cookie
                HttpCookie cookie = new HttpCookie("DYPromotion");
                cookie.Values["pid"] = pid.ToString();
                cookie.Expires = DateTime.Now.AddYears(1);

                string cookieDomain = DY.Config.BaseConfig.CookieDomain;
                if (cookieDomain != string.Empty && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain.TrimStart('.')) > -1 && DY.Site.SiteUtils.IsValidDomain(HttpContext.Current.Request.Url.Host))
                {
                    cookie.Domain = cookieDomain;
                }

                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
    }
}