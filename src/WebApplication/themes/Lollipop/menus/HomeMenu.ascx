<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl" %>
<%@ Import Namespace="Sueetie.Core" %>

<script runat="server" language="C#">

    void Page_Load()
    {

        if (!IsPostBack)
        {

            if (CurrentSueetieUser.IsRegistered)
            {
                if (SueetieConfiguration.Get().Core.UseForumProfile)
                    WelcomeLink.NavigateUrl = SueetieUrls.Instance.MasterAccountInfo().Url;
                else
                    WelcomeLink.NavigateUrl = SueetieUrls.Instance.MyAccountInfo().Url;
                WelcomeLink.Text = string.Format(SueetieLocalizer.GetString("link_greetings_member"), CurrentSueetieUser.DisplayName);
            }
            else
            {
                WelcomeLink.NavigateUrl = SueetieUrls.Instance.Login().Url;
                WelcomeLink.Text = SueetieLocalizer.GetString("link_greetings_anonymous");
            }

        }
    }
</script>

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
                <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" SueetieUrlLinkTo="Logout" TextKey="link_logout" />
            </TrueContentTemplate>
            <FalseContentTemplate>
                <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" SueetieUrlLinkTo="Login" TextKey="link_login" />
            </FalseContentTemplate>
        </SUEETIE:UserRolePlaceHolder>
    </li>
    <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder3" Role="NonMember" runat="server">
        <TrueContentTemplate>
            <li>
                <SUEETIE:SueetieLink ID="SueetieLink2" runat="server" SueetieUrlLinkTo="Register"
                    TextKey="link_register" />
            </li>
        </TrueContentTemplate>
    </SUEETIE:UserRolePlaceHolder>
    <li>
        <asp:HyperLink runat="server" ID="WelcomeLink" />
    </li>
    <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder4" Role="SueetieAdministrator"
        runat="server">
        <TrueContentTemplate>
            <li>
                <SUEETIE:SueetieLink runat="server" SueetieUrlLinkTo="AdminHome" TextKey="menutab_admin" />
            </li>
        </TrueContentTemplate>
    </SUEETIE:UserRolePlaceHolder>
</ul>
