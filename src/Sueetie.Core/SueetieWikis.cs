// -----------------------------------------------------------------------
// <copyright file="SueetieWikis.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class SueetieWikis
    {
        public static List<SueetieWikiPage> GetSueetieWikiPageList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();
            
            var key = WikiPageListCacheKey(contentQuery.GroupID);

            var _cachedSueetieWikiPages = SueetieCache.Current[key] as List<SueetieWikiPage>;
            if (_cachedSueetieWikiPages != null)
            {
                if (contentQuery.NumRecords > 0)
                    return _cachedSueetieWikiPages.Take(contentQuery.NumRecords).ToList();
                return _cachedSueetieWikiPages.ToList();
            }

            var _sueetieWikiPages = from p in provider.GetSueetieWikiPageList(contentQuery)
                select p;

            if (contentQuery.GroupID > -1)
                _sueetieWikiPages = from g in _sueetieWikiPages where g.GroupID == contentQuery.GroupID select g;

            if (contentQuery.ApplicationID > 0)
                _sueetieWikiPages = from a in _sueetieWikiPages where a.ApplicationID == contentQuery.ApplicationID select a;

            if (contentQuery.UserID > 0)
                _sueetieWikiPages = from u in _sueetieWikiPages where u.UserID == contentQuery.UserID select u;

            if (contentQuery.IsRestricted)
                _sueetieWikiPages = from r in _sueetieWikiPages where r.IsRestricted == false select r;

            if (contentQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, _sueetieWikiPages.ToList(), contentQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, _sueetieWikiPages.ToList());

            if (contentQuery.NumRecords > 0)
                return _sueetieWikiPages.Take(contentQuery.NumRecords).ToList();
            return _sueetieWikiPages.ToList();
        }

        public static string WikiPageListCacheKey(int groupID)
        {
            return string.Format("WikiPageList-{0}", groupID);
        }

        public static void ClearWikiPageListCache(int groupID)
        {
            SueetieCache.Current.Remove(WikiPageListCacheKey(groupID));
        }

        public static SueetieWikiPage GetSueetieWikiPage(SueetieWikiPage sueetieWikiPage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieWikiPage(sueetieWikiPage);
        }

        public static int CreateSueetieWikiPage(SueetieWikiPage sueetieWikiPage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateSueetieWikiPage(sueetieWikiPage);
        }

        public static void UpdateSueetieWikiPage(SueetieWikiPage sueetieWikiPage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieWikiPage(sueetieWikiPage);
        }

        public static void EnterWikiPageTags(SueetieTagEntry sueetieTagEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.EnterWikiPageTags(sueetieTagEntry);
        }

        public static int CreateWikiMessage(SueetieWikiMessage sueetieWikiMessage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateWikiMessage(sueetieWikiMessage);
        }

        public static List<SueetieWikiMessage> GetSueetieWikiMessageList()
        {
            var key = SueetieWikiMessageListCacheKey();

            var sueetieWikiMessages = SueetieCache.Current[key] as List<SueetieWikiMessage>;
            if (sueetieWikiMessages == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieWikiMessages = provider.GetSueetieWikiMessageList();
                SueetieCache.Current.Insert(key, sueetieWikiMessages);
            }
            return sueetieWikiMessages;
        }

        public static string SueetieWikiMessageListCacheKey()
        {
            return string.Format("SueetieWikiMessageList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearSueetieWikiMessageListCache()
        {
            SueetieCache.Current.Remove(SueetieWikiMessageListCacheKey());
        }

        public static SueetieWikiMessage GetSueetieWikiMessage(string messageQueryID)
        {
            var _sueetieWikiMessage = GetSueetieWikiMessageList().Find(m => m.MessageQueryID == messageQueryID);
            if (_sueetieWikiMessage == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                _sueetieWikiMessage = provider.GetSueetieWikiMessage(messageQueryID);
            }
            return _sueetieWikiMessage;
        }

        public static void UpdateSueetieWikiMessage(SueetieWikiMessage sueetieWikiMessage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieWikiMessage(sueetieWikiMessage);
        }
    }
}