<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.AddonPack.Views.BlogPostView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="ViewItem">
  <div class="ViewItemTitle ViewItemTitle<%= Post.ApplicationKey.ToLower() %>"><a href="<%= Post.Permalink %>"><%= Post.Title %></a></div>
  <div class="ViewItemDescription"><%= DataHelper.TruncateText(Post.Description,180) %></div>
  <div class="ViewItemBlog">Sur le blog <a href="/<%= Post.ApplicationKey %>/default.aspx"><%= Post.BlogTitle %></a></div>
  <div class="ViewItemAuthorDate">Par <a href="/members/profile.aspx?u=<%= Post.UserID %>"><%= Post.DisplayName %></a> le <%= Post.DateCreated.ToLongDateString() %></div>
</div>