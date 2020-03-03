using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using Sueetie.Blog;
using Sueetie.Wiki;

namespace Sueetie.Web
{
    public partial class AdminThemes : SueetieAdminPage
    {
        public AdminThemes()
            : base("admin_site_themes")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTheme.Text = SiteSettings.Instance.Theme;
                txtMobileTheme.Text = SiteSettings.Instance.MobileTheme;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            string _theme = txtTheme.Text.ToLower().Trim();
            string _mobileTheme = txtMobileTheme.Text.ToLower().Trim();

            // Update Application and Group Themes
            // Each theme has to have an associated blog theme

            foreach (SueetieApplication _sueetieApplication in SueetieApplications.Get().All)
            {
                if (_sueetieApplication.ApplicationTypeID == (int)SueetieApplicationType.Blog && _sueetieApplication.GroupID == 0)
                    try
                    {
                        SueetieBlogUtils.UpdateBlogTheme(_sueetieApplication.ApplicationKey, _theme);
                    }
                    catch (Exception ex)
                    {
                        SueetieLogs.LogException("Blog Theme Update Error: " + ex.Message + " Stacktrace: " + ex.StackTrace);
                    }
            }
            SueetieForums.UpdateForumTheme(_theme);
            WikiThemes.UpdateWikiTheme(SueetieApplications.Get().Wiki.ApplicationKey, _theme);

            //// Update BlogEngine Group Themes - Will add to SueetieApplications logic
            // SueetieBlogUtils.UpdateBlogTheme("groups/demo/blog", _theme);
            // WikiThemes.UpdateWikiTheme("groups/demo/wiki", _theme);

            SueetieCommon.UpdateSiteSetting(new SiteSetting("Theme", _theme));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("MobileTheme", _mobileTheme));

            if (SueetieUIHelper.GetCurrentTrustLevel() >= AspNetHostingPermissionLevel.High)
            {
                HttpRuntime.UnloadAppDomain();
            }
            else
            {
                SueetieLogs.LogException("Unable to restart Sueetie. Must have High/Unrestricted Trust to Unload Application.");
            }

            lblResults.Visible = true;
            lblResults.Text = "Current Themes updated!";
            lblResultsDetails.Visible = true;
            lblResultsDetails.Text = "New application themes may not appear right away. Touch web.configs to restart the app if this is the case.";
        }
    }

}
