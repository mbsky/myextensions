using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public class ConfirmOptionsModel
    {
        public string Title { get; set; }

        public string Messages { get; set; }

        public string[] Options { get; set; }

        public string Callback { get; set; }
    }
}
