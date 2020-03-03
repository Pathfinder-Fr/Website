using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.Security;
using System.Data.Linq;


namespace Sueetie.Web
{
    public partial class NewUsers : SueetieAdminPage
    {
        public NewUsers()
            : base("admin_users_new")
        {
        }

        #region Gridview and a-z navigation

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            btnDeleteSelected.Visible = UsersGridView.Rows.Count > 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindUserAccounts();
                BindAtoZNavigation();
            }
        }

        private void BindAtoZNavigation()
        {
            string[] AtoZfilterOptions = { "All", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            AtoZRepeater.DataSource = AtoZfilterOptions;
            AtoZRepeater.DataBind();
        }

        private void BindUserAccounts()
        {
            MembershipUserCollection allUsers = new MembershipUserCollection();
            if (UsernameToMatch == string.Empty)
            {
                allUsers = Membership.GetAllUsers();
            }
            else if (this.UsernameToMatch == "All")
            {
                this.PageIndex = 0;
                allUsers = Membership.GetAllUsers();
            }
            else
                allUsers = Membership.FindUsersByName(this.UsernameToMatch + "%");


            int totalRecords = 0;
            MembershipUserCollection filteredUsers = new MembershipUserCollection();
            foreach (MembershipUser user in allUsers)
            {
                if (user.IsApproved == true)
                {
                    filteredUsers.Add(user);
                    totalRecords++;
                }
            }
            List<MembershipUser> users = new List<MembershipUser>();
            foreach (MembershipUser user in filteredUsers)
            {
                users.Add(user);
            }

            List<MembershipUser> _sortedUsers = users.OrderByDescending(x => x.CreationDate).ToList();

            if (UsernameToMatch == string.Empty)
                UsersGridView.DataSource = _sortedUsers.Skip(this.PageIndex * this.PageSize).Take(this.PageSize);
            else
                UsersGridView.DataSource = _sortedUsers;
            UsersGridView.DataBind();

            bool visitingFirstPage = (this.PageIndex == 0);
            lnkFirst.Enabled = !visitingFirstPage;
            lnkPrev.Enabled = !visitingFirstPage;

            int lastPageIndex = (totalRecords - 1) / this.PageSize;
            bool visitingLastPage = (this.PageIndex >= lastPageIndex);
            lnkNext.Enabled = !visitingLastPage;
            lnkLast.Enabled = !visitingLastPage;
        }

        protected void AtoZRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "All")
            {
                this.PageIndex = 0;
                this.UsernameToMatch = string.Empty;
            }
            else
                this.UsernameToMatch = e.CommandName;

            BindUserAccounts();
        }

        #endregion

        #region Paging Interface Click Event Handlers

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
            MembershipUserCollection allUsers = new MembershipUserCollection();
            if (UsernameToMatch == string.Empty)
            {
                allUsers = Membership.GetAllUsers();
            }
            else
            {
                allUsers = Membership.FindUsersByName(this.UsernameToMatch + "%");
            }


            int totalRecords = 0;
            MembershipUserCollection filteredUsers = new MembershipUserCollection();
            foreach (MembershipUser user in allUsers)
            {
                if (user.IsApproved == true)
                {
                    filteredUsers.Add(user);
                    totalRecords++;
                }
            }

            this.PageIndex = (totalRecords - 1) / this.PageSize;
            BindUserAccounts();
        }

        #endregion

        #region Properties

        private string UsernameToMatch
        {
            get
            {
                object o = ViewState["UsernameToMatch"];
                if (o == null)
                    return string.Empty;
                else
                    return (string)o;
            }
            set
            {
                ViewState["UsernameToMatch"] = value;
            }
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

        #region Member delete one user click event handlers

        protected void UserAccounts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string username = UsersGridView.DataKeys[e.RowIndex].Value.ToString();
            //ProfileManager.DeleteProfile(userName);
            //Membership.DeleteUser(userName);

            MembershipUser u = Membership.GetUser(username);
            u.IsApproved = false;
            Membership.UpdateUser(u);

            //Response.Redirect("Default.aspx");
            // lblDeleteSuccess.Visible = true;
        }

        #endregion

        #region Delete users selected by checkbox

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in UsersGridView.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("chkRows");
                if (cb != null && cb.Checked)
                {
                    string username = UsersGridView.DataKeys[row.RowIndex].Value.ToString();
                    MembershipUser u = Membership.GetUser(username);
                    u.IsApproved = false;
                    Membership.UpdateUser(u);
                    SueetieUsers.DeactivateUser(username);

                    lblDeleteSuccess.Visible = true;
                }
            }
            Response.Redirect("NewUsers.aspx");
        }

        #endregion

    }
}