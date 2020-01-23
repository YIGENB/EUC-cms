using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DY.OAuthSDK.Connect.Util
{
    public class WeiboParaeter
    {
        public String Name { get; set; }
        public String Value { get; set; }

        public WeiboParaeter() { }

        public WeiboParaeter(String name,String value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
