using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace System.Net
{
    public static class IpHelper
    {
        public static IPAddress[] GetIPAddressList()
        {
           return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }
    }
}
