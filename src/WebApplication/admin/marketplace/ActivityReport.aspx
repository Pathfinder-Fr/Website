<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Commerce.Pages.ActivityReportPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

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
    Sueetie Marketplace - Activity Report
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
                Recent Activity Report</h2>
            <div class="AdminTextTalk">
                <div class="AdminFormDescription">
                    <p>
                        Marketplace downloads and purchases.  Click on Product or Category to filter display, or use the search filter at top right. </p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
        <a href="ActivityReport.aspx">Display All Recent Activity</a>
            <table border="1" id="ProductTable" class="ProductTable MarketplaceActivityTable">
                <thead>
                    <tr>
                        <th>
                            Title
                        </th>
                        <th>Category</th>
                        <th>
                        Date
                        </th>
                        <th>User</th>
                        <th>Action</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptRecentActivity" runat="server" OnItemDataBound="rptRecentActivity_OnItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="col_purchaseTitle">
                            <a href="ActivityReport.aspx?p=<%#Eval("ProductID")%>"><%#Eval("Title")%></a>
                            <asp:Label ID="lblTransactionXID" runat="server" CssClass="PurchaseTableTransactionXID" />
                            </td>
                            <td class="col_purchaseCategory">
                            <a href="ActivityReport.aspx?c=<%#Eval("CategoryID")%>"><%#Eval("CategoryName")%></a>
                            </td>
                            <td class="col_purchaseDate">
                             <%# Convert.ToDateTime(Eval("PurchaseDateTime")).ToString("MM/dd/yyyy") %>
                            </td>
                            <td class="col_purchaseUser">
                            <a href="mailto:<%#Eval("Email")%>"><img src="/themes/lollipop/images/email.gif" alt="Email this Customer" /></a><%#Eval("DisplayName")%>
                            </td>
                            <td class="col_purchaseAction">
                             <%#Eval("ActionCode")%>
                            </td>
                            <td class="col_purchasePrice">
                            <asp:Label ID="lblPrice" runat="server" />
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
                    "iDisplayLength": 25,
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
    </div>
</asp:Content>
