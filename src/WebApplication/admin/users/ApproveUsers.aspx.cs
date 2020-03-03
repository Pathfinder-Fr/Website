using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web 
{
    public partial class ApproveUsers : SueetieAdminPage
    {
        public ApproveUsers()
            : base("admin_users_approve")
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
            UsersGridView.DataSource = SueetieUsers.GetUnapprovedUserList();
            UsersGridView.DataBind();
        }

    }

}