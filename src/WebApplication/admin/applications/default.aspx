<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.AdminApplicationsDefault" %>
<%@ Register Src="../controls/adminApplicationNavLinks.ascx" TagName="adminApplicationNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Applications
</asp:content>
<asp:Content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminApplicationNavLinks ID="adminApplicationNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
 <div class="AdminFormArea">
       <h2>
           Sueetie Application Management</h2>
        <div class="AdminTextTalk">
                <p>
                Applications Management Menu</p>
                <ul>
                    <li><a href="Applications.aspx">Sueetie Site Applications</a></li>     
                    <%--<li><a href="Groups.aspx">Manage Groups</a></li>    --%>      
                    <li><a href="MediaAlbums.aspx">Manage Media Album Types</a></li>       
                    <li><a href="BlogEdit.aspx">Manage Site Blogs</a></li>      
                    <li><a href="BlogAdmin.aspx">Add Blog Administrator User</a></li>     
                    <li><a href="WikiUser.aspx">Add Wiki User Account</a></li>                          
                </ul>
        </div>
        </div>
</asp:content>
