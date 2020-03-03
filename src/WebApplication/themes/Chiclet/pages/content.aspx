<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieCmsPage"
    ValidateRequest="false" %>

<%@ Register Src="~/controls/ContentPageList.ascx" TagName="ContentPageList" TagPrefix="uc" %>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSidePanel" runat="Server">
    <div id="ContentSideTop">
        <SUEETIE:ContentPart ID="ContentPart1" ContentName="cmsSideTop" runat="server" Roles="SueetieAdministrator" />
    </div>
    <div class="ContentSidebarPageList">
    <div class="ContentSidebarListTitle">
    <SUEETIE:SueetieLocal Key="content_pagelist" runat="server" />
    </div>
        <uc:ContentPageList ID="ContentPageList1" runat="server" ContentGroupID="1"  ViewName="ContentPageSidebarView"  SortBy="ListDisplayOrder" />
    </div>
    <div id="ContentSideBottom">
        <SUEETIE:ContentPagePart ID="ContentPagePart1" ContentName="sidebottom" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div id="ContentBodyArea">
        <SUEETIE:ContentPagePart ID="ContentPagePart2" ContentName="body" runat="server"  />
    </div>
</asp:Content>
