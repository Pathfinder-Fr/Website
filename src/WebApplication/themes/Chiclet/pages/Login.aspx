<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieLoginPage" %>

<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
    <div style="text-align: center; width: 100%;">
        <asp:Login ID="Login1" runat="server" class="loginbox">
            <LayoutTemplate>
            <div class="LoginForm">
                                <table>
                                    <tr>
                                        <td class="LoginFormLabel">
                                            <SUEETIE:SueetieLocal ID="SueetieLocal4" runat="server" Key="login_username_label" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" CssClass="LoginBoxBig" />
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                ErrorMessage=" * " CssClass="BigErrorMessage" />
                                        </td>
                                    </tr>
                                    <tr class="LoginFormRowSpace">
                                        <td class="LoginFormLabel">
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
                                            <asp:CheckBox ID="RememberMe" runat="server" Text="&nbsp;Remember my login" />
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
            </div>
            </LayoutTemplate>
        </asp:Login>
        <asp:LoginStatus runat="server" ID="lsLogout" Visible="false" />
    </div>
</asp:content>
