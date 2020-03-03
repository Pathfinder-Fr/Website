<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.PageRulesPage"
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
    Page Rules
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="6" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAnalyticsNavLinks ID="adminAnalyticsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="PageRules">
            <div id="PageRulesTop">
                <div class="AdminTextTalk">
                    <h2>
                        Page Rules</h2>
                    <div class="AdminFormDescription">
                        <p>
                            Enter url excerpt, recorded page url and title rules for the multiple page urls
                            identifying the same page, like "/members/login.aspx?returnurl=..." Logic for excerpt
                            matching: url.Contains(excerpt.) Also use rules to set Analytics Report page titles
                            on specific urls where duplicates may occur.<br />
                        </p>
                    </div>
                </div>
                <asp:GridView ID="PageRulesGridView" CssClass="fatgridviewMain agentTable" runat="server"
                    AutoGenerateColumns="False" DataKeyNames="PageRuleID" EmptyDataText="No records found."
                    AllowSorting="True" DataSourceID="PageRulesDataSource" OnRowUpdating="PageRulesGridView_OnRowUpdating"
                    OnRowDataBound="PageRulesGridView_OnRowDataBound">
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
                                <%#Eval("PageRuleID")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Url Excerpt">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUrlExcerpt" runat="server" Text='<%# Bind("UrlExcerpt") %>' CssClass="ApplicationLongInput"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("UrlExcerpt")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recorded Url">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUrlFinal" runat="server" Text='<%# Bind("UrlFinal") %>' CssClass="ActivityLongInput"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("UrlFinal")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Page Title">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPageTitle" runat="server" Text='<%# Bind("PageTitle") %>' CssClass="ApplicationLongInput"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("PageTitle")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comparison">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddIsEqual" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%# GetComparison(Convert.ToBoolean(Eval("IsEqual"))) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" DeleteText="Remove"
                            EditText="Edit" UpdateText="Update" ControlStyle-CssClass="ActivityGridButton"
                            ItemStyle-CssClass="gridCommandColumn" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="PageRulesDataSource" runat="server" TypeName="Sueetie.Core.SueetieLogs"
                    SelectMethod="GetPageRuleList" InsertMethod="CreatePageRule" DeleteMethod="DeletePageRule"
                    UpdateMethod="UpdatePageRule">
                    <UpdateParameters>
                        <asp:Parameter Name="UrlExcerpt" Type="String" />
                        <asp:Parameter Name="UrlFinal" Type="String" />
                        <asp:Parameter Name="PageTitle" Type="String" />
                        <asp:Parameter Name="IsEqual" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="UrlExcerpt" Type="String" />
                        <asp:Parameter Name="UrlFinal" Type="String" />
                        <asp:Parameter Name="PageTitle" Type="String" />
                        <asp:Parameter Name="IsEqual" Type="String" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <div id="PageRulesLeft">
                    <div class="AddActivitiesForm">
                        <div class="UploadTitle">
                            <strong>Add Page Rule</strong></div>
                        <asp:DetailsView ID="UpdatePageRulesDetailsView" runat="server" DataSourceID="PageRulesDataSource"
                            DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="PageRulesDetailsView_ItemInserting"
                            CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable"
                            OnDataBound="UpdatePageRulesDetailsView_OnDataBound">
                            <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                            <Fields>
                                <asp:BoundField DataField="urlExcerpt" ShowHeader="true" HeaderText="Url Excerpt"
                                    ItemStyle-CssClass="aaMedium" />
                                <asp:BoundField DataField="urlFinal" ShowHeader="true" HeaderText="Recorded Url"
                                    ItemStyle-CssClass="aaMedium" />
                                <asp:BoundField DataField="pageTitle" ShowHeader="true" HeaderText="Page Title" ItemStyle-CssClass="aaMedium" />
                                <asp:TemplateField HeaderText="Comparison Type">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddIsEqual" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                                    ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                            </Fields>
                            <InsertRowStyle Width="100%"></InsertRowStyle>
                        </asp:DetailsView>
                        <div id="ResultMessage" class="ResultsMessage FilteredUserMessage">
                            <asp:Label ID="lblResults" runat="server" />
                        </div>
                    </div>
                </div>
                <div id="PageRulesRight">
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
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="AdminUserFieldInfo">
                                        Enter url root from right to clean duplicate pages from the Analytics Logs. Ex:
                                        <strong>/blog/?tag</strong></div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnCleanPageLog" runat="server" OnClick="btnCleanPageLog_OnClick"
                                        Text="Clean" CssClass="ActivityGridButton" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="Div1" class="ResultsMessage PageRuleMessage">
                                        <asp:Label ID="lblCleanResultsMessage" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div id="PageRulesBottom">
                <table border="1" id="PageRuleTable" class="AgentsTable">
                    <thead>
                        <tr>
                            <th>
                                Page Url
                            </th>
                            <th>
                                Page Title
                            </th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="rptAgents" runat="server" EnableViewState="false">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <a href='<%# Eval("url").ToString()%>' target="_blank">
                                        <%# DataHelper.TruncateText(Eval("url").ToString(),80) %></a> <a href="FilteredUrlActivity.aspx?root=<%# DataHelper.TruncateTextNoElipse(Eval("url").ToString(),40) %>"
                                            target="_blank">
                                            <img src="/themes/lollipop/images/rightarrow.png" /></a>
                                </td>
                                <td>
                                    <%# Eval("PageTitle").ToString()%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <script type="text/javascript">

                    $('#PageRuleTable').dataTable({
                        "aaSorting": [[0, "asc"]],
                        "iDisplayLength": 25,
                        "aoColumns": [
						null,
                        null
					]
                    });

                </script>
            </div>
        </div>
</asp:Content>
