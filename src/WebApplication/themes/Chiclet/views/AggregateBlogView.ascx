<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.BlogView" %>
<%@ Import Namespace="Sueetie.Core" %>
<div class="bloglistitem" id="blog<%= Blog.ApplicationKey.ToLower() %>">
    <h2 class="listblogtitle">
        <a href="/<%= Blog.ApplicationKey %>/default.aspx" class="taggedlink">
            <%= Blog.BlogTitle %></a></h2>
    <div class="MostRecentPost">
        New: <a href="<%= Blog.Permalink  %>" class="BlogPostTitle">
            <%= Blog.PostTitle%></a></div>
    <div class="author">
        by <a href="/members/profile.aspx?u=<%= Blog.PostAuthorID %>">
            <%= Blog.PostAuthorDisplayName %></a> on
        <%= Blog.PostDateCreated.ToLongDateString()%></div>
</div>
