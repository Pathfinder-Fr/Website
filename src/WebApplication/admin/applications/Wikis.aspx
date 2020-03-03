<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wikis.aspx.cs" Inherits="Sueetie.Web.AdminWikis" %>

<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
<%@ Register Src="../controls/adminWikiNavLinks.ascx" TagName="adminWikiNavLinks" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Wiki Management
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="3" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminWikiNavLinks ID="adminWikiNavLinks1" runat="server" />
</asp:Content>

<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Wiki Management</h2>
        <div class="AdminTextTalk">
            <p>
                Here is where we will manage site wikis, wiki users and handle other site-wide future wiki administrative tasks.
                <ul>
                    <li><a href="wikiuser.aspx">Create Wiki User Accounts</a></li>
                </ul>
        </div>
    </div>
</asp:Content>
