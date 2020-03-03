<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.SiteAccessMenuPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Addons - Site Access Control
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="MainMenuContents">
            <h2>
                Sueetie Addon Pack - Site Access Control</h2>
            <div class="AdminTextTalk">
                <p>
                    The Site Access Control Module gives you the ability to control who can access your
                    Sueetie website. You can block by remote IP range, by agent, or by entire country.
                    See the <a href="http://sueetie.com/wiki/SueetieHowTos.ashx">Sueetie How-To Guides</a>
                    in the Sueetie Wiki for details.</p>
                <div class="MainMenuList">
                    <div class="AdminULHeader">
                        Site Access by County, Remote IP and User Agent</div>
                    <ul>
                        <li><a href="BlockByCountry.aspx">Site Access By Country</a></li>
                        <li><a href="ManualIpRanges.aspx">Site Access By Remote IP</a></li>
                        <li><a href="BlockedAgents.aspx">Site Access By User Agent</a></li>
                    </ul>
                    <div class="AdminULHeader">
                        Site Access Maintenance</div>
                    <ul>
                        <li><a href="UpdateIpRanges.aspx">Import/Update Blocked Country IPs From CIDR Files</a></li>
                        <li><a href="CheckIP.aspx">Check Blocked Status of IP Address</a></li>
                        <li><a href="CheckAgent.aspx">Check Blocked Status of Agent String</a></li>
                        <li><a href="BlockedCountries.aspx">Manage Country List</a></li>
                        <li><a href="TruncateRequestTable.aspx">Clear User Agent Request Log</a></li>
                    </ul>
                    <div class="AdminULHeader">
                        Site Access Reporting</div>
                    <ul>
                        <li><a href="RecentAccessReport.aspx">Recent Site Access Activity</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
