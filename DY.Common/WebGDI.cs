using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;

namespace DY.Common
{
    /// <summary>
    /// WebGDI : 文字图片、水印图片。
    /// </summary>
    public abstract class WebGDI
    {
        // Private Field

        #region Public Static Method

        /// <summary>
        /// 生成缩微图，并加上阴影文字
        /// </summary>
        /// <param name="Width">生成缩微图的宽度</param>
        /// <param name="Height">生成缩微图的高度</param>
        /// <param name="SourceImg"></param>
        /// <param name="Text"></param>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="font">new Font("Fixedsys", 9, FontStyle.Regular)</param>
        public static void GetThumbnailImage(int Width, int Height, string SourceImg, string Text, int Left, int Top, Font font)
        {
            string outImg = SourceImg + ".jpg";

            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImg);
            System.Drawing.Image thumbnailImage = imgPhoto.GetThumbnailImage(Width, Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Web.HttpContext.Current.Response.Clear();

            Bitmap bmpPhoto = new Bitmap(thumbnailImage);
            Graphics g = Graphics.FromImage(bmpPhoto);

            g.DrawString(Text, font, new SolidBrush(Color.Black), (Left - 1), (Top + 2), new StringFormat(StringFormatFlags.DirectionVertical));
            g.DrawString(Text, font, new SolidBrush(Color.White), Left, Top, new StringFormat(StringFormatFlags.DirectionVertical));
            bmpPhoto.Save(outImg, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Response.ContentType = "image/gif";

            //释放使用中的资源
            thumbnailImage.Dispose();
            imgPhoto.Dispose();
            bmpPhoto.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// 生成缩微图，并加上阴影文字
        /// </summary>
        /// <param name="Width">生成缩微图的宽度</param>
        /// <param name="Height">生成缩微图的高度</param>
        /// <param name="SourceImg"></param>
        /// <param name="Text"></param>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="font">new Font("Fixedsys", 9, FontStyle.Regular)</param>
        public static void GetThumbnailImage(int Width, int Height, string SourceImg, string SaveImage)
        {
            string outImg = SourceImg + ".jpg";

            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImg);
            System.Drawing.Image thumbnailImage = imgPhoto.GetThumbnailImage(Width, Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Web.HttpContext.Current.Response.Clear();

            Bitmap bmpPhoto = new Bitmap(thumbnailImage);
            Graphics g = Graphics.FromImage(bmpPhoto);

            //g.DrawString(Text, font, new SolidBrush(Color.Black), (Left - 1), (Top + 2), new StringFormat(StringFormatFlags.DirectionVertical));
            //g.DrawString(Text, font, new SolidBrush(Color.White), Left, Top, new StringFormat(StringFormatFlags.DirectionVertical));

            if (!string.IsNullOrEmpty(SaveImage))
                bmpPhoto.Save(outImg, System.Drawing.Imaging.ImageFormat.Jpeg);
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //保存加水印过后的图片
                bmpPhoto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            }
            //Response.ContentType = "image/gif";

            //释放使用中的资源
            thumbnailImage.Dispose();
            imgPhoto.Dispose();
            bmpPhoto.Dispose();
            g.Dispose();
        }

        static bool ThumbnailCallback()
        {
            return true;
        }



        public static void SendSmallImage(string path, int width, int height)
        {
            //生成缩率图
            //1、定义变量
            System.Drawing.Image ObjectImag = null;
            System.Drawing.Size Size;
            System.Drawing.Image Bitmap = null;
            System.Drawing.Graphics g = null;
            //打开图片
            try
            {

                ObjectImag = System.Drawing.Image.FromFile(path);

                //重新计算并限制图片的宽度和高度
                int imgWidth = ObjectImag.Width;
                int imgHeight = ObjectImag.Height;
                if (imgWidth > width)
                {
                    int tmpwidth = width, tmpheight;
                    tmpheight = (int)(((double)imgHeight / (double)imgWidth) * width);
                    if (tmpheight > height)
                    {
                        tmpheight = height;
                        tmpwidth = (int)(((double)imgWidth / (double)imgHeight) * height);
                    }
                    imgWidth = tmpwidth;
                    imgHeight = tmpheight;
                }
                else if (imgHeight > height)
                {
                    imgWidth = (int)(((double)imgWidth / (double)imgHeight) * height);
                    imgHeight = height;
                }
                //计算要绘制的图片和画布大小的比例，将要绘制的图片画到画布中央
                int pixWidth = 0;
                int pixHeight = 0;
                if (imgWidth < width)
                    pixWidth = (width - imgWidth) / 2;
                if (imgHeight < height)
                    pixHeight = (height - imgHeight) / 2;

                //设置要生成的图片大小
                Size = new System.Drawing.Size(width, height);
                //新建一个bmp图片
                Bitmap = new System.Drawing.Bitmap(Size.Width, Size.Height);
                //新建一个画板
                g = System.Drawing.Graphics.FromImage(Bitmap);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空一下画布
                g.Clear(System.Drawing.Color.White);

                //在指定位置画图
                g.DrawImage(ObjectImag, new System.Drawing.Rectangle(pixWidth, pixHeight, imgWidth, imgHeight), new System.Drawing.Rectangle(0, 0, ObjectImag.Width, ObjectImag.Height), System.Drawing.GraphicsUnit.Pixel);
                ObjectImag.Dispose();
                //输出流

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                //保存加水印过后的图片
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭图片
                if (ObjectImag != null)
                {
                    ObjectImag.Dispose();
                }
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                }
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }

        public static void SendSmallImage(string path, string out_path, int width, int height)
        {
            //生成缩率图
            //1、定义变量
            System.Drawing.Image ObjectImag = null;
            System.Drawing.Size Size;
            System.Drawing.Image Bitmap = null;
            System.Drawing.Graphics g = null;
            //打开图片
            try
            {

                ObjectImag = System.Drawing.Image.FromFile(path);

                //重新计算并限制图片的宽度和高度
                int imgWidth = ObjectImag.Width;
                int imgHeight = ObjectImag.Height;
                if (imgWidth > width)
                {
                    int tmpwidth = width, tmpheight;
                    tmpheight = (int)(((double)imgHeight / (double)imgWidth) * width);
                    if (tmpheight > height)
                    {
                        tmpheight = height;
                        tmpwidth = (int)(((double)imgWidth / (double)imgHeight) * height);
                    }
                    imgWidth = tmpwidth;
                    imgHeight = tmpheight;
                }
                else if (imgHeight > height)
                {
                    imgWidth = (int)(((double)imgWidth / (double)imgHeight) * height);
                    imgHeight = height;
                }
                //计算要绘制的图片和画布大小的比例，将要绘制的图片画到画布中央
                int pixWidth = 0;
                int pixHeight = 0;
                if (imgWidth < width)
                    pixWidth = (width - imgWidth) / 2;
                if (imgHeight < height)
                    pixHeight = (height - imgHeight) / 2;

                //设置要生成的图片大小
                Size = new System.Drawing.Size(width, height);
                //新建一个bmp图片
                Bitmap = new System.Drawing.Bitmap(Size.Width, Size.Height);
                //新建一个画板
                g = System.Drawing.Graphics.FromImage(Bitmap);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空一下画布
                g.Clear(System.Drawing.Color.White);

                //在指定位置画图
                g.DrawImage(ObjectImag, new System.Drawing.Rectangle(pixWidth, pixHeight, imgWidth, imgHeight), new System.Drawing.Rectangle(0, 0, ObjectImag.Width, ObjectImag.Height), System.Drawing.GraphicsUnit.Pixel);
                ObjectImag.Dispose();
                //输出流



                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                Bitmap.Save(out_path, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭图片
                if (ObjectImag != null)
                {
                    ObjectImag.Dispose();
                }
                if (Bitmap != null)
                {
                    Bitmap.Dispose();
                }
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }

        private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);

            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }

            return new Size(Convert.ToInt32(maxHeight), Convert.ToInt32(maxWidth));
        }


