using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;

namespace Sueetie.Web
{
    public partial class SueetieLoginPage : SueetieBaseThemedPage
    {
        protected Login Login1;
        protected LoginStatus lsLogout;
        private string returnUrl;
        private string createUserUrl;

        public SueetieLoginPage()
            : base("members_login")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.returnUrl = Request.QueryString["ReturnUrl"];

            string wikiReturnUrl = Request.QueryString["Redirect"];
            if (wikiReturnUrl != null)
                this.returnUrl = wikiReturnUrl;

            if (this.returnUrl != null)
            {
                this.returnUrl = this.returnUrl.Replace("/members", string.Empty);
                if (!this.returnUrl.StartsWith("/"))
                    this.returnUrl = "/" + returnUrl;
            }

            this. createUserUrl = "/members/register.aspx";
            if (this.returnUrl != null)
                this.createUserUrl += "?ReturnUrl=" + this.returnUrl;


            Button LoginButton = Login1.FindControl("LoginButton") as Button;
            Login1.FailureText = SueetieLocalizer.GetString("login_failed_msg", new string[] { this.createUserUrl });

            SetActiveButtonAttributes(LoginButton);
            if (Page.User.Identity.IsAuthenticated)
            {
                if (returnUrl != null)
                    Response.Redirect(returnUrl);
                else
                    Response.Redirect("/default.aspx", true);
            }
            else
            {
                Login1.LoggedIn += new EventHandler(Login1_LoggedIn);
                Login1.LoginError += Login1_LoginError;
                Login1.FindControl("username").Focus();
                InititializeCaptcha();
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

        void changepassword1_ContinueButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, true);
        }

        void Login1_LoginError(object sender, EventArgs e)
        {
            // Default message
            Login1.FailureText = SueetieLocalizer.GetString("login_failed_msg", new string[] { this.createUserUrl });

            try
            {
                // We try to retrieve the user
                var userInfo = Membership.GetUser(Login1.UserName);

                if (userInfo.IsLockedOut)
                {
                    Login1.FailureText = SueetieLocalizer.GetString("login_failed_locked_msg", new string[0]);
                }
                else if (!userInfo.IsApproved)
                {
                    Login1.FailureText = SueetieLocalizer.GetString("login_failed_notapproved_msg", new string[0]);
                }
            }
            catch { }
        }

        void Login1_LoggedIn(object sender, EventArgs e)
        {
            if (IsCaptchaValid && Page.IsValid && IsPostBack)
            {

                MembershipUser user = Membership.GetUser(Login1.UserName);
                SueetieUser sueetieUser = SueetieUsers.GetUser(user.UserName);

                bool hasIP = String.IsNullOrEmpty(sueetieUser.IP);
                string ip = String.IsNullOrEmpty(sueetieUser.IP) ?
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] : sueetieUser.IP;
                if (SueetieUsers.IsIPBanned(ip))
                {
                    Response.Redirect("/members/message.aspx?msgid=9");
                }
                else
                {
                    SueetieUsers.CreateUpdateUserProfileCookie(sueetieUser);
                    SueetieLogs.LogUserEntry(UserLogCategoryType.LoggedIn, -1, sueetieUser.UserID);

                    sueetieUser.IP = ip;
                    SueetieUsers.UpdateSueetieUserIP(sueetieUser);

                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (returnUrl != null)
                        Response.Redirect(returnUrl);
                    else
                        Response.Redirect("/default.aspx", true);
                }
            }
        }

        #region CAPTCHA

        /// <summary> 
        /// Initializes the captcha and registers the JavaScript 
        /// </summary> 
        private void InititializeCaptcha()
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