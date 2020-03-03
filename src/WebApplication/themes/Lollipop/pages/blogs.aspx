<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieBlogsPage"
    EnableViewState="false" %>

<%@ Register Src="~/Controls/AggregateBlogPostList.ascx" TagName="AggregateBlogPostList"
    TagPrefix="uc" %>
<%@ Register Src="~/Controls/AggregateBlogList.ascx" TagName="AggregateBlogList"
    TagPrefix="uc" %>
<%@ Import Namespace="Sueetie.Core" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="server">
        <link rel="alternate" type="application/rss+xml" href="/util/rss/blogs.aspx" title="Recent <%= SiteSettings.Instance.SiteName %> Blog Posts (RSS)" />
        <link rel="alternate" type="application/atom+xml" href="/util/rss/blogs.aspx?type=ATOM" title="Recent <%= SiteSettings.Instance.SiteName %> Blog Posts (ATOM)" />
        </asp:content>
<asp:content id="Content4" contentplaceholderid="cphSidePanel" runat="Server">
    <div id="BlogSideTitle">
        <SUEETIE:SueetieLink ID="SueetieLink2" runat="server" LanguageFile="blogs.xml" SueetieUrlLinkTo="BlogsHome" TextKey="sidebar_allblogs" />
        </div>
    <div class="sidebarcontent">
        <p>
        <SUEETIE:SueetieLocal runat="server" Key="aggregate_sidebar_msg" LanguageFile="blogs.xml" />
        </p>
    </div>
    <div class="sidebarRssImage">
    <a href="/util/rss/blogs.aspx"><img src="/themes/lollipop/images/blogs/rsssleek.png" /></a>
    </div>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphBody" runat="Server">
    <div class="BodyTitleArea">
        <SUEETIE:SueetieLocal ID="SueetieLocal1" runat="server" Key="aggregate_all_blogs" LanguageFile="blogs.xml" />
</div>
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
     <SUEETIE:SueetieLocal ID="SueetieLocal2" runat="server" Key="aggregate_all_recentposts" LanguageFile="blogs.xml" />

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
</asp:content>
