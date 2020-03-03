<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl" %>
<%@ Import Namespace="Sueetie.Core" %>

<script runat="server" language="C#">
    
    void Page_Load()
    {

        if (!IsPostBack)
        {
            MembershipUser user = Membership.GetUser();

            WikiAdminLink.NavigateUrl = "/wiki/admin.aspx";
            if (!Page.User.IsInRole("WikiAdministrator"))
            {
                WikiAdminLI.Visible = false;
                WikiAdminLink.Visible = false;
            }
            UserLink.Text = "Welcome, Guest!";
            UserLink.NavigateUrl = "/members/login.aspx";
            if (Page.User.Identity.IsAuthenticated)
            {
                string _displayName = SueetieUsers.GetUserDisplayName(SueetieContext.Current.User.UserID, false);

                if (SueetieConfiguration.Get().Core.UseForumProfile)
                    UserLink.NavigateUrl = SueetieUrls.Instance.MasterAccountInfo().Url;
                else
                    UserLink.NavigateUrl = SueetieUrls.Instance.MyAccountInfo().Url;

                UserLink.Text = string.Format(SueetieLocalizer.GetString("link_greetings_member"), _displayName);
  
            }
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
                    <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" UrlName="members_logout" TextKey="link_logout" />
                </TrueContentTemplate>
                <FalseContentTemplate>
                    <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" UrlName="members_login" TextKey="link_login" />
                </FalseContentTemplate>
            </SUEETIE:UserRolePlaceHolder>
        </li>
        <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder3" Role="NonMember" runat="server">
            <TrueContentTemplate>
                <li>
                    <SUEETIE:SueetieLink ID="SueetieLink2" runat="server" UrlName="members_register"
                        TextKey="link_register" />
                </li>
            </TrueContentTemplate>
        </SUEETIE:UserRolePlaceHolder>

        <li runat="server" id="WikiAdminLI">
            <asp:HyperLink ID="WikiAdminLink" runat="server" Text="Wiki Admin" /></li>
        <li>
            <asp:HyperLink runat="server" ID="UserLink" />
        </li>
      <li class="UserMenuAvatar">
            <SUEETIE:UserAvatar ID="UserAvatar1" runat="server" CssClass="UserMenuAvatar" Height="28"
                Width="28" BorderWidth="1" UseCachedAvatarRoot="false" />
        </li>
    </ul>
</div>

