<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.AdminContentDefault" %>

<%@ Register Src="../controls/adminContentNavLinks.ascx" TagName="adminContentNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Sueetie Site Content Management
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="2" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
  <uc1:adminContentNavLinks ID="adminContentNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
 <div class="AdminFormArea">
        <h2>
           Content Management</h2>
        <div class="AdminTextTalk">
                      <p>
                Content Management Menu</p>
                <ul>
                    <li><a href="ContentParts.aspx">Content Parts</a></li>                
                    <li><a href="ActivityLogging.aspx">Logged Activity Categories</a></li>
                    <li><a href="ContentGroups.aspx">Content Pages</a></li>
                    <li><a href="SearchAddon.aspx">Sueetie Search</a></li>                
                    <li><a href="CalendarEdit.aspx">Sueetie Event Calendars</a></li>      
                </ul>
        </div>
        </div>
</asp:content>
