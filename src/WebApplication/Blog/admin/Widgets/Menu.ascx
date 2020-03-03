<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="BlogEngine.Admin.Widgets.Menu" %>
<ul>
    <li <%=Current("Blogroll.aspx")%>><a href="Blogroll.aspx"><%=Resources.labels.blogroll %></a></li>
    <li <%=Current("Controls.aspx")%>><a href="Controls.aspx"><%=Resources.labels.commonControls %></a></li>
</ul>