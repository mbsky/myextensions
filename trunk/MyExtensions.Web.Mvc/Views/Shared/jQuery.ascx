<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%--<script src="/Scripts/jquery-1.3.2.min.js.aspx" type="text/javascript"></script>--%>

<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js" type="text/javascript"></script>
<% if (System.Web.Configuration.WebConfigurationManager.AppSettings["jquery.fn.Extensions.compressed"] == "true" && Request.Url.AbsoluteUri.ToLower().Contains("localhost") == false)
   { %>
<script src="/Scripts/jquery.fn.Extensions.js.aspx" type="text/javascript"></script>
<% }
   else
   { %>
<script src="/Scripts/jquery.fn.Extensions.js" type="text/javascript"></script>
<% } %>

<%--<link href="/Scripts/theme/ui.all.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-ui-personalized-1.6rc6.min.js.aspx" type="text/javascript"></script>--%>