using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Config;
using DY.Entity;
using DY.Data;
using DY.Cache;

namespace DY.Site
{
    /// <summary>
    /// 万能表单操作类
    /// </summary>
    public partial class Caches
    {
        #region 万能表单
        /// <summary>
        /// 取得表单
        /// </summary>
        /// <returns></returns>
        public ArrayList GetForm_Position()
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/Form/Position") as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetFormPositionAllList("", "");

                cache.AddObject("/DY/Web/Form/Position", data);
            }
            return data;
        }

        /// <summary>
        /// 获取表弹名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Getform_positioinname(int id)
        {
            string data = SiteBLL.GetFormPositionValue("position_name", "position_id=" + id).ToString();
            return data;
        }
        #endregion

        #region 获取单个控件
        /// <summary>
        /// 设置html控件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetControl(string name)
        {
            //string html="";
            StringBuilder html = new StringBuilder();
            FormAllInfo list = SiteBLL.GetFormAllInfo("class_name='" + name + "'");
            string t = list.type;
            html.AppendFormat("<input type=\"hidden\" value=\"{0}\" name=\"allform_id[{1}]\" />", list.allform_id, list.allform_id);
            html.AppendFormat("<input type=\"hidden\" value=\"{0}\" name=\"parent_id[{1}]\" />", list.parent_id, list.allform_id);
            if (list.type == "text")
            {
                html.AppendFormat("<input type=\"text\" class=\"{0}\" name=\"value[{1}]\" />", list.class_name, list.allform_id);
            }
            else if (list.type == "textarea")
            {
                html.AppendFormat("<textarea class=\"{0}\" name=\"value[{1}]\"></textarea>", list.class_name, list.allform_id);
            }
            else if (list.type == "checkbox")
            {
                foreach (string str in new SiteUtils().Split(list.store_range.ToString(), ","))
                {
                    html.AppendFormat("<input type=\"checkbox\"  class=\"{0}\" value=\"{1}\" name=\"value[{2}]\" />{3}", list.class_name, str, list.allform_id, str);
                }
            }
            else if (list.type == "radio")
            {
                foreach (string str in new SiteUtils().Split(list.store_range.ToString(), ","))
                {
                    html.AppendFormat("<input type=\"radio\"  class=\"{0}\" value=\"{1}\" name=\"value[{2}]\" />{3}", list.class_name, list.allform_id, str, str);
                }
            }
            else if (list.type == "select")
            {
                html.AppendFormat("<select name=\"value[{0}]\"  class=\"{1}\" id=\"{2}\">", list.allform_id, list.class_name, list.class_name);
                foreach (string str in new SiteUtils().Split(list.store_range.ToString(), ","))
                {
                    html.AppendFormat("<option value=\"{0}\">{1}</option>", str, str);
                }
                html.AppendFormat("</select>");
            }
            return html.ToString();
        }
        #endregion

        #region 取得控件列表
        /// <summary>
        /// 取得控件列表
        /// </summary>
        /// <param name="pos_id">表单ID</param>
        /// <returns></returns>
        public static DataTable GetForm(int pos_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/Form/t" + pos_id) as DataTable;
            if (data == null)
            {
                data = DatabaseProvider.GetInstance().GetForm(pos_id);

                cache.AddObject("/DY/Web/Form/t" + pos_id, data);
            }
            return data;
        }
        #endregion

        #region 取得控件值列表
        /// <summary>
        /// 取得控件值列表
        /// </summary>
        /// <param name="allform_id">控件ID</param>
        /// <returns></returns>
        public static ArrayList GetFormValue(int allform_id)
        {
            DYCache cache = DYCache.GetCacheService();
            ArrayList data = cache.RetrieveObject("/DY/Web/FormValue/t" + allform_id) as ArrayList;
            if (data == null)
            {
                data = SiteBLL.GetFromvalueAllList("", "allform_id=" + allform_id );

                cache.AddObject("/DY/Web/FormValue/t" + allform_id, data);
            }
            return data;
        }



        /// <summary>
        /// 取得控件值session_id列表(去重复)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFormValueSessionId(int position_id)
        {
            DYCache cache = DYCache.GetCacheService();
            DataTable data = cache.RetrieveObject("/DY/Web/FormValueSessionId/t" + position_id) as DataTable;
            if (data == null)
            {
                data = DatabaseProvider.GetInstance().GetFormvalueSessionId(position_id);
            }
            return data;
        }
        #endregion
    }
}
