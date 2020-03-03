<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.AdminReportsDefault" %>

<%@ Register Src="../controls/adminReportsNavLinks.ascx" TagName="adminReportsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Reports
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="4" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminReportsNavLinks ID="adminReportsNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
 <div class="AdminFormArea">
        <h2>
           Reports</h2>
        <div class="AdminTextTalk">
            <p>
                Reports Menu</p>
                <ul>
                    <li><a href="BackgroundTasks.aspx">Background Tasks</a></li>                
                    <li><a href="EventLogs.aspx">Event Logs</a></li>
                    <li><a href="Downloads.aspx">Downloads Report</a></li>
                    <li><a href="UserActivities.aspx">User Activities</a></li>
                    <li><a href="Subscribers.aspx">Newsletter Subscribers</a></li>
                </ul>
        </div>
        </div>
</asp:content>
