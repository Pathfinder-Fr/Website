using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Sueetie.Core;
using System.Web;

namespace Sueetie.Controls
{
    /// <summary>
    /// Shows content based on UserRole
    /// </summary>
    public class UserAvatar : SueetieBaseControl
    {

        #region Properties

        
        public virtual int BorderWidth
        {
            get { return (int)(ViewState["BorderWidth"] ?? 0); }
            set { ViewState["BorderWidth"] = value; }
        }
        public virtual int Height
        {
            get { return (int)(ViewState["Height"] ?? SueetieConfiguration.Get().AvatarSettings.ThumbnailHeight); }
            set { ViewState["Height"] = value; }
        }
        public virtual int Width
        {
            get { return (int)(ViewState["Width"] ?? SueetieConfiguration.Get().AvatarSettings.ThumbnailWidth); }
            set { ViewState["Width"] = value; }
        }
        public string CssClass
        {
            get { return ((string)ViewState["CssClass"]) ?? string.Empty; }
            set { ViewState["CssClass"] = value; }
        }
        public bool UseCachedAvatarRoot
        {
            get { return (this.ViewState["UseCachedAvatarRoot"] == null) ? true : (bool)this.ViewState["UseCachedAvatarRoot"]; }
            set { ViewState["UseCachedAvatarRoot"] = value; }
        }
        public bool UseOriginalImage
        {
            get { return (this.ViewState["UseOriginalImage"] == null) ? false : (bool)this.ViewState["UseOriginalImage"]; }
            set { ViewState["UseOriginalImage"] = value; }
        }
        public bool PostBackToProfile
        {
            get { return (this.ViewState["PostBackToProfile"] == null) ? true : (bool)this.ViewState["PostBackToProfile"]; }
            set { ViewState["PostBackToProfile"] = value; }
        }

        public SueetieUser AvatarSueetieUser { get; set; }

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            Image _avatarImage = new Image();

            if (AvatarSueetieUser == null)
                AvatarSueetieUser = CurrentSueetieUser;

            if (AvatarSueetieUser != null)
            {

                _avatarImage.ImageUrl = SueetieUsers.GetUserAvatarUrl(AvatarSueetieUser.UserID, this.UseOriginalImage, this.UseCachedAvatarRoot);
                _avatarImage.AlternateText = AvatarSueetieUser.DisplayName;

                string _profileUrl = SueetieUrls.Instance.MasterProfile(AvatarSueetieUser.ForumUserID).Url;
                if (PostBackToProfile)
                {
                    if (!SueetieConfiguration.Get().Core.UseForumProfile)
                       _profileUrl = SueetieUrls.Instance.MyProfile(AvatarSueetieUser.UserID).Url;
                _avatarImage.Attributes.Add("onClick", "javascript:window.open('" + _profileUrl + "','_self')");
                }
            }
            else
            {
                _avatarImage.ImageUrl = SueetieUsers.GetUserAvatarUrl(-2, this.UseOriginalImage);
            }
            _avatarImage.Style.Add("height", this.Height.ToString() + "px");
            _avatarImage.Style.Add("width", this.Width.ToString() + "px");
            _avatarImage.Style.Add("border-width", this.BorderWidth.ToString() + "px");
            _avatarImage.CssClass = this.CssClass;


            Controls.Add(_avatarImage);
        }



    }


}

