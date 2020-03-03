using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class AdminEventLogs : SueetieAdminPage
    {
        public int EventLogItems { get; set; }
        public AdminEventLogs()
            : base("admin_reports_eventlogs")
        {
        }
        #region EventLog Properties

        public int SiteLogTypeID
        {
            get { return ((int)ViewState["SiteLogTypeID"]) != 0 ? ((int)ViewState["SiteLogTypeID"]) : -1; }
            set { ViewState["SiteLogTypeID"] = value; }
        }
        public int SiteLogCategoryID
        {
            get { return ((int)ViewState["SiteLogCategoryID"]) != 0 ? ((int)ViewState["SiteLogCategoryID"]) : -1; }
            set { ViewState["SiteLogCategoryID"] = value; }
        }
        public int ApplicationID
        {
            get { return ((int)ViewState["ApplicationID"]); }
            set { ViewState["ApplicationID"] = value; }
        }

        #endregion

        #region Load and Bind
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                EventLogItems = SueetieLogs.GetEventLogCount();
                ResetEventlogPageProperties();
                PopulateSiteLogTypeList(ddlSiteLogTypeIDs);
                BindUserAccounts();
            }
        }

        private List<SiteLogEntry> SiteLogEntryList()
        {
            SiteLogEntry siteLogEntry = new SiteLogEntry
            {
                ApplicationID = this.ApplicationID,
                SiteLogTypeID = this.SiteLogTypeID,
                SiteLogCategoryID = this.SiteLogCategoryID
            };

            List<SiteLogEntry> siteLog = SueetieCommon.GetSiteLogEntryList(siteLogEntry);
            return siteLog;
        }

        private void BindUserAccounts()
        {
            List<SiteLogEntry> siteLog = SiteLogEntryList();
            UsersGridView.DataSource = siteLog.Skip(this.PageIndex * this.PageSize).Take(this.PageSize);
            UsersGridView.DataBind();

            int totalRecords = siteLog.Count;
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

        #region Eventlog Property Handling

        private void ResetEventlogPageProperties()
        {
            this.PageIndex = 0;
            this.ApplicationID = 0;
            this.SiteLogTypeID = -1;
            this.SiteLogCategoryID = -1;
        }

        private void PopulateSiteLogTypeList(DropDownList list)
        {
            string selectedValue = SiteLogTypeID.ToString();

            list.Items.Clear();
            List<SiteLogEntry> siteLogEntry = SueetieCommon.GetSiteLogTypeList();
            foreach (SiteLogEntry _siteLogEntry in siteLogEntry)
            {
                list.Items.Add(new ListItem(DataHelper.AllCategoriesIt(_siteLogEntry.SiteLogTypeCode), _siteLogEntry.SiteLogTypeID.ToString()));
            }

            if (list.Items.FindByValue(selectedValue) != null)
                list.SelectedValue = selectedValue;
        }
        protected void ddlSiteLogTypeIDs_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            ResetEventlogPageProperties();
            this.SiteLogTypeID = int.Parse(ddlSiteLogTypeIDs.SelectedValue);
            BindUserAccounts();
        }
        protected void lbSiteLogCategoryID_OnCommand(object sender, CommandEventArgs e)
        {
            ResetEventlogPageProperties();
            PopulateSiteLogTypeList(ddlSiteLogTypeIDs);
            this.SiteLogCategoryID = int.Parse(e.CommandArgument.ToString());
            BindUserAccounts();
        }
        protected void lbApplicationID_OnCommand(object sender, CommandEventArgs e)
        {
            ResetEventlogPageProperties();
            PopulateSiteLogTypeList(ddlSiteLogTypeIDs);
            this.ApplicationID = int.Parse(e.CommandArgument.ToString());
            BindUserAccounts();
        }
        protected void btnRefresh_OnClick(object sender, EventArgs e)
        {
            ResetEventlogPageProperties();
            PopulateSiteLogTypeList(ddlSiteLogTypeIDs);
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

            this.PageIndex = (SiteLogEntryList().Count - 1) / this.PageSize;
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

        #region Clear Log
        protected void lbtnClearLog_OnClick(object sender, EventArgs e)
        {
            SueetieLogs.ClearEventLog();
            Response.Redirect("eventlogs.aspx?r=1");
        }
        #endregion
    }
}
