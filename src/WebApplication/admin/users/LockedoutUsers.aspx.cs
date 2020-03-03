using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class LockedoutUsers : SueetieAdminPage
    {
        public LockedoutUsers()
            : base("admin_users_lockedout")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindUserAccounts();
        }

        private void BindUserAccounts()
        {
            MembershipUserCollection allUsers = Membership.GetAllUsers();
            MembershipUserCollection filteredUsers = new MembershipUserCollection();

            bool isLockedOut = true;
            foreach (MembershipUser user in allUsers)
            {
                if (user.IsLockedOut == isLockedOut)
                {
                    filteredUsers.Add(user);
                }
            }

            UsersGridView.DataSource = filteredUsers;
            UsersGridView.DataBind();
        }
    }
}