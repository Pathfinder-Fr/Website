<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.BlogRssPage"
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
    Blog Rss
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
                        Blog Rss Subscription and Reach</div>
                    <div class="SelectArea">
                        Date Range:
                        <asp:DropDownList ID="ddDaySpans" runat="server" OnSelectedIndexChanged="UpdateQuery"
                            AutoPostBack="true" />
                    </div>
                </div>
                <div class="HeaderBox">
                    Blog Subscription and Reach statistics for Last <strong>
                        <%= DisplayDateRange() %>. </strong>Subscription Requests: <strong>
                            <%= TotalViews %>.</strong> Total Reach: <strong>
                                <%= TotalReach %>.</strong>
                </div>
                <div class="TableTop">
                    <div class="TableTopLeft">
                        <asp:DropDownList ID="ddApplicationID" runat="server" OnSelectedIndexChanged="UpdateQuery"
                            AutoPostBack="true" />
                    </div>
                    <div class="TableTopRight BigLink">
                        <a href='BlogRssViews.aspx?appid=<%= Query.ApplicationID %>?ds=<%= Query.DaySpan %>'>
                            View Reach Report</a>
                    </div>
                </div>
            </div>
            <table border="1" id="PageTable" class="AgentsTable">
                <thead>
                    <tr>
                        <th>
                            Date
                        </th>
                        <th>
                            Subscribers
                        </th>
                        <th>
                            Reach
                        </th>
                    </tr>
                </thead>
                <asp:Repeater ID="rptIpLog" runat="server" EnableViewState="false">
                    <ItemTemplate>
                        <tr>
                            <td class="lightUrlCol rawUrls">
                                <%# string.Format("{0:D}", Convert.ToDateTime(Eval("rssdatetime").ToString()))%>
                            </td>
                            <td>
                                <%# Eval("RssCount").ToString()%>
                            </td>
                            <td>
                                <%# Eval("Reach").ToString()%>
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
        <div class="blogRssNotes">
            <strong>Note: </strong>Subscriber count indicates # unique feed requests for all
            clients. Reach indicates number of individual post views in client newsreader.
        </div>
        <script type="text/javascript">

            $('#PageTable').dataTable({
                "aaSorting": [[0, "desc"]],
                "iDisplayLength": 25,
                "aoColumns": [
						null,
						null,
                        null
					]
            });

        </script>
    </div>
</asp:Content>
