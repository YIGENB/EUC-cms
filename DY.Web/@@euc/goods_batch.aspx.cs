/**
 * 功能描述：Goods管理类
 * 创建时间：2010-1-29 14:20:15
 * 最后修改时间：2010-1-29 14:20:15
 * 作者：gudufy
 * ============================================================================
 * 2009-2015 小K版权所有，并保留所有权利
 * 联系邮箱：421643133@qq.com、QQ：421643133
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 */
using System;
using System.Collections;
using System.Data;
using System.Web;

using DY.Common;
using DY.Config;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class goods_batch : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 批量导入
            if (this.act == "add")
            {
                //检测权限
                this.IsChecked("goods_add");

                if (base.ispost)
                {
                    //获取上传对象
                    HttpPostedFile file = Request.Files["file"];

                    if (file != null && file.ContentLength > 0)
                    {
                        //上传到临时文件夹
                        string[] fileinfo = CommonUtils.UploadFile(file, "csv", 0, true, Server.MapPath(BaseConfig.TemporaryFolder));

                        DataTable dt = new CSVParser(Server.MapPath(BaseConfig.TemporaryFolder + fileinfo[1])).ParseToDataTable();

                        IDictionary context = new Hashtable();
                        context.Add("dt", dt);

                        base.DisplayTemplate(context, "goods/goods_import_in");
                    }
                    else
                    {
                        base.DisplayMessage("请选择要导入的csv文件", 1);
                    }
                }

                base.DisplayTemplate("goods/goods_batch");
            }
            #endregion

            #region 导出
            else if (this.act == "download")
            {
                Response.ContentType = "application/vnd.ms-excel; charset=utf-8";
                Response.AddHeader("Content-Disposition", "attachment;filename=goods.csv");
                Response.ContentEncoding = System.Text.Encoding.Default;

                base.DisplayTemplate("import/goods_import_in");
            }
            #endregion

            #region 分析提交的csv文件
            else if (this.act == "import")
            {
                
            }
            #endregion
        }
    }
}
