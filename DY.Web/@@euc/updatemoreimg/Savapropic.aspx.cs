using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using DY.Common;
using DY.Site;
using DY.Entity;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;


namespace DY.Web.admin.updatemoreimg
{
    public partial class Savapropic : AdminPage
    {
        public string directoryPath ="/Upload/serviceimage";
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (base.act)
            {
                case "InsertGrab":
                    if (ispost)
                        InsertGrab();
                    break;
                case "InsertGrabs":
                    if (ispost)
                        InsertGrabs();
                    break;
                case "Deletedpic":
                    if (ispost)
                        Deletedpic();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InsertGrabs()
        {
            string fileName = "";
            string fileExtension = "";
            string filePath = "";
            string fileNewName = "";
            string uploadTypes = ".gif|.jpg|.jpeg|.png|.bmp";
            StringBuilder sbImgNames = new StringBuilder();
            try
            {
                //目录如果不存在先创建
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                for (int i = 1; i <= Request.Files.Count; i++)
                {
                    HttpPostedFile file = Request.Files[i - 1];
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = file.FileName;
                        //得到扩展名
                        fileExtension = Path.GetExtension(fileName);
                        //创建文件夹
                        string dirPath = directoryPath + "/";
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                        dirPath += ymd + "/";
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }

                        //设置文件名（1#Guid.jpg   序号 + # + Guid + 扩展名
                        fileNewName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExtension;
                        

                        #region 服务器端校验
                        //大小
                        if (file.ContentLength > 5 * 1024 * 1024)
                        {
                            continue;
                        }
                        //扩展名 使用正则匹配
                        if (!Regex.IsMatch(fileExtension, uploadTypes, RegexOptions.IgnoreCase))
                        {
                            continue;
                        }
                        #endregion
                        //路径合并
                        filePath = Path.Combine(dirPath, fileNewName);
                        //保存文件
                        file.SaveAs(Server.MapPath(filePath));
                        int ico_w = Utils.StrToInt(config.GoodsImgIco.Split('*')[0], 0);
                        int ico_h = Utils.StrToInt(config.GoodsImgIco.Split('*')[1], 0);
                        int list_w = Utils.StrToInt(config.GoodsImgList.Split('*')[0], 0);
                        int list_h = Utils.StrToInt(config.GoodsImgList.Split('*')[1], 0);
                        int info_w = Utils.StrToInt(config.GoodsImgInfo.Split('*')[0], 0);
                        int info_h = Utils.StrToInt(config.GoodsImgInfo.Split('*')[1], 0);
                        GoodsGalleryInfo gg = new GoodsGalleryInfo();
                        gg.goods_id = 0;
                        //gg.goods_thumb = DYRequest.getForm("goods_thumb");
                        //gg.goods_img = DYRequest.getForm("goods_img");
                        //gg.info_img = DYRequest.getForm("info_img");
                        string info_img = filePath;
                        gg.original_img = filePath;
                        gg.admin_user_id = base.userid;
                        gg.order_id = 0;
                        gg.img_desc = "";
                        if (!string.IsNullOrEmpty(info_img) && Utils.FileExists(Server.MapPath(info_img)))
                        {
                            if (config.WatermarkGoods && Utils.FileExists(Server.MapPath(config.WatermarkPic)))
                            {
                                WebGDI.GetWaterMarkPicImage(Server.MapPath(info_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(info_img.Replace("_", "info_")), info_w, info_h);
                                WebGDI.GetWaterMarkPicImage(Server.MapPath(info_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(info_img.Replace("_", "list_")), list_w, list_h);
                                //WebGDI.GetWaterMarkPicImage(Server.MapPath(entity.original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(entity.original_img.Replace("_", "ico_")), ico_w, ico_h);
                                WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "ico_")), ico_w, ico_h);
                            }
                            else
                            {
                                //生成缩略图(详细页)
                                WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "info_")), info_w, info_h);
                                //生成缩略图(列表页)
                                WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "list_")), list_w, list_h);
                                //生成缩略图(ico)
                                WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "ico_")), ico_w, ico_h);
                            }
                        }
                        gg.info_img = info_img.Replace("_", "info_");
                        gg.goods_thumb = gg.original_img.Replace("_", "ico_");
                        gg.goods_img = gg.original_img.Replace("_", "list_");
                        Goods.InsertGoodsGallery(gg);
                        string original_img = SiteBLL.GetGoodsGalleryValue("img_id", "original_img='" + filePath + "'").ToString();

                        base.DisplayMemoryTemplate(base.MakeJson(original_img, 0, filePath));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        

        /// <summary>
        /// 批量增加图片
        /// </summary>
        protected void InsertGrab()
        {
            int ico_w = Utils.StrToInt(config.GoodsImgIco.Split('*')[0], 0);
            int ico_h = Utils.StrToInt(config.GoodsImgIco.Split('*')[1], 0);
            int list_w = Utils.StrToInt(config.GoodsImgList.Split('*')[0], 0);
            int list_h = Utils.StrToInt(config.GoodsImgList.Split('*')[1], 0);
            int info_w = Utils.StrToInt(config.GoodsImgInfo.Split('*')[0], 0);
            int info_h = Utils.StrToInt(config.GoodsImgInfo.Split('*')[1], 0);
            GoodsGalleryInfo gg = new GoodsGalleryInfo();
            gg.goods_id = DYRequest.getFormInt("goods_id");
            //gg.goods_thumb = DYRequest.getForm("goods_thumb");
            //gg.goods_img = DYRequest.getForm("goods_img");
            //gg.info_img = DYRequest.getForm("info_img");
            string info_img = DYRequest.getForm("info_img");
            gg.original_img = DYRequest.getForm("original_img");
            gg.admin_user_id = DYRequest.getFormInt("admin_user_id");
            gg.order_id = DYRequest.getFormInt("order_id");
            gg.img_desc = "";
            if (!string.IsNullOrEmpty(info_img) && Utils.FileExists(Server.MapPath(info_img)))
            {
                if (config.WatermarkGoods && Utils.FileExists(Server.MapPath(config.WatermarkPic)))
                {
                    WebGDI.GetWaterMarkPicImage(Server.MapPath(info_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(info_img.Replace("_", "info_")), info_w, info_h);
                    WebGDI.GetWaterMarkPicImage(Server.MapPath(info_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(info_img.Replace("_", "list_")), list_w, list_h);
                    //WebGDI.GetWaterMarkPicImage(Server.MapPath(entity.original_img), Server.MapPath(config.WatermarkPic), config.WatermarkPlace, Server.MapPath(entity.original_img.Replace("_", "ico_")), ico_w, ico_h);
                    WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "ico_")), ico_w, ico_h);
                }
                else
                {
                    //生成缩略图(详细页)
                    WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "info_")), info_w, info_h);
                    //生成缩略图(列表页)
                    WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "list_")), list_w, list_h);
                    //生成缩略图(ico)
                    WebGDI.SendSmallImage(Server.MapPath(info_img), Server.MapPath(info_img.Replace("_", "ico_")), ico_w, ico_h);
                }
            }
            gg.info_img = info_img.Replace("_", "info_");
            gg.goods_thumb = gg.original_img.Replace("_", "ico_");
            gg.goods_img = gg.original_img.Replace("_", "list_");
            Goods.InsertGoodsGallery(gg);
            string original_img = SiteBLL.GetGoodsGalleryValue("img_id", "original_img='" + DYRequest.getForm("original_img") + "'").ToString();
           
            base.DisplayMemoryTemplate(base.MakeJson(original_img, 0, ""));
        }
        /// <summary>
        /// 单个删除图片
        /// </summary>
        protected void Deletedpic()
        {
            if (DYRequest.getFormInt("img_id") > 0)
            {
                //删除文件
                GoodsGalleryInfo goodsgallery = SiteBLL.GetGoodsGalleryInfo(DYRequest.getFormInt("img_id"));
                string goods_thumb = Server.MapPath(goodsgallery.goods_thumb);
                string goods_img = Server.MapPath(goodsgallery.goods_img);
                string info_img = Server.MapPath(goodsgallery.info_img);
                string original_img = Server.MapPath(goodsgallery.original_img);
                //if (FileOperate.IsExist(goods_thumb, FileOperate.FsoMethod.File))
                FileOperate.Delete(goods_thumb, FileOperate.FsoMethod.File);
                FileOperate.Delete(goods_img, FileOperate.FsoMethod.File);
                FileOperate.Delete(info_img, FileOperate.FsoMethod.File);
                FileOperate.Delete(original_img, FileOperate.FsoMethod.File);

                Goods.DeletedGoodsGallery("img_id=" + DYRequest.getFormInt("img_id"));

            }
            else
            {
                string filename = DYRequest.getForm("file");
                string filePath=directoryPath + filename;
                if (FileOperate.IsExist(filePath, FileOperate.FsoMethod.File))
                    FileOperate.Delete(filePath, FileOperate.FsoMethod.File);
            }
         
        }

    }
}