using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace MyExtensions.Web.Security
{
    public static class Md5Utility
    {
        public static string md5(string str, Md5Length code)
        {
            if (code == Md5Length.Sixteen)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 0x10);
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }

    }
}
