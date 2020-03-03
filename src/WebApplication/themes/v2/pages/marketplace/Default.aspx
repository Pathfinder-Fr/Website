<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.MarketplaceDefaultPage" Title="Sueetie Marketplace - Home Page" MasterPageFile="~/Themes/v2/Masters/marketplace.master" %>

<%@ Register TagPrefix="uc1" TagName="CategoryBrowse" Src="/controls/marketplace/CategoryBrowse.ascx" %>
<%@ Import Namespace="Sueetie.Commerce" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/v2/style/datatables.css" rel="stylesheet" type="text/css" />
    <script src="/scripts/jquery.alerts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="NavBar" ID="NavBarContent" runat="server">
    <h1 class="section">
        <a href="default.aspx">Accueil</a><SUEETIE:CommerceBreadCrumbs ID="CommerceBreadCrumbs1" runat="server" />
    </h1>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="cphBody" runat="server">
    <div id="body">
        <asp:Panel runat="server" ID="pnlCategoryBrowser" ClientIDMode="Static">
            <uc1:CategoryBrowse ID="CategoryBrowser" runat="server" AutoNavigate="True"></uc1:CategoryBrowse>
        </asp:Panel>
        <div runat="server" id="borderDiv" class="productListTop" clientidmode="Static">
        </div>
        <asp:Panel ID="pnlProductsDisplay" runat="server" ClientIDMode="Static" CssClass="productDisplayTable">
            <table border="1" id="ProductTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th></th>
                        <th>Nom</th>
                        <th>Publication</th>
                        <th>Prix</th>
                        <th>Categoie</th>
                        <th>Type</th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptProducts" runat="server" OnItemDataBound="rptProducts_OnItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="col_photo">
                                <asp:Image ID="imgProductPhoto" AlternateText="" runat="server" Width="56" Height="56" />
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
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#ProductTable').dataTable({
                "aaSorting": [[2, "desc"]],
                "iDisplayLength": 5,
                "aoColumns": [
                        null,
                        null,
                        null,
                        null,
                        null,
                        null
                ]
            })
        });


        $(document).ready(function () {
            (function ($) {
                if ($.trim($("#pnlCategoryBrowser").text()) == "" || $.trim($("#pnlProductsDisplay").text()) == "") {
                    $('#borderDiv').hide();
                }
            })(jQuery)
        });

    </script>
</asp:Content>
