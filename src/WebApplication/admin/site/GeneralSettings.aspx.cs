using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class GeneralSettings : SueetieAdminPage
    {

        public GeneralSettings()
            : base("admin_site_generalsettings")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSiteName.Text = SiteSettings.Instance.SiteName;
                chkCreateWikiAccount.Checked = SiteSettings.Instance.CreateWikiUserAccount;
                chkRecordAnalytics.Checked = SiteSettings.Instance.RecordAnalytics;
                txtGroupsFolderName.Text = SiteSettings.Instance.GroupsFolderName;
                txtIpGeoLookupUrl.Text = SiteSettings.Instance.IpGeoLookupUrl;

                rblRegistrationType.Items.Add(new ListItem("Automatic", "0"));
                rblRegistrationType.Items.Add(new ListItem("Email Verification", "1"));
                rblRegistrationType.Items.Add(new ListItem("Administrative Approval", "2"));
                rblRegistrationType.Items.Add(new ListItem("Closed", "3"));

                rblRegistrationType.Items.FindByValue(SiteSettings.Instance.RegistrationType.ToString()).Selected = true;
                lblVersion.Text = SiteStatistics.Instance.SueetieVersion;
                txtDefaultLanguage.Text = SiteSettings.Instance.DefaultLanguage;

                rblWwwSubdomain.SelectedValue = SiteSettings.Instance.HandleWwwSubdomain;

                SueetieUIHelper.PopulateTimeZoneList(ddTimeZones);
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            SueetieCommon.UpdateSiteSetting(new SiteSetting("SiteName", txtSiteName.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("RegistrationType", rblRegistrationType.SelectedValue));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("CreateWikiUserAccount", chkCreateWikiAccount.Checked.ToString()));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("GroupsFolderName", txtGroupsFolderName.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("DefaultLanguage", txtDefaultLanguage.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("SitePageTitleLead", txtSitePageTitleLead.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("DefaultTimeZone", ddTimeZones.SelectedValue));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("RecordAnalytics", chkRecordAnalytics.Checked.ToString()));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("HandleWwwSubdomain", rblWwwSubdomain.SelectedValue));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("IpGeoLookupUrl", txtIpGeoLookupUrl.Text));
            SueetieCommon.ClearSiteSettingsCache();

            lblResults.Visible = true;
            lblResults.Text = "Site settings have been updated!";
        }
    }
}