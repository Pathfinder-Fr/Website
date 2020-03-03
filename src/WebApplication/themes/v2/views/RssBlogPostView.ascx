<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.BlogRssPostView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="BlogViewItem">
  <div class="ViewItemTitleRssBlogPost"><a href="<%= Post.Link %>"><%= Post.Title %></a></div>
  <div class="ViewItemDescription"><%= DataHelper.TruncateText(Post.Excerpt,255) %></div>
  <div class="ViewItemAuthorDate"><%= Post.PubDate.ToLongDateString() %></div>
</div>
