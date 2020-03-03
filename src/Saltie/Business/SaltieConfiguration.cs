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

namespace Saltie.Core
{

    #region Configuration Class Objects

    public class SaltieEvents
    {
        public bool PreUserAccountApproval { get; set; }
        public bool PostUserAccountApproval { get; set; }
    }

    public class CoreSetting
    {
        public string MemberContactEmail { get; set; }
    }

    #endregion

    public class SaltieConfiguration
    {

        #region internal properties

        private static readonly string configKey = "SaltieConfig";
        private static readonly object configLocker = new object();
        private XDocument configXML = null;

        #endregion

        #region Configuration Public Properties

        public SaltieEvents Events { get; set; }
        public string ConfigPath { get; set; }
        public CoreSetting Core { get; set; }

        #endregion

        #region Constructor

        public SaltieConfiguration(XDocument doc)
        {
            configXML = doc;
            ConfigPath = HttpContext.Current.Server.MapPath("/util/config/") + "Saltie.config";
            PopulateEventSettings();
            PopulateCoreSettings();
        }

        #endregion

        #region Get()

        public static SaltieConfiguration Get()
        {
            SaltieConfiguration config = SueetieCache.Current[configKey] as SaltieConfiguration;
            if (config == null)
            {
                lock (configLocker)
                {
                    config = SueetieCache.Current[configKey] as SaltieConfiguration;
                    if (config == null)
                    {
                        string configPath = HttpContext.Current.Server.MapPath("/util/config/") + "Saltie.config";
                        XDocument doc = XDocument.Load(configPath);

                        config = new SaltieConfiguration(doc);
                        SueetieCache.Current.InsertMax(configKey, config, new CacheDependency(configPath));
                    }
                }
            }
            return config;

        }

        #endregion

        #region Populate Class Objects


        private void PopulateEventSettings()
        {
            var events = from coresetting in configXML.Descendants("Events")
                         select new SaltieEvents
                         {
                             PreUserAccountApproval = (bool)coresetting.Attribute("PreUserAccountApproval"),
                             PostUserAccountApproval = (bool)coresetting.Attribute("PostUserAccountApproval")
                         };

            Events = events.Single();
        }

        private void PopulateCoreSettings()
        {
            var coresettings = from coresetting in configXML.Descendants("Core")
                               select new CoreSetting
                               {
                                   MemberContactEmail = (string)coresetting.Attribute("MemberContactEmail")
                               };

            Core = coresettings.Single();
        }
        #endregion

    }

}
