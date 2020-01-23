using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using DY.Entity;
using System.Collections;

namespace DY.Data
{
    /// <summary>
    /// 系统接口类
    /// </summary>
    public partial interface IDataProvider
    {
        /// <summary>
        /// 取得网站基本设置信息
        /// </summary>
        /// <returns></returns>
        DataTable GetConfigList(string filter);
        /// <summary>
        /// 获取网站设置数据
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        DbDataReader GetConfigData(int parent_id);
        /// <summary>
        /// 管理员是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        bool AdminExists(int uid);
        /// <summary>
        /// 管理员是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool AdminExists(string username);
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns>如果正确则返回uid</returns>
        int AdminCheckPassword(string userName, string passWord);
        /// <summary>
        /// 返回登录错误日志列表
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        int GetErrLoginCountByIP(string ip);
        /// <summary>
        /// 删除指定ip地址的登录错误日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        void DeleteErrLoginRecord(string ip);
        /// <summary>
        /// 更新管理员登录信息
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateAdminLoginInfo(AdminUserInfo userinfo);
        /// <summary>
        /// 获取当前在线用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        DbDataReader GetAdminOnlineUser(int userId, string passWord);
        /// <summary>
        /// 更新网站配置信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        void UpdateConfigInfo(int id, string val);
        /// <summary>
        /// 根据存储过程查询数据，并显示分页
        /// </summary>
        /// <param name="TableName">要分页显示的表名</param>
        /// <param name="FieldKey">用于定位记录的主键(惟一键)字段,可以是逗号分隔的多个字段</param>
        /// <param name="PageCurrent">要显示的页码</param>
        /// <param name="PageSize">每页的大小(记录数)</param>
        /// <param name="FieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <param name="ResultCount">总页数</param>
        /// <returns></returns>
        DbDataReader GetPagerData(string TableName, string FieldKey, int PageCurrent, int PageSize, string FieldShow, string FieldOrder, string Where, out int ResultCount);
        /// <summary>
        /// 更改指定表的某个字段的值
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="Value">值</param>
        /// <param name="key">主键名</param>
        /// <param name="id">主键值</param>
        /// <returns>返回已更改的标识列</returns>
        void UpdateFieldValue(string TableName, string Field, object Value, string key, int id);
        /// <summary>
        /// 更改指定表的某个字段的值
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="Value">值</param>
        /// <param name="key">主键名</param>
        /// <param name="ids">主键值</param>
        /// <returns>返回已更改的标识列</returns>
        void UpdateFieldValue(string TableName, string Field, object Value, string key, string ids);
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="TableName">要分页显示的表名</param>
        /// <param name="FieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        DbDataReader GetAllData(string TableName, string FieldShow, string FieldOrder, string Where);
        /// <summary>
        /// 根据条件查询表中所有数据
        /// </summary>
        /// <param name="TableName">要分页显示的表名</param>
        /// <param name="FieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段</param>
        /// <param name="FieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC用于指定排序顺序</param>
        /// <param name="Where">查询条件</param>
        /// <returns></returns>
        DataTable GetAllDataToDataTable(string TableName, string FieldShow, string FieldOrder, string Where);
        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        void UpdateGoodsGallery(int goods_id, string img_desc, int img_id);
        /// <summary>
        /// 更新商品相册信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <param name="img_desc"></param>
        /// <param name="img_id"></param>
        void UpdateGoodsGallery(int goods_id, string img_desc, int img_id,int order_id);
        /// <summary>
        /// 删除临时上传且未更新所属商品的图片信息
        /// </summary>
        /// <param name="user_id"></param>
        void DeleteGoodsGalleryByTemp(int user_id);
        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        DataTable GetGoodsAttr(int goods_id, int type_id);
        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        DataTable GetNewsAttr(int goods_id, int type_id);
        /// <summary>
        /// 获取商品属性信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        DataTable GetGoodsAttribute(int attr_id);
        /// <summary>
        /// 删除商品相关属性值
        /// </summary>
        /// <param name="goods_id"></param>
        void DeleteGoodsAttrValueByGoodsId(int goods_id);
        /// <summary>
        /// 删除资讯相关属性值
        /// </summary>
        /// <param name="article_id"></param>
        void DeleteGoodsAttrValueByNewsId(int article_id);
        /// <summary>
        /// 获取商品会员等级价格信息
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        DataTable GetGoodsUserRankPrice(int goods_id);
        /// <summary>
        /// 删除商品相关会员等级价格
        /// </summary>
        /// <param name="goods_id"></param>
        void DeleteGoodsUserRankPriceByGoodsId(int goods_id);
        /// <summary>
        /// 按自定义条件删除指定表中的数据
        /// </summary>
        /// <param name="tbname"></param>
        /// <param name="filter"></param>
        void DeleteData(string tbname, string filter);
        /// <summary>
        /// 获取指定所的所有字段
        /// </summary>
        /// <param name="tbname"></param>
        /// <returns></returns>
        DataTable GetTableColumns(string tbname);
        /// <summary>
        /// 获取数据库版本信息
        /// </summary>
        /// <returns></returns>
        string GetDatabaseVersion();

