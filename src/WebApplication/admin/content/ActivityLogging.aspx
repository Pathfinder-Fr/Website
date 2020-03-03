<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityLogging.aspx.cs" Inherits="Sueetie.Web.AdminActivityLogging" %>

<%@ Register src="../controls/adminContentNavLinks.ascx" tagname="adminContentNavLinks" tagprefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
   Logged Activity Categories
</asp:Content>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminContentNavLinks ID="adminContentNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
    <div class="Activities"
        <div class="AdminTextTalk">
            <h2>
                Logged Activity Categories</h2>
            <div class="AdminFormDescription">
                <p>
                    Denote the Logged Activity Categories you want to display to users or make available in the Site Activities RSS Feed. 
                </p>
            </div>
        </div>
        </div>
        <asp:GridView ID="ActivitiesGridView" CssClass="fatgridviewMain" runat="server" AutoGenerateColumns="False"
            DataKeyNames="UserLogCategoryID" EmptyDataText="No records found." AllowSorting="True"
            DataSourceID="ActivitiesDataSource" OnRowUpdating="ActivitiesGridView_OnRowUpdating">
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
                        <%#Eval("UserLogCategoryID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Activity Category">
<%--                    <EditItemTemplate>
                        <asp:TextBox ID="txtUserLogCategoryCode" runat="server" Text='<%# Bind("UserLogCategoryCode") %>' CssClass="ActivityShortInput"></asp:TextBox>
                    </EditItemTemplate>--%>
                    <ItemTemplate>
                        <%#Eval("UserLogCategoryCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Activity Category Description">
<%--                    <EditItemTemplate>
                        <asp:TextBox ID="txtUserLogCategoryDescription" runat="server" Text='<%# Bind("UserLogCategoryDescription") %>' CssClass="ActivityLongInput"></asp:TextBox>
                    </EditItemTemplate>--%>
                    <ItemTemplate>
                        <%#Eval("UserLogCategoryDescription")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Display">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkRows" runat="server" ToolTip="Clear to not display" Checked='<%#Eval("IsDisplayed")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsDisplayed")%>.png' alt='<%#Eval("IsDisplayed")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="RSS">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkRSS" runat="server" ToolTip="Clear to not include in RSS Feed" Checked='<%#Eval("IsSyndicated")%>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsSyndicated")%>.png' alt='<%#Eval("IsSyndicated")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="False" ShowEditButton="true" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieCommon"
            SelectMethod="GetUserLogCategoryList" UpdateMethod="UpdateUserLogCategory" InsertMethod="CreateUserLogCategory" DeleteMethod="DeleteUserLogCategory" >
            <UpdateParameters>
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="catId" Type="Int32" />
                <asp:Parameter Name="catCode" Type="String" />
                <asp:Parameter Name="catDesc" Type="String" />
                <asp:Parameter Name="isDisplayed" Type="Boolean" />
                <asp:Parameter Name="isSyndicated" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
<%--        <div class="AddActivitiesForm">
            <div class="UploadTitle">
                <strong>Add User Activity</strong> (All fields are required)</div>
            <asp:DetailsView ID="UpdateActivitiesDetailsView" runat="server" DataSourceID="ActivitiesDataSource"
                DefaultMode="Insert" AutoGenerateRows="False" OnItemInserting="ActivitiesDetailsView_ItemInserting"
                CellPadding="2" GridLines="None" CellSpacing="2" CssClass="AddActivitiesFormTable">
                <FieldHeaderStyle CssClass="AddActivitiesLabel" />
                <Fields>
                    <asp:BoundField DataField="UserLogCategoryID" ShowHeader="true" HeaderText="Activity ID" ItemStyle-CssClass="aaShort" />
                    <asp:BoundField DataField="UserLogCategoryCode" ShowHeader="true" HeaderText="Activity code" ItemStyle-CssClass="aaMedium" />
                    <asp:BoundField DataField="UserLogCategoryDescription" ShowHeader="true" HeaderText="Activity description"  ItemStyle-CssClass="aaLong" />
                    <asp:CheckBoxField DataField="IsDisplayed" ShowHeader="true" HeaderText="Display on site activity pages" ItemStyle-CssClass="aaCheckbox" />
                    <asp:CheckBoxField DataField="IsSyndicated" ShowHeader="true" HeaderText="Include in Activity RSS Feed" ItemStyle-CssClass="aaCheckbox"/>
                    <asp:CommandField ShowInsertButton="True" InsertText="Add" ButtonType="Button" ShowCancelButton="False"
                        ControlStyle-CssClass="ActivityGridButton"></asp:CommandField>
                </Fields>
                <InsertRowStyle Width="100%"></InsertRowStyle>
            </asp:DetailsView>
        </div>--%>
    </div>
</asp:Content>
