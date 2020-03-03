using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using Sueetie.Search;

namespace Sueetie.Web
{
    public partial class SearchAddon : SueetieAdminPage
    {

        public SearchAddon()
            : base("admin_content_search")
        {
        }

        private SiteSettings _siteSettings;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblResultsDetails.Visible = false;
                lblResults.Visible = false;
                txtStartDate.Text = string.Empty;
            }
        }

        protected void btnRebuildIndex_OnClick(object sender, EventArgs e)
        {
            SueetieSearch search = new SueetieSearch();
            string _startdate = "6/9/1969";
            if (!string.IsNullOrEmpty(txtStartDate.Text))
                _startdate = txtStartDate.Text.Trim();
            search.AsyncUpdateIndex(Convert.ToDateTime(_startdate));
            lblResults.Visible = true;
            lblResultsDetails.Visible = false;
            lblResults.Text = "Your site content is being indexed on a background application thread. The results will display in the Sueetie Event Log.";
        }

        protected void btnShowStats_OnClick(object sender, EventArgs e)
        {
            lblResultsDetails.Visible = true;
            lblResults.Visible = false;
            SueetieSearch search = new SueetieSearch();
            lblResultsDetails.Text =
                "Indexed Blog Posts: " + search.GetIndexedCountForApplicationTypeID((int)SueetieApplicationType.Blog).ToString() + "<br />" +
                "Indexed Forum Messages: " + search.GetIndexedCountForApplicationTypeID((int)SueetieApplicationType.Forum).ToString() + "<br />" +
                "Indexed Media Objects: " + search.GetIndexedCountForApplicationTypeID((int)SueetieApplicationType.MediaGallery).ToString() + "<br />" +
                "Indexed Wiki Pages: " + search.GetIndexedCountForApplicationTypeID((int)SueetieApplicationType.Wiki).ToString() + "<br />" +
                "Total Items Indexed: " + search.GetTotalIndexedCount().ToString();
        }

    }

}
