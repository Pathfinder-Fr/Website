using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class InactiveUsers : SueetieAdminPage
    {

        public InactiveUsers()
            : base("admin_users_inactive")
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
            UsersGridView.DataSource = SueetieUsers.GetInactiveUserList();
            UsersGridView.DataBind();
        }


    }
}
