// -----------------------------------------------------------------------
// <copyright file="SueetieConfiguration.cs" company="Pathfinder-fr.org">
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

    public class SueetieProvider
    {
        public string Name { get; set; }
        public string Function { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderType { get; set; }
    }

    public class AvatarSetting
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int ThumbnailHeight { get; set; }
        public int ThumbnailWidth { get; set; }
        public int Size { get; set; }
        public string AvatarFolderPath { get; set; }
        public int ImageQuality { get; set; }
    }

    public class CoreSetting
    {
        public int MaxListViewRecords { get; set; }
        public int TruncateTextCount { get; set; }
        public string ApplicationName { get; set; }
        public bool SendEmails { get; set; }
        public string SiteUniqueName { get; set; }
        public string AdminTheme { get; set; }
        public string MobileDevices { get; set; }
        public string ForumFolderName { get; set; }
        public bool UseForumProfile { get; set; }
        public string MarketplaceFolderName { get; set; }
    }

    public class MediaSetting
    {
        public int RecentPhotoCount { get; set; }
        public bool LinkToOriginalImage { get; set; }
        public string HtmlOutput { get; set; }
        public string MobileHtmlOutput { get; set; }
        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }
    }

    public class SiteMenuTab
    {
        public string id { get; set; }
        public string url { get; set; }
        public string app { get; set; }
        public string appkey { get; set; }
        public string roles { get; set; }
        public string maskurl { get; set; }
    }

    public class FooterMenuLink
    {
        public string id { get; set; }
        public string role { get; set; }
    }

    public class SupportedLanguage
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class SueetieConfiguration
    {
        private static readonly string configKey = "SueetieConfig";
        private static readonly object configLocker = new object();
        private XDocument configXML;
        private static string siteRootPath = HttpContext.Current.Server.MapPath("/") == null ? AppDomain.CurrentDomain.BaseDirectory : HttpContext.Current.Server.MapPath("/");

        public List<SueetieProvider> SueetieProviders { get; set; }
        public List<SupportedLanguage> SupportedLanguages { get; set; }
        public AvatarSetting AvatarSettings { get; set; }
        public CoreSetting Core { get; set; }
        public MediaSetting Media { get; set; }
        public List<SiteMenuTab> SiteMenuTabs { get; set; }
        public List<FooterMenuLink> FooterMenuLinks { get; set; }
        public string ConfigPath { get; set; }
        public string SiteRootPath { get; set; }

        public SueetieConfiguration(XDocument doc)
        {
            this.configXML = doc;
            this.ConfigPath = siteRootPath + "util\\config\\Sueetie.config";
            this.SiteRootPath = siteRootPath;
            this.PopulateProviders();
            this.PopulateAvatarSettings();
            this.PopulateCoreSettings();
            this.PopulateMediaSettings();
            this.PopulateMenuTabs();
            this.PopulateFooterLinks();
            this.PopulateLanguages();
        }

        public static SueetieConfiguration Get()
        {
            var config = SueetieCache.Current[configKey] as SueetieConfiguration;
            if (config == null)
            {
                lock (configLocker)
                {
                    config = SueetieCache.Current[configKey] as SueetieConfiguration;
                    if (config == null)
                    {
                        var configPath = siteRootPath + "/util/config/Sueetie.config";
                        //string configPath = AppDomain.CurrentDomain.BaseDirectory + "Sueetie.config";
                        //string configPath = HttpContext.Current.Server.MapPath("/") + "Sueetie.config";
                        var doc = XDocument.Load(configPath);

                        config = new SueetieConfiguration(doc);
                        SueetieCache.Current.InsertMax(configKey, config, new CacheDependency(configPath));
                    }
                }
            }
            return config;
        }

        private void PopulateProviders()
        {
            var providers = from provider in this.configXML.Descendants("Provider")
                select new SueetieProvider
                {
                    ConnectionString = (string)provider.Element("connectionString"),
                    Name = (string)provider.Element("name"),
                    Function = (string)provider.Element("function"),
                    ProviderType = (string)provider.Element("type")
                };
            this.SueetieProviders = providers.ToList();
        }

        private void PopulateLanguages()
        {
            var languages = from language in this.configXML.Descendants("Language")
                select new SupportedLanguage
                {
                    Name = (string)language.Attribute("name"),
                    Key = (string)language.Attribute("key")
                };
            this.SupportedLanguages = languages.ToList();
        }

        private void PopulateAvatarSettings()
        {
            var avatarsettings = from avatarsetting in this.configXML.Descendants("AvatarSettings")
                select new AvatarSetting
                {
                    Height = (int)avatarsetting.Attribute("Height"),
                    Width = (int)avatarsetting.Attribute("Width"),
                    ThumbnailHeight = (int)avatarsetting.Attribute("ThumbnailHeight"),
                    ThumbnailWidth = (int)avatarsetting.Attribute("ThumbnailWidth"),
                    Size = (int)avatarsetting.Attribute("Size"),
                    AvatarFolderPath = (string)avatarsetting.Attribute("AvatarFolderPath"),
                    ImageQuality = (int)avatarsetting.Attribute("ImageQuality")
                };

            this.AvatarSettings = avatarsettings.Single();
        }

        private void PopulateCoreSettings()
        {
            var coresettings = from coresetting in this.configXML.Descendants("Core")
                select new CoreSetting
                {
                    MaxListViewRecords = (int)coresetting.Attribute("MaxListViewRecords"),
                    TruncateTextCount = (int)coresetting.Attribute("TruncateTextCount"),
                    ApplicationName = (string)coresetting.Attribute("ApplicationName"),
                    SendEmails = (bool)coresetting.Attribute("SendEmails"),
                    SiteUniqueName = (string)coresetting.Attribute("SiteUniqueName"),
                    AdminTheme = (string)coresetting.Attribute("AdminTheme"),
                    MobileDevices = (string)coresetting.Attribute("MobileDevices"),
                    ForumFolderName = (string)coresetting.Attribute("ForumFolderName"),
                    UseForumProfile = (bool)coresetting.Attribute("UseForumProfile"),
                    MarketplaceFolderName = (string)coresetting.Attribute("MarketplaceFolderName")
                };

            this.Core = coresettings.Single();
        }

        private void PopulateMenuTabs()
        {
            var tabs = from tab in this.configXML.Descendants("Tab")
                select new SiteMenuTab
                {
                    url = (string)tab.Attribute("url"),
                    app = (string)tab.Attribute("app"),
                    appkey = (string)tab.Attribute("appkey"),
                    id = (string)tab.Attribute("id"),
                    roles = (string)tab.Attribute("roles"),
                    maskurl = (string)tab.Attribute("maskurl")
                };

            this.SiteMenuTabs = tabs.ToList();
        }

        private void PopulateFooterLinks()
        {
            var links = from link in this.configXML.Descendants("Link")
                select new FooterMenuLink
                {
                    id = (string)link.Attribute("id"),
                    role = (string)link.Attribute("role")
                };

            this.FooterMenuLinks = links.ToList();
        }

        private bool getExclusiveBit(string exclusiveBit)
        {
            if (exclusiveBit != null)
                return bool.Parse(exclusiveBit);
            return false;
        }

        private void PopulateMediaSettings()
        {
            var mediasettings = from mediasetting in this.configXML.Descendants("Media")
                select new MediaSetting
                {
                    RecentPhotoCount = (int)mediasetting.Attribute("RecentPhotoCount"),
                    LinkToOriginalImage = (bool)mediasetting.Attribute("LinkToOriginalImage"),
                    HtmlOutput = (string)mediasetting.Attribute("HtmlOutput"),
                    MobileHtmlOutput = (string)mediasetting.Attribute("MobileHtmlOutput"),
                    ThumbnailHeight = (int)mediasetting.Attribute("ThumbnailHeight"),
                    ThumbnailWidth = (int)mediasetting.Attribute("ThumbnailWidth")
                };
            this.Media = mediasettings.Single();
        }
    }
}