using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Net.Mail;
using System.Web.Security;
using Sueetie.Controls;

namespace Sueetie.Web
{
    public partial class ForgotUsernamePage : SueetieBaseThemedPage
    {

        protected SueetieLocal SueetieLocal1;
        protected SueetieLocal SueetieLocal2;
        protected Button CancelButton;
        protected Button SendEmailButton;
        protected Label UserNameLabel;
        protected Literal ResultsText;
        protected TextBox txtEmail;


        public ForgotUsernamePage()
            : base("members_forgot_username")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetActiveButtonAttributes(SendEmailButton);
                SetActiveButtonAttributes(CancelButton);
                if (Request.IsAuthenticated)
                    Response.Redirect("/default.aspx");
                // UserNameLabel
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

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/default.aspx");
        }

        protected void SendEmail_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Length == 0)
            {
                ResultsText.Text = SueetieLocalizer.GetString("forgot_username_empty_email");
                return;
            }
            SueetieUser sueetieUser = SueetieUsers.GetSueetieUserByEmail(txtEmail.Text);
            if (String.IsNullOrEmpty(sueetieUser.UserName))
            {
                ResultsText.Text = SueetieLocalizer.GetString("forgot_username_notindatabase");
                return;
            }

            MailMessage msg = new MailMessage();

            AddFromAndSubject(msg, sueetieUser);
            AddBody(msg, sueetieUser);
            SendIt(msg);

            ResultsText.Text = SueetieLocalizer.GetString("forgot_username_sent_msg", new string[] { txtEmail.Text });
        }

        private MailMessage AddFromAndSubject(MailMessage _msg, SueetieUser _user)
        {
            _msg.From = new MailAddress(SiteSettings.Instance.ContactEmail,
                string.Format(SueetieLocalizer.GetString("forgot_username_user_services"), SiteSettings.Instance.SiteName));
            _msg.Subject = string.Format(SueetieLocalizer.GetString("forgot_username_your_accountinfo"), SiteSettings.Instance.SiteName);

            MailAddress userAddress = new MailAddress(_user.Email, _user.DisplayName);
            _msg.To.Add(userAddress);
            return _msg;
        }

        private MailMessage AddBody(MailMessage _msg, SueetieUser _user)
        {

            string body = string.Empty;
            _msg.IsBodyHtml = true;
            MembershipUser membershipUser = Membership.GetUser(_user.UserName);
            Uri uri = HttpContext.Current.Request.Url;

            body += "<div style=\"font-family: Arial; font-size: 16px; margin-bottom: 15px;\">";
            body += string.Format(SueetieLocalizer.GetString("forgot_username_as_requested"), SiteSettings.Instance.SiteName) + "</div>";
            body += "<div style=\"font-family: Arial; font-size: 14px; margin-bottom: 25px;\">";
            body += "</div>";
            body += "<div style=\"font-family: Arial; font-size: 14px; font-weight: bold; margin-bottom: 15px;\">";
            body += string.Format(SueetieLocalizer.GetString("forgot_username_your_username"), _user.UserName) + "<br />";
            body += string.Format(SueetieLocalizer.GetString("forgot_username_your_password"), membershipUser.ResetPassword()) + "<br />";
            body += "</div>";

            body += "<div style=\"font-family: Tahoma; font-size: 11px; font-weight: normal; margin-bottom: 15px;\">";
            body += "_______________________________________________________<br /><br />";
            body += "<a href=\"http://" + uri.Host + "\">" + string.Format(SueetieLocalizer.GetString("forgot_username_homepage_link"), SiteSettings.Instance.SiteName) + "</a></div>";

            _msg.Body = body;
            return _msg;
        }

        private void SendIt(MailMessage _msg)
        {
            SmtpClient client = new SmtpClient();
            client.Send(_msg);
        }
    }
}
