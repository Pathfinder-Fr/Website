<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.AdminSiteDefault" %>
<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks" TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Site Settings
</asp:content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:Content>

<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:content>

<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
 <div class="AdminFormArea">
        <h2>
           Site Settings</h2>
        <div class="AdminTextTalk">
                        <p>
                Site Administration Menu</p>
                <ul>
                    <li><a href="GeneralSettings.aspx">General Site Settings</a></li>                
                    <li><a href="EmailSettings.aspx">Email Configuration</a></li>
                    <li><a href="Seo.aspx">SEO Analytics and Tracking Configuration</a></li>
                    <li><a href="Themes.aspx">Change Site Theme</a></li>
                    <li><a href="RestartApp.aspx">Restart Sueetie</a></li>
                    <li><a href="Licenses.aspx">Manage Sueetie Licenses</a></li>
                </ul>
        </div>
        </div>
</asp:content>
