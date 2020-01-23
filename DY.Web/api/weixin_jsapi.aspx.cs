using System;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Security;
using Senparc.Weixin.MP;
using DY.Site;
using DY.Common;
using DY.Weixin.MP.Sample.CommonService.CustomMessageHandler;
using Senparc.Weixin.MP.Entities.Request;
using DY.Entity;
using Newtonsoft.Json;
using Senparc.Weixin.MP.Helpers;

namespace DY.Web
{
    public partial class weixin_jsapi : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WeixinInfo weixin = SiteBLL.GetWeixinInfo(1);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            string url = DYRequest.getFormString("url");
            //using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            //{
            //    jsonWriter.Formatting = Newtonsoft.Json.Formatting.None;
            //    jsonWriter.WriteStartObject();
            //    jsonWriter.WritePropertyName("signature");
            //    jsonWriter.WriteValue(new JSSDK(weixin.appid, weixin.appsecret, url).getSignPackage()["signature"]);
            //    jsonWriter.WritePropertyName("appId");
            //    jsonWriter.WriteValue(weixin.appid);
            //    jsonWriter.WritePropertyName("timestamp");
            //    jsonWriter.WriteValue(new JSSDK(weixin.appid, weixin.appsecret, url).getSignPackage()["timestamp"]);
            //    jsonWriter.WritePropertyName("nonceStr");
            //    jsonWriter.WriteValue(new JSSDK(weixin.appid, weixin.appsecret, url).getSignPackage()["nonceStr"]);
            //    jsonWriter.WriteEnd();
            //}
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Newtonsoft.Json.Formatting.None;
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName("signature");
                jsonWriter.WriteValue(JSSDKHelper.GetJsSdkUiPackage(weixin.appid, weixin.appsecret, url).Signature);
                jsonWriter.WritePropertyName("appId");
                jsonWriter.WriteValue(weixin.appid);
                jsonWriter.WritePropertyName("timestamp");
                jsonWriter.WriteValue(JSSDKHelper.GetJsSdkUiPackage(weixin.appid, weixin.appsecret, url).Timestamp);
                jsonWriter.WritePropertyName("nonceStr");
                jsonWriter.WriteValue(JSSDKHelper.GetJsSdkUiPackage(weixin.appid, weixin.appsecret, url).NonceStr);
                jsonWriter.WriteEnd();
            }
            base.DisplayMemoryTemplate(sw.ToString());
        }
    }
}

