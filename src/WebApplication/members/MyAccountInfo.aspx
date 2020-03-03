<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="MyAccountInfo.aspx.cs"
    Inherits="Sueetie.Web.MyAccountInfo" %>

<%@ Import Namespace="Sueetie.Core" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="/controls/editors/htmlEditor.ascx" TagPrefix="cc2" TagName="TextEditor" %>
<asp:content id="Content2" contentplaceholderid="cphHeader" runat="server">
    <link href="/scripts/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var noFollowMsg;
        var noFavoriteMsg;

        function pageLoad() {
            (function ($) {
                $.fn.emptyList = function () {
                    return this.empty();
                }
                $.fn.loadFollowList = function (inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function (index, inputData) {
                        input.append("<div class='followThumb'><a href='profile.aspx?u=" + inputData.First + "'><img src='/images/avatars/" +
                        inputData.Third + "'></a><br /><a href='profile.aspx?u=" + inputData.First + "'>" + inputData.Second + "</a></div>");
                    });
                    this.html(input.toString());
                    return this;
                }
                $.fn.loadFavoriteList = function (inputDataArray) {
                    this.emptyList();
                    var input = new Sys.StringBuilder("");
                    $.each(inputDataArray, function (index, inputData) {
                        input.append("<div class='favoriteDeleteTag'><a href=\"javascript:void(0);\" onclick=\"deleteFavorite('" + inputData.FavoriteID + "');return false;\">")
                        input.append("<img src=\"/images/shared/sueetie/delete.gif\" border=\"0\" /></a></div>");
                        input.append("<div class='favoriteTitle'><a href='" + inputData.Permalink + "'>" + inputData.Title + "</a></div>");
                        input.append("<div class='favoriteDescription'>" + inputData.Description + "</div>");
                        input.append("<div class='favoriteAuthoredBy'>" + inputData.ContentTypeAuthoredBy + "</div>");

                    });
                    this.html(input.toString());
                    return this;
                }

                populateFollows('-2', '0');
                populateFavorites('-2', '1', '0', '0');

            })(jQuery);
        }

        function deleteFavorite(favoriteID) {
            jConfirm('Are you sure you want to delete this favorite?', 'Delete this favorite?', function (r) {
                if (r == 1) {
                    var ws = new Sueetie.Web.SueetieService();
                    Sueetie.Web.SueetieService.DeleteFavorite(favoriteID, refreshFavorites, null);
                }
            });
        }

        function refreshFavorites(result) {
            populateFavorites(result.UserID, result.ContentTypeID, result.GroupID, '0');
        }

        function populateFollows(userid, followType) {
            var followMsg;
            var qstring = $.parseQuery();
            if (qstring.u > 0)
                userid = qstring.u;

            switch (followType) {
                case '1':
                    followMsg = 'Members following you!';
                    noFollowMsg = 'No one is following you at the moment.';
                    break;
                case '2':
                    followMsg = 'Members you are following who are also following you!';
                    noFollowMsg = 'You currently are not following anyone who is also following you.';
                    break;
                default:
                    followMsg = 'Members you are following';
                    noFollowMsg = 'You currently are not following anyone.';
                    break;
            }
            $("#FollowingNote").text(followMsg);

            Sueetie.Web.SueetieService.GetFollowList(userid, followType, getFollowsComplete);
        }

        function CheckForFollowEmpty() {
            if ($("#FollowArea").is(':empty')) {
                $("#FollowArea").text(noFollowMsg).addClass('NoFollowMsg');
            }
        }

        function getFollowsComplete(result) {
            $("#FollowArea").loadFollowList(result);
            CheckForFollowEmpty();
        }

        function CheckForFavoriteEmpty() {
            if ($("#FavoriteArea").is(':empty')) {
                $("#FavoriteArea").text(noFavoriteMsg).addClass('NoFavoriteMsg');
            }
        }

        function populateFavorites(userid, contenttypeid, groupid, isrestricted) {
            var favoriteMsg;
            var qstring = $.parseQuery();
            if (qstring.u > 0)
                userid = qstring.u;

            switch (contenttypeid) {
                case '1':
                    favoriteMsg = 'Your favorite Blog Posts';
                    noFavoriteMsg = 'You have not tagged any blog posts as favorites.';
                    break;
                case '2':
                    favoriteMsg = 'Your favorite Blog Comments';
                    noFavoriteMsg = 'You have not tagged any blog comments as favorites.';
                    break;
                case '3':
                    favoriteMsg = 'Your favorite Forum Topics';
                    noFavoriteMsg = 'You have not tagged any forum topics as favorites.';
                    break;
                case '4':
                    favoriteMsg = 'Your favorite Forum Messages';
                    noFavoriteMsg = 'You have not tagged any forum messages as favorites.';
                    break;
                default:
                    favoriteMsg = 'Your favorite Blog Posts';
                    noFavoriteMsg = 'You have not tagged any blog posts as favorites.';
                    break;
            }
            $("#FavoriteNote").text(favoriteMsg);

            var ws = new Sueetie.Web.SueetieService();
            ws.GetFavoriteContent(userid, contenttypeid, groupid, isrestricted, getFavoritesComplete, null);
        }

        function getFavoritesComplete(result) {
            $("#FavoriteArea").loadFavoriteList(result);
            CheckForFavoriteEmpty();
        }

    </script>

