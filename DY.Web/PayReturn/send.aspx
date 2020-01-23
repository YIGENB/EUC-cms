<script Language="C#" runat="server">
/**
 * @Description: 快钱人民币支付网关接口范例
 * @Copyright (c) 上海快钱信息服务有限公司
 * @version 2.0
 */


void Page_Load(Object sender, EventArgs E){  

//人民币网关账户号
///请登录快钱系统获取用户编号，用户编号后加01即为人民币网关账户号。
    merchantAcctId.Value = "1001531458201";

//人民币网关密钥
///区分大小写.请与快钱联系索取
    String key = "1111111111111111";

//字符集.固定选择值。可为空。
///只能选择1、2、3.
///1代表UTF-8; 2代表GBK; 3代表gb2312
///默认值为1
///如果在web.config文件中设置了编码方式，例如<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>（如未设则默认为utf-8），
///那么，inputCharset的取值应与已设置的编码方式相一致
inputCharset.Value="1";


//服务器接受支付结果的后台地址.与[pageUrl]不能同时为空。必须是绝对地址。
///快钱通过服务器连接的方式将交易结果发送到[bgUrl]对应的页面地址，在商户处理完成后输出的<result>如果为1，页面会转向到<redirecturl>对应的地址。
///如果快钱未接收到<redirecturl>对应的地址，快钱将把支付结果GET到[pageUrl]对应的页面。
bgUrl.Value="http://www.yoursite.com/receive.aspx";
	
//网关版本.固定值
///快钱会根据版本号来调用对应的接口处理程序。
///本代码版本号固定为v2.0
version.Value="v2.0";

//语言种类.固定选择值。
///只能选择1、2、3
///1代表中文；2代表英文
///默认值为1
language.Value="1";

//签名类型.固定值
///1代表MD5签名
///当前版本固定为1
signType.Value="1";
   
//支付人姓名
///可为中文或英文字符
payerName.Value = "支付人";

//支付人联系方式类型.固定选择值
///只能选择1
///1代表Email
payerContactType.Value="1";

//支付人联系方式
///只能选择Email或手机号
payerContact.Value="";

//商户订单号
///由字母、数字、或[-][_]组成
orderId.Value=DateTime.Now.ToString("yyyyMMddHHmmss");

//订单金额
///以分为单位，必须是整型数字
///比方2，代表0.02元
orderAmount.Value="2";
	
//订单提交时间
///14位数字。年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]
///如；20080101010101
orderTime.Value=DateTime.Now.ToString("yyyyMMddHHmmss");

//商品名称
///可为中文或英文字符
productName.Value = "商品名称";

//商品数量
///可为空，非空时必须为数字
productNum.Value="1";

//商品代码
///可为字符或者数字
productId.Value="";

//商品描述
productDesc.Value = "商品描述";
	
//扩展字段1
///在支付结束后原样返回给商户
ext1.Value="";

//扩展字段2
///在支付结束后原样返回给商户
ext2.Value="";
	
//支付方式.固定选择值
///只能选择00、10、11、12、13、14
///00：组合支付（网关支付页面显示快钱支持的各种支付方式，推荐使用）10：银行卡支付（网关支付页面只显示银行卡支付）.11：电话银行支付（网关支付页面只显示电话支付）.12：快钱账户支付（网关支付页面只显示快钱账户支付）.13：线下支付（网关支付页面只显示线下支付方式）
payType.Value="00";


//同一订单禁止重复提交标志
///固定选择值： 1、0
///1代表同一订单号只允许提交1次；0表示同一订单号在没有支付成功的前提下可重复提交多次。默认为0建议实物购物车结算类商户采用0；虚拟产品类商户采用1
redoFlag.Value="0";

//快钱的合作伙伴的账户号
///如未和快钱签订代理合作协议，不需要填写本参数
pid.Value="";


	//生成加密签名串
	///请务必按照如下顺序和规则组成加密串！
	String signMsgVal="";
	signMsgVal=appendParam(signMsgVal,"inputCharset",inputCharset.Value);
	signMsgVal=appendParam(signMsgVal,"bgUrl",bgUrl.Value);
	signMsgVal=appendParam(signMsgVal,"version",version.Value);
	signMsgVal=appendParam(signMsgVal,"language",language.Value);
	signMsgVal=appendParam(signMsgVal,"signType",signType.Value);
	signMsgVal=appendParam(signMsgVal,"merchantAcctId",merchantAcctId.Value);
	signMsgVal=appendParam(signMsgVal,"payerName",payerName.Value);
	signMsgVal=appendParam(signMsgVal,"payerContactType",payerContactType.Value);
	signMsgVal=appendParam(signMsgVal,"payerContact",payerContact.Value);
	signMsgVal=appendParam(signMsgVal,"orderId",orderId.Value);
	signMsgVal=appendParam(signMsgVal,"orderAmount",orderAmount.Value);
	signMsgVal=appendParam(signMsgVal,"orderTime",orderTime.Value);
	signMsgVal=appendParam(signMsgVal,"productName",productName.Value);
	signMsgVal=appendParam(signMsgVal,"productNum",productNum.Value);
	signMsgVal=appendParam(signMsgVal,"productId",productId.Value);
	signMsgVal=appendParam(signMsgVal,"productDesc",productDesc.Value);
	signMsgVal=appendParam(signMsgVal,"ext1",ext1.Value);
	signMsgVal=appendParam(signMsgVal,"ext2",ext2.Value);
	signMsgVal=appendParam(signMsgVal,"payType",payType.Value);	
	signMsgVal=appendParam(signMsgVal,"redoFlag",redoFlag.Value);
	signMsgVal=appendParam(signMsgVal,"pid",pid.Value);
	signMsgVal=appendParam(signMsgVal,"key",key);

    //如果在web.config文件中设置了编码方式，例如<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>（如未设则默认为utf-8），
    //那么，inputCharset的取值应与已设置的编码方式相一致；
    //同时，GetMD5()方法中所传递的编码方式也必须与此保持一致。
    signMsg.Value = GetMD5(signMsgVal, "utf-8").ToUpper();



//打印提交信息
Lab_orderId.Text=orderId.Value;
Lab_orderAmount.Text=orderAmount.Value;
Lab_payerName.Text=payerName.Value;
Lab_productName.Text=productName.Value;

}
   

	//功能函数。将变量值不为空的参数组成字符串
	String appendParam(String returnStr,String paramId,String paramValue){

		if(returnStr!=""){
			
				if(paramValue!=""){
					
					returnStr+="&"+paramId+"="+paramValue;
				}
			
		}else{
			
			if(paramValue!=""){
				returnStr=paramId+"="+paramValue;
			}
		}
		
		return returnStr;
	}
	//功能函数。将变量值不为空的参数组成字符串。结束



    //功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。开始
    private static string GetMD5(string dataStr, string codeType)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(codeType).GetBytes(dataStr));
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
        {
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        }
        return sb.ToString();
    }
    //功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。结束
    
