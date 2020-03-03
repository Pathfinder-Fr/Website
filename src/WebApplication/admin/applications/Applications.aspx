<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Applications.aspx.cs" Inherits="Sueetie.Web.AdminApplications"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="../controls/adminApplicationNavLinks.ascx" TagName="adminApplicationNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Site Applications
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminApplicationNavLinks ID="adminApplicationNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    Site Applications</h2>
                <div class="AdminFormDescription">
                    <p>
                        Edit, Add and Delete applications. This does not perform any physical operations on the applications
                        listed, but used in site contextual awareness and integration. System applications (blog, forum, etc) cannot be deleted, but can be renamed. It is prudent 
                        to deactivate any applications not in use.<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="ActivitiesGridView" CssClass="fatgridviewMain" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ApplicationID" EmptyDataText="No records found." AllowSorting="True"
            DataSourceID="ActivitiesDataSource" OnRowUpdating="ActivitiesGridView_OnRowUpdating"
            OnRowDataBound="ActivitiesGridView_OnRowDataBound">
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
                        ID
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#Eval("ApplicationID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AppKey *">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUserLogCategoryCode" runat="server" Text='<%# Bind("ApplicationKey") %>'
                            CssClass="ApplicationShortInput"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("ApplicationKey")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Application Description">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUserLogCategoryDescription" runat="server" Text='<%# Bind("Description") %>'
                            CssClass="ApplicationLongInput"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddGroups" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("GroupKey")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AppType">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddApplicationTypes" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%#Eval("ApplicationName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Active">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkRows" runat="server" ToolTip="Clear to not display" Checked='<%#Eval("IsActive")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsActive")%>.png' alt='<%#Eval("IsActive")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieCommon"
            SelectMethod="GetSueetieApplicationsList" UpdateMethod="UpdateSueetieApplication"
            InsertMethod="CreateSueetieApplication" DeleteMethod="DeleteSueetieApplication">
            <UpdateParameters>
                <asp:Parameter Name="appKey" Type="String" />
                <asp:Parameter Name="appDescription" Type="String" />
                <asp:Parameter Name="isActive" Type="Boolean" />
                <asp:Parameter Name="groupId" Type="Int32" />
                <asp:Parameter Name="appTypeId" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
            </InsertParameters>
        </asp:ObjectDataSource>
        <div class="AddActivitiesForm">
            <div class="UploadTitle">
                <strong>Add Application</strong> (All fields are required)</div>
            <asp:DetailsView ID="UpdateActivitiesDetailsView" runat="server" DataSourceID="ActivitiesDataSource"
                DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="ActivitiesDetailsView_ItemInserting"
                CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable"
                OnDataBound="UpdateActivitiesDetailsView_OnDataBound">
                <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                <Fields>
                    <asp:BoundField DataField="ApplicationID" ShowHeader="true" HeaderText="AppID" ItemStyle-CssClass="aaShort" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="AdminUserFieldInfo">
                                <strong>Application IDs 1-100 are reserved for Sueetie Framework.</strong> To avoid conflicts, start with 101 for custom applications.</div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ApplicationKey" ShowHeader="true" HeaderText="AppKey *"
                        ItemStyle-CssClass="aaMedium" />
                    <asp:BoundField DataField="Description" ShowHeader="true" HeaderText="Application description"
                        ItemStyle-CssClass="aaLong" />
                    <asp:TemplateField HeaderText="Application Type">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddApplicationDetailsTypes" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Group">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddGroupsDetails" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                        ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                </Fields>
                <InsertRowStyle Width="100%"></InsertRowStyle>
            </asp:DetailsView>
        </div>
        <div class="AdminFooterInfo">
            <strong>* AppKey Note:</strong> The application key must match the app's folder
            name. Ex: http://site.com/<strong>blog</strong> or http://site.com/groups/demo/<strong>forum</strong></div>
    </div>
</asp:Content>
