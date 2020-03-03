<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.MaintenanceMenuPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Analytics - Data Maintenance and Configuration
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="MainMenuContents">
            <h2>
                Analytics Data Maintenance and Configuration</h2>
            <div class="AdminTextTalk">
                <p>
                    Maintenance and Configuration Menu</p>
                <div class="MainMenuList">
                    <div class="AdminULHeader">
                        Analytics Data Maintenance and Configuration</div>
                    <ul>
                        <li><a href="FilterUsers.aspx">Filter Users</a></li>
                        <li><a href="FilterUrls.aspx">Filter Urls</a></li>
                        <li><a href="PageRules.aspx">Page Rules</a></li>
                        <li><a href="../SettingsAnalytics.aspx">Sueetie Analytics Settings</a> </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
