using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace System.Web
{
    public static class HttpApplicationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int GetIISMajorVersion(this HttpApplication context)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp");
            return Int32.Parse(key.GetValue("MajorVersion").ToString());
        }
    }
}
