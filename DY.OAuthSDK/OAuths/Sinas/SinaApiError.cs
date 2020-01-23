using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DY.OAuthV2SDK.OAuths.Sinas
{
    /// <summary>
    /// 新浪微博错误代码对照表
    /// <para>author: xusion</para>
    /// <para>vision: oatuth 2.0</para>
    /// <para>url: http://open.weibo.com/wiki/Error_code</para>
    /// <para>create: 2011-12-14</para>
    /// <para>demo: SinaApiError.GetChinese("20019")</para>
    /// <para>return: 不要太贪心了，已经发送过一次</para>
    /// </summary>
    public static class SinaApiError
    {
        private static Dictionary<string, string> English = null;
        private static Dictionary<string, string> Chinese = null;


        /// <summary>
        /// 获取英文错误信息
        /// </summary>
        /// <param name="error_code">错误代码 </param>
        /// <returns></returns>
        public static string GetEnglish(string error_code)
        {
            Dictionary<string, string> list = GetList(0);
            if (list.ContainsKey(error_code))
            {
                return list[error_code];
            }
            return string.Empty;
        }       

        /// <summary>
        /// 获取中文错误信息
        /// </summary>
        /// <param name="error_code">错误代码</param>
        /// <returns></returns>
        public static string GetChinese(string error_code)
        {
            Dictionary<string, string> list = GetList(1);
            if (list.ContainsKey(error_code))
            {
                return list[error_code];
            }
            return string.Empty;
        }
       

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="type">类型 0为英文，1为中文</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetList(int type)
        {
            if (English == null || Chinese == null)
            {
                English = new Dictionary<string, string>();
                Chinese = new Dictionary<string, string>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(XmlData.Trim());
                foreach (XmlNode node in xmlDoc.DocumentElement.SelectNodes("/data/row"))
                {
                    if (node.SelectSingleNode("code") != null && node.SelectSingleNode("english") != null && node.SelectSingleNode("chinese") != null)
                    {
                        string code = node.SelectSingleNode("code").InnerText.Trim();
                        string english = node.SelectSingleNode("english").InnerText.Trim();
                        string chinese = node.SelectSingleNode("chinese").InnerText.Trim();
                        English.Add(code, english);
                        Chinese.Add(code, chinese);
                    }
                }
            }

            return type == 1 ? Chinese : English;

        }

        /// <summary>
        /// sina_error 新浪微博错误代码对照表
        /// <para>http://open.weibo.com/wiki/Error_code</para>
        /// </summary>
        private static string XmlData
        {
            get
            {
                #region xml data 数据
                return @"
                <?xml version='1.0' encoding='UTF-8' ?>
                <data>
	                <row>
		                <code>21322</code>
		                <english>redirect_uri_mismatch</english>
		                <chinese>重定向地址不匹配</chinese>
	                </row>
	                <row>
		                <code>﻿21323</code>
		                <english>invalid_request</english>
		                <chinese>请求不合法</chinese>
	                </row>
	                <row>
		                <code>21324</code>
		                <english>invalid_client</english>
		                <chinese>client_id或client_secret参数无效</chinese>
	                </row>
	                <row>
		                <code>21325</code>
		                <english>unauthorized_client</english>
		                <chinese>提供的Access Grant是无效的、过期的或已撤销的</chinese>
	                </row>
	                <row>
		                <code>21326</code>
		                <english>invalid_grant</english>
		                <chinese>客户端没有权限</chinese>
	                </row>
	                <row>
		                <code>21327</code>
		                <english>expired_token</english>
		                <chinese>token过期</chinese>
	                </row>
	                <row>
		                <code>21328</code>
		                <english>unsupported_grant_type</english>
		                <chinese>不支持的 GrantType</chinese>
	                </row>
	                <row>
		                <code>21329</code>
		                <english>unsupported_response_type</english>
		                <chinese>不支持的 ResponseType</chinese>
	                </row>
	                <row>
		                <code>21330</code>
		                <english>access_denied</english>
		                <chinese>用户或授权服务器拒绝授予数据访问权限</chinese>
	                </row>
	                <row>
		                <code>21331</code>
		                <english>temporarily_unavailable</english>
		                <chinese>服务暂时无法访问</chinese>
	                </row>
	                <row>
		                <code>﻿10001</code>
		                <english>System error</english>
		                <chinese>系统错误</chinese>
	                </row>
	                <row>
		                <code>10002</code>
		                <english>Service unavailable</english>
		                <chinese>服务暂停</chinese>
	                </row>
	                <row>
		                <code>10003</code>
		                <english>Remote service error</english>
		                <chinese>远程服务错误</chinese>
	                </row>
	                <row>
		                <code>10004</code>
		                <english>IP limit</english>
		                <chinese>IP限制不能请求该资源</chinese>
	                </row>
	                <row>
		                <code>10005</code>
		                <english>Permission denied, need a high level appkey</english>
		                <chinese>该资源需要appkey拥有授权</chinese>
	                </row>
	                <row>
		                <code>10006</code>
		                <english>Source paramter (appkey) is missing</english>
		                <chinese>缺少source (appkey) 参数</chinese>
	                </row>
	                <row>
		                <code>10007</code>
		                <english>Unsupport mediatype (%s)</english>
		                <chinese>不支持的MediaType (%s)</chinese>
	                </row>
	                <row>
		                <code>10008</code>
		                <english>Param error, see doc for more info</english>
		                <chinese>参数错误，请参考API文档</chinese>
	                </row>
	                <row>
		                <code>10009</code>
		                <english>Too many pending tasks, system is busy</english>
		                <chinese>任务过多，系统繁忙</chinese>
	                </row>
	                <row>
		                <code>10010</code>
		                <english>Job expired</english>
		                <chinese>任务超时</chinese>
	                </row>
	                <row>
		                <code>10011</code>
		                <english>RPC error</english>
		                <chinese>RPC错误</chinese>
	                </row>
	                <row>
		                <code>10012</code>
		                <english>Illegal request</english>
		                <chinese>非法请求</chinese>
	                </row>
	                <row>
		                <code>10013</code>
		                <english>Invalid weibo user</english>
		                <chinese>不合法的微博用户</chinese>
	                </row>
	                <row>
		                <code>10014</code>
		                <english>Insufficient app permissions</english>
		                <chinese>应用的接口访问权限受限</chinese>
	                </row>
	                <row>
		                <code>10016</code>
		                <english>Miss required parameter (%s) , see doc for more info</english>
		                <chinese>缺失必选参数 (%s)，请参考API文档</chinese>
	                </row>
	                <row>
		                <code>10017</code>
		                <english>Parameter (%s)'s value invalid, expect (%s) , but get (%s) , see doc for more info</english>
		                <chinese>参数值非法，需为 (%s)，实际为 (%s)，请参考API文档</chinese>
	                </row>
	                <row>
		                <code>10018</code>
		                <english>Request body length over limit</english>
		                <chinese>请求长度超过限制</chinese>
	                </row>
	                <row>
		                <code>10020</code>
		                <english>Request api not found</english>
		                <chinese>接口不存在</chinese>
	                </row>
	                <row>
		                <code>10021</code>
		                <english>HTTP method is not suported for this request</english>
		                <chinese>请求的HTTP方法不支持</chinese>
	                </row>
	                <row>
		                <code>10022</code>
		                <english>IP requests out of rate limit</english>
		                <chinese>IP请求频次超过上限</chinese>
	                </row>
	                <row>
		                <code>10023</code>
		                <english>User requests out of rate limit</english>
		                <chinese>用户请求频次超过上限</chinese>
	                </row>
	                <row>
		                <code>10024</code>
		                <english>User requests for (%s) out of rate limit</english>
		                <chinese>用户请求特殊接口 (%s) 频次超过上限</chinese>
	                </row>
	                <row>
		                <code>20001</code>
		                <english>IDs is null</english>
		                <chinese>IDs参数为空</chinese>
	                </row>
	                <row>
		                <code>20002</code>
		                <english>Uid parameter is null</english>
		                <chinese>Uid参数为空</chinese>
	                </row>
	                <row>
		                <code>20003</code>
		                <english>User does not exists</english>
		                <chinese>用户不存在</chinese>
	                </row>
	                <row>
		                <code>20005</code>
		                <english>Unsupported image type, only suport JPG, GIF, PNG</english>
		                <chinese>不支持的图片类型，仅仅支持JPG、GIF、PNG</chinese>
	                </row>
	                <row>
		                <code>20006</code>
		                <english>Image size too large</english>
		                <chinese>图片太大</chinese>
	                </row>
	                <row>
		                <code>20007</code>
		                <english>Does multipart has image</english>
		                <chinese>请确保使用multpart上传图片</chinese>
	                </row>
	                <row>
		                <code>20008</code>
		                <english>Content is null</english>
		                <chinese>内容为空</chinese>
	                </row>
	                <row>
		                <code>20009</code>
		                <english>IDs is too many</english>
		                <chinese>IDs参数太长了</chinese>
	                </row>
	                <row>
		                <code>20012</code>
		                <english>Text too long, please input text less than 140 characters</english>
		                <chinese>输入文字太长，请确认不超过140个字符</chinese>
	                </row>
	                <row>
		                <code>20013</code>
		                <english>Text too long, please input text less than 300 characters</english>
		                <chinese>输入文字太长，请确认不超过300个字符</chinese>
	                </row>
	                <row>
		                <code>20014</code>
		                <english>Param is error, please try again</english>
		                <chinese>安全检查参数有误，请再调用一次</chinese>
	                </row>
	                <row>
		                <code>20015</code>
		                <english>Account or ip or app is illgal, can not continue</english>
		                <chinese>账号、IP或应用非法，暂时无法完成此操作</chinese>
	                </row>
	                <row>
		                <code>20016</code>
		                <english>Out of limit</english>
		                <chinese>发布内容过于频繁</chinese>
	                </row>
	                <row>
		                <code>20017</code>
		                <english>Repeat content</english>
		                <chinese>提交相似的信息</chinese>
	                </row>
	                <row>
		                <code>20018</code>
		                <english>Contain illegal website</english>
		                <chinese>包含非法网址</chinese>
	                </row>
	                <row>
		                <code>20019</code>
		                <english>Repeat conetnt</english>
		                <chinese>不要太贪心了，已经发送过一次</chinese>
	                </row>
	                <row>
		                <code>20020</code>
		                <english>Contain advertising</english>
		                <chinese>包含广告信息</chinese>
	                </row>
	                <row>
		                <code>20021</code>
		                <english>Content is illegal</english>
		                <chinese>包含非法内容</chinese>
	                </row>
	                <row>
		                <code>20022</code>
		                <english>Your ip's behave in a comic boisterous or unruly manner</english>
		                <chinese>此IP地址上的行为异常</chinese>
	                </row>
	                <row>
		                <code>20031</code>
		                <english>Test and verify</english>
		                <chinese>需要验证码</chinese>
	                </row>
	                <row>
		                <code>20032</code>
		                <english>Update success, while server slow now, please wait 1-2 minutes</english>
		                <chinese>发布成功，目前服务器可能会有延迟，请耐心等待1-2分钟</chinese>
	                </row>
	                <row>
		                <code>20101</code>
		                <english>Target weibo does not exist</english>
		                <chinese>不存在的微博</chinese>
	                </row>
	                <row>
		                <code>20102</code>
		                <english>Not your own weibo</english>
		                <chinese>不是你发布的微博</chinese>
	                </row>
	                <row>
		                <code>20103</code>
		                <english>Can't repost yourself weibo</english>
		                <chinese>不能转发自己的微博</chinese>
	                </row>
	                <row>
		                <code>20104</code>
		                <english>Illegal weibo</english>
		                <chinese>不合法的微博</chinese>
	                </row>
	                <row>
		                <code>20109</code>
		                <english>Weibo id is null</english>
		                <chinese>微博ID为空</chinese>
	                </row>
	                <row>
		                <code>20111</code>
		                <english>Repeated weibo text</english>
		                <chinese>不能发布相同的微博</chinese>
	                </row>
	                <row>
		                <code>20201</code>
		                <english>Target weibo comment does not exist</english>
		                <chinese>不存在的微博评论</chinese>
	                </row>
	                <row>
		                <code>20202</code>
		                <english>Illegal comment</english>
		                <chinese>不合法的评论</chinese>
	                </row>
	                <row>
		                <code>20203</code>
		                <english>Not your own comment</english>
		                <chinese>不是你发布的评论</chinese>
	                </row>
	                <row>
		                <code>20204</code>
		                <english>Comment id is null</english>
		                <chinese>评论ID为空</chinese>
	                </row>
	                <row>
		                <code>20301</code>
		                <english>Can't send direct message to user who is not your follower</english>
		                <chinese>不能给不是你粉丝的人发私信</chinese>
	                </row>
	                <row>
		                <code>20302</code>
		                <english>Illegal direct message</english>
		                <chinese>不合法的私信</chinese>
	                </row>
	                <row>
		                <code>20303</code>
		                <english>Not your own direct message</english>
		                <chinese>不是属于你的私信</chinese>
	                </row>
	                <row>
		                <code>20305</code>
		                <english>Direct message does not exist</english>
		                <chinese>不存在的私信</chinese>
	                </row>
	                <row>
		                <code>20306</code>
		                <english>Repeated direct message text</english>
		                <chinese>不能发布相同的私信</chinese>
	                </row>
	                <row>
		                <code>20307</code>
		                <english>Illegal direct message id</english>
		                <chinese>非法的私信ID</chinese>
	                </row>
	                <row>
		                <code>20401</code>
		                <english>Domain not exist</english>
		                <chinese>域名不存在</chinese>
	                </row>
	                <row>
		                <code>20402</code>
		                <english>Wrong verifier</english>
		                <chinese>Verifier错误</chinese>
	                </row>
	                <row>
		                <code>20501</code>
		                <english>Source_user or target_user does not exists</english>
		                <chinese>参数source_user或者target_user的用户不存在</chinese>
	                </row>
	                <row>
		                <code>20502</code>
		                <english>Please input right target user id or screen_name</english>
		                <chinese>必须输入目标用户id或者screen_name</chinese>
	                </row>
	                <row>
		                <code>20503</code>
		                <english>Need you follow user_id</english>
		                <chinese>参数user_id必须是你关注的用户</chinese>
	                </row>
	                <row>
		                <code>20505</code>
		                <english>Social graph updates out of rate limit</english>
		                <chinese>加关注请求超过上限</chinese>
	                </row>
	                <row>
		                <code>20506</code>
		                <english>Already followed</english>
		                <chinese>已经关注此用户</chinese>
	                </row>
	                <row>
		                <code>20601</code>
		                <english>List name too long, please input text less than 10 characters</english>
		                <chinese>列表名太长，请确保输入的文本不超过10个字符</chinese>
	                </row>
	                <row>
		                <code>20602</code>
		                <english>List description too long, please input text less than 70 characters</english>
		                <chinese>列表描叙太长，请确保输入的文本不超过70个字符</chinese>
	                </row>
	                <row>
		                <code>20603</code>
		                <english>List does not exists</english>
		                <chinese>列表不存在</chinese>
	                </row>
	                <row>
		                <code>20604</code>
		                <english>Only the owner has the authority</english>
		                <chinese>不是列表的所属者</chinese>
	                </row>
	                <row>
		                <code>20605</code>
		                <english>Illegal list name or list description</english>
		                <chinese>列表名或描叙不合法</chinese>
	                </row>
	                <row>
		                <code>20606</code>
		                <english>Object already exists</english>
		                <chinese>记录已存在</chinese>
	                </row>
	                <row>
		                <code>20607</code>
		                <english>DB error, please contact the administator</english>
		                <chinese>数据库错误，请联系系统管理员</chinese>
	                </row>
	                <row>
		                <code>20608</code>
		                <english>List name duplicate</english>
		                <chinese>列表名冲突</chinese>
	                </row>
	                <row>
		                <code>20610</code>
		                <english>Does not support private list</english>
		                <chinese>目前不支持私有分组</chinese>
	                </row>
	                <row>
		                <code>20611</code>
		                <english>Create list error</english>
		                <chinese>创建列表失败</chinese>
	                </row>
	                <row>
		                <code>20612</code>
		                <english>Only support private list</english>
		                <chinese>目前只支持私有分组</chinese>
	                </row>
	                <row>
		                <code>20613</code>
		                <english>You hava subscriber too many lists</english>
		                <chinese>订阅列表达到上限</chinese>
	                </row>
	                <row>
		                <code>20614</code>
		                <english>Too many lists, see doc for more info</english>
		                <chinese>创建列表达到上限，请参考API文档</chinese>
	                </row>
	                <row>
		                <code>20615</code>
		                <english>Too many members, see doc for more info</english>
		                <chinese>列表成员上限，请参考API文档</chinese>
	                </row>
	                <row>
		                <code>20701</code>
		                <english>Repeated tag text</english>
		                <chinese>不能提交相同的收藏标签</chinese>
	                </row>
	                <row>
		                <code>20702</code>
		                <english>Tags is too many</english>
		                <chinese>最多两个收藏标签</chinese>
	                </row>
	                <row>
		                <code>20703</code>
		                <english>Illegal tag name</english>
		                <chinese>收藏标签名不合法</chinese>
	                </row>
	                <row>
		                <code>20801</code>
		                <english>Trend_name is null</english>
		                <chinese>参数trend_name是空值</chinese>
	                </row>
	                <row>
		                <code>20802</code>
		                <english>Trend_id is null</english>
		                <chinese>参数trend_id是空值</chinese>
	                </row>
	                <row>
		                <code>21001</code>
		                <english>Tags parameter is null</english>
		                <chinese>标签参数为空</chinese>
	                </row>
	                <row>
		                <code>21002</code>
		                <english>Tags name too long</english>
		                <chinese>标签名太长，请确保每个标签名不超过14个字符</chinese>
	                </row>
	                <row>
		                <code>21101</code>
		                <english>Domain parameter is error</english>
		                <chinese>参数domain错误</chinese>
	                </row>
	                <row>
		                <code>21102</code>
		                <english>The phone number has been used</english>
		                <chinese>该手机号已经被使用</chinese>
	                </row>
	                <row>
		                <code>21103</code>
		                <english>The account has bean bind phone</english>
		                <chinese>该用户已经绑定手机</chinese>
	                </row>
	                <row>
		                <code>21104</code>
		                <english>Wrong verifier</english>
		                <chinese>Verifier错误</chinese>
	                </row>
	                <row>
		                <code>21301</code>
		                <english>Auth faild</english>
		                <chinese>认证失败</chinese>
	                </row>
	                <row>
		                <code>21302</code>
		                <english>Username or password error</english>
		                <chinese>用户名或密码不正确</chinese>
	                </row>
	                <row>
		                <code>21303</code>
		                <english>Username and pwd auth out of rate limit</english>
		                <chinese>用户名密码认证超过请求限制</chinese>
	                </row>
	                <row>
		                <code>21304</code>
		                <english>Version rejected</english>
		                <chinese>版本号错误</chinese>
	                </row>
	                <row>
		                <code>21305</code>
		                <english>Parameter absent</english>
		                <chinese>缺少必要的参数</chinese>
	                </row>
	                <row>
		                <code>21306</code>
		                <english>Parameter rejected</english>
		                <chinese>OAuth参数被拒绝</chinese>
	                </row>
	                <row>
		                <code>21307</code>
		                <english>Timestamp refused</english>
		                <chinese>时间戳不正确</chinese>
	                </row>
	                <row>
		                <code>21308</code>
		                <english>Nonce used</english>
		                <chinese>参数nonce已经被使用</chinese>
	                </row>
	                <row>
		                <code>21309</code>
		                <english>Signature method rejected</english>
		                <chinese>签名算法不支持</chinese>
	                </row>
	                <row>
		                <code>21310</code>
		                <english>Signature invalid</english>
		                <chinese>签名值不合法</chinese>
	                </row>
	                <row>
		                <code>21311</code>
		                <english>Consumer key unknown</english>
		                <chinese>参数consumer_key不存在</chinese>
	                </row>
	                <row>
		                <code>21312</code>
		                <english>Consumer key refused</english>
		                <chinese>参数consumer_key不合法</chinese>
	                </row>
	                <row>
		                <code>21313</code>
		                <english>Miss consumer key</english>
		                <chinese>参数consumer_key缺失</chinese>
	                </row>
	                <row>
		                <code>21314</code>
		                <english>Token used</english>
		                <chinese>Token已经被使用</chinese>
	                </row>
	                <row>
		                <code>21315</code>
		                <english>Token expired</english>
		                <chinese>Token已经过期</chinese>
	                </row>
	                <row>
		                <code>21316</code>
		                <english>Token revoked</english>
		                <chinese>Token不合法</chinese>
	                </row>
	                <row>
		                <code>21317</code>
		                <english>Token rejected</english>
		                <chinese>Token不合法</chinese>
	                </row>
	                <row>
		                <code>21318</code>
		                <english>Verifier fail</english>
		                <chinese>Pin码认证失败</chinese>
	                </row>
	                <row>
		                <code>21319</code>
		                <english>Accessor was revoked</english>
		                <chinese>授权关系已经被解除</chinese>
	                </row>
	                <row>
		                <code>21320</code>
		                <english>OAuth2 must use https</english>
		                <chinese>使用OAuth2必须使用https</chinese>
	                </row>
	                <row>
		                <code>21321</code>
		                <english>Applications over the unaudited use restrictions</english>
		                <chinese>未审核的应用使用人数超过限制</chinese>
	                </row>
	                <row>
		                <code>21501</code>
		                <english>Urls is null</english>
		                <chinese>参数urls是空的</chinese>
	                </row>
	                <row>
		                <code>21502</code>
		                <english>Urls is too many</english>
		                <chinese>参数urls太多了</chinese>
	                </row>
	                <row>
		                <code>21503</code>
		                <english>IP is null</english>
		                <chinese>IP是空值</chinese>
	                </row>
	                <row>
		                <code>21504</code>
		                <english>Url is null</english>
		                <chinese>参数url是空值</chinese>
	                </row>
	                <row>
		                <code>21601</code>
		                <english>Manage notice error, need auth</english>
		                <chinese>需要系统管理员的权限</chinese>
	                </row>
	                <row>
		                <code>21602</code>
		                <english>Contains forbid world</english>
		                <chinese>含有敏感词</chinese>
	                </row>
	                <row>
		                <code>21603</code>
		                <english>Applications send notice over the restrictions</english>
		                <chinese>通知发送达到限制</chinese>
	                </row>
	                <row>
		                <code>21701</code>
		                <english>Manage remind error, need auth</english>
		                <chinese>提醒失败，需要权限</chinese>
	                </row>
	                <row>
		                <code>21702</code>
		                <english>Invalid category</english>
		                <chinese>无效分类</chinese>
	                </row>
	                <row>
		                <code>21703</code>
		                <english>Invalid status</english>
		                <chinese>无效状态码</chinese>
	                </row>
	                <row>
		                <code>21901</code>
		                <english>Geo code input error</english>
		                <chinese>地理信息输入错误</chinese>
	                </row>
                </data>
                ";
                #endregion
            }
        }
    }
}