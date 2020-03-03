using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminBackgroundTasks : SueetieAdminPage
    {
        public AdminBackgroundTasks()
            : base("admin_reports_backgroundtasks")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUserAccounts();
            }
        }

        private void BindUserAccounts()
        {
            UsersGridView.DataSource = SueetieTaskScheduler.Instance().Tasks;
            UsersGridView.DataBind();
        }

    }
}
