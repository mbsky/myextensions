using System;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class IsCurrentActionHelper
    {
        public static string CurrentActionLink(this HtmlHelper helper, UrlHelper url)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];
            string currentAreaName = (string)helper.ViewContext.RouteData.Values["area"];

            if (currentAreaName.IsNullOrEmpty())
                return url.Action(currentActionName, currentControllerName);
            else
                return url.Action(currentActionName, currentControllerName, new { area = currentAreaName });
        }

        public static bool IsCurrentAction(this HtmlHelper helper, string actionName, string controllerName)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;
        }

        public static bool IsCurrentAction(this HtmlHelper helper, string actionName, string controllerName, string areaName)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];
            string currentAreaName = (string)helper.ViewContext.RouteData.Values["areas"];


            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)
                && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase)
                && (currentAreaName.IsNullOrEmpty() || currentAreaName.Equals(areaName, StringComparison.CurrentCultureIgnoreCase)))
                return true;

            return false;
        }
    }
}
