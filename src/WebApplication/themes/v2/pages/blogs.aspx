<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieBlogsPage"  EnableViewState="false" %>


<%@ Register Src="~/Controls/AggregateBlogPostList.ascx" TagName="AggregateBlogPostList"
    TagPrefix="uc" %>
<%@ Register Src="~/Controls/AggregateBlogList.ascx" TagName="AggregateBlogList"
    TagPrefix="uc" %>
    <%@ Import Namespace="Sueetie.Core" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
        <link rel="alternate" type="application/rss+xml" href="/util/rss/blogs.aspx" title="Recent <%= SiteSettings.Instance.SiteName %> Blog Posts (RSS)" />
        <link rel="alternate" type="application/atom+xml" href="/util/rss/blogs.aspx?type=ATOM" title="Recent <%= SiteSettings.Instance.SiteName %> Blog Posts (ATOM)" />
        </asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphSidePanel" runat="Server">
    <div id="BlogSideTitle">
        <SUEETIE:SueetieLink ID="SueetieLink2" runat="server" LanguageFile="blogs.xml" SueetieUrlLinkTo="BlogsHome" TextKey="sidebar_allblogs" />
        </div>
    <div class="sidebarcontent">
        <p>
            <%= Sueetie.Core.SueetieLocalizer.GetString("aggregate_sidebar_msg","blogs.xml") %>
        </p>
    </div>
    <div class="sidebarRssImage">
    <a href="/util/rss/blogs.aspx"><img src="/themes/lollipop/images/blogs/rsssleek.png" /></a>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="BodyTitleArea">
    <%= Sueetie.Core.SueetieLocalizer.GetString("aggregate_all_blogs","blogs.xml") %></div>
    <div class="aggregatemenu">
        <ul>
            <li><SUEETIE:SueetieLink ID="SueetieLink1" runat="server" LanguageFile="blogs.xml" SueetieUrlLinkTo="BlogsHome" TextKey="menutab_allblogs" /></li>
        </ul>
    </div>
    <div class="blogs">
        <uc:AggregateBlogList ID="AggregateBlogList1" runat="server" IsRestricted="true"
            ViewName="AggregateBlogView" />
    </div>
    <div class="BodyTitleArea">
    <%= Sueetie.Core.SueetieLocalizer.GetString("aggregate_all_recentposts", "blogs.xml")%>
        </div>
    <div class="aggregatemenu">
        <ul>
            <li><SUEETIE:SueetieLink ID="SueetieLink3" runat="server" LanguageFile="blogs.xml" SueetieUrlLinkTo="BlogsHome" TextKey="menutab_allblogs" /></li>
        </ul>
    </div>
    <div class="posts">
        <uc:AggregateBlogPostList ID="AggregateBlogPostList2" runat="server" IsRestricted="true"
            NumRecords="6" ViewName="AggregateBlogPostView" />
    </div>
</asp:Content>