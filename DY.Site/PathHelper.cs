using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace DY.Site
{
    public static class PathHelper
    {
        public static string Root = HttpContext.Current.Request.MapPath("/include/upload/");
        public static string NewRoot = "";

        public static string ToNewPath(this string str)
        {
            var dir="";
            if(!string.IsNullOrEmpty(str))
             dir= str.Substring(Root.Length, str.Length- Root.Length);
            return string.Concat(NewRoot, dir).ToLower();
        }

        public static string ToOriginalPath(this string str)
        {
           var dir = str.Substring(NewRoot.Length, str.Length - NewRoot.Length);

            var ext= Path.GetExtension(dir);

            if (string.IsNullOrEmpty(ext)&& dir.Length>=1)
            {
                if (!dir.EndsWith("/"))
                {
                    dir = string.Concat(dir, "/");
                }
            }

            return Path.Combine(Root, dir);
        }

        public static bool IsRoot(this string str)
        {
            if (str.Length == Root.Length && str.ToLower() == Root.ToLower())
                return true;
            else
                return false;
        }
        public static bool IsNewRoot(this string str)
        {
            if (str.Length == NewRoot.Length && str.ToLower() == NewRoot.ToLower())
                return true;
            else
                return false;
        }

    }
}
