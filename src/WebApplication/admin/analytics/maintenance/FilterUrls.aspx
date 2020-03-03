<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.FilterUrlPage"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>
    <%@ Import Namespace="Sueetie.Core" %>
<%@ Register Src="/admin/controls/adminAnalyticsNavLinks.ascx" TagName="adminAnalyticsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
    <script src="/scripts/jquery.dataTables.js" type="text/javascript"></script>
    <link href="/themes/lollipop/style/datatables.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Filter Users
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="FilteredUrls">
            <div class="AdminTextTalk">
                <h2>
                    Filter Urls</h2>
                <div class="AdminFormDescription">
                    <p>
                        Enter url excerpts to filter out anonymous request logging. Examples: "?tag," "blog/author",
                        "history.aspx?page", etc.  To prevent a page from being logged by all users, add it to /util/config/nolog.config.<br />
                    </p>
                </div>
            </div>
            <div id="FilteredUrlsLeft">
                <asp:GridView ID="FilteredUrlsGridView" CssClass="fatgridviewMain agentTable" runat="server"
                    AutoGenerateColumns="False" DataKeyNames="FilteredUrlID" EmptyDataText="No records found."
                    AllowSorting="True" DataSourceID="FilteredUrlsDataSource" >
                    <RowStyle CssClass="gridRowStyle" />
                    <SelectedRowStyle CssClass="gridrowSelectedBG" />
                    <HeaderStyle CssClass="gridheaderBG" />
                    <AlternatingRowStyle CssClass="gridAlternateRowStyle" />
                    <PagerStyle CssClass="membersGridViewPager2" BorderWidth="0px" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle CssClass="gridheaderBG" Width="1px" />
                            <ItemStyle CssClass="gridheaderBG" Width="1px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Url ID
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("FilteredUrlID")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filtered Url Excerpt">
                            <ItemTemplate>
                                <%#Eval("UrlExcerpt")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="false" DeleteText="Remove"
                            ControlStyle-CssClass="ActivityGridButton" ItemStyle-CssClass="gridCommandColumn" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="FilteredUrlsDataSource" runat="server" TypeName="Sueetie.Analytics.AnalyticsCommon"
                    SelectMethod="GetFilteredUrlList" InsertMethod="CreateFilteredUrl" DeleteMethod="DeleteFilteredUrl">
                    <UpdateParameters>
                    </UpdateParameters>
                    <InsertParameters>
                    </InsertParameters>
                </asp:ObjectDataSource>
                <div class="AddActivitiesForm">
                    <div class="UploadTitle">
                        <strong>Add Url Excerpt to Filter</strong></div>
                    <asp:DetailsView ID="UpdateFilteredUrlsDetailsView" runat="server" DataSourceID="FilteredUrlsDataSource"
                        DefaultMode="Insert" AutoGenerateRows="False" 
                        CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable">
                        <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                        <Fields>
                            <asp:BoundField DataField="urlExcerpt" ShowHeader="true" HeaderText="Url Excerpt"
                                ItemStyle-CssClass="aaMedium" />
                            <asp:CommandField ShowInsertButton="True" InsertText="Filter" ButtonType="Button"
                                ShowCancelButton="False" ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                        </Fields>
                        <InsertRowStyle Width="100%"></InsertRowStyle>
                    </asp:DetailsView>
                    <div id="ResultMessage" class="ResultsMessage FilteredUserMessage">
                        <asp:Label ID="lblResults" runat="server" />
                    </div>
                </div>
                <div class="AddActivitiesForm">
                    <div class="UploadTitle">
                        <strong>Clean Page Log</strong></div>
                    <table class="AddActivitiesFormTable">
                        <tr>
                            <td class="AddActivitiesLabel">
                                Enter Url Root 
                            </td>
                            <td class="aaMedium">
                                <asp:TextBox ID="txtUrlRoot" runat="server" CssClass="aaMedium" />
                            </td>
                        </tr>
                        <tr><td></td><td>
                        <div class="AdminUserFieldInfo">Enter url root from right to clean page log of unwanted urls.  Ex: <strong>/blog/?tag</strong></div>
                        </td></tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnCleanPageLog" runat="server" OnClick="btnCleanPageLog_OnClick"
                                    Text="Clean" CssClass="ActivityGridButton" />
                            </td>
                        </tr>
                        <tr><td colspan="2">
                        <div id="Div1" class="ResultsMessage FilteredUrlMessage">
                        <asp:Label ID="lblCleanResultsMessage" runat="server" />
                    </div>
                        </td></tr>
                    </table>
                </div>
            </div>
            <div id="FilteredUrlsRight">
                <table border="1" id="FilteredUrlTable" class="AgentsTable">
                    <thead>
                        <tr>
                            <th>
                                Page Url
                            </th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="rptAgents" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <tr>
                            <td class="lightUrlCol rawUrls">
                                <a href='<%# Eval("url").ToString()%>' target="_blank">
                                <%# DataHelper.TruncateText(Eval("url").ToString(),80) %></a> 
                                <a href="FilteredUrlActivity.aspx?root=<%# MakeUrlQueryString(Eval("url").ToString()) %>" target="_blank">
                                <img src="/themes/lollipop/images/rightarrow.png" /></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <script type="text/javascript">

                    $('#FilteredUrlTable').dataTable({
                        "aaSorting": [[0, "asc"]],
                        "iDisplayLength": 25,
                        "aoColumns": [
						null
					]
                    });

                </script>
            </div>
        </div>
    </div>
</asp:Content>
