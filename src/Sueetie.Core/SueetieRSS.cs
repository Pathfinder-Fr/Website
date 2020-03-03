// -----------------------------------------------------------------------
// <copyright file="SueetieRSS.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RssToolkit;

    public static class SueetieRSS
    {
        public static List<SueetieRssBlogPost> GetSueetieRssBlogPostList(ContentQuery contentQuery)
        {
            var key = SueetieRssBlogPostListCacheKey(contentQuery.FeedUrl);

            var sueetieRssBlogPosts = SueetieCache.Current[key] as List<SueetieRssBlogPost>;
            if (sueetieRssBlogPosts == null)
            {
                var sueetieRssBlogChannel = SueetieRssBlogChannel.LoadChannel(contentQuery.FeedUrl);
                List<SueetieRssBlogItem> _rssItems = sueetieRssBlogChannel.Items;
                sueetieRssBlogPosts = ConvertBlogPosts(_rssItems);
                SueetieCache.Current.InsertMinutes(key, sueetieRssBlogPosts, contentQuery.CacheMinutes);
            }
            return sueetieRssBlogPosts.Take(contentQuery.NumRecords).ToList();
        }

        public static string SueetieRssBlogPostListCacheKey(string UserName)
        {
            return string.Format("SueetieRssBlogPostList-{0}", UserName);
        }

        public static void ClearSueetieRssBlogPostListCache(string UserName)
        {
            SueetieCache.Current.Remove(SueetieRssBlogPostListCacheKey(UserName));
        }

        private static List<SueetieRssBlogPost> ConvertBlogPosts(List<SueetieRssBlogItem> _rssItems)
        {
            var _rssBlogPosts = new List<SueetieRssBlogPost>();
            foreach (var _rssItem in _rssItems)
            {
                var _sueetieRssBlogPost = new SueetieRssBlogPost();
                _sueetieRssBlogPost.Title = _rssItem.Title;
                _sueetieRssBlogPost.Body = _rssItem.Description;
                _sueetieRssBlogPost.Link = _rssItem.Link;
                _sueetieRssBlogPost.Author = _rssItem.Author;
                _sueetieRssBlogPost.CommentsUrl = _rssItem.Comments;
                _sueetieRssBlogPost.ItemGuid = _rssItem.Guid;
                _sueetieRssBlogPost.PubDate = DateTime.Parse(_rssItem.PubDate);
                _sueetieRssBlogPost.Category = _rssItem.Category;
                _sueetieRssBlogPost.Publisher = _rssItem.DcPublisher;
                _sueetieRssBlogPost.CommentCount = _rssItem.SlashComments;
                _sueetieRssBlogPost.CommentRSS = _rssItem.WfwCommentRss;
                _sueetieRssBlogPost.Excerpt = DataHelper.StripHtml(_rssItem.Description);
                _rssBlogPosts.Add(_sueetieRssBlogPost);
            }
            return _rssBlogPosts;
        }
    }

    public class SueetieRssBlogItem : RssElementBase
    {
        public SueetieRssBlogItem()
        {
        }

        public SueetieRssBlogItem(string title, string description, string link, string author, string comments, string guid, string pubDate, string category, string dcPublisher, string pingbackServer, string pingbackTarget, string slashComments, string trackbackPing, string wfwComment, string wfwCommentRss)
        {
            base.SetAttributeValue("title", title);
            base.SetAttributeValue("description", description);
            base.SetAttributeValue("link", link);
            base.SetAttributeValue("author", author);
            base.SetAttributeValue("comments", comments);
            base.SetAttributeValue("guid", guid);
            base.SetAttributeValue("pubDate", pubDate);
            base.SetAttributeValue("category", category);
            base.SetAttributeValue("dc:publisher", dcPublisher);
            base.SetAttributeValue("pingback:server", pingbackServer);
            base.SetAttributeValue("pingback:target", pingbackTarget);
            base.SetAttributeValue("slash:comments", slashComments);
            base.SetAttributeValue("trackback:ping", trackbackPing);
            base.SetAttributeValue("wfw:comment", wfwComment);
            base.SetAttributeValue("wfw:commentRss", wfwCommentRss);
        }

        public string Title
        {
            get { return base.GetAttributeValue("title"); }
            set { base.SetAttributeValue("title", value); }
        }

        public string Description
        {
            get { return base.GetAttributeValue("description"); }
            set { base.SetAttributeValue("description", value); }
        }

        public string Link
        {
            get { return base.GetAttributeValue("link"); }
            set { base.SetAttributeValue("link", value); }
        }

        public string Author
        {
            get { return base.GetAttributeValue("author"); }
            set { base.SetAttributeValue("author", value); }
        }

        public string Comments
        {
            get { return base.GetAttributeValue("comments"); }
            set { base.SetAttributeValue("comments", value); }
        }

        public string Guid
        {
            get { return base.GetAttributeValue("guid"); }
            set { base.SetAttributeValue("guid", value); }
        }

        public string PubDate
        {
            get { return base.GetAttributeValue("pubDate"); }
            set { base.SetAttributeValue("pubDate", value); }
        }

        public string Category
        {
            get { return base.GetAttributeValue("category"); }
            set { base.SetAttributeValue("category", value); }
        }

        public string DcPublisher
        {
            get { return base.GetAttributeValue("dc:publisher"); }
            set { base.SetAttributeValue("dc:publisher", value); }
        }

        public string PingbackServer
        {
            get { return base.GetAttributeValue("pingback:server"); }
            set { base.SetAttributeValue("pingback:server", value); }
        }

        public string PingbackTarget
        {
            get { return base.GetAttributeValue("pingback:target"); }
            set { base.SetAttributeValue("pingback:target", value); }
        }

        public string SlashComments
        {
            get { return base.GetAttributeValue("slash:comments"); }
            set { base.SetAttributeValue("slash:comments", value); }
        }

        public string TrackbackPing
        {
            get { return base.GetAttributeValue("trackback:ping"); }
            set { base.SetAttributeValue("trackback:ping", value); }
        }

        public string WfwComment
        {
            get { return base.GetAttributeValue("wfw:comment"); }
            set { base.SetAttributeValue("wfw:comment", value); }
        }

        public string WfwCommentRss
        {
            get { return base.GetAttributeValue("wfw:commentRss"); }
            set { base.SetAttributeValue("wfw:commentRss", value); }
        }
    }

    public class SueetieRssBlogImage : RssElementBase
    {
        public override void SetDefaults()
        {
        }
    }

    public class SueetieRssBlogChannel : RssChannelBase<SueetieRssBlogItem, SueetieRssBlogImage>
    {
        public string Title
        {
            get { return base.GetAttributeValue("title"); }
            set { base.SetAttributeValue("title", value); }
        }

        public string Description
        {
            get { return base.GetAttributeValue("description"); }
            set { base.SetAttributeValue("description", value); }
        }

        public string Link
        {
            get { return base.GetAttributeValue("link"); }
            set { base.SetAttributeValue("link", value); }
        }

        public string Docs
        {
            get { return base.GetAttributeValue("docs"); }
            set { base.SetAttributeValue("docs", value); }
        }

        public string Generator
        {
            get { return base.GetAttributeValue("generator"); }
            set { base.SetAttributeValue("generator", value); }
        }

        public string Language
        {
            get { return base.GetAttributeValue("language"); }
            set { base.SetAttributeValue("language", value); }
        }

        public string BlogChannelBlogRoll
        {
            get { return base.GetAttributeValue("blogChannel:blogRoll"); }
            set { base.SetAttributeValue("blogChannel:blogRoll", value); }
        }

        public string BlogChannelBlink
        {
            get { return base.GetAttributeValue("blogChannel:blink"); }
            set { base.SetAttributeValue("blogChannel:blink", value); }
        }

        public string DcCreator
        {
            get { return base.GetAttributeValue("dc:creator"); }
            set { base.SetAttributeValue("dc:creator", value); }
        }

        public string DcTitle
        {
            get { return base.GetAttributeValue("dc:title"); }
            set { base.SetAttributeValue("dc:title", value); }
        }

        public string GeoLat
        {
            get { return base.GetAttributeValue("geo:lat"); }
            set { base.SetAttributeValue("geo:lat", value); }
        }

        public string GeoLong
        {
            get { return base.GetAttributeValue("geo:long"); }
            set { base.SetAttributeValue("geo:long", value); }
        }

        public SueetieRssBlogImage Image
        {
            get { return base.GetImage(); }
        }

        public override void SetDefaults()
        {
            base.SetAttributeValue("title", "Sueetie");
            base.SetAttributeValue("description", "News and Insider Info");
            base.SetAttributeValue("link", "http://sueetie.com/blog/");
            base.SetAttributeValue("docs", "http://www.rssboard.org/rss-specification");
            base.SetAttributeValue("generator", "BlogEngine.NET 1.6.0.0");
            base.SetAttributeValue("language", "en-US");
            base.SetAttributeValue("blogChannel:blogRoll", "http://sueetie.com/blog/opml.axd");
            base.SetAttributeValue("blogChannel:blink", "http://www.dotnetblogengine.net/syndication.axd");
            base.SetAttributeValue("dc:creator", "Dave Burke");
            base.SetAttributeValue("dc:title", "Sueetie");
            base.SetAttributeValue("geo:lat", "0.000000");
            base.SetAttributeValue("geo:long", "0.000000");
            this.Image.SetDefaults();
        }

        public static SueetieRssBlogChannel LoadChannel()
        {
            SueetieRssBlogChannel channel;
            channel = new SueetieRssBlogChannel();
            channel.LoadFromUrl(FeedUrl);
            return channel;
        }

        public static SueetieRssBlogChannel LoadChannel(string _url)
        {
            FeedUrl = _url;
            SueetieRssBlogChannel channel;
            channel = new SueetieRssBlogChannel();
            channel.LoadFromUrl(_url);
            return channel;
        }

        public static List<SueetieRssBlogItem> LoadChannelItems()
        {
            return LoadChannel().Items;
        }

        private static string _feedUrl = "http://sueetie.com/blog/syndication.axd";

        public static string FeedUrl
        {
            get { return _feedUrl; }
            set { _feedUrl = value; }
        }
    }

    public class SueetieRssBlogHttpHandlerBase : RssHttpHandlerBase<SueetieRssBlogChannel, SueetieRssBlogItem, SueetieRssBlogImage>
    {
    }
}