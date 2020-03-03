using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web 
{
    public partial class AdminDefault : SueetieAdminPage
    {
        public AdminDefault()
            : base("admin_default")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                lblVersion.Text = SiteStatistics.Instance.SueetieVersion;
        }
    }
}
