<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.AddonPack.Views.BlogPostView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="ViewItem">
  <div class="ViewItemTitle"><a href="<%= Post.Permalink %>"><%= Post.Title %></a></div>
  <div class="ViewItemAuthorDate">Posted on <%= Post.DateCreated.ToLongDateString() %></div>
</div>