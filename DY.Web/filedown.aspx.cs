using System;
using System.Collections;
using System.Data;
using System.Web;

using CShop.Common;
using CShop.Site;
using CShop.Entity;
using System.IO;

namespace CShop.Web
{
    public partial class filedown :WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int download_id = CShopRequest.getRequestInt("download_id");
            string act = CShopRequest.getRequest("act");
            DownloadInfo cmsinfo = SiteBLL.GetDownloadInfo(download_id);
            if (act == "download")
            {
                if (string.IsNullOrEmpty(cmsinfo.filename))
                {
                    Response.Write("<script>alert('下载目标文件不存在！')</script>");
                }
                else
                {
                    this.download(cmsinfo.filename);
                    //更新访问统计
                    SiteBLL.UpdateDownloadFieldValue("rank", Convert.ToInt32(cmsinfo.rank) + 1, Convert.ToInt32(cmsinfo.down_id));
                }
            }
            else if(act=="vote") {
                bool is_reco = CShopRequest.getRequest("is_reco")=="false"?false:true;
                this.has_vote(download_id,is_reco);
                Response.Write("<script>window.location='/download/detail/"+cmsinfo.urlrewriter+".aspx'</script>");
            }
        }

        /*文件下载*/
        private void download(string filename) {
            string fileName = HttpContext.Current.Server.UrlEncode(filename);
            string filePath = HttpContext.Current.Server.MapPath(filename);   
            //如果要写类的话用HttpResponse ht=Page.Response然后方法写void DownloadFile(HttpResponse response, string serverPath)   
            FileInfo fileinfo = new FileInfo(filePath);
            if (fileinfo.Exists)   
            {   
               const long size = 102400;         //指定下载块的大小   
                byte[] by = new byte[size]; //建立一个100kb的缓存去大小   
                long dataread = 0;          //已读的字节数   
                try  
                {   
                    //打开文件     
                    FileStream filestream = new FileStream(filePath, FileMode.Open, FileAccess.Read);   
                    dataread = filestream.Length;//文件总的大小   
                    int i = 0;   
                    //添加Http头   
                    Response.Clear();   
                    Response.ContentType = "application/octet-stream";   
                    Response.AddHeader("Content-Disposition", "attachement;filename=" + fileName);   
                    Response.AddHeader("Content-Length", dataread.ToString());   
                    while (dataread > 0)   
                    {   
                        if (Response.IsClientConnected)   
                        {   
                            int length = filestream.Read(by,0, Convert.ToInt32(size));   
                            Response.OutputStream.Write(by, 0, length);   
                            Response.Flush();   
                            Response.Clear();   
                            by = new byte[size];   
                            i++;   
                            dataread = dataread - length;//判断是否读取完毕 如果读取完就跳出while循环   
                        }   
                        else  
                        {   
                            dataread = -1;//客户端已经失去连接中段操作   
                        }   
                    }
                    filestream.Close();
                    Response.Close();   
                }   
                catch  { }   
            } 
        }

        /*防止重复投票*/

        private void has_vote(int download_id,bool is_reco) {
            string UserIP = Request.UserHostAddress.ToString();
            HttpCookie oldCookie = Request.Cookies["userIP"];
            string down_id=download_id.ToString();
            if (oldCookie == null)
            {
                this.update_vote(download_id, is_reco);
                //定义新的Cookie对象
                HttpCookie newCookie = new HttpCookie("userIP");
                newCookie.Expires = DateTime.Now.AddMinutes(1);
                //添加新的Cookie变量IPaddress，值为UserIP
                newCookie.Values.Add("IPaddress", UserIP);
                newCookie.Values.Add(down_id, down_id);
                //将变量写入Cookie文件中
                Response.AppendCookie(newCookie);
                Response.Write("<script>alert('投票成功，谢谢您的参与！')</script>");
            }
            else {
               string userIP = oldCookie.Values["IPaddress"];
               string d_id = oldCookie.Values[down_id];
               if (UserIP.Trim() == userIP.Trim() && d_id==down_id)
               {
                   Response.Write("<script>alert('您已经投过票了！');</script>");
               }
               else { 
                   HttpCookie newCookie = new HttpCookie("userIP"); 
                   newCookie.Values.Add("IPaddress", UserIP);
                   newCookie.Values.Add(down_id, down_id);
                   newCookie.Expires = DateTime.Now.AddMinutes(1);
                   Response.AppendCookie(newCookie); 
                   this.update_vote(download_id,is_reco);
                   Response.Write("<script>alert('投票成功，谢谢您的参与！')</script>");
               }
            }
        }
        //更新投票统计
        private void update_vote(int download_id,bool is_reco) {
            DownloadInfo cmsinfo = SiteBLL.GetDownloadInfo(download_id);
            if (is_reco == false)
            {
                SiteBLL.UpdateDownloadFieldValue("no_reco", Convert.ToInt32(cmsinfo.no_reco) + 1, Convert.ToInt32(download_id));
            }
            else {
                SiteBLL.UpdateDownloadFieldValue("is_reco", Convert.ToInt32(cmsinfo.is_reco) + 1, Convert.ToInt32(download_id));
            }
        }
    }
}
