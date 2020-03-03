<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.ForgotUsernamePage" %>

<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                <SUEETIE:SueetieLocal ID="SueetieLocal2" runat="server" Key="forgot_username_title" />
            </h2>
            <div class="MessageContent">
                <div class="FormArea">
                    <div style="height: 25px;">
                    </div>
                            <div class="ForgetPasswordArea">
                            <p>
                            <SUEETIE:SueetieLocal ID="SueetieLocal1" runat="server" Key="forgot_username_enter_email" />
                            </p>
                             <p>
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="txtEmail" CssClass="LoginBoxBig"
                                        Width="220" Text="Your Email Address" /></p>
                            <p>
                            
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="LoginBoxBig" Width="220" /></p>
                            <div class="TextButtonBigArea">
  <asp:Button ID="CancelButton" OnClick="Cancel_Click" runat="server" Text="Cancel"
                                    CssClass="TextButtonBig" />&nbsp;                            
                                <asp:Button ID="SendEmailButton" OnClick="SendEmail_Click" runat="server" Text="Send"
                                    CssClass="TextButtonBig" />
                            </div>
                            <div class="LoginFailureMessageArea">
                                                <asp:Literal ID="ResultsText" runat="server" />
                                            </div>
                            <div style="height: 25px;">
                            </div>
                        </div>
                       
                    </div>
                </div>
            </div>
        </div>
</asp:content>
