using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminDownloadDetails : SueetieAdminPage
    {

        public AdminDownloadDetails()
            : base("admin_reports_downloaddetails")
        {
        }

        #region ContentQuery Properties

        public int GroupID
        {
            get { return ((int)ViewState["GroupID"]) != 0 ? ((int)ViewState["GroupID"]) : -1; }
            set { ViewState["GroupID"] = value; }
        }

        public int ApplicationID
        {
            get { return ((int)ViewState["ApplicationID"]); }
            set { ViewState["ApplicationID"] = value; }
        }

        public int MediaObjectID
        {
            get { return ((int)ViewState["MediaObjectID"]) != 0 ? ((int)ViewState["MediaObjectID"]) : -1; }
            set { ViewState["MediaObjectID"] = value; }

        }


        public int DownloadUserID
        {
            get { return ((int)ViewState["DownloadUserID"]); }
            set { ViewState["DownloadUserID"] = value; }
        }

        public int SortByID
        {
            get { return ((int)ViewState["SortByID"]); }
            set { ViewState["SortByID"] = value; }
        }

        #endregion

        #region Load and Bind
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ResetEventlogPageProperties();
                try
                {
                    this.MediaObjectID = int.Parse(Request.QueryString["moid"]);
                }
                catch
                {
                    this.MediaObjectID = -1;
                }
                BindUserAccounts();
            }
        }

        private List<SueetieDownload> SueetieDownloadList()
        {
            ContentQuery _contentQuery = new ContentQuery
            {
                ApplicationID = this.ApplicationID,
                SueetieContentViewTypeID = -1,
                NumRecords = 1000,
                UserID = this.DownloadUserID,
                GroupID = this.GroupID,
                SourceID = this.MediaObjectID,
                SortBy = (int)SueetieSortBy.MostRecentDateTimeDesc
            };

            List<SueetieDownload> _sueetieDownloads = SueetieMedia.GetSueetieDownloadList(_contentQuery);
            return _sueetieDownloads;
        }

        private void BindUserAccounts()
        {
            List<SueetieDownload> _sueetieDownloads = SueetieDownloadList();
            UsersGridView.DataSource = _sueetieDownloads.Skip(this.PageIndex * this.PageSize).Take(this.PageSize);
            UsersGridView.DataBind();

            int totalRecords = _sueetieDownloads.Count;
            // Enable/disable the pager buttons based on which page we're on
            bool visitingFirstPage = (this.PageIndex == 0);
            lnkFirst.Enabled = !visitingFirstPage;
            lnkPrev.Enabled = !visitingFirstPage;

            int lastPageIndex = (totalRecords - 1) / this.PageSize;
            bool visitingLastPage = (this.PageIndex >= lastPageIndex);
            lnkNext.Enabled = !visitingLastPage;
            lnkLast.Enabled = !visitingLastPage;
        }

        #endregion

        #region ContentQuery Property Handling

        private void ResetEventlogPageProperties()
        {
            this.PageIndex = 0;
            this.ApplicationID = -1;
            this.GroupID = -1;
            this.SortByID = -1;
            this.DownloadUserID = -1;
            this.MediaObjectID = -1;
        }

        protected void lbGroupID_OnCommand(object sender, CommandEventArgs e)
        {
            ResetEventlogPageProperties();
            this.GroupID = int.Parse(e.CommandArgument.ToString());
            BindUserAccounts();
        }
        protected void lbApplicationID_OnCommand(object sender, CommandEventArgs e)
        {
            ResetEventlogPageProperties();
            this.ApplicationID = int.Parse(e.CommandArgument.ToString());
            BindUserAccounts();
        }
        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            ResetEventlogPageProperties();
            this.MediaObjectID = int.Parse(Request.QueryString["moid"]);
            BindUserAccounts();
        }
        protected void btnAllDownloads_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("downloads.aspx");
        }
        protected void lbUserID_OnCommand(object sender, CommandEventArgs e)
        {
            ResetEventlogPageProperties();
            this.DownloadUserID = int.Parse(e.CommandArgument.ToString());
            BindUserAccounts();
        }
        protected void lbMediaObjectID_OnCommand(object sender, CommandEventArgs e)
        {
            ResetEventlogPageProperties();
            this.MediaObjectID = int.Parse(e.CommandArgument.ToString());
            BindUserAccounts();
        }

        #endregion

        #region Paging Interface Click Event Handlers

        // first pager link
        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            this.PageIndex = 0;
            BindUserAccounts();
        }

        // previous pager link
        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            this.PageIndex -= 1;
            BindUserAccounts();
        }

        // next pager link
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            this.PageIndex += 1;
            BindUserAccounts();
        }

        // last pager link
        protected void lnkLast_Click(object sender, EventArgs e)
        {

            this.PageIndex = (SueetieDownloadList().Count - 1) / this.PageSize;
            BindUserAccounts();
        }

        #endregion

        #region Page and Record Property Handling



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
                return 20;
            }
        }

        #endregion

    }
}
