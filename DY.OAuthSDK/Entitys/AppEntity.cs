using System;
using System.Collections.Generic;
using System.Text;

namespace DY.OAuthV2SDK.Entitys
{
    /// <summary>
    /// APPKey实体
    /// </summary>
    [Serializable]
    public class AppEntity
    {
        /// <summary>
        /// 名字，只是做判断使用，但不能重复
        /// </summary>
        public string AppName { set; get; }

        /// <summary>
        /// 申请应用时分配的AppKey
        /// </summary>
        public string AppKey { set; get; }

        /// <summary>
        /// 申请应用时分配的AppSecret
        /// </summary>
        public string AppSecret { set; get; }

        /// <summary>
        /// 应用回调页，当用户授权你的应用后，开放平台会回调你填写的这个地址。
        /// </summary>
        public string RedirectUri { set; get; }

        /// <summary>
        /// 若不传递此参数，代表请求用户的默认权限。
        /// </summary>
        public string Scope { set; get; }

        /// <summary>
        /// 用户名，可选项
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 用户密码，可选项
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// access_token
        /// </summary>
        public string Access_token { set; get; }

        /// <summary>
        /// uid
        /// </summary>
        public string Uid { set; get; }

        /// <summary>
        /// appid
        /// </summary>
        public string Appid { set; get; }

        /// <summary>
        /// 随机@
        /// </summary>
        public string RandomAT { set; get; }

    }

}
