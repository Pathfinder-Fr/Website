using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web 
{
    public partial class AdminCalendarScheduler : SueetieAdminPage
    {

        public string CalendarName
        {
            get { return ((string)ViewState["CalendarName"]) ?? string.Empty; }
            set { ViewState["CalendarName"] = value; }
        }

        public AdminCalendarScheduler()
            : base("admin_content_calendarscheduler")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int calendarID = DataHelper.GetIntFromQueryString("c", 1);
                this.CalendarName = SueetieCalendars.GetSueetieCalendar(calendarID).CalendarTitle;
                lblCalendar.Text = this.CalendarName;
            }
        }
    }
}
