using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DY.Site
{
    public class Explorer
    {
        public static string GetPrevDirectory(string dir, ref bool isRoot)
        {
            if (string.IsNullOrEmpty(dir)) throw new ArgumentNullException(dir);

            if (dir.IsRoot())
            {
                isRoot = true;
                return dir;
            }
            else
            {
                isRoot = false;
                dir = dir.Substring(0, dir.LastIndexOf('/'));
                return dir.Substring(0, dir.LastIndexOf('/') + 1);
            }
        }

        

        public static string GetCurrentPath(HttpContext context)
        {
            string dir, request =context.Request.Form["dir"];
            if (string.IsNullOrEmpty(request) || request == "/" || request == "//")
                dir = PathHelper.Root;
            else
                dir = request.ToOriginalPath();
            return dir;
        }
    }
}
