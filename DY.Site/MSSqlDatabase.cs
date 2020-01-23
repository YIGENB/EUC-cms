using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Configuration;

using SQLDMO;

namespace DY.Site
{
    /**/
    /// <summary>
    /// DbOper类,主要应用SQLDMO实现对Microsoft SQL Server数据库的备份和恢复
    /// </summary>
    public class MSSqlDatabase
    {
        public string server {get;set;}
        public string user {get;set;}
        public string password {get;set;}

        /**/
        /// <summary>
        /// DbOper类的构造函数
        /// </summary>
        public MSSqlDatabase(string _server,string _user,string _password)
        {
            this.server = _server;
            this.user = _user;
            this.password = _password;
        }

        /**/
        /// <summary>
        /// 数据库备份
        /// </summary>
        /// <param name="path">数据库备份的完整路径</param>
        /// <param name="database">要进行备份的数据库名</param>
        public string DbBackup(string path,string database)
        {
            SQLServer svr = new SQLServerClass();
            try
            {
                svr.Connect(this.server, this.user, this.password);
                Backup bak = new BackupClass();
                bak.Action = 0;
                bak.Initialize = true;
                bak.Files = path;
                bak.Database = database;
                bak.SQLBackup(svr);
                return null;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                svr.DisConnect();
            }
        }

        /**/
        /// <summary>
        /// 数据库恢复
        /// </summary>
        public string DbRestore(string path,string database)
        {
            SQLServer svr = new SQLServerClass();
            try
            {
                svr.Connect(this.server, this.user, this.password);
                QueryResults qr = svr.EnumProcesses(-1);
                int iColPIDNum = -1;
                int iColDbName = -1;
                for (int i = 1; i <= qr.Columns; i++)
                {
                    string strName = qr.get_ColumnName(i);
                    if (strName.ToUpper().Trim() == "SPID")
                        iColPIDNum = i;
                    else if (strName.ToUpper().Trim() == "DBNAME")
                        iColDbName = i;

                    if (iColPIDNum != -1 && iColDbName != -1)
                        break;
                }

                for (int i = 1; i <= qr.Rows; i++)
                {
                    string strDBName = qr.GetColumnString(i, iColDbName);
                    if (strDBName.ToUpper() == database.ToUpper())
                        svr.KillProcess(qr.GetColumnLong(i, iColPIDNum));
                }

                Restore res = new RestoreClass();
                res.Action = 0;
                res.Files = path;
                res.Database = database;
                res.ReplaceDatabase = true;
                res.SQLRestore(svr);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                svr.DisConnect();
            }
        }
    }
}
