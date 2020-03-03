using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using Sueetie.Core;
using BlogEngine.Core;
using Sueetie.Blog;

namespace BlogEngine.Themes.Lollipop
{
    public partial class Site : SueetieBaseMasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Page.Header.Controls.Add(SueetieBlogUtils.AddBlogEngineThemeCSS());
                Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/shared.css"));
                Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/blogs.css"));
                Page.Header.Controls.Add(new LiteralControl("<!--[if IE]><link rel=\"stylesheet\" href=\"/themes/" + this.CurrentTheme + "/style/ie.css\" type=\"text/css\" /><![endif]-->"));

                MembershipUser user = Membership.GetUser();

                BlogAdminLink.NavigateUrl = string.Format("{0}admin/dashboard.aspx", Utils.RelativeWebRoot);
                if (!Page.User.IsInRole("BlogAdministrator"))
                {
                    BlogAdminLI.Visible = false;
                    BlogAdminLink.Visible = false;
                }
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