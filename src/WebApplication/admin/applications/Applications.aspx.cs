using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminApplications : SueetieAdminPage
    {
        public AdminApplications()
            : base("admin_applications_applications")
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void UpdateActivitiesDetailsView_OnDataBound(object sender, EventArgs e)
        {
            DropDownList ddApplicationDetailsTypes = UpdateActivitiesDetailsView.FindControl("ddApplicationDetailsTypes") as DropDownList;
            Array values = Enum.GetValues(typeof(SueetieApplicationType));
            foreach (SueetieApplicationType appType in Enum.GetValues(typeof(SueetieApplicationType)))
            {
                ddApplicationDetailsTypes.Items.Add(new ListItem(Enum.GetName(typeof(SueetieApplicationType), appType), appType.ToString("D")));
            }
            ddApplicationDetailsTypes.Items.FindByValue("0").Selected = true;
            //ddApplicationDetailsTypes.DataBind();

            List<SueetieGroup> sueetieGroups = SueetieCommon.GetSueetieGroupList();
            DropDownList ddGroups = UpdateActivitiesDetailsView.FindControl("ddGroupsDetails") as DropDownList;
            foreach (SueetieGroup sueetieGroup in sueetieGroups)
            {
                ddGroups.Items.Add(new ListItem(sueetieGroup.GroupName, sueetieGroup.GroupID.ToString()));
            }
            ddGroups.Items.FindByValue("0").Selected = true;
            //ddGroups.DataBind();

        }

        protected void ActivitiesGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState & DataControlRowState.Edit) > 0))
            {
                SueetieApplication sueetieApplication = ((SueetieApplication)e.Row.DataItem);

                DropDownList ddApplicationTypes = (DropDownList)e.Row.FindControl("ddApplicationTypes") as DropDownList;
                Array values = Enum.GetValues(typeof(SueetieApplicationType));
                foreach (SueetieApplicationType appType in Enum.GetValues(typeof(SueetieApplicationType)))
                {
                    ddApplicationTypes.Items.Add(new ListItem(Enum.GetName(typeof(SueetieApplicationType), appType), appType.ToString("D")));
                }
                ddApplicationTypes.Items.FindByValue(sueetieApplication.ApplicationTypeID.ToString()).Selected = true;
                ddApplicationTypes.DataBind();

                List<SueetieGroup> sueetieGroups = SueetieCommon.GetSueetieGroupList();
                DropDownList ddGroups = (DropDownList)e.Row.FindControl("ddGroups") as DropDownList;
                foreach (SueetieGroup sueetieGroup in sueetieGroups)
                {
                    ddGroups.Items.Add(new ListItem(sueetieGroup.GroupName, sueetieGroup.GroupID.ToString()));
                }
                ddGroups.Items.FindByValue(sueetieApplication.GroupID.ToString()).Selected = true;
                ddGroups.DataBind();
            }
        }

        protected void ActivitiesGridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = ActivitiesGridView.Rows[e.RowIndex];
            DropDownList ddApplicationTypes = ((DropDownList)row.FindControl("ddApplicationTypes")) as DropDownList;
            DropDownList ddGroups = (DropDownList)row.FindControl("ddGroups") as DropDownList;
            SueetieApplication sueetieApplication = new SueetieApplication
            {
                ApplicationKey = ((TextBox)(row.Cells[2].Controls[1])).Text,
                Description = ((TextBox)(row.Cells[3].Controls[1])).Text,
                IsActive = ((CheckBox)(row.Cells[6].Controls[1])).Checked,
                GroupID = int.Parse(ddGroups.SelectedValue),
                ApplicationTypeID = int.Parse(ddApplicationTypes.SelectedValue)
            };

            e.NewValues.Remove("GroupID");
            e.NewValues.Remove("Description");
            e.NewValues.Remove("ApplicationTypeID");
            e.NewValues.Remove("ApplicationKey");
            e.NewValues.Add("appKey", sueetieApplication.ApplicationKey);
            e.NewValues.Add("appDescription", sueetieApplication.Description);
            e.NewValues.Add("isActive", sueetieApplication.IsActive);
            e.NewValues.Add("groupID", sueetieApplication.GroupID);
            e.NewValues.Add("appTypeID", sueetieApplication.ApplicationTypeID);
        }

        protected void ActivitiesDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            DetailsView vw = (DetailsView)sender;
            DropDownList ddApplicationDetailsTypes = ((DropDownList)vw.FindControl("ddApplicationDetailsTypes")) as DropDownList;
            DropDownList ddGroupsDetails = (DropDownList)vw.FindControl("ddGroupsDetails") as DropDownList;

            SueetieApplication sueetieApplication = new SueetieApplication();

            if (e.Values["ApplicationKey"] == null || e.Values["ApplicationID"] == null)
            {
                sueetieApplication.ApplicationKey = "ERROR";
                sueetieApplication.Description = "All fields required";
            }
            else
            {
                sueetieApplication.ApplicationID = int.Parse(e.Values["ApplicationID"].ToString().Trim());
                sueetieApplication.ApplicationKey = e.Values["ApplicationKey"].ToString();
                sueetieApplication.Description = e.Values["Description"].ToString();
                sueetieApplication.GroupID = int.Parse(ddGroupsDetails.SelectedValue);
                sueetieApplication.ApplicationTypeID = int.Parse(ddApplicationDetailsTypes.SelectedValue);
            }


            e.Values.Remove("GroupID");
            e.Values.Remove("Description");
            e.Values.Remove("ApplicationTypeID");
            e.Values.Remove("ApplicationKey");
            e.Values.Add("appKey", sueetieApplication.ApplicationKey);
            e.Values.Add("appDescription", sueetieApplication.Description);
            e.Values.Add("groupID", sueetieApplication.GroupID);
            e.Values.Add("appTypeID", sueetieApplication.ApplicationTypeID);

        }

    }
}
