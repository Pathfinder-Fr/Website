// -----------------------------------------------------------------------
// <copyright file="SiteStatistics.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    public class SiteStatistics
    {
        public static SiteStatistics Instance
        {
            get { return Get(); }
        }

        private static SiteStatistics Get()
        {
            var statsKey = "SueetieSiteStatistics" + SueetieConfiguration.Get().Core.SiteUniqueName;
            var statsLocker = new object();

            var stats = SueetieCache.Current[statsKey] as SiteStatistics;
            if (stats == null)
            {
                lock (statsLocker)
                {
                    stats = SueetieCache.Current[statsKey] as SiteStatistics;
                    if (stats == null)
                    {
                        stats = new SiteStatistics();
                        SueetieCache.Current.InsertMax(statsKey, stats);
                    }
                }
            }
            return stats;
        }

        public string SueetieVersion
        {
            get { return string.Format("Sueetie Version {0}.{1}.{2}.{3}", this.BuildVersion.Major, this.BuildVersion.Minor, this.BuildVersion.Build, this.BuildVersion.Revision); }
        }

        public Version BuildVersion
        {
            get { return this.GetType().Assembly.GetName().Version; }
        }
    }
}