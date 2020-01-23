/**
 * 功能描述：Database管理类
 * 创建时间：2010-1-29 12:53:07
 * 最后修改时间：2010-1-29 12:53:07
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
using System.Configuration;
using System.Data;
using System.Web;

using DY.Common;
using DY.Site;
using DY.Entity;
using DY.Data;

namespace DY.Web.admin
{
    public partial class database : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] dbconn = DbHelper.ConnectionString.Split(';');//ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.Split(';');

            string local="",user="",pass="",database="" ;

            #region 绑定数据库链接串信息
            foreach (string info in dbconn)
            {
                if (info.ToLower().IndexOf("data source") >= 0 || info.ToLower().IndexOf("server") >= 0)
                {
                    local = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("user id") >= 0 || info.ToLower().IndexOf("uid") >= 0)
                {
                    user = info.Split('=')[1].Trim();
                    continue;
                }
                if (info.ToLower().IndexOf("password") >= 0 || info.ToLower().IndexOf("pwd") >= 0)
                {
                    pass = info.Split('=')[1].Trim();
                    continue;
                }

                if (info.ToLower().IndexOf("initial catalog") >= 0 || info.ToLower().IndexOf("database") >= 0)
                {
                    database = info.Split('=')[1].Trim();
                    break;
                }
            }
            #endregion

            MSSqlDatabase _db = new MSSqlDatabase(local, user, pass);

            string bak_path = Server.MapPath("/backup/database/");

            if (!FileOperate.IsExist(bak_path, FileOperate.FsoMethod.Folder))
                FileOperate.Create(bak_path, FileOperate.FsoMethod.Folder);

            #region 备份
            if (this.act == "backup")
            {
                //检测权限
                this.IsChecked("database_backup");

                if (ispost)
                {
                    //日志记录
                    base.AddLog("备份数据库");

                    string msg = _db.DbBackup("[" + bak_path + "CTMON_" + DYRequest.getForm("bak_name") + ".bak]", database);

                    if (msg == null)
                        msg = "数据库已经成功备份";

                    //记录日志
                    this.AddLog("备份数据库");

                    base.DisplayMessage(msg, 2);
                }

                IDictionary context = new Hashtable();
                context.Add("back_name", DateTime.Now.ToString("yyyyMMddhhmmss"));
                context.Add("IsBackup", Database.IsBackup());

                base.DisplayTemplate(context, "database/database_backup");
            } 
            #endregion

            #region 还原
            else if (this.act == "restore" || this.act == "list")
            {
                //检测权限
                this.IsChecked("database_restore"); 
               
                if (ispost)
                {
                    //日志记录
                    base.AddLog("还原数据库");

                    string msg = _db.DbRestore("[" + bak_path + Request.Form["bak_name"] + "]", database);

                    if (msg == null)
                        msg = "数据库已经成功还原";

                    //记录日志
                    this.AddLog("还原数据库");

                    base.DisplayMessage(msg, 2);
                }

                DataTable dt = FileOperate.searchDirectoryAllInfo(bak_path, "*.bak");
                DataView dv = dt.DefaultView;
                dv.Sort = SiteUtils.GetSortOrder("creatime desc");
                dt = dv.ToTable();

                IDictionary context = new Hashtable();
                context.Add("bak_list", dt);
                
                context.Add("IsRestore", Database.IsRestore());
                //to json
                context.Add("sort_by", DYRequest.getRequest("sort_by"));
                context.Add("sort_order", DYRequest.getRequest("sort_order"));

                base.DisplayTemplate(context, "database/database_restore",base.isajax);
            } 
            #endregion

            #region 删除
            else if (this.act == "del")
            { 
                //检测权限
                this.IsChecked("database_bak_del");

                FileOperate.Delete(bak_path + DYRequest.getRequest("file"), FileOperate.FsoMethod.File);

                //记录日志
                this.AddLog("删除数据库备份文件：" + DYRequest.getRequest("file"));

                base.DisplayMessage("成功删除数据库备份文件：" + DYRequest.getRequest("file"), 2);
            }
            #endregion
        }
    }
}
