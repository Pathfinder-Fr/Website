<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.BlogPostView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div class="post xfolkentry" id="post<%= Post.ApplicationKey.ToLower() %>">
<div class="listpost<%= Post.ApplicationKey.ToLower() %>">
    <h1 class="listposttitle">
<a href="<%= Post.Permalink %>"  class="taggedlink"><%= Post.Title %></a></h1>
<span class="author">in <a href="/<%= Post.ApplicationKey  %>/default.aspx" class="BlogTitle"><%= Post.BlogTitle %></a> by <a href="/members/profile.aspx?u=<%= Post.UserID %>"><%= Post.DisplayName %></a></span>
    <span class="pubDate">
        <%= Post.DateCreated.ToString("d. MMMM yyyy HH:mm") %></span>
        </div>
    <div class="listtext">
        <%= DataHelper.TruncateText(Post.Description,295) %> <a href="<%= Post.Permalink %>">[More]</a></div>
</div>