<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.ReportsDefaultPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Analytics - Reports
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
                    Sueetie Analytics - Reports</h2>
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
                            Search and Tag Analytics</div>
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" ID="hlTagClicks" ClientIDMode="Static" Text="Tag Activity" /></li>
                            <li>
                                <asp:HyperLink runat="server" ID="hlSearchTerms" ClientIDMode="Static" Text="Searches" /></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
