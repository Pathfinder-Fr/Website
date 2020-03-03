<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieDefaultPage" ValidateRequest="false" Title="Communauté francophone Pathfinder" EnableViewState="false" %>

<%@ Import Namespace="Sueetie.Core" %>
<%@ Register Src="~/controls/BlogPostList.ascx" TagName="BlogPostList" TagPrefix="uc" %>
<%@ Register Src="~/controls/ForumTopicList.ascx" TagName="ForumTopicList" TagPrefix="uc" %>
<%@ Register Src="~/controls/WikiPageList.ascx" TagName="WikiPageList" TagPrefix="uc" %>
<%@ Register Src="~/controls/UserLogActivityList.ascx" TagName="UserLogActivityList" TagPrefix="uc" %>

<script runat="server">
    
    protected override void OnLoad(EventArgs e)
    {
        if (!Page.IsCallback && !Page.IsPostBack)
        {
            AddMetaTag("description", SueetieLocalizer.GetString("metatag_description"));
            AddMetaTag("keywords", SueetieLocalizer.GetString("metatag_keywords"));
            AddMetaTag("author", SueetieLocalizer.GetString("metatag_author"));
            AddMetaTag("verify-v1", SueetieLocalizer.GetString("metatag_verify-v1"));
        }
        base.OnLoad(e);
    }
    
</script>

<asp:content id="Content3" contentplaceholderid="cphBody" runat="Server">
    <div class="HomeBodyContent">
        <SUEETIE:ContentPart ID="ContentPart2" runat="server" ContentID="HomeContent" ContentName="HomeContent" />
    </div>
    <table>
        <tr>
            <td width="50%">
                <h1>Dernières actualités du Blog</h1>
                <uc:blogpostlist ID="Blogpostlist2" runat="server" isrestricted="true" numrecords="5" applicationid="1" viewname="RecentBlogPostView"  />
            </td>
            <td>
                <h1>En direct de Twitter</h1>
                <a class="twitter-timeline" href="https://twitter.com/PathfinderFR" data-widget-id="352895078802538496" data-chrome="nofooter noborders" data-tweet-limit="5">Tweets de @PathfinderFR</a>
<script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + "://platform.twitter.com/widgets.js"; fjs.parentNode.insertBefore(js, fjs); } }(document, "script", "twitter-wjs");</script>
                <h1>Chat du site</h1>
		        <iframe src="https://discordapp.com/widget?id=106857891419443200&theme=light&username=<%=System.Net.WebUtility.UrlEncode(this.User.Identity.Name)%>" width="405" height="300" allowtransparency="true" frameborder="0"></iframe>
            </td>
        </tr>
    </table>
</asp:content>

<asp:content contentplaceholderid="cphSidePanel" runat="server">
    <sueetie:ContentPart ID="ContentPart1" runat="server" ContentID="HomeSideContent" ContentName="HomeSideContent" />
                            <h4>Activité récente</h4>
                            <uc:UserLogActivityList ID="UserLogActivityList2" runat="server" IsRestricted="true" GroupID="0" NumRecords="15" CacheMinutes="2" />
</asp:content>