        /// <summary>
        /// 在一张图片的指定位置处加入水印文字
        /// </summary>
        /// <param name="SourceImage">指定源图片的绝对路径</param>
        /// <param name="Text">指定文本</param>
        /// <param name="fontFamily">文本字体</param>
        /// <param name="textPos">指定位置</param>
        /// <param name="SaveImage">保存图片的绝对路径</param>
        public static void GetWaterMarkTextImage(string SourceImage, string Text, string fontFamily, wmPosition textPos, string SaveImage)
        {
            // 创建一个对象用于操作需要加水印的源图片
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImage);
            // 获取该源图片的宽度和高度
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            // 创建一个BMP格式的空白图片(宽度和高度与源图片一致)
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设置该新建空白BMP图片的分辨率
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 将该BMP图片设置成一个图形对象
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            // 设置生成图片的质量
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            // 将源图片加载至新建的BMP图片中
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 



            //------------------------------------------------------------
            // 第一步：设置插入文本信息的相关属性
            //------------------------------------------------------------

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will 
            //test multiple Font sizes to determine the largest posible 
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();

            //Loop through the defined sizes checking the length of the Copyright string
            //If its length in pixles is less then the image width choose this Font size.
            for (int i = 0; i < 7; i++)
            {
                //set a Font object to Arial (i)pt, Bold
                crFont = new Font(fontFamily, sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = grPhoto.MeasureString(Text, crFont);

                if ((ushort)crSize.Width < (ushort)phWidth) break;
            }

            //Since all photographs will have varying heights, determine a 
            //position 5% from the bottom of the image
            int yPixlesFromBottom = (int)(phHeight * .05);

            //Now that we have a point size use the Copyrights string height 
            //to determine a y-coordinate to draw the string of the photograph
            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //Determine its x-coordinate by calculating the center of the width of the image
            float xCenterOfImg = (phWidth / 2);

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //------------------------------------------------------------
            // 第二步：第一次绘制文本信息
            //------------------------------------------------------------

            // 设置字体的颜色和透明度 (透明度设置为153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            // 绘制文本在图片中的指定位置
            grPhoto.DrawString(
                Text,												// 文本信息
                crFont,												// 文本字体
                semiTransBrush2,									// 笔刷
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),	// 文本在图片中的位置
                StrFormat);											// 格式化文本




