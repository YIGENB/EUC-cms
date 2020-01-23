<%@ Page Language="C#"%>
<script runat="server">
    public string httpModuleTip = "<br/>请在web.config中system.web->httpModules中添加节点<br/>" +
         HttpUtility.HtmlEncode("<add type=\"DY.UrlRewriter.RewriterHttpModule, DY.UrlRewriter\" name=\"UrlRewriter\" />");

    public string msg="";


    protected void Page_Load(object sender, EventArgs e)
    {
        bool isAssemblyInexistence = false;
        string binfolderpath = HttpRuntime.BinDirectory;
        try
        {
            string[] assemblylist = new string[] { "DY.OAuthSDK.dll", "DY.Cache.dll", "DY.Common.dll", "DY.Config.dll", 
                "CookComputing.XmlRpcV2.dll", "DY.Data.dll", "DY.Data.SqlServer.dll","DY.Entity.dll","DY.Install.dll",
                "DY.Web.dll", "DY.Site.dll","Interop.SQLDMO.dll","Newtonsoft.Json.dll","DY.Weixin.dll","DY.Weixin.MP.dll" };

            ArrayList inexistenceAssemblyList = new ArrayList();
            foreach (string assembly in assemblylist)
            {
                if (!System.IO.File.Exists(binfolderpath + assembly))
                {
                    isAssemblyInexistence = true;
                    inexistenceAssemblyList.Add(assembly);
                }
            }
            if (isAssemblyInexistence)
            {
                foreach (string assembly in inexistenceAssemblyList)
                {
                    msg += "<li>" + assembly + " 文件放置不正确,请将所有的dll文件复制到目录" + binfolderpath + " 中.</li>";
                }
            }
        }
        catch
        {
            msg += "<li>请将所有的dll文件复制到目录 " + binfolderpath + " 中.</li>";
        }

        if (System.IO.File.Exists(binfolderpath.Replace("bin\\", "") + "\\install\\lock.lock"))
        {
            isAssemblyInexistence = true;
            msg += "<li>请删除lock.lock文件进行安装，位于" + binfolderpath.Replace("bin\\", "") + "\\install 目录下.</li>";
        }
        
        if (!System.IO.File.Exists(binfolderpath.Replace("bin\\", "") + "web.config"))
        {
            isAssemblyInexistence = true;
            msg += "<li>web.config文件不存在,请将该文件放置在"+ binfolderpath.Replace("bin\\", "") +" 目录下.</li>";
        }

        else
        {
            string xPath1 = "/configuration/system.web/httpModules";
            
            

            //string xPath2 = "/configuration/system.webServer/modules";
            System.Xml.XmlDocument webConfig = new System.Xml.XmlDocument();
            System.Xml.XmlDocument webConfigOrigin = new System.Xml.XmlDocument();

            webConfig.Load(binfolderpath.Replace("bin\\", "") + "web.config");
            webConfigOrigin = webConfig;

            System.Xml.XmlNode node1 = webConfig.SelectSingleNode(xPath1);
            //System.Xml.XmlNode node2 = webConfig.SelectSingleNode(xPath2);

            if (node1 == null || node1.ChildNodes.Count <= 0 || node1.InnerXml.IndexOf("DY.UrlRewriter.RewriterHttpModule") < 0)
            {
                isAssemblyInexistence = true;
                msg += "<li>web.config中缺少了RewriterHttpModule," + httpModuleTip + "</li>";
            }
        }
        if(!isAssemblyInexistence)
            Response.Redirect("install.aspx");
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Ctmon营销系统安装</title>
<meta name="keywords" content="Ctmon营销系统安装" />
<meta name="description" content="Ctmon营销系统安装" />
<meta name="generator" content="Ctmon 3.0.0" />
<meta http-equiv="x-ua-compatible" content="ie=7" />
<link rel="icon" href="favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
<link rel="stylesheet" href="main.css" type="text/css" media="all" />
</head>

<body>
<div class="wrap cl">
	<h2><img alt="创同盟营销系统" src="images/logo.png"/><cite>安装程序</cite></h2>
	<div class="main cl">
		<h1>基本系统环境检测</h1>
		<div class="inner">
            <ol>
			    <%=msg %>
            </ol>
            <span style="color:Red; font-weight:bold">请将上述问题全部解决再刷新该页面继续安装! </span>
		</div>
	</div>
	<div class="copy">
		深圳市创同盟科技有限公司 &copy; 2013 - 2015 Ctmon Inc. 
	</div>
</div>
</body>
</html>