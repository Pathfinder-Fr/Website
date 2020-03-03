<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.AdminUsersDefault" %>

<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks"
    TagPrefix="uc1" %>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Membership Management
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
 <div class="AdminFormArea">
        <h2>
           Membership</h2>
        <div class="AdminTextTalk">
                      <p>
                User Management Menu</p>
                <ul>
                    <li><a href="RegisteredUsers.aspx">All Registered Users</a></li>                
                    <li><a href="AddUsers.aspx">Add Users</a></li>
                    <li><a href="ApproveUsers.aspx">Approve New Users</a></li>
                    <li><a href="BannedIPs.aspx">Manage Banned IPs</a></li>
                    <li><a href="InactiveUsers.aspx">Inactive Users</a></li>
                    <li><a href="NewUsers.aspx">New Users</a></li>
                    <li><a href="LockedoutUsers.aspx">Locked-out Users</a></li>
                    <li><a href="OnlineUsers.aspx">Users Currently Online</a></li>
                    <li><a href="RoleAdmin.aspx">Manage Roles</a></li>
                    <li><a href="SearchUsers.aspx">Search Users</a></li>
                    <li><a href="UsersByRole.aspx">Display Users by Role</a></li>
                </ul>
        </div>
        </div>
</asp:content>
