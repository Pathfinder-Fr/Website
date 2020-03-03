using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class RestartApp : SueetieAdminPage
    {

        public RestartApp()
            : base("admin_site_restartapp")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            lblResults.Visible = true;
            if (SueetieUIHelper.GetCurrentTrustLevel() >= AspNetHostingPermissionLevel.High)
            {
                HttpRuntime.UnloadAppDomain();
                lblResults.Text = "Sueetie restarted.  The cache is clear.  Surface!  Surface!";
            }
            else
            {
                SueetieLogs.LogException("Unable to restart Sueetie. Must have High/Unrestricted Trust to Unload Application.");
                lblResults.Text = "Unable to restart Sueetie. Must have High/Unrestricted Trust to Unload Application.";
            }
           
           
        }
    }

}
