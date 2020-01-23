using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using DY.OAuthV2SDK.Entitys;

namespace DY.OAuthV2SDK.OAuths
{
    public abstract partial class OAuthBase
    {
        /*请根据需要添加API接口方法，每个子类都必须进行覆盖实现*/

        #region 获取API接口
        /// <summary>
        /// 获取用户id
        /// <para>使用当前AccessToken</para>
        /// </summary>
        /// <returns></returns>
        public virtual ApiResult GetUid()
        {
            return GetUid(this.AccessToken);
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <returns></returns>
        public abstract ApiResult GetUid(string accessToken);


        /// <summary>
        /// 发送微博
        /// <para>使用当前AccessToken</para>
        /// </summary>
        /// <param name="strText">微博内容</param>
        /// <returns></returns>
        public virtual ApiResult SendStatus(string strText)
        {
            return SendStatus(this.AccessToken, strText);
        }

        /// <summary>
        /// 发送微博
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="strText">微博内容</param>
        /// <returns></returns>
        public abstract ApiResult SendStatus(string accessToken, string strText);

        /// <summary>
        /// 发送图片微博
        /// <para>使用当前AccessToken</para>
        /// </summary>
        /// <param name="strText">微博内容</param>
        /// <param name="strFile">图片绝对路径</param>
        /// <returns></returns>
        public virtual ApiResult SendStatusWithPic(string strText, string strFile)
        {
            return SendStatusWithPic(this.AccessToken, strText, strFile);
        }

        /// <summary>
        /// 发送图片微博
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="strText">微博内容</param>
        /// <param name="strFile">图片绝对路径</param>
        /// <returns></returns>
        public abstract ApiResult SendStatusWithPic(string accessToken, string strText, string strFile);

        /// <summary>
        /// 发送文章类型，如头条
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="title">标题</param>
        /// <param name="des">简介</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public abstract ApiResult SendStatusToArticle(string accessToken, string title, string des, string content);
        #endregion


    }
}
