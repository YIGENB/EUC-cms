/**
 * 功能描述：页面相关类
 * 创建时间：2010-1-29 15:17:25
 * 最后修改时间：2010-1-29 15:17:25
 * 作者：gudufy
 * ============================================================================
 * 2009-2010 杨毓强版权所有，并保留所有权利
 * 联系邮箱：gudufy@163.com、手机：15919862907、QQ：84383822
 * ----------------------------------------------------------------------------
 * 这不是一个自由软件！您只能在不用于商业目的的前提下对程序代码进行修改和
 * 使用；不允许对程序代码以任何形式任何目的的再发布。
 * ============================================================================
 * 文件名：Goods.cs
 * ID：fa304b76-21b9-405d-a574-2b877543e079
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DY.Common;
using DY.Data;
using DY.Entity;
using System.Collections;

namespace DY.Site
{
    /// <summary>
    /// CMS相关类
    /// </summary>
    public class CMS
    {
        #region 静态函数
        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="tags">标签列表</param>
        public static void SaveTag(string[] tags)
        {
            foreach (string str in tags)
            {
                if (SiteBLL.GetTagAllList("", "tag_name='" + str + "'").Count <= 0)
                {
                    TagInfo taginfo = new TagInfo(0, 0, str, "", FunctionUtils.Text.ConvertSpellFirst(str));

                    SiteBLL.InsertTagInfo(taginfo);
                }
            }
        }
        /// <summary>
        /// 插入文章分类
        /// </summary>
        /// <param name="catinfo"></param>
        public static int InsertCategory(CmsCatInfo catinfo)
        {
            return DatabaseProvider.GetInstance().InsertCMSCategory(catinfo);
        }
        /// <summary>
        /// 更新文章分类
        /// </summary>
        /// <param name="catinfo"></param>
        public static int UpdateCategory(CmsCatInfo catinfo)
        {
            return DatabaseProvider.GetInstance().UpdateCMSCategory(catinfo);
        }
        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="cat_id"></param>
        public static int DeleteCategory(int cat_id)
        {
            return DatabaseProvider.GetInstance().DeleteCMSCategory(cat_id);
        }
        /// <summary>
        /// 获取当前分类的所有父类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        //public static ArrayList GetPrevCMSCat(int cat_id)
        //{
        //    ArrayList list = new ArrayList();

        //    using (IDataReader sdr = DatabaseProvider.GetInstance().GetPrevCMSCat(cat_id))
        //    {
        //        while (sdr.Read())
        //        {
        //            CmsCatInfo catinfo = new CmsCatInfo();
        //            catinfo.cat_id = sdr.GetInt32(0);
        //            catinfo.cat_name = sdr.GetString(1);
        //            catinfo.urlrewriter = sdr.GetString(2);

        //            list.Add(catinfo);
        //        }
        //    }

        //    return list;
        //}
        #region 上级分类模板
        /// <summary>
        /// 获取上级分类信息
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public static CmsCatInfo GetPrevCMSCat(int parent_id)
        {
            return SiteBLL.GetCmsCatInfo(string.Format("cat_id='{0}'", parent_id));
        }
        #endregion

        /// <summary>
        /// 获取cms信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCMSFullInfo(string urlrewriter)
        {
            return DatabaseProvider.GetInstance().GetCMSFullInfo(urlrewriter);
        }
        /// <summary>
        /// 获取cms信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCMSFullInfo(int id)
        {
            return DatabaseProvider.GetInstance().GetCMSFullInfo(id);
        }

        #endregion

        #region 非静态函数
        /// <summary>
        /// 取得所有CMS分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCMSCatAllList()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("cms_cat", "*", "", "");
        }
        /// <summary>
        /// 取得所有CMS页面分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCMSPageAllList()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("cms_page", "*", "", "");
        }

        /// <summary>
        /// 取得所有自定义菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetWxMenuAllList()
        {
            return DatabaseProvider.GetInstance().GetAllDataToDataTable("weixin_menu", "*", "", "");
        }
        /// <summary>
        /// 获取商品分类信息
        /// </summary>
        /// <param name="parent_id">上级分类ID</param>
        /// <returns></returns>
        public ArrayList GetCMSCatList(int parent_id)
        {
            return SiteBLL.GetCmsCatAllList("sort_order asc", parent_id >= 0 ? "parent_id=" + parent_id : "");
        }
        /// <summary>
        /// 移动商品分类位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public static int MoveCmsPos(string act, int cat_id)
        {
            return DatabaseProvider.GetInstance().MoveCmsPos(act, cat_id);
        }
        /// <summary>
        /// 获取手机分类信息
        /// </summary>
        /// <param name="parent_id">上级分类ID</param>
        /// <returns></returns>
        public ArrayList GetMobileCMSCatList(int parent_id)
        {
            return SiteBLL.GetCmsCatAllList("sort_order asc", parent_id >= 0 ? "parent_id=" + parent_id : "is_mobile=1");
        }
        /// <summary>
        /// 获取当前分类下的所有子类Id，包括当前分类
        /// </summary>
        /// <param name="cat_id">当前分类ID</param>
        /// <returns></returns>
        public string GetCMSCatIds(int cat_id)
        {
            //string ids = "";
            //foreach (CmsCatInfo catinfo in GetCMSCatList(cat_id))
            //{
            //    ids += catinfo.cat_id + ",";
            //}


            //return ids + cat_id;
            string sql = "SELECT a.cat_id FROM " + DY.Config.BaseConfig.TablePrefix + "cms_cat a,CMS_Cid(" + cat_id + ") b WHERE a.cat_id=b.ID";
            return sql;
        }

        /// <summary>
        /// 获取当前分类下的所有子类Id以及下面所有子类的子类，包括当前分类
        /// </summary>
        /// <param name="cat_id">当前分类ID</param>
        /// <returns></returns>
        string idsall = "";
        public string GetCMSCatAllIds(int cat_id)
        {
            ArrayList array = GetCMSCatList(cat_id);
            if (array.Count > 0)
            {
                foreach (CmsCatInfo catinfo in GetCMSCatList(cat_id))
                {
                    if (catinfo.cat_id == catinfo.parent_id)
                    {
                        break;
                    }
                    if (catinfo.parent_id.Value != 0)
                    {
                        idsall += catinfo.cat_id + ",";
                        GetCMSCatAllIds(catinfo.cat_id.Value);
                    }
                    else
                    {
                        idsall += catinfo.cat_id + ",";
                    }
                }
            }

            return idsall + cat_id;
        }

        /// <summary>
        /// 取得某分类下有多少篇文章 
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        public int GetCMSCount(int cat_id)
        {
            object obj = SiteBLL.GetCmsValue("COUNT(article_id)", "cat_id in (" + GetCMSCatIds(cat_id) + ")");

            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 获取文章TAG信息
        /// </summary>
        /// <param name="tags">以逗号分隔的标签列表</param>
        /// <returns></returns>
        public ArrayList GetTags(string tags)
        {
            if (string.IsNullOrEmpty(tags))
                return null;
            StringBuilder sb = new StringBuilder("tag_id>0 and ");
            int i = 0;
            foreach (string str in tags.Split(','))
            {
                if (i == 0)
                    sb.Append("(");

                sb.Append("tag_name='" + str + "'");

                if (i < tags.Split(',').Length - 1)
                    sb.Append(" or ");

                if (i == tags.Split(',').Length - 1)
                    sb.Append(")");

                i++;
            }
            //throw new Exception(filter);
            return SiteBLL.GetTagAllList("", sb.ToString());
        }

        /// <summary>
        /// 获取文章TAG信息
        /// </summary>
        /// <param name="topN">条数</param>
        /// <param name="cat_id">分类ID</param>
        /// <param name="tags">以逗号分隔的标签列表</param>
        /// <returns></returns>
        public ArrayList TagsToCMS(int topN,int cat_id,string tags)
        {
            if (string.IsNullOrEmpty(tags))
                return null;
            StringBuilder sb = new StringBuilder("article_id>0 and ");
            if (cat_id>0)
                sb.Append("cat_id in(" + GetCMSCatIds(cat_id) + ") and ");
            sb.Append(" article_id in(SELECT article_id from " + DY.Config.BaseConfig.TablePrefix + "cms where ");
            int i = 0;
            foreach (string str in tags.Split(','))
            {
                sb.Append("tag like '%" + str + "%'");

                if (i < tags.Split(',').Length - 1)
                    sb.Append(" or ");

                i++;
            }
            sb.Append(")");
            //throw new Exception(filter);
            int ResultCount = 0;
            return SiteBLL.GetCmsList(1, topN, "sort_order desc,newid()", sb.ToString(), out ResultCount);
        }
        /// <summary>
        /// 获取上一篇下一篇文章
        /// </summary>
        /// <param name="article_id">当前文章ID</param>
        /// <param name="cat_id">当前文章分类ID</param>
        /// <param name="next">是否为下一条，否则为上一条</param>
        /// <returns></returns>
        public ArrayList GetPreNext(int article_id, int cat_id, bool next)
        {
            StringBuilder sb = new StringBuilder("cat_id=" + cat_id + " and is_show=1 and ");
            int ResultCount = 0;
            string sort = "";
            if (next)
            {
                sb.Append("article_id>" + article_id);
                sort = "article_id asc";
            }
            else
            {
                sb.Append("article_id<" + article_id);
                sort = "article_id desc";
            }

            return SiteBLL.GetCmsList(1, 1, sort, sb.ToString(), out ResultCount);
        }

        /// <summary>
        /// 获取评论回复
        /// </summary>
        /// <param name="comment_id"></param>
        /// <returns></returns>
        public string GetReplyComment(int comment_id)
        {
            CommentInfo cmtinfo = SiteBLL.GetCommentInfo("parent_id=" + comment_id);
            if (cmtinfo != null)
                return Utils.StrFormat(cmtinfo.content);

            return "";
        }
        #endregion

        public CmsInfo getCmsInfo(int cms_id)
        {
            return SiteBLL.GetCmsInfo(cms_id);
        }

        /// <summary>
        /// 取得类型信息
        /// </summary>
        /// <returns></returns>
        public ArrayList GetCmsTypeLists()
        {
            return SiteBLL.GetGoodsTypeAllList("cat_id asc", "enabled=1 and attr_type=1");
        }

        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public DataTable GetCmsAttr(int goods_id, int type_id)
        {
            return DatabaseProvider.GetInstance().GetNewsAttr(goods_id, type_id);
        }

        public ArrayList GetCmsSpec(int spec_value_id)
        {
            return SiteBLL.GetGoodsSpecIndexAllList("id desc", "spec_value_id=" + spec_value_id + "");
        }

        public ArrayList GetCmsSpec(int spec_value_id, int spec_value_ida, bool flag)
        {
            return SiteBLL.GetGoodsSpecIndexAllList("id desc", "spec_value_id in (" + spec_value_id + "," + spec_value_ida + ")");
        }
        #region 关联资讯操作类
        /// <summary>
        /// JSON生成商品关联数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string Get_json_cms_list(string filter)
        {
            string opt = "{\"error\":0,\"message\":\"\",\"content\":[";

            foreach (CmsInfo cmsinfo in SiteBLL.GetCmsAllList("article_id desc", "article_id > 0 and is_show=1 " + filter))
            {
                opt += "{\"value\":\"" + cmsinfo.article_id + "\",\"text\":\"" + cmsinfo.title + "\",\"data\":\"" + cmsinfo.tag + "\"},";
            }

            opt = opt.TrimEnd(',') + "]}";

            return opt;
        }

        #endregion
    }
}
