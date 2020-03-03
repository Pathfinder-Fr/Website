using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class UserProfile : SueetieBaseThemedPage
    {

        public UserProfile()
            : base("members_profile")
        {
            this.SueetieMasterPage = "alternate.master";
        }

        public SueetieUser UserProfiled { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["u"] == null)
                Response.Redirect("Message.aspx");
            else
            {
                int userID = DataHelper.GetIntFromQueryString("u", -1);
                if (userID > 0)
                {
                    if (SueetieConfiguration.Get().Core.UseForumProfile)
                    {
                        int forumUserID = SueetieUsers.GetThinSueetieUser(userID).ForumUserID;
                        string profileUrl = SueetieUrls.Instance.MasterProfile(forumUserID).Url;
                        Response.Redirect(profileUrl);
                    }
                    UserProfiled = SueetieUsers.GetUser(int.Parse(Request.QueryString["u"].ToString()));
                    if (UserProfiled.IsAnonymous)
                        Response.Redirect("Message.aspx?msg=1");
                    else
                    {
                        if (UserProfiled.HasAvatarImage)
                            AvatarImg.ImageUrl = "/images/avatars/" + UserProfiled.AvatarFilename;
                        else
                            AvatarImg.ImageUrl = "/images/avatars/noavatar.jpg";
                        lblMemberSince.Text = UserProfiled.DisplayName + " has been a member of the Sueetie Community since " + UserProfiled.DateJoined.ToString("y");

                        lblBio.Text = DataHelper.DefaultTextIt(UserProfiled.Bio, UserProfiled.DisplayName + " has not yet created a bio.");
                        Page.Title = SueetieLocalizer.GetString("memberprofile_title") + UserProfiled.DisplayName;
                    }
                }
            }
        }


    }
}

