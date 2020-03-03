<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Analytics.Pages.FilterUserPage"
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
        <div class="FilteredUsers">
            <div class="AdminTextTalk">
                <h2>
                    Filter Users</h2>
                <div class="AdminFormDescription">
                    <p>
                        Enter administrative users you wish to filter from the Sueetie Analytics Request
                        Logs. The activity of these users will not be recorded.<br />
                    </p>
                </div>
            </div>
            <div id="FilteredUsersBody">
                <asp:GridView ID="FilteredUsersGridView" CssClass="fatgridviewMain agentTable" runat="server"
                    AutoGenerateColumns="False" DataKeyNames="FilteredUserID" EmptyDataText="No records found."
                    AllowSorting="True" DataSourceID="FilteredUsersDataSource" OnRowUpdating="FilteredUsersGridView_OnRowUpdating"
                    OnRowDataBound="FilteredUsersGridView_OnRowDataBound">
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
                                User ID
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("UserID")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filtered User">
                            <ItemTemplate>
                                <%#Eval("DisplayName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="false" DeleteText="Remove"
                            ControlStyle-CssClass="ActivityGridButton" ItemStyle-CssClass="gridCommandColumn" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="FilteredUsersDataSource" runat="server" TypeName="Sueetie.Analytics.AnalyticsCommon"
                    SelectMethod="GetFilteredUserList" InsertMethod="CreateFilteredUser" DeleteMethod="DeleteFilteredUser">
                    <UpdateParameters>
                    </UpdateParameters>
                    <InsertParameters>
                    </InsertParameters>
                </asp:ObjectDataSource>
                <div class="AddActivitiesForm">
                    <div class="UploadTitle">
                        <strong>Add User to Filter</strong></div>
                    <asp:DetailsView ID="UpdateFilteredUsersDetailsView" runat="server" DataSourceID="FilteredUsersDataSource"
                        DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="FilteredUsersDetailsView_ItemInserting"
                        CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable"
                        OnDataBound="UpdateFilteredUsersDetailsView_OnDataBound">
                        <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                        <Fields>
                            <asp:BoundField DataField="username" ShowHeader="true" HeaderText="Account Username"
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
            </div>
        </div>
    </div>
</asp:Content>
