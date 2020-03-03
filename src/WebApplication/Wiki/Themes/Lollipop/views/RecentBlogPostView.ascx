<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.AddonPack.Views.BlogPostView" %>
<%@ Import Namespace="Sueetie.Core" %>
<div id="ViewItem" class="BlogViewItem">
    <div class="ViewItemThumbnail">
        <div class="ViewItemThumbnailOuter">
            <div class="ViewItemThumbnailInner">
                <img src='<%= Post.PostImageUrl %>' alt="" />
            </div>
        </div>
    </div>
    <div class="ViewItemText">
        <div class="ViewItemTitle<%= Post.ApplicationKey.ToLower() %>">
            <a href="<%= Post.Permalink %>">
                <%= Post.Title %></a></div>
        <div class="ViewItemAuthorDate">
            By <a href="/members/profile.aspx?u=<%= Post.UserID %>">
                <%= Post.DisplayName %></a> on
            <%= Post.DateCreated.ToLongDateString() %></div>
            <div class="ViewItemPostCategories">
            <span><%= CategoryLabel %></span> <%= SueetieBlogs.CategoryUrls(Post) %>
            </div>
    </div>
</div>
