using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;

namespace DY.Common
{
    /// <summary>
    /// WebGDI : ����ͼƬ��ˮӡͼƬ��
    /// </summary>
    public abstract class WebGDI
    {
        // Private Field

        #region Public Static Method

        /// <summary>
        /// ������΢ͼ����������Ӱ����
        /// </summary>
        /// <param name="Width">������΢ͼ�Ŀ��</param>
        /// <param name="Height">������΢ͼ�ĸ߶�</param>
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

            //�ͷ�ʹ���е���Դ
            thumbnailImage.Dispose();
            imgPhoto.Dispose();
            bmpPhoto.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// ������΢ͼ����������Ӱ����
        /// </summary>
        /// <param name="Width">������΢ͼ�Ŀ��</param>
        /// <param name="Height">������΢ͼ�ĸ߶�</param>
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

                //�����ˮӡ�����ͼƬ
                bmpPhoto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            }
            //Response.ContentType = "image/gif";

            //�ͷ�ʹ���е���Դ
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
            //��������ͼ
            //1���������
            System.Drawing.Image ObjectImag = null;
            System.Drawing.Size Size;
            System.Drawing.Image Bitmap = null;
            System.Drawing.Graphics g = null;
            //��ͼƬ
            try
            {

                ObjectImag = System.Drawing.Image.FromFile(path);

                //���¼��㲢����ͼƬ�Ŀ�Ⱥ͸߶�
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
                //����Ҫ���Ƶ�ͼƬ�ͻ�����С�ı�������Ҫ���Ƶ�ͼƬ������������
                int pixWidth = 0;
                int pixHeight = 0;
                if (imgWidth < width)
                    pixWidth = (width - imgWidth) / 2;
                if (imgHeight < height)
                    pixHeight = (height - imgHeight) / 2;

                //����Ҫ���ɵ�ͼƬ��С
                Size = new System.Drawing.Size(width, height);
                //�½�һ��bmpͼƬ
                Bitmap = new System.Drawing.Bitmap(Size.Width, Size.Height);
                //�½�һ������
                g = System.Drawing.Graphics.FromImage(Bitmap);
                //���ø�������ֵ��
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //���ø�����,���ٶȳ���ƽ���̶�
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //���һ�»���
                g.Clear(System.Drawing.Color.White);

                //��ָ��λ�û�ͼ
                g.DrawImage(ObjectImag, new System.Drawing.Rectangle(pixWidth, pixHeight, imgWidth, imgHeight), new System.Drawing.Rectangle(0, 0, ObjectImag.Width, ObjectImag.Height), System.Drawing.GraphicsUnit.Pixel);
                ObjectImag.Dispose();
                //�����

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                //�����ˮӡ�����ͼƬ
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
                //�ر�ͼƬ
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
            //��������ͼ
            //1���������
            System.Drawing.Image ObjectImag = null;
            System.Drawing.Size Size;
            System.Drawing.Image Bitmap = null;
            System.Drawing.Graphics g = null;
            //��ͼƬ
            try
            {

                ObjectImag = System.Drawing.Image.FromFile(path);

                //���¼��㲢����ͼƬ�Ŀ�Ⱥ͸߶�
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
                //����Ҫ���Ƶ�ͼƬ�ͻ�����С�ı�������Ҫ���Ƶ�ͼƬ������������
                int pixWidth = 0;
                int pixHeight = 0;
                if (imgWidth < width)
                    pixWidth = (width - imgWidth) / 2;
                if (imgHeight < height)
                    pixHeight = (height - imgHeight) / 2;

                //����Ҫ���ɵ�ͼƬ��С
                Size = new System.Drawing.Size(width, height);
                //�½�һ��bmpͼƬ
                Bitmap = new System.Drawing.Bitmap(Size.Width, Size.Height);
                //�½�һ������
                g = System.Drawing.Graphics.FromImage(Bitmap);
                //���ø�������ֵ��
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //���ø�����,���ٶȳ���ƽ���̶�
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //���һ�»���
                g.Clear(System.Drawing.Color.White);

                //��ָ��λ�û�ͼ
                g.DrawImage(ObjectImag, new System.Drawing.Rectangle(pixWidth, pixHeight, imgWidth, imgHeight), new System.Drawing.Rectangle(0, 0, ObjectImag.Width, ObjectImag.Height), System.Drawing.GraphicsUnit.Pixel);
                ObjectImag.Dispose();
                //�����



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
                //�ر�ͼƬ
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
        /// ��һ��ͼƬ��ָ��λ�ô�����ˮӡ����
        /// </summary>
        /// <param name="SourceImage">ָ��ԴͼƬ�ľ���·��</param>
        /// <param name="Text">ָ���ı�</param>
        /// <param name="fontFamily">�ı�����</param>
        /// <param name="textPos">ָ��λ��</param>
        /// <param name="SaveImage">����ͼƬ�ľ���·��</param>
        public static void GetWaterMarkTextImage(string SourceImage, string Text, string fontFamily, wmPosition textPos, string SaveImage)
        {
            // ����һ���������ڲ�����Ҫ��ˮӡ��ԴͼƬ
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImage);
            // ��ȡ��ԴͼƬ�Ŀ�Ⱥ͸߶�
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            // ����һ��BMP��ʽ�Ŀհ�ͼƬ(��Ⱥ͸߶���ԴͼƬһ��)
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // ���ø��½��հ�BMPͼƬ�ķֱ���
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // ����BMPͼƬ���ó�һ��ͼ�ζ���
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            // ��������ͼƬ������
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            // ��ԴͼƬ�������½���BMPͼƬ��
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 



            //------------------------------------------------------------
            // ��һ�������ò����ı���Ϣ���������
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
            // �ڶ�������һ�λ����ı���Ϣ
            //------------------------------------------------------------

            // �����������ɫ��͸���� (͸��������Ϊ153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            // �����ı���ͼƬ�е�ָ��λ��
            grPhoto.DrawString(
                Text,												// �ı���Ϣ
                crFont,												// �ı�����
                semiTransBrush2,									// ��ˢ
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),	// �ı���ͼƬ�е�λ��
                StrFormat);											// ��ʽ���ı�




            //------------------------------------------------------------
            // �����������»���һ���ı���ʹ�ı�������ӰЧ��
            //------------------------------------------------------------

            // �����������ɫ��͸���� (͸��������Ϊ153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            // �����ı���ͼƬ�е�ָ��λ��
            grPhoto.DrawString(
                Text,										// �ı���Ϣ
                crFont,										// �ı�����
                semiTransBrush,								// ��ˢ
                new PointF(xCenterOfImg, yPosFromBottom),	// �ı���ͼƬ�е�λ��
                StrFormat);									// ��ʽ���ı�


            grPhoto.Dispose();
            imgPhoto.Dispose();
            imgPhoto = bmPhoto;

            //------------------------------------------------------------
            // ���Ĳ�������ͼƬ
            //------------------------------------------------------------
            if (!string.IsNullOrEmpty(SaveImage))
                imgPhoto.Save(SaveImage, ImageFormat.Jpeg);
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //�����ˮӡ�����ͼƬ
                imgPhoto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            }

