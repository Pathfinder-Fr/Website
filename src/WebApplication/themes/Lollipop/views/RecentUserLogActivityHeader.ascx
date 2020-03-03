<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.UserLogActivityView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
<div id="ViewItem" class="ViewSubHeader">
<%=  string.Format("{0:ddd MMM d, yyyy}", LogActivity.DateTimeActivity) %> 
</div>
