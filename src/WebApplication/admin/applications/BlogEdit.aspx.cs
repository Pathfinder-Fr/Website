using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class BlogEdit : SueetieAdminPage
    {
        public BlogEdit()
            : base("admin_applications_blogedit")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearForm();
            }
        }

        protected void ddlBlogs_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SueetieBlog _sueetieBlog = SueetieBlogs.GetSueetieBlog(int.Parse(ddlBlogs.SelectedValue));

            if (_sueetieBlog.BlogID > 0)
            {
                PopulateBlogOwnerRoleList(_sueetieBlog.BlogOwnerRole);
                txtBlogTitle.Text = _sueetieBlog.BlogTitle;
                txtBlogDescription.Text = _sueetieBlog.BlogDescription;
                txtBlogAccessRole.Text = _sueetieBlog.BlogAccessRole;
                chkIsActive.Checked = _sueetieBlog.IsActive;
                chkIncludeInAggregateList.Checked = _sueetieBlog.IncludeInAggregateList;
                chkRegisteredComments.Checked = _sueetieBlog.RegisteredComments;
                SetButtonState(false);
            }
            else
            {
                SetButtonState(true);
                ClearForm();
            }
            lblResults.Text = string.Empty;
        }

        public void PopulateBlogOwnerRoleList(string selectedValue)
        {
            ddlBlogOwnerRole.Items.Clear();
            foreach (SueetieRole role in SueetieRoles.GetSueetieBlogOwnerRoleList())
            {
                ddlBlogOwnerRole.Items.Add(new ListItem(role.RoleName, role.RoleName));
            }
            ddlBlogOwnerRole.Items.Insert(0, new ListItem(string.Empty, "-1"));
            if (!string.IsNullOrEmpty(selectedValue))
                ddlBlogOwnerRole.Items.FindByText(selectedValue).Selected = true;
            else
                ddlBlogOwnerRole.Items.FindByValue("-1").Selected = true;
        }


        private void SetButtonState(bool forNew)
        {
            if (forNew)
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                btnUpdate.Enabled = true;
            }
        }

        protected void btnAddUpdate_OnCommand(object sender, CommandEventArgs e)
        {
            SueetieBlog sueetieBlog = new SueetieBlog
            {
                BlogID = int.Parse(ddlBlogs.SelectedValue),
                BlogTitle = txtBlogTitle.Text,
                BlogDescription = txtBlogDescription.Text,
                IsActive = chkIsActive.Checked,
                IncludeInAggregateList = chkIncludeInAggregateList.Checked,
                RegisteredComments = chkRegisteredComments.Checked,
                BlogAccessRole = !string.IsNullOrEmpty(txtBlogAccessRole.Text) ? txtBlogAccessRole.Text : null,
                BlogOwnerRole = ddlBlogOwnerRole.SelectedItem.Text
            };

            SueetieBlogs.UpdateSueetieBlog(sueetieBlog);
            lblResults.Text = "Blog Updated!";

            ClearForm();
        }

        private void ClearForm()
        {
            List<SueetieBlog> sueetieBlogs = SueetieBlogs.GetSueetieBlogTitles();

            txtBlogTitle.Text = string.Empty;
            txtBlogDescription.Text = string.Empty;
            txtBlogAccessRole.Text = string.Empty;
            chkIsActive.Checked = true;
            chkIncludeInAggregateList.Checked = true;
            chkRegisteredComments.Checked = false;
            PopulateBlogOwnerRoleList(null);

            ddlBlogs.Items.Clear();
            foreach (SueetieBlog sueetieBlog in sueetieBlogs)
            {
                if (sueetieBlog.BlogID > 0)
                    ddlBlogs.Items.Add(new ListItem(sueetieBlog.BlogTitle, sueetieBlog.BlogID.ToString()));
            }
            ddlBlogs.Items.Insert(0, new ListItem(string.Empty, "-1"));
            ddlBlogs.Items.FindByValue("-1").Selected = true;
            SetButtonState(true);

        }

    }

}
