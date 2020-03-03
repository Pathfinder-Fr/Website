using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public partial class ForumTopicListWebControl : SueetieBaseControl
    {
        protected PlaceHolder phForumTopics;

        public ForumTopicListWebControl()
        {
            NumRecords = 10;
            UserID = -2;
            GroupID = 0;
            IsRestricted = true;
            ViewName = "RecentForumTopicView";
            CacheMinutes = 5;
        }

        public int NumRecords { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public bool IsRestricted { get; set; }
        public string ViewName { get; set; }
        public int CacheMinutes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            string path = this.ViewName.ControlPath();

            ContentQuery contentQuery = new ContentQuery
            {
                NumRecords = this.NumRecords,
                UserID = this.UserID,
                ContentTypeID = (int)SueetieContentType.ForumTopic,
                GroupID = this.GroupID,
                IsRestricted = this.IsRestricted,
                TruncateText = false,
                CacheMinutes = this.CacheMinutes
            };

            List<SueetieForumTopic> sueetieForumTopics = SueetieForums.GetSueetieForumTopicList(contentQuery);

            foreach (SueetieForumTopic topic in sueetieForumTopics)
            {
                Sueetie.Controls.ForumTopicView control = (Sueetie.Controls.ForumTopicView)LoadControl(path);
                control.Topic = topic;
                phForumTopics.Controls.Add(control);
            }

        }

    }
}