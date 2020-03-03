using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Data;

namespace Sueetie.Web
{
    public partial class AdminUserActivities : SueetieAdminPage
    {
        public AdminUserActivities()
            : base("admin_reports_useractivities")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ActivitiesDataSource.SelectParameters["showAll"].DbType = DbType.Boolean;
            //ActivitiesDataSource.SelectParameters["showAll"].DefaultValue = "1";
        }

        protected void ActivityGrid_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["showAll"] = true;
        }

    }
}