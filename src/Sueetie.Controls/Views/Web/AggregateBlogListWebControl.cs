using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class AggregateBlogListWebControl : SueetieBaseControl
    {
        protected PlaceHolder phAggregatedBlogs;

        public AggregateBlogListWebControl()
        {
            NumRecords = 10;
            CategoryID = -1;
            GroupID = -1;
            IsRestricted = true;
            ViewName = "AggregateBlogView";
            CacheMinutes = 5;
        }

        public int NumRecords { get; set; }
        public int CategoryID { get; set; }
        public int GroupID { get; set; }
        public bool IsRestricted { get; set; }
        public string ViewName { get; set; }
        public int CacheMinutes { get; set; }
        public SueetieBlogSortType BlogSortType { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            string path = this.ViewName.ControlPath();

            ApplicationQuery applicationQuery = new ApplicationQuery
            {
                NumRecords = this.NumRecords,
                CategoryID = this.CategoryID,
                GroupID = this.GroupID,
                IsRestricted = this.IsRestricted,
                TruncateText = false,
                CacheMinutes = this.CacheMinutes,
                SueetieApplicationViewTypeID = (int)SueetieApplicationViewType.Blogs,
                SortBy = (int)this.BlogSortType
            };

            List<SueetieBlog> sueetieBlogs = SueetieBlogs.GetSueetieBlogList(applicationQuery);

            foreach (SueetieBlog blog in sueetieBlogs)
            {
                Sueetie.Controls.BlogView control = (Sueetie.Controls.BlogView)LoadControl(path);
                control.Blog = blog;
                phAggregatedBlogs.Controls.Add(control);
            }

        }


    }
}