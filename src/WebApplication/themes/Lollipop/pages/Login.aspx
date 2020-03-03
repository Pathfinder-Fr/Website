<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieLoginPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">
    <div align="center">
        <asp:Login ID="Login1" runat="server" class="loginbox">
            <LayoutTemplate>
                <div class="CreateAccountArea">
                    <h2 class="MessageTitle">
                          <SUEETIE:SueetieLocal ID="SueetieLocal2" runat="server" Key="members_sign_in_title" />
                    </h2>
                    <div class="MessageContent">
                        <div class="FormArea">
                            <div style="height: 25px;">
                            </div>
                            <div class="JoinArea">
                                <table cellpadding="3" cellspacing="4" border="0" width="650px">
                                    <tr>
                                        <td align="right" width="200">
                                            <SUEETIE:SueetieLocal ID="SueetieLocal4" runat="server" Key="login_username_label" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" CssClass="LoginBoxBig" />
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                ErrorMessage=" * " CssClass="BigErrorMessage" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <SUEETIE:SueetieLocal ID="SueetieLocal3" runat="server" Key="login_password_label" /></td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="LoginBoxBig" />
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                ErrorMessage=" * " CssClass="BigErrorMessage" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="RememberMe" runat="server" /> <SUEETIE:SueetieLocal runat="server" Key="login_remember_my_account" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <div class="TextButtonBigArea">
                                                <asp:Button ID="LoginButton" CommandName="Login" runat="server" Text="Login" CssClass="TextButtonBig" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <div class="LoginFailureMessageArea">
                                                <asp:Literal ID="FailureText" runat="server" />
                                            </div>
                                            <div class="LoginFailureMessageArea">
                                                <a href="ForgotPassword.aspx"><SUEETIE:SueetieLocal ID="SueetieLocal1" runat="server" Key="forgot_password_question" /></a>
                                                 &nbsp;|&nbsp;&nbsp;<a href="Register.aspx"><SUEETIE:SueetieLocal ID="SueetieLocal5" runat="server" Key="members_create_account" /></a>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </LayoutTemplate>
        </asp:Login>
        <asp:LoginStatus runat="server" ID="lsLogout" Visible="false" />
    </div>
</asp:Content>
