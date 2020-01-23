using System;
using System.Text;
using DY.ScheduledEvents;
using DY.Site;
using System.Collections;
using DY.Common;
using DY.Config;
using DY.Data;
using DY.Entity;
using DY.LanguagePack;
using NVelocityTemplateEngine.Interfaces;
using NVelocityTemplateEngine;
using System.Web;
using System.Data;

namespace DY.Event
{
    /// <summary>
    /// �йر������ݿ�ļƻ�����
    /// </summary>
    public class �������ݿ� : IEvent
    {
        #region IEvent ��Ա

        void IEvent.Execute(object state)
        {
            try
            {
                this.BackDB();
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog(e.ToString());
            }
        }

        #endregion

        /// <summary>
        /// �������ݿ�
        /// </summary>
        private void BackDB()
        {
            string[] dbconn = DbHelper.ConnectionString.Split(';');

            string local = "", user = "", pass = "", database = "";

            #region �����ݿ����Ӵ���Ϣ
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
                    user = "publicback";
                    continue;
                }
                if (info.ToLower().IndexOf("password") >= 0 || info.ToLower().IndexOf("pwd") >= 0)
                {
                    pass = info.Split('=')[1].Trim();
                    pass = "Chuangtm@9";
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

            string bak_path = System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"backup/database/";

            if (!FileOperate.IsExist(bak_path, FileOperate.FsoMethod.Folder))
                FileOperate.Create(bak_path, FileOperate.FsoMethod.Folder);

            _db.DbBackup("[" + bak_path + "DY_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak]", database);

        }


        /// <summary>
        /// ɾ��30��֮ǰ���ݵ������ļ�
        /// </summary>
        private void DeleteDbFile()
        {
            string bak_path = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "backup/database/";
            DataTable dt = FileOperate.searchDirectoryAllInfo(bak_path, "*.bak");
            foreach (DataRow dr in dt.Rows)
            {
                DateTime creatime = Convert.ToDateTime(dr["creatime"]);
                string filename=bak_path + dr["name"];
                if (SiteUtils.GetDateDiff(filename).Days > 30)
                {
                    FileOperate.Delete(filename, FileOperate.FsoMethod.File);
                }
            }
        }
    }
}
