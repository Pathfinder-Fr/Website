using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.ServiceModel.Syndication;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class SiteActivityRSS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ContentQuery contentQuery = new ContentQuery
            {
                NumRecords = 100,
                GroupID = 0,
                CacheMinutes = 5,
                SueetieContentViewTypeID = (int)SueetieContentViewType.SyndicatedUserLogActivityList
            };

            List<UserLogActivity> userLogActivityList = SueetieLogs.GetUserLogActivityList(contentQuery);
            var dataItems = from activity in userLogActivityList
                            orderby activity.DateTimeActivity descending
                            select activity;

            const int maxItemsInFeed = 25;

            // Determine whether we're outputting an Atom or RSS feed
            bool outputAtom = (Request.QueryString["Type"] == "ATOM");
            bool outputRss = !outputAtom;

            if (outputRss)
                Response.ContentType = "application/rss+xml";
            else if (outputAtom)
                Response.ContentType = "application/atom+xml";

            // Create the feed and specify the feed's attributes
            SyndicationFeed myFeed = new SyndicationFeed();
            myFeed.Title = TextSyndicationContent.CreatePlaintextContent("Most Recent Site Activity on " + SiteSettings.Instance.SiteName);
            myFeed.Description = TextSyndicationContent.CreatePlaintextContent("A syndication of the most recent site activity on " + SiteSettings.Instance.SiteName);
            myFeed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetFullyQualifiedUrl("~/Default.aspx"))));
            myFeed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(GetFullyQualifiedUrl(Request.RawUrl))));
            myFeed.Copyright = TextSyndicationContent.CreatePlaintextContent("Copyright " + SiteSettings.Instance.SiteName);
            myFeed.Language = "en-us";

            List<SyndicationItem> feedItems = new List<SyndicationItem>();

            foreach (UserLogActivity ula in dataItems.Take(maxItemsInFeed))
            {
                if (outputAtom && ula.DisplayName == null)
                    continue;

                SyndicationItem item = new SyndicationItem();
                string applicationName = ula.ApplicationName;
                if (string.IsNullOrEmpty(applicationName))
                    applicationName = "Member Services";
                item.Title = TextSyndicationContent.CreatePlaintextContent(applicationName);
                string permalink = ula.Permalink;
                if (string.IsNullOrEmpty(permalink))
                    permalink = string.Format("/members/profile.aspx?u={0}", ula.UserID);
                item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetFullyQualifiedUrl(permalink))));
                item.Summary = TextSyndicationContent.CreatePlaintextContent(ula.Activity);
                item.Categories.Add(new SyndicationCategory(applicationName));
                item.Id = GetFullyQualifiedUrl(permalink);
                item.PublishDate = ula.DateTimeActivity;

                SyndicationPerson authInfo = new SyndicationPerson();
                authInfo.Email = ula.Email;
                authInfo.Name = ula.DisplayName;
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
