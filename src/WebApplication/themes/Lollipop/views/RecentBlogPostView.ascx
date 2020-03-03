<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.AddonPack.Views.BlogPostView"  %>
<%@ Import Namespace="Sueetie.Core" %>
  
<div id="ViewItem" class="BlogViewItem"> 
  <div class="ViewItemTitle<%= Post.ApplicationKey.ToLower() %>"><a href="<%= Post.Permalink %>"><%= Post.Title %></a></div>
  <div class="ViewItemDescription"><%= DataHelper.TruncateText(Post.Description,255) %></div>
  <div class="ViewItemBlog">In blog <a href="/<%= Post.ApplicationKey %>/default.aspx"><%= Post.BlogTitle %></a></div>
  <div class="ViewItemAuthorDate">By <a href="/members/profile.aspx?u=<%= Post.UserID %>"><%= Post.DisplayName %></a> on <%= Post.DateCreated.ToLongDateString() %></div>
</div>