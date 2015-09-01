# Introduction #

To Use this Handler you must gzip your resources 1st and upload them.
The FileCompress Tool is here http://myextensions.googlecode.com/svn/trunk/Tools/FileCompression.exe

#### Examples Files in your folder ####
**Scripts Folder**
jquery.min.js
jquery.min.js.gz
jquery.min.js.de

**Css Folder**
Site.css
Site.css.gz
Site.css.de

#### Html Sample ####
```
<link href="../../Content/Site.css.c" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery.min.js.c" type="text/javascript"></script>
```

#### Inteligence Your css or js you could coding (MasterPage) ####
```
<asp:ContentPlaceHolder ID="intellisense" runat="server" Visible="false">
<link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.fn.Extensions.js" type="text/javascript"></script>
</asp:ContentPlaceHolder>
```

#### Inteligence Your css or js you could coding (Page) ####
```
<% if (false) { %>
<link href="../../Content/Site.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/jquery-vsdoc.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.fn.Extensions.js" type="text/javascript"></script>
<% } %>
```

#### Sample Configuration in web.config ####
```
<?xml version="1.0"?>
<configuration>
  <system.web>
    <httpHandlers>
      <add verb="*" path="*.c" type="System.Web.HttpHandlers.CompressionHandler,MyExtensions"/>
    </httpHandlers>
  </system.web>
  <system.webServer>
    <handlers>
      <add name="CompressionHandler" preCondition="integratedMode" verb="*" path="*.c" type="System.Web.HttpHandlers.CompressionHandler,MyExtensions"/>
    </handlers>
  </system.webServer>
</configuration>
```

#### ASP.NET Mvc Application ####
```
public static void RegisterRoutes(RouteCollection routes)
{
  /// your code
  routes.IgnoreRoute("{pathInfo*}/{resource}.c");
  routes.IgnoreRoute("{pathInfo*}/{subpathInfo*}/{resource}.c");
  routes.IgnoreRoute("{pathInfo*}/{subpathInfo*}/{subpathInfo1*}/{resource}.c");  
  /// your code
}

```