            // �ͷ�ʹ���е���Դ
            bmPhoto.Dispose();
            imgPhoto.Dispose();
        }

        /// <summary>
        /// ��һ��ͼƬ��ָ��λ�ô�����һ�ž���ˮӡЧ����ͼƬ
        /// </summary>
        /// <param name="SourceImage">ָ��ԴͼƬ�ľ���·��</param>
        /// <param name="WaterMarkImage">ָ��ˮӡͼƬ�ľ���·��</param>
        /// <param name="wmPos">ָ��λ�ã�0����,1����,2����,3����,4����,5����,6����</param>
        /// <param name="SaveImage">����ͼƬ�ľ���·��</param>
        public static void GetWaterMarkPicImage(string SourceImage, string WaterMarkImage, int wmPos, string SaveImage, int w, int h)
        {
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImage);
            // ��ȡ��ԴͼƬ�Ŀ�Ⱥ͸߶�
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

            // ����һ���������ڲ�����Ҫ��ˮӡ��ԴͼƬ
            //imgPhoto = thumbnailImage;
            imgPhoto = System.Drawing.Image.FromFile(SourceImage);


            // ����һ��BMP��ʽ�Ŀհ�ͼƬ(��Ⱥ͸߶���ԴͼƬһ��)
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // ���ø��½��հ�BMPͼƬ�ķֱ���
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // ����BMPͼƬ���ó�һ��ͼ�ζ���
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            // ��������ͼƬ������
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // ��ԴͼƬ�������½���BMPͼƬ��
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 


            // ����ˮӡͼƬ�� Image ����
            System.Drawing.Image imgWatermark = new Bitmap(WaterMarkImage);

            // ��ȡˮӡͼƬ�Ŀ�Ⱥ͸߶�
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //------------------------------------------------------------
            // ��һ���� ����ˮӡͼƬ
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

            //ˮӡλ�ü���
            int x = 0; //x����
            int y = 0; //y����

            switch (wmPos)
            {
                case 0: //����,
                    x = 10;
                    y = 10;
                    break;
                case 1: //����,
                    x = 10;
                    y = ((phHeight - wmHeight) - 10);
                    break;
                case 2://����,
                    x = ((phWidth - wmWidth) / 2);
                    y = 10;
                    break;
                case 3://����,
                    x = ((phWidth - wmWidth) / 2);
                    y = ((phHeight - wmHeight) / 2);
                    break;
                case 4://����,
                    x = ((phWidth - wmWidth) / 2);
                    y = ((phHeight - wmHeight) - 10);
                    break;
                case 5://����,
                    x = ((phWidth - wmWidth) - 10);
                    y = 10;
                    break;
                case 6://����
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
            // ������������ͼƬ
            //------------------------------------------------------------


            if (!string.IsNullOrEmpty(SaveImage))
                imgPhoto.Save(SaveImage, ImageFormat.Jpeg);
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //�����ˮӡ�����ͼƬ
                imgPhoto.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Jpeg";
                HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            }


            // �ͷ�ʹ���е���Դ
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            bmWatermark.Dispose();
        }

        /// <summary>
        /// ��һ��ͼƬ��ָ��λ�ô�����һ�ž���ˮӡЧ����ͼƬ��һ���ı�
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
            // ����һ���������ڲ�����Ҫ��ˮӡ��ԴͼƬ
            System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(SourceImage);
            // ��ȡ��ԴͼƬ�Ŀ�Ⱥ͸߶�
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            // ����һ��BMP��ʽ�Ŀհ�ͼƬ(��Ⱥ͸߶���ԴͼƬһ��)
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // ���ø��½��հ�BMPͼƬ�ķֱ���
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // ����BMPͼƬ���ó�һ��ͼ�ζ���
            Graphics grPhoto = Graphics.FromImage(bmPhoto);


            // ��������ͼƬ������
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            // ��ԴͼƬ�������½���BMPͼƬ��
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 



            //------------------------------------------------------------
            // ��һ�������ò����ı���Ϣ���������
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
            // �ڶ�������һ�λ����ı���Ϣ
            //------------------------------------------------------------

            // �����������ɫ��͸���� (͸��������Ϊ153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(100, 0, 0, 0));

            // �����ı���ͼƬ�е�ָ��λ��
            grPhoto.DrawString(
                Text,												// �ı���Ϣ
                crFont,												// �ı�����
                semiTransBrush2,									// ��ˢ
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),	// �ı���ͼƬ�е�λ��
                StrFormat);											// ��ʽ���ı�




            //------------------------------------------------------------
            // �����������»���һ���ı���ʹ�ı�������ӰЧ��
            //------------------------------------------------------------

            // �����������ɫ��͸���� (͸��������Ϊ153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

            // �����ı���ͼƬ�е�ָ��λ��
            grPhoto.DrawString(
                Text,										// �ı���Ϣ
                crFont,										// �ı�����
                semiTransBrush,								// ��ˢ
                new PointF(xCenterOfImg, yPosFromBottom),	// �ı���ͼƬ�е�λ��
                StrFormat);									// ��ʽ���ı�


            //------------------------------------------------------------
            // ���Ĳ��� ����ˮӡͼƬ
            //------------------------------------------------------------

            // ����ˮӡͼƬ�� Image ����
            System.Drawing.Image imgWatermark = new Bitmap(WaterMarkImage);

            // ��ȡˮӡͼƬ�Ŀ�Ⱥ͸߶�
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
            // ���岽������ͼƬ
            //------------------------------------------------------------
            imgPhoto.Save(SaveImage, ImageFormat.Jpeg);


            // �ͷ�ʹ���е���Դ
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            bmWatermark.Dispose();
        }




        /// <summary>
        /// ������֤��ͼƬ
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
            crypticFonts[3] = "����";
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

            // ���������MIME����
            //
            context.Response.ContentType = "image/gif";

            // ����ļ������������
            //
            raster.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

            // �ͷ���Դ
            //
            context.Response.Flush();
            context.Response.End();
        }
        #endregion

        #region static public method

        /// <summary>
        /// ���ָ��ͼƬ�Ƿ���ϱ�׼
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
    /// ˮӡЧ����ԴͼƬ�е�λ��
    /// </summary>
    public enum wmPosition
    {
        /// <summary>
        /// ���Ծ���
        /// </summary>
        MM = 0,

        /// <summary>
        /// ����
        /// </summary>
        LT = 1,

        /// <summary>
        /// ����
        /// </summary>
        LB = 2,

        /// <summary>
        /// ����
        /// </summary>
        CT = 3,

        /// <summary>
        /// ����
        /// </summary>
        CB = 4,

        /// <summary>
        /// ����
        /// </summary>
        RT = 5,

        /// <summary>
        /// ����
        /// </summary>
        RB = 6
    }
}
