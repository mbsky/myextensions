using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Microsoft.Web.Mvc.Internal;

namespace System.Web.Mvc
{
    public static class HtmlActionLinkExtensionsAreaViewEngine
    {
        public static string ActionLink<T>(this HtmlHelper html, Expression<Action<T>> action, string linkText, string area)
        where T : Controller
        {

            return html.ActionLink<T>(action, linkText, null, area);
        }

        public static string ActionLink<TController>(this HtmlHelper helper, Expression<Action<TController>> action, string linkText, object htmlAttributes, string area) where TController : Controller
        {

            RouteValueDictionary routingValues = ExpressionHelper.GetRouteValuesFromExpression(action);

            routingValues.Add("area", area);

            return helper.RouteLink(linkText, routingValues, new RouteValueDictionary(htmlAttributes));
        }
    }
}
