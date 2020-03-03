namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using Sueetie.Core;
    using Sueetie.Licensing;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MarketplaceDefaultAdminPage : SueetieAdminPage
    {
        protected Label lblLicenseInfo;
        protected Label lblLicenseType;
        protected Panel pnlMenu;
        protected HtmlGenericControl SueetieProductKeyLI;

        public MarketplaceDefaultAdminPage() : base("admin_marketplace_default")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                DataHelper.GetIntFromQueryString("l", -1);
                if (!LicensingCommon.IsValidLicense(SueetiePackageType.Marketplace))
                {
                    this.lblLicenseType.Text = SueetieLocalizer.GetLicensingString("license_marketplace_notvalid_title");
                    this.lblLicenseInfo.Text = SueetieLocalizer.GetLicensingString("license_marketplace_notvalid_message") + "&nbsp;" + SueetieLocalizer.GetLicensingString("license_visit_sueetiecom");
                }
                else if (LicensingCommon.IsInTrialPeriod(SueetiePackageType.Marketplace))
                {
                    this.lblLicenseType.Text = SueetieLocalizer.GetLicensingString("license_marketplace_trialperiod_title");
                    this.lblLicenseInfo.Text = SueetieLocalizer.GetLicensingString("license_marketplace_trialperiod_message") + "&nbsp;" + SueetieLocalizer.GetLicensingString("license_visit_sueetiecom");
                }
                else if (LicensingCommon.IsFreeLicense(SueetiePackageType.Marketplace))
                {
                    this.lblLicenseType.Text = SueetieLocalizer.GetLicensingString("license_marketplace_free_title");
                    this.lblLicenseInfo.Text = SueetieLocalizer.GetLicensingString("license_marketplace_free_message") + "&nbsp;" + SueetieLocalizer.GetLicensingString("license_visit_sueetiecom");
                }
                else if (LicensingCommon.IsCommercialLicense(SueetiePackageType.Marketplace))
                {
                    this.lblLicenseType.Text = string.Format(SueetieLocalizer.GetLicensingString("license_marketplace_paid_title"), LicenseHelper.LicenseTypeDisplay(LicensingCommon.GetSueetieLicenseType(SueetiePackageType.Marketplace)));
                    this.lblLicenseInfo.Text = SueetieLocalizer.GetLicensingString("license_marketplace_paid_message");
                }
                this.ProcessHyperlinks(LicensingCommon.IsValidLicense(SueetiePackageType.Marketplace));
            }
        }

        private void ProcessHyperlinks(bool isEnabled)
        {
            foreach (Control control in this.pnlMenu.Controls)
            {
                if ((control is HyperLink) && isEnabled)
                {
                    string str = control.ID.Substring(2) + ".aspx";
                    ((HyperLink) control).NavigateUrl = str;
                }
                if ((control is HyperLink) && !isEnabled)
                {
                    ((HyperLink) control).NavigateUrl = "default.aspx?v=1";
                }
            }
            if (!CommerceCommon.IsSueetiePackageDistributor())
            {
                this.SueetieProductKeyLI.Visible = false;
            }
        }
    }
}

