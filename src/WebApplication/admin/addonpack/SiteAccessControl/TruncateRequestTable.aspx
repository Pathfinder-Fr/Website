<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.AddonPack.Pages.TruncateLogPage" MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="/admin/controls/adminAddonPackNavLinks.ascx" TagName="adminAddonPackNavLinks" TagPrefix="uc1" %>
<%@ Register Src="/admin/controls/adminSideNavigation.ascx" TagName="adminSideNavigation" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server" />

<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
  Clear User Agent RequestLog Table
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
                Clear User Agent RequestLog Table</h2>
            <div class="AdminFormDescription">
                <p>
                    Use this support process to truncate the User Agent RequestLog Table.  The User Agent table is separate from other site access data so that it can be periodically purged to keep the SQL database small as possible.  
                </p>
            </div>
        </div>
        <div class="TextButtonBigArea">
            <asp:Button ID="btnClear" runat="server" Text=" Clear " CssClass="TextButtonBig"
                OnClick="btnClear_OnClick"  CausesValidation="false" />
        </div>
        <br />
        <br />
        <div id="ResultMessage" class="ResultsMessage">
            <asp:Label ID="lblResults" runat="server" />
        </div>
    </div>
</asp:Content>
