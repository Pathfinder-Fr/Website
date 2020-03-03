using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Web.Profile;

namespace Sueetie.Web
{
    public partial class MyAccountInfo : SueetieBaseThemedPage
    {

        #region Properties

        #region Control Values

        public string Occupation
        {
            get { return ((string)ViewState["Occupation"]) ?? string.Empty; }
            set { ViewState["Occupation"] = value; }
        }

        public string Gender
        {
            get { return ((string)ViewState["Gender"]) ?? string.Empty; }
            set { ViewState["Gender"] = value; }
        }
        public string Country
        {
            get { return ((string)ViewState["Country"]) ?? string.Empty; }
            set { ViewState["Country"] = value; }
        }

        public bool Newsletter
        {
            get { return (this.ViewState["Newsletter"] == null) ? false : (bool)this.ViewState["Newsletter"]; }
            set { ViewState["Newsletter"] = value; }
        }
        public string TwitterName
        {
            get { return ((string)ViewState["TwitterName"]) ?? string.Empty; }
            set { ViewState["TwitterName"] = value; }
        }
        public string Website
        {
            get { return ((string)ViewState["Website"]) ?? string.Empty; }
            set { ViewState["Website"] = value; }
        }

        #endregion

        #region Control Existence 

        public bool HasOccupationDropDown
        {
            get { return (this.ViewState["HasOccupationDropDown"] == null) ? false : (bool)this.ViewState["HasOccupationDropDown"]; }
            set { ViewState["HasOccupationDropDown"] = value; }
        }

        public bool HasGenderDropDown
        {
            get { return (this.ViewState["HasGenderDropDown"] == null) ? false : (bool)this.ViewState["HasGenderDropDown"]; }
            set { ViewState["HasGenderDropDown"] = value; }
        }

        public bool HasCountryDropDown
        {
            get { return (this.ViewState["HasCountryDropDown"] == null) ? false : (bool)this.ViewState["HasCountryDropDown"]; }
            set { ViewState["HasCountryDropDown"] = value; }
        }

        public bool HasNewsletterCheckBox
        {
            get { return (this.ViewState["HasNewsletterCheckBox"] == null) ? false : (bool)this.ViewState["HasNewsletterCheckBox"]; }
            set { ViewState["HasNewsletterCheckBox"] = value; }
        }

        public bool HasTwitterNameTextBox
        {
            get { return (this.ViewState["HasTwitterNameTextBox"] == null) ? false : (bool)this.ViewState["HasTwitterNameTextBox"]; }
            set { ViewState["HasTwitterNameTextBox"] = value; }
        }

        public bool HasWebsiteTextBox
        {
            get { return (this.ViewState["HasWebsiteTextBox"] == null) ? false : (bool)this.ViewState["HasWebsiteTextBox"]; }
            set { ViewState["HasWebsiteTextBox"] = value; }
        }

        #endregion

        #endregion


