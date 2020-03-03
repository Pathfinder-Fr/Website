using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class BlogRssPostListWebControl : SueetieBaseControl
    {
        protected PlaceHolder phBlogRssPosts;

        public BlogRssPostListWebControl()
        {
            NumRecords = 10;
            ViewName = "RssBlogPostView";
            CacheMinutes = 5;
            FeedUrl = "http://sueetie.com/blog/syndication.axd";
        }

        public int NumRecords { get; set; }
        public string ViewName { get; set; }
        public int CacheMinutes { get; set; }
        public string FeedUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            ContentQuery contentQuery = new ContentQuery
            {
                NumRecords = this.NumRecords,
                FeedUrl = this.FeedUrl,
                CacheMinutes = this.CacheMinutes
            };

            string path = this.ViewName.ControlPath();

            List<SueetieRssBlogPost> sueetieRssBlogPosts = new List<SueetieRssBlogPost>();

            try
            {
                sueetieRssBlogPosts = SueetieRSS.GetSueetieRssBlogPostList(contentQuery);
            }
            catch
            {
                SueetieRssBlogPost _emptyPost = new SueetieRssBlogPost
                {
                    Title = "Feed Unavailable",
                    Excerpt = "You may not be currently connected to the Internet",
                    PubDate = DateTime.Now
                };
                sueetieRssBlogPosts.Add(_emptyPost);
            }

            foreach (SueetieRssBlogPost sueetieRssBlogPost in sueetieRssBlogPosts)
            {
                Sueetie.Controls.BlogRssPostView control = (Sueetie.Controls.BlogRssPostView)LoadControl(path);
                control.Post = sueetieRssBlogPost;
                phBlogRssPosts.Controls.Add(control);
            }

        }

    }
}