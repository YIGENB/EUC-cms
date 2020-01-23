using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Collections;
using DY.Common;
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
    public partial class hct : AdminPage
    {
        /// <summary>
        /// 定义本页hashtable以供模板引擎使用
        /// </summary>
        protected IDictionary context = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {

            //检测权限
            this.IsChecked("goods_import");

            if (ispost)
            {
                if (string.IsNullOrEmpty(DYRequest.getFormString("excel")) && string.IsNullOrEmpty(DYRequest.getFormString("excel_tyle")))
                {
                    base.DisplayTemplate(context, "systems/excel");
                    return;
                }

                int type=DYRequest.getFormInt("excel_tyle");

                DataTable table = GetExcelData(Server.MapPath(DYRequest.getFormString("excel")));

                if (table.Rows.Count < 1)
                { return; }

                int k = 0;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    switch(type)
                    {
                        case 2:
                            #region 导入刮刮卡
                            string card_num = table.Rows[i][1].ToString();
                            if (string.IsNullOrEmpty(card_num))
                            {
                                continue;
                            }
                            CardInfo entity = new CardInfo();
                            entity.card_num = card_num;
                            entity.is_enabled = true;
                            entity.is_validated = false;
                            entity.use_time = null;
                            entity.user_id = 0;

                            SiteBLL.InsertCardInfo(entity);
                            #endregion
                            break;
                        case 1:
                            #region 导入会员
                            UsersInfo usersentity = new UsersInfo();
                            usersentity.user_name = table.Rows[i][0].ToString();
                            usersentity.password = SiteUtils.Encryption(table.Rows[i][1].ToString());
                            usersentity.email = table.Rows[i][2].ToString();
                            usersentity.question = table.Rows[i][3].ToString();
                            usersentity.answer = table.Rows[i][4].ToString();
                            usersentity.sex = table.Rows[i][5].ToString() == "男" ? 1 : 2;
                            //usersentity.mobile = table.Rows[i][6].ToString();
                            //usersentity.address = table.Rows[i][7].ToString();
                            //usersentity.birthday = table.Rows[i][7].ToString();
                            //usersentity.answer = table.Rows[i][5].ToString();
                            //usersentity.answer = table.Rows[i][5].ToString();
                            //usersentity.answer = table.Rows[i][5].ToString();
                            //usersentity.answer = table.Rows[i][5].ToString();
                            usersentity.last_ip = "";
                            //usersentity.user_id = 0;

                            SiteBLL.InsertUsersInfo(usersentity);
                            #endregion
                        break;

                }
                    k++;
                }
                //删除文件
                System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(DYRequest.getFormString("excel")));
                if (file.Exists)
                {
                    file.Delete();
                }

                //日志记录
                base.AddLog("导入数据");

                //显示提示信息
                Hashtable links = new Hashtable();
                links.Add("继续添加", "?act=add");
                //显示提示信息
                this.DisplayMessage("成功导入" + k + "条数据", 2, "", links);
            }

            base.DisplayTemplate(context, "systems/excel");
        }


        /// <summary>    
        /// 获取链接字符串     
        /// </summary>    
        /// <param name="strFilePath"></param>   
        /// <returns></returns>   
        public string GetExcelConnection(string strFilePath)
        {
            if (!File.Exists(strFilePath)) { throw new Exception("指定的Excel文件不存在！"); }
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended properties='Excel 8.0;Imex=1;HDR=Yes;'";
        }


        /// <summary> 获取指定路径、指定工作簿名称的Excel数据 </summary> 
        /// <param name="FilePath">文件存储路径</param>   
        /// <param name="WorkSheetName">工作簿名称</param>  
        /// <returns>如果争取找到了数据会返回一个完整的Table，否则返回异常</returns>    
        public DataTable GetExcelData(string FilePath, string WorkSheetName)
        {
            DataTable dtExcel = new DataTable();
            OleDbConnection con = new OleDbConnection(GetExcelConnection(FilePath));
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from [" + WorkSheetName + "$]", con);
            adapter.Fill(dtExcel);
            con.Close();
            return dtExcel;
        }

        /// <summary> 返回指定文件所包含的工作簿列表;如果有WorkSheet，就返回以工作簿名字命名的ArrayList，否则返回空 </summary>   
        /// <param name="strFilePath">要获取的Excel</param>   
        /// <returns>如果有WorkSheet，就返回以工作簿名字命名的ArrayList，否则返回空</returns>   
        public ArrayList GetExcelWorkSheets(string strFilePath)
        {
            ArrayList alTables = new ArrayList();
            OleDbConnection odn = new OleDbConnection(GetExcelConnection(strFilePath));
            odn.Open();
            DataTable dt = new DataTable();
            dt = odn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null) { throw new Exception("无法获取指定Excel的架构。"); }
            foreach (DataRow dr in dt.Rows)
            {
                string tempName = dr["Table_Name"].ToString(); int iDolarIndex = tempName.IndexOf('$');
                if (iDolarIndex > 0) { tempName = tempName.Substring(0, iDolarIndex); }
                //修正了Excel2003中某些工作薄名称为汉字的表无法正确识别的BUG。           
                if (tempName[0] == '\'')
                {
                    if (tempName[tempName.Length - 1] == '\'')
                    { tempName = tempName.Substring(1, tempName.Length - 2); }
                    else { tempName = tempName.Substring(1, tempName.Length - 1); }
                }
                if (!alTables.Contains(tempName)) { alTables.Add(tempName); }
            } odn.Close();
            if (alTables.Count == 0) { return null; } return alTables;
        }

        /// <summary>获取指定路径、指定工作簿名称的Excel数据:取第一个sheet的数据   
        /// </summary>   
        /// <param name="FilePath">文件存储路径</param>   
        /// <param name="WorkSheetName">工作簿名称</param>    
        /// <returns>如果争取找到了数据会返回一个完整的Table，否则返回异常</returns>   
        public DataTable GetExcelData(string astrFileName)
        {
            string strSheetName = GetExcelWorkSheets(astrFileName)[0].ToString();
            return GetExcelData(astrFileName, strSheetName);
        }
    }
}
