<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieActivatePage" %>

<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                Account Activation
            </h2>
            <div class="MessageContent">
                <div style="height: 25px;">
                </div>
                <div class="WelcomeArea">
                    <div class="WelcomeTitle">
                        <asp:Label ID="lblWelcome" runat="server" />
                    </div>
                    <div class="ActivationMessageArea">
                        <p>
                            <asp:PlaceHolder ID="phActivated" runat="server">
                                <p>
                                <SUEETIE:SueetieLocal Key="activate_thankyou" runat="server" />
                                    </p>
                                
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phNot" runat="server">
                                <p>
                                <SUEETIE:SueetieLocal ID="SueetieLocal1" Key="activate_problem" runat="server" />
                                   </p>
                            </asp:PlaceHolder>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:content>
