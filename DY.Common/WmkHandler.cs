using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;

namespace CShop.Common
{
    public class WmkHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                //得到请求路径
                string url = context.Request.Url.AbsoluteUri.ToLower();
                string monitorPath = ConfigurationManager.AppSettings["WmkPath"];

                //是否包含图片路径

                bool IsInterestUrl = url.Contains(monitorPath);
                System.Drawing.Image imgSource = null;



                //判断原图片是否存在
                string physicalPath = context.Request.PhysicalPath;
                if (!System.IO.File.Exists(physicalPath))
                {
                    context.Response.Write("图片不存在");
                    return;
                }

                //如果不是要加水印的文件或文件夹,就原样输出

                if (!IsInterestUrl)
                {
                    imgSource = System.Drawing.Image.FromFile(physicalPath);
                    imgSource.Save(context.Response.OutputStream, imgSource.RawFormat);
                    imgSource.Dispose();
                    return;
                }

                imgSource = System.Drawing.Image.FromFile(physicalPath);

                //判断是否是索引图像格式
                if (imgSource.PixelFormat == PixelFormat.Format1bppIndexed || imgSource.PixelFormat == PixelFormat.Format4bppIndexed || imgSource.PixelFormat == PixelFormat.Format8bppIndexed)
                {

                    //转成位图,这步很重要
                    Bitmap bitmap = new Bitmap(imgSource.Width, imgSource.Height);

                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);
                    System.Drawing.Font font = new System.Drawing.Font("Arial Black", 30.0f, System.Drawing.FontStyle.Bold);

                    //将原图画在位图上

                    graphic.DrawImage(imgSource, new Point(0, 0));

                    //将水印加在位图上

                    graphic.DrawString("www.dgxyt.com", font, System.Drawing.Brushes.Red, new System.Drawing.PointF());

                    //将位图输入到流
                    bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);

                    graphic.Dispose();
                    imgSource.Dispose();
                    bitmap.Dispose();

                }
                else
                {

                    //不是索引图像格式,直接在上面加上水印

                    System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(imgSource);

                    System.Drawing.Font font = new System.Drawing.Font("Arial Black", 30.0f, System.Drawing.FontStyle.Bold);
                    graphic.DrawString("www.dgxyt.com", font, System.Drawing.Brushes.Red, new System.Drawing.PointF());
                    imgSource.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);


                    imgSource.Dispose();
                    graphic.Dispose();
                }

                //标明类型为jpg：如果不标注，IE没有问题，但Firefox会出现乱码
                context.Response.ContentType = "image/jpeg";
                context.Response.Flush();
                context.Response.End();
            }
            catch
            {
                throw;
            }
        }
    }


}
