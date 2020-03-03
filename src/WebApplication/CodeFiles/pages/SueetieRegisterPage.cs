using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using System.Net.Mail;
using Sueetie.Wiki;
using System.Web.Profile;
using Sueetie.Web.CodeFiles;
using Saltie.Core;

namespace Sueetie.Web
{
    public class SueetieRegisterPage : SueetieBaseThemedPage
    {
        public SueetieRegisterPage()
            : base("members_register")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected Button CreateUserButton;
        protected CheckBox chkNewsletter;
        protected DropDownList ddTimeZones;
        protected Label labelUserMessage;
        protected TextBox txtDisplayName;
        protected TextBox txtEmailAddress;
        protected TextBox txtPassword1;
        protected TextBox txtUsername;

        protected RegularExpressionValidator RegularExpressionDisplayNameValidator;
        protected RegularExpressionValidator RegularExpressionUsernameValidator;
        protected RegularExpressionValidator RegularExpressionEmailValidator;
        protected RegularExpressionValidator RegularExpressionPasswordValidator;
        
        #region Properties

        public SueetieRegistrationType registrationType
        {
            get
            {
                if (ViewState["registrationType"] != null)
                    return (SueetieRegistrationType)ViewState["registrationType"];
                else
                    return SueetieRegistrationType.Automatic;
            }
            set
            {
                ViewState["registrationType"] = value;
            }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetActiveButtonAttributes(CreateUserButton);
                if (Request.IsAuthenticated)
                    Response.Redirect("/default.aspx");
                else if (SueetieContext.Current.SiteSettings.RegistrationType == (int)SueetieRegistrationType.Closed)
                {
                    // temporary redirect to enable during site updates or when you do not want new users to register
                    Response.Redirect("/members/message.aspx?msgid=4");
                }

                SueetieUIHelper.PopulateTimeZoneList(ddTimeZones);
                registrationType = (SueetieRegistrationType)SueetieContext.Current.SiteSettings.RegistrationType;
                InitializeCaptcha();

                CreateUserButton.Text = SueetieLocalizer.GetString("register_createuserbutton_text");
                RegularExpressionDisplayNameValidator.Text = SueetieLocalizer.GetString("register_validator_displayname");
                if (string.IsNullOrWhiteSpace(RegularExpressionDisplayNameValidator.Text))
                    RegularExpressionDisplayNameValidator.Text = "Erreur";

                RegularExpressionUsernameValidator.Text = SueetieLocalizer.GetString("register_validator_username");
                if (string.IsNullOrWhiteSpace(RegularExpressionUsernameValidator.Text))
                    RegularExpressionUsernameValidator.Text = "Erreur";

                RegularExpressionEmailValidator.Text = SueetieLocalizer.GetString("register_validator_email");
                if (string.IsNullOrWhiteSpace(RegularExpressionEmailValidator.Text))
                    RegularExpressionEmailValidator.Text = "Erreur";

                RegularExpressionPasswordValidator.Text = SueetieLocalizer.GetString("register_validator_password");
                if (string.IsNullOrWhiteSpace(RegularExpressionPasswordValidator.Text))
                    RegularExpressionPasswordValidator.Text = "Erreur";

            }
        }

        private void SetActiveButtonAttributes(Button button)
        {
            button.Attributes.CssStyle.Add("background-color", "#eee");
            button.Attributes.Add("OnMouseOut", "this.style.backgroundColor ='#eee';");
            button.Attributes.CssStyle.Add("text-decoration", "none");
            button.Attributes.CssStyle.Add("margin", "3px");
            button.Attributes.CssStyle.Add("padding", "7px 6px 7px 6px");
            button.Attributes.Add("OnMouseOver", "this.style.backgroundColor ='#ddd';");
        }

