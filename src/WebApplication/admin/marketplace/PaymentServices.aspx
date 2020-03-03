<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Commerce.Pages.PaymentServicesPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

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
    Sueetie Marketplace - Payment Services
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
                Manage Payment Services</h2>
            <div class="AdminTextTalk">
                <div class="AdminFormDescription">
                    <p>
                        Select Marketplace payment service to configure.  Select Primary payment service below.</p>
                </div>
            </div>
        </div>
        <div class="MarketPlaceAdministration">
            <table border="1" id="ProductTable" class="ProductTable PaymentSystemTable">
                <thead>
                    <tr>
                        <th>
                            Payment Service
                        </th>
                        <th>
                            Is Primary
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptPaymentServices" runat="server" OnItemDataBound="rptPaymentServices_OnItemDataBound" OnItemCommand="rptPaymentServices_OnCommand">
                    <ItemTemplate>
                        <tr>
                            <td class="col_payService">
                                <asp:HyperLink ID="hlPaymentService" runat="server" CssClass="RepeaterPaymentServiceLink" />
                                <div class="RepeaterPaymentServiceDesc">
                                <%# Eval("PaymentServiceDescription") %>
                                </div>
                            </td>
                            <td class="col_payServiceYesNo">
                                <img src='/images/shared/sueetie/<%#Eval("IsPrimary")%>.png' alt='<%#Eval("IsPrimary")%>' />
                            </td>
                            <td class="col_payServiceButton">
                            <asp:Button ID="btnMakePrimary" runat="server"  Text="Make Primary" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#ProductTable').dataTable({
                "aaSorting": [[0, "asc"]],
                "iDisplayLength": 10,
                "aoColumns": [
						null,
						null,
                        null
					]
            })
        });
    </script>
    </div>
</asp:Content>
