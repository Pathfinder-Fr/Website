using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class BannedIPs : SueetieAdminPage
    {
        public BannedIPs()
            : base("admin_users_bannedips")
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ActivitiesGridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = ActivitiesGridView.Rows[e.RowIndex];
            TextBox txtMask = (TextBox)row.FindControl("txtMask") as TextBox;
            e.NewValues.Remove("Mask");
            e.NewValues.Remove("BannedID");
            e.NewValues.Add("bannedID", e.Keys[0]);
            e.NewValues.Add("ip", txtMask.Text);

        }

    }
}