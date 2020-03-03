using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel.Syndication;
using System.Xml;
using Sueetie.Core;

namespace Sueetie.Web 
{
    public partial class BlogsRSS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContentQuery contentQuery = new ContentQuery
            {
                ContentTypeID = (int)SueetieContentType.BlogPost,
                CacheMinutes = 5,
                SueetieContentViewTypeID = (int)SueetieContentViewType.AggregateBlogPostList
            };
            List<SueetieBlogPost> sueetieBlogPosts = SueetieBlogs.GetSueetieBlogPostList(contentQuery);
            var dataItems = from post in sueetieBlogPosts
                            orderby post.DateCreated descending
                            select post;

            const int maxItemsInFeed = 10;

            // Determine whether we're outputting an Atom or RSS feed
            bool outputAtom = (Request.QueryString["Type"] == "ATOM");
            bool outputRss = !outputAtom;

            if (outputRss)
                Response.ContentType = "application/rss+xml";
            else if (outputAtom)
                Response.ContentType = "application/atom+xml";

            // Create the feed and specify the feed's attributes
            SyndicationFeed myFeed = new SyndicationFeed();
            myFeed.Title = TextSyndicationContent.CreatePlaintextContent("Most Recent Posts on " + SiteSettings.Instance.SiteName);
            myFeed.Description = TextSyndicationContent.CreatePlaintextContent("A syndication of the most recently published posts on " + SiteSettings.Instance.SiteName);
            myFeed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetFullyQualifiedUrl("~/Default.aspx"))));
            myFeed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(GetFullyQualifiedUrl(Request.RawUrl))));
            myFeed.Copyright = TextSyndicationContent.CreatePlaintextContent("Copyright " + SiteSettings.Instance.SiteName);
            myFeed.Language = "en-us";

            List<SyndicationItem> feedItems = new List<SyndicationItem>();

            foreach (SueetieBlogPost p in dataItems.Take(maxItemsInFeed))
            {
                if (outputAtom && p.Author == null)
                    continue;

                SyndicationItem item = new SyndicationItem();
                item.Title = TextSyndicationContent.CreatePlaintextContent(p.BlogTitle + " - " + p.Title);
                item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetFullyQualifiedUrl(p.Permalink))));
                item.Summary = TextSyndicationContent.CreateHtmlContent(p.PostContent);
                item.Categories.Add(new SyndicationCategory(p.BlogTitle));
                item.PublishDate = p.DateCreated;
                item.Id = GetFullyQualifiedUrl(p.Permalink);
                SyndicationPerson authInfo = new SyndicationPerson();
                authInfo.Email = p.Email;
                authInfo.Name = p.DisplayName;
                item.Authors.Add(authInfo);

                feedItems.Add(item);
            }

            myFeed.Items = feedItems;

            XmlWriterSettings outputSettings = new XmlWriterSettings();
            outputSettings.Indent = true;
            XmlWriter feedWriter = XmlWriter.Create(Response.OutputStream, outputSettings);

            if (outputAtom)
            {
                Atom10FeedFormatter atomFormatter = new Atom10FeedFormatter(myFeed);
                atomFormatter.WriteTo(feedWriter);
            }
            else if (outputRss)
            {
                Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(myFeed);
                rssFormatter.WriteTo(feedWriter);
            }

            feedWriter.Close();
        }

        private string GetFullyQualifiedUrl(string url)
        {
            return string.Concat(Request.Url.GetLeftPart(UriPartial.Authority), ResolveUrl(url));
        }
    }

}
