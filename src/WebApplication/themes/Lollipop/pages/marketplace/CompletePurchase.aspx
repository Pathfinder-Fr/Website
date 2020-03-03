<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.CompletePurchasePage" Title="Sueetie Marketplace - Home Page"
    MasterPageFile="~/Themes/Lollipop/Masters/marketplace.master" %>

<%--
<%@ Register TagPrefix="uc1" TagName="FeaturedAd" Src="Controls/FeaturedAd.ascx" %>--%>
<%@ Import Namespace="Sueetie.Commerce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="NavBar" ID="NavBarContent" runat="server">
    <div id="crumbs_text">
        <a href="Default.aspx">Marketplace Home</a>
    </div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="cphBody" runat="server">
    <div id="body">
        <h3 class="section">
        <asp:Label ID="lblPurchaseTitle" runat="server" />
        </h3>
        <div class="purchaseDisplayBody">
            <asp:Panel ID="pnlPurchases" runat="server">
                <div class="purchaseDisplayArea">
                    <asp:Panel ID="pnlTransactionCode" runat="server" CssClass="purchaseTransactionXID">
                        <SUEETIE:SueetieLocal LanguageFile="marketplace.xml" Key="productpurchase_transactionxid"
                            LocalCssID="transactionLabel" runat="server" Anchor="span" />
                        <asp:Label ID="lblTransactionXID" runat="server" CssClass="transactionCode" />
                    </asp:Panel>
                    <div class="purchaseHeader">
                        Your Purchases
                    </div>
                    <asp:Repeater ID="rptPurchases" runat="server" OnItemDataBound="rptPurchases_OnItemDataBound">
                        <HeaderTemplate>
                            <table class="purchaseTable">
                                <thead>
                                    <tr>
                                        <th>
                                            Product
                                        </th>
                                        <th>
                                            Category
                                        </th>
                                        <th>
                                            Price
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="even">
                                <td>
                                    <asp:Label ID="lblTitle" runat="server" />
                                </td>
                                <td>
                                    <%# Eval("Categoryname") %>
                                </td>
                                <td>
                                    <%# Eval("Price", "{0:c}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="odd">
                                <td>
                                    <asp:Label ID="lblTitle" runat="server" />
                                </td>
                                <td>
                                    <%# Eval("Categoryname") %>
                                </td>
                                <td>
                                    <%# Eval("Price", "{0:c}") %>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlLicenses" runat="server">
                <div class="purchaseDisplayArea">
                    <div class="PurchaseHeader">
                        Your Sueetie Product Keys
                    </div>
                    <asp:Repeater ID="rptLicenses" runat="server">
                        <HeaderTemplate>
                            <table class="purchaseTable">
                                <thead>
                                    <tr>
                                        <th>
                                            Product
                                        </th>
                                        <th>
                                            Community License
                                        </th>
                                        <th>
                                            Product Key
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="even">
                                <td>
                                    <%# Eval("PackageTypeDescription") %>
                                </td>
                                <td>
                                    <%# Eval("LicenseTypeDescription")%>
                                </td>
                                <td>
                                    <%# Eval("License")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="odd">
                                <td>
                                    <%# Eval("PackageTypeDescription") %>
                                </td>
                                <td>
                                    <%# Eval("LicenseTypeDescription")%>
                                </td>
                                <td>
                                    <%# Eval("License")%>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlSuccess" runat="server">
                <SUEETIE:ContentPart ID="cpPurchaseSuccess" runat="server" ContentName="PurchaseSuccess" />
            </asp:Panel>
            <asp:Panel ID="pnlFailure" runat="server" Visible="false">
                <SUEETIE:ContentPart ID="cpPurchaseFailure" runat="server" ContentName="PurchaseFailure" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
