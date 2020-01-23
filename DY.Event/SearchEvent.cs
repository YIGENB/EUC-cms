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
    /// 有关搜索Search表的计划任务
    /// </summary>
    public class 更新搜索查看次数 : IEvent
    {
        #region IEvent 成员

        void IEvent.Execute(object state)
        {
            try
            {
                this.UpdateClickCount();
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog(e.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 遍历搜索库并同步查看次数
        /// </summary>
        private void UpdateClickCount()
        {
            foreach (SearchInfo entity in SiteBLL.GetSearchAllList("", ""))
            {
                if(entity.type==1)
                {
                    GoodsInfo  goods= SiteBLL.GetGoodsInfo(entity.type_id.Value);
                    SiteBLL.UpdateSearchFieldValue("click_count", goods.click_count, entity.search_id.Value);
                }
                else if (entity.type == 2)
                {
                    CmsInfo cms = SiteBLL.GetCmsInfo(entity.type_id.Value);
                    SiteBLL.UpdateSearchFieldValue("click_count", cms.click_count, entity.search_id.Value);
                }
            }
        }
    }
}
