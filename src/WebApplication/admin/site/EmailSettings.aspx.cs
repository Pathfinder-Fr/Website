using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class EmailSettings : SueetieAdminPage
    {

        public EmailSettings()
            : base("admin_site_emailsettings")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtContactEmail.Text = SiteSettings.Instance.ContactEmail;
                txtFromEmail.Text = SiteSettings.Instance.FromEmail;
                txtFromName.Text = SiteSettings.Instance.FromName;
                txtSmtpServer.Text = SiteSettings.Instance.SmtpServer;
                txtSmtpUserName.Text = SiteSettings.Instance.SmtpUserName;
                txtSmtpPassword.Text = SiteSettings.Instance.SmtpPassword;
                txtSmtpServerPort.Text = SiteSettings.Instance.SmtpServerPort;
                chkEnableSSL.Checked = SiteSettings.Instance.EnableSSL;
                txtErrorEmails.Text = SiteSettings.Instance.ErrorEmails;

            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            SueetieCommon.UpdateSiteSetting(new SiteSetting("ContactEmail", txtContactEmail.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("FromEmail", txtFromEmail.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("FromName", txtFromName.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("SmtpServer", txtSmtpServer.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("SmtpServerPort", txtSmtpServerPort.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("SmtpUserName", txtSmtpUserName.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("SmtpPassword", txtSmtpPassword.Text));
            SueetieCommon.UpdateSiteSetting(new SiteSetting("ErrorEmails", txtErrorEmails.Text));

            SueetieCommon.UpdateSiteSetting(new SiteSetting("EnableSSL", chkEnableSSL.Checked.ToString()));

            SueetieCommon.ClearSiteSettingsCache();

            lblResults.Visible = true;
            lblResults.Text = "Email settings have been updated!";
        }
    }

}
