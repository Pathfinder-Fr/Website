<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieWelcomePage" ValidateRequest="false" %>
<%@ Import Namespace="Sueetie.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="false" />
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                Welcome to <%= SiteSettings.Instance.SiteName %>
            </h2>
            <div class="MessageContent">
                <div class="WelcomeArea">
                <div class="WelcomeMessageArea">
                        <p>
                    <SUEETIE:ContentPart ID="ContentPart2" runat="server" ContentName="WelcomeContent" />
                    </p>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
