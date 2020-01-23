using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///WeiXinRetInfo 的摘要说明
/// </summary>
public class WeiXinRetInfo//保存登录失败微信公众平台网页返回的信息
{
    public string Ret { get; set; }
    public string ErrMsg { get; set; }
    public string ShowVerifyCode { get; set; }
    public string ErrCode { get; set; }
}
