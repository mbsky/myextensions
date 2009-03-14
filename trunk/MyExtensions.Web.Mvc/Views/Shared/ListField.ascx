<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (false)
   { %>
<link href="../../Content/Site.css" rel="stylesheet" type="text/css" />

<script src="../../Scripts/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>

<% } %>
<%       
    ListField field = ViewData.Model as ListField;
    SelectList options = field.Options;
%>
<div class="field-radio">
    <% 
        if (null != options)
        {
    %>
    <%   foreach (var x in options.ToList())
         {
             switch (field.FieldType)
             {
                 case FieldType.RadioList:

                     object evalValue = ViewData.Eval(field.Name);

                     string checkAtt = string.Empty;

                     if (null != evalValue) //x.Text
                     {
                         string v = evalValue.ToString().Trim();

                         if (v == x.Value || v == x.Text )
                             checkAtt = "checked";
                     }
    %>
   <%-- 调试信息 evalValue.ToString()=<%=evalValue.ToString() %>
    x.Value=<%=x.Value %>--%>
    <input name="<%=field.Name %>" type="radio" value="<%=x.Value %>" <%=checkAtt %> />
    <%=x.Text%>
    <% break;
                 case FieldType.CheckBoxList: %>
    <%=Html.CheckBox(field.Name, field)%><%=x.Text%>
    <% break;
                 case FieldType.Select: %>
    <% break; %>
    <% }
         }%>
    <%--  solution 2--%>
    <% 
                    
        } %>
</div>
