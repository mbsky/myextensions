# Introduction #

MapAreas Sample.

# Details #

```

public static void RegisterRoutes(RouteCollection routes){

routes.MapAreas("{controller}/{action}/{id}",
                "YourArea",
                new string[] { "YourArea" }
            );

routes.MapRootArea("{controller}/{action}/{id}",
                "YourProject.Web",
                new { controller = "Home", action = "Index", id = "" }
            );
}

protected void Application_Start()
{
   ViewEngines.Engines.Clear();
   ViewEngines.Engines.Add(new AreaViewEngine());

   RegisterRoutes(RouteTable.Routes);
}

```