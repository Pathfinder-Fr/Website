namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Caching;
    using System.Xml.Linq;

    public class CommerceConfiguration
    {
        private static readonly string configKey = "CommerceConfig";
        private static readonly object configLocker = new object();
        private XDocument configXML;

        public CommerceConfiguration(XDocument doc)
        {
            this.configXML = doc;
            this.ConfigPath = HttpContext.Current.Server.MapPath("/util/config/") + "Commerce.config";
            this.PopulateCoreSettings();
        }

        public static CommerceConfiguration Get()
        {
            CommerceConfiguration configuration = SueetieCache.Current[configKey] as CommerceConfiguration;
            if (configuration == null)
            {
                lock (configLocker)
                {
                    configuration = SueetieCache.Current[configKey] as CommerceConfiguration;
                    if (configuration == null)
                    {
                        string uri = HttpContext.Current.Server.MapPath("/util/config/") + "Commerce.config";
                        configuration = new CommerceConfiguration(XDocument.Load(uri));
                        SueetieCache.Current.InsertMax(configKey, configuration, new CacheDependency(uri));
                    }
                }
            }
            return configuration;
        }

        private void PopulateCoreSettings()
        {
            IEnumerable<Sueetie.Commerce.CoreSetting> source = from coresetting in this.configXML.Descendants("Core") select new Sueetie.Commerce.CoreSetting { ShowWhatsNew = (bool) coresetting.Attribute("ShowWhatsNew"), DistributionKey = (string) coresetting.Attribute("DistributionKey") };
            this.Core = source.Single<Sueetie.Commerce.CoreSetting>();
        }

        public string ConfigPath { get; set; }

        public Sueetie.Commerce.CoreSetting Core { get; set; }
    }
}

