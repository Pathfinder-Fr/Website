﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.ViewsForPagePage"
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
                        Analytics Page Activity Report - Selected Page</div>
                    <div class="SelectArea">
                        Members Only
                        <asp:CheckBox ID="chkMembersOnly" runat="server" OnCheckedChanged="UpdateQuery" Checked="false"
                            AutoPostBack="true" />
                        Date Range:
                        <asp:DropDownList ID="ddDaySpans" runat="server" OnSelectedIndexChanged="UpdateQuery"
                            AutoPostBack="true" />
                    </div>
                </div>
                <div class="HeaderBox">
                    <strong><a href='<%= PageUrl %>' target='_blank'>
                        <%= PageTitle %></a></strong>. Time Period: <strong>
                            <%= DisplayDateRange() %>.</strong> Total Views for Period: <strong>
                                <%= TotalViews %>.</strong>
                </div>
            </div>
            <table border="1" id="PageTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            User
                        </th>
                        <th>
                            DateTime
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptIpLog" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td class="lightUrlCol rawUrls">
                                <a href='ViewsForMember.aspx?userid=<%# Eval("userid").ToString()%>'>
                                    <%# Eval("displayname")%></a>
                            </td>
                            <td>
                                <%# string.Format("{0:F}", Convert.ToDateTime(Eval("requestdatetime").ToString()))%>
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
            <div class="ActivityLinksArea">
                <a href="ViewsAllMembers.aspx">Member Activity</a>|<a href="ViewsByApplication.aspx?appid=<%= ApplicationID %>">Application
                    Activity</a>|<a href="ViewsAllPages.aspx" class="last">All Page Activity</a>
            </div>
        </div>
        <script type="text/javascript">

            $('#PageTable').dataTable({
                "aaSorting": [[1, "desc"]],
                "iDisplayLength": 25,
                "aoColumns": [
						null,
						null
					]
            });

        </script>
    </div>
</asp:Content>