            //------------------------------------------------------------
            // 第三步：重新绘制一遍文本，使文本具有阴影效果
            //------------------------------------------------------------

            // 设置字体的颜色和透明度 (透明度设置为153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            // 绘制文本在图片中的指定位置
            grPhoto.DrawString(
                Text,										// 文本信息
                crFont,										// 文本字体
                semiTransBrush,								// 笔刷
                new PointF(xCenterOfImg, yPosFromBottom),	// 文本在图片中的位置
                StrFormat);									// 格式化文本


            grPhoto.Dispose();
            imgPhoto.Dispose();
            imgPhoto = bmPhoto;

            //------------------------------------------------------------
            // 第四步：保存图片
            //------------------------------------------------------------
            if (!string.IsNullOrEmpty(SaveImage))
                imgPhoto.Save(SaveImage, ImageFormat.Jpeg);
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //保存加水印过后的图片
                imgPhoto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            }

            // 释放使用中的资源
            bmPhoto.Dispose();
            imgPhoto.Dispose();
        }

        /// <summary>
        /// 在一张图片的指定位置处加入一张具有水印效果的图片
        /// </summary>
        /// <param name="SourceImage">指定源图片的绝对路径</param>
        /// <param name="WaterMarkImage">指定水印图片的绝对路径</param>
        /// <param name="wmPos">指定位置：0左上,1左下,2中上,3正中,4中下,5右上,6右下</param>
        /// <param name="SaveImage">保存图片的绝对路径</param>
        public static void GetWaterMarkPicImage(string SourceImage, string WaterMarkImage, int wmPos, string SaveImage, int w, int h)
        {
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImage);
            // 获取该源图片的宽度和高度
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            if (w == 0)
            {
                w = phWidth;
            }
            if (h == 0)
            {
                h = phWidth;
            }

            System.Drawing.Image thumbnailImage = imgPhoto.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Web.HttpContext.Current.Response.Clear();

            // 创建一个对象用于操作需要加水印的源图片
            //imgPhoto = thumbnailImage;
            imgPhoto = System.Drawing.Image.FromFile(SourceImage);


            // 创建一个BMP格式的空白图片(宽度和高度与源图片一致)
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设置该新建空白BMP图片的分辨率
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 将该BMP图片设置成一个图形对象
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            // 设置生成图片的质量
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // 将源图片加载至新建的BMP图片中
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 


            // 创建水印图片的 Image 对象
            System.Drawing.Image imgWatermark = new Bitmap(WaterMarkImage);

            // 获取水印图片的宽度和高度
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //------------------------------------------------------------
            // 第一步： 插入水印图片
            //------------------------------------------------------------

            //Create a Bitmap based on the previously modified photograph Bitmap
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color 
            //manipulations by defineing a ImageAttributes object and 
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace 
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the 
            //watermark.  This is done by applying a 5x5 matrix that contains the 
            //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = { 
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},       
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //水印位置计算
            int x = 0; //x坐标
            int y = 0; //y坐标

            switch (wmPos)
            {
                case 0: //左上,
                    x = 10;
                    y = 10;
                    break;
                case 1: //左下,
                    x = 10;
                    y = ((phHeight - wmHeight) - 10);
                    break;
                case 2://中上,
                    x = ((phWidth - wmWidth) / 2);
                    y = 10;
                    break;
                case 3://正中,
                    x = ((phWidth - wmWidth) / 2);
                    y = ((phHeight - wmHeight) / 2);
                    break;
                case 4://中下,
                    x = ((phWidth - wmWidth) / 2);
                    y = ((phHeight - wmHeight) - 10);
                    break;
                case 5://右上,
                    x = ((phWidth - wmWidth) - 10);
                    y = 10;
                    break;
                case 6://右下
                    x = ((phWidth - wmWidth) - 10);
                    y = ((phHeight - wmHeight) - 10);
                    break;
            }

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the 
            //left 10 pixles
            int xPosOfWm = x;
            int yPosOfWm = y;


            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            imgPhoto.Dispose();
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();
            bmPhoto.Dispose();

            //------------------------------------------------------------
            // 第三步：保存图片
            //------------------------------------------------------------


            if (!string.IsNullOrEmpty(SaveImage))
                imgPhoto.Save(SaveImage, ImageFormat.Jpeg);
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //保存加水印过后的图片
                imgPhoto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            }


            // 释放使用中的资源
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            bmWatermark.Dispose();
        }

        /// <summary>
        /// 在一张图片的指定位置处加入一张具有水印效果的图片和一段文本
        /// </summary>
        /// <param name="SourceImage"></param>
        /// <param name="WaterMarkImage"></param>
        /// <param name="Text"></param>
        /// <param name="fontFamily"></param>
        /// <param name="wmPos"></param>
        /// <param name="textPos"></param>
        /// <param name="SaveImage"></param>
        public static void GetWarterMarkPicTextImage(string SourceImage, string WaterMarkImage, string Text, string fontFamily, wmPosition wmPos, wmPosition textPos, string SaveImage)
        {
            // 创建一个对象用于操作需要加水印的源图片
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImage);
            // 获取该源图片的宽度和高度
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            // 创建一个BMP格式的空白图片(宽度和高度与源图片一致)
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设置该新建空白BMP图片的分辨率
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 将该BMP图片设置成一个图形对象
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            // 设置生成图片的质量
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            // 将源图片加载至新建的BMP图片中
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 



            //------------------------------------------------------------
            // 第一步：设置插入文本信息的相关属性
            //------------------------------------------------------------

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will 
            //test multiple Font sizes to determine the largest posible 
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();

            //Loop through the defined sizes checking the length of the Copyright string
            //If its length in pixles is less then the image width choose this Font size.
            for (int i = 0; i < 7; i++)
            {
                //set a Font object to Arial (i)pt, Bold
                crFont = new Font(fontFamily, sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = grPhoto.MeasureString(Text, crFont);

                if ((ushort)crSize.Width < (ushort)phWidth) break;
            }

            //Since all photographs will have varying heights, determine a 
            //position 5% from the bottom of the image
            int yPixlesFromBottom = (int)(phHeight * .05);

            //Now that we have a point size use the Copyrights string height 
            //to determine a y-coordinate to draw the string of the photograph
            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //Determine its x-coordinate by calculating the center of the width of the image
            float xCenterOfImg = (phWidth / 2);

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //------------------------------------------------------------
            // 第二步：第一次绘制文本信息
            //------------------------------------------------------------

            // 设置字体的颜色和透明度 (透明度设置为153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(100, 0, 0, 0));

            // 绘制文本在图片中的指定位置
            grPhoto.DrawString(
                Text,												// 文本信息
                crFont,												// 文本字体
                semiTransBrush2,									// 笔刷
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),	// 文本在图片中的位置
                StrFormat);											// 格式化文本




            //------------------------------------------------------------
            // 第三步：重新绘制一遍文本，使文本具有阴影效果
            //------------------------------------------------------------

            // 设置字体的颜色和透明度 (透明度设置为153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

            // 绘制文本在图片中的指定位置
            grPhoto.DrawString(
                Text,										// 文本信息
                crFont,										// 文本字体
                semiTransBrush,								// 笔刷
                new PointF(xCenterOfImg, yPosFromBottom),	// 文本在图片中的位置
                StrFormat);									// 格式化文本


            //------------------------------------------------------------
            // 第四步： 插入水印图片
            //------------------------------------------------------------

            // 创建水印图片的 Image 对象
            System.Drawing.Image imgWatermark = new Bitmap(WaterMarkImage);

            // 获取水印图片的宽度和高度
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //Create a Bitmap based on the previously modified photograph Bitmap
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color 
            //manipulations by defineing a ImageAttributes object and 
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace 
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the 
            //watermark.  This is done by applying a 5x5 matrix that contains the 
            //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = { 
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},       
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the 
            //left 10 pixles
            int xPosOfWm = ((phWidth - wmWidth) - 10);
            int yPosOfWm = 10;


            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            imgPhoto.Dispose();
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();
            bmPhoto.Dispose();


            //------------------------------------------------------------
            // 第五步：保存图片
            //------------------------------------------------------------
            imgPhoto.Save(SaveImage, ImageFormat.Jpeg);


            // 释放使用中的资源
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            bmWatermark.Dispose();
        }




        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="sessionName"></param>
        /// <param name="str"></param>
        public static void MakeSpamImageGen(string sessionName, string str)
        {
            HttpContext context = HttpContext.Current;
            int charsNo = 5;
            int fontSize = 12;
            //int bgWidth = 290;
            //int bgHeight = 80;
            int bgWidth = 60;
            int bgHeight = 20;

            // Get params from QueryString
            //
            //fontSize = 12;   
            //bgWidth = 60;  
            //bgHeight = 20;    
            float x = (bgWidth - (charsNo * (fontSize + 0.5F))) / 2; // TODO: optimize
            float y = (bgHeight - (fontSize * 1.7F)) / 2; // TODO: optimize

            // Load defaults if params are empty
            //
            if (fontSize == -1) fontSize = 30;
            if (bgWidth == -1) bgWidth = 290;
            if (bgHeight == -1) bgHeight = 80;

            // Generate the text
            //
            string genText = str;

            // Add the generate text to a session variable 
            //
            context.Session.Add(sessionName, genText);

            // Create the memory map 
            //
            Bitmap raster;
            //System.Drawing.Imaging.PixelFormat pixFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

            // Select an memory image from file of 290x80 px
            // in the current dir, NoSpamBgImgs folder named bg_X.jpg
            //
            Graphics graphicObj;
            string bgFilePath = context.Server.MapPath(context.Request.ApplicationPath + "/Images/AntiSpamBgImgs/bg_" + new Random().Next(5) + ".jpg");
            System.Drawing.Image imgObj = System.Drawing.Image.FromFile(bgFilePath);

            // Creating the raster image & graphic objects
            //
            raster = new Bitmap(imgObj, bgWidth, bgHeight);
            graphicObj = Graphics.FromImage(raster);

            // Instantiate object of brush with black color
            //
            SolidBrush brushObj = new SolidBrush(Color.Black);

            // Creating an array for most readable yet cryptic fonts for OCR's
            // This is entirely up to developer's discretion
            // CAPTCHA recomandation
            //
            String[] crypticFonts = new String[10];
            crypticFonts[0] = "Arial";
            crypticFonts[1] = "Verdana";
            crypticFonts[2] = "Fixedsys";
            crypticFonts[3] = "宋体";
            crypticFonts[4] = "Haettenschweiler";
            crypticFonts[5] = "Lucida Sans Unicode";
            crypticFonts[6] = "Garamond";
            crypticFonts[7] = "Courier New";
            crypticFonts[8] = "Book Antiqua";
            crypticFonts[9] = "Arial Narrow";

            // Loop to write the characters on image with different fonts
            // CAPTCHA method
            //
            for (int a = 0; a < genText.Length; a++)
            {
                string fontFamily = crypticFonts[new Random().Next(a)];

                Font fontObj = new Font(fontFamily, fontSize, FontStyle.Bold);

                graphicObj.DrawString(genText.Substring(a, 1), fontObj, brushObj, x + (a * fontSize), y);
                graphicObj.Flush();
            }

            // Flush again
            //
            graphicObj.Flush();
            graphicObj.Dispose();

            // 设置输出的MIME类型
            //
            context.Response.ContentType = "image/gif";

            // 输出文件流到浏览器中
            //
            raster.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

            // 释放资源
            //
            context.Response.Flush();
            context.Response.End();
        }
        #endregion

        #region static public method

        /// <summary>
        /// 检测指定图片是否符合标准
        /// </summary>
        /// <param name="imgLoc"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool JustImage(string imgLoc, int width, int height)
        {
            Image image1 = Image.FromFile(imgLoc);
            int num1 = image1.Width;
            int num2 = image1.Height;

            if ((num1 < width) || (num2 < height))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgLoc"></param>
        /// <param name="wmLoc"></param>
        public static void ImageMark(string imgLoc, string wmLoc)
        {
            ImageMark(imgLoc, wmLoc, "RB");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgLoc"></param>
        /// <param name="wmLoc"></param>
        /// <param name="wmAlign"></param>
        public static void ImageMark(string imgLoc, string wmLoc, string wmAlign)
        {
            int num5;
            int num6;
            if (imgLoc.Substring(imgLoc.Length - 3, 3).ToLower() != "jpg")
            {
                return;
            }
            Image image1 = Image.FromFile(imgLoc);
            int num1 = image1.Width;
            int num2 = image1.Height;
            Bitmap bitmap1 = new Bitmap(image1, num1, num2);
            bitmap1.SetResolution(72f, 72f);
            Graphics graphics1 = Graphics.FromImage(bitmap1);
            Image image2 = new Bitmap(wmLoc);
            int num3 = image2.Width;
            int num4 = image2.Height;
            if ((num1 < num3) || (num2 < (num4 * 2)))
            {
                return;
            }
            Bitmap bitmap2 = new Bitmap(bitmap1);
            bitmap2.SetResolution(image1.HorizontalResolution, image1.VerticalResolution);
            Graphics graphics2 = Graphics.FromImage(bitmap2);
            ImageAttributes attributes1 = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace 
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(0, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            attributes1.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] singleArrayArray2 = new float[5][];
            float[] singleArray1 = new float[5];
            singleArray1[0] = 1f;
            singleArrayArray2[0] = singleArray1;
            singleArray1 = new float[5];
            singleArray1[1] = 1f;
            singleArrayArray2[1] = singleArray1;
            singleArray1 = new float[5];
            singleArray1[2] = 1f;
            singleArrayArray2[2] = singleArray1;
            singleArray1 = new float[5];
            singleArray1[3] = 0.8f;
            singleArrayArray2[3] = singleArray1;
            singleArray1 = new float[5];
            singleArray1[4] = 1f;
            singleArrayArray2[4] = singleArray1;
            float[][] singleArrayArray1 = singleArrayArray2;
            ColorMatrix matrix1 = new ColorMatrix(singleArrayArray1);
            attributes1.SetColorMatrix(matrix1, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            if (wmAlign == "LT")
            {
                num5 = 0;
                num6 = 0;
            }
            else if (wmAlign == "LB")
            {
                num5 = 0;
                num6 = num2 - num4;
            }
            else if (wmAlign == "RT")
            {
                num5 = num1 - num3;
                num6 = 0;
            }
            else if (wmAlign == "CT")
            {
                num5 = (num1 - num3) / 2;
                num6 = (num2 - num4) / 2;
            }
            else
            {
                num5 = num1 - num3;
                num6 = num2 - num4;
            }
            graphics2.DrawImage(image2, new Rectangle(num5, num6, num3, num4), 0, 0, num3, num4, GraphicsUnit.Pixel, attributes1);
            image1.Dispose();
            image1 = bitmap2;
            graphics1.Dispose();
            graphics2.Dispose();
            bitmap1.Dispose();
            ImageCodecInfo info1 = GetEncoderInfo("image/jpeg");
            EncoderParameter parameter1 = new EncoderParameter(Encoder.Quality, (long)90);
            EncoderParameters parameters1 = new EncoderParameters(1);
            parameters1.Param[0] = parameter1;
            image1.Save(imgLoc, info1, parameters1);
            image1.Dispose();
            image2.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] infoArray1 = ImageCodecInfo.GetImageEncoders();
            for (int num1 = 0; num1 < infoArray1.Length; num1++)
            {
                if (infoArray1[num1].MimeType == mimeType)
                {
                    return infoArray1[num1];
                }
            }
            return null;
        }
        #endregion
    }


    /// <summary>
    /// 水印效果在源图片中的位置
    /// </summary>
    public enum wmPosition
    {
        /// <summary>
        /// 绝对居中
        /// </summary>
        MM = 0,

        /// <summary>
        /// 左上
        /// </summary>
        LT = 1,

        /// <summary>
        /// 左下
        /// </summary>
        LB = 2,

        /// <summary>
        /// 中上
        /// </summary>
        CT = 3,

        /// <summary>
        /// 中下
        /// </summary>
        CB = 4,

        /// <summary>
        /// 右上
        /// </summary>
        RT = 5,

        /// <summary>
        /// 右下
        /// </summary>
        RB = 6
    }
}
