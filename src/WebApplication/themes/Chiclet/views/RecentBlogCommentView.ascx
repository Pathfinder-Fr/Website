<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.BlogCommentView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="ViewItem">
  <div class="ViewItemTitle"><a href="<%= Comment.Permalink %>"><%= Comment.Title %></a></div>
  <div class="ViewItemDescription"><%= DataHelper.TruncateText(Comment.Comment,150) %></div>
  <div class="ViewItemAuthorDate">By <a href="/members/profile.aspx?u=<%= Comment.UserID %>"><%= Comment.DisplayName%></a> on <%= Comment.CommentDate.ToLongDateString() %></div>
</div>