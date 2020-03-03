<%@ Page Language="C#" AutoEventWireup="true" 
    Inherits="Sueetie.AddonPack.Pages.AccessByIpReportPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Manage Manually Entered Blocked IP Ranges
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
                    Site Access By Remote IP</h2>
                <div class="AdminFormDescription">
                    <p>
                        Site Access Report for <strong>
                            <%= Request.QueryString["ip"].ToString() %></strong>
                    </p>
                </div>
            </div>
       
        <table border="1" id="IpAccessTable" class="AgentsTable">
            <thead>
                <tr>
                    <th>
                        ContentID
                    </th>
                     <th>
                        UserID
                    </th>
                    <th>
                        DateTime
                    </th>
                    <th>
                        Url
                    </th>
                    <th>
                        PageTitle
                    </th>
                </tr>
            </thead>
            <asp:Repeater ID="rptIpLog" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("contentID")%>
                        </td>
                          <td>
                            <%# Eval("userId")%>
                        </td>
                        <td class="remoteIpDateTimeCol">
                            <%# Eval("requestDateTime")%>
                        </td>
                        <td class="lightUrlCol rawUrls">
                            <a href='<%# Eval("url").ToString()%>' target="_blank">
                                <%# Eval("url")%></a>
                        </td>
                        <td>
                            <%# Eval("PageTitle")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
                    <div class="ActivityLinksArea">
                <a href="RecentAccessReport.aspx" class="last">Recent Access Report</a>
            </div>
         </div>
        <script type="text/javascript">

            $('#IpAccessTable').dataTable({
                "aaSorting": [[2, "asc"]],
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
