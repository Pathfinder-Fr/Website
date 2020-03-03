<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Media.aspx.cs" Inherits="Sueetie.Web.AdminMedia" %>

<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<%@ Register Src="../controls/adminMediaNavLinks.ascx" TagName="adminMediaNavLinks"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Media Management
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminMediaNavLinks ID="adminMediaNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Media Management</h2>
        <div class="AdminTextTalk">
            <p>
                Here is where we will manage Sueetie-specific aspects of Media and Document Libraries.
                <ul>
                    <li><a href="mediagalleries.aspx">Media Galleries</a></li>      
                    <li><a href="mediaalbums.aspx">Media Albums</a></li>                
                </ul>
        </div>
    </div>
</asp:content>
