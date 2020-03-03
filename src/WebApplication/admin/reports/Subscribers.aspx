<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subscribers.aspx.cs" Inherits="Sueetie.Web.Subscribers" %>

<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Register Src="../controls/adminReportsNavLinks.ascx" TagName="adminReportsNavLinks"
    TagPrefix="uc1" %>
<%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation"
    TagPrefix="uc3" %>
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Newsletter Subscribers
</asp:content>
<asp:content id="Content2" runat="server" contentplaceholderid="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="4" />
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminReportsNavLinks ID="adminReportsNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
      <div class="Activities">
            <div class="AdminTextTalk">
                <h2>
                    <%= Sueetie.Core.SiteSettings.Instance.SiteName %> Newsletter Subscribers</h2>
                <div class="AdminFormDescription">
                    <p>
                       The Sueetie Newsletter Subscriber Module consists of generating a list of email addresses.  It does not currently have a newsletter generator component.  You have <strong><asp:Label ID="lblSubscriberCount" runat="server" /> active subscribers.</strong>  Their email addresses are listed below by member display name.<br />
                    </p>
                    
                </div>

            </div>
            <h3>Subscriber Email Addresses</h3>
            <div class="SubscriberList">
                <asp:Label runat="server" id="lblEmailAddresses" />
            </div>
            <h3>Subscriber Names</h3>
             <div class="SubscriberList">
                <asp:Label runat="server" id="lblDisplayNames" />
            </div>
        </div>
</div>
</asp:content>
