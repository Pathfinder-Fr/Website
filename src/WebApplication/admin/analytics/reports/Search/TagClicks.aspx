<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.TagClicksPage"
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
    Tag Activity
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
                        Tag Activity</div>
                    <div class="SelectArea">
                        Date Range:
                        <asp:DropDownList ID="ddDaySpans" runat="server" OnSelectedIndexChanged="UpdateQuery"
                            AutoPostBack="true" />
                    </div>
                </div>
                <div class="HeaderBox">
                    Tag Activity for the Last <strong>
                        <%= DisplayDateRange() %>. </strong>Total Clicks: <strong>
                            <%= TotalViews %>.</strong>
                </div>
                <div class="TableTop">
                    <div class="TableTopLeft">
                    </div>
                    <div class="TableTopRight BigLink">
                        Members Only <asp:CheckBox ID="chkMembersOnly" runat="server" OnCheckedChanged="UpdateQuery" AutoPostBack="true" />
                    </div>
                </div>
            </div>
            <table border="1" id="PageTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            Tag
                        </th>
                        <th>
                            Clicks
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptIpLog" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                                <td class="lightUrlCol rawUrls"><a href="TagClickViews.aspx?tagid=<%# Eval("TagMasterID")%>&mo=<%# chkMembersOnly.Checked %>&ds=<%= Query.DaySpan %>">
                                        <%# Eval("tag")%></a>
                            </td>
                            <td>
                                <%# Eval("ClickCount").ToString()%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlFreeLicenseRow" runat="server">
                    <tr>
                        <td colspan="2">
                            <SUEETIE:SueetieLocal ID="SueetieLocal1" Key="analytics_report_limit" LanguageFile="Licensing.xml"
                                runat="server" />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
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
