using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using DY.Entity;
using System.Collections;
using DY.Site;
using DY.Config;
using DY.Common;
using DY.LanguagePack;

namespace DY.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    public partial class CustomMessageHandler
    {
        /// <summary>
        /// 网站基本信息
        /// </summary>
        protected internal BaseConfigInfo config=BaseConfig.Get();

        /// <summary>
        /// 素材库消息类型
        /// </summary>
        public enum MsgType
        {
            singlenews = 0,
            news=1,
            goods=2,
            txt=3,
            music=4,
            image=5,
            voice=6,
            video=7
        }

        /// <summary>
        /// 返回处理过的活动
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public IResponseMessageBase GetActivitiesKeyWordNews(CardInfo card)
        {
            IResponseMessageBase responseMessage = null;
            responseMessage = OnActivitiesResponseMessage(card);
            return responseMessage;
        }

        /// <summary>
        /// 处理会员是否绑定信息
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase GetUsersResponseMessage()
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            responseMessage.Content = "您好，请点击【<a href=\"" + urlrewrite.http + new SiteUtils().GetDomain() + "/login.aspx?act=login&openid=" + responseMessage.ToUserName + "\">绑定微信</a>】账号在使用。";
            return responseMessage;
        }

        /// <summary>
        /// 返回处理过的
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public IResponseMessageBase GetKeyWordNews(WeixinNewsInfo news)
        {
            IResponseMessageBase responseMessage = null;
            switch (news.radio_type)
            {
                case 0: responseMessage = OnSingleNewsResponseMessage(news); break;
                case 3: responseMessage = OnTxtResponseMessage(news); break;
                case 1: responseMessage = OnNewsResponseMessage(news); break;
                case 2: responseMessage = OnGoodsResponseMessage(news); break;
                case 4: responseMessage = OnMusicResponseMessage(news); break;
                case 5: responseMessage = OnImageResponseMessage(news); break;
                case 6: responseMessage = OnVoiceResponseMessage(news); break;
                case 7: responseMessage = OnVideoResponseMessage(news); break;
            }
            return responseMessage;
        }

        /// <summary>
        /// 处理文字
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnTxtResponseMessage(WeixinNewsInfo news)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = news.des;
            return responseMessage;
        }

        /// <summary>
        /// 处理新闻
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnNewsResponseMessage(WeixinNewsInfo news)
        {
            var requestMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            //暂时去掉自选信息
            //for (int i = 0; i < news.double_news.Split(',').Length; i++)
            //{
            //    CmsInfo cms = SiteBLL.GetCmsInfo(Convert.ToInt32(news.double_news.Split(',')[i]));
            //    requestMessage.Articles.Add(new Article()
            //    {
            //        Title = cms.title,
            //        Description = cms.des,
            //        PicUrl = !cms.photo.Contains("http") ? "http://" + new SiteUtils().GetDomain() + cms.photo : cms.photo,
            //        Url = "http://" + new SiteUtils().GetDomain() + "/article/detail/" + cms.article_id + config.UrlRewriterKzm
            //    });
            //}
            //规定信息处理
            #region 推荐新闻
            if (news.double_news=="1")
            {
                foreach (CmsInfo cms in new Caches().BestNews(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = cms.title,
                        Description = cms.des,
                        PicUrl = !cms.photo.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + cms.photo : cms.photo,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + cms.article_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            #region 置顶新闻
            else if (news.double_news == "2")
            {
                foreach (CmsInfo cms in new Caches().TopNews(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = cms.title,
                        Description = cms.des,
                        PicUrl = !cms.photo.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + cms.photo : cms.photo,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + cms.article_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            #region 最新新闻
            else if (news.double_news == "3")
            {
                foreach (CmsInfo cms in new Caches().NewNews(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = cms.title,
                        Description = cms.des,
                        PicUrl = !cms.photo.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + cms.photo : cms.photo,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.article_detail + cms.article_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            return requestMessage;
        }

        /// <summary>
        /// 处理产品
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnGoodsResponseMessage(WeixinNewsInfo news)
        {
            var requestMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            //去掉自选
            //for (int i = 0; i < news.double_news.Split(',').Length; i++)
            //{
            //    GoodsInfo goods = SiteBLL.GetGoodsInfo(Convert.ToInt32(news.double_news.Split(',')[i]));
            //    requestMessage.Articles.Add(new Article()
            //    {
            //        Title = goods.goods_name,
            //        Description = goods.goods_desc,
            //        PicUrl = !goods.original_img.Contains("http") ? "http://" + new SiteUtils().GetDomain() + goods.original_img : goods.original_img,
            //        Url = "http://" + new SiteUtils().GetDomain() + "/product/detail/" + goods.goods_id + config.UrlRewriterKzm
            //    });
            //}
            #region 推荐产品
            if (news.double_news == "1")
            {
                foreach (GoodsInfo goods in new Caches().BestProduct(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = goods.goods_name,
                        Description = goods.goods_desc,
                        PicUrl = !goods.original_img.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + goods.original_img : goods.original_img,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + goods.goods_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            #region 新品产品
            else if (news.double_news == "2")
            {
                foreach (GoodsInfo goods in new Caches().NewProduct(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = goods.goods_name,
                        Description = goods.goods_desc,
                        PicUrl = !goods.original_img.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + goods.original_img : goods.original_img,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + goods.goods_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            #region 热卖产品
            else if (news.double_news == "3")
            {
                foreach (GoodsInfo goods in new Caches().HotProduct(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = goods.goods_name,
                        Description = goods.goods_desc,
                        PicUrl = !goods.original_img.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + goods.original_img : goods.original_img,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + goods.goods_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            #region 特价产品
            else if (news.double_news == "4")
            {
                foreach (GoodsInfo goods in new Caches().SpecialsProduct(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = goods.goods_name,
                        Description = goods.goods_desc,
                        PicUrl = !goods.original_img.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + goods.original_img : goods.original_img,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + goods.goods_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            #region 最新产品
            else if (news.double_news == "5")
            {
                foreach (GoodsInfo goods in new Caches().NewProduct(9))
                {
                    requestMessage.Articles.Add(new Article()
                    {
                        Title = goods.goods_name,
                        Description = goods.goods_desc,
                        PicUrl = !goods.original_img.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + goods.original_img : goods.original_img,
                        Url = urlrewrite.http + new SiteUtils().GetDomain() + urlrewrite.product_detail + goods.goods_id + config.UrlRewriterKzm
                    });
                }
            }
            #endregion

            return requestMessage;
        }

        /// <summary>
        /// 处理音乐
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnMusicResponseMessage(WeixinNewsInfo news)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = news.url;
            responseMessage.Music.Title = news.title;
            responseMessage.Music.Description = news.des;
            return responseMessage;
        }

        /// <summary>
        /// 处理图片
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnImageResponseMessage(WeixinNewsInfo news)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = news.title,
                Description = news.des,
                PicUrl = !news.pic.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + news.pic : news.pic,
                Url = news.url
            });
            return responseMessage;
        }

        /// <summary>
        /// 处理语音
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnVoiceResponseMessage(WeixinNewsInfo news)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = news.url;
            responseMessage.Music.Title = news.title;
            responseMessage.Music.Description = news.des;
            return responseMessage;
        }

        /// <summary>
        /// 处理视频
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnVideoResponseMessage(WeixinNewsInfo news)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = news.title,
                Description = news.des,
                PicUrl = news.pic,
                Url = news.url
            });
            return responseMessage;
        }

        /// <summary>
        /// 处理单图文
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnSingleNewsResponseMessage(WeixinNewsInfo news)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = news.title,
                Description = news.des,
                PicUrl = !news.pic.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + news.pic : news.pic,
                Url = news.url
            });
            return responseMessage;
        }

        /// <summary>
        /// 处理刮刮卡
        /// </summary>
        /// <param name="news">素材库实体类</param>
        /// <returns></returns>
        IResponseMessageBase OnActivitiesResponseMessage(CardInfo card)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageNews>();
            string tlp = "";
            switch (card.type)
            {
                case 0: tlp = "ggk"; break;
                case 1: tlp = "dzp"; break;
            }
            responseMessage.Articles.Add(new Article()
            {
                Title = card.name,
                Description = card.des,
                PicUrl = !card.pic.Contains("http") ? urlrewrite.http + new SiteUtils().GetDomain() + card.pic : card.pic,
                Url = urlrewrite.http + new SiteUtils().GetDomain() + "/activities-" + tlp + "-" + card.card_id + config.UrlRewriterKzm
            });
            return responseMessage;
        }
    }
    
}
