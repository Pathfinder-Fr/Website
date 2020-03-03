<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieMessagePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                <SUEETIE:SueetieLocal runat="server" Key="message_site_message" />
            </h2>
            <div class="MessageContent">
                <div style="height: 25px;">
                </div>
                <div class="WelcomeArea">
                    <div class="WelcomeTitle">
                        <asp:Label ID="lblWelcome" runat="server" />        
                    </div>
                    <div class="WelcomeMessageArea">
                        <p>
                      <asp:Label ID="lblMessage" runat="server" />         
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>