        public MyAccountInfo()
            : base("members_myaccountinfo")
        {
            this.SueetieMasterPage = "alternate.master";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager1.RegisterPostBackControl(UpdateUpload);
            ScriptManager1.RegisterPostBackControl(DeleteAvatarButton);
            ScriptManager1.RegisterPostBackControl(btnUpdateBio);
            ScriptManager1.RegisterPostBackControl(ChangePasswordButton);

            if (Request.QueryString["dv"] == "1" || Request.QueryString["av"] == "1" || Request.QueryString["iv"] == "1")
                TabContainer1.ActiveTabIndex = 2;
            else if (Request.QueryString["bio"] == "1")
                TabContainer1.ActiveTabIndex = 2;
            else if (Request.QueryString["pr"] == "1")
                TabContainer1.ActiveTabIndex = 0;
            else
                TabContainer1.ActiveTabIndex = 0;

            if (!Page.IsPostBack)
            {

                if (Page.User.Identity.IsAuthenticated)
                {

                    SueetieUser sueetieUser = SueetieUsers.GetUser(CurrentUserID, false);
                    SetActiveButtonAttributes(CancelButton);
                    SetActiveButtonAttributes(ChangePasswordButton);
                    SetActiveButtonAttributes(btnUpdateProfile);
                    SetActiveButtonAttributes(btnUpdateBio);

     
                    SueetieUIHelper.PopulateTimeZoneList(ddTimeZones, sueetieUser.TimeZone.ToString());
                    SueetieUserProfile profile = CurrentSueetieUser.Profile;

                    txtCurrentPassword.Text = string.Empty;
                    txtDisplayName.Text = sueetieUser.DisplayName;
                    txtEmail.Text = sueetieUser.Email;

                    #region Optional Profile Property Assignments


                    if (chkNewsletter != null)
                    {
                        HasNewsletterCheckBox = true;
                        chkNewsletter.Checked = profile.Newsletter;
                    }

                    #endregion

                    // When using ProfileBase...
                    //ProfileBase profile = HttpContext.Current.Profile;
                    //txtDisplayName.Text = profile["DisplayName"] as string;
                    //ddlGenders.SelectedValue = profile["Gender"] as string;
                    //ddlOccupations.SelectedValue = profile["Occupation"] as string;
                    //txtWebsite.Text = profile["Website"] as string;
                    //txtTwitterName.Text = profile["TwitterName"] as string;
                    //ddlCountries.SelectedValue = profile["Country"] as string;

                    txtBio.Text = sueetieUser.Bio;

                }
            }

            BindData();
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

        #region Avatar


        private void BindData()
        {

            AvatarImg.Visible = true;
            DeleteAvatarButton.Visible = false;
            DeleteAvatarButton.Text = SueetieLocalizer.GetString("accountinfo_clear_avatar_button");

            if (CurrentSueetieUser.HasAvatarImage)
                AvatarImg.ImageUrl = "/images/avatars/" + CurrentUserID.ToString() + ".jpg?n=" + DateTime.Now.ToLongTimeString();
            else
                AvatarImg.ImageUrl = "/images/avatars/noavatar.jpg";

            AvatarImg.CssClass = "AvatarBig";
            DeleteAvatarButton.Visible = true;


            if (Request.QueryString["dv"] == "1")
                DeleteMessage.Visible = true;
            if (Request.QueryString["av"] == "1")
                UpdateMessage.Visible = true;
            if (Request.QueryString["bio"] == "1")
                lblBioUpdateMessage.Visible = true;
            if (Request.QueryString["pr"] == "1")
                lblProfileMessage.Visible = true;
            if (Request.QueryString["pw"] == "1")
                lblPasswordMessage.Visible = true;
        }
        protected void DeleteAvatar_Click(object sender, System.EventArgs e)
        {
            SueetieUsers.DeleteAvatar(CurrentUserID);
            string path = HttpContext.Current.Server.MapPath("/") + SueetieConfiguration.Get().AvatarSettings.AvatarFolderPath +
                CurrentUserID.ToString() + ".jpg";
            System.IO.File.Delete(path);
            SueetieUsers.ClearUserCache(CurrentUserID);
            Response.Redirect("myaccountinfo.aspx?dv=1&bio=2");
            //BindData();
        }

        protected void UploadUpdate_Click(object sender, System.EventArgs e)
        {
            if (File.PostedFile != null && File.PostedFile.FileName.Trim().Length > 0 && File.PostedFile.ContentLength > 0)
            {
                int width = SueetieConfiguration.Get().AvatarSettings.Width;
                int height = SueetieConfiguration.Get().AvatarSettings.Height;
                int thumbnailWidth = SueetieConfiguration.Get().AvatarSettings.ThumbnailHeight;
                int thumbnailHeight = SueetieConfiguration.Get().AvatarSettings.ThumbnailWidth;
                string imageName = CurrentUserID.ToString() + ".jpg";
                string thumbnailImageName = CurrentUserID.ToString() + "t.jpg";

                ImageFormat imgFormat = ImageFormat.Jpeg;

                try
                {

                    #region Save to disk

                    Bitmap originalBitmap = new Bitmap(File.PostedFile.InputStream);
                    //ImageHelper.CalculateOptimizedWidthAndHeight(originalBitmap, out width, out height);

                    int jpegQuality = SueetieConfiguration.Get().AvatarSettings.ImageQuality;
                    string path = HttpContext.Current.Server.MapPath("/") + SueetieConfiguration.Get().AvatarSettings.AvatarFolderPath;

                    ImageHelper.SaveImageFile(originalBitmap, path + imageName, imgFormat, width, height, jpegQuality);
                    ImageHelper.SaveImageFile(originalBitmap, path + thumbnailImageName, imgFormat, thumbnailWidth, thumbnailHeight, jpegQuality);

                    #endregion

                    #region Save to database for use with Forums

                    SueetieUserAvatar sueetieUserAvatar = new SueetieUserAvatar();

                    MemoryStream stream = new MemoryStream();
                    originalBitmap.Save(stream, ImageFormat.Bmp);

                    byte[] data = new byte[stream.Length];
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                    stream.Read(data, 0, (int)stream.Length);

                    sueetieUserAvatar.UserID = CurrentUserID;
                    sueetieUserAvatar.AvatarImage = data;
                    sueetieUserAvatar.AvatarImageType = "image/jpeg";

                    SueetieUsers.UpdateSueetieUserAvatar(sueetieUserAvatar);
                    SueetieUsers.ClearUserCache(sueetieUserAvatar.UserID);

                    //Response.Redirect("myaccountinfo.aspx?av=1&bio=2&pr=2&pw=2");
                    Response.Redirect("myaccountinfo.aspx?av=1");
                    #endregion
                }
                catch
                {
                    //return null;
                }

            }

            BindData();
        }

        #endregion

        #region Update current Profile Sub

        public void SaveProfile()
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                #region Control Value Assignments

                if (HasNewsletterCheckBox)
                    Newsletter = chkNewsletter.Checked;

                #endregion

                List<Pair> _properties = new List<Pair>()
                {
                     new Pair("DisplayName", txtDisplayName.Text),
                     new Pair("Newsletter", Newsletter.ToString())
                };

                Pair _propertyKeyValuePair = SueetieUsers.GenerateProfileKeyValues(_properties);
                SueetieUsers.UpdateSueetieUserProfile(_propertyKeyValuePair, CurrentSueetieUserID);

                MembershipUser user = Membership.GetUser();
                user.Email = txtEmail.Text;
                Membership.UpdateUser(user);

                SueetieUser sueetieUser = new SueetieUser
                {
                    UserName = CurrentSueetieUser.UserName,
                    UserID = CurrentSueetieUser.UserID,
                    Email = txtEmail.Text.ToLower(),
                    DisplayName = txtDisplayName.Text,
                    IsActive = true,
                    TimeZone = Convert.ToInt32(ddTimeZones.SelectedValue),
                    MembershipID = CurrentSueetieUser.MembershipID
                };

                SueetieUsers.UpdateSueetieUser(sueetieUser);
                SueetieUsers.ClearUserProfileCache(CurrentSueetieUser.UserID);

            }
        }

