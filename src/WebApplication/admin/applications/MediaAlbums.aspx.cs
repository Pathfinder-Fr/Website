using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminMediaAlbums : SueetieAdminPage
    {
        public AdminMediaAlbums()
            : base("admin_applications_mediaalbums")
        {
        }

        public int ContentID
        {
            get { return ((int)ViewState["ContentID"]) != 0 ? ((int)ViewState["ContentID"]) : -1; }
            set { ViewState["ContentID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ActivitiesGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState & DataControlRowState.Edit) > 0))
            {
                SueetieMediaAlbum _sueetieMediaAlbum = ((SueetieMediaAlbum)e.Row.DataItem);

                List<ContentTypeDescription> _contentTypeDescriptions = SueetieMedia.GetAlbumContentTypeDescriptionList();
                DropDownList ddContentTypes = (DropDownList)e.Row.FindControl("ddContentTypes") as DropDownList;
                foreach (ContentTypeDescription _contentTypeDescription in _contentTypeDescriptions)
                {
                    ddContentTypes.Items.Add(new ListItem(_contentTypeDescription.Description, _contentTypeDescription.ContentTypeID.ToString()));
                }
                ddContentTypes.Items.FindByValue(_sueetieMediaAlbum.ContentTypeID.ToString()).Selected = true;
                ddContentTypes.DataBind();
                this.ContentID = _sueetieMediaAlbum.ContentID;
            }

        }

        protected void ActivitiesGridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = ActivitiesGridView.Rows[e.RowIndex];
            SueetieMediaAlbum _sueetieMediaAlbum = ((SueetieMediaAlbum)row.DataItem);
            DropDownList ddContentTypes = (DropDownList)row.FindControl("ddContentTypes") as DropDownList;
            e.NewValues.Remove("contentTypeID");
            e.NewValues.Remove("contentID");
            e.NewValues.Add("contentTypeID", int.Parse(ddContentTypes.SelectedValue));
            e.NewValues.Add("contentID", this.ContentID);
        }

    }
}
