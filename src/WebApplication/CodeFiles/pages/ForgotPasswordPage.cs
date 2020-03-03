using System;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class ForgotPasswordPage : SueetieBaseThemedPage
    {
        protected PasswordRecovery PasswordRecovery1;

        public ForgotPasswordPage()
            : base("members_forgot_password")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PasswordRecovery1.MailDefinition.Subject = SueetieLocalizer.GetString("forgot_password_emailsubject");
                PasswordRecovery1.UserNameFailureText = SueetieLocalizer.GetString("forgot_username_couldnotfind");
                PasswordRecovery1.SuccessText = SueetieLocalizer.GetString("forgot_username_success");
                if (Request.IsAuthenticated)
                    Response.Redirect("/default.aspx");
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

        #region send carbon copy to webmaster

        // this code sends a duplicate email to the webmaster (address below must be corrected)
        // we could have done this from the MailDefinitions property window but it was more fun this way
        protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {

            e.Message.IsBodyHtml = true;
            //e.Message.CC.Add("admin@sueetie.org");
        }

        #endregion
    }
}