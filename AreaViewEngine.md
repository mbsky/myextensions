# Introduction #

Areas In Asp.Net MVC.


# Details #

Related Links:

  * **prototype of this** http://haacked.com/code/AreasDemo.zip

  * **areas-in-aspnetmvc** http://haacked.com/archive/2008/11/04/areas-in-aspnetmvc.aspx

Usage:

  * Create Areas Directory in your MVC project,and your area applications need to be place here and create the directory with the name of area you registered in the your global HttpApplication class (Global.asax.cs).

  * **More Flex Project Solution** You can create a sub mvc project under **areas** folder of the root project and output the build dll to root bin folder.

  * With **AreaRouteHelper** We can coding

> MapAreas,MapRootArea.

> To Register the areas and a root area

```

 routes.MapAreas("{controller}/{action}/{id}",
                "Blog",
                new string[] { "blog" }
            );

            routes.MapAreas("{controller}/{action}/{id}",
                "MySpace",
                new string[] { "myspace" }
                // appSettings.GetArray("MapAreas")
            );

            routes.MapRootArea("{controller}/{action}/{id}",
                "MyPortal.Web",
                new { controller = "Home", action = "Index", id = "" }
            );

            routes.MapRoute(
                "default",
                "{controller}/{action}/{id}",
                new { action = "Index", id = (string)null }
            );
```

### Areas Folder ###

<img src='http://myextensions.googlecode.com/svn/trunk/Samples/images/areaviewengine.jpg' alt='' />

### HtmlActionLinkExtensions for AreaViewEngine ###

Link to root area in area application views**```
<%= Html.ActionLink<HomeController>("c => c.Index(), "Index Page", "root")%>
```** **Link to an area application action view**
```
<%= Html.ActionLink<MySpaceController>(c => c.Index(), "MySpace Home", "myspace")%>
```