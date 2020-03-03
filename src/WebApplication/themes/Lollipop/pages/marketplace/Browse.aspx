<%@ Page Language="C#" Inherits="Sueetie.Commerce.Pages.BrowsePage"
    Title="Sueetie Marketplace - All Products" MasterPageFile="/Themes/Lollipop/Masters/marketplace.master" %>

<%@ Import Namespace="Sueetie.Commerce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="NavBar" ID="NavBarContent" runat="server">
    <div id="crumbs_text">
        <a href="Default.aspx">Marketplace Home</a>&nbsp;&raquo;&nbsp; <a href="Browse.aspx">
            All Products</a>
        <SUEETIE:CommerceBreadCrumbs ID="CommerceCrumbs1" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="cphBody" runat="server">
    <div id="body">
        <asp:Panel ID="pnlWhatsNew" runat="server">
            <div class="WhatsNew">
                What's new:
                <asp:DropDownList ID="CommonWhatsNewRangeList" runat="server">
                    <asp:ListItem Value="30" Selected="True">in the last 30 days</asp:ListItem>
                    <asp:ListItem Value="60">in the last 60 days</asp:ListItem>
                    <asp:ListItem Value="90">in the last quarter</asp:ListItem>
                    <asp:ListItem Value="365">in the last year</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="CommonWhatsNewButton" runat="server" Text="Go" CssClass="TextButtonSmall"
                    PostBackUrl="Browse.aspx" OnClick="CommonWhatsNewButton_Click" />
            </div>
        </asp:Panel>
        <h3 class="section">
            <asp:Label ID="lblTitle" runat="server" />
        </h3>
        <div id="allAdsContent">
            <table border="1" id="ProductTable" class="AgentsTable">
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
        </div>
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
                        null
					]
            })
        });

    </script>
</asp:Content>
