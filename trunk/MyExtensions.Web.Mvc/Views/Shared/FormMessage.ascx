<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (false)
   { %>
<link href="../../Content/Themes/MountainBlue/style.css" rel="stylesheet" type="text/css" />

<script src="../../Scripts/jquery-1.3.2-vsdoc.js" type="text/javascript"></script>

<% } %>
<% 
    var Model = this.ViewData.Model as MessageModel;
    var Notice = Model.Notice;
    var Success = Model.Success;
    var Errors = Model.Errors;

    bool hasNotice = Notice != null && Notice.Count() != 0;
    bool hasErrors = Errors != null && Errors.Count() != 0;
    bool hasSuccess = Success != null && Success.Count() != 0;
    bool needToRender = hasNotice || hasErrors || hasSuccess;
%>
<%        string messageStyle = needToRender ? "inherit" : "none";
%>
<div class="message">
    <% 
        string noticeStyle = hasNotice ? "inherit" : "none";
    %>
    <div class="notice" style="display: <%=noticeStyle%>">
        <ul>
            <% 
                if (null != Notice)
                {
                    int n = 1;
                    var notices = Notice.Where(x => x.IsNullOrEmpty() == false).ToArray();
                    foreach (string nt in notices)
                    {
                        if (nt.IsNullOrEmpty())
                            continue;
            %>
            <li><span>
                <%=n%>.</span>
                <%=nt%></li>
            <% 
                n++;
                    } %>
            <% if (notices.Count() != 0)
               { %>
            <li><a class="luosuo" style="cursor: pointer; color: Red">
                <%=Html.Icon(FamIcon.Tag)%>别啰嗦了
                <%=Html.Icon(FamIcon.PlayBlue)%>
            </a></li>
            <% }
                } %>
        </ul>
    </div>
    <a class="reluosuo" style="display: none; cursor: pointer; float: right; color: Blue;">
        <%=Html.Icon(FamIcon.Tag) %>再啰嗦一次
        <%=Html.Icon(FamIcon.PlayBlue) %>
    </a>
    <% 
        string errorStyle = hasErrors ? "inherit" : "none";
    %>
    <div class="error" style="display: <%=errorStyle%>; margin-top: 10px;">
        <ul>
            <% if (hasErrors)
               {
                   int i = 1;
                   foreach (string err in Errors)
                   { %>
            <li><span>
                <%=i%>.</span>
                <%=err%></li>
            <% 
                i++;
                   } %>
            <% } %>
        </ul>
    </div>
    <% 
        string successStyle = hasSuccess ? "inherit" : "none";
    %>
    <div class="success" style="display: <%=successStyle%>; margin-top: 10px;">
        <ul>
            <% 
                if (hasSuccess)
                {
                    int s = 1;
                    foreach (string sc in Success)
                    { %>
            <li><span>
                <%=s%>.</span>
                <%=sc%></li>
            <%
                s++;
                    }
                } %>
        </ul>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function() {
        $("a.luosuo:first").click(function() {
            $("div.notice:first").hide();
            $("a.reluosuo:first").show();
        });

        $("a.reluosuo:first").click(function() {
            $("div.notice:first").show();
            $(this).hide();
        });
    });
</script>

