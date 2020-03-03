<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Commerce.Pages.ManageProductsPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminMarketplaceNavLinks.ascx" TagName="adminMarketplaceNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<%@ Import Namespace="Sueetie.Commerce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Marketplace - Manage Products
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
                Manage Products</h2>
            <div class="AdminTextTalk">
                <div class="AdminFormDescription">
                    <p>
                        Select Marketplace products to manage. Click on product photo to manage photos.</p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <table border="1" id="ProductTable" class="ProductTable">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                            Title
                        </th>
                        <th>
                            Released
                        </th>
                        <th>
                            Price
                        </th>
                        <th>
                            Category
                        </th>
                        <th>
                            Type
                        </th>
                        <th>
                            Views
                        </th>
                        <th>
                            Actions
                        </th>
                        <th>
                            Active
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptProducts" runat="server" OnItemDataBound="rptProducts_OnItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="col_photo">
                                <a href="ManagePhotos.aspx?id=<%# Eval("ProductID") %>">
                                    <asp:Image ID="imgProductPhoto" AlternateText="" runat="server" Width="56" Height="54" /></a>
                            </td>
                            <td class="col_title">
                                <asp:HyperLink ID="hlProduct" runat="server" Text='<%# Eval("Title") %>' CssClass="mpListTitle" />
                                <div class="title_field_subtext">
                                    <%# Eval("SubTitle") %>
                                </div>
                            </td>
                            <td class="col_startdate">
                                <%# Convert.ToDateTime(Eval("DateCreated")).ToString("MM/dd/yyyy") %>
                            </td>
                            <td class="col_price">
                                <%#  CommerceHelper.FreeIt(Convert.ToDecimal(Eval("Price")))%>
                            </td>
                            <td class="col_category">
                                <asp:HyperLink ID="hlCategory" runat="server" Text='<%# Eval("CategoryName") %>' />
                            </td>
                            <td class="col_general">
                                <%# CommerceHelper.PurchaseTypeToString(Eval("PurchaseTypeID")) %>
                            </td>
                            <td class="col_general">
                                <%# Eval("NumViews") %>
                            </td>
                            <td class="col_general">
                                <%# Eval("NumDownloads") %>
                            </td>
                            <td class="col_activeImage">
                                <asp:Image ID="imgIsActive" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <script type="text/javascript">

            $(document).ready(function () {
                $('#ProductTable').dataTable({
                    "aaSorting": [[2, "desc"]],
                    "iDisplayLength": 10,
                    "aoColumns": [
						null,
						null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null
					]
                })
            });
        </script>
    </div>
</asp:Content>
