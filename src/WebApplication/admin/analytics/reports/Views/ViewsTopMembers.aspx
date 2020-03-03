<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.ViewsTopMembersPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Analytics Page Activity Reports
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AnalyticsReportPages">
            <div class="AdminTextTalk">
                <div class="ReportTitleSelectArea">
                    <div class="Title">
                        Analytics Page Activity Report - All Members</div>
                    <div class="SelectArea">
                        Date Range:
                        <asp:DropDownList ID="ddDaySpans" runat="server" OnSelectedIndexChanged="UpdateQuery"
                            AutoPostBack="true" />
                    </div>
                </div>
                <div class="HeaderBox">
                    Member Activity for Last <strong>
                        <%= DisplayDateRange() %>. </strong>Total Views for Period: <strong>
                            <%= TotalViews %>.</strong>
                </div>
                <div class="TableTopSelectArea">
                    Content Pages Only
                    <asp:CheckBox ID="chkContentOnly" runat="server" OnCheckedChanged="UpdateQuery" Checked="false"
                        AutoPostBack="true" />
                </div>
            </div>
            <table border="1" id="PageTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            Count
                        </th>
                        <th>
                            Member
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptIpLog" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("count")%>
                            </td>
                            <td class="lightUrlCol rawUrls">
                                <a href='ViewsForMember.aspx?userid=<%# Eval("userid").ToString()%>&co=<%# chkContentOnly.Checked %>'>
                                    <%# Eval("displayname").ToString()%>
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlFreeLicenseRow" runat="server">
                    <tr>
                        <td colspan="3">
                            <SUEETIE:SueetieLocal ID="SueetieLocal1" Key="analytics_report_limit" LanguageFile="Licensing.xml"
                                runat="server" />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </div>
        <script type="text/javascript">

            $('#PageTable').dataTable({
                "aaSorting": [[0, "desc"]],
                "iDisplayLength": 25,
                "aoColumns": [
						null,
						null
					]
            });

        </script>
    </div>
</asp:Content>
