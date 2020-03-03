<%@ Control Language="C#" AutoEventWireup="true" %>

    <div class="banner">
        <div class="topmenus">
            <div class="headerrightlinks">
                <ul class="menu">

                    <li><a href="/wiki/GetSueetie.ashx">Get Sueetie</a></li>
                    <li><a href="/marketplace">Marketplace</a></li>                    
                    <li><a href="/blog">Blog</a></li>
                    <li><a href="/forum">Forum</a></li>
                    <li><a href="/media">Media</a></li>
                    <li><a href="/wiki">Wiki</a></li>
                    <li><a href="/groups">Groups</a></li>
                    <li><a href="/blog/contact.aspx">Contact Us</a></li>
                </ul>
            </div>
        </div>
        <div id="TopLeftLogo">
            <a href="/default.aspx">&nbsp;</a></div>
        <div class="headerleftbottomlinksAdmin">
            <ul class="menu">
                <li runat="server" id="UserLI">
                    <asp:HyperLink ID="UserLink" runat="server" /></li>
                <li>
                    <asp:LoginStatus ID="LoginStatus1" runat="Server" LoginText="Sign in" LogoutText="Sign out"
                        EnableViewState="false" />
                </li>
                <li runat="server" id="RegisterLI">
                    <asp:HyperLink ID="RegisterLink" runat="server" Text="Register" NavigateUrl="/members/register.aspx" /></li>
                <li runat="server" id="AdminLI">
                    <asp:HyperLink ID="AdminLink" runat="server" Text="Control Panel" /></li>
            </ul>
        </div>
    </div>
    <div id="bannershadow">
    </div>