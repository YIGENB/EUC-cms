using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DY.Config;

namespace DY.Site
{
    public abstract class CommentsFactory
    {
        protected String short_name = String.Empty;
        protected String secret = String.Empty;
        protected String callbackUrl = String.Empty;

        //url获取
        public abstract String GetAuthorizeURL();
        protected CommentsFactory( )
        {
            
        }
        protected CommentsFactory( String _callbackUrl)
        {
            this.short_name = BaseConfig.GetBaseConfigInfo("short_name");
            this.secret =  BaseConfig.GetBaseConfigInfo("secret");
            this.callbackUrl = _callbackUrl;
        }
        protected CommentsFactory(String _short_name, String _secret, String _callbackUrl)
        {
            this.short_name = _short_name;
            this.secret = _secret;
            this.callbackUrl = _callbackUrl;
        }

        public abstract void Delete();
        public abstract void Insert();
        public abstract void Select();
        public abstract void Update();
    }
}
