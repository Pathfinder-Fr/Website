<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.Analytics.Pages.FilterUrlActivityPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Import Namespace="Sueetie.Core" %>
<%@ Import Namespace="Sueetie.AddonPack" %>
<%@ Register Src="/admin/controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Url Activity Details
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="UrlActivity">
            <div class="AdminTextTalk">
                <h2>
                    Url Activity Details</h2>
                <div class="AdminFormDescription">
                    <p>
                        Activity for Url Root <strong>
                            <%= Request.QueryString["root"].ToString() %></strong>
                    </p>
                </div>
            </div>
            <table border="1" id="UrlActivityTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            Url
                        </th>
                        <th>
                            DateTime
                        </th>
                        <th>
                            Remote IP
                        </th>
                        <th>
                            User Agent
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rtpUrlActivity" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td class="lightUrlCol rawUrls">
                                <a href='<%# Eval("url").ToString()%>' target="_blank">
                                    <%# Eval("url")%></a>
                            </td>
                            <td class="remoteIpDateTimeCol">
                                <%# Eval("requestDateTime")%>
                            </td>
                            <td>
                                <a href='<%# AddonPackHelper.FormatGeoUrl(Eval("remoteIP").ToString()) %>' target='_blank'>
                                    <%# Eval("remoteIP")%></a>
                            </td>
                            <td>
                                <%# Eval("userAgent")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="ActivityLinksArea">
                <a href="../addonpack/manualIpRanges.aspx">Block Access by IP Range</a>|<a href="../addonpack/BlockedAgents.aspx"  class="last">
                    Block Access by Agent</a>
            </div>
        </div>
        <script type="text/javascript">

            $('#UrlActivityTable').dataTable({
                "aaSorting": [[1, "desc"]],
                "iDisplayLength": 50,
                "aoColumns": [
						null,
						null,
                        null,
                        null
					]
            });

        </script>
    </div>
</asp:Content>
