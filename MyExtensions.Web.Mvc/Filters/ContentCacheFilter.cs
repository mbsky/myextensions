using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc
{

    public class ContentCacheFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Filter =
                new ResponseWrapper(filterContext.HttpContext.Response.Filter, 
                    filterContext.HttpContext.Request.Headers["If-None-Match"]);
        }
    }
}
