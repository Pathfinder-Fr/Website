<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieBlogsPage" %>

<%@ Import Namespace="Sueetie.Core" %>

<%@ Register Src="~/Controls/AggregateBlogPostList.ascx" TagName="AggregateBlogPostList"
    TagPrefix="uc" %>
<%@ Register Src="~/Controls/AggregateBlogList.ascx" TagName="AggregateBlogList"
    TagPrefix="uc" %>
    <%@ Import Namespace="Sueetie.Core" %>
        <asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
        </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
<div id="content">
<div id="contentInner">

    <div class="BodyTitleArea">
        All Blogs in this Community</div>
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
        Recent Blog Posts - All Blogs</div>
    <div class="aggregatemenu">
        <ul>
            <li><SUEETIE:SueetieLink ID="SueetieLink3" runat="server" LanguageFile="blogs.xml" SueetieUrlLinkTo="BlogsHome" TextKey="menutab_allblogs" /></li>
        </ul>
    </div>
    <div class="posts">
        <uc:AggregateBlogPostList ID="AggregateBlogPostList2" runat="server" IsRestricted="true"
            NumRecords="6" ViewName="AggregateBlogPostView" />
    </div>
    </div>
</div>
</asp:Content>