        /// <summary>
        /// 获取cms信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        DataTable GetCMSFullInfo(string urlrewriter);
        /// <summary>
        /// 获取cms信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        DataTable GetCMSFullInfo(int id);

        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        bool UserExists(int uid);
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool UserExists(string username);
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool UserOpenidExists(string openid);
        /// <summary>
        /// 检查用户密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <returns>如果正确则返回uid</returns>
        int CheckUserPassword(string userName, string passWord);
        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="userinfo"></param>
        void UpdateUserLoginInfo(UsersInfo userinfo);
        /// <summary>
        /// 获取当前在线用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        DbDataReader GetOnlineUser(int userId, string passWord);
        /// <summary>
        /// 获取指定广告位的广告信息
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        DataTable GetAds(int pos_id);
        /// <summary>
        /// 获取指定表单的控件列表
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        DataTable GetForm(int pos_id);
        /// <summary>
        /// 获取指定表单的不重复session_id列表
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        DataTable GetFormvalueSessionId(int position_id);
        /// <summary>
        /// 添加到收藏夹
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <param name="id_value"></param>
        int SaveFavorites(int user_id, int type, int id_value);
        /// <summary>
        /// 获取商品收藏夹
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        DataTable GetGoodsFavorites(int user_id);
        /// <summary>
        /// 删除收藏夹信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        void DeleteFavorites(int user_id,int id);
        /// <summary>
        /// 获取会员注册项信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        DataTable GetUserRegFields(int user_id);
        /// <summary>
        /// 获取商品属性
        /// </summary>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        DataTable GetGoodsSpecs(int spec_id,int goods_id);
        /// <summary>
        /// 获取当前分类的所有父类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        DbDataReader GetPrevGoodsCat(int cat_id);
        /// <summary>
        /// 获取goods信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        DataTable GetGoodsFullInfo(string urlrewriter);
        /// <summary>
        /// 获取goods信息（包括所属分类信息）
        /// </summary>
        /// <returns></returns>
        DataTable GetGoodsFullInfo(int goods_id);
        /// <summary>
        /// 插入商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        int InsertGoodsCategory(GoodsCategoryInfo catinfo);
        /// <summary>
        /// 更新商品分类
        /// </summary>
        /// <param name="catinfo"></param>
        int UpdateGoodsCategory(GoodsCategoryInfo catinfo);
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="cat_id"></param>
        int DeleteGoodsCategory(int cat_id);
        /// <summary>
        /// 移动商品位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="goods_id"></param>
        /// <returns></returns>
        int MoveGoodsPos(string act, int goods_id);
        /// <summary>
        /// 移动商品分类位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        int MoveGoodsCategoryPos(string act, int cat_id);
        /// <summary>
        /// 移动资讯位置
        /// </summary>
        /// <param name="act"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        int MoveCmsPos(string act, int article_id);
        /// <summary>
        /// 插入文章分类
        /// </summary>
        /// <param name="catinfo"></param>
        int InsertCMSCategory(CmsCatInfo catinfo);
        /// <summary>
        /// 更新文章分类
        /// </summary>
        /// <param name="catinfo"></param>
        int UpdateCMSCategory(CmsCatInfo catinfo);
        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="cat_id"></param>
        int DeleteCMSCategory(int cat_id);
        /// <summary>
        /// 获取当前分类的所有父类
        /// </summary>
        /// <param name="cat_id"></param>
        /// <returns></returns>
        //DbDataReader GetPrevCMSCat(int cat_id);

        /// <summary>
        /// 处理sql语名
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        int SqlProcess(string sql);

        /// <summary>
        /// 设置上次任务计划的执行时间
        /// </summary>
        /// <param name="key">任务的标识</param>
        /// <param name="serverName">主机名</param>
        /// <param name="lastExecuted">最后执行时间</param>
        void SetLastExecuteScheduledEventDateTime(string key, string serverName, DateTime lastExecuted);
        /// <summary>
        /// 获取上次任务计划的执行时间
        /// </summary>
        /// <param name="key">任务的标识</param>
        /// <param name="serverName">主机名</param>
        /// <returns></returns>
        DateTime GetLastExecuteScheduledEventDateTime(string key, string serverName);
    }
}
