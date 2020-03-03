// -----------------------------------------------------------------------
// <copyright file="SueetieTags.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class SueetieTags
    {
        public static List<SueetieTagMaster> GetSueetieTagMasterList()
        {
            return GetSueetieTagMasterList(true);
        }

        public static List<SueetieTagMaster> GetSueetieTagMasterList(bool useCache)
        {
            var sueetieTagMasters = new List<SueetieTagMaster>();
            var provider = SueetieDataProvider.LoadProvider();

            var key = SueetieTagMasterListCacheKey();

            if (useCache)
            {
                sueetieTagMasters = SueetieCache.Current[key] as List<SueetieTagMaster>;
                if (sueetieTagMasters == null)
                {
                    sueetieTagMasters = provider.GetSueetieTagMasterList();
                    SueetieCache.Current.Insert(key, sueetieTagMasters);
                }
            }
            else
            {
                sueetieTagMasters = provider.GetSueetieTagMasterList();
            }
            return sueetieTagMasters;
        }

        public static string SueetieTagMasterListCacheKey()
        {
            return string.Format("SueetieTagMasterList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static SueetieTagMaster GetSueetieTagMasterByTagID(int _tagID)
        {
            return GetSueetieTagMasterList().Find(t => t.TagID == _tagID);
        }

        public static SueetieTagMaster GetSueetieTagMasterByTagMasterID(int _tagMasterID)
        {
            return GetSueetieTagMasterList().Find(t => t.TagMasterID == _tagMasterID);
        }

        public static List<SueetieTagMaster> GetSueetieTagMasterList(int _contentID)
        {
            return GetSueetieTagMasterList(false).Where(t => t.ContentID == _contentID).ToList();
        }


        public static List<SueetieTag> GetSueetieTagList()
        {
            return GetSueetieTagList(true);
        }

        public static List<SueetieTag> GetSueetieTagList(bool useCache)
        {
            var sueetieTags = new List<SueetieTag>();

            var key = SueetieTagListCacheKey();

            if (useCache)
            {
                sueetieTags = SueetieCache.Current[key] as List<SueetieTag>;
                if (sueetieTags == null)
                {
                    var provider = SueetieDataProvider.LoadProvider();
                    sueetieTags = provider.GetSueetieTagList();
                    SueetieCache.Current.Insert(key, sueetieTags);
                }
            }
            else
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieTags = provider.GetSueetieTagList();
            }
            return sueetieTags;
        }

        public static string SueetieTagListCacheKey()
        {
            return string.Format("SueetieTagList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieTagCache()
        {
            SueetieCache.Current.Remove(SueetieTagListCacheKey());
            SueetieCache.Current.Remove(SueetieTagMasterListCacheKey());
        }

        public static List<SueetieTag> GetSueetieTagCloudList(SueetieTagQuery sueetieTagQuery)
        {
            var key = SueetieTagCloudListCacheKey(sueetieTagQuery.ApplicationTypeID);

            var sueetieTags = SueetieCache.Current[key] as List<SueetieTag>;
            if (sueetieTags == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieTags = provider.GetSueetieTagCloudList(sueetieTagQuery);
                SueetieCache.Current.InsertMinutes(key, sueetieTags, 10);
            }
            return sueetieTags;
        }

        public static string SueetieTagCloudListCacheKey(int _applicationTypeID)
        {
            return string.Format("SueetieTagCloudList-{0}-{1}", _applicationTypeID, SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieTagCloudListCache()
        {
            foreach (int appTypeID in Enum.GetValues(typeof (SueetieApplicationType)))
                SueetieCache.Current.Remove(SueetieTagCloudListCacheKey(appTypeID));
        }

        public static string PipedTags(string _tags)
        {
            var _pipedTags = _tags.TrimEnd();
            if (_pipedTags.EndsWith(","))
                _pipedTags = _pipedTags.Substring(0, _pipedTags.LastIndexOf(","));

            _pipedTags = _pipedTags.Replace(",  ", ",");
            _pipedTags = _pipedTags.Replace(", ", ",");
            _pipedTags = _pipedTags.Replace(",", "|");

            return _pipedTags;
        }

        public static string CommaTags(string _tags)
        {
            if (!string.IsNullOrEmpty(_tags))
            {
                var _commaTags = _tags.TrimEnd();
                _commaTags = _commaTags.Replace("|", ", ");

                return _commaTags;
            }
            return _tags;
        }

        public static string TagUrls(string _pipedTags)
        {
            if (string.IsNullOrEmpty(_pipedTags))
                return SueetieLocalizer.GetString("no_tags");

            var sb = new StringBuilder();
            var firstItem = true;
            foreach (var _tag in _pipedTags.Split('|'))
            {
                if (!firstItem)
                    sb.Append(", ");
                sb.Append(string.Format("<a href=\"/search/default.aspx?srch=Tags:{0}\">{1}</a>", _tag.Replace(" ", "+"), _tag));
                firstItem = false;
            }
            return sb.ToString();
        }

        public static string TagUrls(int _contentID)
        {
            var _sueetieTagMasterList = GetSueetieTagMasterList(_contentID);
            if (_sueetieTagMasterList.Count == 0)
                return SueetieLocalizer.GetString("no_tags");

            var sb = new StringBuilder();
            var firstItem = true;
            foreach (var _sueetieTagMaster in _sueetieTagMasterList)
            {
                if (!firstItem)
                    sb.Append(", ");
                sb.Append(string.Format("<a href=\"/util/handlers/taghandler.ashx?tagid={0}\">{1}</a>", _sueetieTagMaster.TagID, _sueetieTagMaster.Tag));
                firstItem = false;
            }
            return sb.ToString();
        }

        public static string TagWeightClass(int _tagCount, int _maxTagCount)
        {
            var _class = "smallest";

            var weight = ((double)_tagCount / _maxTagCount) * 100;
            if (weight >= 99)
                _class = "biggest";
            else if (weight >= 70)
                _class = "big";
            else if (weight >= 40)
                _class = "medium";
            else if (weight >= 20)
                _class = "small";
            else if (weight >= 3)
                _class = "smallest";

            return _class;
        }
    }
}