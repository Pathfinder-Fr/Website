<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieCmsPage"
    ValidateRequest="false" %>

<%@ Register Src="~/controls/ContentPageList.ascx" TagName="ContentPageList" TagPrefix="uc" %>
<%@ Import Namespace="Sueetie.Core" %>
<asp:content id="Content4" contentplaceholderid="cphSidePanel" runat="Server">

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
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphBody" runat="Server">
    <div id="ContentBodyArea">
        <SUEETIE:ContentPagePart ID="ContentPagePart2" ContentName="body" runat="server"  />
        <div class="tagsArea">
        <SUEETIE:SueetieLocal Key="tags_label" runat="server" CssClass="tagsLabel" Anchor="span" /><SUEETIE:TagControl runat="server" ID="TagControl1"  />
        </div>
        <div class="calendarArea">
        <SUEETIE:CalendarControl runat="server" />
        </div>
    </div>
</asp:content>
