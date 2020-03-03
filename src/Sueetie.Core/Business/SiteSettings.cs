// -----------------------------------------------------------------------
// <copyright file="SiteSettings.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_Setting.
    /// </summary>
    [Serializable]
    public class SiteSetting
    {
        public SiteSetting()
        {
        }

        public SiteSetting(string _name, string _value)
        {
            this.SettingName = _name;
            this.SettingValue = _value;
        }

        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }


    public class SiteSettings
    {
        private static readonly string settingsKey = "SueetieSiteSettings";
        private static readonly object settingsLocker = new object();

        public SiteSettings()
        {
            this.Load();
        }

        public string HtmlHeader { get; set; }
        public string TrackingScript { get; set; }
        public string Theme { get; set; }
        public string MobileTheme { get; set; }
        public int RegistrationType { get; set; }
        public string ContactEmail { get; set; }
        public string SiteName { get; set; }
        public string SitePageTitleLead { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ErrorEmails { get; set; }
        public bool CreateWikiUserAccount { get; set; }

        public string SmtpServer { get; set; }
        public string SmtpServerPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSSL { get; set; }

        public string GroupsFolderName { get; set; }
        public string DefaultLanguage { get; set; }
        public string DefaultTimeZone { get; set; }
        public bool RecordAnalytics { get; set; }
        public string HandleWwwSubdomain { get; set; }
        public bool LogAdminActivity { get; set; }
        public string IpGeoLookupUrl { get; set; }

        public static SiteSettings Instance
        {
            get { return Get(); }
        }

        private static SiteSettings Get()
        {
            var settings = SueetieCache.Current[settingsKey] as SiteSettings;
            if (settings == null)
            {
                lock (settingsLocker)
                {
                    settings = SueetieCache.Current[settingsKey] as SiteSettings;
                    if (settings == null)
                    {
                        settings = new SiteSettings();
                        SueetieCache.Current.InsertMax(settingsKey, settings);
                    }
                }
            }
            return settings;
        }

        private void Load()
        {
            var settingsType = this.GetType();

            var dic = SueetieCommon.GetSiteSettingsDictionary();

            foreach (string key in dic.Keys)
            {
                var name = key;
                var value = dic[key];

                foreach (var propertyInformation in settingsType.GetProperties())
                {
                    if (propertyInformation.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            if (propertyInformation.CanWrite)
                            {
                                propertyInformation.SetValue(this, Convert.ChangeType(value, propertyInformation.PropertyType, CultureInfo.CurrentCulture), null);
                            }
                        }
                        catch
                        {
                        }
                        break;
                    }
                }
            }
        }
    }
}