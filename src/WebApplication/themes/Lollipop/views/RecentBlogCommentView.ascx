<%@ Control Language="C#" EnableViewState="False" Inherits="Sueetie.Controls.BlogCommentView" %>
<%@ Import Namespace="Sueetie.Core" %>
 
 <script runat="server" language="C#">

    void Page_Load()
    {
        if (!IsPostBack)
        {
            UserAvatar1.AvatarSueetieUser = SueetieUsers.GetThinSueetieUser(Comment.UserID);
        }
    }
</script>

<div id="ViewItem" class="ViewItem">
<SUEETIE:UserAvatar runat="server" ID="UserAvatar1" Height="30" Width="30"  />
  <div class="ViewItemTitle"><a href="<%= Comment.Permalink %>"><%= Comment.Title %></a></div>
  <div class="ViewItemDescription"><%= DataHelper.TruncateText(Comment.Comment,150) %></div>
  <div class="ViewItemAuthorDate">By <a href="/members/profile.aspx?u=<%= Comment.UserID %>"><%= Comment.DisplayName%></a> on <%= Comment.CommentDate.ToLongDateString() %></div>
</div>