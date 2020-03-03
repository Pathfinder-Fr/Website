using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Sueetie.Core;
using Sueetie.Blog;
using BlogEngine.Core;

namespace BlogEngine.Themes.vTwo
{

    public partial class contact : SueetieBaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Sueetie Modified - Adding Compressed theme/style.css and Sueetie Theme stylesheets

                Page.Header.Controls.Add(SueetieBlogUtils.AddBlogEngineThemeCSS());
                Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/shared.css"));
                Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/blogs.css"));
                Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/contact.css"));
                Page.Header.Controls.Add(new LiteralControl("<!--[if IE]><link rel=\"stylesheet\" href=\"/themes/" + this.CurrentTheme + "/style/ie.css\" type=\"text/css\" /><![endif]-->"));


                MembershipUser user = Membership.GetUser();

                UserLink.Text = "Welcome, Guest!";
                UserLink.NavigateUrl = "/members/login.aspx";
                if (Page.User.Identity.IsAuthenticated)
                {
                    string _displayName = SueetieUsers.GetUserDisplayName(SueetieContext.Current.User.UserID, false);

                    if (SueetieConfiguration.Get().Core.UseForumProfile)
                        UserLink.NavigateUrl = SueetieUrls.Instance.MasterAccountInfo().Url;
                    else
                        UserLink.NavigateUrl = SueetieUrls.Instance.MyAccountInfo().Url;

                    UserLink.Text = string.Format(SueetieLocalizer.GetString("link_greetings_member"), _displayName);

                }

            }
        }

    }

}