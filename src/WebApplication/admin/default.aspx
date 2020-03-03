<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Sueetie.Web.AdminDefault"
    MasterPageFile="/themes/lollipop/masters/admin.master" %>

<%@ Register Src="controls/adminHomeNavLinks.ascx" TagName="adminHomeNavLinks" TagPrefix="uc1" %>
<%@ Register Src="~/controls/BlogRssPostList.ascx" TagName="RssPostList" TagPrefix="uc" %>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Sueetie Administration
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminHomeNavLinks ID="adminHomeNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">
    <div class="adminHomeVersionInfo">
        <asp:Label ID="lblVersion" runat="server" CssClass="BigFormText" />
    </div>
    <div class="AdminFormArea">
        <h2>
            Sueetie Administration Area</h2>
        <div class="AdminMenuLeft">
            <div class="AdminTextTalk">
                <ul>
                    <li><a href="/admin/users/default.aspx">Membership</a></li>
                    <li><a href="/admin/site/default.aspx">Site Settings and Configuration</a></li>
                    <li><a href="/admin/content/default.aspx">Content Management</a></li>
                    <li><a href="/admin/applications/default.aspx">Applications</a></li>
                    <li><a href="/admin/reports/default.aspx">Reports</a></li>
                    <li><a href="/admin/addonpack/default.aspx">Sueetie Addon Pack</a></li>
                    <li><a href="/admin/analytics/default.aspx">Sueetie Analytics</a></li>
                    <li><a href="/admin/marketplace/default.aspx">Marketplace</a></li>
                </ul>
            </div>
        </div>
        <div class="AdminRssRight">
            <div class="AdminSueetieNewsTitle">
                Latest Sueetie News</div>
            <div class="AdminSueetieNews">
                <uc:RssPostList ID="RssPostList1" runat="server" NumRecords="5" FeedUrl="http://sueetie.com/blog/syndication.axd" />
            </div>
        </div>
    </div>
</asp:Content>
