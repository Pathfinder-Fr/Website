<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl" %>
<%@ Import Namespace="Sueetie.Core" %>

<script runat="server" language="C#">
    void Page_Load()
    {
        if (!this.IsPostBack)
        {
            // App Custom
            AdminLI.Visible = Page.User.IsInRole("WikiAdministrator");
            if (AdminLI.Visible)
            {
                AdminLink.NavigateUrl = "/Wiki/admin.aspx";
                AdminLink.Text = "Administration Wiki";
            }
            
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
    }
</script>
<SUEETIE:UserAvatar runat="server" CssClass="UserMenuAvatar" Height="60" Width="60" BorderWidth="1" UseCachedAvatarRoot="false" />
<ul>
    <li>
        <asp:HyperLink runat="server" ID="WelcomeLink" />
    </li>
    <SUEETIE:UserRolePlaceHolder Role="Registered" runat="server">
      <TrueContentTemplate>
        <li><SUEETIE:InboxLink runat="server" /></li>
      </TrueContentTemplate>
    </SUEETIE:UserRolePlaceHolder>
    <li>
        <SUEETIE:UserRolePlaceHolder Role="Registered" runat="server">
            <TrueContentTemplate>
                <SUEETIE:SueetieLink runat="server" UrlName="members_logout" TextKey="link_logout" />
            </TrueContentTemplate>
            <FalseContentTemplate>
                <SUEETIE:SueetieLink runat="server" UrlName="members_login" TextKey="link_login" />
            </FalseContentTemplate>
        </SUEETIE:UserRolePlaceHolder>
    </li>
    <SUEETIE:UserRolePlaceHolder Role="NonMember" runat="server">
        <TrueContentTemplate>
            <li>
                <SUEETIE:SueetieLink runat="server" UrlName="members_register" TextKey="link_register" />
            </li>
        </TrueContentTemplate>
    </SUEETIE:UserRolePlaceHolder>
    <li runat="server" id="AdminLI"><asp:HyperLink ID="AdminLink" runat="server" /></li>    
</ul>