<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ActionButton>" %>
<% RouteValueDictionary values = new RouteValueDictionary();

   IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();

   if (Model.Ajax)
   {
       htmlAttributes.Add("class", "ajax");
   }

   htmlAttributes.Add("submitdisabledcontrols", "true");

   using (Html.BeginForm(Model.ActionName, Model.ControllerName, values, Model.Method, htmlAttributes))
   { %>
<% if (Model.AntiForgeryToken)
   { %>
<%=Html.AntiForgeryToken()%>
<% } %>
<%=Html.SubmitButton("btnAction",Model.Value) %>
<% } %>