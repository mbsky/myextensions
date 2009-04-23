using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.Mvc;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlSubmitLinkExtension
    {
        public static string SubmitLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string linkText) where TController : Controller
        {
            var htmlAttributes = new { @class = "submitlink" };
            return helper.ActionLink<TController>(action, linkText, htmlAttributes);
        }
    }
}
