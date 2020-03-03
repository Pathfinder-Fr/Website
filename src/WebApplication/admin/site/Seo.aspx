<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seo.aspx.cs" Inherits="Sueetie.Web.AdminSeo" ValidateRequest="false" %>

<%@ Register Src="../controls/adminSiteSettingsNavLinks.ascx" TagName="adminSiteSettingsNavLinks"
    TagPrefix="uc1" %>
   <%@ Register Src="../controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>
      
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    SEO Settings
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="1" />
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminSiteSettingsNavLinks ID="adminSiteSettingsNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Header Metatags</h2>
        <div class="AdminFormDescription">
            Add custom code to the HTML head section. An example would be Google Webmaster Tools
            site verification code.</div>
        <asp:TextBox runat="server" ID="txtHtmlHeader" TextMode="multiLine"  />
        <h2>
            Tracking script</h2>
        <div class="AdminFormDescription">
            Enter the JavaScript code from i.e. Google Analytics or other tracking services
            to be added at the bottom of each page. Remember to add the &lt;script&gt; tags.</div>
        <asp:TextBox runat="server" ID="txtTrackingScript" TextMode="multiLine" Height="120px" />
        <div class="TextButtonBigArea">
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="TextButtonBig"
                OnClick="Submit_Click" /> <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage" />
        </div>
</asp:Content>
