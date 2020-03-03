using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class BlogCommentListWebControl : SueetieBaseControl
    {

        protected PlaceHolder phBlogComments;

        public BlogCommentListWebControl()
        {
            NumRecords = 10;
            UserID = -2;
            GroupID = -1;
            ApplicationID = -1;
            IsRestricted = true;
            ViewName = "RecentBlogCommentView";
            CacheMinutes = 5;
        }

        public int NumRecords { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public int ApplicationID { get; set; }
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
                ContentTypeID = (int)SueetieContentType.BlogComment,
                GroupID = this.GroupID,
                ApplicationID = this.ApplicationID,
                IsRestricted = this.IsRestricted,
                TruncateText = false,
                CacheMinutes = this.CacheMinutes
            };

            List<SueetieBlogComment> sueetieBlogComments = SueetieBlogs.GetSueetieBlogCommentList(contentQuery);

            foreach (SueetieBlogComment comment in sueetieBlogComments)
            {
                Sueetie.Controls.BlogCommentView control = (Sueetie.Controls.BlogCommentView)LoadControl(path);
                control.Comment = comment;
                phBlogComments.Controls.Add(control);
            }

        }


    }
}