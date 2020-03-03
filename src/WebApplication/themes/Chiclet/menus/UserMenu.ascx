<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sueetie.Core.SueetieBaseControl" %>
<%@ Import Namespace="Sueetie.Core" %>

<script runat="server" language="C#">

    void Page_Load()
    {

        if (!IsPostBack)
        {

            if (CurrentSueetieUser.IsRegistered)
                GreetingLocal.Parameter1 = CurrentSueetieUser.DisplayName;
            else
                GreetingLocal.Parameter1 = SueetieLocalizer.GetString("guest");

        }
    }
</script>

<div id="topLinks">
    <ul>
        <li id="topLink1">
        <SUEETIE:SueetieLocal runat="server" ID="GreetingLocal" Key="greeting_shared"  />
        </li>
        <SUEETIE:UserRolePlaceHolder ID="UserRolePlaceHolder1" Role="Registered" runat="server">
            <TrueContentTemplate>
                <li id="Li1" class="last">
                    <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" SueetieUrlLinkTo="Logout" TextKey="link_logout" />
                </li>
            </TrueContentTemplate>
            <FalseContentTemplate>
                <li class="last">
                    <SUEETIE:SueetieLink ID="SueetieLink1" runat="server" SueetieUrlLinkTo="Login" TextKey="link_login" />
                </li>
               
            </FalseContentTemplate>
        </SUEETIE:UserRolePlaceHolder>
    </ul>
</div>
