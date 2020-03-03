using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminSeo : SueetieAdminPage
    {

        public AdminSeo()
            : base("admin_site_seo")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTrackingScript.Text = SiteSettings.Instance.TrackingScript;
                txtHtmlHeader.Text = SiteSettings.Instance.HtmlHeader;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            SiteSetting setting = new SiteSetting
            {
                SettingName = "HtmlHeader",
                SettingValue = txtHtmlHeader.Text
            };
            SueetieCommon.UpdateSiteSetting(setting);

            setting.SettingName = "TrackingScript";
            setting.SettingValue = txtTrackingScript.Text ?? string.Empty;
            SueetieCommon.UpdateSiteSetting(setting);

            lblResults.Visible = true;
            lblResults.Text = "Site Header and Tracking Script updated!";
        }
    }
}