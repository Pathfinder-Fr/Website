<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<div class="post xfolkentry" id="post<%=Index %>">
    <div class="titlelogo">
        <h2>
            <a class="postheader taggedlink" href="<%=Post.RelativeLink %>">
                <%=Server.HtmlEncode(Post.Title) %></a></h2>
    </div>
    <div class="entry">
        <asp:PlaceHolder ID="BodyContent" runat="server" />
    </div>
    <div class="postfooter">
        <span>Date Posted:</span>
        <%=Post.DateCreated.ToString("MMMM dd, yyyy")%><br />
        <span>Tags:</span>
        <%=TagLinks(", ") %><br />
        <span>Categories:</span>
        <%=CategoryLinks(" | ") %><br />
    </div>
    <p>
        <a href="#top">
            <img src="/themes/chiclet/images/back_top.png" /></a></p>
</div>
