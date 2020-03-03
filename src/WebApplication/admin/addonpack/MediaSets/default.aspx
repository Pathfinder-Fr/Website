<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.MediaSetDefaultPage" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Media Set Menu
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Media Set Menu</h2>
            <div class="AdminFormDescription">
                <p>
                    With Sueetie Media Sets you can create sets of images and other media from your media galleries, assign them to groups and associate them with site content like wiki pages, blog posts, CMS pages, etc. 
                </p>
            </div>
        </div>
            <div class="MainMenuContents">
                <div class="AdminTextTalk">
                    <div class="MainMenuList">
                    <div class="AdminULHeader">
                        Media Sets and Groups</div>
                    <ul>
                        <li><a href="MediaSetEdit.aspx">Manage Media Sets</a></li>
                        <li><a href="MediaSetGroupEdit.aspx">Manage Media Set Groups</a></li>
                    </ul>
                    </div>
        </div>
 </div>
</asp:content>
