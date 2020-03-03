// -----------------------------------------------------------------------
// <copyright file="SueetieUrls.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Xml.Linq;

    public class SueetieUrl
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Roles { get; set; }
        public string RewrittenUrl { get; set; }
        public string ContentUrl { get; set; }
        public string Pattern { get; set; }
    }

    public class SueetieUrls
    {
        private static List<SueetieUrl> _urls = new List<SueetieUrl>();
        private static readonly string urlCacheKey = SueetieConfiguration.Get().Core.SiteUniqueName + "-SueetieUrls";
        private static readonly object urlLocker = new object();

        public SueetieUrl Home()
        {
            return this.GetSueetieUrl("home");
        }

        public SueetieUrl Contact()
        {
            return this.GetSueetieUrl("contact_us");
        }

        public SueetieUrl SearchHome()
        {
            return this.GetSueetieUrl("search_home");
        }

        public SueetieUrl Login()
        {
            return this.GetSueetieUrl("members_login");
        }

        public SueetieUrl Logout()
        {
            return this.GetSueetieUrl("members_logout");
        }

        public SueetieUrl MyAccountInfo()
        {
            return this.GetSueetieUrl("members_myaccountinfo", new[] { SueetieContext.Current.User.UserID.ToString() });
        }

        public SueetieUrl Register()
        {
            return this.GetSueetieUrl("members_register");
        }

        public SueetieUrl MasterAccountInfo()
        {
            return this.GetSueetieUrl("members_masteraccountinfo", new[] { SueetieConfiguration.Get().Core.ForumFolderName });
        }

        public SueetieUrl MasterProfile(int forumUserID)
        {
            return this.GetSueetieUrl("members_masterprofile", new[] { SueetieConfiguration.Get().Core.ForumFolderName, forumUserID.ToString() });
        }

        public SueetieUrl MyProfile(int userID)
        {
            return this.GetSueetieUrl("members_myprofile", new[] { userID.ToString() });
        }

        public SueetieUrl BlogsHome()
        {
            return this.GetSueetieUrl("blogs_home");
        }

        public SueetieUrl ForumsHome()
        {
            return this.GetSueetieUrl("forums_home");
        }

        public SueetieUrl MediaHome()
        {
            return this.GetSueetieUrl("media_home");
        }

        public SueetieUrl WikiHome()
        {
            return this.GetSueetieUrl("wiki_home");
        }

        public SueetieUrl GroupsHome()
        {
            return this.GetSueetieUrl("groups_home");
        }

        public SueetieUrl AdminHome()
        {
            return this.GetSueetieUrl("admin_home");
        }

        public SueetieUrl MarketplaceHome()
        {
            return this.GetSueetieUrl("marketplace_home");
        }

        public SueetieUrl MarketplaceCategory(int categoryID)
        {
            return this.GetSueetieUrl("marketplace_category", new[] { categoryID.ToString() });
        }

        public SueetieUrl ContentHome(int contentPageID)
        {
            var _sueetieUrl = this.GetSueetieUrl("content_home");
            var _sueetieContentPage =
                SueetieContentParts.GetSueetieContentPageList().Find(p => p.ContentPageID == contentPageID);
            if (!string.IsNullOrEmpty(_sueetieContentPage.PageSlug))
                _sueetieUrl.Url = string.Format(_sueetieUrl.Url, _sueetieContentPage.PageSlug);
            return _sueetieUrl;
        }

        public SueetieUrl CalendarHome()
        {
            return this.GetSueetieUrl("calendar_home");
        }

        public SueetieUrl GetSueetieUrl(string _name)
        {
            var _sueetieUrl = new SueetieUrl();
            foreach (var url in _urls)
            {
                if (url.Name == _name)
                    _sueetieUrl = url;
            }
            if (string.IsNullOrEmpty(_sueetieUrl.Name))
                SueetieLogs.LogException("SUEETIE URL " + _name + " was not found.");

            return _sueetieUrl;
        }

        public SueetieUrl GetSueetieUrl(string _name, string[] args)
        {
            var _sueetieUrl = new SueetieUrl();
            foreach (var url in _urls)
            {
                if (url.Name == _name)
                {
                    url.Url = this.FormatUrl(url.Pattern, args);
                    if (url.RewrittenUrl != null)
                        url.RewrittenUrl = this.FormatUrl(url.RewrittenUrl, args);
                    _sueetieUrl = url;
                }
            }
            if (string.IsNullOrEmpty(_sueetieUrl.Name))
                SueetieLogs.LogException("SUEETIE URL " + _name + " was not found.");

            return _sueetieUrl;
        }

        public string FormatUrl(string _url, string[] args)
        {
            _url = string.Format(_url, args);
            return _url;
        }

        public SueetieUrls()
        {
            LoadUrls();
        }

        public static SueetieUrls Instance
        {
            get { return Get(); }
        }

        public IList<SueetieUrl> All
        {
            get { return new ReadOnlyCollection<SueetieUrl>(_urls); }
        }

        private static SueetieUrls Get()
        {
            var urls = SueetieCache.Current[urlCacheKey] as SueetieUrls;
            var urlConfig = HttpContext.Current.Server.MapPath("/util/config/urls.config");
            var dp = new CacheDependency(urlConfig);

            if (urls == null)
            {
                lock (urlLocker)
                {
                    urls = SueetieCache.Current[urlCacheKey] as SueetieUrls;
                    if (urls == null)
                    {
                        urls = new SueetieUrls();
                        SueetieCache.Current.InsertMax(urlCacheKey, urls, dp);
                    }
                }
            }
            return urls;
        }

        private static List<SueetieUrl> LoadUrls()
        {
            var _context = SueetieContext.Current;
            var urlConfig = HttpContext.Current.Server.MapPath("/util/config/urls.config");

            var cacheKey = string.Format("{0}-{1}", _context.Core.SiteUniqueName, "-SueetieUrlsConfig");

            _urls = SueetieCache.Current[cacheKey] as List<SueetieUrl>;
            if (_urls == null)
            {
                _urls = new List<SueetieUrl>();
                var doc = XDocument.Load(urlConfig);
                var dp = new CacheDependency(urlConfig);

                var urls = from url in doc.Descendants("url")
                    select new SueetieUrl
                    {
                        Name = (string)url.Attribute("name"),
                        Url = (string)url.Attribute("path"),
                        Roles = (string)url.Attribute("roles"),
                        RewrittenUrl = (string)url.Attribute("rewrittenurl"),
                        ContentUrl = (string)url.Attribute("contenturl"),
                        Pattern = (string)url.Attribute("pattern")
                    };
                _urls = urls.ToList();
                SueetieCache.Current.InsertMax(cacheKey, _urls, dp);
            }

            return _urls;
        }
    }
}