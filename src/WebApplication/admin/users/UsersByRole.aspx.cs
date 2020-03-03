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
    public partial class UsersByRole : SueetieAdminPage
    {
        public UsersByRole()
            : base("admin_users_byrole")
        {
        }
        #region Gridview Users

        private void Page_Init()
        {
            if (!Page.IsPostBack)
            {
                UserRoles.DataSource = Roles.GetAllRoles();
                UserRoles.DataBind();
            }
        }

        private void Page_PreRender()
        {
            BindUserAccounts();
        }

        protected void UserRoles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PageIndex = 0;
        }

        private void BindUserAccounts()
        {
            int totalRecords;
            MembershipUserCollection allUsers = Membership.GetAllUsers(this.PageIndex, this.PageSize, out totalRecords);
            if (UserRoles.SelectedIndex > 0)
                allUsers = Membership.GetAllUsers();

            MembershipUserCollection filteredUsers = new MembershipUserCollection();

            if (UserRoles.SelectedIndex > 0)
            {
                string[] usersInRole = Roles.GetUsersInRole(UserRoles.SelectedValue);
                foreach (MembershipUser user in allUsers)
                {
                    foreach (string userInRole in usersInRole)
                    {
                        if (userInRole == user.UserName && user.IsApproved)
                        {
                            filteredUsers.Add(user);

                            break;
                        }
                    }
                }
            }
            else
            {
                filteredUsers = allUsers;
            }

            bool visitingFirstPage = (this.PageIndex == 0);
            lnkFirst.Enabled = !visitingFirstPage;
            lnkPrev.Enabled = !visitingFirstPage;

            int lastPageIndex = (totalRecords - 1) / this.PageSize;
            bool visitingLastPage = (this.PageIndex >= lastPageIndex);
            lnkNext.Enabled = !visitingLastPage;
            lnkLast.Enabled = !visitingLastPage;

            UsersGridView.DataSource = filteredUsers;
            UsersGridView.DataBind();
        }

        #endregion

        #region Paging

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            this.PageIndex = 0;
            BindUserAccounts();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            this.PageIndex -= 1;
            BindUserAccounts();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            this.PageIndex += 1;
            BindUserAccounts();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            // Determine the total number of records
            int totalRecords;
            Membership.GetAllUsers(this.PageIndex, this.PageSize, out totalRecords);

            this.PageIndex = (totalRecords - 1) / this.PageSize;
            BindUserAccounts();
        }

        private int PageIndex
        {
            get
            {
                object o = ViewState["PageIndex"];
                if (o == null)
                    return 0;
                else
                    return (int)o;
            }
            set
            {
                ViewState["PageIndex"] = value;
            }
        }

        private int PageSize
        {
            get
            {
                return 25;
            }
        }

        #endregion


    }
}