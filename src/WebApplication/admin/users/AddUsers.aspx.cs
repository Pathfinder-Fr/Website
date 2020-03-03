using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using Sueetie.Wiki;
using System.Web.Profile;
using Sueetie.Web.CodeFiles;

namespace Sueetie.Web
{
    public partial class AddUsers : SueetieAdminPage
    {
        public AddUsers()
            : base("admin_users_add")
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SueetieUIHelper.PopulateTimeZoneList(ddTimeZones);
                SetActiveButtonAttributes(CreateUserButton);
                lblResults.Visible = false;
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

        protected void Validate_Username(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = (!txtUsername.Text.Trim().Contains(" "));
        }

        protected void CreateUser_Click(object sender, System.EventArgs e)
        {
            if (Page.IsValid && IsPostBack)
            {
                if (Membership.GetUser(txtUsername.Text) == null && Membership.GetUserNameByEmail(txtEmailAddress.Text) == null)
                {
                    var ws = new SueetieService();

                    MembershipUser user = Membership.CreateUser(txtUsername.Text, txtPassword1.Text, txtEmailAddress.Text);

                    ProfileBase profile = (ProfileBase)SueetieUserProfile.Create(txtUsername.Text, true);
                    profile["DisplayName"] = txtDisplayName.Text;
                    profile["Newsletter"] = chkNewsletter.Checked;
                    profile.Save();


                    // SUEETIE NOTE: [BLOG]  BlogEngine.NET will throw an error if authorizing a user and they do not appear in be_User table.
                    // When Blog Application added, uncomment these lines.  

                    beDataContext dataContext = new beDataContext();

                    be_User _be_User = new be_User();
                    _be_User.UserName = user.UserName;
                    _be_User.Password = string.Empty;
                    _be_User.LastLoginTime = DateTime.Now;
                    _be_User.EmailAddress = user.Email;
                    dataContext.be_Users.InsertOnSubmit(_be_User);
                    dataContext.SubmitChanges();

                    Roles.AddUserToRole(user.UserName, "Registered");

                    // SUEETIE NOTE: [WIKI]  Adding user to ScrewTurn Wiki v3 /public/users.cs. 

                    if (SiteSettings.Instance.CreateWikiUserAccount)
                        WikiUsers.AddUser(txtUsername.Text, txtEmailAddress.Text, null, txtDisplayName.Text);

                    // SUEETIE NOTE: SueetieUser creation

                    SueetieUser sueetieUser = new SueetieUser();
                    sueetieUser.UserName = user.UserName.ToLower();
                    sueetieUser.Email = user.Email.ToLower();
                    sueetieUser.MembershipID = (Guid)user.ProviderUserKey;
                    sueetieUser.DisplayName = txtDisplayName.Text;
                    sueetieUser.IP = "127.0.0.1";
                    sueetieUser.IsActive = true;
                    sueetieUser.TimeZone = Convert.ToInt32(ddTimeZones.SelectedValue);

                    int userid = SueetieUsers.CreateSueetieUser(sueetieUser);

                    // SUEETIE NOTE: As of Gummy Bear 1.3, a Forum User Account is Created for All Users
                    SueetieForums.CreateForumUser(sueetieUser);

                    SueetieUsers.ClearSueetieUserListCache(SueetieUserType.RegisteredUser);

                    txtDisplayName.Text = txtEmailAddress.Text = txtPassword1.Text = txtPassword2.Text = txtUsername.Text = string.Empty;

                    lblResults.Visible = true;
                    lblResults.Text = "User Account Created!";
                }
                else
                {
                    string loginUrl = "login.aspx";
                    if (Request.QueryString["ReturnUrl"] != null)
                        loginUrl += "?ReturnUrl=" + Request.QueryString["ReturnUrl"];

                    if (Membership.GetUser(txtUsername.Text) != null)
                        labelUserMessage.Text = "User already exists.  Please enter another username.  If you already have a Sueetie account, please <a href=\"" + loginUrl + "\">login here.</a>";

                    else if (Membership.GetUserNameByEmail(txtEmailAddress.Text) != null)
                        labelUserMessage.Text = "The email address already exists.  Do you already have a Sueetie account?  If so, please <a href=\"" + loginUrl + "\">login here.</a>";

                }
            }
        }

    }

}
