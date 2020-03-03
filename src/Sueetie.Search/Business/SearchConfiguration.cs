using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Caching;
using System.Reflection;
using System.Collections.Specialized;
using System.Web;
using Sueetie.Core;

namespace Sueetie.Search
{

    #region Configuration Class Objects

    public class SearchSetting
    {
        public string IndexPath { get; set; }
        public string[] StopWords { get; set; }
        public float MinimumScore { get; set; }
        public Single TitleBoost { get; set; }
        public Single TagsBoost { get; set; }
        public Single CategoryBoost { get; set; }
        public Single BodyBoost { get; set; }
        public Single UsernameBoost { get; set; }
        public Single ApplicationTypeIDBoost { get; set; }
        public int MinimumDocumentFrequency { get; set; }
        public int MinimumTermFrequency { get; set; }
        public int BodyDisplayLength { get; set; }
    }
    #endregion

    public class SearchConfiguration
    {

        #region internal properties

        private static readonly string configKey = "SearchConfig";
        private static readonly object configLocker = new object();
        private XDocument configXML = null;


        #endregion

        #region Configuration Public Properties

        public SearchSetting SearchSettings { get; set; }
        public string ConfigPath { get; set; }


        #endregion

        #region Constructor

        public SearchConfiguration(XDocument doc)
        {
            configXML = doc;
            ConfigPath = AppDomain.CurrentDomain.BaseDirectory + "/util/config/Search.config";
            PopulateSearchSettings();
        }

        public SearchConfiguration()
        {
            Get();
        }

        #endregion

        #region Get()

        public static SearchConfiguration Get()
        {
            SearchConfiguration config = SueetieCache.Current[configKey] as SearchConfiguration;
            if (config == null)
            {
                lock (configLocker)
                {
                    config = SueetieCache.Current[configKey] as SearchConfiguration;
                    if (config == null)
                    {
                        string configPath = AppDomain.CurrentDomain.BaseDirectory + "/util/config/Search.config";
                        XDocument doc = XDocument.Load(configPath);

                        config = new SearchConfiguration(doc);
                        SueetieCache.Current.InsertMax(configKey, config, new CacheDependency(configPath));
                    }
                }
            }
            return config;

        }

        #endregion

        #region Populate Class Objects

        private void PopulateSearchSettings()
        {
            var searchsettings = from searchsetting in configXML.Descendants("Settings")
                                 select new SearchSetting
                               {
                                   IndexPath = (string)searchsetting.Attribute("IndexPath"),
                                   MinimumScore = (float)searchsetting.Attribute("MinimumScore"),
                                   TitleBoost = (float)searchsetting.Attribute("TitleBoost"),
                                   TagsBoost = (float)searchsetting.Attribute("TagsBoost"),
                                   CategoryBoost = (float)searchsetting.Attribute("CategoryBoost"),
                                   BodyBoost = (float)searchsetting.Attribute("BodyBoost"),
                                   ApplicationTypeIDBoost = (float)searchsetting.Attribute("ApplicationTypeIDBoost"),
                                   UsernameBoost = (float)searchsetting.Attribute("UsernameBoost"),
                                   MinimumDocumentFrequency = (int)searchsetting.Attribute("MinimumDocumentFrequency"),
                                   MinimumTermFrequency = (int)searchsetting.Attribute("MinimumTermFrequency"),
                                   StopWords = ((string)searchsetting.Attribute("StopWords")).Replace(" ", string.Empty).Split(','),
                                   BodyDisplayLength = (int)searchsetting.Attribute("BodyDisplayLength")
                               };

            SearchSettings = searchsettings.Single();
        }

        #endregion

    }

}