</script>

<!doctype html public "-//w3c//dtd html 4.0 transitional//en" >
<html>
	<head>
		<title>使用快钱支付</title>
		<meta http-equiv="content-type" content="text/html; charset=gb2312" >
	</head>
	
<BODY>
	
	<div align="center">
		<table width="259" border="0" cellpadding="1" cellspacing="1" bgcolor="#CCCCCC" >
			<tr bgcolor="#FFFFFF">
				<td width="80">支付方式:</td>
				<td >快钱[99bill]</td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td >订单编号:</td>
				<td ><asp:label id="Lab_orderId" RunAt="Server"/></td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td>订单金额:</td>
				<td><asp:label id="Lab_orderAmount" RunAt="Server"/></td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td>支付人:</td>
				<td><asp:label id="Lab_payerName" RunAt="Server"/></td>
			</tr>
			<tr bgcolor="#FFFFFF">
				<td>商品名称:</td>
				<td><asp:label id="Lab_productName" RunAt="Server"/></td>
			</tr>
			<tr>
				<td></td>
				<td></td>
			</tr>
	  </table>
	</div>

	<div align="center" style="font-size=12px;font-weight: bold;color=red;">
		<form name="kqPay" method="post" action="https://www.99bill.com/gateway/recvMerchantInfoAction.htm"/>
			<input type="hidden" id="inputCharset"  runat="server"/>
			<input type="hidden" id="bgUrl"  runat="server"/>
			<input type="hidden" id="version"  runat="server"/>
			<input type="hidden" id="language"  runat="server"/>
			<input type="hidden" id="signType"  runat="server"/>
			<input type="hidden" id="signMsg"  runat="server"/>
			<input type="hidden" id="merchantAcctId"  runat="server"/>
			<input type="hidden" id="payerName"  runat="server"/>
			<input type="hidden" id="payerContactType"  runat="server"/>
			<input type="hidden" id="payerContact"  runat="server"/>
			<input type="hidden" id="orderId"  runat="server"/>
			<input type="hidden" id="orderAmount"  runat="server"/>
			<input type="hidden" id="orderTime"  runat="server"/>
			<input type="hidden" id="productName"  runat="server"/>
			<input type="hidden" id="productNum"  runat="server"/>
			<input type="hidden" id="productId"  runat="server"/>
			<input type="hidden" id="productDesc"  runat="server"/>
			<input type="hidden" id="ext1"  runat="server"/>
			<input type="hidden" id="ext2"  runat="server"/>
			<input type="hidden" id="payType"  runat="server"/>
			<input type="hidden" id="redoFlag"  runat="server"/>	
			<input type="hidden" id="pid"  runat="server"/>	
			<input type="submit" id="submit" value="提交">
			
		</form>		
	</div>
	
</BODY>
</HTML>