        #endregion

        #region Update current Profile Button Click

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                SaveProfile();
                lblProfileMessage.Text = SueetieLocalizer.GetString("accountinfo_profile_updated");
                lblProfileMessage.Visible = true;
                Response.Redirect("myaccountinfo.aspx?pr=1");
            }
        }

        #endregion

        #region Sueetie Change Password

        protected void Cancel_Click(object sender, EventArgs e)
        {
            txtCurrentPassword.Text = string.Empty;
            txtNewPassword1.Text = string.Empty;
            txtNewPassword2.Text = string.Empty;
        }

        protected void ResetPassword_Click(object sender, EventArgs e)
        {

            MembershipUser user = Membership.GetUser();
            try
            {
                lblPasswordMessage.Visible = true;
                if (user.ChangePassword(txtCurrentPassword.Text, txtNewPassword1.Text))
                {
                    lblPasswordMessage.Text = SueetieLocalizer.GetString("accountinfo_password_changed");
                }
                else
                {
                    if (user.IsLockedOut)
                    {
                        lblPasswordMessage.Text = SueetieLocalizer.GetString("accountinfo_password_exceeded_attempts", new string[] { SiteSettings.Instance.ContactEmail });
                    }
                    else
                        lblPasswordMessage.Text = SueetieLocalizer.GetString("accountinfo_password_current_incorrect");
                }
            }
            catch (Exception exception)
            {
                lblPasswordMessage.Text = exception.Message;
            }

        }

        #endregion

        protected void btnUpdateBio_OnClick(object sender, EventArgs args)
        {
            SueetieUser sueetieUser = new SueetieUser
            {
                UserID = CurrentSueetieUser.UserID,
                Bio = txtBio.Text
            };


            SueetieUsers.ClearUserCache(CurrentSueetieUser.UserID);
            SueetieUsers.UpdateSueetieUserBio(sueetieUser);
            Response.Redirect("myaccountinfo.aspx?bio=1");

        }
    }
}

