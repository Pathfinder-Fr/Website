<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieRegisterPage"
    MasterPageFile="../masters/alternate.master" %>

<asp:Content ContentPlaceHolderID="cphBody" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/scripts/jquery.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/util/services/SueetieService.svc" />
        </Services>
    </asp:ScriptManager>
    <script language="JavaScript">

        function CheckUsername(username) {
            Sueetie.Web.SueetieService.IsNewUsername(username, SetUsernameStatusMessage);
        }

        function SetUsernameStatusMessage(result) {
            if (!result) {
                $("#UsernameCheckMsg").html("<br />Username already in use. <a href='ForgotPassword.aspx'>Forgot your Account Information?</a>");
            } else
                $("#UsernameCheckMsg").html("");
        }

        function CheckEmailAddress(email) {
            Sueetie.Web.SueetieService.IsNewEmailAddress(email, SetEmailStatusMessage);
        }

        function SetEmailStatusMessage(result) {
            if (!result) {
                $("#EmailCheckMsg").html("<br />Email address is already in use. <a href='ForgotPassword.aspx'>Forgot your Account Information?</a>");
            } else
                $("#EmailCheckMsg").html("");
        }

        function CheckDisplayName(displayname) {
            Sueetie.Web.SueetieService.IsNewDisplayName(displayname, SetDisplayNameStatusMessage);
        }

        function SetDisplayNameStatusMessage(result) {
            if (!result) {
                $("#DisplayNameCheckMsg").html("<br />Display Name is already in use. Sorry. Please choose another.");
            } else
                $("#DisplayNameCheckMsg").html("");
        }
        
    </script>
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                <SUEETIE:SueetieLocal runat="server" Key="register_title" />
            </h2>
            <div class="MessageContent">
                <div class="FormArea">
                    <div class="FormFieldDescription">
                        <SUEETIE:SueetieLocal ID="SueetieLocal1" runat="server" Key="register_form_description" />
                    </div>
                    <div class="JoinArea">
                        <table cellpadding="0" cellspacing="4" border="0" width="650px">
                            <tr>
                                <td align="right">
                                    <SUEETIE:SueetieLocal ID="SueetieLocal2" runat="server" Key="register_username_label" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="LoginBoxBig" OnBlur="CheckUsername(this.value)" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtUsername" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionUsernameValidator" ControlToValidate="txtUsername"
                                        Display="Dynamic" ValidationExpression="\w{4,}" runat="Server" CssClass="BigErrorMessage"
                                        Style="color: #df0031; font-size: inherit;" /><div id="UsernameCheckMsg">
                                        </div>
                                </td>
                            </tr>
                            <tr class="password">
                                <td align="right">
                                    <SUEETIE:SueetieLocal ID="SueetieLocal3" runat="server" Key="register_password_label" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword1" runat="server" CssClass="LoginBoxBig" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtPassword1" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionPasswordValidator" ControlToValidate="txtPassword1"
                                        Display="Dynamic" ValidationExpression="^(?=.*[0-9])(?=.*[a-zA-Z])([^ ]){6,18}$"
                                        runat="Server" CssClass="BigErrorMessage" Style="color: #df0031; font-size: inherit;" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <SUEETIE:SueetieLocal ID="SueetieLocal4" runat="server" Key="register_email_label" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="LoginBoxBig" OnBlur="CheckEmailAddress(this.value)" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtEmailAddress" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionEmailValidator" ControlToValidate="txtEmailAddress"
                                        Display="Dynamic" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                                        runat="Server" Style="color: #df0031; font-size: inherit;" />
                                    <div id="EmailCheckMsg">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <SUEETIE:SueetieLocal ID="SueetieLocal5" runat="server" Key="register_displayname_label" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDisplayName" runat="Server" CssClass="LoginBoxBig" OnBlur="CheckDisplayName(this.value)" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtDisplayName" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionDisplayNameValidator" ControlToValidate="txtDisplayName"
                                        Display="Dynamic" ValidationExpression="^[a-zA-Z \.\-\']{2,25}" runat="Server"
                                        CssClass="BigErrorMessage" Style="color: #df0031; font-size: inherit;" />
                                    <div id="DisplayNameCheckMsg">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <SUEETIE:SueetieLocal ID="SueetieLocal6" runat="server" Key="register_timezone_label" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddTimeZones" runat="server" CssClass="BigDropDown" Width="410px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkNewsletter" runat="Server" />
                                    <SUEETIE:SueetieLocal ID="SueetieLocal7" runat="server" Key="register_subscribe_label" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="TextButtonBigArea">
                                        <asp:Button ID="CreateUserButton" runat="server" OnClick="CreateUser_Click" CssClass="TextButtonBig"
                                            CausesValidation="True"  />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <div class="LoginFailureMessageArea">
                                        <asp:Label ID="labelUserMessage" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
