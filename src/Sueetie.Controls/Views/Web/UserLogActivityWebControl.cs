using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class UserLogActivityWebControl : SueetieBaseControl
    {

        protected PlaceHolder phUserLogActivity;

        public UserLogActivityWebControl()
        {
            NumRecords = 100;
            UserID = -2;
            GroupID = -1;
            ApplicationID = -1;
            IsRestricted = true;
            ViewName = "RecentUserLogActivityView";
            CacheMinutes = 5;
            HeaderViewName = "RecentUserLogActivityHeader";
        }

        public int NumRecords { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public int ApplicationID { get; set; }
        public bool IsRestricted { get; set; }
        public string ViewName { get; set; }
        public string HeaderViewName { get; set; }
        public int CacheMinutes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            string path = this.ViewName.ControlPath();
            string headerpath = this.HeaderViewName.ControlPath();

            ContentQuery contentQuery = new ContentQuery
            {
                NumRecords = this.NumRecords,
                UserID = this.UserID,
                GroupID = this.GroupID,
                ApplicationID = this.ApplicationID,
                IsRestricted = this.IsRestricted,
                TruncateText = false,
                CacheMinutes = this.CacheMinutes
            };

            List<UserLogActivity> userLogActivityList = SueetieLogs.GetUserLogActivityList(contentQuery);

            foreach (UserLogActivity userLogActivity in userLogActivityList)
            {
                if (userLogActivity.ShowHeader)
                {
                    Sueetie.Controls.UserLogActivityView headercontrol = (Sueetie.Controls.UserLogActivityView)LoadControl(headerpath);
                    headercontrol.LogActivity = userLogActivity;
                    phUserLogActivity.Controls.Add(headercontrol);
                }

                Sueetie.Controls.UserLogActivityView control = (Sueetie.Controls.UserLogActivityView)LoadControl(path);
                control.LogActivity = userLogActivity;
                phUserLogActivity.Controls.Add(control);
            }

        }

    }
}