<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Themes.aspx.cs" Inherits="Sueetie.Web.AdminThemes" %>

<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Update Current Theme
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminFormLeft">
            <img src="/images/shared/sueetie/themes20.png" />
        </div>
        <div class="AdminFormRight">
            <h2>
                Current Site Theme</h2>
            <div class="AdminHalfDescription">
                Enter the new theme key. The theme key matches the theme's folder name as shown at left.  Clicking "submit" will restart Sueetie, so the new theme should appear immediately. </div>
            <div class="AdditionalMessageNotes">
                <strong>Note: </strong>The Admin area theme is "lollipop" by default. This is updated
                in the /Sueetie.Config Core AdminTheme property.</div>
            <div class="AdminFormInner">
                <table width="100%">
                    <tr class="rwAddSpace">
                        <td class="formLabel">
                            Site Web Theme
                        </td>
                        <td class="formField">
                            <asp:TextBox runat="server" ID="txtTheme" class="adminHeavyText" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel">
                            Mobile Theme
                        </td>
                        <td class="formField">
                            <asp:TextBox runat="server" ID="txtMobileTheme" class="adminHeavyText" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div class="TextButtonBigArea">
                                <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="TextButtonBig"
                                    OnClick="Submit_Click" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage" />
            <br />
            <asp:Label ID="lblResultsDetails" runat="server" Visible="false" CssClass="ResultsMessageDetails" />
        </div>
    </div>
</asp:Content>
