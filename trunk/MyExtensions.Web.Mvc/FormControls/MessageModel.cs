using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Web.Mvc.Html
{
    public class MessageModel
    {
        public virtual string[] Notice { get; set; }

        public virtual string[] Success { get; set; }

        public virtual string[] Errors { get; set; }
    }
}
