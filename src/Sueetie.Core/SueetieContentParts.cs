// -----------------------------------------------------------------------
// <copyright file="SueetieContentParts.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.Caching;

    public static class SueetieContentParts
    {
        public static SueetieContentPart GetSueetieContentPart(string contentName)
        {
            var key = ContentPartCacheKey(contentName);

            var sueetieContentPart = SueetieCache.Current[key] as SueetieContentPart;
            if (sueetieContentPart == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieContentPart = provider.GetSueetieContentPart(contentName);
                SueetieCache.Current.Add(key, sueetieContentPart, null, DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            return sueetieContentPart;
        }

        public static int UpdateSueetieContentPart(SueetieContentPart sueetieContentPart)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var contentPartId = provider.UpdateSueetieContentPart(sueetieContentPart);
            ClearContentPartCache(sueetieContentPart.ContentName);
            return contentPartId;
        }

        public static void ClearContentPartCache(string contentName)
        {
            SueetieCache.Current.Remove(ContentPartCacheKey(contentName));
        }

        private static string ContentPartCacheKey(string contentName)
        {
            return string.Format("ContentPart-{0}", contentName);
        }

        public static List<SueetieContentPart> GetSueetieContentPartList(int pageId)
        {
            var key = SueetieContentPartListCacheKey(pageId);
            var sueetieContentParts = SueetieCache.Current[key] as List<SueetieContentPart>;
            if (sueetieContentParts == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieContentParts = provider.GetSueetieContentPartList(pageId);
                SueetieCache.Current.Insert(key, sueetieContentParts);
            }
            return sueetieContentParts;
        }

        public static string SueetieContentPartListCacheKey(int pageId)
        {
            return string.Format("SueetieContentPartList-{0}", pageId);
        }

        public static void ClearSueetieContentPartListCache(int pageId)
        {
            SueetieCache.Current.Remove(SueetieContentPartListCacheKey(pageId));
        }

        public static SueetieContentPage GetSueetieContentPage(int pageId)
        {
            var key = SueetieContentPageCacheKey(pageId);
            var sueetieContentPage = SueetieCache.Current[key] as SueetieContentPage;
            if (sueetieContentPage == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieContentPage = provider.GetSueetieContentPage(pageId);
                SueetieCache.Current.Insert(key, sueetieContentPage);
            }
            return sueetieContentPage;
        }

        public static string SueetieContentPageCacheKey(int pageId)
        {
            return string.Format("SueetieContentPage-{0}", pageId);
        }

        public static void ClearSueetieContentPageCache(int pageId)
        {
            SueetieCache.Current.Remove(SueetieContentPageCacheKey(pageId));
        }

        public static List<SueetieContentPage> GetSueetieContentPageList()
        {
            return GetSueetieContentPageList(-1);
        }

        public static List<SueetieContentPage> GetSueetieContentPageList(int contentGroupId)
        {
            var key = SueetieContentPageListCacheKey(contentGroupId);

            var sueetieContentPages = SueetieCache.Current[key] as List<SueetieContentPage>;
            if (sueetieContentPages == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieContentPages = provider.GetSueetieContentPageList(contentGroupId);
                SueetieCache.Current.Insert(key, sueetieContentPages);
            }

            return sueetieContentPages;
        }

        public static string SueetieContentPageListCacheKey(int contentGroupId)
        {
            return string.Format("SueetieContentPageList-{0}-{1}", SueetieConfiguration.Get().Core.SiteUniqueName, contentGroupId);
        }

        public static void ClearSueetieContentPageListCache(int contentGroupId)
        {
            SueetieCache.Current.Remove(SueetieContentPageListCacheKey(contentGroupId));
        }

        public static List<SueetieContentPageGroup> GetSueetieContentPageGroupList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieContentPageGroupList();
        }

        public static SueetieContentPageGroup GetSueetieContentPageGroup(int contentPageGroupId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieContentPageGroup(contentPageGroupId);
        }

        public static void CreateContentPageGroup(SueetieContentPageGroup sueetieContentPageGroup)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateContentPageGroup(sueetieContentPageGroup);
        }

        public static void UpdateSueetieContentPageGroup(SueetieContentPageGroup sueetieContentPageGroup)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieContentPageGroup(sueetieContentPageGroup);
        }

        public static int CreateContentPage(SueetieContentPage sueetieContentPage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateContentPage(sueetieContentPage);
        }

        public static void UpdateSueetieContentPage(SueetieContentPage sueetieContentPage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieContentPage(sueetieContentPage);
        }

        public static void DeleteContentPage(int contentpageId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.DeleteContentPage(contentpageId);
        }

        /// Strips all illegal characters from the specified title.
        /// </summary>
        public static string CreatePageSlug(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace(":", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace(".", string.Empty);
            text = text.Replace(",", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace(" ", "-");
            text = RemoveDiacritics(text);
            text = RemoveExtraHyphen(text);

            return HttpUtility.UrlEncode(text).Replace("%", string.Empty);
        }

        private static string RemoveExtraHyphen(string text)
        {
            if (text.Contains("--"))
            {
                text = text.Replace("--", "-");
                return RemoveExtraHyphen(text);
            }

            return text;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for (var i = 0; i < normalized.Length; i++)
            {
                var c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static List<SueetieApplication> GetCmsApplicationList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            var sueetieApplications = provider.GetCMSApplicationList();
            if (sueetieApplications.Count == 0)
            {
                var app = new SueetieApplication
                {
                    ApplicationID = -1,
                    Description = "Not Available"
                };
                sueetieApplications.Add(app);
            }
            else
            {
                foreach (var app in sueetieApplications)
                {
                    app.Description = app.Description + "  (" + app.ApplicationKey + ")";
                }
            }
            return sueetieApplications;
        }

        public static void UpdateCmsPermalink(SueetieContentPage sueetieContentPage)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateCMSPermalink(sueetieContentPage);
        }

        public static SueetieContentPage CurrentContentPage
        {
            get { return GetCurrentContentPage(); }
        }

        private static SueetieContentPage GetCurrentContentPage()
        {
            SueetieContentPage sueetieContentPage = null;

            var pageId = DataHelper.GetIntFromQueryString("pg", -1);
            if (pageId > 0)
            {
                var allContentPages = GetSueetieContentPageList();
                if (allContentPages != null)
                {
                    sueetieContentPage = allContentPages.Find(p => pageId.Equals(p.ContentPageID));
                }
            }
            //string _rawUrl = HttpContext.Current.Request.RawUrl.ToLower();
            //int _segments = HttpContext.Current.Request.Url.Segments.Length;

            //if (_rawUrl.IndexOf(".aspx") > 0 && _segments > 3)
            //{
            //    int _pageSlugStart = _rawUrl.LastIndexOf("/") + 1;
            //    int _pageSlugEnd = _rawUrl.IndexOf(".aspx");
            //    int _pageSlugLength = _pageSlugEnd - _pageSlugStart;
            //    string _rawGroupKey = _rawUrl.Substring(1, _pageSlugStart -2);
            //    string _rawSlug = _rawUrl.Substring(_pageSlugStart, _pageSlugLength);

            //    List<SueetieContentPage> _allContentPages = SueetieContentParts.GetSueetieContentPageList();
            //    foreach (SueetieContentPage _page in _allContentPages)
            //    {
            //        if (_page.GroupKey == _rawGroupKey && _page.PageSlug == _rawSlug)
            //            sueetieContentPage = _page;
            //    }
            //}
            return sueetieContentPage;
        }

        public static void EnterContentPageTags(SueetieTagEntry sueetieTagEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.EnterContentPageTags(sueetieTagEntry);

            ClearSueetieContentPageListCache(GetSueetieContentPage(sueetieTagEntry.ItemID).ContentPageGroupID);
            ClearSueetieContentPageListCache(-1);
        }
    }
}