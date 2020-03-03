<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="Sueetie.Web.UserEdit" MasterPageFile="/themes/lollipop/masters/admin.master" %>
<%@ Register Src="../controls/adminUserNavLinks.ascx" TagName="adminUserNavLinks" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphBodyTitle">
    Edit User Details
</asp:Content>
<asp:Content ID="Content6" runat="server" ContentPlaceHolderID="cphUserNavigation">
    <uc1:adminUserNavLinks ID="adminUserNavLinks1" runat="server" />
</asp:Content>
<asp:Content ID="Content7" runat="server" ContentPlaceHolderID="cphContentBody">


    <div class="AdminFormArea">
        <h2>
            Edit User: <asp:Label id="lblDisplayName" runat="server" /></h2>
            
            <asp:Label ID="ActionMessage" runat="server" Visible="false" CssClass="ActionMessage" />

            <AjaxControlToolkit:TabContainer ID="tcntUserInfo" runat="server" ActiveTabIndex="1" Width="100%">
                <AjaxControlToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Roles">
                    <ContentTemplate>
                            <div class="checkboxList">
                                <asp:CheckBoxList ID="UserRoles" runat="server" RepeatDirection="Vertical" RepeatColumns="3" CssClass="RoleColumns"  />
                            </div>
                            <p>
                                <asp:Button ID="UpdateUserRolesButton" runat="server" OnClick="UpdateUserRoles" Text="Update User Roles"
                                    CausesValidation="True" />
                            </p>
                            <p>
                                <asp:Label ID="lblRolesUpdated" Text="Roles updated!" Visible="false" runat="server" />
                            </p>
                        
                    </ContentTemplate>
                </AjaxControlToolkit:TabPanel>
                <AjaxControlToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="General User Info">
                    <HeaderTemplate>
                        General User Info
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div>
                            <br />
                            <table>
                                <tr>
                                    <td valign="top" style="width: 445px; padding: 4px;">
                                        <asp:DetailsView AutoGenerateRows="False" CssClass="detailsviewMain" DataSourceID="MemberData"
                                            ID="UserInfo" runat="server" OnItemUpdating="UserInfo_ItemUpdating" DefaultMode="Edit"
                                            HeaderText="General User Info" OnDataBound="UserInfo_OnDataBound">
                                            <RowStyle CssClass="detailsviewRowStyle" />
                                            <FieldHeaderStyle CssClass="detailsviewFieldHeader" />
                                            <HeaderStyle CssClass="detailsviewheaderBG" />
                                            <AlternatingRowStyle CssClass="detailsviewAlternateRowStyle" />
                                            <Fields>
                                                <asp:TemplateField ShowHeader="true" HeaderText="Sueetie UserID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSueetieUserID" runat="server" Text="<%# GetSueetieUserID() %>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                   <asp:TemplateField ShowHeader="true" HeaderText="Is Active">
                                                    <ItemTemplate>
                                                         <asp:CheckBox runat="server" Checked="<%# IsActiveSueetieUser() %>"  HeaderText="Is Active" ID="ChkActive" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True"></asp:BoundField>
                                                <asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
                                                <asp:BoundField DataField="Comment" HeaderText="Comment"></asp:BoundField>
                                                <asp:CheckBoxField DataField="IsApproved" HeaderText="Is Approved"></asp:CheckBoxField>
                                                <asp:CheckBoxField DataField="IsLockedOut" HeaderText="Is Locked Out" ReadOnly="True">
                                                </asp:CheckBoxField>
                                                <asp:CheckBoxField DataField="IsOnline" HeaderText="Is Online" ReadOnly="True"></asp:CheckBoxField>
                                                <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LastActivityDate" HeaderText="Last Activity Date" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LastLoginDate" HeaderText="Last Login Date" ReadOnly="True">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="LastPasswordChangedDate" HeaderText="Last Password Changed Date"
                                                    ReadOnly="True"></asp:BoundField>
                                                    <asp:TemplateField ShowHeader="true" HeaderText="IP">
                                                    <ItemTemplate>
                                                    <asp:Label ID="lblSueetieUserIP" runat="server" Text="<%# GetSueetieUserIP() %>" />  <span class="ipIconSpan"><asp:HyperLink ID="hyperlinkIPLookup" runat="server" Target="_blank" ImageUrl="/images/shared/sueetie/ip.png" CssClass="ipIcon" ToolTip="Lookup IP Address" /></span>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                                                            Text="Update User" />
                                                        <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            Text="Cancel" />
                                                        <asp:Button ID="Button4" runat="server" Text="Unlock User" OnClick="UnlockUser" />
                   <asp:Button ID="Button3" runat="server" CausesValidation="True" CommandName="Update"
                                                            Text="Update and Email Approval" CommandArgument="Email" />                                                        
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit"
                                                            Text="Edit User Info" />
                                                    </ItemTemplate>
                                                    <ControlStyle Font-Size="11px" />
                                                </asp:TemplateField>
                                            </Fields>
                                        </asp:DetailsView>
                                        
                                        <div class="AdminUserUpdateMessage">
                                            <asp:Literal ID="UserUpdateMessage" runat="server"></asp:Literal>
                                        </div>
                                        <br />
                                        <asp:ObjectDataSource ID="MemberData" runat="server" DataObjectTypeName="System.Web.Security.MembershipUser"
                                            SelectMethod="GetUser" UpdateMethod="UpdateUser" TypeName="System.Web.Security.Membership">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="username" QueryStringField="username" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="AdminUserInfo"><p><u>Active Sueetie Users</u> have both <strong>Active</strong> and <strong>Approved</strong> flags checked. <u>Inactive users</u> have both cleared.  <u>Users not yet approved</u> have <strong>Active</strong> flag checked but <strong>Approved</strong> flag cleared. To approve a user, check <strong>Is Approved.</strong> To disapprove a new user request, clear the <strong>Active</strong> flag.</p></div>
                    </ContentTemplate>
                    
                </AjaxControlToolkit:TabPanel>
                <AjaxControlToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="User Profile">
                    <ContentTemplate>
                        <div>
                            <p>
                            </p>
                            <div class="formSectionTitle">
                                Community Profile</div>
                            <p>
                            </p>
                            <div class="ProfileMessage">
                            This Profile management form is for sites using the scaled-down, non-Member Dashboard configuration. To update profile information for sites using the Member Dashboard use YetAnotherForum.NET Member Management Services.
                            </div>
                            <table cellpadding="6" cellspacing="8" style="width: 600px">
                                <tr>
                                    <td class="formLabelsText">
                                        Display Name:
                                    </td>
                                    <td width="300">
                                        <asp:TextBox ID="txtDisplayName" runat="server" Width="99%" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                <td class="formLabelsText">
                                    Time Zone:</td>
                                <td>
                                    <asp:DropDownList ID="ddTimeZones" runat="server" Width="420px" />
                            </tr>
                            <tr>
                                <td align="right">
                          </td>
                                <td>
                                    <asp:CheckBox id="chkNewsletter" runat="server" /> Subscribe to Monthly Newsletter
                            </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" ValidationGroup="EditProfile"
                                            OnClick="btnUpdateProfile_Click" />
                                        &nbsp;
                                        <asp:Label ID="lblProfileMessage" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <p>
                            </p>
                        </div>
                    </ContentTemplate>
                </AjaxControlToolkit:TabPanel>
                
                <AjaxControlToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="Change Password">
                    <ContentTemplate>
                        <div>
                            <p>
                            </p>
                            <table cellpadding="6" cellspacing="8" border="0">
                                <tr>
                                    <td>
                                        New Password:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="PasswordTextbox" runat="server" TextMode="Password" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="PasswordRequiredValidator" runat="server" ControlToValidate="PasswordTextbox"
                                            ErrorMessage="Required" ValidationGroup="changepassword" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Confirm Password:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="PasswordConfirmTextbox" runat="server" TextMode="Password" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="PasswordConfirmRequiredValidator" runat="server"
                                            ControlToValidate="PasswordConfirmTextbox" ErrorMessage="Required" ValidationGroup="changepassword" />
                                        <asp:CompareValidator ID="PasswordConfirmCompareValidator" runat="server" ControlToValidate="PasswordConfirmTextbox"
                                            ControlToCompare="PasswordTextBox" ErrorMessage="Confirm password must match password."
                                            ValidationGroup="changepassword" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="ChangePasswordButton" Text="Change Password" OnClick="ChangePassword_OnClick"
                                            runat="server" ValidationGroup="changepassword" />
                                    </td>
                                </tr>
                            </table>
                            <p>
                            </p>
                            <asp:Label ID="Msg" ForeColor="Maroon" runat="server" />
                        </div>
                    </ContentTemplate>
                </AjaxControlToolkit:TabPanel>
                 <AjaxControlToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="Ban">
                    <ContentTemplate>
                        <div>
                            <p>
                            </p>
                            <table cellpadding="3" border="0">
                               <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblBannedIntro" runat="server" class="BannedIntro" />
                                    </td>
                                </tr>
                                <tr id="trBan" runat="Server">
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="BanButton" Text="Ban this User" OnClick="BanUser_OnClick"
                                            runat="server" />
                                    </td>
                                </tr>
                               <tr id="trUnBan" runat="Server">
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="UnBanButton" Text="Lift Ban on this User" OnClick="UnBanUser_OnClick"
                                            runat="server" />
                                    </td>
                                </tr>   
                            <tr><td></td><td>
                            <p>
                            </p>
                            <asp:Label ID="BanMsg" ForeColor="Maroon" runat="server" />
                            </td></tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </AjaxControlToolkit:TabPanel>
            </AjaxControlToolkit:TabContainer>
            <br />
</div>
</asp:Content>
