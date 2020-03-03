using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;

namespace Sueetie.Web
{
    public partial class OnlineUsers : SueetieAdminPage
    {
        public OnlineUsers()
            : base("admin_users_online")
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

            bool isOnline = true;
            foreach (MembershipUser user in allUsers)
            {
                if (user.IsOnline == isOnline)
                {
                    filteredUsers.Add(user);
                }
            }

            UsersGridView.DataSource = filteredUsers;
            UsersGridView.DataBind();
        }

    

    }
}
