<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Blogs.aspx.cs" Inherits="Sueetie.Web.AdminBlogs" %>
<%@ Register Src="../controls/adminBlogNavLinks.ascx" TagName="adminBlogNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Blog Management
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminBlogNavLinks ID="adminBlogNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Blog Management</h2>
        <div class="AdminTextTalk">
            <p>
                Here is where we will manage site blogs, blog users and handle other site-wide future blog administrative tasks.
                <ul>
                    <li><a href="blogedit.aspx">Edit Site Blogs</a></li>                
                    <li><a href="blogadmin.aspx">Add Site Blog Administrative Users</a></li>
                </ul>
        </div>
    </div>
</asp:Content>
