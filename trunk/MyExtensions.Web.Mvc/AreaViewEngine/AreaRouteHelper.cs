using System.Web.Routing;
using System.Web.Mvc;
using System;

namespace System.Web.Routing
{
    public static class AreaRouteHelper {


        //public static void MapAreas(this RouteCollection routes, string url, string rootNamespace, string[] areas)
        //{
        //    Array.ForEach(areas, area =>
        //    {
        //        Route route = new Route("{area}/" + url, new MvcRouteHandler());
        //        route.Constraints = new RouteValueDictionary(new { area });
        //        string areaNamespace = rootNamespace + ".Areas." + area + ".Controllers";
        //        route.DataTokens = new RouteValueDictionary(new { namespaces = new string[] { areaNamespace } });
        //        route.Defaults = new RouteValueDictionary(new { action = "Index", controller = "Home", id = "" });
        //        routes.Add(route);
        //    });
        //}

        public static void MapAreas(this RouteCollection routes, string url, string rootNamespace, string[] areas)
        {
            Array.ForEach(areas, area =>
            {
                Route route = new Route("{area}/" + url, new MvcRouteHandler());
                route.Constraints = new RouteValueDictionary(new { area });
                string areaNamespace = rootNamespace + ".Controllers";
                route.DataTokens = new RouteValueDictionary(new { namespaces = new string[] { areaNamespace } });
                route.Defaults = new RouteValueDictionary(new { action = "Index", controller = "Home", id = "" });
                routes.Add(route);

                ControllerBuilder.Current.DefaultNamespaces.Add(areaNamespace);
            });
        }

        public static void MapRootArea(this RouteCollection routes, string url, string rootNamespace, object defaults) {
            Route route = new Route(url, new MvcRouteHandler());
            route.DataTokens = new RouteValueDictionary(new { namespaces = new string[] { rootNamespace + ".Controllers" } });
            route.Defaults = new RouteValueDictionary(new { area="root", action = "Index", controller = "Home", id = "" });
            routes.Add(route);
        }

        /*
         http://blog.codeville.net/2008/11/05/app-areas-in-aspnet-mvc-take-2/
         
         // Routing config for the blogs area
        routes.CreateArea("blogs", "AreasDemo.Areas.Blogs.Controllers",
            routes.MapRoute(null, "SpecialUrlForPosts", new { controller = "Home", action = "Posts" }),
            routes.MapRoute(null, "blg/{controller}/{action}/{id}", new { action = "Index", controller = "Home", id = "" })
        );

        // Routing config for the forums area
        routes.CreateArea("forums", "AreasDemo.Areas.Forums.Controllers",
            routes.MapRoute(null, "myforums/SecretAdminZone/{action}", new { controller = "Admin", action = "Index" }),
         
            // Equally possible to construct routes using "new Route()" syntax too
            new Route("myforums/{controller}/{action}", new MvcRouteHandler()) {
                Defaults = new RouteValueDictionary(new { controller = "Home", action = "Index" })
            }
        );
         
        // Routing config for the root area
        routes.CreateArea("root", "AreasDemo.Controllers",
            routes.MapRoute(null, "{controller}/{action}", new { controller = "Home", action = "Index" })
        );
         */
        public static void CreateArea(this RouteCollection routes, string areaName, string controllersNamespace, params Route[] routeEntries)
        {
            foreach (var route in routeEntries)
            {
                if (route.Constraints == null) route.Constraints = new RouteValueDictionary();
                if (route.Defaults == null) route.Defaults = new RouteValueDictionary();
                if (route.DataTokens == null) route.DataTokens = new RouteValueDictionary();

                route.Constraints.Add("area", areaName);
                route.Defaults.Add("area", areaName);
                route.DataTokens.Add("namespaces", new string[] { controllersNamespace });

                if (!routes.Contains(route)) // To support "new Route()" in addition to "routes.MapRoute()"
                    routes.Add(route);
            }
        }
    }
}
