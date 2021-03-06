<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<div class="post xfolkentry" id="post<%=Index %>">
    <h1>
        <a href="<%=Post.RelativeLink %>" class="taggedlink">
            <%=Server.HtmlEncode(Post.Title) %></a></h1>
    <span class="author">by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Server.UrlEncode(Post.Author) %>.aspx">
        <%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName : Post.Author %></a></span>
    <span class="pubDate">
        <%=Post.DateCreated.ToString("d. MMMM yyyy HH:mm") %></span>
    <div class="text">
        <asp:PlaceHolder ID="BodyContent" runat="server" />
    </div>
    <div class="bottom">
        <%=Rating %>
        <p class="tags">
            Tags:
            <%=TagLinks(", ") %></p>
        <p class="categories">
            <%=CategoryLinks(" | ") %></p>
    </div>
    <div class="footer">
       
        <%=AdminLinks %>
        <a rel="bookmark" href="<%=Post.PermaLink %>" title="<%=Server.HtmlEncode(Post.Title) %>">
            Permalink</a> | <a rel="nofollow" href="<%=Post.RelativeLink %>#comment">
                <%=Resources.labels.comments %>
                (<%=Post.ApprovedComments.Count %>)</a>
    </div>
  
<%--   <div class="followFavoriteArea">
    <%= SueetieFollow %> <%= SueetieFavePost  %>
   </div>--%>
</div>

