using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{
    public abstract class BaseActionCaller : System.Web.Mvc.Html.IActionCaller
    {
        public virtual string ActionName { get; set; }
        public virtual bool Ajax { get; set; }
        public virtual bool AntiForgeryToken { get; set; }
        public virtual string ControllerName { get; set; }
        public virtual System.Web.Mvc.FormMethod Method { get; set; }
    }
}
