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
    public partial class SearchUsers : SueetieAdminPage
    {
        public SearchUsers()
            : base("admin_users_search")
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private MembershipUserCollection allRegisteredUsers = Membership.GetAllUsers();

        private void BindAllUsers(bool reloadAllUsers)
        {
            MembershipUserCollection allUsers = null;
            if (reloadAllUsers) allUsers = Membership.GetAllUsers();

            string searchText = "";
            if (!string.IsNullOrEmpty(UsersGridView.Attributes["SearchText"]))
                searchText = UsersGridView.Attributes["SearchText"];
            bool searchByEmail = false;
            if (!string.IsNullOrEmpty(UsersGridView.Attributes["SearchByEmail"]))
                searchByEmail = bool.Parse(UsersGridView.Attributes["SearchByEmail"]);
            if (searchText.Length > 0)
            {
                if (searchByEmail)
                    allUsers = Membership.FindUsersByEmail(searchText);
                else
                    allUsers = Membership.FindUsersByName(searchText);
            }
            else
            {
                allUsers = allRegisteredUsers;
            }
            UsersGridView.DataSource = allUsers;
            UsersGridView.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bool searchByEmail = (ddlUserSearchTypes.SelectedValue == "E-mail");
            UsersGridView.Attributes.Add("SearchText", "%" + txtSearchText.Text + "%");
            UsersGridView.Attributes.Add("SearchByEmail", searchByEmail.ToString());
            BindAllUsers(false);
        }


    }
}
