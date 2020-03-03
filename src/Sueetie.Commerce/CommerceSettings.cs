namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class CommerceSettings
    {
        private static readonly string settingsKey = ("CommerceSettings-" + SueetieConfiguration.Get().Core.SiteUniqueName);
        private static readonly object settingsLocker = new object();

        public CommerceSettings()
        {
            this.Load();
        }

        private static CommerceSettings Get()
        {
            CommerceSettings settings = SueetieCache.Current[settingsKey] as CommerceSettings;
            if (settings == null)
            {
                lock (settingsLocker)
                {
                    settings = SueetieCache.Current[settingsKey] as CommerceSettings;
                    if (settings == null)
                    {
                        settings = new CommerceSettings();
                        SueetieCache.Current.InsertMax(settingsKey, settings);
                    }
                }
            }
            return settings;
        }

        private void Load()
        {
            Type type = base.GetType();
            StringDictionary commerceSettingsDictionary = CommerceCommon.GetCommerceSettingsDictionary();
            foreach (string str in commerceSettingsDictionary.Keys)
            {
                string str2 = str;
                string str3 = commerceSettingsDictionary[str];
                foreach (PropertyInfo info in type.GetProperties())
                {
                    if (info.Name.Equals(str2, StringComparison.OrdinalIgnoreCase))
                    {
                        if (info.CanWrite)
                        {
                            info.SetValue(this, Convert.ChangeType(str3, info.PropertyType, CultureInfo.CurrentCulture), null);
                        }
                    }
                }
            }
        }

        public int ActivityReportNum { get; set; }

        public int FixedMediumImageHeight { get; set; }

        public int FixedMediumImageWidth { get; set; }

        public int FixedSmallImageHeight { get; set; }

        public int FixedSmallImageWidth { get; set; }

        public static CommerceSettings Instance
        {
            get
            {
                return Get();
            }
        }

        public int MaxFullImageSize { get; set; }
    }
}

