<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.AddonPackDefaultPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Addon Pack
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <script type="text/javascript">

        $(document).ready(function () {
            (function ($) {
                var qstring = $.parseQuery();
                if (qstring.v > 0)
                    jAlert('30-Day Trial has expired and no product key has been installed.\nFree and Sueetie Supporter product keys are available in the Sueetie Marketplace. ', 'Addon Pack is Disabled', null);
            })(jQuery)
        });


    </script>
    <asp:Panel ID="pnlMenu" runat="server">
        <div class="AdminFormArea">
            <h2>
                Sueetie Addon Pack</h2>
            <div class="AdminTextTalk">
                <p>
                    Sueetie Addon Pack Menu</p>
                <ul>
                    <li>
                        <asp:HyperLink runat="server" ID="hlSettingsAddonPack" ClientIDMode="Static" Text="Addon Pack Settings" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlClientAccess" ClientIDMode="Static" Text="Site Access Control" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlSlideShows" ClientIDMode="Static" Text="Slideshows" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlBlogPostImages" ClientIDMode="Static" Text="Blog Post Thumbnails" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlForumAnswers" ClientIDMode="Static" Text="Forum Answers" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlMediaSets" ClientIDMode="Static" Text="Media Sets" /></li>
                </ul>
            </div>
            <div class="LicenseBox">
                <asp:Label ID="lblLicenseType" runat="server" CssClass="licenseBoxType" />
                <asp:Label ID="lblLicenseInfo" runat="server" CssClass="licenseBoxInfo" />
            </div>
            <asp:Panel ID="pnlExceed" runat="server" Visible="false">
                <div class="LicenseBox">
                    <asp:Label ID="lblExceedHeader" runat="server" CssClass="licenseBoxType" />
                    <asp:Label ID="lblExceedInfo" runat="server" CssClass="licenseBoxInfo" />
                </div>
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
