<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.ImportUpdateIpPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks" TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
   Update IP Ranges from CIDR Import Files
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphSidePanel">
    <uc3:adminSideNavigation ID="adminSideNavigation1" runat="server" ActiveAccordionPanel="5" />
</asp:Content>

<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminAddonPackNavLinks ID="adminAddonPackNavLinks1" runat="server" />
</asp:Content>


<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="AdminFormArea">
        <div class="AdminTextTalk">
            <h2>
                Import/Update Blocked Country IPs From CIDR Files</h2>
            <div class="AdminFormDescription">
                <p>
                    Here you willl be updating the IP ranges for your blocked countries from refreshed IP import files located in /util/ips (or from those installed with Sueetie if your first time.)  Processing will occur on a background thread and the results displayed in the Sueetie Event Log. Read the <a href='http://sueetie.com/wiki/howtoblockcountry.ashx'>Blocking Country Access How-To</a> in the Sueetie Wiki for details.
                </p>
            </div>
        </div>
        <div class="TextButtonBigArea">
            <asp:Button ID="btnUpdate" runat="server" Text=" Update " CssClass="TextButtonBig"
                OnClick="btnUpdate_OnClick"  CausesValidation="false" />
        </div>
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div>
    </div>
</asp:Content>
