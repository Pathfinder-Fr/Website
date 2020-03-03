<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.ForumTopicView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
 <script runat="server" language="C#">

    void Page_Load()
    {
        if (!IsPostBack)
        {
            UserAvatar1.AvatarSueetieUser = SueetieUsers.GetThinSueetieUser(Topic.SueetieUserID);
        }
    }
</script>

 
<div id="ViewItem" class="TopicViewItem">
<div class="ViewItemThumbnail">
<SUEETIE:UserAvatar runat="server" ID="UserAvatar1" Height="40" Width="40" BorderWidth="1" CssClass="ViewItemThumbnailImage" />
</div>
  <div class="ViewItemTitle"><a href="<%= Topic.Permalink %>"><%= Topic.Topic %></a></div>
  <div class="ViewItemDescription">In the forum "<%= Topic.Forum %>"</div> 
  <div class="ViewItemAuthorDate">Discussion started by <a href="/members/profile.aspx?u=<%= Topic.SueetieUserID %>"><%= Topic.DisplayName %></a> on <%= Topic.DateTimeCreated.ToString("MMM dd, yyyy") %></div>
</div>
