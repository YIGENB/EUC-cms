using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.tools
{
    public partial class verifyimagepage : PageBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string bgcolor = DYRequest.getRequest("bgcolor").Trim();
			int textcolor = DYRequest.getRequestInt("textcolor", 1);
            string[] bgcolorArray = bgcolor.Split(',');
            
            Color bg = Color.White;
            
            if (bgcolorArray.Length == 1 && bgcolor != string.Empty)
            {
                bg = Utils.ToColor(bgcolor);
            }
            else if (bgcolorArray.Length == 3 && Utils.IsNumericArray(bgcolorArray))
            {                
                bg = Color.FromArgb(Utils.StrToInt(bgcolorArray[0], 255), Utils.StrToInt(bgcolorArray[1], 255), Utils.StrToInt(bgcolorArray[2], 255));
            }

            string rnd = VerifyImage.GetRnd(config.CaptchaLength, config.CaptchaUseNum, config.CaptchaUseLow, config.CaptchaUseUpp, config.CaptchaUseSpe, config.CaptchaCustom);

            Session["DYCaptcha"] = rnd;

            VerifyImageInfo verifyimg = VerifyImage.GenerateImage(rnd,config.CaptchaWidth,config.CaptchaHeight,bg,textcolor);

            Bitmap image = verifyimg.Image;

            System.Web.HttpContext.Current.Response.ContentType = verifyimg.ContentType;

            image.Save(this.Response.OutputStream, verifyimg.ImageFormat);
        }
    }
}
