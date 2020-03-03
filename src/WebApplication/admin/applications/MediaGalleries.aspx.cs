using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminMediaGalleries : SueetieAdminPage
    {
        public AdminMediaGalleries()
            : base("admin_applications_mediagalleries")
        {
        }

        public int GalleryID
        {
            get { return ((int)ViewState["GalleryID"]) != 0 ? ((int)ViewState["GalleryID"]) : -1; }
            set { ViewState["GalleryID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ActivitiesGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState & DataControlRowState.Edit) > 0))
            {
                SueetieMediaGallery _SueetieMediaGallery = ((SueetieMediaGallery)e.Row.DataItem);

                DropDownList ddDisplayTypes = (DropDownList)e.Row.FindControl("ddDisplayTypes") as DropDownList;
                Array values = Enum.GetValues(typeof(SueetieDisplayType));
                foreach (SueetieDisplayType displayType in Enum.GetValues(typeof(SueetieDisplayType)))
                {
                    ddDisplayTypes.Items.Add(new ListItem(Enum.GetName(typeof(SueetieDisplayType), displayType), displayType.ToString("D")));
                }
                ddDisplayTypes.Items.FindByValue(_SueetieMediaGallery.DisplayTypeID.ToString()).Selected = true;
                ddDisplayTypes.DataBind();
                this.GalleryID = _SueetieMediaGallery.GalleryID;
            }

        }

        protected void ActivitiesGridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = ActivitiesGridView.Rows[e.RowIndex];
            SueetieMediaGallery _SueetieMediaGallery = ((SueetieMediaGallery)row.DataItem);
            DropDownList ddDisplayTypes = (DropDownList)row.FindControl("ddDisplayTypes") as DropDownList;
            e.NewValues.Remove("displayTypeID");
            e.NewValues.Add("displayTypeID", int.Parse(ddDisplayTypes.SelectedValue));

            CheckBox chkPublic = (CheckBox)row.FindControl("chkPublic") as CheckBox;
            CheckBox chkLogged = (CheckBox)row.FindControl("chkLogged") as CheckBox;

            e.NewValues.Add("isPublic", chkPublic.Checked);
            e.NewValues.Add("isLogged", chkLogged.Checked);


        }

    }
}
