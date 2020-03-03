<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.AddonPack.Pages.BlockByCountryPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Block Access By Country
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Block Site Access by Country</h2>
            <div class="AdminFormDescription">
                <p>
                <asp:Label ID="lblFormDescription" runat="server" />
                </p>
            </div>
        </div>
        <div class="AdminFormInner">
            <asp:CheckBoxList runat="server" ID="cblBlockedCountries" RepeatLayout="Table" RepeatColumns="2"
                CssClass="CountryCheckBox" />
        </div>
        <div class="TextButtonBigArea CountryButton">
            <asp:Button ID="btnUpdate" runat="server" Text=" Process " CssClass="TextButtonBig"
                OnClick="btnUpdate_OnClick" CausesValidation="false" />
        </div>
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div>
    </div>
</asp:Content>
