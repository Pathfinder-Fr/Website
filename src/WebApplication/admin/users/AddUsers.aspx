<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUsers.aspx.cs" Inherits="Sueetie.Web.AddUsers" %>

<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks"
    TagPrefix="uc1" %>
<asp:content id="Content1" contentplaceholderid="cphHeader" runat="Server" />
<asp:content id="Content5" runat="server" contentplaceholderid="cphBodyTitle">
    Create User With Roles
</asp:content>
<asp:content id="Content6" runat="server" contentplaceholderid="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:content>
<asp:content id="Content7" runat="server" contentplaceholderid="cphContentBody">
    <div class="AdminFormArea">
        <h2>
            Create User Account</h2>
        <div class="AdminFormDescription">
            Username must contain at least 4 characters. No spaces. Letters and numbers only.
        </div>
        <div class="NewUserForm">
            <table cellpadding="0" cellspacing="4" border="0" width="650px">
                <tr>
                    <td align="right">
                        Username:
                    </td>
                    <td>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="LoginBoxBig" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" * "
                            CssClass="BigErrorMessage" ControlToValidate="txtUsername" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtUsername"
                            Display="Dynamic" Text="<br>Username must contain at least 4 characters.  No spaces.<br>Letters and numbers only."
                            ValidationExpression="\w{4,}" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Display Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDisplayName" runat="Server" CssClass="LoginBoxBig" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" * "
                            CssClass="BigErrorMessage" ControlToValidate="txtDisplayName" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword1" runat="server" CssClass="LoginBoxBig" TextMode="password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * "
                            CssClass="BigErrorMessage" ControlToValidate="txtPassword1" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtPassword1"
                            Display="Dynamic" Text="<br>Password must contain at least 6 characters.  Letters and numbers only.  No spaces."
                            ValidationExpression="\w{6,}" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Confirm Password:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword2" runat="server" CssClass="LoginBoxBig" TextMode="password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" * "
                            CssClass="BigErrorMessage" ControlToValidate="txtPassword2" />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage=" Doesn't match... "
                            CssClass="BigErrorMessage" ControlToValidate="txtPassword2" ControlToCompare="txtPassword1" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Email Address:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="LoginBoxBig" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" * "
                            CssClass="BigErrorMessage" ControlToValidate="txtEmailAddress" />
                    </td>
                </tr>
                 <tr>
                                <td align="right">
                                    Time Zone</td>
                                <td>
                                    <asp:DropDownList ID="ddTimeZones" runat="server" Width="420px" />

                            </tr>
                            <tr><td></td><td>
                            <asp:CheckBox id="chkNewsletter" runat="server" checked="false" /> Subscribe to Monthly Newsletter
                            </td></tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <div class="TextButtonBigArea">
                            <asp:Button ID="CreateUserButton" runat="server" OnClick="CreateUser_Click" Text="Create Account!"
                                CssClass="TextButtonBig" CausesValidation="True" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lblResults" runat="server" Visible="false" CssClass="ResultsMessage" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="LoginFailureMessageArea">
                            <asp:Label ID="labelUserMessage" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:content>
