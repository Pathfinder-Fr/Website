<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieDefaultPage"
    ValidateRequest="false" EnableViewState="false" %>

<%@ Import Namespace="Sueetie.Core" %>
<%@ Register TagPrefix="uc" TagName="UserMenu" Src="~/themes/Lollipop/menus/UserMenu.ascx" %>
<%@ Register TagPrefix="uc" TagName="HomeBodyMenu" Src="~/themes/Lollipop/menus/HomeBodyMenu.ascx" %>
<%@ Register Src="~/controls/BlogPostList.ascx" TagName="BlogPostList" TagPrefix="uc" %>
<%@ Register Src="~/controls/ForumTopicList.ascx" TagName="ForumTopicList" TagPrefix="uc" %>
<%@ Register Src="~/controls/WikiPageList.ascx" TagName="WikiPageList" TagPrefix="uc" %>
<%@ Register Src="~/controls/UserLogActivityList.ascx" TagName="UserLogActivityList"
    TagPrefix="uc" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts />
        <Services>
            <asp:ServiceReference Path="~/util/services/SueetieService.svc" />
        </Services>
    </asp:ScriptManager>
    
                    <div class="HomeTabArea">
                                 
                        <div id="tabs">
                            <ul>
                                <li><a href="#tabs-0">About Sueetie</a></li>
                                <li><a href="#tabs-1">Site Activity</a></li>
                                <li><a href="#tabs-2">Sueetie News</a></li>
                                <li><a href="#tabs-3">Discussions</a></li>
                                <li><a href="#tabs-4">Wiki Updates</a></li>
                                <li><a href="#tabs-5">Site Tag Cloud</a></li>
                            </ul>
                            <div id="tabs-0">
                                
    
                                <div class="aboutSueetieArea">
                                    <SUEETIE:ContentPart ID="ContentPart2" runat="server" ContentName="HomeContent" />
                                </div>
                            </div>
                            <div id="tabs-1">
                                <div class="rssTag">
                                    <span>Subscribe to Site Activity Feed </span><a href="/util/rss/siteactivity.aspx">
                                        <img src="/themes/lollipop/images/blogs/rsstag.png" /></a>
                                </div>
                                <div class="recentUserLog">
                                    <uc:UserLogActivityList ID="UserLogActivityList1" runat="server" IsRestricted="true"
                                        NumRecords="50" GroupID="0" />
                                </div>
                            </div>
                            <div id="tabs-2">
                                <div class="rssTag">
                                    <span>Subscribe to RSS Feed </span><a href="/util/rss/blogs.aspx">
                                        <img src="/themes/lollipop/images/blogs/rsstag.png" /></a>
                                </div>
                                <div class="recentPosts">
                                    <uc:BlogPostList ID="BlogPostList1" runat="server" IsRestricted="true" NumRecords="4" />
                                </div>
                                <div class="ViewAllLink">
                                    <span class="raq">&raquo;</span><a href="/blogs/default.aspx">View All Blogs</a></div>
                            </div>
                            <div id="tabs-3">
                                <div class="recentTopics">
                                    <uc:ForumTopicList ID="ForumTopicList1" runat="server" IsRestricted="true" NumRecords="5"
                                        ViewName="RecentForumTopicView" />
                                </div>
                            </div>
                            <div id="tabs-4">
                                <div class="recentWikiPages">
                                    <uc:WikiPageList ID="RecentWikiPageList1" runat="server" IsRestricted="true" NumRecords="4"
                                        GroupID="0" />
                                </div>
                            </div>
                            <div id="tabs-5">
                                <div class="tagCloudOuterArea">
                                    <div id="tagCloudLinks">
                                        <a href="javascript:void(0);" onclick="populateTags('0');return false;" class="tagAllLink">
                                            All Tags</a> | <a href="javascript:void(0);" onclick="populateTags('1');return false;"
                                                class="tagBlogLink">Blogs</a> | <a href="javascript:void(0);" onclick="populateTags('2');return false;"
                                                    class="tagForumLink">Forums</a> | <a href="javascript:void(0);" onclick="populateTags('3');return false;"
                                                        class="tagWikiLink">Wiki</a> | <a href="javascript:void(0);" onclick="populateTags('4');return false;"
                                                            class="tagMediaLink">Media Gallery</a> | <a href="javascript:void(0);" onclick="populateTags('7');return false;"
                                                                class="tagCMSLink">CMS Pages</a>
                                    </div>
                                    <div class="tagCloudArea">
                                        <ul id="ulTags">
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                        <div class="HomeNavLinksBottom">
                            <uc:HomeBodyMenu ID="HomeBodyMenu2" runat="server" />
                        </div>
</asp:content>
