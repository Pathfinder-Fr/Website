<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl" %>
<%@ Import Namespace="Sueetie.Core" %>

<script runat="server" language="C#">

    string aspUserName;
    object aspProviderKey;
    string aspDisplayName;

    void Page_Load()
    {


        if (CurrentSueetieUser.IsRegistered)
        {
            string _displayName = SueetieUsers.GetUserDisplayName(SueetieContext.Current.User.UserID, false);

            if (SueetieConfiguration.Get().Core.UseForumProfile)
                WelcomeLink.NavigateUrl = SueetieUrls.Instance.MasterAccountInfo().Url;
            else
                WelcomeLink.NavigateUrl = SueetieUrls.Instance.MyAccountInfo().Url;

            WelcomeLink.Text = string.Format(SueetieLocalizer.GetString("link_greetings_member"), _displayName);
        }
        else
        {
            WelcomeLink.NavigateUrl = SueetieUrls.Instance.Login().Url;
            WelcomeLink.Text = SueetieLocalizer.GetString("link_greetings_anonymous");
        }


        var aspUser = System.Web.Security.Membership.GetUser(false);
        if (aspUser != null)
        {
            aspUserName = aspUser.UserName;
            aspProviderKey = aspUser.ProviderUserKey;
        }

        aspDisplayName = HttpContext.Current.Profile["DisplayName"] as string;

    }
</script>
<SUEETIE:UserAvatar ID="UserAvatar1" runat="server" CssClass="UserMenuAvatar" Height="60" Width="60" BorderWidth="1" UseCachedAvatarRoot="false" />
<ul>
    <li>
        <asp:HyperLink runat="server" ID="WelcomeLink" />
        <!-- ProviderUserKey = <%=aspProviderKey%> ; UserName = <%=aspUserName%> ; DisplayName = <%=aspDisplayName %> -->
    </li>

    <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder1" Role="Registered" runat="server">
        <TrueContentTemplate>
            <li>
                <SUEETIE:InboxLink ID="InboxLink1" runat="server" />
            </li>
        </TrueContentTemplate>
    </SUEETIE:UserRolePlaceHolder>
    <li>
        <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder2" Role="Registered" runat="server">
            <TrueContentTemplate>
                <SUEETIE:SueetieLink runat="server" UrlName="members_logout" TextKey="link_logout" />
            </TrueContentTemplate>
            <FalseContentTemplate>
                <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" UrlName="members_login" TextKey="link_login" />
            </FalseContentTemplate>
        </SUEETIE:UserRolePlaceHolder>
    </li>
    <SUEETIE:UserRolePlaceHolder Role="NonMember" runat="server">
        <TrueContentTemplate>
            <li>
                <SUEETIE:SueetieLink ID="SueetieLink2" runat="server" UrlName="members_register" TextKey="link_register" />
            </li>
        </TrueContentTemplate>
    </SUEETIE:UserRolePlaceHolder>
</ul>
