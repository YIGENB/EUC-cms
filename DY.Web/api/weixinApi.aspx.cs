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

namespace DY.Web
{
    public partial class weixinApi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            int pid = DYRequest.getRequestInt("pid");
            WeixinInfo weixin = SiteBLL.GetWeixinInfo(pid);
            Session["pid"]= pid;
            string token = weixin.token;
            string signature = DYRequest.getRequest("signature").ToString();
            string timestamp = DYRequest.getRequest("timestamp").ToString();
            string nonce = DYRequest.getRequest("nonce").ToString();
            string echoStr = DYRequest.getRequest("echoStr").ToString();
            //网站接入
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                if (CheckSignature.Check(signature, timestamp, nonce, token))
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        Response.Write(echoStr);
                        Response.End();
                    }
                }
            }
            else
            {
                //接受处理用户发送过来的信息
                if (!CheckSignature.Check(signature, timestamp, nonce, token))
                {
                    Response.Write("参数错误！");
                    return;
                }

                //post method - 当有用户想公众账号发送消息时触发
                var postModel = new PostModel()
                {
                    Signature = signature,
                    Msg_Signature = Request.QueryString["msg_signature"],
                    Timestamp = timestamp,
                    Nonce = nonce,
                    //以下保密信息不会（不应该）在网络上传播，请注意
                    Token = token,
                    EncodingAESKey = weixin.encodingAESKey,//根据自己后台的设置保持一致
                    AppId = weixin.appid//根据自己后台的设置保持一致
                };
                //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
                var maxRecordCount = 10;

                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);

                try
                {
                    //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                    //messageHandler.RequestDocument.Save(
                    //    Server.MapPath("~/log/" + DateTime.Now.Ticks + "_Request_" +
                    //                   messageHandler.RequestMessage.FromUserName + ".txt"));
                    //执行微信处理过程
                    messageHandler.Execute();
                    //测试时可开启，帮助跟踪数据
                    //messageHandler.ResponseDocument.Save(
                    //    Server.MapPath("~/log/" + DateTime.Now.Ticks + "_Response_" +
                    //                   messageHandler.ResponseMessage.ToUserName + ".txt"));
                    Response.Write(messageHandler.ResponseDocument.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    //using (TextWriter tw = new StreamWriter(Server.MapPath("~/bin/Error_" + DateTime.Now.Ticks + ".txt")))
                    //{
                    //    tw.WriteLine(messageHandler.ResponseDocument);
                    //    tw.WriteLine(ex.Message);
                    //    tw.WriteLine(ex.InnerException.Message);
                    //    if (messageHandler.ResponseDocument != null)
                    //    {
                    //        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    //    }
                    //    tw.Flush();
                    //    tw.Close();
                    //}
                    Response.Write("");
                }
                finally
                {
                    Response.End();
                }

            }

            //    DY.Weixin.MP.Entities.IRequestMessageBase requestMessage = DY.Weixin.MP.RequestMessageFactory.GetRequestEntity(System.Web.HttpContext.Current.Request.InputStream);
            //    DY.Weixin.MP.Entities.RequestMessageEvent_Subscribe aaa = requestMessage;
            //    string xml = "<xml><ToUserName><![CDATA[" + requestMessage.FromUserName + "]]></ToUserName>" +
            //"<FromUserName><![CDATA[" + requestMessage.ToUserName + "]]></FromUserName>" +
            //"<CreateTime>" + requestMessage.CreateTime + "</CreateTime><MsgType><![CDATA[text]]></MsgType>" +
            //"<Content><![CDATA[欢迎来到微信世界---已接收]]></Content></xml> ";
            //    Response.Write(xml);
        }
        //if (Request.HttpMethod.ToLower() == "post")
        //{
        //    Stream s = System.Web.HttpContext.Current.Request.InputStream;
        //    byte[] b = new byte[s.Length];
        //    s.Read(b, 0, (int)s.Length);
        //    postStr = Encoding.UTF8.GetString(b);
        //    if (!string.IsNullOrEmpty(postStr))
        //    {
        //        ResponseMsg(postStr);
        //    }
        //    WriteLog("postStr:" + postStr);
        //}
    }
}

