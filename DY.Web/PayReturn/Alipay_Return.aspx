<%@ Page Language="C#" AutoEventWireup="true" Inherits="DY.Web.PayReturn.Alipay_Return" Codebehind="Alipay_Return.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>支付返回页</title>
  
    <meta name="robots" content="all" />
    <meta http-equiv="Content-Type" content="text/html; charset=urf-8" />
</head>
<body>
    <form id="form1" runat="server">
        
        <!--内容页:开始-->
        <div class="w_line">
            <div class="w_line_c">
                <dl>
                    <dt>
                        <img src="<%=ImgState%>"/></dt>
                    <dd>
                        <%=DDState%>
                    </dd>
                </dl>
                <ul>
                    <%=OrderInfo%>
                </ul>
            </div>
            <div class="w_line_d">
                <ul>
                    <li><a href="/Study/">
                        <img src="/images/PayCenter/btn_1.gif" /></a></li>
                    <li><a href="/User/">
                        <img src="/images/PayCenter/btn_2.gif" /></a></li>
                </ul>
            </div>
        </div>
        <!--内容页:结束-->
    </form>
</body>
</html>
