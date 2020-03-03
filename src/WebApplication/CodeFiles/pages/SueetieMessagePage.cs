using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sueetie.Core;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Sueetie.Web
{
    public enum UserMessage
    {
        Unknown = 0,
        UserNotMember = 1,
        WikiAccessError = 2,
        LoggedOut = 3,
        RegistrationsClosed = 4,
        ChatForMembersOnly = 5,
        RegistrationEmailVerificationSent = 6,
        RegistrationApprovalSent = 7,
        UserBannedOnRegistration = 8,
        UserBannedOnLogin = 9,
        UserNotAuthorized = 10
    }

    public class SueetieMessagePage : SueetieBaseThemedPage
    {

        protected Label lblWelcome;
        protected Label lblMessage;

        public SueetieMessagePage()
            : base("members_message")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["msgid"] == null)
                    lblMessage.Text = SueetieLocalizer.GetString("msg_not_a_good_thing");
                else
                {
                    UserMessage userMessage = new UserMessage();
                    userMessage = (UserMessage)int.Parse(Request.QueryString["msgid"].ToString());
                    switch (userMessage)
                    {
                        case UserMessage.Unknown:
                            break;
                        case UserMessage.UserNotMember:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_unknown_user");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_unknown_user");
                            break;
                        case UserMessage.WikiAccessError:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_wikiaccess_denied");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_wikiaccess_denied");
                            break;
                        case UserMessage.LoggedOut:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_logged_out");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_logged_out");
                            break;
                        case UserMessage.RegistrationsClosed:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_registrations_currently_closed");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_registrations_currently_closed");
                            break;
                        case UserMessage.ChatForMembersOnly:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_chat_members_only");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_chat_members_only");
                            break;
                        case UserMessage.RegistrationEmailVerificationSent:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_registration_email_sent");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_registration_email_sent");
                            break;
                        case UserMessage.RegistrationApprovalSent:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_account_approval_sent");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_account_approval_sent");
                            break;
                        case UserMessage.UserBannedOnRegistration:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_account_banned_registration");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_account_banned_registration");
                            break;
                        case UserMessage.UserBannedOnLogin:
                            FormsAuthentication.SignOut();
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_account_banned_login");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_account_banned_login");
                            break;
                        case UserMessage.UserNotAuthorized:
                            lblWelcome.Text = SueetieLocalizer.GetString("msgtitle_user_not_authorized");
                            lblMessage.Text = SueetieLocalizer.GetString("msg_user_not_authorized");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }


}
