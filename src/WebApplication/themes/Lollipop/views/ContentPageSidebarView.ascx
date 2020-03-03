<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.ContentPageView" %>
<%@ Import Namespace="Sueetie.Core" %>
 

<div id="ViewItem" class="ContentSidebarViewItem">
<div class='<%= this.IsCurrentPage() ? "ContentSidebarTitleCurrent" : "ContentSidebarTitle" %>'><a href='<%= ContentPage.Permalink %>'><%= ContentPage.PageTitle %></a></div>
</div>