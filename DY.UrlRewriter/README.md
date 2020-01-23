UrlRewriter - a rule-based URL Rewriter for .NET.

> Copyright (c)2013 DY
> Author DY
> Author DY
> Version 2.1  

Installation
============
1。打开你的网站，或者创建一个新的。
2。到` DY urlrewriter `组件添加引用。
3。打开“网络”。
4。添加配置节处理程序：
   ```XML  
   <configSections>
     <section
       name="rewriter"
       requirePermission="false"
       type="DY.UrlRewriter.Configuration.RewriterConfigurationSectionHandler, DY.UrlRewriter" />
   </configSections>
   ```
这使得URL重写来读取它的配置从` rewriterules `节点在` Web配置文件。`。
5。urlrewriter HttpModule添加映射：
  
   ```XML
   <system.web>
     <httpModules>
       <add
         type="DY.UrlRewriter.RewriterHttpModule, DY.UrlRewriter"
         name="UrlRewriter" />
     </httpModules>
   </system.web>
   ```
这使得URL重写拦截Web请求和重写URL的请求。
6。你的web.config文件添加一些规则：

   ```XML
   <rewriter>
     <if url="/tags/(.+)" rewrite="/tagcloud.aspx?tag=$1" />
     <!-- same thing as <rewrite url="/tags/(.+)" to="/tagcloud.aspx?tag=$1" /> -->
   </rewriter>
   ```

重写部分的语法是非常强大的。更多细节参见帮助文件
什么是可能的。上面的规则假定您已将所有请求映射到网络运行时。
更多关于如何做到这一点，看到http://urlrewriter.net/index.php/using/installation/
7。编译和测试！
