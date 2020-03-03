<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="BlogEngine.Admin.Tracking.Menu" %>
<%@ Import Namespace="BlogEngine.Core" %>
<ul>
    <li <%=Current("Pingbacks.aspx")%>><a href="Pingbacks.aspx"><%=Resources.labels.pingbacksAndTrackbacks %></a></li>
    <li <%=Current("referrers.aspx")%>><a href="referrers.aspx"><%=Resources.labels.referrers %></a></li>
</ul>