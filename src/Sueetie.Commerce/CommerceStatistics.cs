namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;

    public class CommerceStatistics
    {
        private static CommerceStatistics Get()
        {
            string key = "SueetieCommerceStatistics" + SueetieConfiguration.Get().Core.SiteUniqueName;
            object obj2 = new object();
            CommerceStatistics statistics = SueetieCache.Current[key] as CommerceStatistics;
            if (statistics == null)
            {
                lock (obj2)
                {
                    statistics = SueetieCache.Current[key] as CommerceStatistics;
                    if (statistics == null)
                    {
                        statistics = new CommerceStatistics();
                        SueetieCache.Current.InsertMax(key, statistics);
                    }
                }
            }
            return statistics;
        }

        public Version BuildVersion
        {
            get
            {
                return base.GetType().Assembly.GetName().Version;
            }
        }

        public string CommerceVersion
        {
            get
            {
                return string.Format("Sueetie Marketplace Version {0}.{1}.{2}.{3}", new object[] { this.BuildVersion.Major, this.BuildVersion.Minor, this.BuildVersion.Build, this.BuildVersion.Revision });
            }
        }

        public static CommerceStatistics Instance
        {
            get
            {
                return Get();
            }
        }
    }
}

