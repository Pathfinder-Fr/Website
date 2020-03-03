<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.AddonPack.Pages.SiteAccessReportPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Import Namespace="Sueetie.AddonPack" %>
<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Recent Site Access Activity Report
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="RemoteIpAccess">
            <div class="AdminTextTalk">
                <h2>
                    Recent Site Activity Report</h2>
                <div class="AdminFormDescription">
                    <p>
                        Report of all recent logged site access activity listed by request datetime.
                    </p>
                </div>
            </div>
            <table border="1" id="IpAccessTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            UserID
                        </th>
                        <th>
                            DateTime
                        </th>
                        <th>
                            Remote IP
                        </th>
                        <th>
                            Page
                        </th>
                        <th>
                            User Agent
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptIpLog" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("userId")%>
                            </td>
                            <td class="remoteIpDateTimeCol">
                                <%# Eval("requestDateTime")%>
                            </td>
                            <td class="remoteIpDateTimeCol">
                                <a href='<%# AddonPackHelper.FormatGeoUrl(Eval("remoteIP").ToString()) %>' target='_blank'>
                                    <%# Eval("remoteIP")%></a><a href='RemoteIpPages.aspx?ip=<%# Eval("remoteIP") %>' target='_blank'>
                                <img src="/themes/lollipop/images/rightarrow.png" /></a>
                            </td>
                            <td class="lightUrlCol rawUrls">
                                <a href='<%# Eval("url").ToString()%>' target="_blank">
                                    <%# Eval("url")%></a>
                            </td>
                            <td>
                                <%# Eval("UserAgent")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>

        </div>
        <script type="text/javascript">

            $('#IpAccessTable').dataTable({
                "aaSorting": [[1, "desc"]],
                "iDisplayLength": 50,
                "aoColumns": [
						null,
						null,
                        null,
                        null,
                        null
					]
            });

        </script>
    </div>
</asp:Content>
