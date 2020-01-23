UrlRewriter - a rule-based URL Rewriter for .NET.

> Copyright (c)2013 DY
> Author DY
> Author DY
> Version 2.1  

Installation
============
1���������վ�����ߴ���һ���µġ�
2����` DY urlrewriter `���������á�
3���򿪡����硱��
4��������ýڴ������
   ```XML  
   <configSections>
     <section
       name="rewriter"
       requirePermission="false"
       type="DY.UrlRewriter.Configuration.RewriterConfigurationSectionHandler, DY.UrlRewriter" />
   </configSections>
   ```
��ʹ��URL��д����ȡ�������ô�` rewriterules `�ڵ���` Web�����ļ���`��
5��urlrewriter HttpModule���ӳ�䣺
  
   ```XML
   <system.web>
     <httpModules>
       <add
         type="DY.UrlRewriter.RewriterHttpModule, DY.UrlRewriter"
         name="UrlRewriter" />
     </httpModules>
   </system.web>
   ```
��ʹ��URL��д����Web�������дURL������
6�����web.config�ļ����һЩ����

   ```XML
   <rewriter>
     <if url="/tags/(.+)" rewrite="/tagcloud.aspx?tag=$1" />
     <!-- same thing as <rewrite url="/tags/(.+)" to="/tagcloud.aspx?tag=$1" /> -->
   </rewriter>
   ```

��д���ֵ��﷨�Ƿǳ�ǿ��ġ�����ϸ�ڲμ������ļ�
ʲô�ǿ��ܵġ�����Ĺ���ٶ����ѽ���������ӳ�䵽��������ʱ��
����������������һ�㣬����http://urlrewriter.net/index.php/using/installation/
7������Ͳ��ԣ�
