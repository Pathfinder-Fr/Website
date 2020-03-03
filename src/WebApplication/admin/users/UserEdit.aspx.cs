using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using System.Net.Mail;

namespace Sueetie.Web
{
    public partial class UserEdit : SueetieAdminPage
    {
        public UserEdit()
            : base("admin_users_edit")
        {
        }
        #region Global Variables

        // declare global variables
        protected string username;
        protected MembershipUser _user;
        private DateTime lastActivitydate;
        private SueetieUser sueetieUser;

        #endregion

        #region On Page Prerender

        private void Page_PreRender()
        {
            // Load the User Roles into checkboxes.
            UserRoles.DataSource = Roles.GetAllRoles();
            UserRoles.DataBind();

            // Disable checkboxes if appropriate:
            if (UserInfo.CurrentMode != DetailsViewMode.Edit)
            {
                foreach (ListItem checkbox in UserRoles.Items)
                {
                    checkbox.Enabled = false;
                }
            }

            // Bind these checkboxes to the User's own set of roles.
            string[] userRoles = Roles.GetRolesForUser(username);
            foreach (string role in userRoles)
            {
                ListItem checkbox = UserRoles.Items.FindByValue(role);
                checkbox.Selected = true;
            }
        }

        #endregion

        #region On Page Load

        private void Page_Load()
        {
            // check if username exists in the query string
            username = Request.QueryString["username"];
            sueetieUser = SueetieUsers.GetUser(username);
            lblDisplayName.Text = sueetieUser.DisplayName;

         SueetieUIHelper.PopulateTimeZoneList(ddTimeZones, sueetieUser.TimeZone.ToString());

            if (username == null || username == "")
            {
                Response.Redirect("users.aspx");
            }

            // get membership user account based on username sent in query string
            _user = Membership.GetUser(username, false);
            lastActivitydate = _user.LastActivityDate;

            UserUpdateMessage.Text = "";

            if (_user.IsLockedOut)
            {
                ActionMessage.Visible = true;
                ActionMessage.Text = "User is locked out.  Use the \"Unlock User\" button to unlock";
            }
            else
            {
                
                string bannedIntro = "This user's IP mask (" + SueetieIPHelper.GetIPMask(sueetieUser.IP) + ") is recorded as banned. Click 'Lift Ban on this User' to permit the user to login or create a new account.";
                string unbannedIntro = "User is not banned. Click 'Ban User' to record user's IP mask as banned to thwart attempts to create new site accounts.";
                if (!Page.IsPostBack)
                {
                    if (sueetieUser.IsBanned)
                    {
                        trBan.Visible = false;
                        lblBannedIntro.Text = bannedIntro;
                    }
                    else
                    {
                        trUnBan.Visible = false;
                        lblBannedIntro.Text = unbannedIntro;
                    }


                    SueetieUserProfile profile = SueetieUsers.GetSueetieUserProfile(sueetieUser.UserID);

                    txtDisplayName.Text = profile.DisplayName;
                    chkNewsletter.Checked = profile.Newsletter;
                }

                _user.LastActivityDate = lastActivitydate;
                Membership.UpdateUser(_user);
                _user = Membership.GetUser(username, false);
            }
        }

        #endregion

        #region Update Profile Sub

        public void SaveProfile()
        {
            List<Pair> _properties = new List<Pair>()
                {
                     new Pair("DisplayName", txtDisplayName.Text),
                     new Pair("Newsletter", chkNewsletter.Checked)
                };
            Pair _propertyKeyValuePair = SueetieUsers.GenerateProfileKeyValues(_properties);
            SueetieUsers.UpdateSueetieUserProfile(_propertyKeyValuePair, sueetieUser.UserID);

            _user.LastActivityDate = lastActivitydate;
            Membership.UpdateUser(_user);
            _user = Membership.GetUser(username, false);
        }

        #endregion

        protected int GetSueetieUserID()
        {
            return sueetieUser.UserID;
        }

        protected string GetSueetieUserIP()
        {
            return DataHelper.NotAvailableIt(sueetieUser.IP);
        }

        protected bool IsActiveSueetieUser()
        {
            return sueetieUser.IsActive;
        }

        #region Update Profile Button Click

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {

            SaveProfile();
            lblProfileMessage.Text = "Profile saved successfully!";

            _user.LastActivityDate = lastActivitydate;
            Membership.UpdateUser(_user);
            _user = Membership.GetUser(username, false);

            sueetieUser = SueetieUsers.GetUser(username);
            sueetieUser.DisplayName = txtDisplayName.Text.Trim();
            SueetieUsers.UpdateDisplayName(sueetieUser);

            sueetieUser.DisplayName = txtDisplayName.Text.Trim();
            sueetieUser.TimeZone = Convert.ToInt32(ddTimeZones.SelectedValue);

            SueetieUsers.UpdateSueetieUser(sueetieUser);
            SueetieUsers.ClearUserProfileCache(sueetieUser.UserID);

        }

        #endregion

        #region Update Membership User Info

        protected void UserInfo_OnDataBound(object sender, EventArgs e)
        {
            HyperLink hyperlinkIPLookup = UserInfo.FindControl("hyperlinkIPLookup") as HyperLink;
            hyperlinkIPLookup.NavigateUrl = "http://www.ip2location.com/" + GetSueetieUserIP();
        }

        protected void UserInfo_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (Membership.GetUserNameByEmail((string)e.NewValues[0]) != null && Membership.GetUserNameByEmail((string)e.NewValues[0]).ToLower() != _user.UserName.ToLower())
            {
                UserUpdateMessage.Text += " Email used for another user.  Duplicate emails not permitted.";
                e.Cancel = true;
            }
            else
            {
                _user.Email = (string)e.NewValues[0];
                _user.Comment = (string)e.NewValues[1];
                _user.IsApproved = (bool)e.NewValues[2];

                _user.LastActivityDate = lastActivitydate;

                CheckBox chkActive = ((DetailsView)sender).FindControl("chkActive") as CheckBox;
                sueetieUser.IsActive = chkActive.Checked;
                sueetieUser.Email = _user.Email;
                sueetieUser.DisplayName = txtDisplayName.Text;

                SueetieUsers.UpdateSueetieUser(sueetieUser);


                try
                {
                    Membership.UpdateUser(_user);
                    UserUpdateMessage.Text = "Update Successful.";
                    e.Cancel = true;
                    UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
                    if ((string)e.CommandArgument == "Email")
                    {
                        MailMessage msg = new MailMessage();


                        if (SueetieConfiguration.Get().Core.SendEmails)
                        {
                            AddHeaders(msg, _user);
                            AddBody(msg, _user);
                            SendIt(msg);
                        }


                        UserUpdateMessage.Text += " Approval Email has been sent!";
                    }
                }
                catch (Exception ex)
                {
                    UserUpdateMessage.Text = "Update Failed: " + ex.Message;
                    e.Cancel = true;
                    UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
                }
            }
        }

        #endregion

        #region Send Email

        private MailMessage AddHeaders(MailMessage _msg, MembershipUser _user)
        {
            _msg.From = new MailAddress(SiteSettings.Instance.ContactEmail, SiteSettings.Instance.FromName);
            _msg.Subject = SueetieLocalizer.GetString("approval_email_subject");

            MailAddress userAddress = new MailAddress(_user.Email, SueetieUsers.GetUserDisplayName(_user.UserName));
            _msg.To.Add(userAddress);
            return _msg;
        }

