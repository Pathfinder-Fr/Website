// -----------------------------------------------------------------------
// <copyright file="SueetieLocalizer.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;
    using System.Xml.Linq;

    public class SueetieLocalizer
    {
        public static List<SueetieResource> GetTimeZones()
        {
            var sueetieContext = SueetieContext.Current;
            var _language = sueetieContext.SiteSettings.DefaultLanguage ?? "en-US";
            return LoadLanguageFile(_language, "timezones.xml");
        }

        public static string GetPageTitle(string titleKey)
        {
            var _titleOut = string.Empty;
            try
            {
                var _localizedTitle = GetString(titleKey, "pagetitles.xml");
                if (!string.IsNullOrEmpty(SiteSettings.Instance.SitePageTitleLead))
                    _titleOut = SiteSettings.Instance.SitePageTitleLead + GetString("page_title_separator", "pagetitles.xml") + _localizedTitle;
                else
                    _titleOut = _localizedTitle;
            }
            catch (Exception ex)
            {
                SueetieLogs.LogException(ex.Message);
            }

            return _titleOut;
        }

        public static string GetString(string name)
        {
            return GetString(name, "Sueetie.xml", null);
        }

        public static string GetString(string name, string filename)
        {
            return GetString(name, filename, null);
        }

        public static string GetString(string name, string[] textArgs)
        {
            return GetString(name, "Sueetie.xml", textArgs);
        }

        public static string GetString(string name, bool UseCurrentApplicationFile)
        {
            var filename = SafeAppName(SueetieApplications.Current.ApplicationName) + ".xml";
            return GetString(name, filename, null);
        }

        public static string GetString(string name, string fileName, string[] textArgs)
        {
            List<SueetieResource> locals = null;
            var sueetieContext = SueetieContext.Current;
            var _language = sueetieContext.SiteSettings.DefaultLanguage ?? "en-US";

            if (!string.IsNullOrEmpty(fileName))
                locals = LoadLanguageFile(_language, fileName);
            else
                locals = LoadLanguageFile(_language, "Sueetie.xml");

            string text = null;
            try
            {
                text = locals.Find(l => l.Key == name).Value;
                if (textArgs != null)
                    text = string.Format(text, textArgs);
            }
            catch (Exception ex)
            {
                SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.GeneralException, ex.Message + " STRING NAME: " + name +
                                                                                                      " FILENAME: " + fileName + " STACKTRACE: " + ex.StackTrace);
            }

            if (text == null)
            {
#if DEBUG
                text = string.Format("<strong><FONT color=#ff0000>[{0}]</FONT></strong>", name);
                SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.GeneralException,
                    string.Format("Missing Resource: {0} Page: {1}", name, sueetieContext.RawUrl));
#else
                text = string.Empty;
#endif
            }
            return text;
        }

        public static string GetMarketplaceString(string name)
        {
            return GetString(name, "Marketplace.xml", null);
        }

        public static string GetAnalyticsString(string name)
        {
            return GetString(name, "Analytics.xml", null);
        }

        public static string GetAddonPackString(string name)
        {
            return GetString(name, "AddonPack.xml", null);
        }

        public static string GetForumString(string name)
        {
            return GetString(name, "Forums.xml", null);
        }

        public static string GetLicensingString(string name)
        {
            return GetString(name, "Licensing.xml", null);
        }

        private static List<SueetieResource> LoadLanguageFile(string language, string fileName)
        {
            var _context = SueetieContext.Current;
            var filePath = HttpContext.Current.Server.MapPath("/util/Languages/" + language + "/" + fileName);

            var cacheKey = string.Format("{0}-{1}-{2}", _context.Core.SiteUniqueName, language, fileName);

            var locals = SueetieCache.Current[cacheKey] as List<SueetieResource>;
            if (locals == null)
            {
                locals = new List<SueetieResource>();
                var doc = XDocument.Load(filePath);
                var dp = new CacheDependency(filePath);

                var resources = from resource in doc.Descendants("resource")
                    select new SueetieResource
                    {
                        Key = (string)resource.Attribute("key"),
                        Value = resource.Value
                    };
                locals = resources.ToList();
                SueetieCache.Current.InsertMax(cacheKey, locals, dp);
            }

            return locals;
        }

        private static string SafeAppName(string _appname)
        {
            if (_appname.Contains("Unknown"))
                return "Sueetie";
            return _appname;
        }
    }
}