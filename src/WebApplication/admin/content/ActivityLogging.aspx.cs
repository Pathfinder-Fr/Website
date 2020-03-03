using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminActivityLogging : SueetieAdminPage
    {
        public AdminActivityLogging()
            : base("admin_content_activitylogging")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ActivitiesGridView_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = ActivitiesGridView.Rows[e.RowIndex];
            UserLogCategory userLogCategory = new UserLogCategory
            {
                IsDisplayed = ((CheckBox)(row.Cells[4].Controls[1])).Checked,
                IsSyndicated = ((CheckBox)(row.Cells[5].Controls[1])).Checked
            };
            //e.NewValues.Remove("UserLogCategoryDescription");
            //e.NewValues.Remove("UserLogCategoryCode");
            e.NewValues.Add("isDisplayed", userLogCategory.IsDisplayed);
            e.NewValues.Add("isSyndicated", userLogCategory.IsSyndicated);
        }

        protected void ActivitiesDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            UserLogCategory userLogCategory = new UserLogCategory();
            if (e.Values["UserLogCategoryCode"] == null)
            {
                userLogCategory.IsDisplayed = false;
                userLogCategory.IsSyndicated = false;
                userLogCategory.UserLogCategoryCode = "Error";
                userLogCategory.UserLogCategoryDescription = "Bad entry.  All fields are required. Delete and re-enter.";
                userLogCategory.UserLogCategoryID = -1;
            }
            else
            {
                userLogCategory.IsDisplayed = bool.Parse(e.Values["IsDisplayed"].ToString());
                userLogCategory.IsSyndicated = bool.Parse(e.Values["IsSyndicated"].ToString());
                userLogCategory.UserLogCategoryCode = e.Values["UserLogCategoryCode"] as string;
                userLogCategory.UserLogCategoryDescription = e.Values["UserLogCategoryDescription"] as string;
                userLogCategory.UserLogCategoryID = int.Parse(e.Values["UserLogCategoryID"] as string);
            }

            e.Values["catCode"] = userLogCategory.UserLogCategoryCode;
            e.Values["catDesc"] = userLogCategory.UserLogCategoryDescription;
            e.Values.Add("isDisplayed", userLogCategory.IsDisplayed);
            e.Values.Add("isSyndicated", userLogCategory.IsSyndicated);
            e.Values.Add("catID", userLogCategory.UserLogCategoryID);
            e.Values.Remove("UserLogCategoryID");
            e.Values.Remove("UserLogCategoryCode");
            e.Values.Remove("UserLogCategoryDescription");

        }

    }
}