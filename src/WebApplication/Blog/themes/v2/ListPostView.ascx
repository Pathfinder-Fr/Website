<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<div class="post xfolkentry" id="post<%=Index %>">
    <h1 class="listposttitle">
        <a href="<%=Post.RelativeLink %>" class="taggedlink">
            <%=Server.HtmlEncode(Post.Title) %></a></h1>
    <span class="author">by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + Server.UrlEncode(Post.Author) %>.aspx">
        <%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName : Post.Author %></a></span>
    <span class="pubDate">
        <%=Post.DateCreated.ToString("d. MMMM yyyy HH:mm") %></span>
    <div class="listtext">
        <asp:PlaceHolder ID="BodyContent" runat="server" />
    </div>
    <div class="listbottom">
        <p class="tags">
            Tags:
            <%=TagLinks(", ") %></p>
        <p class="categories">
            <%=CategoryLinks(" | ") %></p>
    </div>
   
<%--   <div class="followFavoriteArea">
    <%= SueetieFollow %> <%= SueetieFavePost  %>
   </div>--%>
</div>

