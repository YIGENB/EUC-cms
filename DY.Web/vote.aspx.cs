using System;
using System.Collections;
using System.Data;
using System.Web;

using CShop.Site;
using CShop.Common;
using CShop.Entity;

namespace CShop.Web
{
    public partial class vote : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tlp = "vote";

            int vote_id = CShopRequest.getRequestInt("vote_id");
            if (vote_id < 0)
            {
                return;
            }

            ArrayList list = SiteBLL.GetVoteOptionAllList("", "vote_id=" + vote_id);
            VoteInfo vote = SiteBLL.GetVoteInfo(vote_id);
            int votelog = SiteBLL.GetVoteLogAllList("", "vote_id=" + vote_id).Count;

            int count = 0;
            foreach (VoteOptionInfo item in list)
            {
                count += item.option_count.Value;
            }

            float[] num = new float[list.Count];
            string[] color = { "#6c81b6", "#5dbc5b", "#ee325f", "#e7ab6d", "#a5cbd6", "#eba0d9", "#908f8f" };
            for (int i = 0; i < list.Count; i++)
            {
                int n = ((VoteOptionInfo)list[i]).option_count.Value;
                num[i] = (float)n / count;
            }

            IDictionary context = new Hashtable();
            context.Add("list", list);
            context.Add("count", count);
            context.Add("vote", vote);
            context.Add("votelog", votelog);
            context.Add("num", num);
            context.Add("color", color);

            base.DisplayTemplate(context, tlp);
        }
    }
}