        private MailMessage AddBody(MailMessage _msg, MembershipUser _user)
        {
            string _password = Membership.Providers["SqlMembershipProvider"].GetPassword(_user.UserName, null);
            string body = string.Empty;
            _msg.IsBodyHtml = true;

            body += "<div style=\"font-family: Arial; font-size: 16px; margin-bottom: 15px;\">";
            body += SueetieLocalizer.GetString("approval_email_firstline");
            body += "</div>";
            body += "<div style=\"font-family: Arial; font-size: 14px; margin-bottom: 25px;\">";
            body += "</div>";
            body += "<div style=\"font-family: Arial; font-size: 14px; font-weight: bold; margin-bottom: 15px;\">";
            body += SueetieLocalizer.GetString("approval_email_username") + _user.UserName + "<br />";
            body += SueetieLocalizer.GetString("approval_email_password") + _password + "<br />";
            body += "</div>";

            body += "<div style=\"font-family: Tahoma; font-size: 11px; font-weight: normal; margin-bottom: 15px;\">";
            body += "_______________________________________________________<br /><br />";
            body += "<a href=\"http://" + HttpContext.Current.Request.Url.Host + "\">" + SueetieLocalizer.GetString("approval_email_homepage") + "</a></div>";

            _msg.Body = body;
            return _msg;
        }

        private void SendIt(MailMessage _msg)
        {
            SmtpClient client = new SmtpClient();
            client.Send(_msg);
        }

        #endregion

        #region Update User Roles

        protected void UpdateUserRoles(object sender, EventArgs e)
        {
            // add or remove user from role based on selection
            foreach (ListItem rolebox in UserRoles.Items)
            {
                if (rolebox.Selected)
                {
                    if (!Roles.IsUserInRole(username, rolebox.Text))
                    {
                        Roles.AddUserToRole(username, rolebox.Text);
                    }
                }
                else
                {
                    if (Roles.IsUserInRole(username, rolebox.Text))
                    {
                        Roles.RemoveUserFromRole(username, rolebox.Text);
                    }
                }
            }
            lblRolesUpdated.Visible = true;
        }
        #endregion

        #region Delete User

        public void DeleteUser(object sender, EventArgs e)
        {
            // Membership.DeleteUser(username, false);
            MembershipUser u = Membership.GetUser(username);
            u.IsApproved = false;
            Membership.UpdateUser(u);
            //ProfileManager.DeleteProfile(username);
            //Membership.DeleteUser(username, true);
            Response.Redirect("users.aspx");
        }

        #endregion

        #region Unlock User

        public void UnlockUser(object sender, EventArgs e)
        {

            // Unlock the user.
            _user.UnlockUser();

            // DataBind the DetailsView to reflect same.
            UserInfo.DataBind();
        }

        #endregion

        #region Change Password Button Click

        public void ChangePassword_OnClick(object sender, EventArgs args)
        {
            // Update the password.
            // check if username exists in the query string
            username = Request.QueryString["username"];
            if (username == null || username == "")
            {
                Response.Redirect("users.aspx");
            }

            MembershipUser u = Membership.GetUser(username);
            string oldPassword = Membership.Providers["SqlMembershipProvider"].GetPassword(username, null);
      
            try
            {
                if (u.ChangePassword(oldPassword, PasswordTextbox.Text))
                {
                    Msg.Text = "Password changed successfully";
                }
                else
                {
                    Msg.Text = "Password change failed. Please re-enter your values and try again.";
                }
            }
            catch (Exception e)
            {
                Msg.Text = "An exception occurred: " + Server.HtmlEncode(e.Message) + ". Please re-enter your values and try again.";
            }
        }

        #endregion

        #region Ban User

        public void BanUser_OnClick(object sender, EventArgs args)
        {

            string mask = SueetieIPHelper.GetIPMask(sueetieUser.IP);
            if (!string.IsNullOrEmpty(sueetieUser.IP))
            {
                SueetieUsers.BanIP(mask);
                BanMsg.Text = "User Masked IP of <b>" + mask + "</b> has been banned.";
            }
            else
                BanMsg.Text = "Currently there is no recorded IP address for this user. No action was taken. IP Addresses are updated upon login and the user may not have logged-in since IP Banning was available. You may find the user's IP in YetAnotherForum yaf_user table.";
        }

        public void UnBanUser_OnClick(object sender, EventArgs args)
        {
            SueetieUsers.RemoveBannedIP(SueetieIPHelper.GetIPMask(sueetieUser.IP));
            BanMsg.Text = "User IP Address Ban on " + SueetieIPHelper.GetIPMask(sueetieUser.IP) + " has been removed.";
        }

        #endregion
    }
}
