<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sueetie.Web.SueetieRegisterPage" %>

<asp:content contentplaceholderid="cphBody" runat="server">
    <div style="text-align: center; width: 100%;">
        
            <div class="LoginForm">
                    <div class="FormFieldDescription">
                        Please fill in the information below to create your account. To reduce bogus accounts created for spam purposes <strong>we require email verification.</strong> Upon registering below you will be sent an email containing a link back to the site at which point you will instantly become a Sueetie Community Member.
                    </div>
                        <table>
                            <tr>
                                <td class="LoginFormLabel">
                                    Username</td>
                                <td>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="LoginBoxBig" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtUsername" />
<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
 ControlToValidate="txtUsername"
 Display="Dynamic"
 Text="<br>Username must contain at least 4 characters.  No spaces.<br>Letters and numbers only."
 ValidationExpression="\w{4,}"
 Runat="Server" />                                        

                                </td>
                            </tr> <tr class="LoginFormRowSpace">
                                <td class="LoginFormLabel">
                                    DisplayName</td>
                                <td>
                                    <asp:TextBox ID="txtDisplayName" runat="Server" CssClass="LoginBoxBig" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage=" * " CssClass="BigErrorMessage"
                                        ControlToValidate="txtDisplayName" />
                                </td>
                            </tr>
                            <tr>
                                <td class="LoginFormLabel">
                                    Password</td>
                                <td>
                                    <asp:TextBox ID="txtPassword1" runat="server" CssClass="LoginBoxBig" TextMode="password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtPassword1" />
                                       <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
 ControlToValidate="txtPassword1"
 Display="Dynamic"
 Text="<br />Password must contain at least 6 characters.  Letters and numbers only.  No spaces."
 ValidationExpression="\w{6,}"
 Runat="Server" />    </td>
                            </tr>
   <tr>
                                <td class="LoginFormLabel">
                                    Confirm</td>
                                <td>
                                    <asp:TextBox ID="txtPassword2" runat="server" CssClass="LoginBoxBig" TextMode="password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtPassword2" />
  <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="<br />Passwords do not match... "
                                        CssClass="BigErrorMessage"  ControlToValidate="txtPassword2" ControlToCompare="txtPassword1" />                                        
                                      </td>
                            </tr>                            
                           
                            <tr>
                                <td class="LoginFormLabel">
                                    Email</td>
                                <td>
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="LoginBoxBig" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=" * "
                                        CssClass="BigErrorMessage" ControlToValidate="txtEmailAddress" /></td>
                            </tr>
                               <tr>
                                <td class="LoginFormLabel">
                                    Time Zone</td>
                                <td>
                                    <asp:DropDownList ID="ddTimeZones" runat="server"  CssClass="BigDropDown"  />
                                    </td>
                            </tr>
                            <tr>
                                            <td align="right">
                                                </td>
                                            <td>
                                                <asp:CheckBox ID="chkNewsletter" runat="Server" /> Subscribe to Monthly Newsletter
                                   </tr>
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
