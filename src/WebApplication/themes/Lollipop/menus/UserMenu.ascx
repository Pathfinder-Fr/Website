<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl" %>
<%@ Import Namespace="Sueetie.Core" %>

<script runat="server" language="C#">



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

    }
</script>

<div class="pagetoplinks">
    <ul class="menu">
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
                    <SUEETIE:SueetieLink ID="SueetieLink2" runat="server" UrlName="members_register"
                        TextKey="link_register" />
                </li>
            </TrueContentTemplate>
        </SUEETIE:UserRolePlaceHolder>
        <li>
            <asp:HyperLink runat="server" ID="WelcomeLink" />
        </li>
        <li class="UserMenuAvatar">

            <SUEETIE:UserAvatar ID="UserAvatar1" runat="server" CssClass="UserMenuAvatar" Height="28"
                Width="28" BorderWidth="1" UseCachedAvatarRoot="false" />
        </li>
    </ul>
</div>
