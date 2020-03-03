<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Commerce.Pages.SettingsMarketplacePage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Marketplace - Manage Settings
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="7" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminMarketplaceNavLinks ID="adminMarketplaceNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <h2>
                Marketplace Settings</h2>
            <div class="AdminTextTalk">
                <div class="AdminFormDescription">
                    <p>
                        General Marketplace Configuration Settings</p>
                </div>
            </div>
        </div>
        <div class="AdminFormInner">
            <table width="100%" class="SettingsAddonPackTable">
                <tr class="rwAddSpace">
                    <td class="formLabel">
                        Marketplace Release
                    </td>
                    <td class="formField">
                        <asp:Label ID="lblVersion" runat="server" CssClass="BigFormText" />
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        Activity Report Records
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtActivityReportNum" class="adminHeavyText" Width="60px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" *"
                            CssClass="BigErrorMessage" ControlToValidate="txtActivityReportNum" />
                        <div class="AdminUserFieldInfo">
                            Records retrieved in Recent Activity Report. Default: <strong>500</strong></div>
                    </td>
                </tr>
                  <tr>
                    <td class="formLabel">
                        Maximum Full Image Size
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtMaxFullImageSize" class="adminHeavyText" Width="60px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *"
                            CssClass="BigErrorMessage" ControlToValidate="txtMaxFullImageSize" />
                        <div class="AdminUserFieldInfo">
                            Used in calculating size of large product image. Default: <strong>450</strong></div>
                    </td>
                </tr>
                   <tr>
                    <td class="formLabel">
                        Medium Image Height/Width
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtFixedMediumImageHeight" class="adminHeavyText" Width="60px" /> 
                        <asp:TextBox runat="server" ID="txtFixedMediumImageWidth" class="adminHeavyText" Width="60px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *"
                            CssClass="BigErrorMessage" ControlToValidate="txtFixedMediumImageHeight" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" *"
                            CssClass="BigErrorMessage" ControlToValidate="txtFixedMediumImageWidth" />
                        <div class="AdminUserFieldInfo">
                            Fixed height and width of medium-sized images. Default: <strong>102h x 136w</strong></div>
                    </td>
                </tr>
                                   <tr>
                    <td class="formLabel">
                        Small Image Height/Width
                    </td>
                    <td class="formField">
                        <asp:TextBox runat="server" ID="txtFixedSmallImageHeight" class="adminHeavyText" Width="60px" /> 
                        <asp:TextBox runat="server" ID="txtFixedSmallImageWidth" class="adminHeavyText" Width="60px" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" *"
                            CssClass="BigErrorMessage" ControlToValidate="txtFixedSmallImageHeight" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage=" *"
                            CssClass="BigErrorMessage" ControlToValidate="txtFixedSmallImageWidth" />
                        <div class="AdminUserFieldInfo">
                            Fixed height and width of medium-sized images. Default: <strong>42h x 56w</strong></div>
                    </td>
                </tr>
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
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
