<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FormModel>" %>
<% if (false)
   { %>
<link href="../../Content/Themes/MountainBlue/style.css" rel="stylesheet" type="text/css" />

<script src="../../Scripts/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>

<% } %>
<% if (null != Model)
   { %>
<%
    var Notice = Model.Notice;

    FamIcon icon = Model.Icon == FamIcon.None ? FamIcon.ApplicationEdit : Model.Icon;
    int fieldCount = Model.Count();
    string formContainerId = Model.Name + "_Container";
%>
<%--form Begin--%>
<div id="<%=formContainerId %>" class="form">
    <h1>
        <%=Html.Icon(icon)%>
        <%=Model.Title%>
    </h1>
    <%--TODO: Check Html.GetMessage(null, Model)--%>
    <% Html.RenderPartial("FormMessage", Html.GetMessage(null, Model)); %>
    <%--fields Begin--%>
    <div class="fields">
        <% if (fieldCount != 0)
           {

               RouteValueDictionary values = new RouteValueDictionary();

               IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();

               htmlAttributes.Add("id", Model.Name);

               if (Model.Ajax)
               {
                   htmlAttributes.Add("class", "ajax");
               }

               htmlAttributes.Add("submitdisabledcontrols", "true");

               using (Html.BeginForm(Model.ActionName, Model.ControllerName, values, Model.Method, htmlAttributes))
               { %>
        <input type="hidden" name="formContainerId" value="<%=formContainerId %>" />
        <% if (Model.AntiForgeryToken)
           { %>
        <%=Html.AntiForgeryToken()%>
        <% } %>
        <% if (Model.Count() != 0)
           {
               int i = 1;

               foreach (FieldBase field in Model)
               {

                   if (!Model.Prefix.IsNullOrEmpty())
                   {
                       field.Name = Model.Prefix + "." + field.Name;
                   }

                   if (field is IRegField)
                   {
                       string reg = (field as IRegField).GetReg();
                       if (!reg.IsNullOrEmpty())
                       {
                           field["reg"] = reg;
                           field["check"] = "";
                       }
                   }
               }

               foreach (Hidden field in Model.Where(x => x.FieldType == FieldType.Hidden))
               {
                   object hiddenValue = ViewData[field.Name];

                   if (null == hiddenValue)
                   {
                       hiddenValue = field.Value;
                   }
        %>
        <%--        <%=ViewData[field.Name]%>--%>
        <input name='<%=field.Name %>' type="hidden" value='<%=hiddenValue %>' />
        <%-- <%=Html.Hidden(field.Name, hiddenValue)%>--%>
        <% }

               bool NotOneColumn = Model.FieldsColumnCount == FieldsColumn.Two;
               bool OneColumn = !NotOneColumn;
        %>
        <%--        <h3>帐户信息</h3>
        <hr />--%>
        <%
            var normalFields = Model.Where(x => x.FieldType != FieldType.Hidden && x.FieldType != FieldType.TextArea);
            int normalFieldsCount = normalFields.Count();
            int renderCount = 0;
            foreach (FieldBase field in normalFields)
            {
                renderCount++;
                bool needBeginAField = OneColumn || (NotOneColumn && (i % 2 != 0));
                // 当输出的filed input 是双数 或者是最后一个filed时需要输出结束标记
                bool endAField = OneColumn || (renderCount == normalFieldsCount) || (NotOneColumn && (i % 2 == 0));
        %>
        <% if (needBeginAField)
           { %>
        <div class="field">
            <% } %>
            <% if (NotOneColumn)
               { %>
            <div style="margin-right: 20px; float: left;">
                <% } %>
                <div class="field-label">
                    <label for="<%=field.Name %>">
                        <%=field.Label%>:</label>
                    <% if (field.FieldType == FieldType.CheckBox)
                       { %>
                    <%=Html.CheckBox(field.Name)%>
                    <% } %>
                </div>
                <%--ListField Begin --%>
                <% if (field is ListField)
                   {
                       Html.RenderPartial("ListField", field, ViewData);
                   }
                   //ListField End
                   else
                   {
                       if (field is TextFieldBase)
                       {
                           if (!field.ContainsKey("size"))
                           {
                               field.Add("size", 40);
                           }

                           if (field.Required || ViewData.ModelState.IsValidField(field.Name) == false)
                           {
                               field.Add("class", "input-validation-error");
                           }
                       }

                       switch (field.FieldType)
                       { %>
                <%--TextBox--%>
                <% 
                    case FieldType.Text:             
                %>
                <div class="field-input">
                    <%=Html.TextBox(field.Name, null, field)%>
                </div>
                <% break; %>
                <%--Password--%>
                <% case FieldType.Password: %>
                <div class="field-input">
                    <%=Html.Password(field.Name, null, field)%>
                </div>
                <% break; %>
                <%--Number--%>
                <% case FieldType.Number: %>
                <div class="field-input">
                    <%=Html.TextBox(field.Name, null, field)%>
                </div>
                <% break; %>
                <% }
                   } %>
                <% if (NotOneColumn)
                   { %>
            </div>
            <% } %>
            <% if (endAField)
               { %>
        </div>
        <div class="seperator">
        </div>
        <% } %>
        <% 
            i++;
            } %>
        <% } %>
        <% foreach (TextArea ta in Model.Where(x => x.FieldType == FieldType.TextArea))
           {
               Html.RenderPartial("TextAreaField", ta, ViewData);
           } %>
        <div class="footer">
            <% if (fieldCount != 0)
               { %>
            <input type="submit" value="提交" />
            <input type="reset" value="重置" />
            <input type="button" value="清除" onclick='$("div.fields",$("#<%=Model.Name %>")).clearForm();' />
            <% if (Model.Ajax && Request.UrlReferrer != null)
               {
                   string oldUrl = Request.UrlReferrer.AbsolutePath;
                   string toOldUrlScript = string.Format("window.location='{0}';", oldUrl);
            %>
            <input type="button" value="返回前一页" onclick="<%=toOldUrlScript %>" />
            <% } %>
            <% } %>
        </div>
        <% }
           } %>
    </div>
    <%--fields End--%>
</div>
<%--form End--%>
<div class="clear">
</div>
<% if (Model.NotRenderAsPanel)
   { %>

<script type="text/javascript">

    $(document).ready(function() {
        var container = $("#<%=formContainerId %>");
        $("h1:first", container).hide();
        $("div.message", container).css("border", "none");
        $("div.fields", container).css("border", "none");
    });
</script>

<% } %>
<% }
   else
   { %>
FormModel is null.
<% } %>
