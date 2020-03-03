using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminContentPages : SueetieAdminPage
    {
        public AdminContentPages()
            : base("admin_content_contentpages")
        {
        }

        public int GroupID
        {
            get { return ((int)ViewState["GroupID"]); }
            set { ViewState["GroupID"] = value; }
        }

        public SueetieContentPage _contentPage
        {
            get { return ((SueetieContentPage)ViewState["ContentPage"]); }
            set { ViewState["ContentPage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GroupID = DataHelper.GetIntFromQueryString("grp", -2);
                ClearForm();
            }
        }

        protected void ddlContentPages_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this._contentPage = SueetieContentParts.GetSueetieContentPage(int.Parse(ddlContentPages.SelectedValue));
            if (_contentPage.ContentPageID > 0)
            {
                txtPageTitle.Text = _contentPage.PageTitle;
                txtPageKey.Text = _contentPage.PageKey;
                txtPageSlug.Text = _contentPage.PageSlug;
                txtDescription.Text = _contentPage.PageDescription;
                txtReaders.Text = _contentPage.ReaderRoles;
                txtDisplayOrder.Text = _contentPage.DisplayOrder.ToString();
                chkActive.Checked = _contentPage.IsPublished;
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
            if (forNew)
            {
                btnAddNew.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnAddNew.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        protected void btnAddUpdate_OnCommand(object sender, CommandEventArgs e)
        {
            SueetieContentPageGroup _group = SueetieContentParts.GetSueetieContentPageGroup(this.GroupID);
            SueetieContentPage _page = new SueetieContentPage
            {
                PageKey = txtPageKey.Text.Trim(),
                PageTitle = txtPageTitle.Text,
                PageDescription = txtDescription.Text,
                ReaderRoles = txtReaders.Text,
                LastUpdateUserID = CurrentSueetieUserID,
                IsPublished = chkActive.Checked,
                DisplayOrder = DataHelper.IntOrDefault(txtDisplayOrder.Text.Trim(),-1),
                PageSlug = string.IsNullOrEmpty(txtPageSlug.Text) ? SueetieContentParts.CreatePageSlug(txtPageTitle.Text) : SueetieContentParts.CreatePageSlug(txtPageSlug.Text),
                ContentPageGroupID = this.GroupID
            };
            if (e.CommandName == "Add")
            {
                if (int.Parse(ddlContentPages.SelectedValue) < 0)
                {
                    int _contentPageID = SueetieContentParts.CreateContentPage(_page);
                    SueetieContent sueetieContent = new SueetieContent
                    {
                        ApplicationID = _group.ApplicationID,
                        ContentTypeID = (int)SueetieContentType.CMSPage,
                        SourceID = _contentPageID,
                        UserID = CurrentSueetieUserID,
                        IsRestricted = !string.IsNullOrEmpty(txtReaders.Text),
                        Permalink = "/" + _group.ApplicationKey + "/" + _page.PageSlug + ".aspx"
                    };
                    int contentID = SueetieCommon.AddSueetieContent(sueetieContent);
                    SueetieLogs.LogUserEntry(UserLogCategoryType.CMSPageCreated, contentID, CurrentSueetieUserID);
                    lblResults.Text = "Content Page Created!";
                }
                else
                {
                    lblResults.Text = "Content Page dropdown selector must be empty when creating a content page.";
                    return;
                }
            }
            else if (e.CommandName == "Update")
            {
                _page.ContentPageID = int.Parse(ddlContentPages.SelectedValue);

                SueetieContentParts.UpdateSueetieContentPage(_page);
                _page.Permalink = "/" + _group.ApplicationKey + "/" + _page.PageSlug + ".aspx";
                SueetieContentParts.UpdateCmsPermalink(_page);
                SueetieCommon.UpdateSueetieContentIsRestricted(_page.ContentID, !string.IsNullOrEmpty(txtReaders.Text));

                SueetieCommon.ClearUserLogActivityListCache((int)SueetieContentViewType.Unassigned);
                SueetieCommon.ClearUserLogActivityListCache((int)SueetieContentViewType.SyndicatedUserLogActivityList);

                lblResults.Text = "Content Page Updated!";
                SueetieContentParts.ClearSueetieContentPageCache(int.Parse(ddlContentPages.SelectedValue));

            }
            else
            {
                SueetieContentParts.DeleteContentPage(int.Parse(ddlContentPages.SelectedValue));
            }
            SueetieContentParts.ClearSueetieContentPageListCache(this.GroupID);
            SueetieContentParts.ClearSueetieContentPageListCache(-1); // Clear All Pages for ContentPartView Control
            ClearForm();
        }

        private void ClearForm()
        {
         
            List<SueetieContentPage> _pages = (from p in SueetieContentParts.GetSueetieContentPageList(this.GroupID)
                                               orderby p.DisplayOrder ascending
                                               select p).ToList();

            txtPageTitle.Text = string.Empty;
            txtPageKey.Text = string.Empty;
            txtReaders.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtPageSlug.Text = string.Empty;
            txtDisplayOrder.Text = string.Empty;
            chkActive.Checked = true;

            ddlContentPages.DataSource = _pages;
            ddlContentPages.DataValueField = "ContentPageID";
            ddlContentPages.DataTextField = "PageTitle";

            ddlContentPages.DataBind();
            ddlContentPages.Items.Insert(0, new ListItem(string.Empty, "-1"));
            ddlContentPages.Items.FindByValue("-1").Selected = true;

            SetButtonState(true);

        }

    }
}