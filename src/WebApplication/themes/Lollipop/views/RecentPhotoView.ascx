<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.RecentPhotoView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="RecentThumbnail">
  <a href='/media/default.aspx?moid=<%= RecentPhoto.MediaObjectID %>'><img src='<%= RecentPhoto.Permalink%>' alt=""  /></a>
</div>
