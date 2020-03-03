<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Commerce.Pages.PaypalStandardPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<%@ Import Namespace="Sueetie.Commerce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Marketplace - PayPal Standard Configuration
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
                PayPal Standard Configuration</h2>
            <div class="AdminTextTalk">
                <div class="AdminFormDescription">
                    <p>
                        Configuration for using PayPal Standard Live and Sandbox Payment Systems.</p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
        <asp:Label ID="lblPaymentServiceName" runat="server" CssClass="payServiceFormTitle" />
            <div class="paymentForm">
                <table width="100%">
                    <tr>
                        <td class="formLabel">
                            Account Name
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtAccountName" runat="server" CssClass="BigTextBox" Width="250px" />
                            <asp:RequiredFieldValidator runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtAccountName" />
                                <div class="AdminUserFieldInfo">Your PayPal email account name, or if Sandbox your test business owner email</div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabel verticalTop">
                            Purchase Url
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtPurchaseUrl" runat="server" CssClass="BigTextBox" Width="700px" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtPurchaseUrl" />
                                <div class="AdminUserFieldInfo">Link used when adding an item to the shopping cart</div>
                        </td>
                    </tr>
                                      <tr>
                        <td class="formLabel verticalTop">
                            Cart Url
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtCartUrl" runat="server" CssClass="BigTextBox" Width="700px" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtCartUrl" />
                                <div class="AdminUserFieldInfo">Link used when clicking on the shopping cart</div>
                        </td>
                    </tr>
                                      <tr>
                        <td class="formLabel verticalTop">
                            Transaction Url
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtTransactionUrl" runat="server" CssClass="BigTextBox" Width="700px" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtTransactionUrl" />
                                <div class="AdminUserFieldInfo">Url used when retrieving transaction details</div>
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Return Url
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtReturnUrl" runat="server" CssClass="BigTextBox" Width="500px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtReturnUrl" />
                                 <div class="AdminUserFieldInfo">Url on return from a PayPal Purchase</div>
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            Shopping Url
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtShoppingUrl" runat="server" CssClass="BigTextBox" Width="500px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtShoppingUrl" />
                                 <div class="AdminUserFieldInfo">Url on return from PayPal "Continue Shopping" link</div>
                        </td>
                    </tr>
                      <tr>
                        <td class="formLabel">
                            PayPal Identity Token
                        </td>
                        <td class="formField">
                            <asp:TextBox ID="txtIdentityToken" runat="server" CssClass="BigTextBox" Width="600px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" *" CssClass="BigErrorMessage"
                                ControlToValidate="txtIdentityToken" />
                                 <div class="AdminUserFieldInfo">Used in obtaining transaction details on Sueetie ReturnUrl completepurchase.aspx page</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div class="TextButtonBigArea">
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="TextButtonBig"
                                    OnCommand="btnUpdate_OnCommand" CausesValidation="true" CommandName="Update" />
                            </div>
                            <br />
                            <br />
                            <div id="ResultMessage" class="ResultsMessage">
                                <asp:Label ID="lblResults" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
