<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl"  %>

<script runat="server" language="C#">

    void Page_Load()
    {
        if (!IsPostBack)
        {
            if (IsGroup)
                GroupLI.Attributes.Add("class", "current");
        }

        AdminLink.NavigateUrl = "/admin/default.aspx";

        if (!Page.User.IsInRole("SueetieAdministrator"))
        {
            AdminLI.Visible = false;
            AdminLink.Visible = false;
        }
        
    }
</script>

<div class="menulinks">
    <ul class="menu">
        <li><a href="/wiki/GetSueetie.ashx">Get Sueetie</a></li>
        <li ><a href="/marketplace" >Marketplace</a></li>
        <li><a href="/blog">Blog</a></li>
        <li><a href="/forum">Forums</a></li>
        <li class="current"><a href="/media">Media</a></li>
        <li><a href="/wiki">Wiki</a></li>
        <li runat="server" id="GroupLI"><a href="/groups">Groups</a></li>
        <li><a href="/blog/contact.aspx">Contact Us</a></li>
        <li runat="server" id="AdminLI">
            <asp:HyperLink ID="AdminLink" runat="server" Text="[CP]" /></li>        
    </ul>
</div>

