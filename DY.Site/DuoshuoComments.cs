using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.HttpUtility;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using System.Collections;
using DY.Entity;
using DY.Config;

namespace DY.Site
{
    public class DuoshuoComments : CommentsFactory
    {
        public DuoshuoComments()  { }
        public DuoshuoComments(String url) : base(url) { }
        public override string GetAuthorizeURL()
        {
            Dictionary<String, String> config = new Dictionary<string, string>(){
               { "short_name", this.short_name},
               {"secret",this.secret},
               {"order","desc"}
            };
            UriBuilder builder = new UriBuilder(this.callbackUrl);
            builder.Query = Caches.BuildQueryString(config);
            return builder.ToString();
        }
        override public void Delete(){}
        override public void Insert()
        {
             int ResultCount = 0;
            string returnText = Caches.HttpGet(GetAuthorizeURL(), null);
            JObject jsonObj = JObject.Parse(returnText);
            JArray jar = JArray.Parse(jsonObj["response"].ToString());
            ArrayList tfc_list = SiteBLL.GetTpcommentsList(1, 1, SiteUtils.GetSortOrder("id desc"), "", out ResultCount);
            string tfc_info = "";
            if (tfc_list.Count > 0)
                tfc_info = tfc_list[0].ToString();
            foreach (JObject row in jar)
            {
                try
                {
                    string post_id = "";
                    if (row["action"].ToString() == "delete-forever")
                    {
                        SiteBLL.DeleteTpcommentsInfo("post_id ='" + row["meta"].ToString() + "'");
                    }
                    else if (row["action"].ToString() == "create")
                    {
                        TpcommentsInfo entity = new TpcommentsInfo();
                        entity.log_id = row["log_id"].ToString();
                        entity.user_id = row["user_id"].ToString();
                        entity.post_id = row["meta"]["post_id"].ToString();
                        entity.thread_id = row["meta"]["thread_id"].ToString();
                        entity.thread_key = row["meta"]["thread_key"].ToString();
                        entity.author_id = row["meta"]["author_id"].ToString();
                        entity.author_name = row["meta"]["author_name"].ToString();
                        entity.ip = row["meta"]["ip"].ToString();
                        entity.created_at = Convert.ToDateTime(row["meta"]["created_at"].ToString());
                        entity.message = row["meta"]["message"].ToString();
                        entity.parent_id = row["meta"]["parent_id"].ToString();
                        entity.action = row["action"].ToString();
                        if (tfc_info != "")
                        {
                            post_id = row["meta"]["post_id"].ToString();
                            TpcommentsInfo tfinto = null;
                            tfinto = SiteBLL.GetTpcommentsInfo("post_id = '" + post_id + "'");
                            if (tfinto != null)
                                break;
                            else
                                SiteBLL.InsertTpcommentsInfo(entity);

                        }
                        else
                        {
                            SiteBLL.InsertTpcommentsInfo(entity);
                        }
                    }

                }
                catch
                {

                }
            }
        }
        override public void Select(){}
        override public void Update(){}
    }
}
