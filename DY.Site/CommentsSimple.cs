using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DY.Site
{
    public class CommentsSimple
    {
        public CommentsFactory Insert(string CommentsType)
        {
            switch (CommentsType.Trim().ToLower())
            {
                case "duoshuo":
                    return new DuoshuoComments("http://api.duoshuo.com/log/list.json");
                default :
                    return null;
            }
        }
    }
}
