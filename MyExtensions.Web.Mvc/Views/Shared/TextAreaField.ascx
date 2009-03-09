<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TextArea>" %>
<% 
    if (!Model.ContainsKey("rows"))
        Model["rows"] = 6;
    if (!Model.ContainsKey("cols"))
        Model["cols"] = 90;
%>
<div class="field">
    <div class="field-label">
        <label for="<%=Model.Name %>">
            <%=Model.Label%>:</label>
    </div>
    <div class="field-textarea">
        <%=Html.TextArea(Model.Name, null, Model)%>
    </div>
</div>
