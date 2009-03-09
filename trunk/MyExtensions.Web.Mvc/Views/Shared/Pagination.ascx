<%@ Control Language="C#" Inherits="ViewUserControl<IPagedList>" %>
<%
    string currentControllerName = (string)Html.ViewContext.RouteData.Values["controller"];
    //string currentActionName = (string)Html.ViewContext.RouteData.Values["action"];
%>
<% if (false)
   {
%>
<link href="../../Content/Themes/MountainBlue/style.css" rel="stylesheet" type="text/css" />
<% } %>
<div class="pagination">
    <div class="results">
        <span>Showing results
            <%=Model.StartIndex%>-<%=Model.EndIndex%>
            of
            <%=Model.TotalCount%></span>
    </div>
    <div class="pager">
        <% if (Model.HasPreviousPage)
           { %>
        <span>
            <%--<%=Html.ActionLink<AdTypeController>(x => x.Index(Model.PageNumber - 1), "prev")%>--%>
            <% 
                int prev = Model.PageNumber - 1;
                string href = "/" + currentControllerName + "/Index/" + prev.ToString();
            %>
            <a href="<%=href %>">prev</a> </span>
        <% }
           else
           { %>
        <span class="disabled">&laquo; prev</span>
        <% } %>
        <% if (Model.PageCount > 1)
           { %>
        <% for (int i = 1; i <= Model.PageCount; i++)
           { %>
        <% if (i == Model.PageNumber)
           { %>
        <span class="current">
            <%=i %></span>
        <% }
           else
           { %>
        <%--<%=Html.ActionLink<AdTypeController>(x => x.Index(i), i.ToString())%>--%>
        <% 
            string href = "/" + currentControllerName + "/Index/" + i.ToString();
        %>
        <a href="<%=href %>">
            <%=i.ToString()%></a>
        <% } %>
        <% } %>
        <% } %>
        <% if (Model.HasNextPage)
           { %>
        <%--<%=Html.ActionLink<AdTypeController>(x => x.Index(Model.PageNumber + 1), "next «")%>--%>
        <% 
            string href = "/" + currentControllerName + "/Index/" + (Model.PageNumber + 1).ToString();
        %>
        <a href="<%=href %>">next &laquo;</a>
        <% }
           else
           { %>
        <span class="disabled">next &laquo;</span>
        <% } %>
    </div>
</div>
<%--<% if (Model.Count() != 0)
   { %>--%>

<script type="text/javascript">

    $(document).ready(function() {

        $("a[class=lnkDel]", $("#ListBody")).each(function() {
            var lnkDelete = $(this);
            lnkDelete.click(function() {
                return confirm("您是否确定要删除这条记录");
            });
        });
    });
        
</script>
<%--测试参数
TotalCount
<%=Model.TotalCount %>
PageSize
<%=Model.PageSize %>
PageCount
<%=Model.PageCount %>
PageNumber
<%=Model.PageNumber %>
Count
<%=Model.Count %>--%>
