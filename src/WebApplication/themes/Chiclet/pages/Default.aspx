<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieDefaultPage" %>

<%@ Register Src="~/controls/BlogPostList.ascx" TagName="BlogPostList" TagPrefix="uc" %>
<%@ Register Src="~/controls/UserLogActivityList.ascx" TagName="UserLogActivityList"
    TagPrefix="uc" %>
<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="false" />

    <div id="content">

                    <div id="tabs">
                        <ul>
                                                    <li><a href="#tabs-0">Welcome</a></li>

                            <li><a href="#tabs-1">News</a></li>
                            <li><a href="#tabs-2">Site Activity</a></li>
                        </ul>
                        <div id="tabs-0">
                            <div class="recentPosts">
                                                   <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder1" runat="server" Role="Registered">
                            <TrueContentTemplate>
                                <div class="WelcomeMessage">
                                    <SUEETIE:WelcomeLabel ID="WelcomeLabel1" runat="server" />
                                </div>
                            </TrueContentTemplate>
                        </SUEETIE:UserRolePlaceHolder>
                                            
	
	<p>Sueetie is a free .NET-based online community platform that starts with the very best social networking applications in the business--BlogEngine.NET, Gallery Server Pro, ScrewTurn Wiki and YetAnotherForum.NET. Then we add centralized membership, theming, SEO, groups and more to give you the ability to create your own online community. Here's our latest <a style="font-weight: bold;" target="" title="" href="http://sueetie.com/wiki/SueetieSnapshot.ashx">Sueetie Features Snapshot!</a></p><p>Your community can be up and running in minutes with <b>Gummy Bear,</b> a free website package that contains everything you see here at Sueetie.com. <span id="ContentPart2">The current version&nbsp; of Gummy Bear is v1.3 
released June 21, 2010. </span></p><p class="Opening">For fellow .NET developers we provide <b>Sueetie Atomo,</b> a single Visual Studio Solution containing all framework source code for customizing Sueetie Communities
 and building all-new applications with the Sueetie Framework. </p>
 <p>Gummy Bear and Atomo are available in the Sueetie Marketplace, so please remember to come back next time you visit us on a non-mobile device to download the best Online Community platform in the business.</p>
                            </div>
                            </div>
                        <div id="tabs-1">
                        <div class="recentPostsTitle">Sueetie News</div>
                            <div class="recentPosts">
                                <uc:BlogPostList ID="BlogPostList1" runat="server" IsRestricted="true" NumRecords="7"
                                    ApplicationID="1" ViewName="RecentBlogPostView" />
                            </div>
                            <br class="clearB" />
                            <div class="ViewAllLink"><span class="raq">&raquo;</span><a href="/blogs/default.aspx">Go to Blogs</a></div>
                        </div>
                        <div id="tabs-2">
                            <div class="recentUserLog">
                                    <uc:UserLogActivityList ID="UserLogActivityList1" runat="server" IsRestricted="true"
                                        NumRecords="25" GroupID="0" />
                                </div>
                        </div>
                    </div>
                </div>
    
        <br class="clearB" />
    
</asp:content>
