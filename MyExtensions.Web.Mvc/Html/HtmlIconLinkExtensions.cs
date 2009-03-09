using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Microsoft.Web.Mvc;
using Microsoft.Web.Mvc.Internal;

namespace System.Web.Mvc.Html
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlIconLinkExtensions
    {
        #region ActionLink With FamFamFamIcon

        private static string GetLink(string iconHtml, string linkHtml)
        {

            if (iconHtml.IsNullOrEmpty())
                return linkHtml;

            int i = linkHtml.IndexOf('>') + 1;

            linkHtml = linkHtml.Insert(i, iconHtml);

            return linkHtml;
        }

        public static string IconLink(this HtmlHelper html, FamIcon icon, string linkText, string actionName)
        {
            return html.IconLink(icon, linkText, actionName, (object)null);
        }

        public static string IconLink(this HtmlHelper html, FamIcon icon, string linkText, string actionName, object values)
        {
            string iconHtml = html.Icon(icon);

            string linkHtml = html.ActionLink(linkText, actionName, values);

            return GetLink(iconHtml, linkHtml);
        }

        public static string IconLink(this HtmlHelper html, FamIcon icon, string linkText, string actionName, string controllerName)
        {
            return GetLink(html.Icon(icon), html.ActionLink(linkText, actionName, controllerName));
        }

        public static string IconLink<T>(this HtmlHelper html, FamIcon icon, Expression<Action<T>> action, string linkText)
        where T : Controller
        {
            return GetLink(html.Icon(icon), html.ActionLink<T>(action, linkText));
        }

        public static string IconLink<T>(this HtmlHelper html, FamIcon icon, Expression<Action<T>> action, string linkText, object values)
where T : Controller
        {
            return GetLink(html.Icon(icon), html.ActionLink<T>(action, linkText, values));
        }

        #endregion
    }
}
