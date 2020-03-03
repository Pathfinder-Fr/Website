<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.ForgotPasswordPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                <SUEETIE:SueetieLocal runat="server" Key="forgot_password_title" />
            </h2>
            <div class="MessageContent">
                <div class="FormArea">
                    <div style="height: 25px;">
                    </div> 
                    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" OnSendingMail="PasswordRecovery1_SendingMail" >
                        <MailDefinition BodyFileName="~/util/Email/PasswordRecovery.htm" IsBodyHtml="true">
                        </MailDefinition>
                        <UserNameTemplate>
                            <div class="ForgetPasswordArea">
                                <p><SUEETIE:SueetieLocal ID="SueetieLocal1" runat="server" Key="forgot_password_enterusername" /></p>
                                <p><asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" CssClass="LoginBoxBig" Width="220" Text="Identifiant utilisateur" /></p>
                                <p> 
                                    <asp:TextBox ID="UserName" runat="server" CssClass="LoginBoxBig" Width="220" />
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Vous devez indiquer votre identifiant." ToolTip="L'identifiant doit être renseigné." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                </p>
                                <div class="TextButtonBigArea">
                                    <asp:Button ID="CancelButton" OnClick="Cancel_Click" runat="server" Text="Annuler" CssClass="TextButtonBig" />&nbsp;
                                    <asp:Button ID="SendEmailButton" CommandName="Submit" runat="server" Text="Envoyer" CssClass="TextButtonBig" ValidationGroup="PasswordRecovery1" />
                                </div>
                                <div class="LoginFailureMessageArea">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </div>
                                <div class="LoginFailureMessageArea">
                                    <a href="ForgotUsername.aspx"><SUEETIE:SueetieLocal ID="SueetieLocal2" runat="server" Key="forgot_username_question" /></a>
                                </div>
                                <div style="height: 25px;">
                                </div>
                            </div>
                        </UserNameTemplate>
                    </asp:PasswordRecovery>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
