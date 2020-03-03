using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminContentGroups : SueetieAdminPage
    {
        public AdminContentGroups()
            : base("admin_content_contentgroups")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetButtonState(true);
                ClearForm();
            }
        }

        protected void ddlContentGroups_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SueetieContentPageGroup _group =
SueetieContentParts.GetSueetieContentPageGroup(int.Parse(ddlContentGroups.SelectedValue));
            if (_group.ContentPageGroupID > 0)
            {
                txtTitle.Text = _group.ContentPageGroupTitle;
                lblApplicationKey.Text = _group.ApplicationKey;
                txtEditors.Text = _group.EditorRoles;
                chkActive.Checked = _group.IsActive;
                lblResults.Text = string.Empty;
                SetButtonState(false);
            }
            else
            {
               SetButtonState(true);
               ClearForm();
            }
        }

        private void SetButtonState(bool forNew)
        {
            tdApplicationKeyDropDown.Visible = forNew;
            tdApplicationKeyLabel.Visible = !forNew;

            if (forNew)
            {
                btnAddNew.Enabled = true;
                btnUpdate.Enabled = false;
                btnManage.Enabled = false;
            }
            else
            {
                btnAddNew.Enabled = false;
                btnUpdate.Enabled = true;
                btnManage.Enabled = true;
            }
        }

        protected void btnAddUpdate_OnCommand(object sender, CommandEventArgs e)
        {
            SueetieContentPageGroup _group = new SueetieContentPageGroup
            {
                ContentPageGroupTitle = txtTitle.Text,
                EditorRoles = txtEditors.Text,
                IsActive = chkActive.Checked
            };
            if (e.CommandName == "Add")
            {
                if (int.Parse(ddlContentGroups.SelectedValue) < 0)
                {
                    _group.ApplicationID = int.Parse(ddlApplicationKeys.SelectedValue);
                    SueetieContentParts.CreateContentPageGroup(_group);
                    lblResults.Text = "Content Page Group Created!";
                }
                else
                {
                    lblResults.Text = "Content Page Group dropdown selector must be empty when creating a content page group.";
                    return;
                }
            }
            else
            {
                _group.ContentPageGroupID = int.Parse(ddlContentGroups.SelectedValue);
                _group.IsActive = chkActive.Checked;

                SueetieContentParts.ClearSueetieContentPageListCache(int.Parse(ddlContentGroups.SelectedValue));
                SueetieContentParts.UpdateSueetieContentPageGroup(_group);
                lblResults.Text = "Content Page Group Updated!";
            }
            ClearForm();
        }

        private void ClearForm()
        {
            List<SueetieContentPageGroup> _groups = SueetieContentParts.GetSueetieContentPageGroupList();
            txtTitle.Text = string.Empty;
            lblApplicationKey.Text = string.Empty;
            txtEditors.Text = string.Empty;
            chkActive.Checked = true;


            ddlApplicationKeys.DataSource = SueetieContentParts.GetCmsApplicationList();
            ddlApplicationKeys.DataValueField = "ApplicationID";
            ddlApplicationKeys.DataTextField = "Description";
            ddlApplicationKeys.DataBind();

            ddlContentGroups.DataSource = _groups;
            ddlContentGroups.DataValueField = "ContentPageGroupID";
            ddlContentGroups.DataTextField = "ContentPageGroupTitle";

            ddlContentGroups.DataBind();
            ddlContentGroups.Items.Insert(0, new ListItem(string.Empty, "-1"));
            ddlContentGroups.Items.FindByValue("-1").Selected = true;

            if (int.Parse(ddlApplicationKeys.SelectedValue) == -1)
                btnAddNew.Enabled = false;
        }

        protected void btnManage_OnClick(object sender, EventArgs e)
        {
            if (int.Parse(ddlContentGroups.SelectedValue) > 0)
                Response.Redirect("contentpages.aspx?grp=" + ddlContentGroups.SelectedValue);
            else
                lblResults.Text = "Please select a Content Page Group.";
        }

    }
}