        protected void CreateUser_Click(object sender, System.EventArgs e)
        {
            if (txtDisplayName.Text.Trim().Length < 2)
            {
                labelUserMessage.Text = SueetieLocalizer.GetString("register_validator_displayname");
                InitializeCaptcha();
                return;
            }

            if (!SueetieUsers.IsNewDisplayName(txtDisplayName.Text))
            {
                labelUserMessage.Text = string.Format(SueetieLocalizer.GetString("register_exists_displayname_long"));
                InitializeCaptcha();
                return;
            }

            string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (SueetieUsers.IsIPBanned(ip))
            {

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(SiteSettings.Instance.FromEmail, SiteSettings.Instance.FromName);
                MailAddress userAddress = new
                    MailAddress(SiteSettings.Instance.ContactEmail, SiteSettings.Instance.SiteName + SueetieLocalizer.GetString("register_emailtoadmin_admin"));
                msg.To.Add(userAddress);
                msg.Subject = SueetieLocalizer.GetString("register_bannedattempt_subject");

                string body = SueetieLocalizer.GetString("register_bannedattempt_firstline");
                body += Environment.NewLine + Environment.NewLine;
                body += SueetieLocalizer.GetString("register_bannedattempt_ipaddress") + ip;
                body += SueetieLocalizer.GetString("register_bannedattempt_user") + txtUsername.Text + " (" + txtDisplayName.Text + ") " + txtEmailAddress.Text;
                body += Environment.NewLine + Environment.NewLine;
                msg.Body = body;

                if (SueetieConfiguration.Get().Core.SendEmails)
                {
                    EmailHelper.AsyncSendEmail(msg);
                }

                Response.Redirect("/members/message.aspx?msgid=8");
                return;
            }

            if (!IsCaptchaValid || !Page.IsValid || !IsPostBack)
            {
                return;
            }

            if (Membership.GetUser(txtUsername.Text) != null || Membership.GetUserNameByEmail(txtEmailAddress.Text) != null)
            {
                string loginUrl = "/members/login.aspx";
                if (Request.QueryString["ReturnUrl"] != null)
                    loginUrl += "?ReturnUrl=" + Request.QueryString["ReturnUrl"];

                if (Membership.GetUser(txtUsername.Text) != null)
                {
                    labelUserMessage.Text = string.Format(SueetieLocalizer.GetString("register_exists_username_long"), SiteSettings.Instance.SiteName, loginUrl);
                    InitializeCaptcha();
                }
                else if (Membership.GetUserNameByEmail(txtEmailAddress.Text) != null)
                {
                    labelUserMessage.Text = string.Format(SueetieLocalizer.GetString("register_exists_email_long"), SiteSettings.Instance.SiteName, loginUrl);
                    InitializeCaptcha();
                }

                return;
            }

            if (registrationType == SueetieRegistrationType.Automatic)
            {
                FormsAuthentication.SetAuthCookie(txtUsername.Text, true);
            }

            var user = Membership.CreateUser(txtUsername.Text, txtPassword1.Text, txtEmailAddress.Text);

            if (registrationType != SueetieRegistrationType.Automatic)
            {
                user.IsApproved = false;
                Membership.UpdateUser(user);
            }

            ProfileBase profile = (ProfileBase)SueetieUserProfile.Create(txtUsername.Text, true);
            profile["DisplayName"] = txtDisplayName.Text;
            if (chkNewsletter != null)
                profile["Newsletter"] = chkNewsletter.Checked;
            profile.Save();

            // SUEETIE NOTE: [BLOG]  BlogEngine.NET will throw an error if authorizing a user and they do not appear in be_User table.
            // When Blog Application added, uncomment these lines.  

            try
            {
                beDataContext dataContext = new beDataContext();
                be_User _be_User = new be_User();
                _be_User.UserName = user.UserName;
                _be_User.Password = string.Empty;
                _be_User.LastLoginTime = DateTime.Now;
                _be_User.EmailAddress = user.Email;
                dataContext.be_Users.InsertOnSubmit(_be_User);
                dataContext.SubmitChanges();
            }
            catch { }

            Roles.AddUserToRole(user.UserName, "Registered");

            if (SiteSettings.Instance.CreateWikiUserAccount)
            {
                WikiUsers.AddUser(txtUsername.Text, txtEmailAddress.Text, null, txtDisplayName.Text);
            }

            SueetieUser sueetieUser = new SueetieUser();
            sueetieUser.UserName = user.UserName.ToLower();
            sueetieUser.Email = user.Email.ToLower();
            sueetieUser.MembershipID = (Guid)user.ProviderUserKey;
            sueetieUser.DisplayName = txtDisplayName.Text;
            sueetieUser.IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            sueetieUser.IsActive = true;
            sueetieUser.TimeZone = Convert.ToInt32(ddTimeZones.SelectedValue);

            int userid = SueetieUsers.CreateSueetieUser(sueetieUser);

            SueetieForums.CreateForumUser(sueetieUser);

            Uri uri = HttpContext.Current.Request.Url;
            string port = uri.Port != 80 ? ":" + uri.Port : string.Empty;

            if (registrationType == SueetieRegistrationType.EmailVerification)
            {
                string valBinary;

                valBinary = user.CreationDate.ToBinary().ToString();

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(SiteSettings.Instance.FromEmail, SiteSettings.Instance.FromName);

                MailAddress userAddress = new MailAddress(user.Email.ToLower().ToString(), txtDisplayName.Text.ToString());
                msg.To.Add(userAddress);
                msg.Subject = string.Format(SueetieLocalizer.GetString("register_emailvalidation_subject"), SiteSettings.Instance.SiteName);

                string msgbody;

                string activateUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + port + "/members/Activate.aspx";

                msgbody = string.Format(SueetieLocalizer.GetString("register_emailvalidation_firstline"), SiteSettings.Instance.SiteName);
                msgbody += Environment.NewLine + Environment.NewLine;
                msgbody += activateUrl + "?uname=" + user.UserName + "&uid=" + userid + "&key=" + valBinary;
                msgbody += Environment.NewLine + Environment.NewLine;
                msgbody += SueetieLocalizer.GetString("register_emailvalidation_yourusername") + txtUsername.Text + Environment.NewLine;
                //msgbody += SueetieLocalizer.GetString("register_emailvalidation_yourpassword") + txtPassword1.Text + Environment.NewLine;

                msg.Body = msgbody;

                if (SueetieConfiguration.Get().Core.SendEmails)
                {
                    EmailHelper.AsyncSendEmail(msg);
                }

            }
            else if (registrationType == SueetieRegistrationType.AdministrativeApproval)
            {


                string approveUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + port + "/admin/approveusers.aspx";

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(SiteSettings.Instance.FromEmail, SiteSettings.Instance.FromName);

                MailAddress userAddress = new MailAddress(SiteSettings.Instance.ContactEmail, SiteSettings.Instance.SiteName + SueetieLocalizer.GetString("register_emailtoadmin_admin"));
                msg.To.Add(userAddress);
                msg.Subject = SueetieLocalizer.GetString("register_emailtoadmin_subject");

                string msgbody;

                msgbody = SueetieLocalizer.GetString("register_emailtoadmin_firstline");
                msgbody += Environment.NewLine + Environment.NewLine;
                msgbody += user.UserName.ToString() + " (" + sueetieUser.DisplayName + ")";
                msgbody += Environment.NewLine + Environment.NewLine;
                msgbody += approveUrl;


                msg.Body = msgbody;

                if (SueetieConfiguration.Get().Core.SendEmails)
                {
                    EmailHelper.AsyncSendEmail(msg);
                }
            }

            if (registrationType != SueetieRegistrationType.Automatic)
            {
                SaltieUserEvents.OnPreUserAccountApproval(CurrentSueetieUser);
                SueetieLogs.LogUserEntry(UserLogCategoryType.Registered, -1, userid);
            }

            switch (registrationType)
            {
                case SueetieRegistrationType.Automatic:
                    SaltieUserEvents.OnPostUserAccountApproval(sueetieUser);
                    SueetieUsers.CreateUpdateUserProfileCookie(sueetieUser);
                    SueetieLogs.LogUserEntry(UserLogCategoryType.JoinedCommunity, -1, userid);
                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (returnUrl != null)
                        Response.Redirect(returnUrl);
                    else
                        Response.Redirect("/members/welcome.aspx", true);
                    break;
                case SueetieRegistrationType.EmailVerification:
                    Response.Redirect("/members/message.aspx?msgid=6");
                    break;
                case SueetieRegistrationType.AdministrativeApproval:
                    Response.Redirect("/members/message.aspx?msgid=7");
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region CAPTCHA

        /// <summary> 
        /// Initializes the captcha and registers the JavaScript 
        /// </summary> 
        private void InitializeCaptcha()
        {
            if (ViewState[DateTime.Today.Ticks.ToString()] == null)
            {
                ViewState[DateTime.Today.Ticks.ToString()] = Guid.NewGuid().ToString();
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("function SetCaptcha(){");
            sb.AppendLine("var form = document.getElementById('" + Page.Form.ClientID + "');");
            sb.AppendLine("var el = document.createElement('input');");
            sb.AppendLine("el.type = 'hidden';");
            sb.AppendLine("el.name = '" + DateTime.Today.Ticks + "';");
            sb.AppendLine("el.value = '" + ViewState[DateTime.Today.Ticks.ToString()] + "';");
            sb.AppendLine("form.appendChild(el);}");

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "captchascript", sb.ToString(), true);
            Page.ClientScript.RegisterOnSubmitStatement(GetType(), "captchayo", "SetCaptcha()");
        }

        /// <summary> 
        /// Gets whether or not the user is human 
        /// </summary> 
        private bool IsCaptchaValid
        {
            get
            {
                if (ViewState[DateTime.Today.Ticks.ToString()] != null)
                {
                    return Request.Form[DateTime.Today.Ticks.ToString()] == ViewState[DateTime.Today.Ticks.ToString()].ToString();
                }

                return false;
            }
        }

        #endregion
    }

}