</asp:content>
<asp:content id="Content1" contentplaceholderid="cphBody" runat="server">
    <div align="center">
        <div class="CreateAccountArea">
            <h2 class="MessageTitle">
                Your Account Information
            </h2>
            <div class="MessageContent">
                <div class="FormArea">
                    <div class="FormFieldDescription">
                        Introduce yourself to the Sueetie Community by sharing information about yourself.
                        The information you provide here will be displayed on your <a href="profile.aspx?u=<%= CurrentUserID %>">
                            public member profile.</a>
                    </div>

                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                            <Scripts>
                                <asp:ScriptReference Path="~/scripts/jquery.js" />
                                <asp:ScriptReference Path="~/scripts/jquery.alerts.js" />
                                <asp:ScriptReference Path="~/scripts/jquery-utils.js" />
                            </Scripts>
                            <Services>
                                <asp:ServiceReference Path="~/util/services/SueetieService.svc" />
                            </Services>
                        </asp:ScriptManager>
                        <div class="ProfileArea">
                                <cc1:TabContainer ID="TabContainer1" runat="server" Width="100%" Font-Size="10px">
                                    <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Change Password">
                                        <ContentTemplate>
                                            <div style="font-size: 14px;">
                                                <p>
                                                </p>
                                                <div class="formSectionTitle">
                                                    Update your Password</div>
                                                <p>
                                                    <table width="600" cellpadding="4" cellspacing="4" border="0">
                                                        <tr>
                                                            <td align="right">
                                                                Current Password
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCurrentPassword" runat="server" Width="220" TextMode="password" /><asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator3" runat="server" ErrorMessage=" * " CssClass="BigErrorMessage"
                                                                    ControlToValidate="txtCurrentPassword" ValidationGroup="PasswordGroup" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                New password
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNewPassword1" runat="server" Width="220" TextMode="password" /><asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * " CssClass="BigErrorMessage"
                                                                    ControlToValidate="txtNewPassword1" />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtNewPassword1"
                                                                    Display="Dynamic" Text="<br>Password must contain 6 to 18 characters of both letters and numbers. No spaces. Special characters okay. Ex: !1something"  ValidationExpression="^(?=.*[0-9])(?=.*[a-zA-Z])([^ ]){6,18}$" runat="Server" ValidationGroup="PasswordGroup" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                Confirm password
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNewPassword2" runat="server" Width="220" TextMode="password" /><asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator2" runat="server" ErrorMessage=" Doesn't match... "
                                                                    CssClass="BigErrorMessage" ControlToValidate="txtNewPassword2" />
                                                                <asp:CompareValidator ID="ComparePassword" runat="server" ControlToValidate="txtNewPassword2"
                                                                    ControlToCompare="txtNewPassword1" Display="Dynamic" CssClass="BigErrorMessage"
                                                                    ValidationGroup="PasswordGroup">*</asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <div class="TextButtonBigArea">
                                                                    <asp:Button ID="CancelButton" OnClick="Cancel_Click" runat="server" Text="Cancel"
                                                                        CssClass="TextButtonBig" CausesValidation="False" />&nbsp;
                                                                    <asp:Button ID="ChangePasswordButton" OnClick="ResetPassword_Click" runat="server"
                                                                        Text="Update" CssClass="TextButtonBig" ValidationGroup="PasswordGroup" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="ChangePasswordMessageArea">
                                                        <asp:Label ID="lblPasswordMessage" runat="server" />
                                                    </div>
                                            </div>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Your Profile">
                                        <ContentTemplate>
                                            <div style="font-size: 14px;">
                                                <p>
                                                </p>
                                                <div class="formSectionTitle">
                                                    Current Profile</div>
                                                <p>
                                                </p>
                                                <table cellpadding="2" style="width: 595px">
                                                    <tr>
                                                        <td class="formLabelsText" width="180">
                                                            Display Name
                                                        </td>
                                                        <td width="410">
                                                            <asp:TextBox ID="txtDisplayName" runat="server" Width="200" MaxLength="50" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage=" * "
                                                                CssClass="BigErrorMessage" ControlToValidate="txtDisplayName" ValidationGroup="ProfileGroup" />
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtDisplayName"
                                        Display="Dynamic" Text="<br>Display Name must contain at least 2 characters. Spaces okay. Letters only."
                                        ValidationExpression="^[a-zA-Z \.\-\']{2,25}" runat="Server" CssClass="BigErrorMessage" Style="color: #df0031;
                                        font-size: inherit;" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="formLabelsText">
                                                            Email
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmail" runat="server" Width="200" MaxLength="100" />
                                                            <asp:RequiredFieldValidator ID="emailReq" runat="server" ControlToValidate="txtEmail"
                                                                ErrorMessage=" * " CssClass="BigErrorMessage" ValidationGroup="ProfileGroup" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                            <td align="right">
                                                                Time Zone</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddTimeZones" runat="server" Width="420px" />
                                                   </tr>
                                                     <tr>
                                                            <td align="right">
                                                                </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkNewsletter" runat="Server" checked="false" /> Subscribe to Monthly Newsletter
                                                   </tr>

                                                    <tr>
                                                        <td colspan="2" style="height: 12px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" OnClick="btnUpdateProfile_Click"
                                                                CssClass="TextButtonBig" ValidationGroup="ProfileGroup" />
                                                            &nbsp;
                                                            <asp:Label ID="lblProfileMessage" runat="server" Visible="false">Your Profile has been updated!</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="height: 20px;">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Site Photo">
                                        <ContentTemplate>
                                            <div style="font-size: 14px;">
                                                <p>
                                                </p>
                                                <div class="formSectionTitle">
                                                    Current Photo</div>
                                                <p>
                                                </p>
                                                <table cellpadding="2" style="width: 585px">
                                                    <tr runat="server" id="AvatarImageRow">
                                                        <td align="center" width="240">
                                                            <div class="AvatarBig">
                                                                <asp:Image ID="AvatarImg" runat="server" Visible="true" />
                                                            </div>
                                                            <br />
                                                            <div align="center">
                                                                <asp:Button runat="server" ID="DeleteAvatarButton" Visible="false" OnClick="DeleteAvatar_Click"
                                                                    CssClass="TextButtonSmall" CausesValidation="false" />
                                                            </div>
                                                        </td>
                                                        <td class="post" valign="top" width="335">
                                                            <p>
                                                                Upload new site photo</p>
                                                            <asp:FileUpload ID="File" runat="server" />
                                                            <p>
                                                            </p>
                                                            <asp:Button ID="UpdateUpload" CssClass="TextButtonBig" runat="server" OnClick="UploadUpdate_Click"
                                                                Text="Update" CausesValidation="false" />
                                                            <p>
                                                                <asp:Label ID="DeleteMessage" runat="server" Visible="false">Your site photo has been removed.  Due to caching you may not return to Lollipop Head immediately.</asp:Label></p>
                                                            <p>
                                                                <asp:Label ID="UpdateMessage" runat="server" Visible="false">Your site photo has been updated.  Due to caching it may not appear immediately.</asp:Label></p>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel ID="TabPanel5" runat="server" HeaderText="Bio">
                                        <ContentTemplate>
                                            <div id="AccountEditorArea">
                                                <cc2:TextEditor runat="server" ID="txtBio" />
                                                <br />
                                                <br />
                                                <asp:Button ID="btnUpdateBio" OnClick="btnUpdateBio_OnClick" runat="server" CssClass="TextButtonBig"
                                                    Text="Submit" CausesValidation="false" />
                                                <br />
                                                <br />
                                                <asp:Label ID="lblBioUpdateMessage" runat="server" Visible="false">Your bio has been updated!  Due to server caching, it may not appear on your Public Profile immediately.</asp:Label></p>
                                            </div>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                    </div>
                 </div>
            </div>
        </div>
    </div>
</asp:content>
