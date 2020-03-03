namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using System;
    using System.Web.UI.WebControls;

    public class SettingsMarketplacePage : MarketplaceAdminPage
    {
        protected Label lblResults;
        protected Label lblVersion;
        protected Button SubmitButton;
        protected TextBox txtActivityReportNum;
        protected TextBox txtFixedMediumImageHeight;
        protected TextBox txtFixedMediumImageWidth;
        protected TextBox txtFixedSmallImageHeight;
        protected TextBox txtFixedSmallImageWidth;
        protected TextBox txtMaxFullImageSize;

        public SettingsMarketplacePage() : base("admin_marketplace_settings")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.txtActivityReportNum.Text = CommerceSettings.Instance.ActivityReportNum.ToString();
                this.txtMaxFullImageSize.Text = CommerceSettings.Instance.MaxFullImageSize.ToString();
                this.txtFixedMediumImageHeight.Text = CommerceSettings.Instance.FixedMediumImageHeight.ToString();
                this.txtFixedMediumImageWidth.Text = CommerceSettings.Instance.FixedMediumImageWidth.ToString();
                this.txtFixedSmallImageHeight.Text = CommerceSettings.Instance.FixedSmallImageHeight.ToString();
                this.txtFixedSmallImageWidth.Text = CommerceSettings.Instance.FixedSmallImageWidth.ToString();
                this.lblVersion.Text = CommerceStatistics.Instance.CommerceVersion;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            CommerceCommon.UpdateCommerceSetting(new CommerceSetting("ActivityReportNum", this.txtActivityReportNum.Text));
            CommerceCommon.UpdateCommerceSetting(new CommerceSetting("MaxFullImageSize", this.txtMaxFullImageSize.Text));
            CommerceCommon.UpdateCommerceSetting(new CommerceSetting("FixedMediumImageHeight", this.txtFixedMediumImageHeight.Text));
            CommerceCommon.UpdateCommerceSetting(new CommerceSetting("FixedMediumImageWidth", this.txtFixedMediumImageWidth.Text));
            CommerceCommon.UpdateCommerceSetting(new CommerceSetting("FixedSmallImageHeight", this.txtFixedSmallImageHeight.Text));
            CommerceCommon.UpdateCommerceSetting(new CommerceSetting("FixedSmallImageWidth", this.txtFixedSmallImageWidth.Text));
            CommerceCommon.ClearCommerceSettingsCache();
            this.lblResults.Visible = true;
            this.lblResults.Text = "Marketplace settings have been updated!";
        }
    }
}

