<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Commerce.Pages.MarketplaceDefaultAdminPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Marketplace
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="7" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminMarketplaceNavLinks ID="adminMarketplaceNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <script type="text/javascript">

        $(document).ready(function () {
            (function ($) {
                var qstring = $.parseQuery();
                if (qstring.v > 0)
                    jAlert('30-Day Trial has expired and no license has been installed.\nFree and paid licenses are available in the Sueetie Marketplace. ', 'Marketplace is Disabled', null);
            })(jQuery)
        });


    </script>
    <asp:Panel ID="pnlMenu" runat="server">
        <div class="AdminFormArea">
            <h2>
                Sueetie Marketplace</h2>
            <div class="AdminTextTalk">
                <p>
                    Sueetie Marketplace Menu</p>
                <ul>
                    <li>
                        <asp:HyperLink runat="server" ID="hlManageProducts" ClientIDMode="Static" Text="Manage Products" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlAddNewProduct" ClientIDMode="Static" Text="Add New Product" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlManageCategories" ClientIDMode="Static" Text="Manage Categories" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlSettingsMarketplace" ClientIDMode="Static" Text="Marketplace Settings" /></li>
                    <li>
                        <asp:HyperLink runat="server" ID="hlPaymentServices" ClientIDMode="Static" Text="Payment Services" /></li>
                    <li class="SpacerLI">
                        <asp:HyperLink runat="server" ID="hlActivityReport" ClientIDMode="Static" Text="Marketplace Activity Report" /></li>
                    <li id="SueetieProductKeyLI" runat="server">
                        <a href="SueetieProductKey.aspx">Create Member Sueetie Product Key</a></li>
                </ul>
            </div>
            <div class="LicenseBox">
                <asp:Label ID="lblLicenseType" runat="server" CssClass="licenseBoxType" />
                <asp:Label ID="lblLicenseInfo" runat="server" CssClass="licenseBoxInfo" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
