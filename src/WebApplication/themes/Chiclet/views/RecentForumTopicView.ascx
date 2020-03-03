<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.ForumTopicView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="ViewItem">
  <div class="ViewItemTitle"><a href="<%= Topic.Permalink %>"><%= Topic.Topic %></a></div>
  <div class="ViewItemDescription">In the forum "<%= Topic.Forum %>"</div> 
  <div class="ViewItemAuthorDate">Discussion started by <a href="/members/profile.aspx?u=<%= Topic.SueetieUserID %>"><%= Topic.DisplayName %></a> on <%= Topic.DateTimeCreated.ToString("MMM dd, yyyy") %></div>
</div>