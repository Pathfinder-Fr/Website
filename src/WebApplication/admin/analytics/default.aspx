<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.AnalyticsDefaultPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Analytics
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <script type="text/javascript">

        $(document).ready(function () {
            (function ($) {
                var qstring = $.parseQuery();
                if (qstring.v > 0)
                    jAlert('30-Day Trial has expired and no license has been installed.\nFree and paid licenses are available in the Sueetie Marketplace. ', 'Sueetie Analytics is Disabled', null);
            })(jQuery)
        });

    </script>
    <asp:Panel ID="pnlMenu" runat="server">
        <div class="AdminFormArea">
            <div class="MainMenuContents">
                <h2>
                    Sueetie Analytics</h2>
                <div class="AdminTextTalk">
                    <p>
                        Sueetie Analytics Menu</p>
                    <div class="MainMenuList">
                        <div class="AdminULHeader">
                            Page View Analytics</div>
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" ID="hlViewsAllPages" ClientIDMode="Static" Text="All Page Activity" /></li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlViewsAllMembers" ClientIDMode="Static" Text="All Member Activity" /></li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlViewsTopMembers" ClientIDMode="Static" Text="Most Active Members" /></li>
                        </ul>
                        <div class="AdminULHeader">
                            Blog Analytics</div>
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" ID="hlBlogRss" ClientIDMode="Static" Text="Blog Rss Subscription and Reach" /></li>
                        </ul>
                        <div class="AdminULHeader">
                            Search Analytics</div>
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" ID="hlTagClicks" ClientIDMode="Static" Text="Tag Activity" /></li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlSearchTerms" ClientIDMode="Static" Text="Searches" /></li>
                        </ul>
                        <div class="AdminULHeader">
                            Analytics Data Maintenance and Configuration</div>
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" ID="hlFilterUsers" ClientIDMode="Static" Text="Filter Users" /></li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlFilterUrls" ClientIDMode="Static" Text="Filter Urls" /></li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlPageRules" ClientIDMode="Static" Text="Page Rules" /></li>
                            <%--                            <li>
                                <asp:HyperLink runat="server" ID="hlRemoveRefreshes" ClientIDMode="Static" Text="Remove Refreshed Page Entries" /></li>--%>
                            <li>
                                <asp:HyperLink runat="server" ID="hlSettingsAnalytics" ClientIDMode="Static" Text="Sueetie Analytics Settings" /></li>
                        </ul>
                    </div>
                    <div class="LicenseBox">
                        <asp:Label ID="lblLicenseType" runat="server" CssClass="licenseBoxType" />
                        <asp:Label ID="lblLicenseInfo" runat="server" CssClass="licenseBoxInfo" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<%--
  <li><a href="ReportAllPages.aspx">All Page Activity</a></li>
                        <li><a href="ReportAllMembers.aspx">All Member Activity</a></li>
                        <li><a href="ReportTopMembers.aspx">Most Active Members</a></li>
                    </ul>
                    <div class="AdminULHeader">
                        Analytics Data Maintenance and Configuration</div>
                    <ul>
                        <li><a href="FilterUsers.aspx">Filter Users</a></li>
                        <li><a href="FilterUrls.aspx">Filter Urls</a></li>
                        <li><a href="PageRules.aspx">Page Rules</a></li>
                        <li><a href="RemoveRefreshes.aspx">Remove Refreshed Page Entries</a></li>
                        <li><a href="SettingsAnalytics.aspx">Sueetie Analytics Settings</a> </li>
--%>