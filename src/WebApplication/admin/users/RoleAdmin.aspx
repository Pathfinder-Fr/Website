<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleAdmin.aspx.cs" Inherits="Sueetie.Web.RoleAdmin" %>

<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Membership Roles
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
                    Roles</h2>
        <div class="Activities">
            <div class="AdminTextTalk">
               
                <div class="AdminFormDescription">
                    <p>
                        Edit, Add and Delete roles. Indicate if the role serves as a group administrator
                        or user role. System Roles are locked and cannot be
                        deleted.<br />
                    </p>
                </div>
            </div>
        </div>
        <asp:GridView ID="UserRoles" runat="server" AutoGenerateColumns="False" DataKeyNames="RoleName"
            Width="400px" AllowSorting="True" ShowFooter="false" CssClass="fatgridviewMain"
            DataSourceID="ActivitiesDataSource" OnRowUpdating="UserRoles_OnRowUpdating">
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
                        Role Name
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("RoleName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Group Admin</HeaderTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsGroupAdminRole")%>.png' alt='<%#Eval("IsGroupAdminRole")%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkIsGroupAdmin" runat="server" Checked='<%# Eval("IsGroupAdminRole") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Group Users</HeaderTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsGroupUserRole")%>.png' alt='<%#Eval("IsGroupUserRole")%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkIsGroupUser" runat="server" Checked='<%# Eval("IsGroupUserRole") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField>
                    <HeaderTemplate>
                        Blog Owner</HeaderTemplate>
                    <ItemTemplate>
                        <img src='/images/shared/sueetie/<%#Eval("IsBlogOwnerRole")%>.png' alt='<%#Eval("IsBlogOwnerRole")%>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkIsBlogOwner" runat="server" Checked='<%# Eval("IsBlogOwnerRole") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="true" UpdateText="Update"
                    ControlStyle-CssClass="ActivityGridButton" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ActivitiesDataSource" runat="server" TypeName="Sueetie.Core.SueetieRoles"
            SelectMethod="GetSueetieRoleList" UpdateMethod="UpdateSueetieRole" DeleteMethod="DeleteSueetieRole">
            <UpdateParameters>
                <asp:Parameter Name="isGroupAdminRole" Type="Boolean" />
                <asp:Parameter Name="isGroupUserRole" Type="Boolean" />
                <asp:Parameter Name="isBlogOwnerRole" Type="Boolean" />
            </UpdateParameters>
        </asp:ObjectDataSource>
        <div class="AddActivitiesForm">
            <div class="UploadTitle">
                <strong>Add Role</strong></div>
            <table class="AddActivitiesFormTable" width="100%">
                <tr>
                    <td class="AddActivitiesLabel">
                        Role Name
                    </td>
                    <td class="aaMedium">
                        <asp:TextBox runat="server" ID="NewRole" MaxLength="50" ToolTip="Type the name of a new role you want to create." />
                    </td>
                </tr>
                <tr>
                    <td class="AddActivitiesLabel">
                        Is Group Admin Role
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsGroupAdminRole" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="AddActivitiesLabel">
                        Is Group User Role
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsGroupUserRole" runat="server" />
                    </td>
                </tr>
                   <tr>
                    <td class="AddActivitiesLabel">
                        Is Blog Owner Role
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsBlogOwnerRole" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="AddActivitiesLabel">
                    </td>
                    <td>
                        <div class="TextButtonBigArea">
       
                        <asp:Button ID="Button2" runat="server" OnClick="AddRole" Text="Add Role" ToolTip="Click to create new role."
                            CssClass="TextButtonBig" />
       </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
