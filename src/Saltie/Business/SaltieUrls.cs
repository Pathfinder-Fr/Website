using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Web.Caching;
using System.Web;
using System.Linq;
using Sueetie.Core;

namespace Saltie.Core
{
    public class SaltieUrl
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Roles { get; set; }
        public string RewrittenUrl { get; set; }
        public string ContentUrl { get; set; }
        public string Pattern { get; set; }
    }

    public class SaltieUrls
    {

        #region internal properties

        private static List<SaltieUrl> _urls = new List<SaltieUrl>();
        private static readonly string urlCacheKey = SueetieConfiguration.Get().Core.SiteUniqueName + "-SaltieUrls";
        private static readonly object urlLocker = new object();

        #endregion

        #region Individual Urls

        #region Common

        public SaltieUrl Home()
        {
            return GetSaltieUrl("home");
        }

        public SaltieUrl Contact()
        {
            return GetSaltieUrl("contact_us");
        }

        public SaltieUrl SearchHome()
        {
            return GetSaltieUrl("search_home");
        }
        	
        #endregion

        #region Members

        public SaltieUrl Login()
        {
            return GetSaltieUrl("members_login");
        }

        public SaltieUrl Logout()
        {
            return GetSaltieUrl("members_logout");
        }

        public SaltieUrl MyAccountInfo()
        {
            return GetSaltieUrl("members_myaccountinfo");
        }

        public SaltieUrl Register()
        {
            return GetSaltieUrl("members_register");
        }

        #endregion

        #endregion

        #region Url Utilities

        public SaltieUrl GetSaltieUrl(string _name)
        {
            SaltieUrl _SaltieUrl = new SaltieUrl();
            foreach (SaltieUrl url in _urls)
            {
                if (url.Name == _name)
                    _SaltieUrl = url;
            }
            if (string.IsNullOrEmpty(_SaltieUrl.Name))
                SueetieLogs.LogException("Saltie URL " + _name + " was not found.");

            return _SaltieUrl;
        }


        public SaltieUrl GetSaltieUrl(string _name, string[] args)
        {
            SaltieUrl _SaltieUrl = new SaltieUrl();
            foreach (SaltieUrl url in _urls)
            {
                if (url.Name == _name)
                {
                    url.Url = FormatUrl(url.Pattern, args);
                    _SaltieUrl = url;
                }
            }
            if (string.IsNullOrEmpty(_SaltieUrl.Name))
                SueetieLogs.LogException("SALTIE URL " + _name + " was not found.");

            return _SaltieUrl;
        }

        public string FormatUrl(string _url, string[] args)
        {
            _url = String.Format(_url, args);
            return _url;
        }

        #endregion

        #region Constructor and Load UrlData

        public SaltieUrls()
        {
            LoadUrls();
        }
        public static SaltieUrls Instance
        {
            get
            {
                return Get();
            }
        }
        public IList<SaltieUrl> All
        {
            get
            {
                return new ReadOnlyCollection<SaltieUrl>(_urls);
            }
        }
        private static SaltieUrls Get()
        {
            SaltieUrls urls = SueetieCache.Current[urlCacheKey] as SaltieUrls;
            string urlConfig = HttpContext.Current.Server.MapPath("/util/config/urls.config");
            CacheDependency dp = new CacheDependency(urlConfig);

            if (urls == null)
            {
                lock (urlLocker)
                {
                    urls = SueetieCache.Current[urlCacheKey] as SaltieUrls;
                    if (urls == null)
                    {
                        urls = new SaltieUrls();
                        SueetieCache.Current.InsertMax(urlCacheKey, urls, dp);
                    }
                }
            }
            return urls;
        }
        private static List<SaltieUrl> LoadUrls()
        {
            SueetieContext _context = SueetieContext.Current;
            string urlConfig = HttpContext.Current.Server.MapPath("/util/config/urls.config");

            string cacheKey = string.Format("{0}-{1}", _context.Core.SiteUniqueName, "-SaltieUrlsConfig");

            _urls = SueetieCache.Current[cacheKey] as List<SaltieUrl>;
            if (_urls == null)
            {
                _urls = new List<SaltieUrl>();
                XDocument doc = XDocument.Load(urlConfig);
                CacheDependency dp = new CacheDependency(urlConfig);

                var urls = from url in doc.Descendants("url")
                           select new SaltieUrl
                           {
                               Name = (string)url.Attribute("name"),
                               Url = (string)url.Attribute("path"),
                               Roles = (string)url.Attribute("roles"),
                               RewrittenUrl = (string)url.Attribute("rewrittenurl"),
                               ContentUrl = (string)url.Attribute("contenturl")
                           };
                _urls = urls.ToList();
                SueetieCache.Current.InsertMax(cacheKey, _urls, dp);
            }

            return _urls;
        }

        #endregion

    }
}
