/**
 * 功能描述：Grab管理类
 * 创建时间：2011-9-27 17:18:41
 * 最后修改时间：2011-9-27 17:18:41
 * 作者：gudufy
 * 文件标识：df84780e-3008-49fb-ad1a-fc086cb5a73d
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
using DY.Site;
using DY.Entity;

namespace DY.Web.admin
{
	public partial class Grab : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			#region 列表
			if (this.act == "list")
			{
				//检测权限
				this.IsChecked("grab_list");

				//显示列表数据
				this.GetList();
			}
			#endregion

			#region 添加
			else if (this.act == "add")
			{
				//检测权限
				this.IsChecked("grab_add");

				if (ispost)
				{
					base.id = SiteBLL.InsertGrabInfo(this.SetEntity());

					//日志记录
					base.AddLog("添加grab");

					Hashtable links = new Hashtable();
					links.Add("继续添加", "?act=add");

					//显示提示信息
					this.DisplayMessage("grab添加成功", 2, "?act=list", links);
				}

				IDictionary context = new Hashtable();

				base.DisplayTemplate(context, "grab/grab_info");
			}
			#endregion

			#region 修改
			else if (this.act == "edit")
			{
				//检测权限
				this.IsChecked("grab_edit");

				if (ispost)
				{
					SiteBLL.UpdateGrab2Info(this.SetEntity());

					//日志记录
					base.AddLog("修改grab");

					base.DisplayMessage("grab修改成功", 2, "?act=list");
				}

				IDictionary context = new Hashtable();
				context.Add("entity", SiteBLL.GetGrab2Info(base.id));
				context.Add("update", DYRequest.getRequest("update"));

				base.DisplayTemplate(context, "grab/grab_info");
			}
			#endregion

			#region 更新单个字段值
			else if (this.act == "edit_field_value")
			{
				//检测权限
				this.IsChecked("grab_edit", true);

				if (ispost)
				{
					base.id = DYRequest.getFormInt("id");
					object val = DYRequest.getForm("val");
					string fieldName = DYRequest.getForm("fieldName");

					//执行修改
					SiteBLL.UpdateGrab2FieldValue(fieldName, val, base.id);

					//日志记录
					base.AddLog("修改grab");

					//输出json数据
					base.DisplayMemoryTemplate(base.MakeJson(val.ToString(), 0, null));
				}
			}
			#endregion

			#region 更新多条记录的单个字段值
			else if (this.act == "edit_field_values")
			{
				//检测权限
				this.IsChecked("grab_edit", true);

				if (ispost)
				{
					string ids = DYRequest.getForm("ids");
					object val = DYRequest.getForm("val");
					string fieldName = DYRequest.getForm("fieldName");

					if (!string.IsNullOrEmpty(ids))
					{
						//执行修改
						SiteBLL.UpdateGrab2FieldValue(fieldName, val, ids.Remove(ids.Length - 1, 1));
					}

					//输出json数据
					base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
				}
			}
			#endregion

			#region 批量彻底删除
			else if (this.act == "CompletelyDelete")
			{
				//检测权限
				this.IsChecked("grab_del", true);

				if (ispost)
				{
					string ids = DYRequest.getForm("ids");

					if (!string.IsNullOrEmpty(ids))
					{
						//执行删除
						SiteBLL.DeleteGrab2Info("id in (" + ids.Remove(ids.Length - 1, 1) + ")");

						//日志记录
						base.AddLog("删除grab");
					}

					//输出json数据
					base.DisplayMemoryTemplate(base.MakeJson("", 0, ""));
				}
			}
			#endregion

			#region 删除记录
			else if (this.act == "remove")
			{
				//检测权限
				this.IsChecked("grab_del", true);

				//执行删除
				SiteBLL.DeleteGrab2Info(base.id);

				//日志记录
				base.AddLog("删除grab");

				//显示列表数据
				this.GetList();
			}
			#endregion

			#region 查看
			else if (this.act == "reply")
			{
				//检测权限
				this.IsChecked("grab_reply");								

				IDictionary context = new Hashtable();
				context.Add("entity", SiteBLL.GetGrab2Info(base.id));				

				base.DisplayTemplate(context, "grab/grab_reply");
			}
			#endregion
		}
		/// <summary>
		/// 获取列表数据
		/// </summary>
		protected void GetList()
		{
			string filter = "";

			this.GetList("grab/grab_list", filter);
		}
		/// <summary>
		/// 获取列表数据
		/// </summary>
		protected void GetList(string tpl, string filter)
		{
			IDictionary context = new Hashtable();
			context.Add("list", SiteBLL.GetGrab2List(base.pageindex, base.pagesize, SiteUtils.GetSortOrder("id desc"), SiteUtils.GetFilter(context) + filter, out base.ResultCount));
			context.Add("pager", Utils.GetAdminPageNumbers(base.ResultCount, base.pageindex, base.pagesize));
			//to json
			context.Add("sort_by", DYRequest.getRequest("sort_by"));
			context.Add("sort_order", DYRequest.getRequest("sort_order"));
			context.Add("page", base.pageindex);
			//context.Add("isajax", base.isajax);//
			base.DisplayTemplate(context, tpl, base.isajax);
		}
		/// <summary>
		/// 给实体赋值
		/// </summary>
		protected Grab2Info SetEntity()
		{
			Grab2Info entity = new Grab2Info();

			entity.companyname = DYRequest.getForm("companyname");
			entity.companyurl = DYRequest.getForm("companyurl");
			entity.companyaddress = DYRequest.getForm("companyaddress");
			entity.companyscale = DYRequest.getFormInt("companyscale");
			entity.predictcount = DYRequest.getFormInt("predictcount");
			entity.monthcount = DYRequest.getFormInt("monthcount");
			entity.yearcount = DYRequest.getFormInt("yearcount");
			entity.askfor = DYRequest.getForm("askfor");
			entity.photo = DYRequest.getForm("photo");
            entity.monthlyConsumption = DYRequest.getForm("monthlyConsumption");
			entity.id = base.id;

			return entity;
		}
	}
}


